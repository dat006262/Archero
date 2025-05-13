




using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;

public class SkillCreateBase : MonoBehaviour
{
	protected EntityBase m_Entity;

	protected Action<SkillCreateBase> mCallback;

	protected float time;

	private Sequence seq;

	private void Awake()
	{
		OnAwake();
	}

	protected virtual void OnAwake()
	{
	}

	public void Init(EntityBase entity, string[] args)
	{
		m_Entity = entity;
		OnInit(args);
		if (time == 0f)
		{
			SdkManager.Bugly_Report("SkillCreateBase.cs", Utils.FormatString("Init {0} time is 0", GetType().ToString()));
		}
		seq = DOTween.Sequence().AppendInterval(time).AppendCallback(delegate
		{
			if (mCallback != null)
			{
				mCallback(this);
			}
		});
	}

	protected virtual void OnInit(string[] args)
	{
	}

	public void SetTimeCallback(Action<SkillCreateBase> callback)
	{
		mCallback = callback;
	}

	public void Deinit()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
		OnDeinit();
		GameLogic.EffectCache(base.gameObject);
	}

	protected virtual void OnDeinit()
	{
	}
}
