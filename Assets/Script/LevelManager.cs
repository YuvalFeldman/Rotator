using UnityEngine;
using System.Collections;
using System; 
using System.IO; 
using System.Runtime.Serialization.Formatters.Binary;

public class LevelManager : MonoBehaviour {
	public static LevelManager manager;
	//-----Level Array Here-----
 
    public eAnimationSkin AnimationSkin { get; set; }
	public int numOfStarsGathered;
	public int numOfMovesMade;
	public int currentLevel;
    private bool gameSoundMuted;
	private int[] amountOfStarsAvailablePerLevel = {0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3};
	private int[] amountOfMovesAvailablePerLevel = {0, 4, 10, 6, 23, 10, 16, 9, 13, 20, 10, 16, 24, 25, 30, 25, 1, 6, 26, 16, 14};
	private int[] minAmountOfMovesAvailablePerLevel = {0, 2, 8, 4, 6, 4, 7, 4, 6, 15, 5, 12, 20, 12, 18, 23, 1, 4, 20, 12, 12};
    private int[] orbsNeededToUnlockLevel = { 0, 0, 0, 0, 2, 4, 5, 7, 9, 10, 11, 12, 14, 16, 17, 18, 20, 21, 22, 24, 25, 0, 0 };
    //Uncomment instead of line above to unlock all levels
    //private int[] orbsNeededToUnlockLevel = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    private bool[] skinsUnlocked;
	private bool[] levelFinished;
	private int[] numOfStarsGatheredAllLevels;
	private int[] numOfMovesMadeAllLevels;
	
	private string fileEnding = ".sat";

	public int TotalNumberOfLevels = 20;
	
	void Awake () {
        YinYanUnlocked = false;
        AnimationSkin = (eAnimationSkin)0;
		numOfStarsGatheredAllLevels = new int[41];
		numOfMovesMadeAllLevels = new int[41];
		if(manager == null){
			DontDestroyOnLoad(gameObject);
			manager = this;
		} else if(manager != this){
			Destroy(gameObject);
		}
		for(int i = 0; i < 41; i++){
			numOfStarsGatheredAllLevels[i] = 0;
			numOfMovesMadeAllLevels[i] = 100;
		}
		currentLevel = 7;
		Load();
	}

    public bool YinYanUnlocked { get; set; }
	
	public void Save(int levelNum, int numOfStarsGathered, int numOfMovesMade)
	{	
			BinaryFormatter bFormatter = new BinaryFormatter();

			FileStream file = new FileStream(Application.persistentDataPath + "/levelInfo" + fileEnding,
		                              FileMode.OpenOrCreate, 
		                              FileAccess.ReadWrite, 
		                              FileShare.None);

			LevelData dataToSave = new LevelData();

			dataToSave.levelFinished = this.levelFinished;
			dataToSave.numOfStarsGathered = this.numOfStarsGatheredAllLevels;
			dataToSave.numOfMovesMade = this.numOfMovesMadeAllLevels;

			if(numOfStarsGathered >= dataToSave.numOfStarsGathered[levelNum]){
				dataToSave.numOfStarsGathered[levelNum] = numOfStarsGathered;
			}

			if(numOfMovesMade < dataToSave.numOfMovesMade[levelNum]){
				dataToSave.numOfMovesMade[levelNum] = numOfMovesMade;
			}
			dataToSave.levelFinished[levelNum] = true;
            if (YinYanUnlocked != null)
            {
                dataToSave.skinsUnlocked[1] = YinYanUnlocked;
            }
            dataToSave.skinsUnlocked[0] = true;

			bFormatter.Serialize(file, dataToSave);
			file.Close();
	}

    /// <summary>
    /// Save used only for the store
    /// </summary>
    public void StoreSave()
    {
        BinaryFormatter bFormatter = new BinaryFormatter();
        FileStream file = new FileStream(Application.persistentDataPath + "/levelInfo" + fileEnding,
                                  FileMode.OpenOrCreate,
                                  FileAccess.ReadWrite,
                                  FileShare.None);
        LevelData dataToSave = new LevelData();

        dataToSave.numOfStarsGathered = this.numOfStarsGatheredAllLevels;
        dataToSave.skinsUnlocked[1] = YinYanUnlocked;

        bFormatter.Serialize(file, dataToSave);
        file.Close();
    }

	public void Load()
	{
		resetGathered();
		if(File.Exists(Application.persistentDataPath + "/levelInfo"  + fileEnding)){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/levelInfo" + fileEnding, FileMode.Open);

			LevelData dataToLoad = (LevelData)bf.Deserialize(file);

			this.levelFinished = dataToLoad.levelFinished;
			this.numOfStarsGatheredAllLevels = dataToLoad.numOfStarsGathered;
			this.numOfMovesMadeAllLevels = dataToLoad.numOfMovesMade;
            if (dataToLoad.skinsUnlocked != null)
            {
                skinsUnlocked = dataToLoad.skinsUnlocked;
            }
            if (dataToLoad.skinsUnlocked[1] != null)
            {
                YinYanUnlocked = dataToLoad.skinsUnlocked[1];
            }
            if (dataToLoad.currentlyInUse != null)
            {
                AnimationSkin = dataToLoad.currentlyInUse;
            }

			file.Close();
		} else {
			CreateNewSaveData();
		}
	}
	
	public void CreateNewSaveData(){

		BinaryFormatter bFormatter = new BinaryFormatter();
		
		FileStream file = new FileStream(Application.persistentDataPath + "/levelInfo" + fileEnding,
		                                 FileMode.OpenOrCreate, 
		                                 FileAccess.ReadWrite, 
		                                 FileShare.None);
		
		LevelData dataToSave = new LevelData();
        dataToSave.skinsUnlocked[0] = true;

		for(int i = 1; i < 41; i++){
            dataToSave.skinsUnlocked[i] = false;
			dataToSave.numOfStarsGathered[i] = 0;
			if(i < amountOfMovesAvailablePerLevel.Length) {
				dataToSave.numOfMovesMade[i] = amountOfMovesAvailablePerLevel[i];
			} else {
				dataToSave.numOfMovesMade[i] = 100;
			}
			dataToSave.levelFinished[i] = false;
		}
        
		this.levelFinished = dataToSave.levelFinished;
		bFormatter.Serialize(file, dataToSave);
		file.Close();
	}

    public bool SoundFxMuted { get; set; }
    public bool MusicMuted { get; set; }

    public bool AllSoundsMuted
    {
        get
        {
            return gameSoundMuted;
        }

        set
        {
            gameSoundMuted = value;
            MusicMuted = value;
            SoundFxMuted = value;
        }
    }

    private void resetGathered(){
		numOfStarsGathered = 0;
		numOfMovesMade = 0;
	}

	public void ResetAll(){
		CreateNewSaveData();
	}

	public int SumOfStarsGathered(){
		int sum = 0;
		Array.ForEach(numOfStarsGatheredAllLevels, delegate(int i) { sum += i; });
        return sum;
	}

    public int TotalPaidForStars
    {
        get
        {
            return numOfStarsGatheredAllLevels[0];
        }
        set
        {
            numOfStarsGatheredAllLevels[0] = value;
        }
    }

	public int CollectedNumberOfStarsForAGivenLevel(int levelNum){
		return numOfStarsGatheredAllLevels[levelNum];
	}

	public bool FinishedLevelWithAllStars(int levelNum){
		return (numOfStarsGatheredAllLevels[levelNum] == amountOfStarsAvailablePerLevel[levelNum]);
	}

	public bool FinishedLevelWithMinimumMoves(int levelNum){
		return (numOfMovesMadeAllLevels[levelNum] == minAmountOfMovesAvailablePerLevel[levelNum]);
	}

	public bool StageFinishedWithMinimumMoves(int stage){
		if (stage > 2) {
			return false;
		}
		int minimum = (stage - 1) * 10 + 1;
		int maximum = stage * 10;
		for(int current = minimum; current <= maximum; current++){
			if(numOfMovesMadeAllLevels[current] != minAmountOfMovesAvailablePerLevel[current]){
				return false;
			}
		}
		return true;
	}

	public bool StageFinishedWithAllStars(int stage){
		if (stage > 2) {
			return false;
		}
		int minimum = ((stage - 1) * 10 + 1);
		int maximum = stage * 10;
		for(int current = minimum; current <= maximum; current++){
			if(numOfStarsGatheredAllLevels[current] != amountOfStarsAvailablePerLevel[current]){
				return false;
			}
		}
		return true;
	}

	public bool wasLevelFinished(int levelNumber){
		return levelFinished[levelNumber];
	}

	public int orbsLeftToOpenLevel(int levelNum){
		return orbsNeededToUnlockLevel[levelNum] - SumOfStarsGathered();
	}

	public bool isStagedUnlocked(int levelNumber){
		return SumOfStarsGathered() >= orbsNeededToUnlockLevel[levelNumber];
	}
	public int numberOfStarsCollectedInStage(int stage){
		int starsCollected = 0;
		/*for (int i = ((stage - 1) * 10 + 1); i < stage * 10; i++) {
			starsCollected += CollectedNumberOfStarsForAGivenLevel(i);
		}
		return starsCollected;*/

		switch (stage) {
		case 1:
			for(int i = 1; i <= 10; i++){
				starsCollected += CollectedNumberOfStarsForAGivenLevel(i);
			}
			break;
		case 2:
			for(int i = 11; i <= 20; i++){
				starsCollected += CollectedNumberOfStarsForAGivenLevel(i);
			}
			break;
		}
		return starsCollected;
	}
	public int numberOfStarsTotalInStage(int stage){
		/*if (stage > 2) {
			return 1;
		}
		int totalStars = 0;
		for (int i = ((stage - 1) * 10 + 1); i <= stage * 10; i++) {
			totalStars += amountOfStarsAvailablePerLevel[i];
		}
		return totalStars;*/
		switch(stage){
		case 1:
			return 24;
		case 2:
			return 27;
		default:
			return 1;
		}
	}
}		


[Serializable]
class LevelData{
	public int[] numOfStarsGathered = new int[41];
	public int[] numOfMovesMade = new int[41];
	public bool[] levelFinished = new bool[41];
    public bool[] skinsUnlocked = new bool[41];
    public eAnimationSkin currentlyInUse;
}