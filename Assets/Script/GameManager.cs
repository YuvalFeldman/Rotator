using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    bool blackCatIsChosen = true;
    bool whiteCatIsChosen = false;
    private GameObject whiteCat, blackCat;
    private HexChart hexChartScript;
    private bool running = true;
    private bool firstTimeChosen = true;
    private string[] directions;
    private GameObject[] colored = new GameObject[6];

    // Use this for initialization
    void Start()
    {
        hexChartScript = GameObject.Find("HexGrid").GetComponent<HexChart>();
        directions = new string[6] { "topLeft", "topRight", "left", "right", "bottomLeft", "bottomRight" };
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (running && LevelManager.manager.currentLevel <= 20)
        {
            running = false;
            Initialize();
        }
    }

    public void Initialize()
    {
        if (blackCatIsChosen)
        {
            chooseBlackCat();
            highlightMovementOptions(blackCat);
        }
        else
        {
            chooseWhiteCat();
            highlightMovementOptions(whiteCat);
        }
    }

    // Set which cat is now the moving cat
    private void chooseWhiteCat()
    {
        blackCatIsChosen = false;
        whiteCatIsChosen = true;
        whiteCat.GetComponent<Movement>().set_Chosen(true);
        blackCat.GetComponent<Movement>().set_Chosen(false);
    }
    private void chooseBlackCat()
    {
        firstTimeChosen = false;
        blackCatIsChosen = true;
        whiteCatIsChosen = false;
        blackCat.GetComponent<Movement>().set_Chosen(true);
        whiteCat.GetComponent<Movement>().set_Chosen(false);
    }

    // Connect the Gamemanager to the cats - initialized at the start of the level when the cats are created
    public void set_blackCat(GameObject cat)
    {
        blackCat = cat;
    }
    public void set_whiteCat(GameObject cat)
    {
        whiteCat = cat;
    }

    //For example if Thomas (the black cat) is chosen (he's my favorite btw), choose Jerry (the white cat).
    public void chooseOtherCat()
    {
        if (blackCatIsChosen)
        {
            //SoundManager.soundManager.playWhiteChosen();
            whiteCat.GetComponent<Movement>().playChooseAnimation();
            switchToWhiteCat();
        }
        else
        {
            //SoundManager.soundManager.playBlackChosen();
            blackCat.GetComponent<Movement>().playChooseAnimation();
            switchToBlackCat();
        }
    }

    public void nextTurn()
    {
        if (blackCatIsChosen)
        {
            switchToBlackCat();
        }
        else
        {
            switchToWhiteCat();
        }
    }

    public void highlightMovementOptions(GameObject currentCat)
    {
        resetBoardMoves();
        //TODO Bug when trying to color hex with another cat in it
        Vector2[] possibleMoves = currentCat.GetComponent<Movement>().possibleMoves;
        int x = currentCat.GetComponent<Movement>().get_XPositionInChart();
        int y = currentCat.GetComponent<Movement>().get_YPositionInChart();
        for (int i = 0; i < 6; i++)
        {
            possibleMoves[i] = currentCat.GetComponent<Movement>().CheckPath(directions[i], x, y, 0);
        }
    }

    public void resetBoardMoves()
    {
        hexChartScript.resetTempBlockSprite();
        hexChartScript.resetMovementGrid();
        hexChartScript.resetHexTags();
    }

    private void switchToBlackCat()
    {
        chooseBlackCat();
        highlightMovementOptions(blackCat);
    }
    private void switchToWhiteCat()
    {
        chooseWhiteCat();
        highlightMovementOptions(whiteCat);
    }
}
