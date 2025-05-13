




using Dxx.Util;
using UnityEngine;

public class HitEdit : MonoBehaviour
{
	public enum EHittedScaleAnimation
	{
		eNone,
		jelly_likebat,
		jelly_likeflower,
		jelly_likestone,
		jelly_likeghost,
		jelly_likestonesmall1,
		jelly_likestonesmall2
	}

	public enum EHittedPositionAnimation
	{
		eNone,
		pos_likestone
	}

	private EntityBase m_Entity;

	[Header("受击变白时间")]
	public float HittedWhiteTime = 0.3f;

	[Header("受击变白曲线")]
	public AnimationCurve HittedWhiteCurve;

	[Header("受击变白最大值")]
	public float HittedWhiteMax = 1f;

	private Animator m_AnimatorJelly;

	[Header("受击变形动画")]
	public EHittedScaleAnimation HittedScaleAnimation;

	private string mHittedScaleAnimation;

	private Animator m_AnimatorPosition;

	private EHittedPositionAnimation HittedPositionAnimation;

	private string mHittedPositionAnimation;

	private bool bPlayJelly;

	private int mPlayJellyFrame;

	private bool bPlayPosition;

	private int mPlayPositionFrame;

	public void Init(EntityBase entity)
	{
		m_Entity = entity;
		InitHittedWhiteCurve();
		InitHittedAnimation();
		Updater.AddUpdate(Utils.FormatString("{0}.HitEdit", m_Entity.m_EntityData.CharID), OnUpdate);
	}

	public void DeInit()
	{
		DeInitHittedAnimation();
		Updater.RemoveUpdate(Utils.FormatString("{0}.HitEdit", m_Entity.m_EntityData.CharID), OnUpdate);
	}

	private void OnEnable()
	{
		if ((bool)m_AnimatorJelly)
		{
			m_AnimatorJelly.enabled = true;
		}
		if ((bool)m_AnimatorPosition)
		{
			m_AnimatorPosition.enabled = true;
		}
	}

	private void OnDisable()
	{
		if ((bool)m_AnimatorJelly)
		{
			m_AnimatorJelly.enabled = false;
		}
		if ((bool)m_AnimatorPosition)
		{
			m_AnimatorPosition.enabled = false;
		}
	}

	private void InitHittedWhiteCurve()
	{
		int length = HittedWhiteCurve.length;
		SdkManager.Bugly_Report(length > 0, "HitEdit.cs", Utils.FormatString("EntityID:{0}, HitEdit.HittedWhiteCurve.length <= 0", m_Entity.m_EntityData.CharID));
		float time = HittedWhiteCurve[length - 1].time;
		float num = HittedWhiteTime / time;
		if (num > 1f)
		{
			for (int num2 = length - 1; num2 >= 0; num2--)
			{
				MoveKey(num2, num, HittedWhiteCurve[num2]);
			}
		}
		else if (num < 1f)
		{
			for (int i = 0; i < length; i++)
			{
				MoveKey(i, num, HittedWhiteCurve[i]);
			}
		}
	}

	private void MoveKey(int index, float scale, Keyframe keyframe)
	{
		HittedWhiteCurve.MoveKey(index, new Keyframe
		{
			time = keyframe.time * scale,
			value = keyframe.value,
			inTangent = keyframe.inTangent,
			outTangent = keyframe.outTangent,
			tangentMode = keyframe.tangentMode
		});
	}

	public float GetHittedWhiteByTime(float time)
	{
		if (time < 0f)
		{
			return 1f;
		}
		if (time > HittedWhiteTime)
		{
			return 0f;
		}
		return HittedWhiteCurve.Evaluate(time) * HittedWhiteMax;
	}

	public bool IsHittedWhiteEnd(float time)
	{
		return time >= HittedWhiteTime;
	}

	private void InitHittedAnimation()
	{
		if (!m_Entity.IsSelf)
		{
			m_AnimatorJelly = m_Entity.m_Body.gameObject.GetComponent<Animator>();
			if (m_AnimatorJelly == null)
			{
				m_AnimatorJelly = m_Entity.m_Body.gameObject.AddComponent<Animator>();
				m_AnimatorJelly.runtimeAnimatorController = ResourceManager.Load<RuntimeAnimatorController>("Game/Animator/Jelly");
			}
			mHittedScaleAnimation = HittedScaleAnimation.ToString();
			m_AnimatorPosition = m_Entity.m_Body.RotateMask.GetComponent<Animator>();
			if (m_AnimatorPosition == null)
			{
				m_AnimatorPosition = m_Entity.m_Body.RotateMask.AddComponent<Animator>();
				m_AnimatorPosition.runtimeAnimatorController = ResourceManager.Load<RuntimeAnimatorController>("Game/Animator/PosAni");
			}
			mHittedPositionAnimation = HittedPositionAnimation.ToString();
		}
	}

	public void HittedAnimationCallBack()
	{
		if (m_AnimatorJelly != null && HittedScaleAnimation != 0)
		{
			if (m_AnimatorJelly.GetCurrentAnimatorStateInfo(0).IsName(mHittedScaleAnimation))
			{
				bPlayJelly = true;
				mPlayJellyFrame = 0;
			}
			else
			{
				m_AnimatorJelly.Play(mHittedScaleAnimation);
			}
		}
	}

	public void HittedPosAni()
	{
		if (m_AnimatorPosition != null && HittedPositionAnimation != 0)
		{
			if (m_AnimatorPosition.GetCurrentAnimatorStateInfo(0).IsName(mHittedPositionAnimation))
			{
				bPlayPosition = true;
				mPlayPositionFrame = 0;
			}
			else
			{
				m_AnimatorPosition.Play(mHittedPositionAnimation);
			}
		}
	}

	private void OnUpdate(float delta)
	{
		if (bPlayJelly && m_AnimatorJelly != null)
		{
			if (mPlayJellyFrame == 0)
			{
				m_AnimatorJelly.Play("Normal");
			}
			else if (mPlayJellyFrame == 2)
			{
				m_AnimatorJelly.Play(mHittedScaleAnimation);
				bPlayJelly = false;
			}
			mPlayJellyFrame++;
		}
		if (bPlayPosition && m_AnimatorPosition != null)
		{
			if (mPlayPositionFrame == 0)
			{
				m_AnimatorPosition.Play("Normal");
			}
			else if (mPlayPositionFrame == 2)
			{
				m_AnimatorPosition.Play(mHittedPositionAnimation);
				bPlayPosition = false;
			}
			mPlayPositionFrame++;
		}
	}

	private void DeInitHittedAnimation()
	{
		if (m_AnimatorJelly != null)
		{
			UnityEngine.Object.Destroy(m_AnimatorJelly);
		}
		if (m_AnimatorPosition != null)
		{
			UnityEngine.Object.Destroy(m_AnimatorPosition);
		}
	}
}
