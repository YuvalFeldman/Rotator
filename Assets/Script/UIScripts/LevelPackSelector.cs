using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelPackSelector : MonoBehaviour 
{
	#region Editor Fields

	public UIManager manager;
	
	/// <summary>
	/// The panel that contains all level packs to display.
	/// </summary>
	public Transform levelPackPanel;
	
	/// <summary>
	/// Represents the selector of a level pack in the UI.
	/// </summary>
	public GameObject levelPackPrefab;

	public List<LevelPack> levelPackList;
	
	#endregion

	#region Properties

	public LevelPack SelectedLevelPack { get; set; }

	#endregion
	
	// Use this for initialization
	void Start () 
	{
		// Initialize the level pack list
		levelPackList = LoadLevelPacks();
		
		// Fill the selector in the UI
		FillLevelPacksSelector();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void FillLevelPacksSelector()
	{
		foreach (var levelPack in levelPackList) 
		{
			GameObject levelPackObject = Instantiate(levelPackPrefab);
			LevelPackPresenter presenter = levelPackObject.GetComponent<LevelPackPresenter>();
			presenter.Init(levelPack);
			presenter.onSelected.AddListener(OnLevelPackSelected);
			levelPackObject.transform.SetParent(levelPackPanel);
			levelPackObject.transform.localScale = new Vector3(1,1,1);
		}
	}

	private void OnLevelPackSelected(LevelPack selectedLevelPack)
	{
		manager.ShowLevelSelect(selectedLevelPack);
	}

	static List<LevelPack> LoadLevelPacks()
	{
				// In a real application, the data would come from an external source
		// but for now lets just use this
		var levelPackList = new List<LevelPack>(3);

		// First
		var levelPack = new LevelPack(1, "Buttons");
		var levels = new List<int>(10);

		for (int i = 1; i < 11; i++) 
		{
			levels.Add(i);
		}
		levelPack.Levels = levels;
		levelPackList.Add(levelPack);

		// Second
		levelPack = new LevelPack(2, "Portals");
		levels = new List<int>(10);

		for (int i = 11; i < 21; i++) 
		{
			levels.Add(i);
		}
		levelPack.Levels = levels;
		levelPackList.Add(levelPack);

		// Third
		levelPack = new LevelPack(3, "Coming Soon");
		levels = new List<int>();
		levelPack.Levels = levels;
		levelPackList.Add(levelPack);

		return levelPackList;
	}
}
