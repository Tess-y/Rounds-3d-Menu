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

        private void Awake() {
            new Harmony(ModId).PatchAll();
            SceneManager.LoadScene(
                Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("menu", typeof(Main).Assembly).GetAllScenePaths().First(), LoadSceneMode.Single);
        }

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

    }
}
