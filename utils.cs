using Devdog.General.UI;
using HarmonyLib;
using moveen.utils;
using System;
using System.IO;
using System.Reflection;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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
        public static Font GetFont(string name)
        {
            Object[] fonts = Object.FindObjectsOfTypeAll(typeof(Font));
            foreach (Font font in fonts)
            {
                if (font.name == name) return font;
            }
            return null;
        }
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
            //GameObject original = GameObject.Find("MAINMENU/Canvas/Pages/Setting/Resolution/VSync/Background");
            text.font = GetFont("Orbitron-Bold");
            text.text = display;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.color = new Color(0.9843f, 0.6902f, 0.2314f);
            GameObject background = Toggle.AddObject("Background");
            background.AddComponent<RectTransform>().anchoredPosition = new Vector2(10, 0);
            background.AddComponent<CanvasRenderer>();
            background.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            Image image = background.AddComponent<Image>();
            Sprite sprite = Sprite.Create(Plugin.SpriteAssets.LoadAsset<Texture2D>("assets/ComponentAssets/Nuke Panel Sprite.png"), new Rect(0, 0, 32, 32), new Vector2(16, 16));
            Plugin.Log.LogInfo(sprite);
            image.sprite = sprite;//original.GetComponent<Image>().sprite;
            GameObject checkmark = background.AddObject("Checkmark");   
            checkmark.AddComponent<RectTransform>();
            checkmark.AddComponent<CanvasRenderer>();
            Image checkmarkImage = checkmark.AddComponent<Image>();
            Sprite checkmarkSprite = Sprite.Create(Plugin.SpriteAssets.LoadAsset<Texture2D>("assets/ComponentAssets/Checkmark.png"), new Rect(0, 0, 32, 32), new Vector2(16, 16));
            checkmarkImage.enabled = false;
            toggle.image = checkmarkImage;
            checkmarkImage.sprite = checkmarkSprite;
            toggle.onValueChanged.AddListener((bool active) =>
            {
                checkmarkImage.enabled = active;
            });
            return Toggle;
        }

        public static GameObject CreateButton(string display, string id)
        {
            GameObject Button = new(id);
            Button.AddComponent<RectTransform>();
            Button.AddComponent<CanvasRenderer>();
            Image img = Button.AddComponent<Image>();
            img.type = Image.Type.Simple;
            img.color = new Color(1, 1, 1, 0.0588f);
            Button.AddComponent<LayoutElement>();
            Button button = Button.AddComponent<Button>();
            button.image = img;
            button.transition = Selectable.Transition.ColorTint;
            GameObject name = Button.AddObject("ItemName");
            RectTransform transform = name.AddComponent<RectTransform>();
            transform.anchoredPosition = new Vector2(65, 0);
            name.AddComponent<CanvasRenderer>();
            Text text = name.AddComponent<Text>();
            text.text = display;
            text.alignment = TextAnchor.MiddleLeft;
            text.font = GetFont("Orbitron-Regular");
            text.fontSize = 20;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.resizeTextMaxSize = 40;
            text.resizeTextMinSize = 2;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            /*GameObject Settings = GameObject.Find("MAINMENU/Canvas/Pages/Setting");
            GameObject Container = Settings.Find("Content/GameObject");
            GameObject Audio = Container.Find("audio");
            GameObject Button = Audio.Instantiate();
            Button.name = id;
            GameObject ItemName = Button.Find("ItemName");
            Text text = ItemName.GetComponent<Text>();
            text.text = display;*/
            return Button;
        }

        public static GameObject CreateSlider(string display, string id)
        {
            /*GameObject Settings = GameObject.Find("MAINMENU/Canvas/Pages/Setting");
            GameObject Sensitivity = Settings.Find("AUDIO/SFX");
            GameObject Slider = Sensitivity.Instantiate();
            Slider.name = id;
            Slider.GetComponent<Text>().text = display;*/
            GameObject Slider = new(id);
            Slider.AddComponent<RectTransform>();
            Slider.AddComponent<CanvasRenderer>();
            Text text = Slider.AddComponent<Text>();
            text.alignment = TextAnchor.MiddleLeft;
            text.font = GetFont("Orbitron-Medium");
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.fontSize = 20;
            text.text = display;
            text.resizeTextMaxSize = 40;
            text.resizeTextMinSize = 1;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            GameObject SliderObj = Slider.AddObject("Slider");
            SliderObj.AddComponent<RectTransform>().anchoredPosition = new Vector2(-146.4058f, -36.39f);
            GameObject background = SliderObj.AddObject("Background");
            background.AddComponent<RectTransform>();
            background.AddComponent<CanvasRenderer>();
            Image bg = background.AddComponent<Image>();
            bg.sprite = Sprite.Create(Plugin.SpriteAssets.LoadAsset<Texture2D>("assets/ComponentAssets/Spritemap.png"), new Rect(310, 939, 68, 68), new Vector2(34, 34));
            bg.type = Image.Type.Sliced;
            GameObject FillArea = Slider.AddObject("Fill Area");
            FillArea.AddComponent<RectTransform>().anchoredPosition = new Vector2(-5, 0);
            GameObject Fill = FillArea.AddObject("Fill");
            Fill.AddComponent<RectTransform>();
            Fill.AddComponent<CanvasRenderer>();
            Image fill = Fill.AddComponent<Image>();
            fill.sprite = Sprite.Create(Plugin.SpriteAssets.LoadAsset<Texture2D>("assets/ComponentAssets/Spritemap.png"), new Rect(310, 939, 68, 68), new Vector2(34, 34));
            GameObject HandleSlide = Slider.AddObject("Handle Slide Area");
            HandleSlide.AddComponent<RectTransform>();
            GameObject Handle = HandleSlide.AddObject("Handle");
            Handle.AddComponent<RectTransform>();
            Handle.AddComponent<CanvasRenderer>();
            Image handle = Handle.AddComponent<Image>();
            handle.sprite = Sprite.Create(Plugin.SpriteAssets.LoadAsset<Texture2D>("assets/ComponentAssets/Seperator_bar.png"), new Rect(0, 0, 17, 20), new Vector2(8.5f, 10));
            Slider slider = SliderObj.AddComponent<Slider>();
            slider.direction = UnityEngine.UI.Slider.Direction.LeftToRight;
            slider.fillRect = Fill.GetComponent<RectTransform>();
            slider.handleRect = Handle.GetComponent<RectTransform>();
            slider.transition = Selectable.Transition.ColorTint;
            return Slider;
        }

        /*public static GameObject CreateScrollbar(string id)
        {

        }*/
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
            ModPage.AddComponent<CanvasRenderer>();
            ScrollVertical.SetParent(ModPage, false);
            ScrollVertical.GetComponent<RectTransform>().anchoredPosition = new Vector2(-106.245f, -154.1404f);
            Scrollbar bar = ScrollVertical.GetComponent<Scrollbar>();
            bar.direction = Scrollbar.Direction.BottomToTop;
            bar.interactable = true;
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
            RectTransform ViewportRect = Viewport.AddComponent<RectTransform>();
            Viewport.AddComponent<CanvasRenderer>();
            Viewport.AddComponent<Animator>();
            CanvasGroup canvas = Viewport.AddComponent<CanvasGroup>();
            canvas.blocksRaycasts = true;
            canvas.interactable = true;
            Viewport.AddComponent<Mask>();
            Viewport.AddComponent<AnimatorHelper>();
            Viewport.SetParent(ModPage, false);
            ViewportRect.anchoredPosition = new Vector2(73.3544f, -190.1612f);
            ViewportRect.sizeDelta = new Vector2(1000, 700);
            GameObject Content = Viewport.AddObject("Content");
            RectTransform contentTransform = Content.AddComponent<RectTransform>();
            contentTransform.anchoredPosition = new Vector2(-11.8327f, 0.0009f);
            contentTransform.sizeDelta = new Vector2(300, 575);
            scroll.content = contentTransform;
            scroll.inertia = true;
            scroll.horizontal = false;
            scroll.decelerationRate = 0.135f;
            scroll.elasticity = 0.1f;
            scroll.movementType = ScrollRect.MovementType.Elastic;
            scroll.scrollSensitivity = 25;
            scroll.vertical = true;
            scroll.verticalScrollbar = bar;
            scroll.viewport = ViewportRect;
            scroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
            ContentSizeFitter SizeFitter = Content.AddComponent<ContentSizeFitter>();
            SizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            SizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            GridLayoutGroup group = Content.AddComponent<GridLayoutGroup>();
            group.childAlignment = TextAnchor.UpperLeft;
            group.spacing = new Vector2(80, 20);
            group.cellSize = new Vector2(350, 50);
            group.startCorner = GridLayoutGroup.Corner.UpperLeft;
            group.startAxis = GridLayoutGroup.Axis.Horizontal;
            group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            group.constraintCount = 2;
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
