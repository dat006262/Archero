




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPChanger : MonoBehaviour
{
	private static Dictionary<HitType, float> mTimes = new Dictionary<HitType, float>
	{
		{
			HitType.Normal,
			0.6f
		},
		{
			HitType.Crit,
			0.83f
		},
		{
			HitType.HeadShot,
			1.33f
		},
		{
			HitType.Rebound,
			0.6f
		},
		{
			HitType.Add,
			0.6f
		},
		{
			HitType.Block,
			0.6f
		},
		{
			HitType.Miss,
			0.6f
		},
		{
			HitType.HPMaxChange,
			1.33f
		}
	};

	private Transform mTransform;

	private EntityBase m_Entity;

	private Vector3 entitypos;

	private Vector3 entitybodypos;

	private Text text;

	private float OffsetX;

	private float OffsetY;

	private Vector3 MovePer = default(Vector3);

	private Vector3 MoveAll;

	private const int MoveCount = 14;

	private int CurrentMoveCount;

	private int FontSize;

	private float CritFontScale = 1.5f;

	private float HeadShotFontScale = 1.5f;

	private const string Ani_Normal = "HPChanger_Normal";

	private const string Ani_Crit = "HPChanger_Crit";

	private const string Ani_HeadShot = "HPChanger_HeadShot";

	private float starttime;

	private CanvasGroup mCanvasGroup;

	private HitType mHitType;

	private AnimationCurve curve_pos;

	private AnimationCurve curve_scale;

	private AnimationCurve curve_alpha;

	private Vector3 screens;

	private float percent;

	private void Awake()
	{
		mTransform = base.transform;
		mCanvasGroup = GetComponent<CanvasGroup>();
		text = mTransform.Find("Text").GetComponent<Text>();
		FontSize = text.fontSize;
	}

	private void LateUpdate()
	{
		if (!this || !mTransform)
		{
			Despawn();
			return;
		}
		percent = (Updater.AliveTime - starttime) / mTimes[mHitType];
		percent = MathDxx.Clamp01(percent);
		if (percent >= 1f)
		{
			Despawn();
			return;
		}
		if ((bool)m_Entity)
		{
			entitypos = m_Entity.position;
		}
		if ((bool)m_Entity && (bool)m_Entity.m_Body)
		{
			entitybodypos = m_Entity.m_Body.HPMask.transform.localPosition;
		}
		if (CurrentMoveCount > 0)
		{
			CurrentMoveCount--;
			MoveAll += MovePer;
		}
		screens = Utils.World2Screen(entitypos);
		if (curve_pos != null)
		{
			screens.y += entitybodypos.y * 23f * GameLogic.HeightScale + curve_pos.Evaluate(percent);
		}
		mTransform.position = screens + MoveAll;
		if (curve_scale != null)
		{
			mTransform.localScale = Vector3.one * curve_scale.Evaluate(percent);
		}
		if (curve_alpha != null && (bool)mCanvasGroup)
		{
			mCanvasGroup.alpha = curve_alpha.Evaluate(percent);
		}
	}

	public void Despawn()
	{
		GameLogic.EffectCache(base.gameObject);
	}

	public void Init(EntityBase entity, HitStruct hs)
	{
		mHitType = hs.type;
		curve_pos = GameLogic.GetHPChangerAnimation(mHitType, 0);
		curve_scale = GameLogic.GetHPChangerAnimation(mHitType, 1);
		curve_alpha = GameLogic.GetHPChangerAnimation(mHitType, 2);
		starttime = Updater.AliveTime;
		OffsetX = GameLogic.Random(-50f, 50f);
		OffsetY = GameLogic.Random(0f, 30f);
		MovePer.x = OffsetX / 14f;
		MovePer.y = OffsetY / 14f;
		CurrentMoveCount = 14;
		MoveAll = Vector3.zero;
		m_Entity = entity;
		entitypos = entity.position;
		entitybodypos = entity.m_Body.HPMask.transform.localPosition;
		if (hs.element == EElementType.eNone)
		{
			switch (mHitType)
			{
			case HitType.Crit:
				text.text = Utils.FormatString("{0}!", hs.real_hit);
				text.color = Color.red;
				text.fontSize = (int)((float)FontSize * CritFontScale);
				break;
			case HitType.HeadShot:
				text.text = GameLogic.Hold.Language.GetLanguageByTID("爆头");
				text.color = Color.red;
				text.fontSize = (int)((float)FontSize * HeadShotFontScale);
				break;
			case HitType.Add:
				text.text = Utils.FormatString("+{0}", hs.real_hit);
				text.color = Color.green;
				break;
			case HitType.Block:
				text.text = hs.real_hit.ToString();
				text.color = Color.gray;
				break;
			case HitType.Miss:
				text.text = "Miss";
				text.color = Color.yellow;
				break;
			case HitType.HPMaxChange:
			{
				string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("生命上限");
				text.text = Utils.FormatString("{0}{1}{2}", languageByTID, MathDxx.GetSymbolString(hs.real_hit), MathDxx.Abs(hs.real_hit));
				text.color = ((hs.real_hit < 0) ? Color.red : Color.green);
				break;
			}
			default:
				text.text = hs.real_hit.ToString();
				text.color = Color.white;
				text.fontSize = FontSize;
				break;
			}
		}
		else
		{
			text.color = EntityData.ElementData[hs.element].color;
			text.text = hs.real_hit.ToString();
		}
	}
}
