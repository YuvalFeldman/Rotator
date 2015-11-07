using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : MonoBehaviour {

    private Text appPath;
	// Use this for initialization
	void Start () {
        appPath = this.GetComponentInParent<Text>();
        appPath.text = Application.persistentDataPath;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
