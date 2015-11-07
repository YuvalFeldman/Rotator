using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	#region Editor Variables
	
	public SpriteRenderer spriteBounds;
	
	public GameObject hexChart;
	
	public float cameraPanningSpeed = 1F;
	
	#endregion

	#region Private Fields

	private float moveSensetivityX = 4.0F;

	private float moveSensetivityY = 4.0F;

	private Camera _camera;

	private float rightBound;

	private float leftBound;

	private float topBound;

	private float bottomBound;

	private Vector3 pos;

	private Transform target;

	float gridWidth;

	float gridHeight;

	#endregion

	#region Methods

	// Use this for initialization
	void Start () {
		_camera = Camera.main;
		Vector2 endOfTheGrid = hexChart.GetComponent<HexChart>().get_cameraParameters();
		gridWidth = endOfTheGrid.x;
		gridHeight = endOfTheGrid.y;
		//Debug.Log ("Max Width: " + hexChart.GetComponent<HexChart>().get_cameraParameters().x);
		//Debug.Log ("Max Height: " + hexChart.GetComponent<HexChart>().get_cameraParameters().y);
		_camera.transform.position = new Vector3 (gridWidth/2, gridHeight/2, transform.position.z);
		setBounds(1f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		Touch[] touches = Input.touches;
		if (touches.Length > 0) {
			if(touches[0].phase == TouchPhase.Moved){
				Vector2 delta = touches[0].deltaPosition;
				_camera.transform.Translate(-delta.x * cameraPanningSpeed * Time.deltaTime, -delta.y * cameraPanningSpeed * Time.deltaTime, 0);
				_camera.transform.position = new Vector3(
									Mathf.Clamp(_camera.transform.position.x, leftBound, rightBound),
									Mathf.Clamp(_camera.transform.position.y, 2*bottomBound, 2*topBound),
									_camera.transform.position.z);
			}
		}
	}

	#endregion

	#region Private Methods

	private void setBounds(float verticalMultiplier, float horizontalMultiplier){
		leftBound = - (gridWidth/2) * verticalMultiplier;
		rightBound =  gridWidth * 1.5f * verticalMultiplier;
		topBound = - (gridHeight /2) * horizontalMultiplier;
		bottomBound =  gridHeight * 1.5f * horizontalMultiplier;
	}

	#endregion
}
