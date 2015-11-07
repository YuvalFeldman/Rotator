using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System;

public class SwapButton : MonoBehaviour {

	private bool m_blackChosen = true;
	private Animator m_buttonAnimator;

    /// <summary>
    /// The animation for swapping characters
    /// </summary>
    private RuntimeAnimatorController m_SwapAnimator;


    private Sprite m_StartingSprite;

	// Use this for initialization
	void Start () {
        initializeStartingSprites();
        initializeSwapAnimations();
        this.gameObject.GetComponent<Image>().sprite = m_StartingSprite;
		m_buttonAnimator = this.gameObject.GetComponent<Animator>();
        m_buttonAnimator.runtimeAnimatorController = m_SwapAnimator;
	}

    /// <summary>
    /// Initialize the swapping animation for the button
    /// </summary>
    private void initializeSwapAnimations()
    {
        m_SwapAnimator = Resources.Load("RunTimeAnimations/SwapAnimation/" + Enum.GetName(typeof(eAnimationSkin),
                            LevelManager.manager.AnimationSkin)) as RuntimeAnimatorController;
    }

    /// <summary>
    /// Initialize Starting Sprite for the button
    /// </summary>
    private void initializeStartingSprites()
    {
        m_StartingSprite = Resources.Load<Sprite>("StartingSprite/" + Enum.GetName(typeof(eAnimationSkin),
                            LevelManager.manager.AnimationSkin));
    }
	
	// Update is called once per frame
	public void Swap () {
		if(m_blackChosen){
			m_buttonAnimator.SetBool("SwapToBlack", false);
			m_buttonAnimator.SetBool("SwapToWhite", true);
			m_blackChosen = false;
		} else {
			m_buttonAnimator.SetBool("SwapToWhite", false);
			m_buttonAnimator.SetBool("SwapToBlack", true);
			m_blackChosen = true;
		}
	}
}
