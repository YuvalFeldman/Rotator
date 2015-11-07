using UnityEngine;
using System.Collections;

public class NarratorText : MonoBehaviour
{
    private int currentlyShowingTextNum = 0;
    private int totalSentances = 0;
    string[] introNarration = new string[1];
    string[] orbsNarration = new string[1];
    string[] characterSwapNarration = new string[1];
    string[] permaButtonNarration = new string[1];
    string[] tempButtonNarration = new string[1];
    string[] portalNarration = new string[1];
    float[] HandPlacement = new float[6];

    public string[] getNarration()
    {
        switch (LevelManager.manager.currentLevel)
        {
            case 1:
                introNarration[0] = "Press a destination tile to move to it, try and help them connect together!";
                totalSentances = introNarration.Length;
                return introNarration;
            case 2:
                orbsNarration[0] = "Make sure you beat the puzzle before you run out of moves!";
                totalSentances = orbsNarration.Length;
                return orbsNarration;
            case 3:
                characterSwapNarration[0] = "Collect power orbs to unlock more levels.";
                totalSentances = characterSwapNarration.Length;
                return characterSwapNarration;
            case 4:
                characterSwapNarration[0] = "Use this button to swap between characters, try it out!";
                totalSentances = characterSwapNarration.Length;
                return characterSwapNarration;
            case 5:
                permaButtonNarration[0] = "Standing on purple hex buttons will permanently change purple hexes!";
                totalSentances = permaButtonNarration.Length;
                return permaButtonNarration;
            case 6:
                characterSwapNarration[0] = "You can restart the level at any time.";
                totalSentances = characterSwapNarration.Length;
                return characterSwapNarration;

            case 7:
                tempButtonNarration[0] = "Standing on orange hex buttons will permanently change orange hexes!";
                totalSentances = tempButtonNarration.Length;
                return tempButtonNarration;
            case 16:
                portalNarration[0] = "Use teleportation hexes to instantly jump between far away hexes!";
                totalSentances = portalNarration.Length;
                return portalNarration;

        }
        return null;
    }

    public float[] getCharacter()
    {
        switch (LevelManager.manager.currentLevel)
        {
            case 1:
                HandPlacement[0] = -62;
                HandPlacement[1] = 72;
                HandPlacement[2] = 21;
                HandPlacement[3] = 72;
                HandPlacement[4] = 235;
                HandPlacement[5] = 56;
                return HandPlacement;
            case 2:
                HandPlacement[0] = 170;
                HandPlacement[1] = 70;
                HandPlacement[2] = 125;
                HandPlacement[3] = 70;
                HandPlacement[4] = 0;
                HandPlacement[5] = 0;
                return HandPlacement;
            case 3:
                HandPlacement[0] = -40;
                HandPlacement[1] = 40;
                HandPlacement[2] = 85;
                HandPlacement[3] = -38;
                HandPlacement[4] = 85;
                HandPlacement[5] = 1;
                return HandPlacement;
            case 4:
                HandPlacement[0] = 130;
                HandPlacement[1] = -20;
                HandPlacement[2] = 103;
                HandPlacement[3] = -61;
                HandPlacement[4] = 180;
                HandPlacement[5] = 230;
                return HandPlacement;
            case 5:
                HandPlacement[0] = -100;
                HandPlacement[1] = -55;
                HandPlacement[2] = 17;
                HandPlacement[3] = 37;
                HandPlacement[4] = 100;
                HandPlacement[5] = 1;
                return HandPlacement;
            case 6:
                HandPlacement[0] = -120;
                HandPlacement[1] = 78;
                HandPlacement[2] = -117;
                HandPlacement[3] = 78;
                HandPlacement[4] = 1;
                HandPlacement[5] = 1;
                return HandPlacement;

            case 7:
                HandPlacement[0] = -88;
                HandPlacement[1] = -45;
                HandPlacement[2] = 20;
                HandPlacement[3] = 25;
                HandPlacement[4] = 100;
                HandPlacement[5] = 1;
                return HandPlacement;
            case 16:
                HandPlacement[0] = 17;
                HandPlacement[1] = -67;
                HandPlacement[2] = 1;
                HandPlacement[3] = 42;
                HandPlacement[4] = 182;
                HandPlacement[5] = 1;
                return HandPlacement;

            default:
                HandPlacement[0] = 1000;
                HandPlacement[1] = 1000;
                HandPlacement[2] = 1000;
                HandPlacement[3] = 1000;
                HandPlacement[4] = 1;
                HandPlacement[5] = 1;
                return HandPlacement;
        }
    }
    public int getCurrentlyShowingText()
    {
        return currentlyShowingTextNum;
    }
    public void incrementCurrentlyShowingText()
    {
        currentlyShowingTextNum++;
    }
    public void resetCurrentlyShowingText()
    {
        currentlyShowingTextNum = 0;
    }
    public bool lastSentance()
    {
        return currentlyShowingTextNum == totalSentances - 1;
    }
}
