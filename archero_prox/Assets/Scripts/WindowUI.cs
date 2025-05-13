




using DG.Tweening;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class WindowUI
{
	private static WindowID mCurrentCurrencyID = WindowID.WindowID_Invaild;

	private static WindowID mBeforeCurrencyID = WindowID.WindowID_Invaild;

	private static int mMaskCount = 0;

	private static int mNetDoingCount = 0;

	private static List<WindowID> mInGameList = new List<WindowID>();

	private static List<WindowID> mOutGameList = new List<WindowID>();

	private static List<WindowID> mAllList = new List<WindowID>();

	private const float WindowCheckTime = 180f;

	private static bool Update_bInit = false;

#if ENABLE_COMPILER_GENERATED
	[CompilerGenerated]
#endif
	private static TweenCallback _003C_003Ef__mg_0024cache0;

	public static event Action<WindowID, List<WindowID>> OnInGameWindowClose;

	public static event Action<WindowID, List<WindowID>> OnInGameWindowOpen;

	public static void Init()
	{
		Update_Init();
	}

    public static void ShowWindow(WindowID id)
    {
        switch (id)
        {
            case WindowID.WindowID_Invaild:
                return;
            case WindowID.WindowID_Main:
                GameLogic.Release.Game.SetGameState(GameManager.GameState.eMain);
                GameOver();
                break;
            case WindowID.WindowID_Battle:
                GameLogic.Release.Game.SetGameState(GameManager.GameState.eGaming);
                GameBegin();
                break;
        }
        int state = UIResourceDefine.windowClass[id].State;
        if (state == 3 || state == 2 || (state == 0 && GameLogic.Release.Game.gameState == GameManager.GameState.eMain) || (state == 1 && GameLogic.Release.Game.gameState == GameManager.GameState.eGaming))
        {
            ShowWindowInternal(id);
        }
    }

	private static void ShowWindowInternal(WindowID id)
	{
		string className = UIResourceDefine.windowClass[id].ClassName;
		AddOpenWindow(id);
		Type type = Type.GetType(className);
		WindowMediator mediator = (WindowMediator)Activator.CreateInstance(type);
		Facade.Instance.RegisterMediator(mediator);
	}

	public static void CloseWindow(WindowID id)
	{
		CloseWindowInternal(id);
		AddCloseWindow(id);
	}

	private static void CloseWindowInternal(WindowID id)
	{
		string className = UIResourceDefine.windowClass[id].ClassName;
		Facade.Instance.RemoveMediator(className);
		int state = UIResourceDefine.windowClass[id].State;
		if (state < 3 && mAllList.Contains(id))
		{
			mAllList.Remove(id);
			if (WindowUI.OnInGameWindowClose != null)
			{
				WindowUI.OnInGameWindowClose(id, mAllList);
			}
		}
	}

	public static bool IsWindowOpened(WindowID id)
	{
		string className = UIResourceDefine.windowClass[id].ClassName;
		IMediator mediator = Facade.Instance.RetrieveMediator(className);
		return mediator != null;
	}

	public static void ShowCurrency(WindowID id)
	{
		mBeforeCurrencyID = mCurrentCurrencyID;
		if (mCurrentCurrencyID != 0 && mCurrentCurrencyID != id)
		{
			CloseWindow(mCurrentCurrencyID);
		}
		mCurrentCurrencyID = id;
		ShowWindow(mCurrentCurrencyID);
	}

	private static void CloseCurrencyInternal()
	{
		if (mCurrentCurrencyID != 0)
		{
			CloseWindow(mCurrentCurrencyID);
			mCurrentCurrencyID = WindowID.WindowID_Invaild;
		}
	}

	public static void CloseCurrency()
	{
		CloseCurrencyInternal();
		OpenBeforeCurrency();
	}

	public static void OpenBeforeCurrency()
	{
		if (mBeforeCurrencyID != 0)
		{
			ShowCurrency(mBeforeCurrencyID);
		}
	}

	public static void ShowRewardSimple(List<Drop_DropModel.DropData> list)
	{
		if (list != null && list.Count != 0)
		{
			Facade.Instance.RegisterProxy(new RewardSimpleProxy(new RewardSimpleProxy.Transfer
			{
				list = list
			}));
			ShowWindow(WindowID.WindowID_RewardSimple);
		}
	}

	public static void ShowGoldBuy(CoinExchangeSource buytype, long needgold, Action<int> callback)
	{
		Facade.Instance.RegisterProxy(new GoldBuyModuleProxy(new GoldBuyModuleProxy.Transfer
		{
			buytype = buytype,
			gold = needgold,
			callback = callback
		}));
		ShowWindow(WindowID.WindowID_GoldBuy);
	}

	public static void ShowShopSingle(ShopSingleProxy.SingleType type, Action onclose = null)
	{
		ShopSingleProxy.Transfer transfer = new ShopSingleProxy.Transfer();
		transfer.type = type;
		ShopSingleProxy shopSingleProxy = new ShopSingleProxy(transfer);
		shopSingleProxy.Event_Para0 = onclose;
		Facade.Instance.RegisterProxy(shopSingleProxy);
		ShowWindow(WindowID.WindowID_ShopSingle);
	}

	public static void ShowServerAssert(long time)
	{
		ServerAssertProxy.Transfer transfer = new ServerAssertProxy.Transfer();
		transfer.assertendtime = time;
		ServerAssertProxy proxy = new ServerAssertProxy(transfer);
		Facade.Instance.RegisterProxy(proxy);
		ShowWindow(WindowID.WindowID_ServerAssert);
	}

	public static void ShowMask(bool value)
	{
		int num = mMaskCount;
		mMaskCount += (value ? 1 : (-1));
		int num2 = mMaskCount;
		if (mMaskCount == 1 && value)
		{
			ShowWindow(WindowID.WindowID_Mask);
		}
		else if (mMaskCount == 0 && !value)
		{
			CloseWindow(WindowID.WindowID_Mask);
		}
	}

	public static void ShowNetDoing(bool value, NetDoingType type = NetDoingType.netdoing_http)
	{
		mNetDoingCount += (value ? 1 : (-1));
		if (mNetDoingCount == 1 && value)
		{
			NetDoingProxy.Transfer transfer = new NetDoingProxy.Transfer();
			transfer.type = type;
			NetDoingProxy proxy = new NetDoingProxy(transfer);
			Facade.Instance.RegisterProxy(proxy);
			ShowWindow(WindowID.WindowID_NetDoing);
		}
		else if (mNetDoingCount == 0 && !value)
		{
			CloseWindow(WindowID.WindowID_NetDoing);
		}
	}

	public static void ShowLoading(Action loading, Action end1 = null, Action end2 = null, BattleLoadProxy.LoadingType type = BattleLoadProxy.LoadingType.eMiss)
	{
		BattleLoadProxy.BattleLoadData battleLoadData = new BattleLoadProxy.BattleLoadData();
		battleLoadData.LoadingDo = loading;
		battleLoadData.LoadEnd1Do = end1;
		battleLoadData.LoadEnd2Do = end2;
		battleLoadData.loadingType = type;
		Facade.Instance.RegisterProxy(new BattleLoadProxy(battleLoadData));
		ShowWindow(WindowID.WindowID_BattleLoad);
	}

	public static void TryLogin()
	{
		LocalSave.Instance.TryLogin(delegate(bool result, CRespUserLoginPacket data)
		{
			ulong serverUserID = LocalSave.Instance.GetServerUserID();
			if (!result && data != null && serverUserID != 0 && serverUserID != data.m_nUserRawId)
			{
				Action callback_sure = delegate
				{
					LocalSave.Instance.RefreshUserIDFromTemp();
					GameLogic.Hold.BattleData.RemoveStageLocal();
					LocalSave.Instance.StageDiscount_Init(null);
					LocalSave.Instance.DoLoginCallBack(data, delegate
					{
						ReOpenMain();
					});
				};
				Action callback_confirm = delegate
				{
					LocalSave.Instance.SetUserTemp(string.Empty, string.Empty);
				};
				ChangeAccountProxy proxy = new ChangeAccountProxy(new ChangeAccountProxy.Transfer
				{
					callback_sure = callback_sure,
					callback_confirm = callback_confirm
				});
				Facade.Instance.RegisterProxy(proxy);
				ShowWindow(WindowID.WindowID_ChangeAccount);
			}
		});
	}

	public static void ShowPopWindowUI(string title, string content, Action callback)
	{
		PopWindowProxy.Transfer transfer = new PopWindowProxy.Transfer();
		transfer.title = title;
		transfer.content = content;
		transfer.callback = callback;
		Facade.Instance.RegisterProxy(new PopWindowProxy(transfer));
		ShowWindow(WindowID.WindowID_PopWindow);
	}

	public static void ShowPopWindowOneUI(string title, string content, string sure, bool closebuttonshow, Action callback)
	{
		PopWindowOneProxy.Transfer transfer = new PopWindowOneProxy.Transfer();
		transfer.title = title;
		transfer.content = content;
		transfer.sure = sure;
		transfer.callback = callback;
		transfer.showclosebutton = closebuttonshow;
		Facade.Instance.RegisterProxy(new PopWindowOneProxy(transfer));
		ShowWindow(WindowID.WindowID_PopWindowOne);
	}

	public static void ShowRewardUI(List<Drop_DropModel.DropData> list)
	{
		Facade.Instance.RegisterProxy(new RewardShowProxy(new RewardShowProxy.Transfer
		{
			list = list
		}));
		ShowWindow(WindowID.WindowID_RewardShow);
	}

	public static void ShowAdInsideUI(AdInsideProxy.EnterSource source, Action callback)
	{
		Facade.Instance.RegisterProxy(new AdInsideProxy(new AdInsideProxy.Transfer
		{
			source = source,
			finish_callback = callback
		}));
		ShowWindow(WindowID.WindowID_AdInside);
	}

	public static void AddOpenWindow(WindowID id)
	{
		int state = UIResourceDefine.windowClass[id].State;
		switch (state)
		{
		case 1:
			mInGameList.Add(id);
			break;
		case 0:
			mOutGameList.Add(id);
			break;
		}
		if (state < 3)
		{
			mAllList.Add(id);
			if (WindowUI.OnInGameWindowOpen != null)
			{
				WindowUI.OnInGameWindowOpen(id, mAllList);
			}
		}
	}

	public static void AddCloseWindow(WindowID id)
	{
		switch (UIResourceDefine.windowClass[id].State)
		{
		case 1:
			if (mInGameList.Contains(id))
			{
				mInGameList.Remove(id);
			}
			break;
		case 0:
			if (mOutGameList.Contains(id))
			{
				mOutGameList.Remove(id);
			}
			break;
		}
	}

	public static bool GetOnlyMain()
	{
		if (mOutGameList.Count == 1 && mOutGameList[0] == WindowID.WindowID_Main)
		{
			return true;
		}
		return false;
	}

	public static void GameBegin()
	{
		CloseGameOut();
	}

	public static void GameOver()
	{
		CloseGameIn();
	}

	private static void CloseGameOut()
	{
		int i = 0;
		for (int count = mOutGameList.Count; i < count; i++)
		{
			CloseWindowInternal(mOutGameList[i]);
		}
		mOutGameList.Clear();
	}

	private static void CloseGameIn()
	{
		int i = 0;
		for (int count = mInGameList.Count; i < count; i++)
		{
			CloseWindowInternal(mInGameList[i]);
		}
		mInGameList.Clear();
	}

	public static void CloseAllWindows()
	{
		CloseGameOut();
		CloseGameIn();
	}

	private static bool GetReOpenMainClose(WindowID id)
	{
		bool result = true;
		if (id == WindowID.WindowID_BattleLoad || id == WindowID.WindowID_Mask || id == WindowID.WindowID_NetDoing)
		{
			result = false;
		}
		return result;
	}

	public static void ReOpenMain()
	{
		ShowLoading(delegate
		{
			Dictionary<WindowID, UIResourceDefine.WindowData>.Enumerator enumerator = UIResourceDefine.windowClass.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (GetReOpenMainClose(enumerator.Current.Key))
				{
					CloseWindowInternal(enumerator.Current.Key);
				}
			}
			mOutGameList.Clear();
			mInGameList.Clear();
			mAllList.Clear();
			ShowWindow(WindowID.WindowID_Main);
		});
	}

	private static void ShowPop(WindowID id)
	{
	}

	private static void Pop_MissBefore()
	{
	}

	public static void PopClose()
	{
	}

	private static void Update_Init()
	{
		if (!Update_bInit)
		{
			Update_bInit = true;
			DOTween.Sequence().AppendInterval(34.5f).AppendCallback(OnUpdate)
				.SetLoops(-1)
				.SetUpdate(isIndependentUpdate: true);
		}
	}

	private static void OnUpdate()
	{
		Dictionary<string, WindowMediator.WindowCacheData>.Enumerator enumerator = WindowMediator.mCacheUIPanel.GetEnumerator();
		WindowMediator.WindowCacheData value;
		do
		{
			if (enumerator.MoveNext())
			{
				value = enumerator.Current.Value;
				continue;
			}
			return;
		}
		while (Facade.Instance.RetrieveMediator(value.name) != null || !(Time.realtimeSinceStartup - enumerator.Current.Value.lasttime > 180f));
		WindowMediator.RemoveCache(value.name);
	}
}
