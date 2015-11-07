using UnityEngine;
using System.Collections;

public class AndroidButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			#if UNITY_ANDROID

			// Get the unity player activity
			AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

			activity.Call<bool>("moveTaskToBack", true);
			#endif
		}
	}
}
