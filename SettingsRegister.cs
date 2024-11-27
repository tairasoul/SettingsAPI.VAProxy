using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SettingsAPI
{
	public class SettingsRegister : MonoBehaviour
	{
		internal bool MenuLoaded = false;
		internal bool NeedCreateMods = false;
		internal Scene currentScene = SceneManager.GetActiveScene();
		private void Awake()
		{
			SceneManager.activeSceneChanged += (Scene old, Scene newS) =>
			{
				currentScene = newS;
				if (newS.name == "Menu")
				{
					MenuLoaded = true;
					NeedCreateMods = true;
				}
				if (newS.name != "Menu" && MenuLoaded)
				{
					Plugin.Log.LogInfo("Creating and setting up ModSettings button.");
					GameObject Button = SettingsUtils.CreateSettingsButton();
					SettingsUtils.SetupSettingsButton(Button);
					// /Canvas/Pages/Setting
					GameObject Settings = GameObject.Find("MAINMENU").Find("Canvas").Find("Pages").Find("Setting");
					Settings.Find("ModSettingsPage").SetActive(false);
				}
			};
		}

		/// <summary>
		/// Register a mod.
		/// </summary>
		/// <param name="ModId">Id of mod.</param> 
		/// <param name="display">Text to display to user on mod's button.</param> 
		/// <param name="options">Options the mod has.</param> 
		/// <param name="InitialCreateCallback">Callback used to setup everything needed for the page. Called with the GameObject of the mod page assigned to this mod.</param>

		public static void RegisterMod(string ModId, string display, Option[] options, Action<GameObject> InitialCreateCallback)
		{
			RawMod mod = new()
			{
				ModId = ModId,
				display = display,
				options = options,
				Create = InitialCreateCallback
			};
			Mods.rawMods = Mods.rawMods.Append(mod);
		}

		/// <summary>
		/// Register a mod.
		/// </summary>
		/// <param name="ModId">Id of mod.</param> 
		/// <param name="display">Text to display to user on mod's button.</param> 
		/// <param name="options">Options the mod has.</param> 

		public static void RegisterMod(string ModId, string display, Option[] options)
		{
			RawMod mod = new()
			{
				ModId = ModId,
				display = display,
				options = options,
				Create = (GameObject none) => { }
			};
			Mods.rawMods = Mods.rawMods.Append(mod);
		}


		private void CreateMods()
		{
			GameObject Settings = GameObject.Find("MAINMENU").Find("Canvas").Find("Pages").Find("Setting");
			GameObject res = Settings.Find("Content").Find("GameObject").Find("resolution");
			Button resb = res.GetComponent<Button>();
			GameObject Game = Settings.Find("Game");
			resb.onClick.AddListener(() => Game.SetActive(true));
			Settings.Find("ModSettingsPage").SetActive(false);
			foreach (RawMod mod in Mods.rawMods)
			{
				Plugin.Log.LogInfo($"Creating mod {mod.ModId}");
				GameObject ModSettings = Settings.Find("Content").Find("GameObject").Find("ModSettings");
				GameObject ModSetting = ModSettings.Instantiate();
				GameObject Viewport = Settings.Find("ModSettingsPage").Find("Viewport").Find("Content");
				ModSetting.SetParent(Viewport, false);
				LayoutElement element = ModSetting.GetComponent<LayoutElement>();
				element.preferredHeight = 30f;
				element.minWidth = 300f;
				element.ignoreLayout = false;
				element.layoutPriority = 1;
				ModSetting.Find("ItemName").GetComponent<Text>().resizeTextForBestFit = true;
				ModSetting.Find("ItemName").GetComponent<Text>().text = mod.display;
				ModSetting.Find("ItemName").GetComponent<RectTransform>().anchoredPosition = new Vector2(31.5272f, -1.4878f);
				ModSetting.name = mod.ModId;
				Button button = ModSetting.GetComponent<Button>();
				GameObject ModSettingPage = Settings.AddObject(mod.ModId);
				ModSettingPage.AddComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
				mod.Create(ModSettingPage);
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(() =>
				{
					GameObject[] children = Settings.GetChildren();
					foreach (GameObject child in children)
					{
						if (child.name != "Header" && child.name != "Content" && child.name != mod.ModId)
						{
							child.SetActive(false);
						}
					}
					SettingsUtils.PageHeader.GetComponent<Text>().text = mod.display;
					ModSettingPage.SetActive(true);
				});
				Plugin.Log.LogInfo("Creating mod options");
				foreach (Option option in mod.options)
				{
					option.Create(ModSettingPage);
				}
			}
		}

		private void Update()
		{
			if (currentScene.name != "Intro" && currentScene.name != "Menu")
			{
				GameObject Settings = GameObject.Find("MAINMENU").Find("Canvas").Find("Pages").Find("Setting");
				if (Settings == null)
					return;
				if (NeedCreateMods && Settings.Find("ModSettingsPage") != null && Settings.Find("ModSettingsPage").Find("Viewport").Find("Content") != null)
				{
					NeedCreateMods = false;
					CreateMods();
				}
				Image img = Settings.Find("ModSettingsPage").Find("Viewport").Find("Content").GetComponent<Image>();
				if (Settings.Find("Exit Confirm").activeSelf) 
					img.raycastTarget = false;
				else
					img.raycastTarget = true;
			}
			else if (currentScene.name == "Menu") 
			{
				NeedCreateMods = true;
			}
		}
	}

	public struct Option
	{
		/// <summary>
		/// The function that creates this option.
		/// Gets the page created for it passed in as an argument.
		/// </summary>
		public Action<GameObject> Create;
	}

	internal class Mods
	{
		public static RawMod[] rawMods = [];
	}

	internal struct RawMod
	{
		/// <summary>
		/// Id of the mod used for the name of the game object.
		/// </summary>
		public string ModId;
		/// <summary>
		/// Name displayed to user on mod button.
		/// </summary>
		public string display;
		/// <summary>
		/// Options the mod has.
		/// </summary>
		public Option[] options;
		/// <summary>
		/// Called to create the mod setting page's needed components.
		/// </summary>
		public Action<GameObject> Create;
	}
}
