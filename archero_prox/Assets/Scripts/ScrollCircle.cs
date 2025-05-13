




using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollCircle : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	public delegate void JoyTouchStart(JoyData data);

	public delegate void JoyTouching(JoyData data);

	public delegate void JoyTouchEnd(JoyData data);

	public static Action OnDoubleClick;

	[SerializeField]
	private JoyNameType JoyType;

	private Dictionary<JoyNameType, string> JoyDic = new Dictionary<JoyNameType, string>
	{
		{
			JoyNameType.MoveJoy,
			"MoveJoy"
		},
		{
			JoyNameType.AttackJoy,
			"AttackJoy"
		}
	};

	protected Vector2 Origin;

	protected float mRadius;

	protected float mRadiusSmall;

	protected Transform child;

	protected Transform bgParent;

	protected Transform bgParengbgbg;

	protected Transform touch;

	protected Transform direction;

	private Vector3 StartPos;

	private bool bShowDirection = true;

	protected JoyData m_Data = default(JoyData);

	private bool disable;

	private bool touchdown;

	private Vector3 touchdownpos;

	private static bool TouchIn = true;

	private int mTouchID = -1;

	private float fClickTime;

	private float ClickDelayTime = 0.2f;

	private Animator mAni_ScreenTouch;

	private bool bDrag;

	private Vector3 pos_v;

	private float pos_w;

	private Vector3 pos_2 = new Vector3(0.5f, 0.5f, 0f);

	private Vector2 DealDrag_touchpos;

	private Vector2 DealDrag_touchpos1;

	public static event JoyTouchStart On_JoyTouchStart;

	public static event JoyTouching On_JoyTouching;

	public static event JoyTouchEnd On_JoyTouchEnd;

	private void OnEnable()
	{
		disable = false;
		touchdown = false;
		(base.transform.parent as RectTransform).SetAsFirstSibling();
	}

	private void OnDisable()
	{
		OnPointerUp(null);
		disable = true;
		touchdown = false;
	}

	private void Awake()
	{
		ClickDelayTime = SettingDebugMediator.DoubleClick;
		child = base.transform.Find("panel/bg");
		bgParent = child.transform.Find("bgParent");
		bgParengbgbg = bgParent.Find("bg/bg");
		touch = child.transform.Find("touch");
		direction = bgParent.transform.Find("direction");
		StartPos = child.localPosition;
		touch.localScale = Vector3.one * SettingDebugMediator.JoyScaleTouch / 100f;
		Vector2 sizeDelta = (child as RectTransform).sizeDelta;
		mRadius = sizeDelta.x * 0.5f * (float)SettingDebugMediator.JoyRadius / 100f;
		mRadiusSmall = mRadius;
		if (TouchIn)
		{
			float num = mRadius;
			Vector2 sizeDelta2 = (touch as RectTransform).sizeDelta;
			float num2 = sizeDelta2.x * 0.5f * (float)SettingDebugMediator.JoyRadius / 100f;
			Vector3 localScale = touch.localScale;
			mRadiusSmall = num - num2 * localScale.x;
		}
		mRadius *= (float)SettingDebugMediator.JoyScaleBG / 100f;
		mRadiusSmall *= (float)SettingDebugMediator.JoyScaleBG / 100f;
		m_Data.name = JoyDic[JoyType];
		if (m_Data.name == "MoveJoy")
		{
			m_Data.action = "Run";
		}
		bgParent.Find("bg/bg").localScale = Vector3.one * SettingDebugMediator.JoyScaleBG / 100f;
		direction.gameObject.SetActive(bShowDirection);
		SettingDebugMediator.OnValueChange = OnValueChange;
		m_Data.direction = default(Vector3);
	}

	private void OnValueChange()
	{
		Vector2 sizeDelta = (child as RectTransform).sizeDelta;
		mRadius = sizeDelta.x * 0.5f * (float)SettingDebugMediator.JoyRadius / 100f;
		mRadiusSmall = mRadius;
		if (TouchIn)
		{
			float num = mRadius;
			Vector2 sizeDelta2 = (touch as RectTransform).sizeDelta;
			float num2 = sizeDelta2.x * 0.5f * (float)SettingDebugMediator.JoyRadius / 100f;
			Vector3 localScale = touch.localScale;
			mRadiusSmall = num - num2 * localScale.x;
		}
		mRadius *= (float)SettingDebugMediator.JoyScaleBG / 100f;
		mRadiusSmall *= (float)SettingDebugMediator.JoyScaleBG / 100f;
		bgParengbgbg.localScale = Vector3.one * SettingDebugMediator.JoyScaleBG / 100f;
	}

	private Vector3 GetPos(Vector3 pos)
	{
		pos_v = GameNode.m_Camera.ScreenToViewportPoint(pos) - pos_2;
		pos_w = (float)GameLogic.DesignHeight / (float)Screen.height * (float)Screen.width;
		return new Vector3(pos_w * pos_v.x, (float)GameLogic.DesignHeight * pos_v.y, 0f);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		mTouchID = eventData.pointerId;
		OnPointerDown(GetPos(eventData.position));
	}

	private void OnPointerDown(Vector3 pos)
	{
		if (disable)
		{
			return;
		}
		bDrag = false;
		if (OnDoubleClick != null)
		{
			GameObject gameObject = GameLogic.EffectGet("Game/UI/ScreenTouch");
			gameObject.transform.SetParent(GameNode.m_Joy);
			gameObject.transform.position = pos;
			if (Updater.AliveTime - fClickTime < ClickDelayTime)
			{
				fClickTime = -1f;
				if ((bool)GameLogic.Self)
				{
					OnDoubleClick();
				}
			}
			else
			{
				fClickTime = Updater.AliveTime;
			}
		}
		else
		{
			touchdownpos = pos;
			touchdown = true;
			child.localPosition = pos;
			child.gameObject.SetActive(value: true);
			Origin = pos;
			DealDrag(Origin);
			if (ScrollCircle.On_JoyTouchStart != null)
			{
				ScrollCircle.On_JoyTouchStart(m_Data);
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (disable || mTouchID != eventData.pointerId)
		{
			return;
		}
		if (!touchdown)
		{
			child.gameObject.SetActive(value: true);
			Vector3 vector = touchdownpos = GetPos(eventData.position);
			touchdown = true;
			child.localPosition = vector;
			Origin = vector;
			ProfilerHelper.BeginSample("Circle drag 1");
			DealDrag(Origin);
			if (ScrollCircle.On_JoyTouchStart != null)
			{
				ScrollCircle.On_JoyTouchStart(m_Data);
			}
			ProfilerHelper.EndSample();
		}
		bDrag = true;
		ProfilerHelper.BeginSample("Circle drag 2");
		DealDrag(GetPos(eventData.position));
		if (ScrollCircle.On_JoyTouching != null)
		{
			ScrollCircle.On_JoyTouching(m_Data);
		}
		ProfilerHelper.EndSample();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData != null && !disable && mTouchID == eventData.pointerId)
		{
			touchdown = false;
			if (bDrag)
			{
				fClickTime = -1f;
			}
			if (ScrollCircle.On_JoyTouchEnd != null)
			{
				touch.localPosition = Vector2.zero;
				ScrollCircle.On_JoyTouchEnd(m_Data);
			}
			if ((bool)child)
			{
				child.localPosition = StartPos;
				direction.localRotation = Quaternion.identity;
			}
		}
	}

	private void DealDrag(Vector2 pos, bool updateui = true)
	{
		DealDrag_touchpos = pos - Origin;
		if (DealDrag_touchpos.magnitude > mRadius)
		{
			DealDrag_touchpos = DealDrag_touchpos.normalized * mRadius;
		}
		DealDrag_touchpos1 = DealDrag_touchpos;
		if (DealDrag_touchpos1.magnitude > mRadiusSmall)
		{
			DealDrag_touchpos1 = DealDrag_touchpos1.normalized * mRadiusSmall;
		}
		m_Data.length = DealDrag_touchpos.magnitude;
		ref Vector3 reference = ref m_Data.direction;
		Vector2 normalized = DealDrag_touchpos.normalized;
		reference.x = normalized.x;
		ref Vector3 reference2 = ref m_Data.direction;
		Vector2 normalized2 = DealDrag_touchpos.normalized;
		reference2.z = normalized2.y * 1.23f;
		m_Data.angle = Utils.getAngle(m_Data.direction);
		if (updateui)
		{
			touch.localPosition = DealDrag_touchpos1;
			direction.localRotation = Quaternion.Euler(0f, 0f, 0f - m_Data.angle);
		}
	}
}
