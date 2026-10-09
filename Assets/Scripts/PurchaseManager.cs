using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;
using Unity.Services.Core;
using Doozy.Engine;
using Dices;
using Zenject;

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    [Inject]
    Dices.UIConnection.DonationManager _donationManager;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    private int currentProductIndex;
    private bool _isInitializing;

    [Tooltip("Многоразовые товары. Больше подходит для покупки игровой валюты и т.п.")]
    public string[] C_PRODUCTS;

    [Tooltip("Режим окна Fake Store в редакторе. StandardUser — окно подтверждения покупки, DeveloperUser — выбор успеха/причины ошибки, Default — без окна.")]
    public FakeStoreUIMode fakeStoreUIMode = FakeStoreUIMode.DeveloperUser;

    // Сброс статических полей при входе в Play Mode (важно, если отключен Domain Reload).
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        m_StoreController = null;
        m_StoreExtensionProvider = null;
    }

    private async void Start()
    {
        await InitializePurchasingAsync();
    }

    /// <summary>
    /// Проверить, куплен ли товар.
    /// </summary>
    public static bool CheckBuyState(string id)
    {
        if (m_StoreController == null) return false;
        Product product = m_StoreController.products.WithID(id);
        return product != null && product.hasReceipt;
    }

    // Оставлено для совместимости с существующими вызовами.
    public void InitializePurchasing()
    {
        _ = InitializePurchasingAsync();
    }

    public async Task InitializePurchasingAsync()
    {
        if (IsInitialized() || _isInitializing) return;

        if (C_PRODUCTS == null || C_PRODUCTS.Length == 0)
        {
            Debug.LogError("PurchaseManager: C_PRODUCTS пуст. Заполните список товаров в инспекторе.");
            return;
        }

        _isInitializing = true;
        try
        {
            // В новых версиях IAP сначала нужно инициализировать Unity Gaming Services.
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            var module = StandardPurchasingModule.Instance();
            // Диалог Fake Store в редакторе: по умолчанию (Default) окно не показывается.
            module.useFakeStoreUIMode = fakeStoreUIMode;
            var builder = ConfigurationBuilder.Instance(module);
            foreach (string s in C_PRODUCTS)
            {
                if (!string.IsNullOrEmpty(s)) builder.AddProduct(s, ProductType.Consumable);
            }
            UnityPurchasing.Initialize(this, builder);
        }
        catch (Exception e)
        {
            _isInitializing = false;
            Debug.LogError("PurchaseManager: ошибка инициализации Unity Services / IAP: " + e);
        }
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyConsumable(int index)
    {
        Debug.Log("BuyConsumable, amount = " + _donationManager.DonationAmount);
        if (_donationManager == null)
        {
            Debug.LogError("PurchaseManager: _donationManager == null (Zenject не выполнил инъекцию).");
            return;
        }

        int _amount = _donationManager.DonationAmount;

        switch (_amount)
        {
            case 1:
                index = 0;
                break;
            case 5:
                index = 1;
                break;
            case 10:
                index = 2;
                break;
            default:
                index = 2;
                break;
        }

        if (_amount <= 0)
        {
            Debug.Log("BuyConsumable: сумма пожертвования не выбрана.");
            return;
        }

        if (C_PRODUCTS == null || index < 0 || index >= C_PRODUCTS.Length)
        {
            Debug.LogError($"BuyConsumable: индекс {index} вне диапазона C_PRODUCTS (длина: {(C_PRODUCTS == null ? 0 : C_PRODUCTS.Length)}). Проверьте список в инспекторе.");
            return;
        }

        currentProductIndex = index;
        BuyProductID(C_PRODUCTS[index]);
    }

    void BuyProductID(string productId)
    {
        if (!IsInitialized())
        {
            Debug.LogWarning("BuyProductID: магазин ещё не инициализирован. Пробуем инициализировать снова, повторите покупку позже.");
            _ = InitializePurchasingAsync();
            return;
        }

        Product product = m_StoreController.products.WithID(productId);

        if (product != null && product.availableToPurchase)
        {
            Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
            m_StoreController.InitiatePurchase(product);
        }
        else
        {
            Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase: " + productId);
            OnPurchaseFailed(product, PurchaseFailureReason.ProductUnavailable);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");

        _isInitializing = false;
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        _isInitializing = false;
        Debug.LogError("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        _isInitializing = false;
        Debug.LogError($"IAP init failed: {error}, message: {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string id = args.purchasedProduct.definition.id;

        if (C_PRODUCTS != null && Array.IndexOf(C_PRODUCTS, id) >= 0)
            OnSuccessC(args);
        else
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", id));

        return PurchaseProcessingResult.Complete;
    }

    public delegate void OnSuccessConsumable(PurchaseEventArgs args);
    protected virtual void OnSuccessC(PurchaseEventArgs args)
    {
        Debug.Log(args.purchasedProduct.definition.id + " Buyed!");
        GameEventMessage.SendEvent(EventsLibrary.OnSuccessConsumable);
    }
    public delegate void OnSuccessNonConsumable(PurchaseEventArgs args);

    public delegate void OnFailedPurchase(Product product, PurchaseFailureReason failureReason);
    protected virtual void OnFailedP(Product product, PurchaseFailureReason failureReason)
    {
        // product может быть null (например, товар не найден) — не обращаемся к нему без проверки.
        string id = product != null ? product.definition.storeSpecificId : "<null>";
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", id, failureReason));
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        OnFailedP(product, failureReason);
    }
}
