




using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopUICtrl : MediatorCtrlBase
{
	public const string String_ShopOneStageDiscount = "ShopOneStageDiscount";

	public const string String_ShopOneEquipExp = "ShopOneEquipExp";

	public const string String_ShopOneFreeBox = "ShopOneFreeBox";

	public const string String_ShopOneDiamondBox = "ShopOneDiamondBox";

	public const string String_ShopOneDiamond = "ShopOneDiamond";

	public const string String_ShopOneGold = "ShopOneGold";

	public ScrollRectBase mScrollRect;

	public MainUIScrollRectInsideCtrl mInsideCtrl;

	public GameObject window;

	private Dictionary<string, ShopOneBase> mList = new Dictionary<string, ShopOneBase>();

	private List<string> openlist = new List<string>
	{
		"ShopOneStageDiscount",
		"ShopOneDiamondBox",
		"ShopOneDiamond",
		"ShopOneGold"
	};

	private Dictionary<string, Func<bool>> mOpenCondition = new Dictionary<string, Func<bool>>();

	private float gotopos;

	private float maxcontenty;

	private float uppos;

	private int opencheck;

	private RectTransform windowt;

	private bool bOpened;

	protected override void OnInit()
	{
		RectTransform rectTransform = base.transform as RectTransform;
		windowt = (window.transform as RectTransform);
		RectTransform rectTransform2 = windowt;
		Vector2 sizeDelta = windowt.sizeDelta;
		float x = sizeDelta.x;
		Vector2 sizeDelta2 = rectTransform.sizeDelta;
		float y = sizeDelta2.y;
		Vector2 anchoredPosition = windowt.anchoredPosition;
		rectTransform2.sizeDelta = new Vector2(x, y + anchoredPosition.y);
		mOpenCondition.Clear();
		mOpenCondition.Add("ShopOneDiamondBox", () => LocalSave.Instance.mGuideData.is_system_open(1) || GameLogic.Hold.Guide.mEquip.process > 0 || LocalSave.Instance.GetDiamondBoxFreeCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal) > 0 || LocalSave.Instance.GetHaveEquips(havewear: true).Count > 1);
		mOpenCondition.Add("ShopOneStageDiscount", () => (!PurchaseManager.Instance.IsValid()) ? false : ShopOneStageDiscount.IsValid());
	}

	protected override void OnSetArgs(object o)
	{
		mInsideCtrl.anotherScrollRect = (o as ScrollRectBase);
	}

	private int GetOpenCheck()
	{
		int num = 0;
		if (GameLogic.Hold.Guide.mEquip.process > 0)
		{
			num |= 2;
		}
		if (mOpenCondition["ShopOneStageDiscount"]())
		{
			num |= 4;
		}
		return num;
	}

	protected override void OnOpen()
	{
		bOpened = true;
		SdkManager.send_event_iap("SHOW", ShopOpenSource.ESHOP_PAGE, string.Empty, string.Empty, string.Empty);
		int num = opencheck = GetOpenCheck();
		LocalSave.Instance.mShop.bRefresh = false;
		InitUI();
		OnLanguageChange();
	}

	private void InitUI()
	{
		uppos = -70f + PlatformHelper.GetFringeHeight();
		float num = uppos;
		float num2 = 0f - num;
		int i = 0;
		for (int count = openlist.Count; i < count; i++)
		{
			Func<bool> value = null;
			if (mOpenCondition.TryGetValue(openlist[i], out value) && value != null && !value())
			{
				ShopOneBase shopOneBase = get_one(openlist[i]);
				if (shopOneBase != null)
				{
					shopOneBase.gameObject.SetActive(value: false);
				}
				continue;
			}
			ShopOneBase shop = GetShop(openlist[i]);
			shop.gameObject.SetActive(value: true);
			shop.Init();
			shop.mRectTransform.anchoredPosition = new Vector3(0f, num, 0f);
			float num3 = num;
			Vector2 sizeDelta = shop.mRectTransform.sizeDelta;
			num = num3 - sizeDelta.y;
			float num4 = num2;
			Vector2 sizeDelta2 = shop.mRectTransform.sizeDelta;
			num2 = num4 + sizeDelta2.y;
		}
		UpdateNet();
		num2 += 200f;
		RectTransform content = mScrollRect.content;
		Vector2 sizeDelta3 = mScrollRect.content.sizeDelta;
		content.sizeDelta = new Vector2(sizeDelta3.x, num2);
		float num5 = num2;
		Vector2 sizeDelta4 = windowt.sizeDelta;
		maxcontenty = num5 - sizeDelta4.y;
		maxcontenty = MathDxx.Clamp(maxcontenty, 0f, maxcontenty);
		Goto(0);
	}

	private void UpdateList()
	{
	}

	private ShopOneBase get_one(string str)
	{
		ShopOneBase value = null;
		if (mList.TryGetValue(str, out value))
		{
		}
		return value;
	}

	private void Goto(int index, bool play = false)
	{
		for (int i = index; i < openlist.Count && !Goto(openlist[i], play); i++)
		{
		}
	}

	private bool Goto(string name, bool play = false)
	{
		if (mList.TryGetValue(name, out ShopOneBase value))
		{
			Vector2 anchoredPosition = value.mRectTransform.anchoredPosition;
			gotopos = 0f - anchoredPosition.y + uppos;
			gotopos = MathDxx.Clamp(gotopos, 0f, maxcontenty);
			mScrollRect.Goto(gotopos, play);
			return true;
		}
		return false;
	}

	private ShopOneBase GetShop(string path)
	{
		if (mList.TryGetValue(path, out ShopOneBase value))
		{
			return value;
		}
		string path2 = Utils.FormatString("UIPanel/ShopUI/{0}", path);
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>(path2));
		gameObject.SetParentNormal(mScrollRect.content.transform);
		value = gameObject.GetComponent<ShopOneBase>();
		value.name = path;
		mList.Add(path, value);
		return value;
	}

	protected override void OnClose()
	{
		bOpened = false;
		Dictionary<string, ShopOneBase>.Enumerator enumerator = mList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.Deinit();
		}
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
		string name = notification.Name;
		object body = notification.Body;
		if (name == null)
		{
			return;
		}
		if (!(name == "PUB_NETCONNECT_UPDATE"))
		{
			if (!(name == "MainUI_GotoShop"))
			{
				ShopOneBase value;
				if (!(name == "MainUI_TimeBoxUpdate"))
				{
					if (name == "ShopUI_Update")
					{
						LocalSave.Instance.mShop.bRefresh = true;
						if (bOpened)
						{
							OnOpen();
						}
					}
				}
				else if (mList.TryGetValue("ShopOneFreeBox", out value))
				{
					value.UpdateNet();
				}
			}
			else
			{
				string name2 = (string)body;
				Goto(name2, play: true);
			}
		}
		else
		{
			UpdateNet();
		}
	}

	private void UpdateNet()
	{
		Dictionary<string, ShopOneBase>.Enumerator enumerator = mList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.UpdateNet();
		}
	}

	public override void OnLanguageChange()
	{
		Dictionary<string, ShopOneBase>.Enumerator enumerator = mList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.OnLanguageChange();
		}
	}
}
