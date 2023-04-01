using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace R3DCore.Menu {
    public class SetobbySize:MonoBehaviour {
        public Text text;
        public void Set(float i) {
            text.text = i.ToString();
            Networking.MaxPlayers = (byte)(int)i;
        }
    }
}
