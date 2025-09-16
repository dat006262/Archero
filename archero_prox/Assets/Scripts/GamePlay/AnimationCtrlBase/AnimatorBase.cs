




using UnityEngine;

//@TODO Show info
//[System.Serializable]
public class AnimatorBase
{
    public const int AttackPrev = 20;

    public const int AttackNext = 21;

    public const int Dead = 100;

    public const int Hitted = 200;

    public EntityBase m_Entity;

    private Animation m_AnimationBase;

    public virtual void Init(EntityBase entity)
    {
        UnityEngine.Debug.Log("@LOG AnimatorBase.Init entity:" + entity.name);
        m_Entity = entity;
        if (m_Entity.mAniCtrlBase != null)
        {
            m_Entity.mAniCtrlBase.DeInit();
            m_Entity.mAniCtrlBase = null;
        }
        switch (entity.Type)
        {
            case EntityType.Hero:
                m_Entity.mAniCtrlBase = new AnimationCtrlHero();
                break;
            case EntityType.Soldier:
                m_Entity.mAniCtrlBase = new AnimationCtrlMonster();
                break;
            case EntityType.Boss:
                m_Entity.mAniCtrlBase = new AnimationCtrlBoss();
                break;
            default:
                m_Entity.mAniCtrlBase = new AnimationCtrlHero();
                break;
        }
        if (m_Entity.m_Body != null)
        {
            m_AnimationBase = m_Entity.m_Body.AnimatorBodyObj.GetComponent<Animation>();
        }
        m_Entity.mAniCtrlBase.OnStart();
        m_Entity.mAniCtrlBase.SetAnimatorBase(this);
        m_Entity.mAniCtrlBase.SetAnimation(m_AnimationBase);
        m_Entity.mAniCtrlBase.SetHittedCallBack(m_Entity.m_HitEdit.HittedAnimationCallBack);
        m_Entity.mAniCtrlBase.SetHeroPlayMakerColtrol(m_Entity.m_Body.mHeroPlayMakerCtrl);
        m_AnimationBase.enabled = true;
        SendEvent("Idle", force: true);
    }

    public void DeInit()
    {
        if (m_Entity.mAniCtrlBase != null)
        {
            m_Entity.mAniCtrlBase.DeInit();
        }
    }

    public void SendEvent(string eventName, bool force = false)
    {
        //Debug.LogFormat("@LOG AnimatorBase.SendEvent {0}, {1}", eventName, force);
        if (m_Entity.mAniCtrlBase != null)
        {
            //Debug.LogFormat("@LOG AnimatorBase.SendEvent 1 {0}, {1}", eventName, force);
            m_Entity.mAniCtrlBase.SendEvent(eventName, force);
        }
    }

    public void Reborn()
    {
        if (m_Entity.mAniCtrlBase != null)
        {
            m_Entity.mAniCtrlBase.Reborn();
        }
    }

    public float GetAnimationTime(string eventName)
    {
        if (m_Entity.mAniCtrlBase == null)
        {
            return 1f;
        }
        return m_Entity.mAniCtrlBase.GetAnimationTime(eventName);
    }

    public string GetAnimationValue(string eventName)
    {
        if (m_Entity.mAniCtrlBase == null)
        {
            return string.Empty;
        }
        return m_Entity.mAniCtrlBase.GetAnimationValue(eventName);
    }

    public void DeadDown()
    {
        m_Entity.m_Body.DeadDown();
    }

    public void ClearString(string name)
    {
        if (m_Entity.mAniCtrlBase != null)
        {
            m_Entity.mAniCtrlBase.SetAnimationClear(name);
        }
    }

    public void SetString(string name, string value = "")
    {
        if (m_Entity.mAniCtrlBase != null)
        {
            m_Entity.mAniCtrlBase.SetAnimationValue(name, value);
        }
    }

    public string GetString(string name)
    {
        if (m_Entity.mAniCtrlBase == null)
        {
            return name;
        }
        return m_Entity.mAniCtrlBase.GetAnimationValue(name);
    }

    public void SetBool(string name, bool value)
    {
        if (m_Entity.mAniCtrlBase != null)
        {
            m_Entity.mAniCtrlBase.SetBool(name, value);
        }
    }

    public void SetTouchMoveJoy(bool value)
    {
        SetBool("TouchMoveJoy", value);
    }
}
