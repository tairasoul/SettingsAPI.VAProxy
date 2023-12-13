using Devdog.General.UI;
using HarmonyLib;
using moveen.utils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SettingsAPI
{
    public static class ArrayExtensions
    {
        public static T[] Append<T>(this T[] array, T value) where T : struct
        {
            return array.AddToArray(value);
        }
    }
    public static class GameObjectExtensions
    {
        public static GameObject Find(this GameObject @object, string name)
        {
            return @object.transform.Find(name).gameObject;
        }

        public static void SetParent(this GameObject @object, GameObject parent, bool worldPositionStays)
        {
            @object.transform.SetParent(parent.transform, worldPositionStays);
        }

        public static GameObject AddObject(this GameObject @object, string name)
        {
            GameObject obj = new(name);
            obj.SetParent(@object, false);
            return obj;
        }

        public static GameObject Instantiate(this GameObject @object)
        {
            return GameObject.Instantiate(@object);
        }

        public static GameObject[] GetChildren(this GameObject @object)
        {
            int childCount = @object.transform.childCount;
            GameObject[] children = new GameObject[childCount];
            for (int i = 0; i < childCount; i++)
            {
                children[i] = @object.transform.GetChild(i).gameObject;
            }
            return children;
        }
    }

    public class ComponentUtils
    {
        public static GameObject CreateToggle(string display, string id)
        {
            GameObject Toggle = new(id);
            Toggle.AddComponent<RectTransform>();
            Toggle.transform.localScale = new Vector3(2.1268f, 2.1268f, 2.1268f);
            Toggle toggle = Toggle.AddComponent<Toggle>();
            GameObject Label = Toggle.AddObject("Label");
            Label.AddComponent<CanvasRenderer>();
            Label.AddComponent<RectTransform>();
            Label.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            Text text = Label.AddComponent<Text>();
            text.alignment = TextAnchor.MiddleLeft;
            text.fontSize = 100;
            GameObject original = GameObject.Find("MAINMENU/Canvas/Pages/Setting/Resolution/VSync/Background");
            text.font = original.transform.parent.gameObject.Find("Label").GetComponent<Text>().font;
            text.text = display;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.color = new Color(0.9843f, 0.6902f, 0.2314f);
            GameObject background = Toggle.AddObject("Background");
            background.AddComponent<RectTransform>().anchoredPosition = new Vector2(10, 0);
            background.AddComponent<CanvasRenderer>();
            background.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            Image image = background.AddComponent<Image>();
            image.sprite = original.GetComponent<Image>().sprite;
            GameObject checkmark = background.AddObject("Checkmark");
            checkmark.AddComponent<RectTransform>();
            checkmark.AddComponent<CanvasRenderer>();
            Image checkmarkImage = checkmark.AddComponent<Image>();
            checkmarkImage.enabled = false;
            toggle.image = checkmarkImage;
            checkmarkImage.sprite = original.Find("Checkmark").GetComponent<Image>().sprite;
            return Toggle;
        }

        public static GameObject CreateButton(string display, string id)
        {
            GameObject Settings = GameObject.Find("MAINMENU/Canvas/Pages/Setting");
            GameObject Container = Settings.Find("Content/GameObject");
            GameObject Audio = Container.Find("audio");
            GameObject Button = Audio.Instantiate();
            Button.name = id;
            GameObject ItemName = Button.Find("ItemName");
            Text text = ItemName.GetComponent<Text>();
            text.text = display;
            return Button;
        }

        public static GameObject CreateSlider(string display, string id)
        {
            GameObject Settings = GameObject.Find("MAINMENU/Canvas/Pages/Setting");
            GameObject Sensitivity = Settings.Find("AUDIO/SFX");
            GameObject Slider = Sensitivity.Instantiate();
            Slider.name = id;
            Slider.GetComponent<Text>().text = display;
            return Slider;
        }
    }
    internal class SettingsUtils
    {
        internal static GameObject ModSettingsMiniPage;

        internal static Action[] updateActions = { };

        internal static void SetupButton(Button button)
        {
            button.onClick.AddListener(() =>
            {
                GameObject Settings = GameObject.Find("MAINMENU/Canvas/Pages/Setting");
                GameObject[] children = Settings.GetChildren();
                foreach (GameObject child in children)
                {
                    if (child.name != "Header" && child.name != "Content" && child.name.ToLower() != button.name.ToLower())
                    {
                        child.SetActive(false);
                    }
                }
            });
        }
        internal static GameObject CreateSettingsButton()
        {
            GameObject Settings = GameObject.Find("MAINMENU/Canvas/Pages/Setting");
            UIWindowPage Setting = Settings.GetComponent<UIWindowPage>();
            Setting.OnShow += () =>
            {
                Settings.Find("ModSettingsPage").SetActive(false);
                foreach (RawMod mod in Mods.rawMods)
                {
                    Settings.Find(mod.ModId)?.SetActive(false);
                }
            };
            GameObject AudioPage = Settings.Find("AUDIO");
            GameObject Container = Settings.Find("Content/GameObject");
            GameObject ScrollbarVertical = GameObject.Find("MAINMENU/Canvas/Pages/Inventory/Content/__INVENTORY_CONTAINER/Container/InventorySlots/Scrollbar Vertical");
            GameObject ScrollVertical = ScrollbarVertical.Instantiate();
            ScrollVertical.name = "Scrollbar Vertical";
            GameObject Audio = Container.Find("audio");
            SetupButton(Audio.GetComponent<Button>());
            SetupButton(Container.Find("controls").GetComponent<Button>());
            SetupButton(Container.Find("resolution").GetComponent<Button>());
            GameObject ModSettings = Audio.Instantiate();
            ModSettings.SetParent(Container, false);
            ModSettings.name = "ModSettings";
            ModSettings.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.0005f, -170.8331f);
            GameObject ItemName = ModSettings.Find("ItemName");
            Text text = ItemName.GetComponent<Text>();
            text.text = "Mod Settings";
            GameObject ModPage = AudioPage.Instantiate();
            ModPage.name = "ModSettingsPage";
            ModPage.SetParent(Settings, false);
            ModPage.removeAllChildrenImmediate(false);
            ModPage.SetActive(false);
            ScrollVertical.SetParent(ModPage, false);
            ScrollVertical.GetComponent<RectTransform>().anchoredPosition = new Vector2(-106.245f, -154.1404f);
            Scrollbar bar = ScrollVertical.GetComponent<Scrollbar>();
            bar.direction = Scrollbar.Direction.TopToBottom;
            bar.interactable = true;
            bar.navigation = Navigation.defaultNavigation;
            bar.useGUILayout = true;
            ScrollVertical.GetComponent<RectTransform>().localScale = new Vector3(1, 0.6f, 1);
            GameObject Exit = Container.Find("exit");
            Exit.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.0005f, -227.8329f);
            ScrollRect scroll = ModPage.AddComponent<ScrollRect>();
            scroll.scrollSensitivity = 25;
            scroll.movementType = ScrollRect.MovementType.Elastic;
            scroll.verticalScrollbar = ScrollbarVertical.GetComponent<Scrollbar>();
            scroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
            scroll.vertical = true;
            scroll.horizontal = false;
            GameObject Viewport = new("Viewport");
            Viewport.SetParent(ModPage, false);
            GameObject Content = Viewport.AddObject("Content");
            Content.AddComponent<RectTransform>().anchoredPosition = new Vector2(-309.7879f, 132.4231f);
            ContentSizeFitter SizeFitter = Content.AddComponent<ContentSizeFitter>();
            SizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            SizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            VerticalLayoutGroup group = Content.AddComponent<VerticalLayoutGroup>();
            group.childControlHeight = true;
            group.childControlWidth = true;
            group.childAlignment = TextAnchor.UpperLeft;
            group.spacing = 25f;
            /*group.spacing = new Vector2(10, 10);
            group.cellSize = new Vector2(50, 20);
            group.startAxis = GridLayoutGroup.Axis.Horizontal;
            group.startCorner = GridLayoutGroup.Corner.UpperLeft;
            group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            group.constraintCount = 5;*/
            return ModSettings;
        }

        internal static void SetupSettingsButton(GameObject SettingsButton)
        {
            Button button = SettingsButton.GetComponent<Button>();
            GameObject Settings = GameObject.Find("MAINMENU/Canvas/Pages/Setting");
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                GameObject[] children = Settings.GetChildren();
                foreach (GameObject child in children)
                {
                    if (child.name != "Header" && child.name != "Content")
                    {
                        child.SetActive(false);
                    }
                }
                Settings.Find("ModSettingsPage").SetActive(true);
            });
        }
    }
}
