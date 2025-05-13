




using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BodyMask : PauseObject
{
	public class BodyElementData
	{
		public Color color;

		public int count;
	}

	public GameObject LeftWeapon;

	public GameObject RightWeapon;

	public GameObject LeftBullet;

	public GameObject Body;

	public GameObject EffectMask;

	public GameObject HPMask;

	public GameObject FootMask;

	public GameObject HeadMask;

	public GameObject RotateMask;

	public GameObject BulletHitMask;

	public GameObject HeadTopEffect;

	public GameObject SpecialHitMask;

	public List<GameObject> BulletList;

	public List<SkinnedMeshRenderer> Body_Extra;

	[NonSerialized]
	public GameObject ZeroMask;

	public GameObject AnimatorBodyObj;

	[NonSerialized]
	public HeroPlayMakerControl mHeroPlayMakerCtrl;

	protected EntityBase m_Entity;

	private Dictionary<int, Transform> mWeaponPosList = new Dictionary<int, Transform>();

	private Animation ani;

	private bool bOffset;

	private bool bFlyStone;

	private float m_fAddScale;

	private BodyMaskCamera mCamera;

	private BodyShaderBase mShaderBase;

	public Transform BodyCenter;

	private Dictionary<EElementType, BodyElementData> ElementColor = new Dictionary<EElementType, BodyElementData>
	{
		{
			EElementType.eFire,
			new BodyElementData
			{
				color = new Color(1f, 58f / 255f, 0f)
			}
		},
		{
			EElementType.eIce,
			new BodyElementData
			{
				color = new Color(0f, 181f / 255f, 1f)
			}
		}
	};

	private bool bVisible;

	protected bool bHittedColor;

	protected float mHittedTime;

	private bool bTargetColor;

	private Dictionary<int, GameObject> mHeadTopList = new Dictionary<int, GameObject>();

	private List<int> mHeadIDs = new List<int>();

	public Transform DeadNode
	{
		get
		{
			if (BodyCenter == null)
			{
				return base.transform;
			}
			return BodyCenter;
		}
	}

	private void Awake()
	{
		SdkManager.Bugly_Report(BulletHitMask != null, "BodyMask.cs", " BulletHitMask == null");
		SdkManager.Bugly_Report(Body != null, "BodyMask.cs", " Body == null");
		SdkManager.Bugly_Report(EffectMask != null, "BodyMask.cs", " EffectMask == null");
		SdkManager.Bugly_Report(HPMask != null, "BodyMask.cs", " HPMask == null");
		SdkManager.Bugly_Report(HeadMask != null, "BodyMask.cs", " HeadMask == null");
		ani = AnimatorBodyObj.GetComponent<Animation>();
		ZeroMask = new GameObject("Zero");
		ZeroMask.SetParentNormal(base.transform);
	}

	public void SetLocalPosition(float x, float z)
	{
		base.transform.localPosition = new Vector3(x, 0f, z);
	}

	private void OnEnable()
	{
		ani.enabled = true;
	}

	private void OnDisable()
	{
		ani.enabled = false;
	}

	protected virtual void AwakeInit()
	{
	}

	public void SetEntity(EntityBase entity)
	{
		m_Entity = entity;
		if (!m_Entity.IsSelf)
		{
			GameMode mode = GameLogic.Hold.BattleData.GetMode();
			if (mode == GameMode.eGold1)
			{
				mShaderBase = new BodyShaderGold();
			}
			else
			{
				mShaderBase = new BodyShaderDefault();
			}
		}
		else
		{
			mShaderBase = new BodyShaderDefault();
		}
		mShaderBase.Init(m_Entity);
		mCamera = new BodyMaskCamera(entity);
		mHeroPlayMakerCtrl = new HeroPlayMakerControl();
		mHeroPlayMakerCtrl.Init(entity);
		SetTexture(m_Entity.m_Data.TextureID);
		mShaderBase.ReturnToDefault();
		AddScale(0f);
		if (m_Entity.IsElite)
		{
			AddScale(0.2f);
			OnElite();
		}
	}

	public void SetTexture(string value)
	{
		if (mShaderBase != null)
		{
			mShaderBase.SetTexture(value);
		}
	}

	public void SetTextureWithoutInit(string value)
	{
		string path = Utils.FormatString("Game/ModelsTexture/{0}", value);
		Texture texture = ResourceManager.Load<Texture>(path);
		if (texture == null)
		{
			return;
		}
		SkinnedMeshRenderer component = Body.GetComponent<SkinnedMeshRenderer>();
		if (component != null)
		{
			component.material.SetTexture("_MainTex", texture);
			return;
		}
		MeshRenderer component2 = Body.GetComponent<MeshRenderer>();
		if (component2 != null)
		{
			component2.material.SetTexture("_MainTex", texture);
		}
	}

	public void SetStrengh()
	{
		if (mShaderBase != null)
		{
			mShaderBase.SetStrengh();
		}
	}

	public void AddScale(float scale)
	{
		m_fAddScale += scale;
		if ((bool)EffectMask && (bool)RotateMask && (bool)m_Entity && m_Entity.m_Data != null)
		{
			EffectMask.transform.parent.localScale = Vector3.one * (m_Entity.m_Data.BodyScale + m_fAddScale);
			RotateMask.transform.localScale = EffectMask.transform.parent.localScale;
			ZeroMask.transform.localScale = EffectMask.transform.parent.localScale;
		}
	}

	private void OnElite()
	{
	}

	public Transform GetBullet(int index)
	{
		if (BulletList.Count > index && index >= 0)
		{
			return BulletList[index].transform;
		}
		if ((bool)LeftBullet)
		{
			return LeftBullet.transform;
		}
		return EffectMask.transform;
	}

	public void SetIsVislble(bool value)
	{
		bVisible = value;
	}

	public bool GetIsInCamera()
	{
		return bVisible;
	}

	public virtual void Hitted(Vector3 HittedDirection, HitType type)
	{
		bHittedColor = true;
		mHittedTime = Updater.AliveTime;
		mShaderBase.SetHitted();
		OnHittedColorBefore();
	}

	protected virtual void OnHittedColorBefore()
	{
	}

	protected override void UpdateProcess()
	{
		OnHittedColor();
	}

	protected virtual void OnHittedColor()
	{
		if (bHittedColor)
		{
			float hittedWhiteByTime = m_Entity.m_HitEdit.GetHittedWhiteByTime(Updater.AliveTime - mHittedTime);
			mShaderBase.OnUpdateHitted(hittedWhiteByTime);
			if (m_Entity.m_HitEdit.IsHittedWhiteEnd(Updater.AliveTime - mHittedTime))
			{
				bHittedColor = false;
				UpdateElement();
			}
		}
	}

	public void AddElement(EElementType type)
	{
		ElementColor[type].count++;
		if (!bHittedColor)
		{
			UpdateElement();
		}
	}

	public void RemoveElement(EElementType type)
	{
		BodyElementData bodyElementData = ElementColor[type];
		bodyElementData.count--;
		if (bodyElementData.count == 0 && !bHittedColor)
		{
			mShaderBase.ReturnToDefault();
		}
	}

	private void UpdateElement()
	{
		Dictionary<EElementType, BodyElementData>.Enumerator enumerator = ElementColor.GetEnumerator();
		bool flag = false;
		while (enumerator.MoveNext())
		{
			BodyElementData value = enumerator.Current.Value;
			if (value.count > 0)
			{
				mShaderBase.SetElement(value.color);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			mShaderBase.ReturnToDefault();
		}
	}

	public void DeadDown()
	{
		bHittedColor = false;
		Dictionary<EElementType, BodyElementData>.Enumerator enumerator = ElementColor.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.count = 0;
		}
		Vector3 localPosition = RotateMask.transform.localPosition;
		if (localPosition.y > 0f)
		{
			RotateMask.transform.DOKill();
			RotateMask.transform.DOLocalMoveY(0f, 0.3f);
		}
	}

	public void SetFlyStone(bool fly)
	{
		bFlyStone = fly;
	}

	public void DeInit()
	{
		if (mCamera != null)
		{
			mCamera.DeInit();
		}
		mShaderBase.ReturnToDefault();
		if ((bool)this)
		{
			base.transform.localScale = Vector3.one;
		}
	}

	public void CacheEffect()
	{
		CacheNode(EffectMask);
	}

	private void CacheNode(GameObject node)
	{
		if ((bool)node)
		{
			CacheNode(node.transform);
		}
	}

	private void CacheNode(Transform node)
	{
		if ((bool)node)
		{
			int childCount = node.childCount;
			for (int num = childCount - 1; num >= 0; num--)
			{
				Transform child = node.GetChild(num);
				GameLogic.EffectCache(child.gameObject);
			}
		}
	}

	public void SetTarget(bool value)
	{
	}

	public void SetBodyScale(float value)
	{
		if ((bool)Body)
		{
			HPMask.transform.parent.localScale = Vector3.one * value;
			base.transform.localScale = Vector3.one * value;
		}
	}

	public float GetBodyScale()
	{
		Vector3 localScale = base.transform.localScale;
		return localScale.x;
	}

	public void SetOrder()
	{
		if ((bool)this && (bool)base.transform && (bool)m_Entity)
		{
			Vector3 position = base.transform.position;
			int order = (int)(0f - position.z - 1f - 0.666f);
			if (bFlyStone)
			{
				order = 999;
			}
			mShaderBase.SetOrder(order);
			if (m_Entity.m_Weapon != null)
			{
				m_Entity.m_Weapon.SetOrder(order);
			}
		}
	}

	public Transform GetWeaponNode(int index, Transform t)
	{
		if (!mWeaponPosList.TryGetValue(index, out Transform value))
		{
			GameObject gameObject = new GameObject(Utils.FormatString("WeaponNode_{0}", index.ToString()));
			value = gameObject.transform;
			value.SetParentNormal(m_Entity.m_Body.EffectMask.transform.parent);
			value.position = t.position;
			mWeaponPosList.Add(index, value);
		}
		return value;
	}

	public void AddHeadTop(int skillaloneid, GameObject o)
	{
		if (!mHeadTopList.ContainsKey(skillaloneid))
		{
			mHeadTopList.Add(skillaloneid, o);
			mHeadIDs.Add(skillaloneid);
			o.transform.SetParent(HeadTopEffect.transform);
			o.transform.localRotation = Quaternion.identity;
			o.transform.localScale = Vector3.one;
			UpdateHeadTop();
		}
	}

	public void RemoveHeadTop(int skillaloneid)
	{
		if (mHeadTopList.ContainsKey(skillaloneid))
		{
			mHeadTopList.Remove(skillaloneid);
			mHeadIDs.Remove(skillaloneid);
			UpdateHeadTop();
		}
	}

	public Vector3 GetHeadPosition(int skillaloneid)
	{
		if (mHeadTopList.TryGetValue(skillaloneid, out GameObject value))
		{
			return value.transform.position;
		}
		SdkManager.Bugly_Report("BodyMask_HeadTop.cs", Utils.FormatString("GetHeadPosition[{0}] dont have!", skillaloneid));
		return base.transform.position;
	}

	private void UpdateHeadTop()
	{
		float num = 120f;
		float num2 = (float)(mHeadIDs.Count - 1) * num;
		float num3 = num2 / 2f;
		int i = 0;
		for (int count = mHeadIDs.Count; i < count; i++)
		{
			GameObject gameObject = mHeadTopList[mHeadIDs[i]];
			float angle = (float)i * num - num3;
			float x = MathDxx.Sin(angle);
			float z = MathDxx.Cos(angle);
			gameObject.transform.localPosition = new Vector3(x, 0f, z) * 1.5f;
		}
	}
}
