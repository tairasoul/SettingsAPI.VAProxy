using Devdog.General.UI;
using HarmonyLib;
using moveen.utils;
using System;
using Unity.VisualScripting;
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
    internal class SettingsUtils
    {
        public static GameObject ModSettingsMiniPage;

        public static Action[] updateActions = { };

        public static void SetupButton(Button button)
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
        public static GameObject CreateSettingsButton()
        {
            GameObject Settings = GameObject.Find("MAINMENU/Canvas/Pages/Setting");
            UIWindowPage Setting = Settings.GetComponent<UIWindowPage>();
            Setting.OnShow += () =>
            {
                Settings.Find("ModSettingsPage").SetActive(false);
            };
            GameObject AudioPage = Settings.Find("AUDIO");
            GameObject Container = Settings.Find("Content/GameObject");
            GameObject ScrollbarVertical = GameObject.Find("MAINMENU/Canvas/Pages/Inventory/Content/__INVENTORY_CONTAINER/Container/InventorySlots/Scrollbar Vertical");
            GameObject ScrollVertical = GameObject.Instantiate(ScrollbarVertical);
            ScrollVertical.name = "Scrollbar Vertical";
            GameObject Audio = Container.Find("audio");
            SetupButton(Audio.GetComponent<Button>());
            SetupButton(Container.Find("controls").GetComponent<Button>());
            SetupButton(Container.Find("resolution").GetComponent<Button>());
            GameObject ModSettings = GameObject.Instantiate(Audio);
            ModSettings.SetParent(Container, false);
            ModSettings.name = "ModSettings";
            ModSettings.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.0005f, -170.8331f);
            //ModSettings.transform.SetPositionAndRotation(new Vector3(Audio.transform.position.x, Audio.transform.position.y - 25, Audio.transform.position.z), new Quaternion());
            GameObject ItemName = ModSettings.Find("ItemName");
            Text text = ItemName.GetComponent<Text>();
            text.text = "Mod Settings";
            GameObject ModPage = GameObject.Instantiate(AudioPage);
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
            Content.AddComponent<RectTransform>().anchoredPosition = new Vector2(-292.5862f, 108.9978f);
            ContentSizeFitter SizeFitter = Content.AddComponent<ContentSizeFitter>();
            SizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            SizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            GridLayoutGroup group = Content.AddComponent<GridLayoutGroup>();
            group.spacing = new Vector2(10, 10);
            group.startAxis = GridLayoutGroup.Axis.Horizontal;
            group.startCorner = GridLayoutGroup.Corner.UpperLeft;
            group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            group.constraintCount = 5;
            //Exit.transform.SetPositionAndRotation(new Vector3(Exit.transform.position.x, Exit.transform.position.y - 25, Exit.transform.position.z), new Quaternion());
            return ModSettings;
            /*GameObject SettingsButton = new GameObject("ModSettings");
            SettingsButton.layer = LayerMask.NameToLayer("UI");
            RectTransform transform = SettingsButton.AddComponent<RectTransform>();
            transform.anchorMax = new Vector2(1, 1);
            transform.anchorMin = new Vector2(0, 0);
            transform.offsetMax = new Vector2(70.4986f, -252.9987f);
            transform.offsetMin = new Vector2(-70.5014f, -202.9987f);
            transform.sizeDelta = new Vector2(141, -50);*/
            //transform.anchoredPosition = new Vector2(-0.0005f, -170.8797f);
            /*transform.position = new Vector3(1079.926f, 460.4991f, -1709);
            SettingsButton.AddComponent<CanvasRenderer>();
            Image image = SettingsButton.AddComponent<Image>();
            image.type = Image.Type.Simple;
            image.color = new Color(1, 1, 1, 0.0588f);
            SettingsButton.AddComponent<LayoutElement>();
            SettingsButton.AddComponent<Button>();
            GameObject itemName = new GameObject("ItemName");
            RectTransform item = itemName.AddComponent<RectTransform>();
            itemName.AddComponent<CanvasRenderer>();
            itemName.transform.SetParent(SettingsButton.transform);
            //item.anchoredPosition = new Vector2(64.9991f, 0);
            Text text = itemName.AddComponent<Text>();
            text.text = "Mod Settings";
            item.position = new Vector3(1055.512f, 460.4991f, -1709);*/
            /*updateActions = updateActions.Add(() =>
            {
                //transform.anchoredPosition = new Vector2(-0.0005f, -170.8797f);
                transform.position = new Vector3(1079.926f, 460.4991f, -1709);
                transform.anchorMax = new Vector2(1, 1);
                transform.anchorMin = new Vector2(0, 0);
                transform.offsetMax = new Vector2(70.4986f, -252.9987f);
                transform.offsetMin = new Vector2(-70.5014f, -202.9987f);
                transform.sizeDelta = new Vector2(141, -50);
                //transform.localPosition = new Vector3(1079.926f, 460.4991f, -1709);
                transform.localScale = new Vector3(1, 1, 1);
                //item.anchoredPosition = new Vector2(64.9991f, 0);
                item.position = new Vector3(1055.512f, 460.4991f, -1709);
                //item.localPosition = new Vector3(1055.512f, 460.4991f, -1709);
            });
            return SettingsButton;*/
        }

        public static void SetupSettingsButton(GameObject SettingsButton)
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
