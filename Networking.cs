using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Linq;



namespace Rounds3DModding {
    [HarmonyPatch]
    public class Networking:MonoBehaviour {
        private static List<string> clientSideGUIDs = new List<string>();
        internal static int roomNumber = 0;
        internal static byte MaxPlayers = 4;
        public static void RegesterClientSide(string GUID) {
            if(!clientSideGUIDs.Contains(GUID)) { clientSideGUIDs.Add(GUID); }
        }

        public void Play() {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Connection), "OnConnectedToMaster")]
        public static bool OnConnectedToMasterPatch() {
            QuickPlay();
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Connection), "OnJoinRoomFailed")]
        public static bool OnJoinRoomFailedPatch() {
            QuickPlay();
            return false;
        }

        private static void QuickPlay() {
            string roomName = $"{roomNumber++}__ModdedRoom";
            var loadedMods = BepInEx.Bootstrap.Chainloader.PluginInfos;
            List<string> modIds = new List<string>();

            foreach(var modId in loadedMods.Keys.Where(id => !clientSideGUIDs.Contains(loadedMods[id].Metadata.GUID))) {
                modIds.Add(loadedMods[modId].Metadata.GUID + loadedMods[modId].Metadata.Version);
            }
            modIds.Sort();
            roomName += modIds.Join();
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = MaxPlayers;
            options.IsOpen = true;
            options.IsVisible = false;
            PhotonNetwork.JoinOrCreateRoom(roomName, options, null, null);
            UnityEngine.Debug.Log(roomName);
        }
    }
}
