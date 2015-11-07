using UnityEngine;
using System.Collections;

public class starScript : MonoBehaviour {
	private int starXPos;
	private int starYPos;
	private GameObject whiteCat, blackCat;
	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Finished")) {
			//LevelManager.manager.numOfStarsGathered++;
			stepstarcounter.starsCollected++;
			Destroy (gameObject);
		}
	}

	public void set_XY(int x, int y){
		starXPos = x;
		starYPos = y;
	}
	public int get_XpositionInChart(){
		return starXPos;
	}
	public int get_YpositionInChart(){
		return starYPos;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
	    if (!other.GetComponent<Movement>().collectingStar) return;
	    SoundManager.soundManager.playEatOrbSound();
	    anim.SetBool("Collect", true);
	}
}
