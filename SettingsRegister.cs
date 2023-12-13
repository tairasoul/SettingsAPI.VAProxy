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
        public void Awake()
        {
            SceneManager.activeSceneChanged += (Scene old, Scene newS) =>
            {
                currentScene = newS;
                if (newS.name == "Menu")
                {
                    MenuLoaded = true;
                    NeedCreateMods = true;
                    SettingsUtils.updateActions = new Action[] { };
                }
                if (newS.name != "Menu" && MenuLoaded)
                {
                    Plugin.Log.LogInfo("Creating and setting up ModSettings button.");
                    GameObject Button = SettingsUtils.CreateSettingsButton();
                    SettingsUtils.SetupSettingsButton(Button);
                    GameObject Settings = GameObject.Find("MAINMENU/Canvas/Pages/Setting");
                    Settings.Find("ModSettingsPage").SetActive(false);
                }
            };
        }

        /// <summary>
        /// Register a mod.
        /// </summary>
        /// <param name="ModId"></param> Id of mod.
        /// <param name="display"></param> Text to display to user on mod's button.
        /// <param name="options"></param> Options the mod has.

        public void RegisterMod(string ModId, string display, Option[] options)
        {
            RawMod mod = new()
            {
                ModId = ModId,
                display = display,
                options = options
            };
            Mods.rawMods = Mods.rawMods.Append(mod);
        }

        private void CreateMods()
        {
            GameObject Settings = GameObject.Find("MAINMENU/Canvas/Pages/Setting");
            Settings.Find("ModSettingsPage").SetActive(false);
            foreach (RawMod mod in Mods.rawMods)
            {
                GameObject ModSettings = Settings.Find("Content/GameObject/ModSettings");
                GameObject ModSetting = ModSettings.Instantiate();
                GameObject Viewport = Settings.Find("ModSettingsPage/Viewport/Content");
                ModSetting.SetParent(Viewport, false);
                LayoutElement element = ModSetting.AddComponent<LayoutElement>();
                element.preferredHeight = 10f;
                element.preferredWidth = 50f;
                element.ignoreLayout = false;
                element.layoutPriority = 1;
                ModSetting.Find("ItemName").GetComponent<Text>().text = mod.display;
                ModSetting.Find("ItemName").GetComponent<RectTransform>().anchoredPosition = new Vector2(31.5272f, -1.4878f);
                ModSetting.name = mod.ModId;
                GameObject ModSettingPage = Settings.AddObject(mod.ModId);
                foreach(Option option in mod.options)
                {
                    option.Create(ModSettingPage);
                }
            }
        }

        public void Update()
        {
            foreach (Action action in SettingsUtils.updateActions)
            {
                action.Invoke();
            }
            GameObject Settings = GameObject.Find("MAINMENU/Canvas/Pages/Setting");
            if (NeedCreateMods && currentScene.name != "Intro" && currentScene.name != "Menu" && Settings != null && Settings.Find("ModSettingsPage") != null && Settings.Find("ModSettingsPage/Viewport/Content") != null)
            {
                NeedCreateMods = false;
                CreateMods();
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
    }
}
