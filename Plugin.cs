﻿using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace SettingsAPI
{
    [BepInPlugin("tairasoul.settingsapi.vaproxy", "SettingsAPI", "1.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        public static bool init = false;

        public static SettingsRegister API { get; private set; }

        public static GameObject RegisteredSettingsButton;

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

        public void Init()
        {
            if (!init)
            {
                init = true;
                RegisteredSettingsButton = new GameObject("ModSettingsAPI");
                DontDestroyOnLoad(RegisteredSettingsButton);
                API = RegisteredSettingsButton.AddComponent<SettingsRegister>();
                Log.LogInfo("ModSettingsAPI setup. Have fun!");
            }
        }
    }
}
