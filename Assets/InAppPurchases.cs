using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class InAppPurchases : MonoBehaviour, IStoreListener {
	private static IStoreController m_StoreController;
	// The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider;
	// The store-specific Purchasing subsystems.

	public static string kProductIDConsumable = "exp55555";
	public static string kProductIDNonConsumableAll = "unlock_all_worlds_and_blocks";
	public static string kProductIDNonConsumableWorld2 = "world_2_unlock";
	public static string kProductIDNonConsumableWorld3 = "world_3_unlock";
	public static string kProductIDNonConsumableWorld4 = "world_4_unlock";

	void Start () {
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null) {
			// Begin to configure our connection to Purchasing
			InitializePurchasing ();
		}
	}

	public void InitializePurchasing () {
		// If we have already connected to Purchasing ...
		if (IsInitialized ()) {
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());

		// Add a product to sell / restore by way of its identifier, associating the general identifier
		// with its store-specific identifiers.
		builder.AddProduct (kProductIDConsumable, ProductType.Consumable);
		// Continue adding the non-consumable product.
		builder.AddProduct (kProductIDNonConsumableAll, ProductType.NonConsumable);
		builder.AddProduct (kProductIDNonConsumableWorld2, ProductType.NonConsumable);
		builder.AddProduct (kProductIDNonConsumableWorld3, ProductType.NonConsumable);
		builder.AddProduct (kProductIDNonConsumableWorld4, ProductType.NonConsumable);

		// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
		// and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
		UnityPurchasing.Initialize (this, builder);
	}

	private bool IsInitialized () {
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void BuyConsumable () {
		BuyProductID (kProductIDConsumable);
	}

	public void BuyNonConsumableAll () {
		BuyProductID (kProductIDNonConsumableAll);
	}

	public void BuyNonConsumableWorld2 () {
		BuyProductID (kProductIDNonConsumableWorld2);
	}

	public void BuyNonConsumableWorld3 () {
		BuyProductID (kProductIDNonConsumableWorld3);
	}

	public void BuyNonConsumableWorld4 () {
		BuyProductID (kProductIDNonConsumableWorld4);
	}
		
	void BuyProductID (string productId) {
		// If Purchasing has been initialized ...
		if (IsInitialized ()) {
			// ... look up the Product reference with the general product identifier and the Purchasing 
			// system's products collection.
			Product product = m_StoreController.products.WithID (productId);

			// If the look up found a product for this device's store and that product is ready to be sold ... 
			if (product != null && product.availableToPurchase) {
				Debug.Log (string.Format ("Purchasing product asychronously: '{0}'", product.definition.id));
				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
				// asynchronously.
				m_StoreController.InitiatePurchase (product);
			}
			// Otherwise ...
			else {
				// ... report the product look-up failure situation  
				Debug.Log ("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		// Otherwise ...
		else {
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
			// retrying initiailization.
			Debug.Log ("BuyProductID FAIL. Not initialized.");
		}
	}

	public void checkReceipts () {
		if (IsInitialized ()) {
			Product allWorlds = m_StoreController.products.WithID (kProductIDNonConsumableAll);
			Product world2 = m_StoreController.products.WithID (kProductIDNonConsumableWorld2);
			Product world3 = m_StoreController.products.WithID (kProductIDNonConsumableWorld3);
			Product world4 = m_StoreController.products.WithID (kProductIDNonConsumableWorld4);

			if (allWorlds != null && allWorlds.hasReceipt) {
				for (int i = 0; i < 50; i++) {
					PlayerPrefs.SetInt("World" + i, 1);
				}
				PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 1);
			}
			if (world2 != null && world2.hasReceipt) {
				PlayerPrefs.SetInt("World0", 1);
				PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 1);
			}
			if (world3 != null && world3.hasReceipt) {
				PlayerPrefs.SetInt("World1", 1);
				PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 1);
			}
			if (world4 != null && world4.hasReceipt) {
				PlayerPrefs.SetInt("World2", 1);
				PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 1);
			}
		}
	}

	// Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google.
	// Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
	public void RestorePurchases () {
		// If Purchasing has not yet been set up ...
		if (!IsInitialized ()) {
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log ("RestorePurchases FAIL. Not initialized.");
			return;
		}

		// If we are running on an Apple device ... 
		if (Application.platform == RuntimePlatform.IPhonePlayer ||
			Application.platform == RuntimePlatform.OSXPlayer) {
			// ... begin restoring purchases
			Debug.Log ("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions> ();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
			// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions ((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then 
				// no purchases are available to be restored.
				Debug.Log ("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		// Otherwise ...
		else {
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log ("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	//
	// --- IStoreListener
	//

	public void OnInitialized (IStoreController controller, IExtensionProvider extensions) {
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log ("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}
		
	public void OnInitializeFailed (InitializationFailureReason error) {
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log ("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs args) {
		if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumableAll, StringComparison.Ordinal)) {
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
			for (int i = 0; i < 50; i++) {
				PlayerPrefs.SetInt("World" + i, 1);
			}
			PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 1);
			if (GetComponent<MainMenuInterface>() != null) {
				GetComponent<MainMenuInterface>().boughtItem();
			} else if (GetComponent<EditorInterface>() != null) {
				GetComponent<EditorInterface>().unlockedWorld();
			}
		} else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumableWorld2, StringComparison.Ordinal)) {
			PlayerPrefs.SetInt("World0", 1);
			PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 1);
			if (GetComponent<MainMenuInterface>() != null) {
				GetComponent<MainMenuInterface>().boughtItem();
			} else if (GetComponent<EditorInterface>() != null) {
				GetComponent<EditorInterface>().unlockedWorld();
			}
		} else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumableWorld3, StringComparison.Ordinal)) {
			PlayerPrefs.SetInt("World1", 1);
			PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 1);
			if (GetComponent<MainMenuInterface>() != null) {
				GetComponent<MainMenuInterface>().boughtItem();
			} else if (GetComponent<EditorInterface>() != null) {
				GetComponent<EditorInterface>().unlockedWorld();
			}
		} else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumableWorld4, StringComparison.Ordinal)) {
			PlayerPrefs.SetInt("World2", 1);
			PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 1);
			if (GetComponent<MainMenuInterface>() != null) {
				GetComponent<MainMenuInterface>().boughtItem();
			} else if (GetComponent<EditorInterface>() != null) {
				GetComponent<EditorInterface>().unlockedWorld();
			}
		}
		// Or ... an unknown product has been purchased by this user. Fill in additional products here....
		else {
			Debug.Log (string.Format ("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
		}

		// Return a flag indicating whether this product has completely been received, or if the application needs 
		// to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
		// saving purchased products to the cloud, and when that save is delayed. 
		return PurchaseProcessingResult.Complete;
	}


	public void OnPurchaseFailed (Product product, PurchaseFailureReason failureReason) {
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		Debug.Log (string.Format ("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}
}
