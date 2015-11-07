using UnityEngine;
using System.Collections;

public class BringToFront : MonoBehaviour {

	void OnEnable () 
	{
		// Makes the object to be on top
		transform.SetAsLastSibling ();
	}
}
