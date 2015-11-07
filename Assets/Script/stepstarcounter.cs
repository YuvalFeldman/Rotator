using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class stepstarcounter : MonoBehaviour {

    private int[] amountOfMovesAvailablePerLevel = { 0, 4, 10, 6, 23, 10, 16, 9, 13, 20, 10, 16, 24, 25, 30, 25, 1, 6, 26, 16, 14 };
    private int[] minAmountOfMovesAvailablePerLevel = { 0, 2, 8, 4, 6, 4, 7, 4, 6, 15, 5, 12, 20, 12, 18, 23, 1, 4, 20, 12, 12 };
    private int[] amountOfStarsAvailablePerLevel = { 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3 };
    private int currentLevel;
	private int movesMade = 0;
	public static int starsCollected = 0;
	private const string MovesTextconst = "Moves left: ";
	public Text movesLeftText;

	public int getStars(){
		return starsCollected;
	}
	public void setStars(int numberOfStars){
		starsCollected = numberOfStars;
	}
	public static void incrementStars(){
		starsCollected++;
	}
	public void resetStars(){
		starsCollected = 0;
	}
	public bool allStarsCollected(){
		return starsCollected == amountOfStarsAvailablePerLevel [LevelManager.manager.currentLevel];
	}
	public int getSteps(){
		return movesMade;
	}
	public void setSteps(int movesMade){
		this.movesMade = movesMade;
	}
	public void incrementSteps(){
		movesMade++;
	}
	public void resetSteps(){
		movesMade = 0;
	}
	public bool noMoreMoves(){
		return movesLeft() <= 0;
	}
	public int movesLeft(){
		return amountOfMovesAvailablePerLevel[LevelManager.manager.currentLevel] - movesMade;
	}
	public void updateMovesLeftText(){
		movesLeftText.text = MovesTextconst + movesLeft();
	}
	public bool doneInMinMoves(){
		return movesMade <= minAmountOfMovesAvailablePerLevel [LevelManager.manager.currentLevel];
	}
}
