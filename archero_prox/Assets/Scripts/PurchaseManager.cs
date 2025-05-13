




using com.F4A.MobileThird;
using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
//using UnityEngine.Purchasing;
//using UnityEngine.Purchasing.Security;

public class PurchaseManager : MonoBehaviour
    //, IStoreListener
{
    public Dictionary<int, string> mDataInt = new Dictionary<int, string>
    {
        {
            0,
            "$0.99"
        },
        {
            1,
            "$4.99"
        },
        {
            2,
            "$9.99"
        },
        {
            3,
            "$19.99"
        },
        {
            4,
            "$49.99"
        },
        {
            5,
            "$99.99"
        }
    };

    public Dictionary<string, string> mDataString = new Dictionary<string, string>
    {
        {
            "com.game2019.archero_d1",
            "$0.99"
        },
        {
            "com.game2019.archero_d2",
            "$4.99"
        },
        {
            "com.game2019.archero_d3",
            "$9.99"
        },
        {
            "com.game2019.archero_d4",
            "$19.99"
        },
        {
            "com.game2019.archero_d5",
            "$49.99"
        },
        {
            "com.game2019.archero_d6",
            "$99.99"
        },
        {
            "com.game2019.archero_discount101",
            "$0.99"
        },
        {
            "com.game2019.archero_discount102",
            "$4.99"
        },
        {
            "com.game2019.archero_discount103",
            "$4.99"
        },
        {
            "com.game2019.archero_discount104",
            "$9.99"
        },
        {
            "com.game2019.archero_discount105",
            "$9.99"
        },
        {
            "com.game2019.archero_discount106",
            "$19.99"
        },
        {
            "com.game2019.archero_discount107",
            "$19.99"
        },
        {
            "com.game2019.archero_discount108",
            "$19.99"
        },
        {
            "com.game2019.archero_discount109",
            "$49.99"
        },
        {
            "com.game2019.archero_discount110",
            "$49.99"
        },
        {
            "com.game2019.archero_discount111",
            "$49.99"
        },
        {
            "com.game2019.archero_discount112",
            "$49.99"
        }
    };

    public const string TransactionID = "transactionid";

    //private Dictionary<int, string> mProductList = new Dictionary<int, string>();

    //private IStoreController controller;

    //private IAppleExtensions m_AppleExtensions;

    //private CrossPlatformValidator validator;

    private bool m_PurchaseInProgress;

    private ShopOpenSource opensource;

    private Action<bool, CRespInAppPurchase> m_PurchaseCallback;

    private Sequence seq_purchase;

    public static PurchaseManager Instance
    {
        get;
        private set;
    }

    private void Start()
    {
        Instance = this;
        //DOTween.Sequence().AppendInterval(0.1f).AppendCallback(delegate
        //{
        //    init();
        //});
    }

    private void OnEnable()
    {
        IAPManager.OnBuyPurchaseSuccessed += IAPManager_OnBuyPurchaseSuccessed;
        IAPManager.OnBuyPurchaseFailed += IAPManager_OnBuyPurchaseFailed;
    }
    private void OnDisable()
    {
        IAPManager.OnBuyPurchaseSuccessed -= IAPManager_OnBuyPurchaseSuccessed;
        IAPManager.OnBuyPurchaseFailed -= IAPManager_OnBuyPurchaseFailed;
    }

    private void IAPManager_OnBuyPurchaseFailed(string id, string reason)
    {
        SetProgress(value: false);
        SdkManager.send_event_iap("FINISH", opensource, id, "FAIL", reason.ToString().ToUpper());
        switch (reason)
        {
            case "UserCancelled":
                break;
            case "PurchasingUnavailable":
                {
                    string languageByTID4 = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_unavailable_title");
                    string languageByTID5 = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_unavailable_content");
                    string languageByTID6 = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_unavailable_ok");
                    WindowUI.ShowPopWindowOneUI(languageByTID4, languageByTID5, languageByTID6, closebuttonshow: true, delegate
                    {
                    });
                    break;
                }
            default:
                {
                    string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_fail_title");
                    string languageByTID2 = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_fail_content");
                    string languageByTID3 = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_fail_retry");
                    WindowUI.ShowPopWindowOneUI(languageByTID, languageByTID2, languageByTID3, closebuttonshow: true, delegate
                    {
                        OnPurchaseClicked(id, m_PurchaseCallback);
                    });
                    break;
                }
        }
    }

    private void IAPManager_OnBuyPurchaseSuccessed(string id, bool modeTest, string receipt)
    {
        SetProgress(value: false);
        Send(id, receipt);
    }

    //private void init()
    //{
    //    StandardPurchasingModule standardPurchasingModule = StandardPurchasingModule.Instance();
    //    standardPurchasingModule.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
    //    ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(standardPurchasingModule);
    //    configurationBuilder.useCatalogProvider = true;
    //    configurationBuilder.Configure<IMoolahConfiguration>().appKey = "ea6cb49d4d909aa31691e0472c6044f4";
    //    configurationBuilder.Configure<IMoolahConfiguration>().hashKey = "cc";
    //    configurationBuilder.Configure<IUnityChannelConfiguration>().fetchReceiptPayloadOnPurchase = false;
    //    ProductCatalog productCatalog = ProductCatalog.LoadDefaultCatalog();
    //    int num = 0;
    //    foreach (ProductCatalogItem allValidProduct in productCatalog.allValidProducts)
    //    {
    //        if (allValidProduct.allStoreIDs.Count > 0)
    //        {
    //            IDs ds = new IDs();
    //            foreach (StoreID allStoreID in allValidProduct.allStoreIDs)
    //            {
    //                ds.Add(allStoreID.id, allStoreID.store);
    //            }
    //            mProductList.Add(num, allValidProduct.id);
    //            num++;
    //            configurationBuilder.AddProduct(allValidProduct.id, allValidProduct.type, ds);
    //        }
    //        else
    //        {
    //            mProductList.Add(num, allValidProduct.id);
    //            num++;
    //            configurationBuilder.AddProduct(allValidProduct.id, allValidProduct.type);
    //        }
    //    }
    //    configurationBuilder.Configure<IMoolahConfiguration>().SetMode(CloudMoolahMode.AlwaysSucceed);
    //    //@TODO validator CrossPlatformValidator
    //    //validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);

    //    UnityPurchasing.Initialize(this, configurationBuilder);
    //}

    public bool IsValid()
    {
        //return controller != null;
        return IAPManager.Instance.IsInitialized();
    }

    //public ProductMetadata GetProductMetadata(int index)
    //{
    //    if (!mProductList.ContainsKey(index))
    //    {
    //        return null;
    //    }
    //    return GetProductMetadata(mProductList[index]);
    //}

    //public ProductMetadata GetProductMetadata(string id)
    //{
    //    if (controller == null || controller.products == null)
    //    {
    //        return null;
    //    }
    //    return controller.products.WithID(id)?.metadata;
    //}

    public string GetProduct_localpricestring(int index)
    {
        //if (!mProductList.ContainsKey(index))
        //{
        //    if (mDataInt.ContainsKey(index))
        //    {
        //        return mDataInt[index];
        //    }
        //    return index.ToString();
        //}
        //return GetProduct_localpricestring(mProductList[index]);

        var product = IAPManager.Instance.GetProductInfoByIndex(index);
        if (product != null) return IAPManager.Instance.GetProductPriceStringById(product.Id);
        else return "0.01$";
    }

    //public string GetProduct_localpricestring(string id)
    //{
    //    ProductMetadata productMetadata = GetProductMetadata(id);
    //    if (productMetadata == null || string.IsNullOrEmpty(productMetadata.localizedPriceString))
    //    {
    //        if (mDataString.ContainsKey(id))
    //        {
    //            return mDataString[id];
    //        }
    //        return id;
    //    }
    //    return productMetadata.localizedPriceString;
    //}

    public string GetProduct_localpricestring(string id)
    {
        return IAPManager.Instance.GetProductPriceStringById(id);
    }

    public string GetProductID(int index)
    {
        //if (mProductList.ContainsKey(index))
        //{
        //    return mProductList[index];
        //}
        //return string.Empty;

        var product = IAPManager.Instance.GetProductInfoByIndex(index);
        if (product != null) return product.Id;
        return string.Empty;
    }

    public void SetOpenSource(ShopOpenSource source)
    {
        opensource = source;
    }

    public ShopOpenSource GetOpenSource()
    {
        return opensource;
    }

    //public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    //{
    //    Debugger.Log("支付 OnInitialized success");
    //    this.controller = controller;
    //    m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
    //    m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);
    //    Facade.Instance.SendNotification("ShopUI_Update");
    //}

    //private void OnDeferred(Product item)
    //{
    //    SetProgress(value: false);
    //}

    //public void OnInitializeFailed(InitializationFailureReason error)
    //{
    //    Debugger.Log("支付 OnInitializedFailed " + error.ToString());
    //}

    //public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    //{
    //    SetProgress(value: false);
    //    Send(e.purchasedProduct.definition.id, e.purchasedProduct.receipt);
    //    return PurchaseProcessingResult.Complete;
    //}

    private bool is_only_one_currency(string id)
    {
        bool result = false;
        if (id.Equals("com.game2019.archero_d1") || id.Equals("com.game2019.archero_d2") || id.Equals("com.game2019.archero_d3") || id.Equals("com.game2019.archero_d4") || id.Equals("com.game2019.archero_d5") || id.Equals("com.game2019.archero_d6"))
        {
            result = true;
        }
        return result;
    }

    public List<Drop_DropModel.DropData> GetGotList(CRespInAppPurchase data, List<Drop_DropModel.DropData> currencylist)
    {
        List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
        if (currencylist != null)
        {
            int i = 0;
            for (int count = currencylist.Count; i < count; i++)
            {
                Drop_DropModel.DropData dropData = currencylist[i];
                if (dropData.is_base_currency)
                {
                    list.Add(dropData);
                }
            }
        }
        if (data != null && data.m_arrEquipInfo != null)
        {
            int j = 0;
            for (int num = data.m_arrEquipInfo.Length; j < num; j++)
            {
                Drop_DropModel.DropData dropData2 = new Drop_DropModel.DropData();
                dropData2.type = PropType.eEquip;
                dropData2.id = (int)data.m_arrEquipInfo[j].m_nEquipID;
                dropData2.count = (int)data.m_arrEquipInfo[j].m_nFragment;
                list.Add(dropData2);
            }
        }
        return list;
    }

    private void Send(string id, string receipt)
    {
        SdkManager.GameCenter_clear_login_count();
        Debug.Log(Utils.FormatString("Archero:{0} : {1}", Debugger.Tag.ePurchase.ToString(), "id: " + id + ", Receipt: " + receipt));
        LocalSave.Instance.mPurchase.AddPurchase(receipt);
        CInAppPurchase cInAppPurchase = new CInAppPurchase();
        cInAppPurchase.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
        cInAppPurchase.m_nPlatformIndex = 1;
        cInAppPurchase.m_nProductID = id;
        cInAppPurchase.m_strReceiptData = receipt;
        if (is_only_one_currency(id))
        {
            CRespInAppPurchase cRespInAppPurchase = new CRespInAppPurchase();
            if (m_PurchaseCallback != null)
            {
                cRespInAppPurchase.product_id = id;
                m_PurchaseCallback(arg1: true, cRespInAppPurchase);
            }
        }
        CInstance<TipsUIManager>.Instance.Show(ETips.Tips_PurchaseSuccess);
        Debugger.Log(Debugger.Tag.eHTTP, "purchaseandroid id = " + cInAppPurchase.m_nProductID + " receipt = " + cInAppPurchase.m_strReceiptData);
        NetManager.SendInternal(cInAppPurchase, SendType.eCache, delegate (NetResponse response)
        {
            if (response.data != null)
            {
                CRespInAppPurchase cRespInAppPurchase2 = response.data as CRespInAppPurchase;
                if (cRespInAppPurchase2 != null)
                {
                    LocalSave.Instance.Equip_Add(cRespInAppPurchase2.m_arrEquipInfo);
                    LocalSave.Instance.mPurchase.RemovePurchase(cRespInAppPurchase2.m_strIAPTransID);
                    LocalSave.Instance.UserInfo_SetRebornCount(cRespInAppPurchase2.m_nBattleRebornCount);
                    if (!is_only_one_currency(id))
                    {
                        LocalSave.Instance.UserInfo_SetDiamond((int)cRespInAppPurchase2.m_nTotalDiamonds);
                        LocalSave.Instance.UserInfo_SetGold((int)cRespInAppPurchase2.m_nTotalCoins);
                        LocalSave.Instance.UserInfo_SetRebornCount(cRespInAppPurchase2.m_nBattleRebornCount);
                        LocalSave.Instance.SetDiamondExtraCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal, cRespInAppPurchase2.m_nNormalDiamondItems);
                        LocalSave.Instance.SetDiamondExtraCount(LocalSave.TimeBoxType.BoxChoose_DiamondLarge, cRespInAppPurchase2.m_nLargeDiamondItems);
                        cRespInAppPurchase2.product_id = id;
                        if (m_PurchaseCallback != null)
                        {
                            m_PurchaseCallback(arg1: true, cRespInAppPurchase2);
                        }
                    }
                    SdkManager.send_event_iap("FINISH", opensource, id, "SUCCESS", string.Empty);
                }
                else
                {
                    CInstance<TipsUIManager>.Instance.Show("InAppPurchase response is not a CRespInAppPurchase type");
                    SdkManager.send_event_iap("FINISH", opensource, id, "FAIL", "TYPE_ERROR");
                }
            }
            else if (response.error != null)
            {
                SdkManager.Bugly_Report("PurchaseManager", Utils.FormatString("ProcessPurchase id:{0} response error code:{1} ", id, response.error.m_nStatusCode), receipt);
                SdkManager.send_event_iap("FINISH", opensource, id, "FAIL", Utils.FormatString("SERVER_ERROR_CODE_{0}", response.error.m_nStatusCode));
            }
            else
            {
                SdkManager.Bugly_Report("PurchaseManager", "response.error == null");
                SdkManager.send_event_iap("FINISH", opensource, id, "FAIL", "ERROR_NULL");
            }
        });
    }

    //public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    //{
    //    SetProgress(value: false);
    //    SdkManager.send_event_iap("FINISH", opensource, item.definition.id, "FAIL", r.ToString().ToUpper());
    //    switch (r)
    //    {
    //        case PurchaseFailureReason.UserCancelled:
    //            break;
    //        case PurchaseFailureReason.PurchasingUnavailable:
    //            {
    //                string languageByTID4 = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_unavailable_title");
    //                string languageByTID5 = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_unavailable_content");
    //                string languageByTID6 = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_unavailable_ok");
    //                WindowUI.ShowPopWindowOneUI(languageByTID4, languageByTID5, languageByTID6, closebuttonshow: true, delegate
    //                {
    //                });
    //                break;
    //            }
    //        default:
    //            {
    //                string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_fail_title");
    //                string languageByTID2 = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_fail_content");
    //                string languageByTID3 = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_fail_retry");
    //                WindowUI.ShowPopWindowOneUI(languageByTID, languageByTID2, languageByTID3, closebuttonshow: true, delegate
    //                {
    //                    OnPurchaseClicked(item.definition.id, m_PurchaseCallback);
    //                });
    //                break;
    //            }
    //    }
    //}

    public void OnPurchaseClicked(string productId, Action<bool, CRespInAppPurchase> callback = null)
    {
        Debug.Log("@LOG OnPurchaseClicked productId:" + productId);
        if (!NetManager.IsNetConnect)
        {
            CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
        }
        else if (m_PurchaseInProgress)
        {
            CInstance<TipsUIManager>.Instance.Show(GameLogic.Hold.Language.GetLanguageByTID("商店_正在支付"));
        }
        //else if(IAPManager.Instance.IsInitialized())
        //{
        //    //Debugger.Log("购买 " + productId);
        //    m_PurchaseCallback = callback;
        //    SetProgress(value: true);
        //    IAPManager.Instance.BuyProductByID(productId);
        //}
        //else
        //{
        //    CInstance<TipsUIManager>.Instance.Show(ETips.Tips_ShopNotReady);
        //    Debugger.Log("PurchaseManager.controller is null");
        //}


        else
        {
            m_PurchaseCallback = callback;
            SetProgress(value: true);
            IAPManager.Instance.BuyProductByID(productId);
        }
    }

    private void SetProgress(bool value)
    {
        m_PurchaseInProgress = value;
    }

    private void KillSequence()
    {
        if (seq_purchase != null)
        {
            seq_purchase.Kill();
            seq_purchase = null;
        }
    }

    //private void ShowUI(int purchase_state, string id, string receipt)
    //{
    //}
}
