using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Linq;



namespace R3DCore {
    [HarmonyPatch]
    public class Networking:MonoBehaviour {
        private static List<string> clientSideGUIDs = new List<string>();
        internal static int roomNumber = 0;
        internal static byte MaxPlayers = 4;
        internal static string lobbyName;
        internal static PlayType playType;


        public string LobbyName { get => lobbyName; set => lobbyName = value; }

        public static void RegesterClientSide(string GUID) {
            if(!clientSideGUIDs.Contains(GUID)) { clientSideGUIDs.Add(GUID); }
        }

        public void Play(int type) {
            Play((PlayType)type);
        }

        public void Play(PlayType type) {
            playType = type;
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Connection), "OnConnectedToMaster")]
        public static bool OnConnectedToMasterPatch() {
            if(playType == PlayType.Quick) {
                QuickPlay();
            } else if(playType == PlayType.Host) {

                RoomOptions options = new RoomOptions();
                options.MaxPlayers = 4;
                options.IsOpen = true;
                options.IsVisible = false;

                string roomName = lobbyName;
                var loadedMods = BepInEx.Bootstrap.Chainloader.PluginInfos;
                List<string> modIds = new List<string>();

                foreach(var modId in loadedMods.Keys.Where(id => !clientSideGUIDs.Contains(loadedMods[id].Metadata.GUID))) {
                    modIds.Add(loadedMods[modId].Metadata.GUID + loadedMods[modId].Metadata.Version);
                }
                modIds.Sort();
                roomName += modIds.Join();
                PhotonNetwork.CreateRoom(roomName, options);

            } else if(playType == PlayType.Join) {
                string roomName = lobbyName;
                var loadedMods = BepInEx.Bootstrap.Chainloader.PluginInfos;
                List<string> modIds = new List<string>();

                foreach(var modId in loadedMods.Keys.Where(id => !clientSideGUIDs.Contains(loadedMods[id].Metadata.GUID))) {
                    modIds.Add(loadedMods[modId].Metadata.GUID + loadedMods[modId].Metadata.Version);
                }
                modIds.Sort();
                roomName += modIds.Join();
                PhotonNetwork.JoinRoom(roomName);
            }
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Connection), "OnJoinRoomFailed")]
        public static bool OnJoinRoomFailedPatch(short returnCode, string message) {
            if(playType == PlayType.Quick) {
                QuickPlay();
            } else if(playType == PlayType.Host) {
                NotificationHandler.instance.PlayNotification("Hosting Has Failed");
                NotificationHandler.instance.PlayNotification(message, 3);

            } else if(playType == PlayType.Join) {
                NotificationHandler.instance.PlayNotification("Join Failed");
                NotificationHandler.instance.PlayNotification(message, 3);
            }
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
            options.MaxPlayers = 4;
            options.IsOpen = true;
            options.IsVisible = false;
            PhotonNetwork.JoinOrCreateRoom(roomName, options, null, null);
            UnityEngine.Debug.Log(roomName);
        }

        public enum PlayType {
            Quick = 0,
            Host = 1,
            Join = 2,
        }
    }
}
