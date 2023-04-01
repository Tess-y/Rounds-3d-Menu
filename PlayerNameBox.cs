using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace R3DCore.Menu {
    public class PlayerNameBox:MonoBehaviour {
        // Start is called before the first frame update
        void Start() {
            transform.Find("Placeholder").GetComponent<Text>().text = PlayerPrefs.GetString("PlayerName", "SetPlayerName");
        }
        public void SetName() {
            string text = transform.Find("Text").GetComponent<Text>().text.Replace(" ", "_");
            PlayerPrefs.SetString("PlayerName", text == "" ? "Player" : text);
        }
    }
}
