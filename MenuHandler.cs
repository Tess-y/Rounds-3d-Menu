using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace R3DCore.Menu {
    public class MenuHandler:MonoBehaviour {

        public static MenuHandler instance;

        private static int activeMenu = 0;

        public int ActiveMenu {
            get => activeMenu;
            set {
                transform.GetChild(activeMenu).gameObject.SetActive(false);
                activeMenu = value;
                transform.GetChild(activeMenu).gameObject.SetActive(true);
            }
        }

        void Awake() {
            instance = this;
            activeMenu = 0;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Networking.roomNumber = 0;
        }




    }
}
