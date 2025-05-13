




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUICtrl : MediatorCtrlBase
{
	public ButtonCtrl Button_Key;

	public ButtonCtrl Button_Gold;

	public ButtonCtrl Button_Diamond;

	public CurrencyLevelCtrl mLevelCtrl;

	public Text Text_UseKey;

	public CanvasGroup mUseKey;

	public Transform Tran_Key;

	public Image Image_Key;

	public Image Image_Gold;

	public Image Image_Diamond;

	public Text Text_Gold;

	public Text Text_Diamond;

	public Text Text_Time;

	public Animation keyrotate;

	public ProgressTextCtrl mProgressCtrl;

	private static Dictionary<CurrencyType, string> mCurrencyPathList = new Dictionary<CurrencyType, string>
	{
		{
			CurrencyType.Gold,
			"CurrencyFly_Gold"
		},
		{
			CurrencyType.Diamond,
			"CurrencyFly_Diamond"
		},
		{
			CurrencyType.Key,
			"CurrencyFly_Key"
		}
	};

	private long mKeyStartTime;

	private int PerKeyTime;

	private CurrencyFlyCtrl mFlyCtrl;

	protected override void OnInit()
	{
		SetUseKeyShow(value: false);
		Text_UseKey.text = Utils.FormatString("-{0}", GameConfig.GetModeLevelKey());
		(base.transform as RectTransform).anchoredPosition = new Vector3(0f, PlatformHelper.GetFringeHeight(), 0f);
		PerKeyTime = GameConfig.GetKeyRecoverTime();
		Button_Key.onClick = delegate
		{
			KeyBuyUICtrl.SetSource(KeyBuySource.ECURRENCY);
			WindowUI.ShowWindow(WindowID.WindowID_KeyBuy);
		};
		Button_Gold.onClick = delegate
		{
			Facade.Instance.SendNotification("MainUI_GotoShop", "ShopOneGold");
		};
		Button_Diamond.onClick = delegate
		{
			Facade.Instance.SendNotification("MainUI_GotoShop", "ShopOneDiamond");
		};
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
		UpdateCurrency();
	}

	protected override void OnClose()
	{
		if (mFlyCtrl != null)
		{
			mFlyCtrl.DeInit();
		}
	}

	private void SetUseKeyShow(bool value)
	{
		if (mUseKey != null)
		{
			mUseKey.gameObject.SetActive(value);
		}
	}

	private void Update()
	{
		if (Text_Time == null)
		{
			return;
		}
		if (LocalSave.Instance.IsKeyMax())
		{
			if (Text_Time.text != string.Empty)
			{
				Text_Time.text = string.Empty;
			}
			return;
		}
		long currentTime = Utils.CurrentTime;
		long num = (long)((float)(currentTime - mKeyStartTime) / (float)PerKeyTime);
		if (num > 0)
		{
			LocalSave.Instance.Modify_Key(num, over: false);
			mKeyStartTime += num * PerKeyTime;
			LocalSave.Instance.SetKeyTime(mKeyStartTime);
			UpdateCurrency();
		}
		else
		{
			int second = (int)(PerKeyTime - (currentTime - mKeyStartTime));
			string second2String = Utils.GetSecond2String(second);
			Text_Time.text = second2String;
		}
	}

	public override object OnGetEvent(string eventName)
	{
		if (eventName != null)
		{
			if (eventName == "GetEvent_GetGoldPosition")
			{
				return GetUseStartPos(CurrencyType.Gold);
			}
			if (eventName == "GetEvent_GetDiamondPosition")
			{
				return GetUseStartPos(CurrencyType.Diamond);
			}
			if (eventName == "GetEvent_GetKeyPosition")
			{
				return GetUseStartPos(CurrencyType.Key);
			}
		}
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
		if (!(name == "PUB_UI_UPDATE_CURRENCY"))
		{
			if (!(name == "CurrencyKeyRotate"))
			{
				if (!(name == "UseCurrencyKey"))
				{
					if (!(name == "UseCurrency"))
					{
						if (name == "GetCurrency")
						{
							CurrencyFlyCtrl.CurrencyGetStruct currencyGetStruct = (CurrencyFlyCtrl.CurrencyGetStruct)body;
							if (mFlyCtrl == null)
							{
								mFlyCtrl = new CurrencyFlyCtrl();
							}
							mFlyCtrl.UseAction(mCurrencyPathList[currencyGetStruct.type], base.transform, currencyGetStruct.startpos, GetUseStartPos(currencyGetStruct.type), currencyGetStruct.count, null);
						}
					}
					else
					{
						CurrencyFlyCtrl.CurrencyUseStruct currencyUseStruct = (CurrencyFlyCtrl.CurrencyUseStruct)body;
						if (mFlyCtrl == null)
						{
							mFlyCtrl = new CurrencyFlyCtrl();
						}
						mFlyCtrl.UseAction(mCurrencyPathList[currencyUseStruct.type], base.transform, GetUseStartPos(currencyUseStruct.type), currencyUseStruct.endpos, currencyUseStruct.count, currencyUseStruct.callback);
					}
				}
				else
				{
					SetUseKeyShow(value: true);
					if ((bool)mUseKey)
					{
						mUseKey.alpha = 1f;
						mUseKey.transform.localPosition = Vector3.zero;
						DOTween.Sequence().Append(mUseKey.DOFade(0.8f, 1f)).Join(mUseKey.transform.DOLocalMoveY(-100f, 1f))
							.AppendCallback(delegate
							{
								SetUseKeyShow(value: false);
							});
					}
				}
			}
			else if ((bool)keyrotate)
			{
				keyrotate.Play("KeyRotate");
			}
		}
		else
		{
			UpdateCurrency();
		}
	}

	private Vector3 GetUseStartPos(CurrencyType type)
	{
		switch (type)
		{
		case CurrencyType.Gold:
			if (Image_Gold != null)
			{
				return Image_Gold.transform.position;
			}
			break;
		case CurrencyType.Diamond:
			if (Image_Diamond != null)
			{
				return Image_Diamond.transform.position;
			}
			break;
		case CurrencyType.Key:
			if (Image_Key != null)
			{
				return Image_Key.transform.position;
			}
			break;
		}
		throw new Exception("currencyui dont have CurrencyType." + type + " in CurrencyUICtrl.cs");
	}

	private void UpdateCurrency()
	{
		mKeyStartTime = LocalSave.Instance.GetKeyTime();
		LocalSave.UserInfo userInfo = LocalSave.Instance.GetUserInfo();
		int maxKeyCount = GameConfig.GetMaxKeyCount();
		mProgressCtrl.max = maxKeyCount;
		mProgressCtrl.current = userInfo.Key;
		mLevelCtrl.UpdateUI();
		if ((bool)Text_Gold)
		{
			Text_Gold.text = userInfo.Show_Gold.ToString();
		}
		if ((bool)Text_Diamond)
		{
			Text_Diamond.text = userInfo.Show_Diamond.ToString();
		}
	}

	public override void OnLanguageChange()
	{
	}
}
