




using DG.Tweening;
using Dxx.Util;
using GameProtocol;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDiamond : MonoBehaviour
{
	public Text Text_Title;

	public ButtonCtrl Button_Get;

	public Image Image_Icon;

	public Text Text_Count;

	public Text Text_Money;

	private Shop_Shop shopdata;

	private int mIndex;

	public static void PurchaseFly(string id, Transform t)
	{
		DOTween.Sequence().AppendInterval(1f).AppendCallback(delegate
		{
			PurchaseFlyInternal(id, t);
		})
			.SetUpdate(isIndependentUpdate: true);
	}

	private static void PurchaseFlyInternal(string id, Transform t)
	{
		long num = 0L;
		long num2 = 0L;
		switch (id)
		{
		case "com.game2019.archero_d1":
			num2 = LocalModelManager.Instance.Shop_Shop.GetBeanById(201).ProductNum;
			break;
		case "com.game2019.archero_d2":
			num2 = LocalModelManager.Instance.Shop_Shop.GetBeanById(202).ProductNum;
			break;
		case "com.game2019.archero_d3":
			num2 = LocalModelManager.Instance.Shop_Shop.GetBeanById(203).ProductNum;
			break;
		case "com.game2019.archero_d4":
			num2 = LocalModelManager.Instance.Shop_Shop.GetBeanById(204).ProductNum;
			break;
		case "com.game2019.archero_d5":
			num2 = LocalModelManager.Instance.Shop_Shop.GetBeanById(205).ProductNum;
			break;
		case "com.game2019.archero_d6":
			num2 = LocalModelManager.Instance.Shop_Shop.GetBeanById(206).ProductNum;
			break;
		}
		if (num2 > 0)
		{
			if ((bool)t)
			{
				LocalSave.Instance.Modify_Diamond(num2, updateui: false);
				CurrencyFlyCtrl.PlayGet(CurrencyType.Diamond, num2, t.position);
			}
			else
			{
				LocalSave.Instance.Modify_Diamond(num2);
			}
		}
		if (num > 0)
		{
			if ((bool)t)
			{
				LocalSave.Instance.Modify_Gold(num, updateui: false);
				CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, num, t.position);
			}
			else
			{
				LocalSave.Instance.Modify_Gold(num);
			}
		}
	}

	private void Awake()
	{
		if ((bool)Button_Get)
		{
            Button_Get.SetDepondNet(value: true);
			Button_Get.onClick = delegate
			{
                OnClickButtonInternal(PurchaseManager.Instance.GetProductID(mIndex));
			};
		}
	}

	private void OnClickButtonInternal(string productID)
	{
		Debug.Log("ShopItemDiamond.OnClickButtonInternal productID:" + productID);
		if (PurchaseManager.Instance != null)
		{
			SdkManager.send_event_iap("CLICK", PurchaseManager.Instance.GetOpenSource(), productID, string.Empty, string.Empty);
			PurchaseManager.Instance.OnPurchaseClicked(productID, delegate(bool success, CRespInAppPurchase data)
			{
				PurchaseFly(data.product_id, (!this) ? null : base.transform);
			});
		}
	}

	public void Init(int index)
	{
		mIndex = index;
		shopdata = LocalModelManager.Instance.Shop_Shop.GetBeanById(201 + index);
		Image_Icon.sprite = SpriteManager.GetMain(Utils.FormatString("ic_gem_{0}", index + 1));
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_钻石{0}", shopdata.ID));
		Text_Count.text = shopdata.ProductNum.ToString();
		Text_Money.text = PurchaseManager.Instance.GetProduct_localpricestring(index);
	}

	public int GetDiamond()
	{
		return shopdata.ProductNum;
	}

	public void OnLanguageChange()
	{
		Init(mIndex);
	}

	public void UpdateNet()
	{
	}
}
