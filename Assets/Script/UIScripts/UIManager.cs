using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages the flow of the Main Menu Scene.
/// </summary>
public class UIManager : MonoBehaviour 
{
	#region Variables

	// The Screens
	public GameObject mainMenuCanvas;
	public GameObject levelPackSelectionCanvas;
	public GameObject levelSelectionCanvas;
	public GameObject optionsCanvas;
	public GameObject loadingCanvas;
	public GameObject aboutCanvas;
	public GameObject storeCanvas;

	#endregion

	#region Private Fields

	private GameObject currentScreen;

	#endregion

	#region Public Functions

	/// <summary>
	/// Starts the game by displaying the Select Level Pack screen.
	/// </summary>
	public void Play()
	{
		// Show the Level Pack Selection screen
		SetActiveLevelPackSelectionScreen();
	}

	/// <summary>
	/// Displays the Options screen.
	/// </summary>
	public void ShowOptions()
	{
		// Show the Options screen
		SetActiveOptionsScreen();
	}

	public void ShowLevelSelect(LevelPack selectedLevelPack)
	{
		// Fill the levels in the screen
		levelSelectionCanvas.GetComponent<LevelSelectionPresenter>().Init(selectedLevelPack);

		// Set the Level Selection screen as the active one
		SetActiveLevelSelectionScreen();
	}

	public void PlayLevel(int selectedLevel)
	{
		LevelManager.manager.currentLevel = selectedLevel;
		levelSelectionCanvas.SetActive (false);
		loadingCanvas.SetActive (true);
		Application.LoadLevel("BuilderLevel");
	}

	public void ShowStore()
	{
		// Show the Store screen
		SetActiveStoreScreen();
	}

	#endregion

	// Use this for initialization
	void Start () 
	{
		// Handle the onRequestBack event for each screen
		mainMenuCanvas.GetComponent<GameScreen>().onRequestBack.AddListener(OnScreenRequestBack);
		levelPackSelectionCanvas.GetComponent<GameScreen>().onRequestBack.AddListener(OnScreenRequestBack);
		levelSelectionCanvas.GetComponent<GameScreen>().onRequestBack.AddListener(OnScreenRequestBack);
		optionsCanvas.GetComponent<GameScreen>().onRequestBack.AddListener(OnScreenRequestBack);
		aboutCanvas.GetComponent<GameScreen>().onRequestBack.AddListener(OnScreenRequestBack);
		storeCanvas.GetComponent<GameScreen>().onRequestBack.AddListener(OnScreenRequestBack);

		// We start with the Main Menu at this point
		levelPackSelectionCanvas.SetActive(false);
		levelSelectionCanvas.SetActive(false);
		optionsCanvas.SetActive(false);
		currentScreen = mainMenuCanvas;
		mainMenuCanvas.SetActive(true);
		loadingCanvas.SetActive (false);
		aboutCanvas.SetActive(false);
		storeCanvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnScreenRequestBack(GameScreen sender)
	{
		// Deactivate the requesting screen
		sender.screenCanvas.SetActive(false);

		// Activate the linked screen
		currentScreen = sender.goBackTo.screenCanvas;
		sender.goBackTo.screenCanvas.SetActive(true);
	}

	private void SetActiveMainMenuScreen()
	{
		// Deactivate the current screen
		currentScreen.SetActive(false);

		// Activate the Main Menu screen
		currentScreen = mainMenuCanvas;
		mainMenuCanvas.SetActive(true);
	}

	private void SetActiveLevelPackSelectionScreen()
	{
		// Deactivate the current screen
		currentScreen.SetActive(false);
		
		// Activate the Main Menu screen
		currentScreen = levelPackSelectionCanvas;
		levelPackSelectionCanvas.SetActive(true);
	}

	private void SetActiveLevelSelectionScreen()
	{
		// Deactivate the current screen
		currentScreen.SetActive(false);
		
		// Activate the Main Menu screen
		currentScreen = levelSelectionCanvas;
		levelSelectionCanvas.SetActive(true);
	}

	private void SetActiveOptionsScreen()
	{
		// Deactivate the current screen
		currentScreen.SetActive(false);
		
		// Activate the Main Menu screen
		currentScreen = optionsCanvas;
		optionsCanvas.SetActive(true);
	}
	public void SetActiveAboutCanvas()
	{
		currentScreen.SetActive (false);
		currentScreen = aboutCanvas;
		aboutCanvas.SetActive (true);
	}

	public void SetActiveStoreScreen()
	{
		// Deactivate the current screen
		currentScreen.SetActive(false);

		// Activate the Store screen
		currentScreen = storeCanvas;
		storeCanvas.SetActive(true);
	}
}
