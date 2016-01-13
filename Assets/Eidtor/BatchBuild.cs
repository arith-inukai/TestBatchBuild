#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BatchBuild : EditorWindow{
	// ビルド対象のシーン
	//private static string[] scene = new string[]{"Assets/Scenes/test.unity"};

	private static string android_package_name = "Wow00.apk";
	
	// keystore Path
	private static string keystorePath = "Android/release.keystore";
	
	// keystoreのパスワードはUnityEditorで設定できるが保持されないのでここに記述
	private static string keystorePass = "arith2133";
	private static string keyaliasPass = "arith2133";


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
		
		Debug.Log("DevelopmentBuild");
		
		//if ( BuildiOS(false)==false ) EditorApplication.Exit(1);
		
		if ( BuildAndroid(false)==false ) EditorApplication.Exit(1);
		
		EditorApplication.Exit(0);
	}
	
	// iOSビルド
	private static bool BuildiOS(bool release){
		
		Debug.Log("Start Build( iOS )");

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
			Debug.Log("Build( iOS ) Success.");
			return true;
			
		}
		
		Debug.Log("Build( iOS ) ERROR!");
		Debug.LogError(errorMsg);
		
		return false;
	}
	
	// Androidビルド
	private static bool BuildAndroid(bool release){
		
		Debug.Log("Start Build( Android )");
		
		BuildOptions opt = BuildOptions.None;
		
		// 開発用ビルドの場合のオプション設定
		/*if ( release==false ){
			opt |= BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging;
		}*/
		
		// keystoreファイルのの場所を設定
		string keystoreName =
			System.IO.Directory.GetCurrentDirectory()+"/"+ keystorePath;
		
		// set keystoreName
		PlayerSettings.Android.keystoreName = keystoreName;
		
		// パスワードの再設定
		PlayerSettings.Android.keystorePass = keystorePass;
		
		// パスワードの再設定
		PlayerSettings.Android.keyaliasPass = keyaliasPass;

		// ビルド
		// シーン、出力ファイル（フォルダ）、ターゲット、オプションを指定
		string errorMsg = BuildPipeline.BuildPlayer(GetScenes(), @"C:\Jenkins\workspace\TestBatchBuild\aaaaa.apk", BuildTarget.Android, BuildOptions.None);
		
		// errorMsgがない場合は成功
		if ( string.IsNullOrEmpty(errorMsg) ){
			Debug.Log("Build( Android ) Success.");
			return true;
		}
		
		Debug.Log("Build( Android ) ERROR!");
		Debug.LogError(errorMsg);
		
		return false;
		
	}
}
#endif
