using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class DebugFlagManager : ScriptableObject {

	public enum FlagType {
		HOME,
		ADVENTURE,
	}

	[SerializeField]
	public bool HOME_SCENE_DEBUG;
	[SerializeField]
	public bool ADV_SCENE_DEBUG;

	const string ASSET_NAME = "DebugFlagManager";
	const string SETTING_PATH = "Resources";
	const string SETTING_ASSET_EXTENSION = ".asset";

	private static DebugFlagManager s_pInstance;
	static DebugFlagManager Instance {
		get {
			if (s_pInstance == null) {
				s_pInstance = Resources.Load (ASSET_NAME) as DebugFlagManager;
				if (s_pInstance == null) {
					s_pInstance = CreateInstance<DebugFlagManager> ();
					#if UNITY_EDITOR
					string pProperPath = Path.Combine(Application.dataPath, SETTING_PATH);
					if(!Directory.Exists(pProperPath)) {
						AssetDatabase.CreateFolder("Assets", "Resources");
					}
					AssetDatabase.CreateAsset (s_pInstance, "Assets/Resources/DebugFlagManager.asset");
					#endif
				}
			}
			return s_pInstance;
		}
	}
	public static bool GetDebugFlag(FlagType type) {
		switch (type) {
		case FlagType.HOME:
			return Instance.HOME_SCENE_DEBUG;
		case FlagType.ADVENTURE:
			return Instance.ADV_SCENE_DEBUG;
		}
		return false;
	}
	public static void SetDebugFlag(FlagType type, bool value) {
		switch (type) {
		case FlagType.HOME:
			Instance.HOME_SCENE_DEBUG = value;
			break;
		case FlagType.ADVENTURE:
			Instance.ADV_SCENE_DEBUG = value;
			break;
		}
		#if UNITY_EDITOR
		EditorUtility.SetDirty(Instance);
		AssetDatabase.SaveAssets();
		#endif
	}
}
