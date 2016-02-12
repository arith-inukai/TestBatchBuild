using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	void OnGUI() {
		GUI.Label (new Rect (10f, 10f, 100f, 50f), "Home Debug : " + DebugFlagManager.GetDebugFlag (DebugFlagManager.FlagType.HOME));
		GUI.Label (new Rect (10f, 60f, 100f, 50f), "Adv Debug : " + DebugFlagManager.GetDebugFlag (DebugFlagManager.FlagType.ADVENTURE));
	}
}
