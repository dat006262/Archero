




using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
	private static volatile GameLauncher _Instance;

	private float touchtime;

	private bool bPause;

	public static GameLauncher Instance => _Instance;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		Screen.sleepTimeout = -1;
		GameLogic.Width = 720;
		GameLogic.Height = (int)((float)Screen.height * (float)GameLogic.Width / (float)Screen.width);
		GameLogic.WidthScale = (float)GameLogic.Width / (float)GameLogic.DesignWidth;
		GameLogic.HeightScale = (float)GameLogic.Height / (float)GameLogic.DesignHeight;
		GameLogic.HeightScale = (float)GameLogic.Height / (float)GameLogic.DesignHeight;
		GameLogic.ScreenSize = new Vector2((float)(Screen.width * GameLogic.DesignHeight) / (float)Screen.height, GameLogic.DesignHeight);
		GameLogic.WidthScaleAll = ((!(GameLogic.WidthScale < GameLogic.HeightScale)) ? 1f : (GameLogic.WidthScale / GameLogic.HeightScale));
		float num = (float)GameLogic.Width / (float)GameLogic.Height * (float)GameLogic.DesignHeight;
		num = (GameLogic.WidthReal = MathDxx.Clamp(num, num, GameLogic.DesignWidth));
		GameLogic.ResetMaxResolution();
	}

	private void Start()
	{
		_Instance = this;
		ResourceManager.Init();
		GameConfig.Init();
		SdkManager.set_first_setup_time();
		SdkManager.send_event("app_start");
		LocalModelManager.Instance.Stage_Level_chapter1.Init();
		LocalModelManager.Instance.Equip_equip.Init();
		LocalModelManager.Instance.Character_Level.Init();
		LocalModelManager.Instance.Stage_Level_stagechapter.Init();
		LocalModelManager.Instance.Stage_Level_activitylevel.Init();
		LocalModelManager.Instance.Stage_Level_activity.Init();
		LocalModelManager.Instance.Achieve_Achieve.Init();
		LocalModelManager.Instance.Shop_MysticShop.Init();
		CInstance<PlayerPrefsMgr>.Instance.Init();
		AdsRequestHelper.Init();
		AdsRequestHelper.getRewardedAdapter().isLoaded();
		LocalSave.Instance.InitData();
		LocalSave.Instance.BattleIn_CheckInit();
		GameLogic.Hold.Guide.Init();
		_InitNameGenerator();
		SdkManager.InitSdks();
		NetManager.mNetCache.Init();
		NetManager.StartPing();
		WindowUI.Init();
		_InitPureMVC();
	}

	protected void _InitNameGenerator()
	{
	}

	protected void _InitPureMVC()
	{
		new AppFacade();
	}

	private void Update()
	{
		if ((Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsPlayer) && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			if (Time.time - touchtime > 2f)
			{
				touchtime = Time.time;
				CInstance<TipsUIManager>.Instance.Show(GameLogic.Hold.Language.GetLanguageByTID("再按一次退出游戏"), (float)GameLogic.Height * 0.12f);
			}
			else
			{
				Application.Quit();
			}
		}
	}

	private void buy_gold()
	{
		CDiamondToCoin cDiamondToCoin = new CDiamondToCoin();
		cDiamondToCoin.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
		cDiamondToCoin.m_nCoins = 1u;
		cDiamondToCoin.m_nDiamonds = 1u;
		NetManager.SendInternal(cDiamondToCoin, SendType.eForceOnce, delegate(NetResponse response)
		{
#if ENABLE_NET_MANAGER
			if (response.IsSuccess && response.data != null && response.data is CRespDimaonToCoin)
			{
				CRespDimaonToCoin cRespDimaonToCoin = response.data as CRespDimaonToCoin;
				LocalSave.Instance.UserInfo_SetDiamond((int)cRespDimaonToCoin.m_nDiamonds);
				LocalSave.Instance.UserInfo_SetGold((int)cRespDimaonToCoin.m_nCoins);
				buy_gold();
			}
			else if (response.error != null)
			{
				CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
			}
#else
            CRespDimaonToCoin cRespDimaonToCoin = new CRespDimaonToCoin()
            {
                m_nCoins = 1u,
                m_nDiamonds = 1u
            };
            LocalSave.Instance.UserInfo_SetDiamond((int)cRespDimaonToCoin.m_nDiamonds);
            LocalSave.Instance.UserInfo_SetGold((int)cRespDimaonToCoin.m_nCoins);
            buy_gold();
#endif
        });
    }

	private void update_touch()
	{
		if (UnityEngine.Input.GetKeyUp(KeyCode.P))
		{
			bPause = !bPause;
			GameLogic.SetPause(bPause);
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.F))
		{
			CameraControlM.CameraFollow = !CameraControlM.CameraFollow;
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.T))
		{
			buy_gold();
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.V))
		{
			List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
			list.Add(new Drop_DropModel.DropData
			{
				type = PropType.eCurrency,
				id = 1,
				count = 2000
			});
			list.Add(new Drop_DropModel.DropData
			{
				type = PropType.eCurrency,
				id = 2,
				count = 100
			});
			list.Add(new Drop_DropModel.DropData
			{
				type = PropType.eCurrency,
				id = 3,
				count = 10
			});
			list.Add(new Drop_DropModel.DropData
			{
				type = PropType.eCurrency,
				id = 4,
				count = 3
			});
			if (MathDxx.RandomBool())
			{
				list.Add(new Drop_DropModel.DropData
				{
					type = PropType.eEquip,
					id = 1040101,
					count = 1
				});
				list.Add(new Drop_DropModel.DropData
				{
					type = PropType.eEquip,
					id = 1040102,
					count = 1
				});
				list.Add(new Drop_DropModel.DropData
				{
					type = PropType.eEquip,
					id = 30101,
					count = 5
				});
				list.Add(new Drop_DropModel.DropData
				{
					type = PropType.eEquip,
					id = 30103,
					count = 10
				});
			}
			WindowUI.ShowRewardSimple(list);
		}
	}
}
