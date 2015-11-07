using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using Soomla.Store;
using System;


/// <summary>
/// Manages the UI logic for the Store canvas. 
/// </summary>
public class StorePresenter : MonoBehaviour
{
    #region Variables

    public Text yinYanButtonText;

    private PaymentModalPanel modalPanel;

    private string confirmationString;

    private UnityAction buyAction;

    // We do noting on cancel
    private UnityAction cancelAction;

    public const string ORBS_ID = "ten_orbs";

    public const string OLD_ANIMATIONS_ID = "old_animations";

    #endregion

    #region Public Functions

    public void BuyOrbsCommand()
    {
        // Show modal panel to confirm purchase
        buyAction = new UnityAction(PurchaseOrbs);
        modalPanel.ShowModalWindow(
            confirmationString,
            "Orbs",
            "0.99$",
            buyAction,
            cancelAction);
    }

    public void BuyFlameVanityCommand()
    {
        // Show modal panel to confirm purchase
        buyAction = new UnityAction(PurchaseYinYanVanity);
        modalPanel.ShowModalWindow(
            confirmationString,
            "Flame Design",
            "0.99$",
            buyAction,
            cancelAction);
    }


    public void BuyYinYanCommand()
    {
        // Show modal panel to confirm purchase
        buyAction = new UnityAction(PurchaseYinYan);
        string buyButtonText = "0.99$";
        switch (yinYanButtonText.text)
        {
            case "0.99$":
                modalPanel.ShowModalWindow(
                    confirmationString,
                    "YinYan",
                    "0.99$",
                    buyAction,
                    cancelAction);
                PurchaseYinYanVanity();
                break;

            case "Activate Skin":
                LevelManager.manager.AnimationSkin = eAnimationSkin.YinYan;
                PurchaseYinYanVanity();
                break;

            case "Deactivate Skin":
                LevelManager.manager.AnimationSkin = eAnimationSkin.Rotator;
                PurchaseYinYanVanity();
                break;
        }
    }

    #endregion

    // Use this for initialization
    void Start()
    {
        // Get a modal panel to use for purchase confirmation
        modalPanel = PaymentModalPanel.GetInstance();

        confirmationString = "Are you sure you want to buy this item?";

        // Do nothing for now
        cancelAction = new UnityAction(Cancel);

        PurchaseYinYanVanity();
    }

    public void PurchaseOrbs()
    {
        try
        {
            StoreInventory.BuyItem(ORBS_ID);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void PurchasedTenOrbs()
    {
        LevelManager.manager.TotalPaidForStars += 10;
    }

    public void PurchaseYinYanVanity()
    {
        string buyButtonText = "0.99$";

        if (LevelManager.manager.YinYanUnlocked)
        {
            if (LevelManager.manager.AnimationSkin == eAnimationSkin.Rotator)
            {
                buyButtonText = "Activate Skin";
            }
            else
            {
                buyButtonText = "Deactivate Skin";
            }
            yinYanButtonText.text = buyButtonText;
        }
        else
        {
            yinYanButtonText.text = buyButtonText;
        }
    }

    public void PurchaseYinYan()
    {
        try
        {
            StoreInventory.BuyItem(OLD_ANIMATIONS_ID);   
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            PurchaseYinYanVanity();
        }
    }

    public void unlockYinYan()
    {
        LevelManager.manager.YinYanUnlocked = true;
    }

    private void Cancel()
    {

    }
}
