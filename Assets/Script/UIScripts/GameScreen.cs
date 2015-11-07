using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a UI screen in the game (like the Level Pack Selection screen).
/// </summary>
public class GameScreen : MonoBehaviour 
{
	#region Variables

	public GameObject screenCanvas;

	/// <summary>
	/// The previous game screen.
	/// </summary>
	public GameScreen goBackTo = null;

	#endregion

	#region Events

	public ScreenRequestBackEvent onRequestBack = new ScreenRequestBackEvent();

	#endregion

	#region Public Functions

	/// <summary>
	/// Raised when a Back button on the screen was pressed.
	/// </summary>
	public void RequestBack()
	{
		if (onRequestBack != null) 
		{
			onRequestBack.Invoke(this);
		}
	}

	#endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
