using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using System.IO;

namespace SettingsAPI
{
    [BepInPlugin("tairasoul.settingsapi.vaproxy", "SettingsAPI", "1.3.6")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        public static bool init = false;

        public static SettingsRegister API { get; private set; }

        public static GameObject RegisteredSettingsButton;

        public static AssetBundle SpriteAssets;

        public void Awake()
        {
            Log = Logger;
        }

        public void Start()
        {
            Init();
        }

        public void OnDestroy()
        {
            Init();
        }
        
        Stream GetAssets()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream("SettingsAPI.component.sprites");
        }

        public void Init()
        {
            if (!init)
            {
                SpriteAssets = AssetBundle.LoadFromStream(GetAssets());
                init = true;
                RegisteredSettingsButton = new GameObject("ModSettingsAPI");
                DontDestroyOnLoad(RegisteredSettingsButton);
                API = RegisteredSettingsButton.AddComponent<SettingsRegister>();
                Log.LogInfo("ModSettingsAPI setup. Have fun!");
            }
        }
    }
}
