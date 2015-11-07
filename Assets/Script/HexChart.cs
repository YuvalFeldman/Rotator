using UnityEngine;
using System.Collections;
using System;
using System.Text;
//TODO REMOVE - TESTING ONLY
using UnityEngine.UI;


public class HexChart : MonoBehaviour
{
    #region Public Variables
    public Transform Hexagon, tempHexagon;
	public GameObject[][] TheHexChart, movementGrid;
	public Sprite[] tilesSprites;
	public float radius = 0.5f;
	public GameObject blackCat, whiteCat, star, Background;
    public bool useAsInnerCircleRadius = true;
    public GameManager manager;
    public GameObject[] stars = new GameObject[6]; // An array of the stars in the level.
    public int numberOfStars; // The amount of stars in the level.
    #endregion

    #region Private Variables
    private Sprite ForestBackground, MountainBackground;
    private GameObject instantiatedWhiteCat, instantiatedBlackCat;
	private int[][] formation;
	private int maxWidth;

    /// <summary>
    /// The path to the runtime animations folder
    /// </summary>
	private string m_AnimationPath = "RunTimeAnimations/";

    /// <summary>
    /// The path to the character animations folder
    /// </summary>
    private string m_CharacterAnimationPath;

    /// <summary>
    /// The path beginning to the character animations folder
    /// Used to build the full path
    /// </summary>
    private const string m_CharacterAnimationPathBeginning = "RunTimeAnimations/Characters/";

    private string m_PortalPath;
    private float offsetX, offsetY, unitLength;
    #endregion

    void Start() {
        initializeCharacterPath();
        m_PortalPath = buildPortalString();
        initializeBackground();
        blackCat = Resources.Load(m_CharacterAnimationPath + "blackCat") as GameObject;
        whiteCat = Resources.Load(m_CharacterAnimationPath + "whiteCat") as GameObject;
		unitLength = ( useAsInnerCircleRadius )? (radius / (Mathf.Sqrt(3)/2)) : radius;
		offsetX = unitLength * Mathf.Sqrt(3);
		offsetY = unitLength * 1.5f;
		manager = this.GetComponent<GameManager>();
		maxWidth = 0;
		GetComponent<stepstarcounter> ().resetStars ();
		GetComponent<stepstarcounter> ().resetSteps ();
		GetComponent<NarratorText> ().resetCurrentlyShowingText ();
		HexVariables.narratorDoneSpeaking = true;
	    if (LevelManager.manager.currentLevel > 10)
	    {
            tilesSprites[HexVariables.W] = tilesSprites[HexVariables.WHT];
            tilesSprites[HexVariables.S] = tilesSprites[HexVariables.WHT];
            tilesSprites[HexVariables.WC] = tilesSprites[HexVariables.WHT];
            tilesSprites[HexVariables.BC] = tilesSprites[HexVariables.WHT];
            Background.gameObject.GetComponent<SpriteRenderer>().sprite = MountainBackground;
	    }
	    else
	    {
            tilesSprites[HexVariables.W] = tilesSprites[HexVariables.FHT];
            tilesSprites[HexVariables.S] = tilesSprites[HexVariables.FHT];
            tilesSprites[HexVariables.WC] = tilesSprites[HexVariables.FHT];
            tilesSprites[HexVariables.BC] = tilesSprites[HexVariables.FHT];
            Background.gameObject.GetComponent<SpriteRenderer>().sprite = ForestBackground;
	    }
	}

    /// <summary>
    /// Initializes the backgrounds as necessary
    /// </summary>
    private void initializeBackground()
    {
        bool backgroundIsMountain = LevelManager.manager.currentLevel > 10;

        if (backgroundIsMountain)
        {
            MountainBackground = Resources.Load<Sprite>("Background/MountainBackground");
        }
        else
        {
            ForestBackground = Resources.Load<Sprite>("Background/ForestBackground");
        } 
    }

    /// <summary>
    /// Initializes the path to load character prefabs from
    /// </summary>
    private void initializeCharacterPath()
    {
        StringBuilder characterAnimationPathBuilder = new StringBuilder(m_CharacterAnimationPathBeginning);
        characterAnimationPathBuilder.Append(Enum.GetName(typeof(eAnimationSkin),
                            LevelManager.manager.AnimationSkin) + "/");
        m_CharacterAnimationPath = characterAnimationPathBuilder.ToString();
    }

	// create the level.
	public void createLevel(int[][] hexChart){
		formation = hexChart;
		TheHexChart = new GameObject[hexChart.Length][];
		movementGrid = new GameObject[hexChart.Length][];
		for(int height = 0; height < hexChart.Length; height++) {
			TheHexChart[height] = new GameObject[hexChart[height].Length];
			movementGrid[height] = new GameObject[hexChart[height].Length];
			if(maxWidth < hexChart[height].Length){
				maxWidth = hexChart[height].Length;
			}
			for(int width = 0; width < hexChart[height].Length; width++) {
                if (hexChart[height][width] == HexVariables.T) 
                {
                    TheHexChart[height][width] = null;
                    movementGrid[height][width] = null;
                    continue; 
                }
				Vector2 hexpos = HexOffset(width, height);
				Vector3 pos = new Vector3(hexpos.x, hexpos.y, 1);
				Vector3 temppos = new Vector3(hexpos.x, hexpos.y, -2);
				Vector3 movePos = new Vector3(hexpos.x, hexpos.y, -1);
				TheHexChart[height][width] = (Instantiate(Hexagon, pos, Quaternion.identity) as Transform).gameObject;
				movementGrid[height][width] = (Instantiate(tempHexagon, movePos, Quaternion.identity) as Transform).gameObject;

				movementGrid[height][width].gameObject.GetComponent<SpriteRenderer> ().sprite = tilesSprites [0];

				TheHexChart[height][width].GetComponent<Hex>().setCordsHex(height, width);
				TheHexChart[height][width].GetComponent<Hex>().set_isPath(true);
				if(hexChart[height][width] == HexVariables.T ||
				   (TheHexChart[height][width].GetComponent<Hex>().get_hexType() >= HexVariables.VP1 &&
				 TheHexChart[height][width].GetComponent<Hex>().get_hexType() <= HexVariables.BP1)){
					TheHexChart[height][width].GetComponent<Hex>().set_isPath(false);
				}

				if(hexChart[height][width] == HexVariables.S){ // Do this if the current hexagon is a star hexagon
					Vector3 starPos = new Vector3(hexpos.x, hexpos.y, -4);
					stars[numberOfStars] = (GameObject)Instantiate(star, starPos, Quaternion.identity);
					stars[numberOfStars].GetComponent<starScript>().set_XY(width, height);
					numberOfStars++;
					set_formationVariable(HexVariables.W, height, width);
				} else if(hexChart[height][width] == HexVariables.WC) { // Do this if the current hexagon is a spawn whitecat hexagon
					formation[height][width] = HexVariables.W;
					Vector3 catPosition = new Vector3( hexpos.x, hexpos.y, -5 );
					instantiatedWhiteCat = (GameObject)Instantiate(whiteCat, catPosition, Quaternion.identity);
					instantiatedWhiteCat.GetComponent<Movement>().set_CatType("White");
					instantiatedWhiteCat.GetComponent<Movement>().set_XPositionInChart(height);
					instantiatedWhiteCat.GetComponent<Movement>().set_YPositionInChart(width);
					instantiatedWhiteCat.GetComponent<Movement>().Character = instantiatedWhiteCat;
					GetComponent<GameManager>().set_whiteCat(instantiatedWhiteCat);
				} else if (hexChart[height][width] == HexVariables.BC){ // Do this if the current hexagon is a spawn blackcat hexagon
					formation[height][width] = HexVariables.W;
					Vector3 catPosition = new Vector3( hexpos.x, hexpos.y, -5 );
					instantiatedBlackCat = (GameObject)Instantiate(blackCat, catPosition, Quaternion.identity);
					instantiatedBlackCat.GetComponent<Movement>().set_CatType("Black");
					instantiatedBlackCat.GetComponent<Movement>().set_XPositionInChart(height);
					instantiatedBlackCat.GetComponent<Movement>().set_YPositionInChart(width);
					instantiatedBlackCat.GetComponent<Movement>().Character = instantiatedBlackCat;
					GetComponent<GameManager>().set_blackCat(instantiatedBlackCat);
				} else if (hexChart[height][width] == HexVariables.PTT){
					TheHexChart[height][width].AddComponent<Animator>();
					Animator animator = TheHexChart[height][width].GetComponent<Animator>();
                    animator.runtimeAnimatorController = Resources.Load(m_PortalPath) as RuntimeAnimatorController;
					animator.SetBool("PTT", true);
				} else if (hexChart[height][width] == HexVariables.PTO){
					TheHexChart[height][width].AddComponent<Animator>();
					Animator animator = TheHexChart[height][width].GetComponent<Animator>();
                    animator.runtimeAnimatorController = Resources.Load(m_PortalPath) as RuntimeAnimatorController;
					animator.SetBool("PTO", true);
				} else if (hexChart[height][width] == HexVariables.SP1){
					//Perma button, starts lowered
					TheHexChart[height][width].AddComponent<Animator>();
					Animator animator = TheHexChart[height][width].GetComponent<Animator>();
					animator.runtimeAnimatorController = Resources.Load(m_AnimationPath+"Pink Door") as RuntimeAnimatorController;
					animator.SetBool("Close", true);
				} else if (hexChart[height][width] == HexVariables.TP1){
					//Perma button, starts raised
					TheHexChart[height][width].AddComponent<Animator>();
					Animator animator = TheHexChart[height][width].GetComponent<Animator>();
					animator.runtimeAnimatorController = Resources.Load(m_AnimationPath+"Pink Door") as RuntimeAnimatorController;
					animator.SetBool("Open", true);
				}else if (hexChart[height][width] == HexVariables.VP1){
					//Temp button, starts lowered
					TheHexChart[height][width].AddComponent<Animator>();
					Animator animator = TheHexChart[height][width].GetComponent<Animator>();
					animator.runtimeAnimatorController = Resources.Load(m_AnimationPath+"Orange Door") as RuntimeAnimatorController;
					animator.SetBool("Close", true);
				}else if (hexChart[height][width] == HexVariables.BP1){
					//Temp button, starts raised
					TheHexChart[height][width].AddComponent<Animator>();
					Animator animator = TheHexChart[height][width].GetComponent<Animator>();
					animator.runtimeAnimatorController = Resources.Load(m_AnimationPath+"Orange Door") as RuntimeAnimatorController;
					animator.SetBool("Open", true);
				}
				if(hexChart[height][width] == HexVariables.SP1 || hexChart[height][width] == HexVariables.VP1){
					TheHexChart [height] [width].GetComponent<Hex> ().set_isPath (false);
				}
				TheHexChart[height][width].GetComponent<Hex>().set_hexType(formation[height][width]);
				//TheHexChart[height][width].gameObject.GetComponent<SpriteRenderer> ().sprite = tilesSprites [formation[height][width]];
            }
		}
		instantiatedWhiteCat.GetComponent<Movement> ().set_theOtherCat (instantiatedBlackCat);
		instantiatedBlackCat.GetComponent<Movement> ().set_theOtherCat (instantiatedWhiteCat);
		switchCharacterActive ();
	}

	// the math used to calculate the hexagon placement.
	public Vector2 HexOffset(int x, int y) {
		Vector2 position = Vector2.zero;
		
		if( y % 2 == 0 ) {
			position.x = x * offsetX;
			position.y = -1 * y * offsetY;
		}
		else {
			position.x = ( x + 0.5f ) * offsetX;
			position.y = -1 * y * offsetY;
		}
		
		return position;
	}

	// the math used to calculate the distance between hexagon vectors.
	public Vector2 reverseOffset(float x, float y){
		Vector2 position = Vector2.zero;
		position.y = (int) (-1 * y)/offsetY;
		
		if( y % 2 == 0 ) {
			position.x = (int) (x / offsetX);
		}
		else {
			position.x = (int) (x - (0.5f * offsetX))/offsetX;
		}
		
		return position;
	}

	/// <summary>
    /// Check if the current block can be reached by the character.
	/// </summary>
	/// <param name="i_XPositionInArray"></param>
	/// <param name="i_YPositionInArray"></param>
	/// <returns></returns>
	public bool checkIfCharacterCanReachBlock(int i_XPositionInArray, int i_YPositionInArray){
        bool reacheable = true;

        if(!(i_XPositionInArray < 0 ||
           i_YPositionInArray < 0 ||
           i_XPositionInArray >= get_Height() ||
           i_YPositionInArray >= get_Width(i_XPositionInArray))) {
                    if (TheHexChart[i_XPositionInArray][i_YPositionInArray] == null)
                    {
                        reacheable = false;
                    }
                    else if (!TheHexChart[i_XPositionInArray][i_YPositionInArray].GetComponent<Hex>().get_isPath())
                    {
                        reacheable = false;
                    }
        } else {
            reacheable = false;
        }

        return reacheable;
	}

	// get the total width of a specific row.
	public int get_Width (int row){
		return TheHexChart[row].Length;
	}

	public int get_maxWidth(){
		int maxWidthHere = 0;
		for (int i = 0; i < get_Height(); i++) {
			if(get_Width(i) > maxWidthHere){
				maxWidthHere = get_Width(i);
			}
		}
		return maxWidthHere;
	}

	public float get_maxHeightInWorld(){
		Vector2 maxHeight = HexOffset (get_maxWidth (), get_Height ());
		return maxHeight.y;
	}


	/* ***********************************************
	 * Used by CameraMovement, do not touch!!!!!!!!!!!
	 * ***********************************************/
	public Vector2 get_cameraParameters(){
		if (LevelManager.manager.currentLevel <= 20) {
			Vector2 cameraPlace = new Vector2 (((maxWidth + 0.5f) * offsetX), (-1 * (TheHexChart.Length - 2) * offsetY));
			return cameraPlace;
		} else {
			return new Vector2(0,0);
		}
	}



	public float get_maxWidthInWorld(){
		Vector2 maxHeight = HexOffset (get_maxWidth (), get_Height ());
		return maxHeight.x;
	}

	// get the total hight of the map.
	public int get_Height(){
		if (TheHexChart == null) {
			Debug.Log ("null");
		}

		return TheHexChart.Length;
	}

	// Permanently changes the sprite of a hexagon
	public void ChangeBlockSprite(int newTile, int hexXPosition, int hexYPosition){
		TheHexChart [hexXPosition] [hexYPosition].gameObject.GetComponent<SpriteRenderer> ().sprite = tilesSprites [newTile];
		formation [hexXPosition] [hexYPosition] = newTile;
	}
	// Temporarily changes the sprite of a hexagon - until the next reset.
	public void temporaryChangeBlockSprite(int newTile, int hexXPosition, int hexYPosition){
		TheHexChart [hexXPosition] [hexYPosition].gameObject.GetComponent<SpriteRenderer> ().sprite = tilesSprites [newTile];
	}
	// Resets all the temporary changes made to the hexgrid.
	public void resetTempBlockSprite(){
		for (int i = 0; i < TheHexChart.Length; i++) {
			for (int j = 0; j < TheHexChart[i].Length; j++){
                if (TheHexChart[i][j] != null)
                {
                    if (TheHexChart[i][j].gameObject.GetComponent<Hex>().get_hexType() != HexVariables.PTO ||
                       TheHexChart[i][j].gameObject.GetComponent<Hex>().get_hexType() != HexVariables.PTT)
                    {
                        TheHexChart[i][j].gameObject.GetComponent<SpriteRenderer>().sprite = tilesSprites[formation[i][j]];
                    }
                }
			}
		}
	}
	public void findButtonBlocks(){
		for (int i = 0; i < TheHexChart.Length; i++) {
			for (int j = 0; j < TheHexChart[i].Length; j++){
                 if (TheHexChart[i][j] != null)
                {
				    if(TheHexChart [i] [j].gameObject.GetComponent<Hex>().get_hexType() != HexVariables.SP1){
					    TheHexChart [i] [j].gameObject.GetComponent<SpriteRenderer> ().sprite = tilesSprites [formation[i][j]];
				    } else if (TheHexChart [i] [j].gameObject.GetComponent<Hex>().get_hexType() != HexVariables.TP1){
					    TheHexChart [i] [j].gameObject.GetComponent<SpriteRenderer> ().sprite = tilesSprites [formation[i][j]];
				    } else if(TheHexChart [i] [j].gameObject.GetComponent<Hex>().get_hexType() != HexVariables.VP1){
					    TheHexChart [i] [j].gameObject.GetComponent<SpriteRenderer> ().sprite = tilesSprites [formation[i][j]];
				    } else if (TheHexChart [i] [j].gameObject.GetComponent<Hex>().get_hexType() != HexVariables.BP1){
					    TheHexChart [i] [j].gameObject.GetComponent<SpriteRenderer> ().sprite = tilesSprites [formation[i][j]];
				    }
                }
			}
		}
	}

	public void resetTempPathMovability(){
		for (int i = 0; i < TheHexChart.Length; i++) {
			for (int j = 0; j < TheHexChart[i].Length; j++){
                if (TheHexChart[i][j] != null)
                {
                    if (TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.VP1)
                    {
                        TheHexChart[i][j].GetComponent<Hex>().set_isPath(false);
                    }
                    else if (TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.BP1)
                    {
                        TheHexChart[i][j].GetComponent<Hex>().set_isPath(true);
                    }
                }
			}
		}
	}

	public void changeMovementGrid(int newTile, int hexXPosition, int hexYPosition){
		movementGrid [hexXPosition] [hexYPosition].gameObject.GetComponent<SpriteRenderer> ().sprite = tilesSprites [newTile];
	}
	public void resetMovementGrid(){
		for (int i = 0; i < movementGrid.Length; i++) {
			for (int j = 0; j < movementGrid[i].Length; j++){
                if (movementGrid[i][j] != null)
                {
                    movementGrid[i][j].gameObject.GetComponent<SpriteRenderer>().sprite = tilesSprites[0];
                }
            }
		}
	}



	// Change the tag of the current hex to endPoint (endpoint is an optional point the current moving character can move to).
	public void setHexTag(int hexXPosition, int hexYPosition){
		TheHexChart [hexXPosition] [hexYPosition].gameObject.tag = "endPoint";
	}
	// Change the tag of the current hex to endPoint (endpoint is an optional point the current moving character can move to).
	public void setHexPortalTag(int portalType, int hexXPosition, int hexYPosition){
		if (portalType == HexVariables.PTO) {
			TheHexChart [hexXPosition] [hexYPosition].gameObject.tag = "PTOFirst";
		} else if (portalType == HexVariables.PTT) {
			TheHexChart [hexXPosition] [hexYPosition].gameObject.tag = "PTTFirst";
		}
	}

	// Resets all the temporary changes made to the hexgrid.
	public void resetHexTags(){
		for (int i = 0; i < TheHexChart.Length; i++) {
			for (int j = 0; j < TheHexChart[i].Length; j++){
                if (TheHexChart[i][j] != null)
                {
                    TheHexChart[i][j].gameObject.tag = "notAnEndPoint";
                }
			}
		}
	}

	// Change a variable in the formation array.
	public void set_formationVariable(int newTile, int hexXPosition, int hexYPosition){
		formation [hexXPosition] [hexYPosition] = newTile;
	}

	// retreive a variable from the formation array.
	public int get_TileType(int xPosition, int yPosition){
		return formation [xPosition] [yPosition];
	}

	public void activate_Button(int buttonType){
		for (int i = 0; i < TheHexChart.Length; i++) {
			for (int j = 0; j < TheHexChart[i].Length; j++){
                if (TheHexChart[i][j] == null)
                {
                    continue;
                }
				else if (buttonType == HexVariables.TB1 && 
				    TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.TP1) 
				{
					// permanetly lowers
                    SoundManager.soundManager.playOpenGateSound();
					TheHexChart[i][j].GetComponent<Animator>().SetBool("Open", false);
					TheHexChart[i][j].GetComponent<Animator>().SetBool("Close", true);
					formation [i] [j] = HexVariables.SP1;
					TheHexChart[i][j].GetComponent<Hex>().set_hexType(HexVariables.SP1);
				} 
				else if(buttonType == HexVariables.RB1 &&
				        TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.SP1) 
				{
					//permanently raises;
                    SoundManager.soundManager.playCloseGateSound();
					TheHexChart[i][j].GetComponent<Animator>().SetBool("Close", false);
					TheHexChart[i][j].GetComponent<Animator>().SetBool("Open", true);
					formation [i] [j] = HexVariables.TP1;
					TheHexChart[i][j].GetComponent<Hex>().set_hexType(HexVariables.TP1);
				} 
				else if(buttonType == HexVariables.LB1 && 
				        TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.BP1) 
				{
					// temporarily lowers
                    SoundManager.soundManager.playOpenGateSound();
					TheHexChart[i][j].GetComponent<Animator>().SetBool("Open", false);
					TheHexChart[i][j].GetComponent<Animator>().SetBool("Close", true);
					formation [i] [j] = HexVariables.VP1;
					TheHexChart[i][j].GetComponent<Hex>().set_hexType(HexVariables.VP1);
				} 
				else if(buttonType == HexVariables.NB1 &&
				        TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.VP1) 
				{
					// temporarily raises
                    SoundManager.soundManager.playCloseGateSound();
					TheHexChart[i][j].GetComponent<Animator>().SetBool("Close", false);
					TheHexChart[i][j].GetComponent<Animator>().SetBool("Open", true);
					formation [i] [j] = HexVariables.BP1;
					TheHexChart[i][j].GetComponent<Hex>().set_hexType(HexVariables.BP1);
				} 
			}
		}
	}
	/*public void temp_Activate_Button(int buttonType){
		for (int i = 0; i < TheHexChart.Length; i++) {
			for (int j = 0; j < TheHexChart[i].Length; j++){
				if (buttonType == HexVariables.TB1 &&
				    (TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.SP1 ||
				 TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.TP1)) {
					temporaryChangeBlockSprite(HexVariables.SP1, i, j);
				} else if(buttonType == HexVariables.RB1 &&
				          (TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.SP1 ||
				 TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.TP1)) {
					temporaryChangeBlockSprite(HexVariables.TP1, i, j);
				} else if(buttonType == HexVariables.LB1 &&
				          (TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.VP1 ||
				 TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.BP1)) {
					temporaryChangeBlockSprite(HexVariables.VP1, i, j);
				} else if(buttonType == HexVariables.NB1 &&
				          (TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.VP1 ||
				 TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.BP1)) {
					temporaryChangeBlockSprite(HexVariables.BP1, i, j);
				} 
			}
		}
	}*/

	public void activatePath(int buttonType){
		for (int i = 0; i < TheHexChart.Length; i++) {
			for (int j = 0; j < TheHexChart[i].Length; j++){
                if (TheHexChart[i][j] == null)
                {
                    continue;
                }
				else if (buttonType == HexVariables.TB1 &&
				    (TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.SP1 ||
				 TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.TP1)) {
					TheHexChart [i] [j].GetComponent<Hex> ().set_isPath (false);
				} else if(buttonType == HexVariables.RB1 &&
				          (TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.SP1 ||
				 TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.TP1)) {
					TheHexChart [i] [j].GetComponent<Hex> ().set_isPath (true);
				} else if(buttonType == HexVariables.LB1 &&
				          (TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.VP1 ||
				 TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.BP1)) {
					TheHexChart [i] [j].GetComponent<Hex> ().set_isPath (false);
				} else if(buttonType == HexVariables.NB1 &&
				          (TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.VP1 ||
				 TheHexChart[i][j].GetComponent<Hex>().get_hexType() == HexVariables.BP1)) {
					TheHexChart [i] [j].GetComponent<Hex> ().set_isPath (true);
				} 
			}
		}
	}
	public GameObject getStar(int starnum){
		return stars [starnum];
	}
	public int get_numberOfStars(){
		return numberOfStars;
	}
	public int[] findPartnerPortalInt(int portal){
		int otherPortal;
		int[] portalPlacement = new int[2];
		if (portal == HexVariables.PTO) {
			otherPortal = HexVariables.PTT;
		} else {
			otherPortal = HexVariables.PTO;
		}
		for (int i = 0; i < TheHexChart.Length; i++) {
			for (int j = 0; j < TheHexChart[i].Length; j++){
                if (TheHexChart[i][j] != null)
                {
                    if (TheHexChart[i][j].GetComponent<Hex>().get_hexType() == otherPortal)
                    {
                        portalPlacement[0] = j;
                        portalPlacement[1] = i;
                    }
                }
			}
		}
		return portalPlacement;
	}
	public Vector2 findPartnerPortal(int portal){
		int otherPortal = HexVariables.PTO;
		Vector2 returnVector = new Vector2(0,0);
		if (portal == HexVariables.PTO) {
			otherPortal = HexVariables.PTT;
		}
		for (int i = 0; i < TheHexChart.Length; i++) {
			for (int j = 0; j < TheHexChart[i].Length; j++){
                if (TheHexChart[i][j] != null)
                {
                    if (TheHexChart[i][j].GetComponent<Hex>().get_hexType() == otherPortal)
                    {
                        returnVector = HexOffset(j, i);
                    }
                }
			}
		}
		return returnVector;
	}
	public void commingSoonOpenMenu(){
		GetComponent<IngameScript> ().shutDownIngameCanvas ();
		GetComponent<IngameScript> ().activateCommingSoonScreen();
	}

	public void narrator(){
		string[] narratorSays = GetComponent<NarratorText> ().getNarration ();
		float[] characterApearance = GetComponent<NarratorText> ().getCharacter ();
		if (narratorSays != null) {
			GetComponent<IngameScript> ().shutDownIngameCanvas ();
            GetComponent<IngameScript>().activateNarratingCharacter(characterApearance);
			GetComponent<IngameScript> ().activateNarratorCanvas (narratorSays [GetComponent<NarratorText> ().getCurrentlyShowingText ()]);
		} else {
			HexVariables.narratorDoneSpeaking = true;
            GetComponent<IngameScript>().deactivateNarratorCanvas();
		}
	}
	public void nextTextNarrator(){
		GetComponent<NarratorText> ().incrementCurrentlyShowingText ();
		narrator ();
	}
	public void switchCharacterActive(){
		if (LevelManager.manager.currentLevel < 4) {
			GetComponent<IngameScript> ().deactivateSwitchCharacter();
		}
	}

    /// <summary>
    /// Used to build the String for portal path
    /// </summary>
    /// <returns></returns>
    private string buildPortalString()
    {
        StringBuilder pathBuilder = new StringBuilder();
        pathBuilder.Append(m_AnimationPath);
        pathBuilder.Append("Portal Animator");
        pathBuilder.Append((int)LevelManager.manager.AnimationSkin);
        return pathBuilder.ToString();
    }
}