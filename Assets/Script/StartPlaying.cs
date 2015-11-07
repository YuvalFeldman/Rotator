using UnityEngine;
using System.Collections;
using System.IO;

public class StartPlaying : MonoBehaviour {
	public GameObject hexGrid;
	private int[][] theHexChart;
    private bool isProcessing = false;

	// Use this for initialization
	void Awake () {
		if (LevelManager.manager.currentLevel <= 20) {
			theHexChart = 
				CSVReader.CSVmanager.Read ("Level" + LevelManager.manager.currentLevel); //Change Level to file name
		}
		HexVariables.narratorNeedsToSpeek = true;
		HexVariables.narratorDoneSpeaking = false;

	}

	void Start(){
		LevelManager.manager.numOfMovesMade = 0;
		LevelManager.manager.numOfStarsGathered = 0;

		if (LevelManager.manager.currentLevel <= 20) {
			hexGrid.GetComponent<HexChart> ().createLevel (theHexChart);
		} else {
			hexGrid.GetComponent<HexChart> ().commingSoonOpenMenu();
		}
	}

	public void Restart(){
		SoundManager.soundManager.playMenuButtonSound ();
		Application.LoadLevel(Application.loadedLevel);
	}

	public void Next(){
		SoundManager.soundManager.playMenuButtonSound ();
		LevelManager.manager.currentLevel++;
		LevelManager.manager.Load();
		Application.LoadLevel(Application.loadedLevel);
	}

	public void BackToMenu(){
		SoundManager.soundManager.playMenuButtonSound ();
		Application.LoadLevel("Menu");
	}
	public void returnToGame(){
		SoundManager.soundManager.playMenuButtonSound ();
		hexGrid.GetComponent<IngameScript> ().deactivateIngameMenu ();
	}
	public void openPauseCanvas(){
		SoundManager.soundManager.playMenuButtonSound ();
		hexGrid.GetComponent<IngameScript> ().activateIngameMenu (false);
	}

	public void Share() {
        if (!isProcessing) 
        {
            StartCoroutine(ShareScreenshot());
        }
    }
    
    /*
		SoundManager.soundManager.playMenuButtonSound ();
		#if UNITY_ANDROID
			Application.CaptureScreenshot("shot.png");
			string filename = "/shot.png";
			string destination = System.IO.Path.Combine (Application.persistentDataPath, filename);
			Debug.Log (destination);
			// block to open the file and share it ------------START
			AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
			AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse","file://" + destination);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
			//intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "testo");
			//intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");
			intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
			AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
			
			// option one:
			currentActivity.Call("startActivity", intentObject);
		#endif
	*/

    public IEnumerator ShareScreenshot()
    {
        isProcessing = true;

        // wait for graphics to render
        yield return new WaitForEndOfFrame();
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
        // create the texture
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);

        // put buffer into texture
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);

        // apply
        screenTexture.Apply();
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO

        byte[] dataToSave = screenTexture.EncodeToPNG();

        string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");

        File.WriteAllBytes(destination, dataToSave);

        if (!Application.isEditor)
        {
            // block to open the file and share it ------------START
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "testo");
            //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");
            intentObject.Call<AndroidJavaObject>("setType", "image/png");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            // option one WITHOUT chooser:
            //currentActivity.Call("startActivity", intentObject);

            // option two WITH chooser:
            AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "YO BRO! WANNA SHARE?");
            currentActivity.Call("startActivity", jChooser);

            // block to open the file and share it ------------END

        }
        isProcessing = false;
    }

	public void Rate(){
		SoundManager.soundManager.playMenuButtonSound ();
		Application.OpenURL("market://details?id="+"com.papaya.MeowangMeowyng");
	}
}
