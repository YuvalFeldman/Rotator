using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace Soomla.Store.Rotator
{
    public class StoreManager : MonoBehaviour
    {
        public const string ORBS_ID = "ten_orbs";
        public const string OLD_ANIMATIONS_ID = "old_animations";

        // Initializes the Soomla Store
        void Start()
        {
            if (!SoomlaStore.Initialized)
            {
                // Initialize the store
                SoomlaStore.Initialize(new RotatorAssets());

                // Sign up to receive store events
                StoreEvents.OnMarketPurchase += OnMarketPurchase;
                StoreEvents.OnMarketPurchaseStarted += OnMarketPurchaseStarted;
                StoreEvents.OnItemPurchaseStarted += OnItemPurchaseStarted;
                StoreEvents.OnItemPurchased += OnItemPurchased;
                StoreEvents.OnUnexpectedErrorInStore += OnUnexpectedErrorInStore;

                Debug.Log("Successfully Initialized");
            }
        }

		public void OnMarketPurchaseStarted(PurchasableVirtualItem pvi) 
		{
			Debug.Log( "OnMarketPurchaseStarted: " + pvi.ItemId );
		}

        public void OnMarketPurchase(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> extra)
        {
            Debug.Log("This happened only after purchase was successful " + pvi.ID);
            unityAnalyticsPurchase(pvi, extra);

            switch (pvi.ID)
            {
                case ORBS_ID: 
                    GetComponent<StorePresenter>().PurchasedTenOrbs();
                    break;
                case OLD_ANIMATIONS_ID:
                    GetComponent<StorePresenter>().unlockYinYan();
                    break;
            }

            LevelManager.manager.StoreSave();
            LevelManager.manager.Load();
        }

		public void OnItemPurchaseStarted(PurchasableVirtualItem pvi) 
		{
			Debug.Log( "OnItemPurchaseStarted: " + pvi.ItemId );
		}
		
		public void OnItemPurchased(PurchasableVirtualItem pvi, string payload) 
		{
			Debug.Log( "OnItemPurchased: " + pvi.ItemId );
		}

		public void OnUnexpectedErrorInStore(string err) 
		{
			Debug.Log( "OnUnexpectedErrorInStore" + err );
		}

        private void unityAnalyticsPurchase(PurchasableVirtualItem io_PVI, Dictionary<string, string> io_Extra) {
#if UNITY_IPHONE
            bool onlyReceipt = extra.ContainsKey("receiptBase64");

            if (onlyReceipt)
            {
                Analytics.Transaction(pvi.ID, (decimal)0.99,
                    "US Dollar", extra["receiptBase64"],
                    null);
            } 
            else
            {
                Analytics.Transaction(pvi.ID, (decimal)0.99,
                    "US Dollar", null,
                    null);
            }
#endif

#if UNITY_ANDROID
            bool allDataPresent = io_Extra.ContainsKey("originalJson") && io_Extra.ContainsKey("signature");
            bool onlyReceipt = io_Extra.ContainsKey("originalJson");
            bool onlySignature = io_Extra.ContainsKey("signature");

            if (allDataPresent)
            {
                Analytics.Transaction(io_PVI.ID, (decimal)0.99,
                    "US Dollar", io_Extra["originalJson"],
                    io_Extra["signature"]);
            }
            else if (onlyReceipt)
            {
                Analytics.Transaction(io_PVI.ID, (decimal)0.99,
                    "US Dollar", io_Extra["originalJson"],
                    null);
            }
            else if (onlySignature)
            {
                Analytics.Transaction(io_PVI.ID, (decimal)0.99,
                    "US Dollar", null,
                    io_Extra["signature"]);
            }
            else
            {
                Analytics.Transaction(io_PVI.ID, (decimal)0.99,
                    "US Dollar", null,
                    null);
            }
#endif
        }
    }
}