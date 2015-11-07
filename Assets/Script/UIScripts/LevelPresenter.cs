
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelPresenter : MonoBehaviour 
{
	#region Variables

	public Button button;
	public Text displayName;
 
	// The panel presented when the level is locked
	public GameObject lockedPanel;

	// The text component that displays the number of orbs
	// to collect to unlock the level
	public Text orbsCountToUnlockText;

	public Color levelOpenTextColor;

	public Color levelCompletedTextColor;

	public Color levelLockedTextColor;

	// The sprite to show when the level is open
	public Sprite levelOpenSprite;

	// The sprite to show when the level is locked
	public Sprite levelLockedSprite;

	// The sprite to show when the level is complete
	public Sprite levelCompleteSprite;

	// The Image component the presents an achievement icon
	public Image achievementIcon;

	public Sprite noAchievementsSprite;

	// The sprite to show when the user achieved the following:
	// 1. All orbs collected
	public Sprite allOrbsCollectedSprite;

	// The sprite to show when the user achieved the following:
	// 1. Level completed in minimum steps
	public Sprite minimumStepsSprite;

	// The sprite to show when the user achieved the following:
	// 1. All orbs collected
	// 2. Level completed in minimum steps
	public Sprite allOrbsAndMinimumStepsSprite;

	// Text colors
	//public static Color levelCompleteTextColor = new Color(0.16f, 0.75f, 1);
	//public static Color levelOpenTextColor = new Color(1f, 1f, 1f);
	//public static Color levelLockedTextColor = new Color(0.16f, 0.59f, 0.89f);

	#endregion

	#region Private Fields

	private int levelNumber;
	private int stageNumber;

	#endregion

	#region Properties

	public bool IsLocked { get; set; }

	public bool IsComplete { get; set; }

	public bool MinimumSteps { get; set; }

	public bool AllOrbsCollected { get; set; }

	#endregion

	#region Events

	/// <summary>
	/// UnityEvent to be fired when the level is selected.
	/// </summary>
	public LevelSelectedEvent onSelected = new LevelSelectedEvent();

	#endregion

	#region Public Functions

	/// <summary>
	/// Initializes the object using a stage number and a level number.
	/// </summary>
	/// <param name="stageNumber">Stage number.</param>
	/// <param name="levelNumber">Level number.</param>
	public void Init(int stageNumber, int levelNumber)
	{
		this.stageNumber = stageNumber;
		this.levelNumber = levelNumber;

		this.displayName.text = levelNumber.ToString();
		this.button.onClick.AddListener(Select);

		// Set the state of the level
		IsLocked = !LevelManager.manager.isStagedUnlocked(levelNumber);
		if (!IsLocked) 
		{
			IsComplete = LevelManager.manager.wasLevelFinished(levelNumber);
		}

		// Set the amount of orbs collected
		AllOrbsCollected = LevelManager.manager.FinishedLevelWithAllStars(levelNumber);

		// Check the level was completed with minimum moves
		MinimumSteps = LevelManager.manager.FinishedLevelWithMinimumMoves(levelNumber);

		SetDisplay();
	}

	public void Select()
	{
		OnSelected();
	}

	#endregion

	#region Private Functions

	private void SetDisplay()
	{
		// Check the stage is locked
		if (IsLocked) 
		{
			SetLevelLocked();
		}
		else
		{
			// Check the level is complete
			if (IsComplete) 
			{
				SetLevelComplete();
			}
			else
			{
				SetLevelOpen();
			}

			// Set the achievements for the level
			if (AllOrbsCollected && MinimumSteps) 
			{
				SetAllOrbsAndMinimumSteps();
			}
			else if (AllOrbsCollected)
			{
				SetAllOrbsCollected();
			}
			else if (MinimumSteps)
			{
				SetMinimumSteps();
			}
			else
			{
				SetNoAchievements();
			}
		}
	}

	private void SetLevelOpen()
	{
		button.image.overrideSprite = levelOpenSprite;
		displayName.color = levelOpenTextColor;

		// Hide the Lock panel
		lockedPanel.SetActive(false);

		// Show achievement icon
		achievementIcon.gameObject.SetActive(true);

		// The button is interactible
		button.GetComponent<Button>().interactable = true;
	}

	private void SetLevelLocked()
	{
		button.image.overrideSprite = levelLockedSprite;
		displayName.color = levelLockedTextColor;

		// Hide achievement icon
		achievementIcon.gameObject.SetActive(false);

		// Show the Lock panel
		lockedPanel.SetActive(true);

		// Orbs count to unlock
		orbsCountToUnlockText.text = LevelManager.manager.orbsLeftToOpenLevel(levelNumber).ToString();
	}

	private void SetLevelComplete()
	{
		button.image.overrideSprite = levelCompleteSprite;
		displayName.color = levelCompletedTextColor;

		// The button is interactible
		button.GetComponent<Button>().interactable = true;

		// Show achievement icon
		achievementIcon.gameObject.SetActive(true);

		// Keep Lock panel hidden
		lockedPanel.SetActive(false);
	}

	private void SetNoAchievements()
	{
		achievementIcon.GetComponent<Image>().sprite = noAchievementsSprite;
	}

	private void SetAllOrbsCollected()
	{
		// Display the image indicating all orbs were collected
		achievementIcon.GetComponent<Image>().sprite = allOrbsCollectedSprite;
		achievementIcon.gameObject.SetActive(true);
	}

	private void SetMinimumSteps()
	{
		achievementIcon.GetComponent<Image>().sprite = minimumStepsSprite;
	}

	private void SetAllOrbsAndMinimumSteps()
	{
		achievementIcon.GetComponent<Image>().sprite = allOrbsAndMinimumStepsSprite;
	}

	#endregion

	/// <summary>
	/// Raises the onSelected event.
	/// </summary>
	private void OnSelected()
	{
		// Invoke all registered callbacks
		onSelected.Invoke(this.levelNumber);
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
