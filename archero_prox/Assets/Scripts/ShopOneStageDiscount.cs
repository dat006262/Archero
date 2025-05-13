




using Dxx.Util;
using GameProtocol;
using Newtonsoft.Json;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ShopOneStageDiscount : ShopOneBase
{
	private static List<Color> mLightColors = new List<Color>
	{
		new Color(1f, 76f / 85f, 49f / 255f, 0.7058824f),
		new Color(37f / 51f, 1f, 22f / 51f, 0.7058824f),
		new Color(44f / 51f, 127f / 255f, 1f, 0.7058824f)
	};

	private const float Width = 180f;

	public Text Text_Title;

	public Text Text_New;

	public Text Text_Content;

	public Text Text_Price;

	public Text Text_Value;

	public GameObject RewardParent;

	public ButtonCtrl Button_Click;

	public GameObject itemone;

	public GameObject itemadd;

	public Image Image_BG;

	public Image Image_Light;

	private LocalUnityObjctPool mPool;

	private List<ShopOneStageDiscountOneCtrl> mList = new List<ShopOneStageDiscountOneCtrl>();

	private List<Drop_DropModel.DropData> rewards;

	private string mID;

	public static bool IsValid()
	{
		if (!IsUnlock())
		{
			return false;
		}
		if (!LocalSave.Instance.StageDiscount_IsValid())
		{
			return false;
		}
		return true;
	}

	private static bool IsUnlock()
	{
		int key = LocalSave.Instance.StageDiscount_GetCurrentID();
		Box_Activity beanById = LocalModelManager.Instance.Box_Activity.GetBeanById(key);
		if (beanById == null)
		{
			return false;
		}
		if (beanById.ShowCond.Length > 0)
		{
			int result = 0;
			int.TryParse(beanById.ShowCond[0], out result);
			switch (result)
			{
			case 1:
				if (beanById.ShowCond.Length == 2)
				{
					int result2 = -1;
					int.TryParse(beanById.ShowCond[1], out result2);
					if (result2 >= 0 && LocalSave.Instance.mStage.CurrentStage > result2)
					{
						return true;
					}
				}
				break;
			case 2:
				if (beanById.ShowCond.Length != 3)
				{
				}
				break;
			}
		}
		return false;
	}

	protected override void OnAwake()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<ShopOneStageDiscountOneCtrl>(itemone);
		mPool.CreateCache<RectTransform>(itemadd);
		itemone.SetActive(value: false);
		itemadd.SetActive(value: false);
		Button_Click.onClick = delegate
		{
			LocalSave.Instance.StageDiscount_Send(delegate(string data)
			{
				LocalSave.StageDiscountBody stageDiscountBody = null;
				try
				{
					stageDiscountBody = JsonConvert.DeserializeObject<LocalSave.StageDiscountBody>(data);
					if (stageDiscountBody.current_purchase == null || stageDiscountBody.current_purchase.product_id != LocalSave.Instance.StageDiscount_GetProductID())
					{
						CInstance<TipsUIManager>.Instance.Show(ETips.Tips_StageDiscountChange);
						LocalSave.Instance.StageDiscount_Init(data);
					}
					else
					{
						SdkManager.send_event_iap("CLICK", PurchaseManager.Instance.GetOpenSource(), stageDiscountBody.current_purchase.product_id, string.Empty, string.Empty);
						PurchaseManager.Instance.OnPurchaseClicked(stageDiscountBody.current_purchase.product_id, delegate(bool success, CRespInAppPurchase resp)
						{
							List<Drop_DropModel.DropData> gotList = PurchaseManager.Instance.GetGotList(resp, LocalSave.Instance.StageDiscount_GetList());
							WindowUI.ShowRewardSimple(gotList);
							LocalSave.Instance.StageDiscount_Init(null);
							LocalSave.Instance.StageDiscount_Send(delegate(string d)
							{
								LocalSave.Instance.StageDiscount_Init(d);
							});
						});
					}
				}
				catch
				{
					Debugger.Log("StageDiscount_Init init failed! ::: " + data);
				}
			});
		};
	}

	protected override void OnInit()
	{
		mList.Clear();
		mPool.Collect<ShopOneStageDiscountOneCtrl>();
		mPool.Collect<RectTransform>();
		mID = LocalSave.Instance.StageDiscount_GetProductID();
		rewards = LocalSave.Instance.StageDiscount_GetList();
		if (rewards != null)
		{
			int count = rewards.Count;
			float num = 180f * (float)(count - 1);
			for (int i = 0; i < count; i++)
			{
				ShopOneStageDiscountOneCtrl shopOneStageDiscountOneCtrl = mPool.DeQueue<ShopOneStageDiscountOneCtrl>();
				shopOneStageDiscountOneCtrl.Init(rewards[i].id, rewards[i].count);
				RectTransform rectTransform = shopOneStageDiscountOneCtrl.transform as RectTransform;
				rectTransform.SetParentNormal(RewardParent);
				rectTransform.anchoredPosition = new Vector2((0f - num) / 2f + 180f * (float)i, 0f);
				mList.Add(shopOneStageDiscountOneCtrl);
				if (i > 0 && i < count)
				{
					RectTransform rectTransform2 = mPool.DeQueue<RectTransform>();
					rectTransform2.SetParentNormal(RewardParent);
					RectTransform rectTransform3 = rectTransform2;
					Vector2 anchoredPosition = (mList[i].transform as RectTransform).anchoredPosition;
					float x = anchoredPosition.x;
					Vector2 anchoredPosition2 = (mList[i - 1].transform as RectTransform).anchoredPosition;
					rectTransform3.anchoredPosition = new Vector2((x + anchoredPosition2.x) / 2f, 0f);
				}
			}
		}
		OnLanguageChange();
	}

	protected override void OnDeinit()
	{
	}

	public override void OnLanguageChange()
	{
		int result = 101;
		if (string.IsNullOrEmpty(mID) || mID.Length <= 3 || int.TryParse(mID.Substring(mID.Length - 3, 3), out result))
		{
		}
		int num = (result - 101) % mLightColors.Count;
		Image_BG.sprite = SpriteManager.GetMain(Utils.FormatString("ShopUI_Discount_BG{0}", num));
		Image_Light.color = mLightColors[num];
		Text_New.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("discount_package_{0}描述", result));
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("discount_package_{0}", result));
		Text_Value.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("discount_value"));
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("discount_buy"));
		Text_Price.text = Utils.FormatString("{0} {1}", languageByTID, PurchaseManager.Instance.GetProduct_localpricestring(mID));
		for (int i = 0; i < mList.Count; i++)
		{
			mList[i].OnLanguageUpdate();
		}
	}

	public override void UpdateNet()
	{
	}
}
