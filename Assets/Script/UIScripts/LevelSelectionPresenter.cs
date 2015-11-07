using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class LevelSelectionPresenter : MonoBehaviour
{
    #region Editor Variables

    public UIManager manager;

    public Text levelPackName, numberOfOrbs;
	public Text stageNumber;

    public Transform contentPanelFirstRow;
    public Transform contentPanelSecondRow;

    public GameObject levelPrefab;

    public List<int> levels;

    #endregion

    #region Private Fields

    private LevelPack levelPack;

    private List<GameObject> levelObjects = new List<GameObject>();

	// Holds an instance of the modal panel
	private PaymentModalPanel modalPanel;

	/// <summary>
	/// Holds the selected level. 
	/// This is used for the store when you want to unlock a level
	/// by buying orbs.
	/// </summary>
	private int selectedLevel;

    private const string ORBS_ID = "ten_orbs";
    #endregion

    #region Public Functions

    public void Init(LevelPack newLevelPack)
    {
        if (newLevelPack == null)
            throw new ArgumentNullException("levelPack");

        // Check the level pack has changed
        //if (levelPack != null && levelPack.LevelPackNumber == newLevelPack.LevelPackNumber)
        //    return;

        this.levelPack = newLevelPack;

        // Set the header
		Init();
    }

    public void Init()
    {
        // Set the header
        stageNumber.text = string.Format("{0}", levelPack.LevelPackNumber);
        levelPackName.text = string.Format("{0}", levelPack.Name);

        // Populate the levels collection with LevelPresenters
        this.CreateAllLevels();

        numberOfOrbs.text = LevelManager.manager.SumOfStarsGathered().ToString();
    }

    #endregion

    // Use this for initialization
    void Start()
    {
		// Obtain an instance of the modal panel
		modalPanel = PaymentModalPanel.GetInstance();
    }

    void OnEnable()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateAllLevels()
    {
        // Clean all previous
        if (levelObjects.Count != 0)
        {
            foreach (var levelObject in levelObjects)
            {
                Destroy(levelObject);
            }
            Clear();
        }
        int levelInStage = 0;
        levels = levelPack.Levels;
        foreach (var level in levels)
        {
            levelInStage++;
            var levelObject = Instantiate(levelPrefab);
            var levelPresenter = levelObject.GetComponent<LevelPresenter>();
            levelPresenter.Init(levelPack.LevelPackNumber, level);
            levelPresenter.onSelected.AddListener(OnLevelSelected);
            if (levelInStage <= 5)
            {
                levelObject.transform.SetParent(contentPanelFirstRow);
            }
            else
            {
                levelObject.transform.SetParent(contentPanelSecondRow);
            }
            levelObject.transform.localScale = Vector3.one;
            levelObjects.Add(levelObject);
        }
    }

    private void Clear()
    {
        foreach (var levelObject in levelObjects)
            Destroy(levelObject);

        levelObjects.Clear();
    }

    private void OnLevelSelected(int selectedLevel)
    {
		// Save currently selected level
		this.selectedLevel = selectedLevel;

		// Check the level is locked
		if (LevelManager.manager.isStagedUnlocked(selectedLevel)) 
		{
			// You can play the level
			manager.PlayLevel(selectedLevel);
		}
		else
		{
			// Let's earn some money!
			int orbsToUnlock = LevelManager.manager.orbsLeftToOpenLevel(selectedLevel);
			modalPanel.ShowModalWindow(
				String.Format("You need {0} more orbs to open this level.", orbsToUnlock),
				"Would you like to buy some?",
				"0.99$",
				new UnityAction(BuyOrbsToUnlockLevel),
				new UnityAction(CancelPurchase));
		}
    }

	private void BuyOrbsToUnlockLevel()
	{
        manager.ShowStore();
	}

	private void CancelPurchase()
	{

	}
}
