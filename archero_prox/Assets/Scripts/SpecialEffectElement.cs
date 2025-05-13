




using System;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
public class SpecialEffectElement : MonoBehaviour
{
	public enum ElementPlayStyle
	{
		Once,
		Loop,
		Unreset
	}

	[HideInInspector]
	[SerializeField]
	public float startTime;

	[HideInInspector]
	[SerializeField]
	public bool isLoop = true;

	[HideInInspector]
	[SerializeField]
	public ElementPlayStyle playStyle;

	[HideInInspector]
	[SerializeField]
	public float playTime;

	[NonSerialized]
	[HideInInspector]
	private bool canShow = true;

	protected float currPlayTime;

	[NonSerialized]
	[HideInInspector]
	private bool isPlaying;

	private float speedScale = 1f;

	public bool CanShow
	{
		get
		{
			return canShow;
		}
		set
		{
			canShow = value;
			if (!value)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			UpdateState(currPlayTime);
			_CustomOperate(currPlayTime);
		}
	}

	public float SpeedScale => speedScale;

	public bool Equals(object o)
	{
		if (o == null)
		{
			return false;
		}
		if (o == this)
		{
			return true;
		}
		if (GetType() != o.GetType())
		{
			return false;
		}
		SpecialEffectElement specialEffectElement = o as SpecialEffectElement;
		if (startTime != specialEffectElement.startTime)
		{
			return false;
		}
		if (playTime != specialEffectElement.playTime)
		{
			return false;
		}
		if (playStyle != specialEffectElement.playStyle)
		{
			return false;
		}
		return true;
	}

	public int GetHashCode()
	{
		return startTime.GetHashCode();
	}

	public bool _CopyValues(SpecialEffectElement o)
	{
		return false;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Awake()
	{
		_Init();
	}

	private void Start()
	{
	}

	public bool IsPlaying()
	{
		return isPlaying;
	}

	public bool IsEnable()
	{
		return base.gameObject.activeSelf;
	}

	public void SetEnable(bool b)
	{
		if (canShow)
		{
			base.gameObject.SetActive(b);
		}
	}

	public void Play()
	{
		_PlayImpl();
		isPlaying = true;
	}

	public void Pause()
	{
		_PauseImpl();
		isPlaying = false;
	}

	public void Stop()
	{
		_ResetImpl();
		Pause();
	}

	public void Reset()
	{
		_ResetImpl();
		if (isPlaying)
		{
			Play();
		}
		else
		{
			Pause();
		}
	}

	public void SetCurrPlayTime(float t)
	{
		currPlayTime = t;
		if (IsEnable())
		{
			float num = _CalcLocalTime(t);
			if (num > playTime && playStyle == ElementPlayStyle.Loop)
			{
				num = Mathf.Repeat(num, playTime);
			}
			_SetCurrPlayTime(num);
		}
	}

	public void UpdateState(float elapseTime)
	{
		if (IsInPlayTimeInterval(elapseTime))
		{
			if (!IsEnable() && canShow)
			{
				SetEnable(b: true);
				_OnEnableElement();
			}
		}
		else if (IsEnable())
		{
			Stop();
			_OnDisableElement();
			SetEnable(b: false);
		}
	}

	public void UpdatePlayingState(float elapseTime)
	{
		if (IsInPlayTimeInterval(elapseTime) && !IsPlaying())
		{
			Play();
			float num = speedScale;
			SetSpeedScale(1f);
			SetCurrPlayTime(elapseTime);
			Play();
			SetSpeedScale(num);
		}
	}

	protected bool IsInPlayTimeInterval(float elapseTime)
	{
		if (playTime <= 0f)
		{
			return false;
		}
		if (playStyle == ElementPlayStyle.Loop || playStyle == ElementPlayStyle.Unreset)
		{
			if (elapseTime >= startTime)
			{
				return true;
			}
			return false;
		}
		if (elapseTime - (startTime + playTime) < Mathf.Epsilon && elapseTime >= startTime)
		{
			return true;
		}
		return false;
	}

	protected float _CalcLocalTime(float elapseTime)
	{
		return elapseTime - startTime;
	}

	protected virtual void _Init()
	{
	}

	protected virtual void _PlayImpl()
	{
	}

	protected virtual void _PauseImpl()
	{
	}

	protected virtual void _ResetImpl()
	{
	}

	protected virtual void _OnEnableElement()
	{
	}

	protected virtual void _OnDisableElement()
	{
	}

	protected virtual void _SetCurrPlayTime(float t)
	{
	}

	protected virtual void _CustomOperate(float elapseTime)
	{
	}

	public virtual void SetSpeedScale(float scale)
	{
		speedScale = scale;
		if (IsPlaying())
		{
			UpdateSpeed();
		}
	}

	public virtual void UpdateSpeed()
	{
	}
}
