using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace R3DCore.Menu {
    public class MenuHandler:MonoBehaviour {

        public static MenuHandler instance;

        public GameObject Template;
        public GameObject TemplateButton;

        private static int activeMenu = 0;

        internal static List<(Canvas,MenuType,Canvas)> menus = new List<(Canvas,MenuType,Canvas)> ();

        private static List<int> menuTree = new List<int>(); 

        public int ActiveMenu {
            get => activeMenu;
            set {
                transform.GetChild(activeMenu).gameObject.SetActive(false);
                menuTree.Add(activeMenu);
                activeMenu = value;
                transform.GetChild(activeMenu).gameObject.SetActive(true);
            }
        }

        void Awake() {
            instance = this;
            activeMenu = 0;
            menuTree = new List<int>();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Networking.roomNumber = 0;
        }

        private void Start() {
            StartCoroutine(AddMenues());
            IEnumerator AddMenues() {
                for(int _=0;_<5;_++)
                    yield return null;
                foreach (var menu in menus) {
                    AddMenu(menu.Item1, menu.Item2, menu.Item3);
                }
            }
        }

        void Update() {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                GoBack();
            }
        }

        public void GoBack() {
            if(menuTree.Count > 0) {
                ActiveMenu = menuTree.Last();
                menuTree.RemoveAt(menuTree.Count - 1);
                menuTree.RemoveAt(menuTree.Count - 1);
            } else {
                ActiveMenu = 0;
                menuTree.RemoveAt(menuTree.Count - 1);
            }
        }

        public static void ResgesterMenu(Canvas canvas, MenuType type, Canvas parent = null) {
            if(type == MenuType.Sub && parent == null)
                throw new ArgumentException("SubMenus require a parent");
            if(type == MenuType.Sub && !menus.Any(item => item.Item1.Equals(parent)))
                throw new ArgumentException("SubMenus can't be regestered before parent");

            menus.Add((canvas, type, parent));
        }

        private int AddMenu(Canvas canvas, MenuType type, Canvas parent) {
            canvas = Instantiate(canvas);
            canvas.name = canvas.name.Replace("(Clone)", "");
            switch(type) {
                case MenuType.Play:
                    SetUpButton(transform.GetChild(1).Find("Scroll View/Viewport/Content/"), transform.childCount, canvas.name);
                    break;
                case MenuType.Mod:
                    SetUpButton(transform.GetChild(2).Find("Scroll View/Viewport/Content/"), transform.childCount, canvas.name);
                    break;
                case MenuType.Option:
                    SetUpButton(transform.GetChild(3).Find("Scroll View/Viewport/Content/"), transform.childCount, canvas.name);
                    break;
                case MenuType.Sub:
                    SetUpButton(transform.GetChild(menus.IndexOf(menus.First(item => item.Item1.Equals(parent)))).Find("Scroll View/Viewport/Content/"), transform.childCount, canvas.name);
                    break;
            }
            canvas.transform.SetParent(transform, false);
            canvas.gameObject.SetActive(false);
            Instantiate(Template.transform.GetChild(0).gameObject, canvas.transform);
            return transform.childCount-1;
        }

        private void SetUpButton(Transform parent, int ID, string name) {
            var button = Instantiate(TemplateButton, parent);
            button.SetActive(true);
            button.transform.localPosition = new Vector3(950-8.5f, parent.childCount * -160);
            button.GetComponent<Button>().onClick.AddListener(() => ActiveMenu = ID);
            button.GetComponentInChildren<TextMeshProUGUI>().text = name;
        }

        public enum MenuType {
            Play,
            Mod,
            Option,
            Sub
        }

    }
}
