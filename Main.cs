using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BepInEx;
using System;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
using HarmonyLib;

namespace R3DCore.Menu {
    // Declares our Mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our Mod Is associated with
    [BepInProcess("ROUNDS 3D.exe")]
    public class Main:BaseUnityPlugin {

        private const string ModId = "com.Root.Menu";
        private const string ModName = "Menu";
        public const string Version = "0.0.0"; // What version are we On (major.minor.patch)?
        public static string Menu;
        internal AssetBundle assetBundle;

        void Awake() {
            new Harmony(ModId).PatchAll();
            assetBundle = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("menu", typeof(Main).Assembly);
            Menu = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("menuscene", typeof(Main).Assembly).GetAllScenePaths().First();
            SceneManager.LoadScene(Menu, LoadSceneMode.Single);
        }
        void Start() {
            var Host = assetBundle.LoadAsset<GameObject>("Host");
            var Join = assetBundle.LoadAsset<GameObject>("Join");
            MenuHandler.ResgesterMenu(Host.GetComponent<Canvas>(), MenuHandler.MenuType.Play);
            MenuHandler.ResgesterMenu(Join.GetComponent<Canvas>(), MenuHandler.MenuType.Play);
        }
    }
}
