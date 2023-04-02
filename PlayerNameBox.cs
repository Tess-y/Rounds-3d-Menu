using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;
using System.Linq;
using System.Reflection.Emit;

namespace R3DCore.Menu {
    [HarmonyPatch]
    public class PlayerNameBox:MonoBehaviour {
        // Start is called before the first frame update
        void Start() {
            transform.Find("Placeholder").GetComponent<Text>().text = PlayerPrefs.GetString("PlayerName", "SetPlayerName");
        }
        public void SetName() {
            string text = transform.Find("Text").GetComponent<Text>().text.Replace(" ", "_");
            PlayerPrefs.SetString("PlayerName", text == "" ? "Player" : text);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(PL_Damagable), "RPCA_ADDCARDWITHID")]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            List<CodeInstruction> injectedCode = new List<CodeInstruction>() {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PL_Damagable),"player")),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Player),"refs")),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Player.PlayerRefs),"view")),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Photon.Pun.PhotonView), "Owner")),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Photon.Realtime.Player), "NickName")),
                new CodeInstruction(OpCodes.Ldstr, " Got ")
            };

            var code = instructions.ToList();
            int i = 0;
            while(code[++i].operand == null || !code[i].operand.ToString().Contains("I got ")) ;
            code[i].operand = "You got ";
            while(code[++i].operand == null || !code[i].operand.ToString().Contains("Opponent got")) ;
            code.RemoveAt(i);
            code.InsertRange(i, injectedCode);
            while(!code[++i].opcode.Equals(OpCodes.Call) || !code[i].operand.ToString().Contains("Concat")) ;
            code.Insert(i, code[i]);
            return code;
        }
    }
}
