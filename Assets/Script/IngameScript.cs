using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IngameScript : MonoBehaviour {
	public GameObject pauseCanvas, openMenuButton, returnToMenu, inGameCanvas, 
	swapCatsButton, quickReset, winCanvas, mainCamera, loseCanvas, commingSoonCanvas, 
	narratorCanvas, winScreenNextLevelButton, loseScreenNextLevelButton, ingameMenuNextLevelButton, yinCharacter, 
    yanCharacter, movesLeft, winOrbOne, winOrbsTwo, winOrbThree, winScreenMinMoveStar, 
	ingameMenuSoundOn, ingameMenuSoundOff, narratorOn, narratorOff, soundButton, whiteHand, blackHand,
    narratorMovesLeft, narratorQuickReset, narratorCharacterSwap;

    public Sprite SoundOn, SoundOff;
	private bool activeStatus;
	public Text starsGatheredEndTextWinWindow, movesMadeEndText, narratorText, levelNumber;
	private const string k_NUM_OF_STARS_GATHERED = "ORBS: ";
	private const string k_NUM_OF_MOVES_MADE = "MOVES: ";

	// Use this for initialization
	void Start () {
		activateBackToGameButton ();
		activeStatus = false;
		pauseCanvas.SetActive (activeStatus);
		inGameCanvas.SetActive (true);
		winCanvas.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
        updateSoundSprite();
    }

    private void updateSoundSprite()
    {
        soundButton.gameObject.GetComponent<Image>().sprite = 
            LevelManager.manager.AllSoundsMuted ? SoundOff : SoundOn;
    }

    public void ActivateIngameSoundMuteButton()
    {
        LevelManager.manager.AllSoundsMuted = !LevelManager.manager.AllSoundsMuted;
        updateSoundSprite();
    }

	public void activateIngameMenu(bool winLevel){
		activeStatus = true;
		showNarratorStatus ();
		mainCamera.GetComponent<CameraMovement> ().enabled = false;
		movesLeft.SetActive (false);
		levelNumber.text = "Level " + LevelManager.manager.currentLevel;
		openMenuButton.SetActive (!activeStatus);
		quickReset.SetActive (!activeStatus);
        soundButton.SetActive(!activeStatus);

		if (LevelManager.manager.currentLevel > 3) {
			swapCatsButton.SetActive (!activeStatus);
		}
		pauseCanvas.SetActive (activeStatus);
		if (LevelManager.manager.isStagedUnlocked (LevelManager.manager.currentLevel + 1)) {
			ingameMenuNextLevelButton.SetActive (true);
		} else {
			ingameMenuNextLevelButton.SetActive (false);
		}
	}

	public void deactivateIngameMenu(){
		activeStatus = false;
		movesLeft.SetActive (true);
		mainCamera.GetComponent<CameraMovement> ().enabled = true;
		openMenuButton.SetActive (!activeStatus);
		quickReset.SetActive (!activeStatus);
        soundButton.SetActive(!activeStatus);

		if (LevelManager.manager.currentLevel > 3) {
			swapCatsButton.SetActive (!activeStatus);
		}
		pauseCanvas.SetActive (activeStatus);
	}

	public void activateBackToGameButton(){
		returnToMenu.SetActive (true);
	}

	public void deactivateBackToGameButton(){
		returnToMenu.SetActive (false);
	}

	public void shutDownIngameCanvas(){
		inGameCanvas.SetActive (false);
	}

	public void activateWinScreen(){
		mainCamera.GetComponent<CameraMovement> ().enabled = false;
		starsGatheredEndTextWinWindow.text = k_NUM_OF_STARS_GATHERED;
		if(GetComponent<stepstarcounter>().doneInMinMoves()){
			winScreenMinMoveStar.SetActive(true);
		} else {
			winScreenMinMoveStar.SetActive(false);
		}
		if (LevelManager.manager.currentLevel == 1 ||
		    LevelManager.manager.currentLevel == 2 ||
		    LevelManager.manager.currentLevel == 16) {
			winOrbOne.SetActive (true);
			winOrbsTwo.SetActive (true);
			winOrbThree.SetActive (true);
		} else {
			switch (GetComponent<stepstarcounter> ().getStars ()) {
			case 1:
				winOrbOne.SetActive (true);
				winOrbsTwo.SetActive (false);
				winOrbThree.SetActive (false);
				break;
			case 2:
				winOrbOne.SetActive (true);
				winOrbsTwo.SetActive (true);
				winOrbThree.SetActive (false);
				break;
			case 3:
				winOrbOne.SetActive (true);
				winOrbsTwo.SetActive (true);
				winOrbThree.SetActive (true);
				break;
			default:
				winOrbOne.SetActive (false);
				winOrbsTwo.SetActive (false);
				winOrbThree.SetActive (false);
				break;
			}
		}
		movesMadeEndText.text = k_NUM_OF_MOVES_MADE + GetComponent<stepstarcounter> ().getSteps ();
		winCanvas.SetActive (true);
		if (LevelManager.manager.isStagedUnlocked (LevelManager.manager.currentLevel + 1)) {
			ingameMenuNextLevelButton.SetActive (true);
		} else {
			winScreenNextLevelButton.SetActive (false);
		}
	}
	public void activateLoseScreen(){
		mainCamera.GetComponent<CameraMovement> ().enabled = false;
		//starsGathereredEndTextLoseWindow.text = k_NUM_OF_STARS_GATHERED + GetComponent<stepstarcounter>().getStars();
		loseCanvas.SetActive (true);
		if (LevelManager.manager.currentLevel != 20 && 
		    LevelManager.manager.isStagedUnlocked (LevelManager.manager.currentLevel + 1)) {
			ingameMenuNextLevelButton.SetActive (true);
		} else {
			loseScreenNextLevelButton.SetActive (false);
		}
	}	
	public void activateCommingSoonScreen(){
		mainCamera.GetComponent<CameraMovement> ().enabled = false;
		commingSoonCanvas.SetActive (true);
	}
	public void activateNarratorCanvas(string narratorText){
		mainCamera.GetComponent<CameraMovement> ().enabled = false;
		this.narratorText.text = narratorText;
		narratorCanvas.SetActive (true);

	    switch (LevelManager.manager.currentLevel)
	    {
            case 2:
                narratorMovesLeft.SetActive(true);
                narratorQuickReset.SetActive(false);
                narratorCharacterSwap.SetActive(false);
	            break;
            case 4:
                narratorCharacterSwap.SetActive(true);
                narratorQuickReset.SetActive(false);
                narratorMovesLeft.SetActive(false);
                break;
            case 6:
                narratorQuickReset.SetActive(true);
                narratorMovesLeft.SetActive(false);
                narratorCharacterSwap.SetActive(false);
                break;
            default:
                narratorMovesLeft.SetActive(false);
                narratorQuickReset.SetActive(false);
                narratorCharacterSwap.SetActive(false);
                break;
	    }
	}
	public void deactivateNarratorCanvas(){
		inGameCanvas.SetActive (true);
		mainCamera.GetComponent<CameraMovement> ().enabled = true;
		narratorCanvas.SetActive (false);
		HexVariables.narratorDoneSpeaking = true;
	}
	public void deactivateSwitchCharacter(){
		swapCatsButton.SetActive (false);
	}
	public void activateNarratingCharacter(float[] handPositions){
        Vector3 whiteHandPosition = new Vector3(handPositions[0], handPositions[1], 0);
        Vector3 blackHandPositon = new Vector3(handPositions[2], handPositions[3], 0);
        Quaternion whiteHandRotationQuaternion = whiteHand.transform.rotation * Quaternion.Euler(0, 0, handPositions[4]);
        Quaternion blackHandRotationQuaternion = blackHand.transform.rotation * Quaternion.Euler(0, 0, handPositions[5]);

        blackHand.GetComponent<RectTransform>().localPosition = blackHandPositon;
	    whiteHand.GetComponent<RectTransform>().localPosition = whiteHandPosition;
        blackHand.GetComponent<RectTransform>().localRotation = blackHandRotationQuaternion;
        whiteHand.GetComponent<RectTransform>().localRotation = whiteHandRotationQuaternion;
	}

    public void changeNarratorStatus(){
		HexVariables.narratorStatusIsMute = !HexVariables.narratorStatusIsMute;
		showNarratorStatus ();
	}
	public void showNarratorStatus(){
		if (HexVariables.narratorStatusIsMute) 
		{
			narratorOn.SetActive(false);
			narratorOff.SetActive(true);
		} 
		else 
		{
			narratorOn.SetActive(true);
			narratorOff.SetActive(false);
		}
	}
}
