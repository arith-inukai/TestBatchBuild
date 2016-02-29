#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections;

public class FoxPluginAndroid {
	private const string playerClass	= "com.unity3d.player.UnityPlayer";
	private const string adClass		= "jp.appAdForce.android.unity.AdManagerUnity";
	private const string ltvClass		= "jp.appAdForce.android.unity.LtvManagerUnity";
	private const string analyticsClass = "jp.appAdForce.android.unity.AnalyticsManagerUnity";
	private const string notifyClass 	= "jp.appAdForce.android.unity.NotifyManagerUnity";
	private const string gcmClass		= "com.google.android.gcm.unity.GCMRegistrarUnity";
	
	private static AndroidJavaObject currentActivity = new AndroidJavaClass(playerClass).GetStatic<AndroidJavaObject>("currentActivity");
	private static AndroidJavaClass analytics = new AndroidJavaClass(analyticsClass);
	private static AndroidJavaObject ad = new AndroidJavaObject(adClass, currentActivity);
	private static Hashtable p = new Hashtable();

	public static void sendConversion(){
		ad.Call("sendConversionUnity");
	}
	
	public static void sendConversion(string url){
		ad.Call("sendConversionUnity", url);
	}
	
	public static void sendConversionOnconsent(string url){
		ad.Call("sendConversionUnity", url);
	}
		
	public static void sendConversion(string url, string buid){
		ad.Call("sendConversionUnity", url, buid);
	}
	
	public static void sendConversionForMobage(string mbgaAppId){
		ad.Call("sendConversionForMobageUnity", mbgaAppId);
	}
	
	public static void sendConversionForMobage(string url, string mbgaAppId){
		ad.Call("sendConversionForMobageUnity", url, mbgaAppId);
	}
	
	public static void sendUserIdForMobage(string mbgaUserId){
		ad.Call("sendUserIdForMobageUnity", mbgaUserId);
	}
	
	public static void sendConversionWithCAReward(string url, string carAppKey){
		ad.Call("sendConversionWithCARewardUnity", url);
	}
	
	public static void sendConversionWithCAReward(string url, string buid, string carAppKey){
		ad.Call("sendConversionWithCARewardUnity", url, buid);
	}
	
	public static void sendConversionForMobageWithCAReward(string mbgaAppId, string carAppKey){
		ad.Call("sendConversionForMobageWithCARewardUnity", mbgaAppId);
	}
	
	public static void sendConversionForMobageWithCAReward(string url, string mbgaAppId, string carAppKey){
		ad.Call("sendConversionForMobageWithCARewardUnity", url, mbgaAppId);
	}
	
	public static void setServerUrl(string serverUrl){
		ad.Call("setServerUrl", serverUrl);
	}
	
	public static void setMode(string mode){
		Debug.LogWarning("this method iOS only");
	}

	public static void setMessage(String title, string msg){
		Debug.LogWarning("this method iOS only");
	}
	
	public static void setOptout(bool optout){
		ad.Call("setOptout", optout);
	}
	
	public static void updateFrom2_10_4g(){
		ad.CallStatic("updateFrom2_10_4g");
	}
	
	public static void sendLtv(int cvId){
		_sendLtv(cvId, null);
	}	
	
	public static void sendLtv(int cvId, string adId){
		_sendLtv(cvId, adId);
	}
	
	private static void _sendLtv(int cvId, string adId){
		AndroidJavaObject ltv = new AndroidJavaObject(ltvClass, currentActivity, ad);
		foreach (DictionaryEntry de in p) {
			ltv.Call("addParam", de.Key, de.Value);
		}
		if (adId == null) {
			ltv.Call("sendLtvConversionUnity", cvId);
		} else {
			ltv.Call("sendLtvConversionUnity", cvId, adId);
		}
		ltv.Dispose();
		p = new Hashtable();
	}

	public static void addParameter(string name, string val){
		p[name] = val;
	}

	public static void ltvOpenBrowser(string url){
		AndroidJavaObject ltv = new AndroidJavaObject(ltvClass, currentActivity, ad);
		ltv.Call("ltvOpenBrowserUnity", url);
		ltv.Dispose();
	}

	public static void sendStartSession(){
		analytics.CallStatic("sendStartSession", currentActivity);
	}
	
	public static void sendEndSession(){}
	
	public static void sendEvent(string eventName,
			string action, string label, int value) {
		analytics.CallStatic("sendEvent", currentActivity, eventName, action, label, value);
	}

	public static void sendEventPurchase(string eventName,
			string action, string label, string orderId, string sku,
			string itemName, double price, int quantity, string currency) {
		analytics.CallStatic("sendEvent", currentActivity, eventName, action, label,
				orderId, sku, itemName, price, quantity, currency);
	}

	public static void sendUserInfo(string userId,
			string userAttr1, string userAttr2, string userAttr3,
			string userAttr4, string userAttr5) {
		analytics.CallStatic("sendUserInfo", currentActivity, userId, userAttr1,
				userAttr2, userAttr3, userAttr4, userAttr5);
	}
		
	public static void registerToGCM(string senderId) {		
		AndroidJavaObject notify = new AndroidJavaObject(notifyClass, currentActivity, ad);
		notify.Call("registerToGCMUnity",currentActivity, senderId);
	}
}
#endif
