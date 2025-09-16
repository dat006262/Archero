




using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class AdTurnTableCtrl : MonoBehaviour, AdsRequestHelper.AdsCallback
{
	public Text Text_Title;

	public Image Image_Ad;

	public ButtonCtrl Button_Cancel;

	public ButtonCtrl Button_Ad;

	public GameTurnTableCtrl mTurnCtrl;

	public Text Text_Turn;

	public Text Text_Last;

	public Action onClickClose;

	private float Text_TurnX;

	private bool bStartTurn;

	private TurnTableType resultType;

	private int[] qualities = new int[6]
	{
		1,
		1,
		1,
		3,
		3,
		4
	};

	private bool bAdReward;

	public void Init()
	{
		Vector2 anchoredPosition = Text_Turn.rectTransform.anchoredPosition;
		Text_TurnX = anchoredPosition.x;
		mTurnCtrl.TurnEnd = delegate(TurnTableData data)
		{
			AdTurnTableCtrl adTurnTableCtrl = this;
			resultType = data.type;
			LocalSave.Instance.BattleAd_Use();
			CReqItemPacket itemPacket = NetManager.GetItemPacket(null);
			itemPacket.m_nPacketType = 19;
			NetManager.SendInternal(itemPacket, SendType.eUDP, delegate(NetResponse response)
			{
#if ENABLE_NET_MANAGER
				if (response.IsSuccess)
#endif
				{
					long num = 0L;
					long num2 = 0L;
					if (data.type == TurnTableType.Gold)
					{
						num = (long)data.value;
					}
					else if (data.type == TurnTableType.Diamond)
					{
						num2 = (long)data.value;
					}
					SdkManager.send_event_ad(ADSource.eTurntable, "REWARD", (int)num, (int)num2, string.Empty, string.Empty);
				}
			});
			if (data.type == TurnTableType.Diamond)
			{
				DOTween.Sequence().AppendInterval(1.8f).AppendCallback(delegate
				{
					if (adTurnTableCtrl.onClickClose != null)
					{
						adTurnTableCtrl.onClickClose();
					}
				})
					.SetUpdate(isIndependentUpdate: true);
			}
			else if (onClickClose != null)
			{
				onClickClose();
			}
		};
	}

	public void Open()
	{
		show_close(value: false);
		AdsRequestHelper.getRewardedAdapter().AddCallback(this);
		if (LocalSave.Instance.IsAdFree())
		{
			Image_Ad.enabled = false;
			RectTransform rectTransform = Text_Turn.rectTransform;
			Vector2 anchoredPosition = Text_Turn.rectTransform.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(0f, anchoredPosition.y);
		}
		else
		{
			Image_Ad.enabled = true;
			RectTransform rectTransform2 = Text_Turn.rectTransform;
			float text_TurnX = Text_TurnX;
			Vector2 anchoredPosition2 = Text_Turn.rectTransform.anchoredPosition;
			rectTransform2.anchoredPosition = new Vector2(text_TurnX, anchoredPosition2.y);
		}
		Text_Last.text = Utils.FormatString("{0}: {1}", GameLogic.Hold.Language.GetLanguageByTID("key_ad_count"), LocalSave.Instance.BattleAd_Get());
		bStartTurn = false;
		GameLogic.Hold.Sound.PlayUI(1000004);
		Button_Cancel.onClick = delegate
		{
			if (onClickClose != null)
			{
				onClickClose();
			}
		};
		Button_Ad.SetDepondNet(value: true);
		Button_Ad.onClick = delegate
		{
			bAdReward = false;
			SdkManager.send_event_ad(ADSource.eTurntable, "CLICK", 0, 0, string.Empty, string.Empty);
			if (!NetManager.IsNetConnect)
			{
				CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
				SdkManager.send_event_ad(ADSource.eTurntable, "IMPRESSION", 0, 0, "FAIL", "NET_ERROR");
			}
			else if (LocalSave.Instance.IsAdFree())
			{
				show_button(value: false);
				mTurnCtrl.Init();
			}
			else if (!AdsRequestHelper.getRewardedAdapter().isLoaded())
			{
				SdkManager.send_event_ad(ADSource.eTurntable, "IMPRESSION", 0, 0, "FAIL", "AD_NOT_READY");
				WindowUI.ShowAdInsideUI(AdInsideProxy.EnterSource.eGameTurn, delegate
				{
					show_button(value: false);
					mTurnCtrl.Init();
				});
			}
			else
			{
				SdkManager.send_event_ad(ADSource.eTurntable, "IMPRESSION", 0, 0, "SUCCESS", string.Empty);
				AdsRequestHelper.getRewardedAdapter().Show(this);
			}
		};
		show_button(value: true);
		InitUI();
	}

	private void InitUI()
	{
		List<TurnTableData> list = new List<TurnTableData>();
		string[] adTurn = LocalModelManager.Instance.Stage_Level_stagechapter.GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage).AdTurn;
		int i = 0;
		for (int num = adTurn.Length; i < num && i < 6; i++)
		{
			TurnTableData turnTableData = new TurnTableData();
			string[] array = adTurn[i].Split(',');
			int result = 0;
			int.TryParse(array[0], out result);
			long result2 = 0L;
			long.TryParse(array[1], out result2);
			if (result2 > 0)
			{
				if (result == 1)
				{
					turnTableData.type = TurnTableType.Gold;
				}
				else
				{
					turnTableData.type = TurnTableType.Diamond;
				}
				turnTableData.value = result2;
			}
			else
			{
				turnTableData.type = TurnTableType.Empty;
			}
			turnTableData.quality = qualities[i];
			list.Add(turnTableData);
		}
		for (int j = list.Count; j < 6; j++)
		{
			TurnTableData turnTableData2 = new TurnTableData();
			turnTableData2.type = TurnTableType.Empty;
			list.Add(turnTableData2);
		}
		mTurnCtrl.InitGood(list);
	}

	public void show_close(bool value)
	{
		Button_Cancel.transform.parent.gameObject.SetActive(value);
	}

	private void show_button(bool value)
	{
		Button_Ad.transform.parent.gameObject.SetActive(value);
		Button_Cancel.transform.gameObject.SetActive(value);
	}

	public void Deinit()
	{
		AdsRequestHelper.getRewardedAdapter().RemoveCallback(this);
	}

	public void OnLanguageChange()
	{
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_AdTitle");
		Text_Turn.text = GameLogic.Hold.Language.GetLanguageByTID("event_ad_turntable_turn");
	}

	public void onRequest(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("AdTurnTableCtrl onRequest");
	}

	public void onLoad(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("AdTurnTableCtrl onLoad");
	}

	public void onFail(AdsRequestHelper.AdsDriver sender, string msg)
	{
		AdsRequestHelper.DebugLog("AdTurnTableCtrl onFail");
	}

	public void onOpen(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("AdTurnTableCtrl onOpen");
	}

	public void onClose(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("AdTurnTableCtrl onClose");
		DOTween.Sequence().AppendInterval(0.1f).AppendCallback(delegate
		{
			if (!bAdReward)
			{
				SdkManager.send_event_ad(ADSource.eTurntable, "CLOSE_BEFORE", 0, 0, string.Empty, string.Empty);
			}
		});
	}

	public void onClick(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("AdTurnTableCtrl onClick");
	}

	public void onReward(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("AdTurnTableCtrl onReward");
		show_button(value: false);
		mTurnCtrl.Init();
	}
}
