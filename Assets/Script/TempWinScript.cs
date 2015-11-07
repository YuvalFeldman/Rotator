using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class TempWinScript : MonoBehaviour {
	public GameObject blackcat;
	public GameObject whitecat;
	public AudioClip winSound;

	public bool win = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!win &&
		    blackcat.GetComponent<Movement> ().get_XPositionInChart () == 
		    whitecat.GetComponent<Movement> ().get_XPositionInChart () &&
		    blackcat.GetComponent<Movement> ().get_YPositionInChart () == 
		    whitecat.GetComponent<Movement> ().get_YPositionInChart ()) {
			MakeSound(winSound);
			Debug.Log ("hi");
			win = true;
		}
	}
	private void MakeSound(AudioClip originalClip)
	{
		// As it is not 3D audio clip, position doesn't matter.
		AudioSource.PlayClipAtPoint(originalClip, transform.position);
	}
}
