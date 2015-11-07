using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

public class CSVReader : MonoBehaviour {
	public static CSVReader CSVmanager;
	static string LINE_SPLIT = @"\r\n|\n\r|\n|\r";
	private Dictionary<string, int> list = new Dictionary<string, int>();


	void Awake () {
		SetValues();
		if(CSVmanager == null){
			DontDestroyOnLoad(gameObject);
			CSVmanager = this;
		} else if(CSVmanager != this){
			Destroy(gameObject);
		}
	}

	//Read - given a name of a cvs file (IN RESOURCES FOLDER!!!) returns an array of string of the form:
	/*BC, W, W, SP1 , TP1
	  W, W
	  W, W, TB1
	  W, W, W, LB1
	  W, W, W, W, RB1
	  S, W, S, W, W, WC, S
	 */
	public int[][] Read (string file) {
		TextAsset data = Resources.Load (file) as TextAsset;
		var lines = Regex.Split (data.text, LINE_SPLIT);
		string[][] readLines = new string[lines.Length][];

		for(var i=0; i < lines.Length; i++) {
			readLines[i] = SplitCsvLine(lines[i]);
		}

		int[][] convertedLines = new int[readLines.Length][];
		for(int i = 0; i < readLines.Length; i++){
			convertedLines[i] = new int[readLines[i].Length];
			for(int j = 0; j < readLines[i].Length; j++){
				convertedLines[i][j] = list[readLines[i][j]];
			}
		}
		return convertedLines;
	}
	
	private string[] SplitCsvLine(string line)
	{
		string pattern = @"
     # Match one value in valid CSV string.
     (?!\s*$)                                      # Don't match empty last value.
     \s*                                           # Strip whitespace before value.
     (?:                                           # Group for value alternatives.
       '(?<val>[^'\\]*(?:\\[\S\s][^'\\]*)*)'       # Either $1: Single quoted string,
     | ""(?<val>[^""\\]*(?:\\[\S\s][^""\\]*)*)""   # or $2: Double quoted string,
     | (?<val>[^,'""\s\\]*(?:\s+[^,'""\s\\]+)*)    # or $3: Non-comma, non-quote stuff.
     )                                             # End group of value alternatives.
     \s*                                           # Strip whitespace after value.
     (?:,|$)                                       # Field ends on comma or EOS.
     ";
		
		string[] values = (from Match m in Regex.Matches(line, pattern, 
		                                                 RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)
		                   select m.Groups[1].Value).ToArray();
		
		return values;        
	}

	public void SetValues()
	{
		list.Add( "T" , HexVariables.T);
		list.Add( "W" , HexVariables.W);
		list.Add( "S" , HexVariables.S); 
		list.Add( "WC" , HexVariables.WC);
		list.Add( "BC", HexVariables.BC);
		list.Add( "WBM" , HexVariables.WBM);
		list.Add( "WWM" , HexVariables.WWM);
		list.Add( "WBE" , HexVariables.WBE);
		list.Add( "WWE" , HexVariables.WWE);
		list.Add( "TB1" , HexVariables.TB1);
		list.Add( "RB1" , HexVariables.RB1); 
		list.Add( "LB1" , HexVariables.LB1); 
		list.Add( "NB1" , HexVariables.NB1); 
		list.Add( "SP1" , HexVariables.SP1); 
		list.Add( "TP1" , HexVariables.TP1); 
		list.Add( "VP1" , HexVariables.VP1); 
		list.Add( "BP1" , HexVariables.BP1); 
		list.Add( "PTO" , HexVariables.PTO);
		list.Add( "PTT" , HexVariables.PTT);
	}

}
