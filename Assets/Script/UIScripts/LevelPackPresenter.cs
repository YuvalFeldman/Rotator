using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages logic to present a Level Pack in the UI.
/// </summary>
public class LevelPackPresenter : MonoBehaviour 
{
	#region Editor Fields

	public GameObject backgroundPanel;

	public GameObject progressBarPanel;

	public Button button;

	public Text nameLabel, levelNumber;

	// An image component that displays the
	// achievements of the user in the current
	// level pack
	public Image achievementImage;

	// The sprite to set as background when 
	// the level pack is at it's basic state
	public Sprite basicLevelBackground;

	// The sprite to set as background when
	// all levels were completed with minimum steps
	public Sprite minimumMovesAchievementBackground;

	// The sprite to set as background when all 
	// orbs were collected
	public Sprite allOrbsCollectedBackground;

	// The sprite to set as background when all
	// orbs were collected AND all levels were completed 
	// with minimum steps
	public Sprite movesAndOrbsAchievementBackground;

	#endregion

	#region Private Fields

	private LevelPack levelPackModel;

	#endregion

	#region Properties

	public bool AllOrbsCollected { get; set; }

	public bool MinimumMovesAchievement { get; set; } 

	public int OrbsCount { get; set; }

	public int OrbsCollected { get; set; }

	#endregion

	#region Events

	/// <summary>
	/// UnityEvent to be fired when the level pack is selected.
	/// </summary>
	public LevelPackSelectedEvent onSelected;

	#endregion

	#region Public Methods

	public void Init(LevelPack levelPack)
	{
		if (levelPack == null)
			throw new UnityException("levelPack is null.");

		if (onSelected == null)
			onSelected = new LevelPackSelectedEvent();

		// Initialize the properties of this display object
		levelPackModel = levelPack;
        this.nameLabel.text = string.Format("{0}", levelPackModel.Name);
        this.levelNumber.text = string.Format("{0}", levelPackModel.LevelPackNumber);
        this.button.onClick.AddListener(Select);

		OrbsCount = LevelManager.manager.numberOfStarsTotalInStage(levelPackModel.LevelPackNumber);
		OrbsCollected = LevelManager.manager.numberOfStarsCollectedInStage(levelPackModel.LevelPackNumber);
		AllOrbsCollected = (OrbsCount == OrbsCollected);
		MinimumMovesAchievement = LevelManager.manager.StageFinishedWithMinimumMoves(levelPackModel.LevelPackNumber);
		SetDisplay();
	}

	public void Select()
	{
		OnSelected();
	}

	#endregion

	#region Private Methods

	void Start()
	{
		if (onSelected == null)
			onSelected = new LevelPackSelectedEvent();
	}

	/// <summary>
	/// Raises the onSelected event.
	/// </summary>
	private void OnSelected()
	{
		// Invoke all registered callbacks
		onSelected.Invoke(this.levelPackModel);
	}

	private void SetDisplay()
	{
		// Check if all orbs were collected and the
		// minimum moves achievement received
		if (AllOrbsCollected && MinimumMovesAchievement) 
		{
			SetAllOrbsAndMinimumMoves();
		}
		else if (AllOrbsCollected) 
		{
			// If all orbs collected - change background
			SetAllOrbsCollected();
		}
		else
		{
			if(MinimumMovesAchievement)
			{
				// If minimum moves achievement received -
				// change background
				SetMinimumMovesAchievement();
			}			

			// Update counters
			UpdateCounter();
		}
	}

	private void SetAllOrbsCollected()
	{
		// Change the achievement icon
		achievementImage.GetComponent<Image>().sprite = allOrbsCollectedBackground;
		achievementImage.gameObject.SetActive(true);
	}

	private void SetMinimumMovesAchievement()
	{
		// Change the background
		achievementImage.GetComponent<Image>().sprite = minimumMovesAchievementBackground;

		achievementImage.gameObject.SetActive(true);
	}

	private void SetAllOrbsAndMinimumMoves()
	{
		// Change the background
		achievementImage.GetComponent<Image>().sprite = movesAndOrbsAchievementBackground;
		achievementImage.gameObject.SetActive(true);
	}

	private void UpdateCounter()
	{
		// Update progress bar
		float progressBarValue = ((float)OrbsCollected / (float)OrbsCount);
		progressBarPanel.GetComponent<Image>().fillAmount = progressBarValue;
	}

	#endregion
}
