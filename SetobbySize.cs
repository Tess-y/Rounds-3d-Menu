using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace R3DCore.Menu {
    public class SetobbySize:MonoBehaviour {
        public TextMeshProUGUI text;
        public void Set(float i) {
            text.text = i.ToString();
            Networking.MaxPlayers = (byte)(int)i;
        }
    }
}
