using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class Movement : MonoBehaviour {

	Vector2 positionInTheWorld, touchStartingPosition, touchMovement, touchRaycastPosition;
	private string catType;
	private int CharacterXPosition, CharacterYPosition, checkedOutButtonType;
	private bool chosen, catsHaveMet, touchMoved, currentlyMoving, wonLevel, notSavedYet, justMoved, playingWinSound, winConditionSatisfied;
	public GameObject Character, ScoreTracker;
	private GameObject Grid, theOtherCat, tempStar;
	private HexChart hexChartScript;
	RaycastHit2D hit;
	public Vector2[] possibleMoves = new Vector2[6];
	private float timeLeft = 3;
	private const float k_iTweenTime = 0.8f;
    private float m_Timer;
    private float m_TimeElapsed = 0;


	#region Animation Variables
	public Animator Animator;
	private float blinkBreakTime = 4;
	private float animationTimeLeft;
	public bool collectingStar = false;

    /// <summary>
    /// Sets the character to face left
    /// </summary>
    private Quaternion m_LookLeft;

    /// <summary>
    /// Sets the character to face right
    /// </summary>
    private Quaternion m_LookRight;
	#endregion

	// Use this for initialization
	void Start () {
        m_Timer = 0;
        m_LookRight = transform.rotation;
        m_LookLeft = m_LookRight * Quaternion.Euler(0, 180, 0);
		animationTimeLeft =  Random.Range(0F, 5F);
		Grid = GameObject.Find("HexGrid");
		hexChartScript = Grid.GetComponent<HexChart> ();
		currentlyMoving = false;
		iTween.Defaults.easeType = iTween.EaseType.easeOutBounce;
		catsHaveMet = false;
		touchMoved = false;
		chosen = false;
		wonLevel = false;
		notSavedYet = true;
		justMoved = false;
		playingWinSound = false;
		winConditionSatisfied = false;
		Grid.GetComponent<stepstarcounter>().updateMovesLeftText();
	}
	
	// Update is called once per frame
	void Update () {
        m_Timer += Time.deltaTime;

		if (HexVariables.narratorNeedsToSpeek) {
			HexVariables.narratorNeedsToSpeek = false;
			if(!HexVariables.narratorStatusIsMute){
			HexVariables.narratorDoneSpeaking = false;
			Grid.GetComponent<HexChart>().narrator();
			}
		}
		if (!wonLevel && HexVariables.narratorDoneSpeaking) {
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0) {
				timeLeft = 3;
			}

			#region Animation Checks

			blinkBreakTime =  Random.Range(3F, 7F);

			//Animation updates
			animationTimeLeft -= Time.deltaTime;

			if(animationTimeLeft < 0) {
				animationTimeLeft = blinkBreakTime;
				Animator.SetBool("Blink", true);
			}
			if (Animator.GetCurrentAnimatorStateInfo (0).IsName ("Blink")) {
				animationTimeLeft = blinkBreakTime;
				Animator.SetBool("Blink", false);
				Animator.SetBool("NotMoveStart", true);
			}
			if (Animator.GetCurrentAnimatorStateInfo (0).IsName ("MoveStart")) {
				Animator.SetBool("NotMoveStart", false);
				animationTimeLeft = blinkBreakTime;
				Animator.SetBool("Blink", false);
			}
			if (Animator.GetCurrentAnimatorStateInfo (0).IsName ("Chosen")) {
				animationTimeLeft = blinkBreakTime;
				Animator.SetBool("Blink", false);
				Animator.SetBool("NotMoveStart", true);
				Animator.SetBool("Chosen", false);
			}

			#endregion


			//Checks whether the damn cat reached it's Final Destination
			if (get_XPositionInWorld () == positionInTheWorld.x && get_YPositionInWorld () == positionInTheWorld.y) {
				if(catsHaveMet){
					winConditionSatisfied = true;
					if(notSavedYet){
                        Analytics.CustomEvent("Level " + LevelManager.manager.currentLevel + " Finished Successfully", 
                        new Dictionary<string, object>
                        {
                            { "Star Amount", Grid.GetComponent<stepstarcounter>().getStars() },
                            { "Steps Taken", Grid.GetComponent<stepstarcounter>().getSteps() },
                            { "Timer", m_TimeElapsed }
                        });
						LevelManager.manager.Save(LevelManager.manager.currentLevel, 
						                          Grid.GetComponent<stepstarcounter>().getStars(), 
						                          Grid.GetComponent<stepstarcounter>().getSteps());
						notSavedYet = false;
					}
					resetPaths();
					if(!playingWinSound){
                        SoundManager.soundManager.playConnectingCharactersSound();
						playingWinSound = true;
					}
					if (Animator.GetCurrentAnimatorStateInfo (0).IsName ("Finished")) {
						this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
						wonLevel = true;
						winOpenMenu();
					}
				} else if(Grid.GetComponent<stepstarcounter>().noMoreMoves() && 
				          get_XPositionInChart() != theOtherCat.GetComponent<Movement>().get_XPositionInChart() &&
				          get_YPositionInChart() != theOtherCat.GetComponent<Movement>().get_YPositionInChart()){
					resetPaths();
					wonLevel = true;
                    m_TimeElapsed = m_Timer;
                    Analytics.CustomEvent("Level " + LevelManager.manager.currentLevel + " Fail - Moves Ran Out",
                        new Dictionary<string, object>
                        {
                            { "Star Amount", Grid.GetComponent<stepstarcounter>().getStars() },
                            { "Steps Taken", Grid.GetComponent<stepstarcounter>().getSteps() },
                            { "Timer", m_TimeElapsed }
                        });
					loseOpenMenu();
				}

				int standingOnHex = hexChartScript.TheHexChart [get_XPositionInChart ()] [get_YPositionInChart ()].GetComponent<Hex> ().get_hexType ();
				if (justMoved && 
				    (standingOnHex == HexVariables.TB1 || 
				    standingOnHex == HexVariables.RB1 || 
				    standingOnHex == HexVariables.NB1 || 
				    standingOnHex == HexVariables.LB1)) {
					hexChartScript.activate_Button (standingOnHex);
					justMoved = false;
				}
				currentlyMoving = false;
			}

			if (!currentlyMoving && chosen) {

				//mouse controlled movement for debuging in computer
				#if UNITY_EDITOR
				if (Input.GetMouseButtonDown (0)) {

					Vector2 pos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
					RaycastHit2D hitInfo = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (pos), Vector2.zero);
					// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
					if (hitInfo) {
						if (hitInfo.transform.gameObject.tag == "endPoint") {
							MoveToPoint (hitInfo.transform.gameObject.GetComponent<Hex> ().xChartPos, hitInfo.transform.gameObject.GetComponent<Hex> ().yChartPos);
						} else if (hitInfo.transform.gameObject.tag == "PTOFirst") {
							StartCoroutine (portalMoveThroughPTO (hitInfo.transform.gameObject.GetComponent<Hex> ().xChartPos, hitInfo.transform.gameObject.GetComponent<Hex> ().yChartPos));
						} else if (hitInfo.transform.gameObject.tag == "PTTFirst") {
							StartCoroutine (portalMoveThroughPTT (hitInfo.transform.gameObject.GetComponent<Hex> ().xChartPos, hitInfo.transform.gameObject.GetComponent<Hex> ().yChartPos));
						}
						timeLeft = 3;
					}
				}

				#endif

				//touch controlled movement
				if (Input.touchCount > 0) {
					Touch touch = Input.touches [0];
					checkedOutButtonType = -1;
					touchMoved = false;
					touchRaycastPosition = new Vector2 (touch.position.x, touch.position.y);
					hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (touchRaycastPosition), Vector2.zero);
					checkedOutButtonType = hit.transform.gameObject.GetComponent<Hex> ().get_hexType ();

					switch (touch.phase) {

					//Case in which the device has received any touch input but has not necasserily ended.
					case TouchPhase.Began:
						touchStartingPosition = touch.position;
						break;

					//Case in which the device has received any touch and the user has moved his finger.
					case TouchPhase.Moved:
						touchMoved = true;
						touchMovement = touch.position - touchStartingPosition;
						break;

					//Case in which the device has received any touch input and the user has lifted his finger.
					case TouchPhase.Ended:
						if (!touchMoved && hit) {
							if (hit.transform.gameObject.tag == "endPoint") {
								MoveToPoint (hit.transform.gameObject.GetComponent<Hex> ().xChartPos, hit.transform.gameObject.GetComponent<Hex> ().yChartPos);
							} else if (hit.transform.gameObject.tag == "PTOFirst") {
								StartCoroutine (portalMoveThroughPTO (hit.transform.gameObject.GetComponent<Hex> ().xChartPos, hit.transform.gameObject.GetComponent<Hex> ().yChartPos));
							} else if (hit.transform.gameObject.tag == "PTTFirst") {
								StartCoroutine (portalMoveThroughPTT (hit.transform.gameObject.GetComponent<Hex> ().xChartPos, hit.transform.gameObject.GetComponent<Hex> ().yChartPos));
							}
							//LevelManager.manager.numOfMovesMade++;
						}
						break;
					}
				}

			}
		}
	}


	IEnumerator portalMoveThroughPTO(int xTouchPosition, int yTouchPosition) {
		Vector2 ptoPosition = hexChartScript.findPartnerPortal(HexVariables.PTT);
		Vector2 pttPosition = hexChartScript.findPartnerPortal(HexVariables.PTO);

        if (catType == "Black")
        {
            SoundManager.soundManager.PlayBlackMoveSound();
        }
        else
        {
            SoundManager.soundManager.PlayWhiteMoveSound();
        }
        playStartMoveAnimation();

		iTween.MoveTo(Character, iTween.Hash("time", k_iTweenTime, "x", ptoPosition.x, "z", -5, "y", ptoPosition.y));
		yield return new WaitForSeconds(1.1f);
        if (catType == "Black")
        {
            SoundManager.soundManager.playBlackCharacterPortalSound();
        }
        else
        {
            SoundManager.soundManager.playWhiteCharacterPortalSound();
        }
        transform.position = new Vector3(pttPosition.x, pttPosition.y, -5);
		MoveToPoint(xTouchPosition, yTouchPosition);

	}
	IEnumerator portalMoveThroughPTT(int xTouchPosition, int yTouchPosition) {
		Vector2 ptoPosition = hexChartScript.findPartnerPortal(HexVariables.PTT);
		Vector2 pttPosition = hexChartScript.findPartnerPortal(HexVariables.PTO);

        if (catType == "Black")
        {
            SoundManager.soundManager.PlayBlackMoveSound();
        }
        else
        {
            SoundManager.soundManager.PlayWhiteMoveSound();
        }
        playStartMoveAnimation();

		iTween.MoveTo(Character, iTween.Hash("time", k_iTweenTime, "x", pttPosition.x, "z", -5, "y", pttPosition.y));
		yield return new WaitForSeconds(1.1f);
        if (catType == "Black")
        {
            SoundManager.soundManager.playBlackCharacterPortalSound();
        }
        else
        {
            SoundManager.soundManager.playWhiteCharacterPortalSound();
        }
        transform.position = new Vector3(ptoPosition.x, ptoPosition.y, -5);
		MoveToPoint(xTouchPosition, yTouchPosition);
	}


	//Destroy cat
	public void DestroyMe(){
		Destroy(gameObject);
	}

	/*
	 * CheckPath() receives the direction the player whants to move and the current position the player is located in.
	 * Once the data is received the code uses ITween and recursion to move the character to the last place it can go in the provided direction.
	 */
	public Vector2 CheckPath(string inputDirection, int givenXPosition, int givenYPosition, int wentThroughPortal){
		if (givenXPosition != get_XPositionInChart () || givenYPosition != get_YPositionInChart ()) {
			if (hexChartScript.TheHexChart [givenXPosition] [givenYPosition].GetComponent<Hex> ().get_hexType () == HexVariables.PTO ||
				hexChartScript.TheHexChart [givenXPosition] [givenYPosition].GetComponent<Hex> ().get_hexType () == HexVariables.PTT) {
				wentThroughPortal = hexChartScript.TheHexChart [givenXPosition] [givenYPosition].GetComponent<Hex> ().get_hexType ();
				if (catType == "Black") {
					if (givenXPosition != get_XPositionInChart () || givenYPosition != get_YPositionInChart ()) {
						hexChartScript.changeMovementGrid (HexVariables.WBM, givenXPosition, givenYPosition);
					} 
				} else {
					if (givenXPosition != get_XPositionInChart () || givenYPosition != get_YPositionInChart ()) {
						hexChartScript.changeMovementGrid (HexVariables.WWM, givenXPosition, givenYPosition);
					} 
				} 
				int[] newPortalPosition = 
				hexChartScript.findPartnerPortalInt (hexChartScript.TheHexChart [givenXPosition] [givenYPosition].GetComponent<Hex> ().get_hexType ());
				givenXPosition = newPortalPosition [1];
				givenYPosition = newPortalPosition [0];
				//return CheckPath(inputDirection, checkThisX, checkThisY);
			}
		}

		
		// CheckThisX and checkThisY are the x and y positions in the graph, the Hexagon at that location will be checked as an option for the next movement.
		int checkThisX = -1, checkThisY = -1;

		// Find the next hexagon in the provided direction
		switch (inputDirection) {
		case "topLeft":
			checkThisX = givenXPosition - 1;
			if(givenXPosition % 2 == 0){
				checkThisY = givenYPosition - 1;
			} else {
				checkThisY = givenYPosition;
			}
			break;
		case "topRight":
			checkThisX = givenXPosition - 1;
			if(givenXPosition % 2 == 1){
				checkThisY = givenYPosition + 1;
			} else {
				checkThisY = givenYPosition;
			}
			break;
		case "left":
			checkThisX = givenXPosition;
			checkThisY = givenYPosition - 1;
			break;
		case "right":
			checkThisX = givenXPosition;
			checkThisY = givenYPosition + 1;
			break;
		case "bottomLeft":
			checkThisX = givenXPosition + 1;
			if(givenXPosition % 2 == 0){
				checkThisY = givenYPosition - 1;
			} else {
				checkThisY = givenYPosition;
			}
			break;
		case "bottomRight":
			checkThisX = givenXPosition + 1;
			if((givenXPosition % 2) == 1){
				checkThisY = givenYPosition + 1;
			} else {
				checkThisY = givenYPosition;
			}
			break;
		}

		/*
		 * If the Hexagon we are checking is the last hexagon in the provided direction the player 
		 * can move to then move to it, otherwise continue checking recursively.
		 */
		if(hexChartScript.checkIfCharacterCanReachBlock(checkThisX, checkThisY)){
			if(catType == "Black"){
				if(givenXPosition != get_XPositionInChart() || givenYPosition != get_YPositionInChart()){
					hexChartScript.changeMovementGrid(HexVariables.WBM ,givenXPosition, givenYPosition);
				} 
			} else {
				if(givenXPosition != get_XPositionInChart() || givenYPosition != get_YPositionInChart()){
					hexChartScript.changeMovementGrid(HexVariables.WWM ,givenXPosition, givenYPosition);
				} 
			} 
			return CheckPath(inputDirection, checkThisX, checkThisY, wentThroughPortal);
		} else {

			int standingOnHex = hexChartScript.TheHexChart[givenXPosition][givenYPosition].GetComponent<Hex>().get_hexType();

			if(catType == "Black" && (givenXPosition != get_XPositionInChart() || givenYPosition != get_YPositionInChart())){
				hexChartScript.changeMovementGrid(HexVariables.WBE ,givenXPosition, givenYPosition);
			} else if (givenXPosition != get_XPositionInChart() || givenYPosition != get_YPositionInChart()){
				hexChartScript.changeMovementGrid(HexVariables.WWE ,givenXPosition, givenYPosition);
			}
			if(givenXPosition != get_XPositionInChart() || givenYPosition != get_YPositionInChart()){
				if(wentThroughPortal == 0){
					hexChartScript.setHexTag(givenXPosition, givenYPosition);
				} else if (wentThroughPortal == HexVariables.PTO){
					hexChartScript.setHexPortalTag(HexVariables.PTO, givenXPosition, givenYPosition);
				} else if (wentThroughPortal == HexVariables.PTT){
					hexChartScript.setHexPortalTag(HexVariables.PTT, givenXPosition, givenYPosition);
				}
			}
			return new Vector2(givenXPosition, givenYPosition);
		}
	}

	//Use iTween to move the character to the hexagon at the provided coordinats.
	private void MoveToPoint(int givenXPosition, int givenYPosition){
		int currentlyOnHex = hexChartScript.TheHexChart [get_XPositionInChart()] [get_YPositionInChart()].GetComponent<Hex> ().get_hexType ();
		set_Chosen(false);
		set_XPositionInChart(givenXPosition);
		set_YPositionInChart(givenYPosition);
		positionInTheWorld = hexChartScript.HexOffset( givenYPosition, givenXPosition );
		Vector3 position = new Vector3( positionInTheWorld.x, positionInTheWorld.y, -5);
        bool shouldBeFacingLeft = position.x < transform.position.x;

        if (shouldBeFacingLeft)
        {
            transform.rotation = m_LookLeft;
        }
        else
        {
            transform.rotation = m_LookRight;
        }

		int standingOnHex = hexChartScript.TheHexChart [givenXPosition] [givenYPosition].GetComponent<Hex> ().get_hexType ();
		Grid.GetComponent<stepstarcounter>().incrementSteps();
		Grid.GetComponent<stepstarcounter>().updateMovesLeftText();
		justMoved = true;
	    if (catType == "Black")
	    {
	        SoundManager.soundManager.PlayBlackMoveSound();
	    }
	    else
	    {
            SoundManager.soundManager.PlayWhiteMoveSound();
        }

		if(standingOnHex >= HexVariables.TB1 && standingOnHex <= HexVariables.NB1){
			hexChartScript.activatePath(hexChartScript.TheHexChart[givenXPosition][givenYPosition].GetComponent<Hex>().get_hexType());
		}
		if (currentlyOnHex == HexVariables.LB1) {
			hexChartScript.activate_Button(HexVariables.NB1);
			hexChartScript.resetTempPathMovability ();

		} 
		else if (currentlyOnHex == HexVariables.NB1) {
			hexChartScript.activate_Button(HexVariables.LB1);
			hexChartScript.resetTempPathMovability ();

		}
	    Grid.GetComponent<GameManager>().resetBoardMoves();
		if (givenXPosition == theOtherCat.GetComponent<Movement> ().get_XPositionInChart () &&
			givenYPosition == theOtherCat.GetComponent<Movement> ().get_YPositionInChart ()) {
			playStartMoveAnimation();
			iTween.MoveTo (Character, iTween.Hash ("time", k_iTweenTime, "x", position.x, "z", position.z, "y", position.y, "oncomplete", "endGameConditionMet", "oncompletetarget", this.gameObject));
		} else if(checkGivenHexForStars(givenYPosition, givenXPosition)){
			playStartMoveAnimation();
			collectingStar = true;
			iTween.MoveTo (Character, iTween.Hash ("time", k_iTweenTime, "x", position.x, "z", position.z, "y", position.y, "oncomplete", "starConditionMet", "oncompletetarget", this.gameObject));
		} else {
			playStartMoveAnimation();
			iTween.MoveTo (Character, iTween.Hash ("time", k_iTweenTime, "x", position.x, "z", position.z, "y", position.y, "oncomplete", "moveConditionMet", "oncompletetarget", this.gameObject));
		}
	}

	// Changes the CharacterYPosition to a new X.
	public void set_XPositionInChart(int newXPosition){
		CharacterXPosition = newXPosition;
	}

	// Changes the CharacterYPosition to a new Y.
	public void set_YPositionInChart(int newYPosition){
		CharacterYPosition = newYPosition;
	}

	// Returns the X relative location the character is on the graph.
	public int get_XPositionInChart(){
		return CharacterXPosition;
	}

	// Returns the X relative location the character is on the graph.
	public int get_YPositionInChart(){
		return CharacterYPosition;
	}

	// Returns this cats current X position on the map
	public float get_XPositionInWorld(){
		return this.transform.position.x;
	}

	// Returns this cats current Y position in the map
	public float get_YPositionInWorld(){
		return this.transform.position.y;
	}

	// Set the Cats current color \ type - black \ white
	public void set_CatType(string catColor){
		catType = catColor;
	}

	// Returns the current cats color \ type - black \ white
	public string get_CatType(){
		return catType;
	}

	public void set_Chosen(bool newStatus){
		chosen = newStatus;
	}

	public bool get_Chosen(){
		return chosen;
	}
	public void set_theOtherCat(GameObject otherCat){
		theOtherCat = otherCat;
	}

	public bool getWinState(){
		return wonLevel;
	}

	public void winOpenMenu(){
        SoundManager.soundManager.playLevelWinSound();
		Grid.GetComponent<IngameScript> ().shutDownIngameCanvas ();
		Grid.GetComponent<IngameScript> ().activateWinScreen();
	}
	public void loseOpenMenu(){
        SoundManager.soundManager.playLevelFailSound();
		Grid.GetComponent<IngameScript> ().shutDownIngameCanvas ();
		Grid.GetComponent<IngameScript> ().activateLoseScreen();
	}

	private void resetPaths(){
		hexChartScript.resetTempBlockSprite();
		hexChartScript.resetMovementGrid();
	}

	private bool checkGivenHexForStars(int xPosition, int yPosition){
		bool foundAStar = true;
		for (int i = 0; i < hexChartScript.get_numberOfStars(); i++) {
			if (hexChartScript.getStar (i) &&
			    (xPosition == hexChartScript.getStar (i).GetComponent<starScript> ().get_XpositionInChart ()) && 
			    (yPosition == hexChartScript.getStar (i).GetComponent<starScript> ().get_YpositionInChart ())) {
				tempStar = hexChartScript.getStar (i);
				return foundAStar;
			}
		}
		return !foundAStar;
	}

	#region Animation Functions

	private void moveConditionMet(){
		Grid.GetComponent<GameManager>().nextTurn();
		Animator.SetBool("Start Move", false);
		Animator.SetBool("Star", false);
		Animator.SetBool("End Move", true);
	}

	private void starConditionMet(){
		Grid.GetComponent<GameManager>().nextTurn();
		animationTimeLeft = blinkBreakTime + 1;
		Animator.SetBool("Blink", false);
		Animator.SetBool("Start Move", false);
		Animator.SetBool("Star", true);
		Animator.SetBool("End Move", true);
		collectingStar = false;
	}

	//Play the unite animation and show score menu after
	private void endGameConditionMet(){
        m_TimeElapsed = m_Timer;
		theOtherCat.GetComponent<SpriteRenderer>().enabled = false;
		Animator.SetBool("Start Move", false);
		Animator.SetBool("Combine", true);
		catsHaveMet = true;
	}

	private void playStartMoveAnimation(){
		Animator.SetBool("End Move", false);
		Animator.SetBool("Start Move", true);
	}

	public void playChooseAnimation(){
        if (catType == "Black")
        {
            SoundManager.soundManager.playBlackChosen();
        }
        else
        {
            SoundManager.soundManager.playWhiteChosen();
        }
		Animator.SetBool("Chosen", true);
	}
	
	#endregion
}
