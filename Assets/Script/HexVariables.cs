using UnityEngine;
using System.Collections;

public class HexVariables : MonoBehaviour {

	public static int T = 0; //Blank hexagon;
	public static int W = 1; //Normal pathway;
	public static int S = 2; // Star prefab;
	public static int WC = 3; //White cat's starting position;
	public static int BC = 4; //Black cat's starting position;
	public static int WBM = 5; //blackcat's midway pathway;
	public static int WWM = 6; //whitecat's midway pathway;
	public static int WBE = 7; //blackcat's end path pathway;
	public static int WWE = 8; //whitecat's end path pathway;
	
	public static int TB1 = 9; // A button that permanently lowers a path
	public static int RB1 = 10; // A button that permanently raises a path

	public static int LB1 = 11; // A button that temporarily lowers a path
	public static int NB1 = 12; // A button that temporarily raises a path

	public static int SP1 = 13; // A path that belongs to button types that permanently raise or lowers, starts lowered.
	public static int TP1 = 14; // A path that belongs to button types that permanently raise or lowers, starts raised.

	public static int VP1 = 15; // A path that belongs to button types that temporarily raise or lowers, starts lowered.
	public static int BP1 = 16; // A path that belongs to button types that temporarily raise or lowers, starts raised.

	public static int PTO = 17; // portal number one
    public static int PTT = 18; // portal number two

    public static int WHT = 19; // winterHex
    public static int FHT = 20; // forestHex

	public static bool narratorNeedsToSpeek = true;
	public static bool narratorDoneSpeaking = false;
	public static bool narratorStatusIsMute = false;


}
