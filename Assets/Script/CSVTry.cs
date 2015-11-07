using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class CSVTry : MonoBehaviour {

	static string LINE_SPLIT = @"\r\n|\n\r|\n|\r";

	//Read - given a name of a cvs file returns an array of string of the form:
	/*BC, W, W, SP1 , TP1
	  W, W
	  W, W, TB1
	  W, W, W, LB1
	  W, W, W, W, RB1
	  S, W, S, W, W, WC, S
	 */
	public string[][] Read (string file) {
		TextAsset data = Resources.Load (file) as TextAsset;
		var lines = Regex.Split (data.text, LINE_SPLIT);
		string[][] readLines = new string[lines.Length][];

		for(var i=0; i < lines.Length; i++) {
			readLines[i] = SplitCsvLine(lines[i]);
		}
		return readLines;
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

}
