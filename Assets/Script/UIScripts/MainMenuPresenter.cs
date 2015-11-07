using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Analytics;

/// <summary>
/// Manages the presentation of the Main Menu.
/// </summary>
public class MainMenuPresenter : MonoBehaviour 
{
	#region Variables

	public UIManager manager = null;

	#endregion

	#region Public Functions

	public void Play()
	{
		manager.Play();
	}

	public void ShowOptions()
	{
		manager.ShowOptions();
	}

	public void ShowStore()
	{
        Analytics.CustomEvent("Shop Opened", null);
		manager.ShowStore();
	}

	#endregion

	// Use this for initialization
	void Start () 
	{
		if (manager == null) 
			throw new NullReferenceException("manager is null");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
