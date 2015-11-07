using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour {
	public int xChartPos, yChartPos, hexType;
	public bool isPath = false;

	public void setCordsHex (int xPos, int yPos){
		xChartPos = xPos;
		yChartPos = yPos;
	}
	public void set_isPath(bool status){
		isPath = status;
	}
	public bool get_isPath(){
		return isPath;
	}
	public void set_hexType(int hexType){
		this.hexType = hexType;
	}
	public int get_hexType(){
		return hexType;
	}
}
