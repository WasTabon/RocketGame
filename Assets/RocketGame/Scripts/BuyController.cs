using PowerLines.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class BuyController : MonoBehaviour
{
    private string _donateId = "com.rocketpost.pack01";

    public GameObject loadingButton;
    public AudioClip buySound;
    public TextMeshProUGUI buttonText;
    public GameObject panel;

    public void OnPurchaseComplete(Product product)
    {
        try
        {
            if (product?.definition?.id == null)
            {
                Debug.LogError("OnPurchaseComplete: product or product.definition.id is null");
                return;
            }

            if (product.definition.id == _donateId)
            {
                Debug.Log("Purchase complete for product: " + product.definition.id);

                if (RocketHubController.Instance == null)
                    Debug.LogError("RocketHubController.Instance is null");
                else
                    RocketHubController.Instance.money += 500;

                if (MusicController.Instance == null)
                    Debug.LogError("MusicController.Instance is null");
                else if (buySound == null)
                    Debug.LogError("buySound is null");
                else
                    MusicController.Instance.PlaySpecificSound(buySound);

                if (loadingButton != null)
                    loadingButton.SetActive(false);
                else
                    Debug.LogError("loadingButton is null");

                if (panel != null)
                    panel.SetActive(true);
                else
                    Debug.LogError("panel is null");
            }
            else
            {
                Debug.LogWarning($"OnPurchaseComplete: Unexpected product ID: {product.definition.id}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"OnPurchaseComplete Exception: {ex}");
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription description)
    {
        try
        {
            string productId = product?.definition?.id;

            if (string.IsNullOrEmpty(productId))
            {
                Debug.LogError("OnPurchaseFailed: product or product.definition.id is null");
                return;
            }

            if (productId == _donateId)
            {
                if (loadingButton != null)
                    loadingButton.SetActive(false);
                else
                    Debug.LogError("loadingButton is null during OnPurchaseFailed");

                if (description != null)
                    Debug.LogError($"Purchase failed: {description.reason} - {description.message}");
                else
                    Debug.LogError("Purchase failed: description is null");
            }
            else
            {
                Debug.LogWarning($"OnPurchaseFailed: Unexpected product ID: {productId}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"OnPurchaseFailed Exception: {ex}");
        }
    }

    public void OnProductFetched(Product product)
    {
        try
        {
            if (product == null || product.metadata == null)
            {
                Debug.LogError("OnProductFetched: product or product.metadata is null");
                return;
            }

            Debug.Log("Product fetched: " + product.definition.id);

            if (buttonText != null)
                buttonText.text = product.metadata.localizedPriceString;
            else
                Debug.LogError("buttonText is null in OnProductFetched");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"OnProductFetched Exception: {ex}");
        }
    }
}
