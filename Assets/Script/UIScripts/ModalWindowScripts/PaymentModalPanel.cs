using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class PaymentModalPanel : MonoBehaviour 
{
	public Text buyMainText;
	public Text itemName;
	public Text itemPrice;

	public Button buyButton;

	public Button cancelButton;

	public GameObject modalPanelObject;

	#region Singleton

	// Holds the only instance of this object
	private static PaymentModalPanel instance;

	public static PaymentModalPanel GetInstance()
	{
		if (!instance) 
		{
			instance = FindObjectOfType(typeof (PaymentModalPanel)) as PaymentModalPanel;
			if (!instance)
				Debug.LogError ("There needs to be one active ModalPanel script on a GameObject in your scene.");
		}

		return instance;
	}

	#endregion

	#region Functions

	public void ShowModalWindow(
		string mainText, 
		string subText,
		string itemPrice,
		UnityAction buyCommand, 
		UnityAction cancelCommand)
	{
		modalPanelObject.SetActive (true);
		
		buyButton.onClick.RemoveAllListeners();
		buyButton.onClick.AddListener (buyCommand);
		buyButton.onClick.AddListener (ClosePanel);
		
		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener (cancelCommand);
		cancelButton.onClick.AddListener (ClosePanel);
		
		this.buyMainText.text = mainText;
		this.itemName.text = subText;
		this.itemPrice.text = itemPrice;
		
		buyButton.gameObject.SetActive (true);
		cancelButton.gameObject.SetActive (true);
	}

	#endregion

	#region Functions

	/// <summary>
	/// Closes the panel when you're done executing the button event.
	/// </summary>
	private void ClosePanel () 
	{
		modalPanelObject.SetActive (false);
	}

	#endregion
}
