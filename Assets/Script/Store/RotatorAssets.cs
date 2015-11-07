using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

/// <summary>
/// This class defines the game's economy.
/// </summary>
public class RotatorAssets : IStoreAssets
{
    /**
     * IStoreAssets - Interface implementations
     */
    public int GetVersion()
    {
        return 3;
    }

    public VirtualCurrency[] GetCurrencies()
    {
        return new VirtualCurrency[] { };
    }

    public VirtualGood[] GetGoods()
    {
        return new VirtualGood[] { TEN_ORBS, ORIGINAL_ANIMATIONS };
    }

    public VirtualCurrencyPack[] GetCurrencyPacks()
    {
        return new VirtualCurrencyPack[] { };
    }

    public VirtualCategory[] GetCategories()
    {
        return new VirtualCategory[] { };
    }

    public const string ORBS_ID = "ten_orbs";
    public const string OLD_ANIMATIONS_ID = "old_animations";

	#region Virtual Goods

	/// <summary>
	/// A LifeTimeVG is a virtual good that is bought once and kept forever.
	/// The LifeTimeVG's characteristics are:
	/// 1. Can only be purchased once.
	/// 2. Your users cannot have more then one of this item.
	/// 
	/// Buy:
	/// StoreInventory.Buy(OLD_ANIMATIONS_ID);
	/// 
	/// Check ownership:
	/// int balance = StoreInventory.GetItemBalance(OLD_ANIMATIONS_ID);
	/// if(balance >0) { The user owns the Old Animations skin }
	/// </summary>
    public static VirtualGood ORIGINAL_ANIMATIONS = new LifetimeVG(
        "Original Animations",
        "Enjoy the original animations of YinYan, before they were awesome",
        OLD_ANIMATIONS_ID,
        new PurchaseWithMarket(OLD_ANIMATIONS_ID, 2.99));

	/// <summary>
	/// The SingleUseVG's characteristics are:
	/// 1. Can be purchased an unlimited number of times.
	/// 2. Has a balance that is saved in the database. Its balance goes up when you "give" it or
	/// "buy" it. The balance goes down when you "take" or "refund" it.
	/// 
	/// To Buy: 
	/// StoreInventory.BuyItem(ORBS_ID);
	/// 
	/// Get the Balance:
	/// StoreInventory.GetItemBalance(ORBS_ID);
	/// </summary>
    public static SingleUseVG TEN_ORBS = new SingleUseVG(
		"Ten Orbs",
        "Pack of ten orbs",
        ORBS_ID,
        new PurchaseWithMarket(ORBS_ID, 2.99));

	#endregion
}