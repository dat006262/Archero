




using DG.Tweening;
using Dxx.Net;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class GameOverLevelCtrl : GameOverModeCtrlBase
{
    public Image Image_Title;

    public RectTransform Title_Left;

    public RectTransform Title_Right;

    public Image Image_BG;

    public GameObject stageparent;

    public GameObject goldparent;

    public GameObject bestparent;

    public GameObject getparent;

    public GameObject getparents;

    public RectTransform topnode;

    public Text Text_ReachLevel;

    public Text Text_Stage;

    public Text Text_Layer;

    public Text Text_GoldName;

    public Text Text_Get;

    public Text Text_Beat;

    public Text Text_Close;

    public Image Image_NewBest;

    public GoldTextCtrl mScoreCtrl;

    public ScrollRectBase mScrollRect;

    public Image viewpoint;

    public ButtonCtrl Button_Close;

    public GameOver_NoNetCtrl mNoNetCtrl;

    public GameOverChallengeCtrl mChallengeCtrl;

    public MainUIBattleLevelCtrl mLevelCtrl;

    private const float TextStartScale = 1.5f;

    private const float playTime = 0.15f;

    private const float DropWidth = 130f;

    private const float DropHeight = 130f;

    private const float DropTop = 10f;

    private const float DropTime = 0.12f;

    private const int LineCount = 5;

    private const float EquipScale = 0.9f;

    private int gochapter;

    private int gostage;

    private int alllayer;

    private bool bNewBest;

    private int getgold;

    private int getexp;

    private float imagebgy;

    private float imagetitlex;

    private GameObject copyitem;

    private LocalUnityObjctPool mPool;

    private List<PropOneEquip> mDropList = new List<PropOneEquip>();

    private List<LocalSave.EquipOne> mEquipDatas = new List<LocalSave.EquipOne>();

    private float startscale = 0.3f;

    private int mNetBackState;

    private bool bShowGot;

    private void InitItem()
    {
        copyitem = CInstance<UIResourceCreator>.Instance.GetPropOneEquip(base.transform).gameObject;
        copyitem.SetActive(value: false);
        mPool = LocalUnityObjctPool.Create(base.gameObject);
        mPool.CreateCache<PropOneEquip>(copyitem);
    }

    protected override void OnInit()
    {
        Vector2 sizeDelta = Image_BG.rectTransform.sizeDelta;
        imagebgy = sizeDelta.y;
        Vector2 sizeDelta2 = Image_Title.rectTransform.sizeDelta;
        imagetitlex = sizeDelta2.x;
        InitItem();
        Button_Close.onClick = OnClickClose;
    }

    protected override void OnOpen()
    {
        mNetBackState = 0;
        bShowGot = false;
        mChallengeCtrl.Show(value: false);
        if (GameLogic.Hold.BattleData.Challenge_ismainchallenge())
        {
            mChallengeCtrl.Show(value: true);
            LocalSave.Instance.Achieve_ExcuteCurrentStage();
            if (LocalSave.Instance.Achieve_IsFinish(GameLogic.Hold.BattleData.ActiveID))
            {
                mChallengeCtrl.SetContent("挑战成功...");
            }
            else
            {
                mChallengeCtrl.SetContent("挑战失败...");
            }
        }
        mPool.Collect<PropOneEquip>();
        mNoNetCtrl.SetShow(value: false);
        topnode.anchoredPosition = new Vector2(0f, -200f);
        Image_NewBest.gameObject.SetActive(value: false);
        Text_ReachLevel.gameObject.SetActive(value: false);
        stageparent.gameObject.SetActive(value: false);
        bestparent.gameObject.SetActive(value: false);
        getparent.gameObject.SetActive(value: false);
        getparents.gameObject.SetActive(value: false);
        Button_Close.gameObject.SetActive(value: false);
        Text_Close.gameObject.SetActive(value: false);
        Image_Title.gameObject.SetActive(value: false);
        Image_BG.gameObject.SetActive(value: false);
        Title_Left.gameObject.SetActive(value: false);
        Title_Right.gameObject.SetActive(value: false);
        mLevelCtrl.gameObject.SetActive(value: false);
        Image_NewBest.transform.localScale = Vector3.one * startscale;
        Text_ReachLevel.transform.localScale = Vector3.one * startscale;
        RectTransform rectTransform = Image_Title.rectTransform;
        Vector2 sizeDelta = Image_Title.rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(0f, sizeDelta.y);
        RectTransform rectTransform2 = Image_BG.rectTransform;
        Vector2 sizeDelta2 = Image_BG.rectTransform.sizeDelta;
        rectTransform2.sizeDelta = new Vector2(sizeDelta2.x, 0f);
        Title_Left.transform.localScale = Vector3.one;
        Title_Right.transform.localScale = Vector3.one;
        stageparent.transform.localScale = Vector3.one * startscale;
        bestparent.transform.localScale = Vector3.one * startscale;
        getparent.transform.localScale = Vector3.one * 1.5f;
        getparents.transform.localScale = Vector3.one * 1.5f;
        mLevelCtrl.transform.localScale = Vector3.one * startscale;
        gochapter = GameLogic.Hold.BattleData.Level_CurrentStage;
        gostage = GameLogic.Hold.BattleData.GetLayer();
        getexp = LocalModelManager.Instance.Stage_Level_stagechapter.GetExp();
        mLevelCtrl.UpdateLevel();
        getgold = (int)GameLogic.Hold.BattleData.GetGold();
        alllayer = get_all_layer();
        LocalSave.Instance.Stage_CheckUnlockNext(gostage);
        LocalSave.Instance.mStage.UpdateMaxLevel(alllayer);
        bNewBest = LocalSave.Instance.mStage.bNewBestLevel;
        LocalSave.Instance.mStage.bNewBestLevel = false;
        mEquipDatas = GameLogic.Hold.BattleData.GetEquips();
        excute_reward();
        int @int = PlayerPrefsEncrypt.GetInt("game_end_newbest");
        @int++;
        int num = 0;
        if (bNewBest)
        {
            num = 0;
            PlayerPrefsEncrypt.SetInt("game_end_newbest", 0);
        }
        else
        {
            PlayerPrefsEncrypt.SetInt("game_end_newbest", @int);
            num = @int;
        }
        int survive_times = GameConfig.GetRebornCount() - GameLogic.Hold.BattleData.GetRebornCount();
        int levelUpCount = LocalModelManager.Instance.Character_Level.GetLevelUpCount(getexp);
        int level = LocalSave.Instance.GetLevel() + levelUpCount;
        SdkManager.send_event_game_end(survive_times, BattleSource.eWorld, BattleEndType.EMAIN_GAMEOVER, getgold, mEquipDatas.Count, gochapter, alllayer, num, getexp, levelUpCount, level);
        UpdateUI();
        SendGameOver();
    }

    private void excute_reward()
    {
        TurnTableType rewardType = GameLogic.Hold.BattleData.GetRewardType();
        int num = 1;
        int num2 = 1;
        switch (rewardType)
        {
            case TurnTableType.Reward_Gold2:
                num = 2;
                break;
            case TurnTableType.Reward_Gold3:
                num = 3;
                break;
            case TurnTableType.Reward_Item2:
                num2 = 2;
                break;
            case TurnTableType.Reward_Item3:
                num2 = 3;
                break;
            case TurnTableType.Reward_All2:
                num = 2;
                num2 = 2;
                break;
            case TurnTableType.Reward_All3:
                num = 3;
                num2 = 3;
                break;
        }
        getgold *= num;
        List<LocalSave.EquipOne> list = new List<LocalSave.EquipOne>();
        int i = 0;
        for (int count = mEquipDatas.Count; i < count; i++)
        {
            LocalSave.EquipOne equipOne = mEquipDatas[i];
            if (equipOne.Overlying)
            {
                for (int j = 0; j < num2; j++)
                {
                    equipOne.Count *= num2;
                }
                list.Add(equipOne);
            }
            else
            {
                for (int k = 0; k < num2; k++)
                {
                    list.Add(equipOne);
                }
            }
        }
        mEquipDatas = list;
    }

    protected override void OnClose()
    {
        mPool.Collect<PropOneEquip>();
        mDropList.Clear();
    }

    private int get_all_layer()
    {
        int allMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(gochapter - 1);
        return gostage + allMaxLevel;
    }

    private void SendGameOver()
    {
        if (mEquipDatas.Count > 0)
        {
            LocalSave.Instance.mGuideData.check_diamondbox_first_open();
        }
        int i = 0;
        for (int count = mEquipDatas.Count; i < count; i++)
        {
            LocalSave.EquipOne equipOne = mEquipDatas[i];
            SdkManager.send_event_equipment("GET", equipOne.EquipID, equipOne.Count, 1, EquipSource.EMain_battle, 0);
        }
        List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
        if (getgold > 0)
        {
            list.Add(new Drop_DropModel.DropData
            {
                type = PropType.eCurrency,
                id = 1,
                count = getgold
            });
        }
        if (getexp > 0)
        {
            list.Add(new Drop_DropModel.DropData
            {
                type = PropType.eCurrency,
                id = 1001,
                count = getexp
            });
        }
        int j = 0;
        for (int count2 = mEquipDatas.Count; j < count2; j++)
        {
            list.Add(new Drop_DropModel.DropData
            {
                type = PropType.eEquip,
                id = mEquipDatas[j].EquipID,
                count = mEquipDatas[j].Count
            });
        }
        CReqItemPacket itemPacket = NetManager.GetItemPacket(list, addequipmust: true);
        itemPacket.m_nPacketType = 1;
        itemPacket.m_nExtraInfo = (uint)alllayer;
        NetManager.SendInternal(itemPacket, SendType.eCache, delegate (NetResponse response)
        {
#if ENABLE_NET_MANAGER
            if (response.IsSuccess)
#endif
            {
                mNetBackState = 2;
            }
#if ENABLE_NET_MANAGER
            else
            {
                mNetBackState = 1;
            }
#endif
        });
    }

    private void UpdateUI()
    {
        RectTransform rectTransform = Image_Title.rectTransform;
        Vector2 sizeDelta = Image_Title.rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(0f, sizeDelta.y);
        RectTransform rectTransform2 = Image_BG.rectTransform;
        Vector2 sizeDelta2 = Image_BG.rectTransform.sizeDelta;
        rectTransform2.sizeDelta = new Vector2(sizeDelta2.x, 0f);
        UpdateImageTitleLeftRight();
        Text_Stage.text = GameLogic.Hold.Language.GetLanguageByTID("ChapterIndex_x", gochapter);
        Text_Layer.text = gostage.ToString();
        float num = 0.2f;
        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(isIndependentUpdate: true);
        sequence.AppendInterval(0.2f);
        if (bNewBest)
        {
            sequence.AppendCallback(delegate
            {
                Image_NewBest.gameObject.SetActive(value: true);
            });
            sequence.Append(Image_NewBest.transform.DOScale(1f, 0.3f).SetUpdate(isIndependentUpdate: true).SetEase(Ease.OutBack)).SetUpdate(isIndependentUpdate: true);
        }
        sequence.AppendCallback(delegate
        {
            Image_Title.gameObject.SetActive(value: true);
            Title_Left.gameObject.SetActive(value: true);
            Title_Right.gameObject.SetActive(value: true);
        });
        Sequence s = sequence;
        RectTransform rectTransform3 = Image_Title.rectTransform;
        float x = imagetitlex;
        Vector2 sizeDelta3 = Image_Title.rectTransform.sizeDelta;
        s.Append(rectTransform3.DOSizeDelta(new Vector2(x, sizeDelta3.y), 0.4f).OnUpdate(UpdateImageTitleLeftRight).SetUpdate(isIndependentUpdate: true)
            .SetEase(Ease.OutBack));
        Sequence s2 = DOTween.Sequence().AppendInterval(0.25f).AppendCallback(delegate
        {
            Image_BG.gameObject.SetActive(value: true);
        });
        RectTransform rectTransform4 = Image_BG.rectTransform;
        Vector2 sizeDelta4 = Image_BG.rectTransform.sizeDelta;
        Sequence t = s2.Append(rectTransform4.DOSizeDelta(new Vector2(sizeDelta4.x, imagebgy), 0.3f).SetUpdate(isIndependentUpdate: true).SetEase(Ease.OutBack)).SetUpdate(isIndependentUpdate: true);
        sequence.Join(t);
        sequence.AppendInterval(0.3f);
        sequence.AppendCallback(delegate
        {
            Text_ReachLevel.gameObject.SetActive(value: true);
        });
        sequence.Append(Text_ReachLevel.transform.DOScale(1f, num).SetUpdate(isIndependentUpdate: true).SetEase(Ease.OutBack));
        sequence.AppendCallback(delegate
        {
            stageparent.gameObject.SetActive(value: true);
        });
        sequence.Append(stageparent.transform.DOScale(1f, num).SetUpdate(isIndependentUpdate: true).SetEase(Ease.OutBack));
        sequence.AppendCallback(delegate
        {
            mLevelCtrl.gameObject.SetActive(value: true);
        });
        sequence.Append(mLevelCtrl.transform.DOScale(0.9f, num).SetUpdate(isIndependentUpdate: true).SetEase(Ease.OutBack));
        sequence.AppendInterval(0.1f);
        if (getexp > 0)
        {
            mLevelCtrl.AddExpAnimation(getexp, sequence);
        }
        sequence.AppendCallback(delegate
        {
            bestparent.gameObject.SetActive(value: true);
        });
        sequence.Append(bestparent.transform.DOScale(1f, num).SetUpdate(isIndependentUpdate: true).SetEase(Ease.OutBack));
        sequence.AppendInterval(num * 0.5f);
        if (HaveReward())
        {
            sequence.Append(topnode.DOAnchorPosY(-30f, num).SetUpdate(isIndependentUpdate: true).SetEase(Ease.Linear));
            sequence.AppendInterval(num * 0.5f);
            sequence.AppendCallback(delegate
            {
                PlayGet();
            });
        }
        else
        {
            sequence.AppendCallback(AnimationEnd);
        }
    }

    private void UpdateImageTitleLeftRight()
    {
        Vector2 sizeDelta = Image_Title.rectTransform.sizeDelta;
        float x = sizeDelta.x;
        RectTransform title_Left = Title_Left;
        float x2 = (0f - x) / 2f - 20f;
        Vector2 anchoredPosition = Title_Left.anchoredPosition;
        title_Left.anchoredPosition = new Vector2(x2, anchoredPosition.y);
        RectTransform title_Right = Title_Right;
        float x3 = x / 2f + 20f;
        Vector2 anchoredPosition2 = Title_Right.anchoredPosition;
        title_Right.anchoredPosition = new Vector2(x3, anchoredPosition2.y);
    }

    private void InitGet()
    {
        mScrollRect.verticalNormalizedPosition = 1f;
        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(isIndependentUpdate: true);
        if (getgold > 0)
        {
            AddGoldOne(sequence);
        }
        int count = mEquipDatas.Count;
        for (int i = 0; i < count; i++)
        {
            AddEquipOne(sequence, i);
        }
        sequence.AppendCallback(delegate
        {
            AnimationEnd();
        });
        Vector2 sizeDelta = mScrollRect.content.sizeDelta;
        float y = sizeDelta.y;
        int num = count;
        if (getgold > 0)
        {
            num++;
        }
        float num2 = (num % 5 != 0) ? ((float)(num / 5) * 130f + 130f) : ((float)(num / 5) * 130f);
        y = num2;
        RectTransform content = mScrollRect.content;
        Vector2 sizeDelta2 = mScrollRect.content.sizeDelta;
        content.sizeDelta = new Vector2(sizeDelta2.x, y);
        viewpoint.enabled = (num > 10);
    }

    private void AddGoldOne(Sequence s)
    {
        s.AppendCallback(delegate
        {
            PropOneEquip propOneEquip = mPool.DeQueue<PropOneEquip>();
            propOneEquip.transform.SetParentNormal(mScrollRect.content);
            propOneEquip.transform.SetTop();
            propOneEquip.InitCurrency(1, getgold);
            mDropList.Add(propOneEquip);
            propOneEquip.transform.localScale = Vector3.one * 1.5f;
            propOneEquip.transform.DOScale(Vector3.one * 0.9f, 0.12f).SetUpdate(isIndependentUpdate: true);
            float x = -260f;
            (propOneEquip.transform as RectTransform).anchoredPosition = new Vector3(x, -10f);
        });
        s.AppendInterval(0.12f);
    }

    private void AddEquipOne(Sequence s, int index)
    {
        PropOneEquip one = null;
        s.AppendCallback(delegate
        {
            one = mPool.DeQueue<PropOneEquip>();
            one.SetAlreadyGet(alreadyget: true);
            one.InitEquip(mEquipDatas[index].EquipID, mEquipDatas[index].Count);
            one.transform.SetParentNormal(mScrollRect.content);
            one.transform.SetTop();
            mDropList.Add(one);
            one.transform.localScale = Vector3.one * 1.5f;
            one.transform.DOScale(Vector3.one * 0.9f, 0.12f).SetUpdate(isIndependentUpdate: true);
            if (getgold > 0)
            {
                index++;
            }
            float x = -260f + (float)(index % 5) * 130f;
            float y = (float)(index / 5) * -130f - 10f;
            (one.transform as RectTransform).anchoredPosition = new Vector3(x, y);
        });
        s.AppendInterval(0.22f);
    }

    private void PlayGetInternal()
    {
        if (!bShowGot)
        {
            bShowGot = true;
            Sequence sequence = DOTween.Sequence();
            sequence.SetUpdate(isIndependentUpdate: true);
            sequence.AppendCallback(delegate
            {
                getparent.gameObject.SetActive(value: true);
            });
            sequence.Append(getparent.transform.DOScale(Vector3.one, 0.15f).SetUpdate(isIndependentUpdate: true));
            sequence.AppendInterval(0.15f);
            sequence.AppendCallback(delegate
            {
                getparents.gameObject.SetActive(value: true);
            });
            sequence.Append(getparents.transform.DOScale(Vector3.one, 0.15f).SetUpdate(isIndependentUpdate: true));
            sequence.AppendInterval(0.15f);
            sequence.AppendCallback(delegate
            {
                InitGet();
            });
        }
    }

    private void PlayGet()
    {
        PlayGetInternal();
    }

    private void AnimationEnd()
    {
        DOTween.Sequence().AppendInterval(0.2f).AppendCallback(delegate
        {
            Text_Close.gameObject.SetActive(value: true);
            Button_Close.gameObject.SetActive(value: true);
            Text_Close.color = new Color(1f, 1f, 1f, 0f);
            Text_Close.DOKill();
            Text_Close.DOFade(1f, 1.5f).SetUpdate(isIndependentUpdate: true).SetLoops(-1, LoopType.Yoyo);
        })
            .SetUpdate(isIndependentUpdate: true);
    }

    private void OnClickClose()
    {
        Debug.Log("@LOG GameOverLevelCtrl.OnClickClose");
        WindowUI.ShowLoading(delegate
        {
            Debug.Log("@LOG GameOverLevelCtrl.OnClickClose");
            WindowUI.ShowWindow(WindowID.WindowID_Main);
            LocalSave.Instance.Modify_Gold(getgold, updateui: false);
            PlayRewards();
        });
    }

    public void PlayRewards()
    {
        if (getgold > 0)
        {
            Facade.Instance.SendNotification("MainUI_GetGold", getgold);
        }
    }

    private bool HaveReward()
    {
        return getgold > 0 || mEquipDatas.Count > 0;
    }

    public override object OnGetEvent(string eventName)
    {
        return base.OnGetEvent(eventName);
    }

    public override void OnHandleNotification(INotification notification)
    {
    }

    public override void OnLanguageChange()
    {
        Text_ReachLevel.text = GameLogic.Hold.Language.GetLanguageByTID("GameOver_Reach");
        Text_GoldName.text = GameLogic.Hold.Language.GetLanguageByTID("GameOver_Gold");
        int layer = LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(gochapter - 1) + gostage;
        string beat = LocalModelManager.Instance.Beat_beat.GetBeat(layer);
        Text_Beat.text = GameLogic.Hold.Language.GetLanguageByTID("击败", beat);
        Text_Close.text = GameLogic.Hold.Language.GetLanguageByTID("TapToClose");
        Text_Get.text = GameLogic.Hold.Language.GetLanguageByTID("GameOver_Items");
        mNoNetCtrl.OnLanguageUpdate();
    }
}
