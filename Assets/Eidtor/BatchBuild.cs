#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BatchBuild : EditorWindow{
	// ビルド対象のシーン
	//private static string[] scene = new string[]{"Assets/Scenes/test.unity"};

	private static string temp_file_name			= "Wow00.apk";

	private static string bundle_version			= "0.0.0";
	private static string bundle_code				= "0";
		
	// keystore Path
	private static string and_keystore_file_path	= "Android/release.keystore";
	
	// keystoreのパスワードはUnityEditorで設定できるが保持されないのでここに記述
	private static string and_keystore_pass 		= "arith2133";
	private static string and_keystore_alias_pass 	= "arith2133";

	private static string log_string				= "********** ";


	public static void SetBuildInfo() {

		Debug.Log(log_string + "SetBuildInfo");

		var commands = System.Environment.GetCommandLineArgs ();
		string pTmp = "";
		string[] pTmps = null;
		for (int i = 0; i < commands.Length; i++) {
			pTmp = commands [i];
			pTmps = pTmp.Split ('=');
			if (pTmps.Length > 1) {
				switch (pTmps [0]) {
				case "temp_file_name":
					temp_file_name = pTmps [1];
					break;
				case "bundle_version":
					bundle_version = pTmps [1];
					break;
				case "bundle_code":
					bundle_code = pTmps [1];
					break;
				case "and_keystore_file_path":
					and_keystore_file_path = pTmps [1];
					break;
				case "and_keystore_pass":
					and_keystore_pass = pTmps [1];
					break;
				case "and_keystore_alias_pass":
					and_keystore_alias_pass = pTmps [1];
					break;
				}
			}

			Debug.Log (log_string + string.Format("command i = {0}, value = {1}", i, pTmp));
		}
	}


	// ビルド対象のシーンの取得
	private static string[] GetScenes() {
		List<string> scene_names = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if(scene.enabled) {
				scene_names.Add(scene.path);
			}
		}
		return scene_names.ToArray();
	}
	
	// リリースビルド
	public static void ReleaseBuild(){
		
		if ( BuildiOS(true)==false ) EditorApplication.Exit(1);
		
		if ( BuildAndroid(true)==false ) EditorApplication.Exit(1);
		
		EditorApplication.Exit(0);
	}
	
	// 開発用ビルド
	[MenuItem("DevelopmentBuild/start!!!", false, 0)]
	public static void DevelopmentBuild(){
		
		Debug.Log(log_string + "DevelopmentBuild");
		
		//if ( BuildiOS(false)==false ) EditorApplication.Exit(1);
		
		if ( BuildAndroid(false)==false ) EditorApplication.Exit(1);
		
		EditorApplication.Exit(0);
	}
	
	// iOSビルド
	private static bool BuildiOS(bool release){
		
		Debug.Log(log_string + "Start Build( iOS )");

		BuildOptions opt = BuildOptions.SymlinkLibraries;
		
		// 開発用ビルドの場合のオプション設定
		
		if ( release==false ){
			opt |= BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging;
		}
		
		// ビルド
		// シーン、出力ファイル（フォルダ）、ターゲット、オプションを指定
		string errorMsg =
			BuildPipeline.BuildPlayer(GetScenes(),"ios", BuildTarget.iOS, opt);
		
		// errorMsgがない場合は成功
		if ( string.IsNullOrEmpty(errorMsg) ){
			Debug.Log(log_string + "Build( iOS ) Success.");
			return true;
			
		}
		
		Debug.Log(log_string + "Build( iOS ) ERROR!");
		Debug.LogError(errorMsg);
		
		return false;
	}
	
	// Androidビルド
	private static bool BuildAndroid(bool release){
		
		Debug.Log(log_string + "Start Build( Android )");
		
		BuildOptions opt = BuildOptions.None;
		
		// 開発用ビルドの場合のオプション設定
		if ( release==false ){
			opt |= BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging;
		}
		
		// keystoreファイルのの場所を設定
		string keystoreName =
			System.IO.Directory.GetCurrentDirectory()+"/"+ and_keystore_file_path;
		
		// set keystoreName
		PlayerSettings.Android.keystoreName = keystoreName;
		
		// パスワードの再設定
		PlayerSettings.Android.keystorePass = and_keystore_pass;
		
		// パスワードの再設定
		PlayerSettings.Android.keyaliasPass = and_keystore_alias_pass;

		// ビルド
		// シーン、出力ファイル（フォルダ）、ターゲット、オプションを指定
		string errorMsg = BuildPipeline.BuildPlayer(GetScenes(), temp_file_name, BuildTarget.Android, opt);
		
		// errorMsgがない場合は成功
		if ( string.IsNullOrEmpty(errorMsg) ){
			Debug.Log(log_string + "Build( Android ) Success.");
			return true;
		}
		
		Debug.Log(log_string + "Build( Android ) ERROR!");
		Debug.LogError(errorMsg);
		
		return false;
		
	}
}
#endif
