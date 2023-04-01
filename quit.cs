using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace R3DCore.Menu {
    [HarmonyPatch]
    internal class quit:MonoBehaviour {
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
        public void Quit() {
            Application.Quit();
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(ArchiveScript), "Update")]
        public static bool QuitHandler(float ___escapeCounter, ArchiveScript __instance) {
            if(Input.GetKey(KeyCode.Escape) && ___escapeCounter + Time.deltaTime > 1f) {
                PhotonNetwork.Disconnect();
                DestroyImmediate(__instance.gameObject);
                typeof(ArchiveScript).GetField("intance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, null);
                SceneManager.LoadScene(Main.Menu, LoadSceneMode.Single);
                return false;
            }
            return true;
        }
    }
}
