




using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleLevelUICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_Pause;

    public BattleExpCtrl mExpCtrl;

    public BattleBossHPCtrl mHPCtrl;

    public BattleGoldCtrl mGoldCtrl;

    public GameObject copyitems;

    public GameObject copyGold;

    public RectTransform Image_Gold;

    public BattleLevelAchieveCtrl mAchieveCtrl;

    public Transform challenge_parent;

    private ActionUpdateCtrl mActionUpdateCtrl;

    private BattleLevelWaveCtrl mLevelWaveCtrl;

    private Sequence seq_levelup;

    private int levelupCount;

    private Tweener tGold;

    private List<long> getgoldlist = new List<long>();

    private bool bGoldAniPlaying;

    private LocalUnityObjctPool mObjPool;

    protected override void OnInit()
    {
        if ((bool)copyGold)
        {
            mObjPool = LocalUnityObjctPool.Create(base.gameObject);
            mObjPool.CreateCache<MainUIGoldAddCtrl>(copyGold);
        }
        copyitems.SetActive(value: false);
    }

    protected override void OnOpen()
    {
        SdkManager.send_event_game_start(BattleSource.eWorld, GameLogic.Hold.BattleData.Level_CurrentStage);
        InitUI();
    }

    private void InitUI()
    {
        Debug.Log("@LOG BattleLevelUICtrl.InitUI");
        mAchieveCtrl.Show(value: false);
        mGoldCtrl.gameObject.SetActive(value: true);
        WindowUI.CloseCurrency();
        if ((bool)Button_Pause)
        {
            Button_Pause.gameObject.SetActive(!GameLogic.Hold.Guide.GetNeedGuide());
            Button_Pause.onClick = OnClickPause;
        }
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
        GameLogic.Hold.Sound.PlayBackgroundMusic(SoundManager.BackgroundMusicType.eBattle);
        CameraControlM.Instance.PlayStartAnimate();
        if (mExpCtrl != null)
        {
            mExpCtrl.Init();
        }
        if ((bool)mHPCtrl)
        {
            mHPCtrl.Init();
        }
        mActionUpdateCtrl = new ActionUpdateCtrl();
        mActionUpdateCtrl.Init();
        UpdateGold();
        ShowBossHP(show: false);
        StartGame();
    }

    private void init_level_wave()
    {
        if (mLevelWaveCtrl == null)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/BattleUI/waveparent"));
            gameObject.SetParentNormal(mExpCtrl.transform.parent);
            mLevelWaveCtrl = gameObject.GetComponent<BattleLevelWaveCtrl>();
            mLevelWaveCtrl.SetActive(value: false);
        }
    }

    protected override void OnClose()
    {
        if ((bool)mHPCtrl)
        {
            mHPCtrl.DeInit();
        }
        if (seq_levelup != null)
        {
            seq_levelup.Kill();
        }
        if ((bool)mExpCtrl)
        {
            mExpCtrl.DeInit();
        }
        mObjPool.Collect<MainUIGoldAddCtrl>();
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
        mActionUpdateCtrl.DeInit();
        GameLogic.SetGameState(GameLogic.EGameState.Over);
        GameLogic.Release.Release();
        if (mLevelWaveCtrl != null)
        {
            mLevelWaveCtrl.Deinit();
            UnityEngine.Object.Destroy(mLevelWaveCtrl.gameObject);
        }
    }

    protected void ShowBossHP(bool show)
    {
        if ((bool)mHPCtrl)
        {
            mHPCtrl.Show(show);
        }
        if ((bool)mExpCtrl)
        {
            mExpCtrl.Show(!show);
        }
    }

    private void OnClickPause()
    {
        WindowUI.ShowWindow(WindowID.WindowID_Pause);
    }

    private void OnGotoNextRoom(RoomGenerateBase.Room room)
    {
        if (GameLogic.Hold.Guide != null && Button_Pause != null)
        {
            Button_Pause.gameObject.SetActive(!GameLogic.Hold.Guide.GetNeedGuide());
        }
        else
        {
            Button_Pause.gameObject.SetActive(value: true);
        }
    }

    private void StartGame()
    {
        GameLogic.SetGameState(GameLogic.EGameState.Gaming);
    }

    private void OnCloseLevelUpUI()
    {
        levelupCount--;
        if (levelupCount < 0)
        {
            levelupCount = 0;
        }
        if (levelupCount > 0)
        {
            OpenLevelUpUI();
        }
    }

    private void OpenLevelUpUI()
    {
        GameLogic.Self.LevelUp();
        seq_levelup = DOTween.Sequence().AppendInterval(0.8f).AppendCallback(delegate
        {
            if (GameLogic.InGame)
            {
                Facade.Instance.RegisterProxy(new ChooseSkillProxy(new ChooseSkillProxy.Transfer
                {
                    type = ChooseSkillProxy.ChooseSkillType.eLevel
                }));
                WindowUI.ShowWindow(WindowID.WindowID_ChooseSkill);
            }
        });
    }

    private void CacheGoldText(MainUIGoldAddCtrl ctrl)
    {
        mObjPool.EnQueue<MainUIGoldAddCtrl>(ctrl.gameObject);
    }

    public override object OnGetEvent(string eventName)
    {
        if (eventName != null && eventName == "Event_GetGoldPosition")
        {
            return Image_Gold.position;
        }
        return null;
    }

    private void UpdateGold()
    {
        if ((bool)mGoldCtrl)
        {
            float gold = GameLogic.Hold.BattleData.GetGold();
            //Debug.LogFormat("@LOG UpdateGold gold: {0}", gold);
            mGoldCtrl.SetGold((long)gold);
        }
    }

    //@TODO CANNOT DECODE _003C_003Ef__switch_0024map7
    Dictionary<string, int> _003C_003Ef__switch_0024map7;

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if (name == null)
        {
            return;
        }
        if (_003C_003Ef__switch_0024map7 == null)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>(9);
            dictionary.Add("BATTLE_GET_GOLD", 0);
            dictionary.Add("BATTLE_GAMEOVER", 1);
            dictionary.Add("BATTLE_UI_BOSSHP_UPDATE", 2);
            dictionary.Add("BATTLE_EXP_UP", 3);
            dictionary.Add("BATTLE_LEVEL_UP", 4);
            dictionary.Add("BATTLE_CHOOSESKILL_TO_BATTLE_CLOSE", 5);
            dictionary.Add("BATTLE_ROOM_TYPE", 6);
            dictionary.Add("Currency_BattleKey", 7);
            dictionary.Add("BattleUI_level_wave_update", 8);
            _003C_003Ef__switch_0024map7 = dictionary;
        }
        if (!_003C_003Ef__switch_0024map7.TryGetValue(name, out int value))
        {
            return;
        }
        switch (value)
        {
            case 9:
                break;
            case 0:
                UpdateGold();
                break;
            case 1:
                if (GameConfig.GetCanOpenRateUI())
                {
                    WindowUI.ShowWindow(WindowID.WindowID_Rate);
                }
                else
                {
                    WindowUI.ShowWindow(WindowID.WindowID_GameOver);
                }
                break;
            case 2:
                if ((bool)mHPCtrl)
                {
                    ShowBossHP(show: true);
                    mHPCtrl.UpdateBossHP((float)body);
                    if (!mHPCtrl.IsShow() && (bool)mExpCtrl)
                    {
                        mExpCtrl.Show(show: true);
                    }
                }
                break;
            case 3:
                if ((bool)mExpCtrl)
                {
                    mExpCtrl.ExpUP((ProgressAniManager)body);
                }
                break;
            case 4:
                {
                    GameLogic.Hold.Sound.PlayUI(5000007);
                    int level = GameLogic.Self.m_EntityData.GetLevel();
                    levelupCount++;
                    if ((bool)mExpCtrl)
                    {
                        mExpCtrl.SetLevel(level);
                    }
                    if (levelupCount == 1)
                    {
                        OpenLevelUpUI();
                    }
                    break;
                }
            case 5:
                OnCloseLevelUpUI();
                break;
            case 6:
                {
                    RoomGenerateBase.RoomType roomType = (RoomGenerateBase.RoomType)body;
                    if (roomType != RoomGenerateBase.RoomType.eBoss)
                    {
                        ShowBossHP(show: false);
                    }
                    break;
                }
            case 7:
                if ((bool)body)
                {
                    WindowUI.ShowCurrency(WindowID.WindowID_CurrencyBattleKey);
                }
                else
                {
                    WindowUI.CloseCurrency();
                }
                break;
            case 8:
                {
                    BattleLevelWaveData info = (BattleLevelWaveData)body;
                    init_level_wave();
                    mLevelWaveCtrl.SetInfo(info);
                    break;
                }
        }
    }

    public override void OnLanguageChange()
    {
    }

    private void challenge_init()
    {
        mAchieveCtrl.Show(value: true);
        mGoldCtrl.gameObject.SetActive(value: false);
        GameLogic.Hold.BattleData.Challenge_SetUIParent(challenge_parent);
    }
}
