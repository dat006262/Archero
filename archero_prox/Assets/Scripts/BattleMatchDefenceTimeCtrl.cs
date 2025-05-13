




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using UnityEngine;

public class BattleMatchDefenceTimeCtrl : BattleLevelUICtrl
{
    public BattleMatchDefenceTime_DeadCtrl mDeadCtrl;

    private Transform parent;

    private BattleMatchDefenceTime_ConditionCtrl mCtrl;

    private SequencePool mPool = new SequencePool();

    protected override void OnInit()
    {
        base.OnInit();
        GameObject gameObject = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/BattleUI/BattleMatchDefenceTime_Condition"));
        gameObject.SetParentNormal(GameNode.m_InGame2);
        RectTransform rectTransform = gameObject.transform as RectTransform;
        rectTransform.anchoredPosition = new Vector2(0f, PlatformHelper.GetFringeHeight());
        parent = rectTransform;
        mCtrl = parent.GetComponent<BattleMatchDefenceTime_ConditionCtrl>();
        mExpCtrl.SetFringe();
        mDeadCtrl.Show(value: false);
    }

    protected override void OnOpen()
    {
        mPool.Clear();
        if (mExpCtrl != null)
        {
            mExpCtrl.SetDropExp(GameLogic.Hold.BattleData.Challenge_DropExp());
        }
        if ((bool)parent)
        {
            parent.gameObject.SetActive(value: true);
        }
        base.OnOpen();
        GameLogic.Hold.BattleData.Challenge_SetUIParent(parent);
    }

    protected override void OnClose()
    {
        mPool.Clear();
        if ((bool)parent)
        {
            parent.gameObject.SetActive(value: false);
        }
        base.OnClose();
    }

    public override object OnGetEvent(string eventName)
    {
        return base.OnGetEvent(eventName);
    }

    public override void OnHandleNotification(INotification notification)
    {
        base.OnHandleNotification(notification);
        string name = notification.Name;
        object body = notification.Body;
        if (name == null)
        {
            return;
        }
        if (!(name == "BATTLE_GET_GOLD"))
        {
            if (!(name == "BATTLE_ROOM_TYPE") && name == "MatchDefenceTime_me_dead")
            {
                Sequence s = mPool.Get();
                s.AppendInterval(0.6f).AppendCallback(delegate
                {
                    GameLogic.SetPause(pause: true);
                    mDeadCtrl.Show(value: true);
                    mDeadCtrl.SetTime(10, delegate
                    {
                        GameLogic.SetPause(pause: false);
                        GameLogic.Self.DoRebornInternal();
                    });
                });
            }
        }
        else
        {
            int num = (int)GameLogic.Hold.BattleData.GetGold();
            GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_me_updatescore", num);
            Singleton<MatchDefenceTimeSocketCtrl>.Instance.Send(MatchMessageType.eScoreUpdate, num);
        }
    }

    public override void OnLanguageChange()
    {
        base.OnLanguageChange();
    }
}
