




using System.Collections.Generic;

public class UIResourceDefine
{
	public class WindowData
	{
		public string ClassName;

		public WindowMediator.LayerType LayerType;

		public bool isPop;

		public int State;
	}

	public static Dictionary<WindowID, WindowData> windowClass = new Dictionary<WindowID, WindowData>
	{
		{
			WindowID.WindowID_Main,
			new WindowData
			{
				ClassName = "MainModuleMediator",
				LayerType = WindowMediator.LayerType.eRoot
			}
		},
		{
			WindowID.WindowID_Battle,
			new WindowData
			{
				ClassName = "BattleModuleMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1
			}
		},
		{
			WindowID.WindowID_BoxChoose,
			new WindowData
			{
				ClassName = "BoxChooseMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_GameOver,
			new WindowData
			{
				ClassName = "GameOverModuleMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1
			}
		},
		{
			WindowID.WindowID_Loading,
			new WindowData
			{
				ClassName = "LoadingModuleMediator",
				LayerType = WindowMediator.LayerType.eFrontMask
			}
		},
		{
			WindowID.WindowID_Setting,
			new WindowData
			{
				ClassName = "SettingMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_ChangeAccount,
			new WindowData
			{
				ClassName = "ChangeAccountMediator",
				LayerType = WindowMediator.LayerType.eFrontMask,
				State = 2,
				isPop = true
			}
		},
		{
			WindowID.WindowID_Char,
			new WindowData
			{
				ClassName = "CharModuleMediator",
				LayerType = WindowMediator.LayerType.eRoot
			}
		},
		{
			WindowID.WindowID_Mask,
			new WindowData
			{
				ClassName = "MaskModuleMediator",
				LayerType = WindowMediator.LayerType.eFrontMask,
				State = 2
			}
		},
		{
			WindowID.WindowID_Guide,
			new WindowData
			{
				ClassName = "GuideModuleMediator",
				LayerType = WindowMediator.LayerType.eFront2
			}
		},
		{
			WindowID.WindowID_ChooseSkill,
			new WindowData
			{
				ClassName = "ChooseSkillModuleMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1
			}
		},
		{
			WindowID.WindowID_GameTurnTable,
			new WindowData
			{
				ClassName = "GameTurnTableMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1
			}
		},
		{
			WindowID.WindowID_GameThree,
			new WindowData
			{
				ClassName = "GameThreeMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1
			}
		},
		{
			WindowID.WindowID_EventAngel,
			new WindowData
			{
				ClassName = "EventAngelMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1
			}
		},
		{
			WindowID.WindowID_EventBlackShop,
			new WindowData
			{
				ClassName = "EventBlackShopMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1,
				isPop = true
			}
		},
		{
			WindowID.WindowID_EventFirstShop,
			new WindowData
			{
				ClassName = "EventFirstShopMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1,
				isPop = true
			}
		},
		{
			WindowID.WindowID_EventDemon,
			new WindowData
			{
				ClassName = "EventDemonMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1
			}
		},
		{
			WindowID.WindowID_EquipWear,
			new WindowData
			{
				ClassName = "EquipWearModuleMediator",
				LayerType = WindowMediator.LayerType.eRoot
			}
		},
		{
			WindowID.WindowID_EventChect1,
			new WindowData
			{
				ClassName = "EventChest1Mediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1
			}
		},
		{
			WindowID.WindowID_BattleLoad,
			new WindowData
			{
				ClassName = "BattleLoadMediator",
				LayerType = WindowMediator.LayerType.eFrontMask,
				State = 2
			}
		},
		{
			WindowID.WindowID_UnlockStage,
			new WindowData
			{
				ClassName = "UnlockStageMediator",
				LayerType = WindowMediator.LayerType.eFront2
			}
		},
		{
			WindowID.WindowID_PopWindow,
			new WindowData
			{
				ClassName = "PopWindowMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				State = 2,
				isPop = true
			}
		},
		{
			WindowID.WindowID_Active,
			new WindowData
			{
				ClassName = "ActiveMediator",
				LayerType = WindowMediator.LayerType.eRoot
			}
		},
		{
			WindowID.WindowID_Pause,
			new WindowData
			{
				ClassName = "PauseModuleMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1,
				isPop = true
			}
		},
		{
			WindowID.WindowID_Rate,
			new WindowData
			{
				ClassName = "RateModuleMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1,
				isPop = true
			}
		},
		{
			WindowID.WindowID_Currency,
			new WindowData
			{
				ClassName = "CurrencyModuleMediator",
				LayerType = WindowMediator.LayerType.eFront2,
				State = 3
			}
		},
		{
			WindowID.WindowID_EquipInfo,
			new WindowData
			{
				ClassName = "EquipInfoModuleMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				State = 2,
				isPop = true
			}
		},
		{
			WindowID.WindowID_CurrencyEquip,
			new WindowData
			{
				ClassName = "CurrencyEquipMediator",
				LayerType = WindowMediator.LayerType.eFront,
				State = 2
			}
		},
		{
			WindowID.WindowID_GoldBuy,
			new WindowData
			{
				ClassName = "GoldBuyMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true,
				State = 2
			}
		},
		{
			WindowID.WindowID_KeyBuy,
			new WindowData
			{
				ClassName = "KeyBuyMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_EquipCombine,
			new WindowData
			{
				ClassName = "EquipCombineMediator",
				LayerType = WindowMediator.LayerType.eFront
			}
		},
		{
			WindowID.WindowID_CurrencyBattleKey,
			new WindowData
			{
				ClassName = "CurrencyBattleKeyMediator",
				LayerType = WindowMediator.LayerType.eRoot,
				State = 2
			}
		},
		{
			WindowID.WindowID_LevelUp,
			new WindowData
			{
				ClassName = "LevelUpMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				State = 2,
				isPop = true
			}
		},
		{
			WindowID.WindowID_RewardShow,
			new WindowData
			{
				ClassName = "RewardShowMediator",
				LayerType = WindowMediator.LayerType.eFront2
			}
		},
		{
			WindowID.WindowID_EventRewardTurn,
			new WindowData
			{
				ClassName = "EventRewardTurnMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1,
				isPop = true
			}
		},
		{
			WindowID.WindowID_Login,
			new WindowData
			{
				ClassName = "LoginMediator",
				LayerType = WindowMediator.LayerType.eFrontNet
			}
		},
		{
			WindowID.WindowID_Card,
			new WindowData
			{
				ClassName = "CardModuleMediator",
				LayerType = WindowMediator.LayerType.eRoot
			}
		},
		{
			WindowID.WindowID_EquipCombineUp,
			new WindowData
			{
				ClassName = "EquipCombineUpMediator",
				LayerType = WindowMediator.LayerType.eFront2
			}
		},
		{
			WindowID.WindowID_AdHarvest,
			new WindowData
			{
				ClassName = "AdHarvestMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_BoxOpen,
			new WindowData
			{
				ClassName = "BoxOpenModuleMediator",
				LayerType = WindowMediator.LayerType.eFront2
			}
		},
		{
			WindowID.WindowID_CardLevelUp,
			new WindowData
			{
				ClassName = "CardLevelUpMediator",
				LayerType = WindowMediator.LayerType.eFront2
			}
		},
		{
			WindowID.WindowID_Challenge,
			new WindowData
			{
				ClassName = "ChallengeMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_BoxOpenOne,
			new WindowData
			{
				ClassName = "BoxOpenOneMediator",
				LayerType = WindowMediator.LayerType.eFront3
			}
		},
		{
			WindowID.WindowID_Achieve,
			new WindowData
			{
				ClassName = "AchieveMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_Mail,
			new WindowData
			{
				ClassName = "MailMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_MailInfo,
			new WindowData
			{
				ClassName = "MailInfoMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_Shop,
			new WindowData
			{
				ClassName = "ShopMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_ShopSingle,
			new WindowData
			{
				ClassName = "ShopSingleMediator",
				LayerType = WindowMediator.LayerType.eFront3,
				isPop = true,
				State = 2
			}
		},
		{
			WindowID.WindowID_SettingDebug,
			new WindowData
			{
				ClassName = "SettingDebugMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_Language,
			new WindowData
			{
				ClassName = "LanguageMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_Producer,
			new WindowData
			{
				ClassName = "ProducterMediator",
				LayerType = WindowMediator.LayerType.eFront2,
				isPop = true
			}
		},
		{
			WindowID.WindowID_CheckBattleIn,
			new WindowData
			{
				ClassName = "CheckBattleInMediator",
				LayerType = WindowMediator.LayerType.eFront2
			}
		},
		{
			WindowID.WindowID_RewardSimple,
			new WindowData
			{
				ClassName = "RewardSimpleMediator",
				LayerType = WindowMediator.LayerType.eFront3,
				State = 2,
				isPop = true
			}
		},
		{
			WindowID.WindowID_StageList,
			new WindowData
			{
				ClassName = "StageListMediator",
				LayerType = WindowMediator.LayerType.eFront2,
				isPop = true
			}
		},
		{
			WindowID.WindowID_LevelBox,
			new WindowData
			{
				ClassName = "LevelBoxMediator",
				LayerType = WindowMediator.LayerType.eFront2
			}
		},
		{
			WindowID.WindowID_StageBox,
			new WindowData
			{
				ClassName = "StageBoxMediator",
				LayerType = WindowMediator.LayerType.eFront2
			}
		},
		{
			WindowID.WindowID_NetDoing,
			new WindowData
			{
				ClassName = "NetDoingMediator",
				LayerType = WindowMediator.LayerType.eFrontNet,
				State = 2
			}
		},
		{
			WindowID.WindowID_BattleReborn,
			new WindowData
			{
				ClassName = "BattleRebornMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1,
				isPop = true
			}
		},
		{
			WindowID.WindowID_PurchaseOK,
			new WindowData
			{
				ClassName = "PurChaseOKMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_GoldBuyActive,
			new WindowData
			{
				ClassName = "GoldBuyActiveMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_EventFirstGold,
			new WindowData
			{
				ClassName = "EventFirstGoldMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1
			}
		},
		{
			WindowID.WindowID_LayerBox,
			new WindowData
			{
				ClassName = "LayerBoxMediator",
				LayerType = WindowMediator.LayerType.eFront
			}
		},
		{
			WindowID.WindowID_VideoPlay,
			new WindowData
			{
				ClassName = "VideoMediator",
				LayerType = WindowMediator.LayerType.eFrontMask
			}
		},
		{
			WindowID.WindowID_EquipBuyInfo,
			new WindowData
			{
				ClassName = "EquipBuyInfoMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1,
				isPop = true
			}
		},
		{
			WindowID.WindowID_ForceUpdate,
			new WindowData
			{
				ClassName = "ForceUpdateMediator",
				LayerType = WindowMediator.LayerType.eFrontMask,
				State = 2,
				isPop = true
			}
		},
		{
			WindowID.WindowID_PopWindowOne,
			new WindowData
			{
				ClassName = "PopWindowOneMediator",
				LayerType = WindowMediator.LayerType.eFront3,
				State = 2,
				isPop = true
			}
		},
		{
			WindowID.WindowID_TestNotice,
			new WindowData
			{
				ClassName = "TestNoticeMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_BuyGoldSure,
			new WindowData
			{
				ClassName = "BuyGoldSureMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_EventAdTurnTable,
			new WindowData
			{
				ClassName = "EventAdTurnTableMediator",
				LayerType = WindowMediator.LayerType.eInGame,
				State = 1,
				isPop = true
			}
		},
		{
			WindowID.WindowID_BoxOpenSingle,
			new WindowData
			{
				ClassName = "BoxOpenSingleMediator",
				LayerType = WindowMediator.LayerType.eFront2
			}
		},
		{
			WindowID.WindowID_ServerAssert,
			new WindowData
			{
				ClassName = "ServerAssertMediator",
				LayerType = WindowMediator.LayerType.eFrontMask,
				State = 2,
				isPop = true
			}
		},
		{
			WindowID.WindowID_AdInside,
			new WindowData
			{
				ClassName = "AdInsideMediator",
				LayerType = WindowMediator.LayerType.eFrontMask,
				State = 3
			}
		},
		{
			WindowID.WindowID_Report,
			new WindowData
			{
				ClassName = "ReportMediator",
				LayerType = WindowMediator.LayerType.eFront
			}
		}
	};

	public static string UIPrefabPath = "UIPanel/";

	public static WindowMediator.LayerType GetWindowLayerType(string classname)
	{
		Dictionary<WindowID, WindowData>.Enumerator enumerator = windowClass.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.ClassName == classname)
			{
				return enumerator.Current.Value.LayerType;
			}
		}
		return WindowMediator.LayerType.eRoot;
	}

	public static bool GetWindowPop(string classname)
	{
		Dictionary<WindowID, WindowData>.Enumerator enumerator = windowClass.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.ClassName == classname)
			{
				return enumerator.Current.Value.isPop;
			}
		}
		return false;
	}
}
