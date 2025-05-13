




using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
	public class PartBodyData
	{
		public int ID;

		public int alivecount;

		public int maxcount;

		public void Add()
		{
			alivecount++;
		}

		public void Remove()
		{
			alivecount--;
		}

		public bool CanAdd()
		{
			return alivecount < maxcount;
		}
	}

	public class RotateFollowData
	{
		private int name;

		public EntityBase parent;

		public float rotate;

		private float currentrotate;

		private float range;

		private List<EntityBase> mList = new List<EntityBase>();

		private GameObject test;

		public RotateFollowData(int name)
		{
			this.name = name;
		}

		public void Init(EntityBase parent, float rotate, float range)
		{
			this.parent = parent;
			this.rotate = rotate;
			this.range = range;
			currentrotate = 0f;
			Updater.AddUpdate(Utils.FormatString("{0}.RotateFollowData", parent.m_EntityData.CharID), OnUpdate);
		}

		public void DeInit()
		{
			Updater.RemoveUpdate(Utils.FormatString("{0}.RotateFollowData", parent.m_EntityData.CharID), OnUpdate);
		}

		public void Add(EntityBase entity)
		{
			if (!mList.Contains(entity))
			{
				mList.Add(entity);
				UpdateEntities();
			}
		}

		public void Remove(EntityBase entity)
		{
			if (mList.Contains(entity))
			{
				mList.Remove(entity);
				UpdateEntities();
			}
		}

		private void UpdateEntities()
		{
			int i = 0;
			for (int count = mList.Count; i < count; i++)
			{
				EntityBase entityBase = mList[i];
				entityBase.SetRotateFollowIndex(name, i);
			}
		}

		public Vector3 GetPosition(EntityBase entity)
		{
			float num = 360f / (float)mList.Count % 360f * (float)entity.GetRotateFollowIndex(name);
			num += currentrotate;
			float x = MathDxx.Sin(num) * range;
			float z = MathDxx.Cos(num) * range;
			return parent.position + new Vector3(x, 0f, z);
		}

		private void OnUpdate(float delta)
		{
			currentrotate += rotate;
			currentrotate %= 360f;
		}
	}

	protected class RotateClass
	{
		public EntityBase parent;

		public string name;

		public float rotate;

		public float allangle;

		protected float time;

		private Transform RotateAttribute;

		protected List<Transform> mRotateAttrList = new List<Transform>();

		public void Init(EntityBase parent, string name, float rotate, float allangle)
		{
			this.parent = parent;
			this.name = name;
			this.rotate = rotate;
			this.allangle = allangle;
			time = 0f;
			RotateAttribute = new GameObject(name).transform;
			RotateAttribute.SetParent(parent.transform);
			RotateAttribute.localPosition = Vector3.zero;
			RotateAttribute.localRotation = Quaternion.identity;
			Updater.AddUpdate(Utils.FormatString("{0}.RotateClass", parent.m_EntityData.CharID), OnRotateAttributeUpdate);
		}

		public void DeInit()
		{
			Updater.RemoveUpdate(Utils.FormatString("{0}.RotateClass", parent.m_EntityData.CharID), OnRotateAttributeUpdate);
		}

		public void AddNewRotateAttribute(GameObject o)
		{
			mRotateAttrList.Add(o.transform);
			o.transform.SetParent(RotateAttribute);
			o.transform.localScale = Vector3.one;
			o.transform.localPosition = Vector3.zero;
			RotateAttributeUpdatePosition();
		}

		public void Remove(GameObject o)
		{
			if ((bool)o)
			{
				mRotateAttrList.Remove(o.transform);
				GameLogic.EffectCache(o);
				RotateAttributeUpdatePosition();
			}
		}

		private void RotateAttributeUpdatePosition()
		{
			int count = mRotateAttrList.Count;
			if (count > 0)
			{
				float num = allangle / (float)count;
				for (int i = 0; i < count; i++)
				{
					mRotateAttrList[i].localRotation = Quaternion.Euler(0f, num * (float)i, 0f);
				}
			}
			OnAddorMove();
		}

		protected virtual void OnAddorMove()
		{
		}

		private void OnRotateAttributeUpdate(float delta)
		{
			if (RotateAttribute != null)
			{
				Transform rotateAttribute = RotateAttribute;
				Vector3 localEulerAngles = RotateAttribute.localEulerAngles;
				rotateAttribute.localRotation = Quaternion.Euler(0f, localEulerAngles.y + rotate, 0f);
			}
		}
	}

	protected class RotateBallClass : RotateClass
	{
		public float rangemin;

		public float rangemax;

		private Dictionary<Transform, List<Transform>> mList = new Dictionary<Transform, List<Transform>>();

		private float angle;

		private float radius;

		public void SetRadius(float radius)
		{
			this.radius = radius;
		}

		protected override void OnAddorMove()
		{
			for (int i = 0; i < mRotateAttrList.Count; i++)
			{
				Transform t = mRotateAttrList[i];
				Transform one = GetOne(t, 0);
				Transform transform = one;
				float x = radius;
				Vector3 localPosition = one.localPosition;
				float y = localPosition.y;
				Vector3 localPosition2 = one.localPosition;
				transform.localPosition = new Vector3(x, y, localPosition2.z);
				Transform one2 = GetOne(t, 1);
				Transform transform2 = one2;
				float x2 = 0f - radius;
				Vector3 localPosition3 = one2.localPosition;
				float y2 = localPosition3.y;
				Vector3 localPosition4 = one2.localPosition;
				transform2.localPosition = new Vector3(x2, y2, localPosition4.z);
			}
		}

		private Transform GetOne(Transform t, int index)
		{
			if (!mList.TryGetValue(t, out List<Transform> value))
			{
				value = new List<Transform>();
				mList.Add(t, value);
			}
			if (value.Count > index)
			{
				return value[index];
			}
			Transform transform = t.Find(index.ToString());
			value.Add(transform);
			return transform;
		}
	}

	public Action Event_DeInit;

	public Action Event_OnAttack;

	[NonSerialized]
	public string ClassName;

	[NonSerialized]
	public int ClassID;

	protected string HPSliderName = "HPSlider";

	[NonSerialized]
	public Character_Char m_Data;

	public EntityData m_EntityData;

	public float HPOffsetY = 100f;

	protected GameObject child;

	//[NonSerialized]
	public AnimatorBase m_AniCtrl;

	//[NonSerialized]
	public MoveControl m_MoveCtrl;

	//[NonSerialized]
	public AttackControl m_AttackCtrl;

	public WeaponBase m_Weapon;

	public AnimationCtrlBase mAniCtrlBase;

	public BodyMask m_Body;

	public HitEdit m_HitEdit;

	protected SphereCollider m_SphereCollider;

	protected CapsuleCollider m_CapsuleCollider;

	protected BoxCollider m_BoxCollider;

	protected Dictionary<string, BoxCollider> m_ChildsBoxCollider = new Dictionary<string, BoxCollider>();

	protected Dictionary<string, SphereCollider> m_ChildsSphereCollider = new Dictionary<string, SphereCollider>();

	protected Dictionary<string, CapsuleCollider> m_ChildsCapsuleCollider = new Dictionary<string, CapsuleCollider>();

	protected const string Entity2MapOutWall = "Entity2MapOutWall";

	protected const string Entity2Stone = "Entity2Stone";

	protected const string Entity2Water = "Entity2Water";

	public HpSlider m_HPSlider;

	public float AliveTime;

	[NonSerialized]
	private bool m_bElite;

	protected float HittedX;

	protected float HittedY;

	public Vector3 HittedDirection = Vector3.zero;

	public float HittedAngle;

	public float HittedV;

	public Vector3 hittedoffset;

	[SerializeField]
	protected EntityState m_State;

	[SerializeField]
	[Tooltip("当前生命值")]
	private string HPPercent;

	private int showhpcount = 1;

	private int showmeshcount = 1;

	private Transform[] childs;

	private bool showchallenge = true;

	private EntityBase m_HatredTargetP;

	private bool bInit;

	private bool Dead_bPlay;

	private int Dead_PlayCount = 20;

	private int Dead_CurrentCount;

	private float Dead_StartAngle;

	private float Dead_PerAngle;

	private bool bFlyWater;

	private bool bFlyStone;

	public Action<bool> OnPlayHittedAction;

	public Action<EntityBase, Vector3> OnKillAction;

	public Action<EntityBase, Vector3> OnHitAction;

	public Action OnSkillActionEnd;

	public Action OnWillDead;

	public Action<long, long, float, long> OnChangeHPAction;

	public Action<long, long> OnMaxHpUpdate;

	public Action<EntityBase> OnMonsterDeadAction;

	public Action<int> OnLevelUp;

	public Action<long> Shield_CountAction;

	public Action<long> Shield_ValueAction;

	public Action<bool> OnMoveEvent;

	public Action OnMissAngel;

	public Action OnMissDemon;

	public Action OnMissShop;

	public Action OnInBossRoom;

	public Action<EntityBase, long> OnHitted;

	public Action<long> OnCrit;

	public Action OnFullHP;

	public Action<bool> OnDizzy;

	public Action OnMiss;

	public Action<EntityBase> OnLight45;

	private List<int> mBabySkillIds = new List<int>();

	private List<long> mBabyArgs = new List<long>();

	private List<Vector3> mBabyGroundPos = new List<Vector3>
	{
		new Vector3(-2f, 0f, 0f),
		new Vector3(-1.5f, 0f, -1.5f),
		new Vector3(0f, 0f, -1.5f),
		new Vector3(1.5f, 0f, -1.5f),
		new Vector3(2f, 0f, 0f)
	};

	private int mBabyGroundIndex;

	private List<int> DebuffList = new List<int>();

	private int mCallID;

	private Vector3 mCallEndPos;

	private EntityHitCtrl mHitCtrl;

	public int collidercount = 1;

	private List<EntityCtrlBase> ctrlsList = new List<EntityCtrlBase>();

	public float HittedLastTime;

	private bool bCanHit = true;

	private GameObject CantHitTarget;

	private GameObject HitTarget;

	protected Sequence mDeadSeq;

	public bool bCall;

	protected ActionBasic mAction = new ActionBasic();

	private int divide_frame;

	private int divide_maxframe = 5;

	private Vector3 dividemove;

	private Vector3 _position;

	private Vector3 _eulerAngles;

	private Rigidbody _rigid;

	private Vector3 SetPositionBy_P;

	private int move_layermask;

	private float move_offset;

	private RaycastHit[] move_hits;

	private RaycastHit move_hit;

	private float move_dis;

	private Vector3 move_vec = new Vector3(0f, 1f, 0f);

	private int mSuperArmor;

	private Dictionary<int, PartBodyData> mPartBodyList = new Dictionary<int, PartBodyData>
	{
		{
			1801,
			new PartBodyData
			{
				alivecount = 0,
				maxcount = 10
			}
		},
		{
			1802,
			new PartBodyData
			{
				alivecount = 0,
				maxcount = 8
			}
		},
		{
			1803,
			new PartBodyData
			{
				alivecount = 0,
				maxcount = 8
			}
		},
		{
			1804,
			new PartBodyData
			{
				alivecount = 0,
				maxcount = 8
			}
		},
		{
			1805,
			new PartBodyData
			{
				alivecount = 0,
				maxcount = 8
			}
		},
		{
			1806,
			new PartBodyData
			{
				alivecount = 0,
				maxcount = 8
			}
		},
		{
			1807,
			new PartBodyData
			{
				alivecount = 0,
				maxcount = 8
			}
		}
	};

	private int PartBody_AliveCount;

	private int PartBody_MaxCount = 10;

	private Dictionary<int, RotateFollowData> mRotateFollowList = new Dictionary<int, RotateFollowData>();

	private Dictionary<int, int> mRotateIndexList = new Dictionary<int, int>();

	private RotateBallClass mRotateAttribute;

	private RotateBallClass mRotateSword;

	private RotateClass mRotateShield;

	private Dictionary<int, SkillBase> skillsList = new Dictionary<int, SkillBase>();

	private Dictionary<int, SkillBase> skillsAutoList = new Dictionary<int, SkillBase>();

	private List<SkillBase> skillsAttributeList = new List<SkillBase>();

	private List<int> skillidList = new List<int>();

	private List<SkillBase> skillsOverlyingList = new List<SkillBase>();

	[SerializeField]
	private EntityType m_Type;

	private string _namep = string.Empty;

	public bool bDivide;

	private string mDivideID = string.Empty;

	public GameObject m_WeaponHand;

	protected virtual string ModelPath => "Game/Player/player";

	public GameObject Child => child;

	public bool IsElite => m_bElite;

	public EntityState State => m_State;

	public EntityBase m_HatredTarget
	{
		get
		{
			if ((bool)m_HatredTargetP && ((!m_HatredTargetP.GetIsDead() && !m_HatredTargetP.IsSelf) || m_HatredTargetP.IsSelf))
			{
				return m_HatredTargetP;
			}
			m_HatredTargetP = GameLogic.FindTarget(this);
			return m_HatredTargetP;
		}
		set
		{
			if (IsSelf || ((bool)value && !value.GetIsDead()))
			{
				m_HatredTargetP = value;
			}
		}
	}

	public int CallID => mCallID;

	public Vector3 CallEndPos => mCallEndPos;

	public bool IsSelf => (bool)GameLogic.Release && (bool)GameLogic.Release.Entity && this == GameLogic.Self;

	public Vector3 position
	{
		get
		{
			if ((bool)this && (bool)base.transform)
			{
				return base.transform.position;
			}
			return _position;
		}
	}

	public Vector3 eulerAngles => _eulerAngles;

	private Rigidbody rigid
	{
		get
		{
			if (_rigid == null)
			{
				_rigid = GetComponent<Rigidbody>();
			}
			return _rigid;
		}
	}

	private string _name
	{
		get
		{
			if (_namep == string.Empty)
			{
				_namep = base.gameObject.name;
			}
			return _namep;
		}
	}

	public string DivideID
	{
		get
		{
			return mDivideID;
		}
		set
		{
			if (mDivideID == string.Empty)
			{
				mDivideID = value;
				GameLogic.Release.Entity.AddDivide(mDivideID, new EntityManager.DivideTransfer
				{
					divedeid = mDivideID,
					charid = m_Data.CharID,
					entitytype = Type
				});
			}
		}
	}

	public EntityType Type
	{
		get
		{
			if (m_Type == EntityType.Invalid)
			{
				SdkManager.Bugly_Report("EntityBase.cs", Utils.GetString("EntityType Invalid ", base.name));
			}
			return m_Type;
		}
	}

	public event Action<Vector3> Event_PositionBy;

	public void SetElite(bool value)
	{
		m_bElite = value;
	}

	public void ShowHP(bool show)
	{
		showhpcount += (show ? 1 : (-1));
		if ((bool)m_HPSlider)
		{
			m_HPSlider.ShowHP(showhpcount > 0);
		}
	}

	public void ShowMesh(bool show)
	{
		if ((bool)m_Body)
		{
			showmeshcount += (show ? 1 : (-1));
			if (childs == null)
			{
				childs = m_Body.transform.GetComponentsInChildren<Transform>(includeInactive: true);
			}
			int layer = (showmeshcount <= 0) ? LayerManager.Hide : LayerManager.Player;
			m_Body.transform.ChangeChildLayer(layer);
		}
	}

	public bool GetMeshShow()
	{
		return showmeshcount > 0;
	}

	public void ShowEntity(bool show)
	{
		ShowHP(show);
		ShowMesh(show);
		SetCollider(show);
	}

	private void ShowChallengeEntity(bool show)
	{
		if (showchallenge != show)
		{
			showchallenge = show;
			ShowEntity(show);
		}
	}

	private void OnInitAfter()
	{
		if (m_MoveCtrl != null)
		{
			m_MoveCtrl.Start();
		}
		if (m_AttackCtrl != null)
		{
			m_AttackCtrl.Start();
		}
		if (m_HitEdit != null)
		{
			m_HitEdit.Init(this);
		}
		if ((bool)m_HPSlider)
		{
			m_HPSlider.Init(this);
		}
		AddInitSkills();
		AliveTime = Updater.AliveTime;
		InitBossHP();
		m_Body.SetEntity(this);
		m_EntityData.InitAfter();
		StartInit();
	}

	public void CurrentHPUpdate()
	{
	}

	protected virtual void StartInit()
	{
	}

	public void Init(int id)
	{
        Debug.Log("@LOG EntityBase.Init id:" + id);
        ClassID = id;
        m_Data = LocalModelManager.Instance.Character_Char.GetBeanById(ClassID);
        UpdateName();
        if (m_Data.Speed == 0)
		{
			rigid.constraints = RigidbodyConstraints.FreezeAll;
		}
		if (m_Type == EntityType.Invalid)
		{
			SetEntityType((EntityType)m_Data.TypeID);
		}
        OnInitBefore();
		m_HitEdit = GetComponent<HitEdit>();
		SdkManager.Bugly_Report(m_HitEdit != null, ClassName, " dont have HitEdit!!!");
		m_EntityData = new EntityData();
		m_AniCtrl = new AnimatorBase();
		mHitCtrl = new EntityHitCtrl();
		mHitCtrl.Init(this);
        AddController<EntityLifeCtrl>();
        AddController<BuffCtrl>();
        mAction.Init();
        SetPosition(base.transform.position);
        InitCharacter();
        CreateModel();
        OnInit();
        OnInitAfter();
        MissBossHP();
    }

    protected virtual void OnInitBefore()
	{
	}

	protected virtual void OnInit()
	{
	}

	public void DeInit()
	{
		DeInitLogic();
		DeInitMesh(showeffect: false);
		OnDeInit();
	}

	protected virtual void OnDeInit()
	{
	}

	protected void DeInitLogic()
	{
		if (Event_DeInit != null)
		{
			Event_DeInit();
		}
		GameLogic.Release.Entity.RemoveLogic(this);
		m_AttackCtrl.DeInit();
		m_MoveCtrl.DeInit();
		m_HitEdit.DeInit();
		mAction.DeInit();
		m_Body.DeInit();
		if ((bool)m_HPSlider)
		{
			m_HPSlider.DeInit();
		}
		if (m_Weapon != null)
		{
			m_Weapon.UnInstall();
			m_Weapon = null;
		}
		m_EntityData.DeInit();
		m_MoveCtrl.ResetRigidBody();
		if (mRotateAttribute != null)
		{
			mRotateAttribute.DeInit();
		}
		if (mRotateShield != null)
		{
			mRotateShield.DeInit();
		}
		SetCollider(enable: false);
		RemoveColliders();
		RemoveControllers();
		UnInstallAllSkills();
		RemoveDivideUpdate();
		OnDeInitLogic();
	}

	protected virtual void OnDeInitLogic()
	{
	}

	protected void DeInitMesh(bool showeffect)
	{
		if (showeffect && (bool)m_Body)
		{
			GameObject gameObject = GameLogic.Release.MapEffect.Get("Effect/Battle/eff_die");
			if ((bool)gameObject)
			{
				gameObject.transform.position = m_Body.DeadNode.transform.position;
			}
		}
		if (m_AniCtrl != null)
		{
			m_AniCtrl.DeInit();
		}
		if (mDeadSeq != null)
		{
			mDeadSeq.Kill();
			mDeadSeq = null;
		}
		if ((bool)m_Body)
		{
			m_Body.CacheEffect();
		}
		if ((bool)child && m_Data != null)
		{
			GameLogic.EntityCache(child, m_Data.Cache);
			child = null;
		}
		if ((bool)this && (bool)base.gameObject)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public virtual void RemoveMove()
	{
	}

	public void DeadBefore()
	{
		OnDeadBefore();
	}

	protected virtual void OnDeadBefore()
	{
		RemoveController<BuffCtrl>();
		RemoveMove();
	}

	private string GetBodyString(string value)
	{
		return Utils.FormatString("Game/Models/{0}", value);
	}

	protected void CreateModel()
	{
		string modelID = m_Data.ModelID;
        //Debug.Log("@LOG CreateModel 1 modelID:" + modelID);
        GameObject t = null;
        if (IsSelf)
		{
			int num = LocalSave.Instance.Equip_GetCloth();
			if (num <= 0 || !ResourceManager.TryLoad(GetBodyString(num.ToString()), out t))
			{
				t = ResourceManager.Load<GameObject>(GetBodyString(modelID));
			}
		}
		else
		{
			t = ResourceManager.Load<GameObject>(GetBodyString(modelID));
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(t);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		child = gameObject;
		m_Body = gameObject.GetComponent<BodyMask>();
		CreateHP();
        if (m_AniCtrl != null)
		{
			m_AniCtrl.Init(this);
		}
		OnCreateModel();
    }

    protected virtual void OnCreateModel()
	{
	}

	protected virtual void InitCharacter()
	{
		m_EntityData.Init(this, ClassID);
	}

	public virtual void InitWeapon(int WeaponID)
	{
		string typeName = "Weapon" + WeaponID;
		Type type = System.Type.GetType(typeName);
		WeaponBase weaponBase = null;
		(m_Weapon = ((!(type == null)) ? (type.Assembly.CreateInstance(typeName) as WeaponBase) : new WeaponBase())).Init(this, WeaponID);
	}

	public void ChangeWeapon(int WeaponID)
	{
		if (m_Weapon != null)
		{
			m_Weapon.UnInstall();
			m_Weapon = null;
		}
		InitWeapon(WeaponID);
	}

	public void PlayAttack()
	{
        Debug.Log("@LOG EntityBase.PlayAttack");
		m_AniCtrl.SendEvent("AttackEnd");
		if (m_Weapon != null)
		{
            Debug.Log("@LOG EntityBase.PlayAttack 2");
            m_Weapon.Attack();
        }
	}

	private void Update()
	{
		if (!GameLogic.Paused)
		{
			UpdateProcess(Updater.delta);
		}

        if(Input.GetKey(KeyCode.Space))
        {
            PlayAttack();
        }
	}

	private void FixedUpdate()
	{
		if (!GameLogic.Paused)
		{
			UpdateFixed();
		}
	}

	protected virtual void UpdateProcess(float delta)
	{
		if (m_EntityData != null)
		{
			if (!m_EntityData.IsDizzy())
			{
				AliveTime += Updater.delta;
			}
			if ((bool)GameLogic.Self && GameLogic.Hold.BattleData.Challenge_MonsterHide() && !IsSelf)
			{
				ShowChallengeEntity(Vector3.Distance(position, GameLogic.Self.position) <= GameLogic.Hold.BattleData.Challenge_MonsterHideRange());
			}
			UpdateDead();
		}
	}

	protected virtual void UpdateFixed()
	{
	}

	public void AddHatredTarget(EntityBase entity)
	{
	}

	protected void StartDeadOffSet()
	{
		Dead_bPlay = true;
	}

	private void UpdateDead()
	{
		if (Dead_bPlay)
		{
			Dead_CurrentCount++;
			if (Dead_CurrentCount == Dead_PlayCount)
			{
				Dead_bPlay = false;
			}
			base.transform.Translate(new Vector3(HittedX, 0f, HittedY) * (Dead_PlayCount - Dead_CurrentCount) * 0.02f);
		}
	}

	public bool GetIsInCamera()
	{
		return m_Body.GetIsInCamera();
	}

	public void SetFlying(bool fly)
	{
		SetFlyWater(fly);
		SetFlyStone(fly);
		OnSetFlying(fly);
	}

	public bool GetFlying()
	{
		return bFlyStone;
	}

	public void SetFlyStone(bool fly)
	{
		bFlyStone = fly;
		SetFlyOne("Entity2Stone", fly);
		m_Body.SetFlyStone(fly);
		if (m_Weapon != null)
		{
			m_Weapon.SetFlying(fly);
		}
		OnSetFlying(fly);
	}

	public void SetFlyWater(bool fly)
	{
		bFlyWater = fly;
		SetFlyOne("Entity2Water", fly);
	}

	private void SetFlyOne(string layer, bool fly)
	{
		mHitCtrl.SetFlyOne(layer, fly);
	}

	protected virtual void OnSetFlying(bool fly)
	{
	}

	public void SetBodyScale(float value)
	{
		mHitCtrl.SetBodyScale(value);
		m_Body.SetBodyScale(value);
	}

	public void GetGoods(int goodid)
	{
		GetGoodsInternal(goodid);
		LocalSave.Instance.BattleIn_UpdateGood(goodid);
	}

	public void BattleInGetGoods(int goodid)
	{
		GetGoodsInternal(goodid);
	}

	private void GetGoodsInternal(int goodid)
	{
		LocalModelManager.Instance.Goods_food.GetBeanById(goodid).GetGoods(this);
	}

	public void PlaySound(int soundid)
	{
		if (soundid != 0)
		{
			GameLogic.Hold.Sound.PlayHitted(soundid, position);
		}
	}

	public void AddBabySkillID(int id)
	{
		mBabySkillIds.Add(id);
	}

	public void RemoveBabySkillID(int id)
	{
		mBabySkillIds.Remove(id);
	}

	public void BabiesClone()
	{
		int i = 0;
		for (int count = mBabySkillIds.Count; i < count; i++)
		{
			AddSkillOverLying(mBabySkillIds[i]);
		}
	}

	public void SetBabyArgs(long value)
	{
		mBabyArgs.Add(value);
	}

	public long GetBabyArgs(int index)
	{
		if (index >= 0 && index < mBabyArgs.Count)
		{
			return mBabyArgs[index];
		}
		SdkManager.Bugly_Report("EntityBase_Baby", Utils.FormatString("GetBabyArgs[{0}] is out of range.", index));
		return 0L;
	}

	public int GetBabyGroundIndex()
	{
		int result = mBabyGroundIndex;
		mBabyGroundIndex++;
		return result;
	}

	public Vector3 GetBabyGroundPos(int index)
	{
		if (index < 0)
		{
			index = 0;
		}
		index %= mBabyGroundPos.Count;
		return mBabyGroundPos[index];
	}

	public void AddDebuff(int debuffid)
	{
		DebuffList.Add(debuffid);
	}

	public void RemoveDebuff(int debuffid)
	{
		DebuffList.Remove(debuffid);
	}

	public List<int> GetDebuffList()
	{
		return DebuffList;
	}

	public void AddThunder2Round(EntityBase target, int debuffid, float ThunderRatio)
	{
		AddThunder2Round(target, debuffid, 6f, ThunderRatio);
	}

	public void AddThunder2Round(EntityBase target, int debuffid, float range, float ThunderRatio)
	{
		List<EntityBase> roundEntities = GameLogic.Release.Entity.GetRoundEntities(target, range, haveself: false);
		int i = 0;
		for (int count = roundEntities.Count; i < count; i++)
		{
			EntityBase entityBase = roundEntities[i];
			GameLogic.SendBuffInternal(entityBase, this, debuffid, ThunderRatio);
			GameObject gameObject = GameLogic.EffectGet("Effect/Attributes/ThunderLine");
			gameObject.GetComponent<ThunderLineCtrl>().UpdateEntity(target, entityBase);
		}
		if (roundEntities.Count > 0)
		{
			GameLogic.Hold.Sound.PlayBattleSpecial(5000010, target.position);
		}
	}

	public void SetCallID(int callid, Vector3 endpos)
	{
		mCallID = callid;
		mCallEndPos = endpos;
	}

	public virtual void SetCollider(bool enable)
	{
		collidercount += (enable ? 1 : (-1));
		mHitCtrl.SetCollider(collidercount > 0);
		SetFlyStone(bFlyStone);
		SetFlyWater(bFlyWater);
	}

	public float GetColliderHeight()
	{
		return mHitCtrl.GetColliderHeight();
	}

	public void SetObstacleCollider(bool value)
	{
		if (!value)
		{
			SetFlyStone(fly: true);
			SetFlyWater(fly: true);
		}
		else
		{
			SetFlyStone(bFlyStone);
			SetFlyWater(bFlyWater);
		}
	}

	public void SetTrigger(bool value)
	{
		mHitCtrl.SetTrigger(value);
	}

	public bool GetTrigger()
	{
		return mHitCtrl.GetTrigger();
	}

	public void RemoveColliders()
	{
		mHitCtrl.RemoveColliders();
	}

	public float GetCollidersSize()
	{
		return mHitCtrl.GetCollidersSize();
	}

	public void SetCollidersScale(float scale)
	{
		mHitCtrl.SetCollidersScale(scale);
	}

	public bool GetColliderEnable()
	{
		return mHitCtrl.GetColliderEnable();
	}

	protected bool GetColliderTrigger()
	{
		return mHitCtrl.GetColliderTrigger();
	}

	public GameObject GetEffect(int fxId)
	{
		return PlayEffect(fxId);
	}

	public GameObject PlayEffect(int fxId)
	{
		return PlayEffect(fxId, Vector3.zero, Quaternion.identity);
	}

	public GameObject PlayEffect(int fxId, Vector3 pos)
	{
		return PlayEffect(fxId, pos, Quaternion.identity);
	}

	public GameObject PlayEffect(int fxId, Vector3 pos, Quaternion rota)
	{
		Fx_fx beanById = LocalModelManager.Instance.Fx_fx.GetBeanById(fxId);
		if (beanById == null)
		{
			SdkManager.Bugly_Report("EntityBase_Ctrl", Utils.FormatString("PlayEffect[{0}] is null.", fxId));
		}
		Transform transform = GameLogic.Release.MapEffect.Get(beanById.Path).transform;
		Transform ketNode = GetKetNode(beanById.Node);
		if (!ketNode)
		{
			transform.SetParent(GameNode.m_PoolParent);
			transform.position = pos;
		}
		else if (beanById.Node == 8)
		{
			transform.SetParent(m_Body.EffectMask.transform);
			transform.localScale = Vector3.one;
			transform.localPosition = Vector3.zero;
			transform.SetParent(GameNode.m_PoolParent);
		}
		else
		{
			transform.SetParent(ketNode);
			transform.localScale = Vector3.one;
			transform.localPosition = Vector3.zero;
		}
		transform.rotation = rota;
		return transform.gameObject;
	}

	public Transform GetKetNode(int index)
	{
		switch (index)
		{
		case 1:
			return m_Body.Body.transform;
		case 2:
		case 8:
			return m_Body.EffectMask.transform;
		case 3:
			return m_Body.HPMask.transform;
		case 4:
			return m_Body.FootMask.transform;
		case 5:
			return m_Body.HeadMask.transform;
		case 6:
			return m_Body.BulletHitMask.transform;
		case 7:
			if (m_HPSlider != null)
			{
				return m_HPSlider.transform;
			}
			break;
		case 9:
			return base.transform;
		}
		return null;
	}

	public Transform GetBulletCreateNode(int index)
	{
		if (index >= 10)
		{
			return m_Body.BulletList[index - 10].transform;
		}
		switch (index)
		{
		case 1:
			return m_Body.FootMask.transform;
		case 2:
			return base.transform;
		default:
			return m_Body.LeftBullet.transform;
		}
	}

	public void AddController<T>() where T : EntityCtrlBase, new()
	{
		T val = new T();
		val.OnStart(val.mActionsList);
		val.SetEntity(this);
		if (val.UseUpdate)
		{
			string name = Utils.FormatString("{0}.AddController<{1}>", m_EntityData.CharID, val.GetType().ToString());
			T val2 = val;
			Updater.AddUpdate(name, val2.OnUpdate);
		}
		ctrlsList.Add(val);
	}

	public void RemoveController<T>() where T : EntityCtrlBase
	{
		int num = ctrlsList.Count - 1;
		EntityCtrlBase entityCtrlBase;
		while (true)
		{
			if (num >= 0)
			{
				entityCtrlBase = ctrlsList[num];
				if (entityCtrlBase is T)
				{
					break;
				}
				num--;
				continue;
			}
			return;
		}
		if (entityCtrlBase.UseUpdate)
		{
			string name = Utils.FormatString("{0}.AddController<{1}>", m_EntityData.CharID, entityCtrlBase.GetType().ToString());
			EntityCtrlBase entityCtrlBase2 = entityCtrlBase;
			Updater.RemoveUpdate(name, entityCtrlBase2.OnUpdate);
		}
		entityCtrlBase.OnRemove();
		ctrlsList.RemoveAt(num);
	}

	public void RemoveControllers()
	{
		for (int num = ctrlsList.Count - 1; num >= 0; num--)
		{
			EntityCtrlBase entityCtrlBase = ctrlsList[num];
			if (entityCtrlBase.UseUpdate)
			{
				string name = Utils.FormatString("{0}.AddController<{1}>", m_EntityData.CharID, entityCtrlBase.GetType().ToString());
				EntityCtrlBase entityCtrlBase2 = entityCtrlBase;
				Updater.RemoveUpdate(name, entityCtrlBase2.OnUpdate);
			}
			entityCtrlBase.OnRemove();
		}
		ctrlsList.Clear();
	}

	public void ExcuteCommend(EBattleAction action, object data)
	{
		int i = 0;
		for (int count = ctrlsList.Count; i < count; i++)
		{
			if (ctrlsList.Count > i)
			{
				EntityCtrlBase entityCtrlBase = ctrlsList[i];
				entityCtrlBase.ExcuteCommend(action, data);
			}
		}
	}

	public HittedData GetHittedData(bool bulletthrough, float bulletangle)
	{
		HittedData hittedData = new HittedData();
		hittedData.type = EHittedType.eNormal;
		if (!GetCanHitted())
		{
			hittedData.type = EHittedType.eInvincible;
		}
		else
		{
			hittedData = OnHittedData(hittedData, bulletthrough, bulletangle);
		}
		return hittedData;
	}

	public HittedData GetHittedData(BulletBase bullet)
	{
		bool bThroughWall = bullet.m_Data.bThroughWall;
		Vector3 eulerAngles = bullet.transform.eulerAngles;
		return GetHittedData(bThroughWall, eulerAngles.y);
	}

	protected virtual HittedData OnHittedData(HittedData data, bool bulletthrough, float bulletangle)
	{
		return data;
	}

	private bool GetCanHitted()
	{
		return Updater.AliveTime - HittedLastTime >= m_EntityData.HittedInterval && !m_EntityData.GetInvincible();
	}

	public virtual void UpdateHittedTime()
	{
		HittedLastTime = Updater.AliveTime;
	}

	public virtual bool SetHitted(HittedData data)
	{
		if (GetSuperArmor())
		{
			return false;
		}
		HittedAngle = data.angle;
		HittedX = MathDxx.Sin(HittedAngle) * data.backtatio;
		HittedY = MathDxx.Cos(HittedAngle) * data.backtatio;
		HittedDirection = new Vector3(HittedX, 0f, HittedY);
		if (data.GetPlayHitted())
		{
			m_AniCtrl.SendEvent("Hitted");
			if (!IsSelf)
			{
				m_Body.Hitted(HittedDirection, data.hittype);
			}
		}
		return true;
	}

	public Vector3 GetHittedDirection()
	{
		return HittedDirection;
	}

	public void SetCanHit(bool value)
	{
		if (bCanHit != value)
		{
			bCanHit = value;
			CantTitTargetShow(!bCanHit);
		}
	}

	private void CantTitTargetShow(bool show)
	{
		if ((bool)CantHitTarget)
		{
			CantHitTarget.SetActive(show);
		}
		if ((bool)HitTarget)
		{
			HitTarget.SetActive(!show);
		}
	}

	public virtual Transform GetHittedMask()
	{
		SdkManager.Bugly_Report("EntityBase.cs", Utils.GetString("EntityBase ", m_Type, " don't achieve GetHittedTransform"));
		return null;
	}

	public void PlayHittedSound()
	{
		int hittedEffectID = m_Data.HittedEffectID;
		if (hittedEffectID != 0)
		{
			GameLogic.Hold.Sound.PlayHitted(hittedEffectID, position);
		}
	}

	private void OnTriggerEnter(Collider o)
	{
		OnTriggerEnterExtra(o);
	}

	protected virtual void OnTriggerEnterExtra(Collider o)
	{
	}

	private void OnTriggerExit(Collider o)
	{
		OnTriggerExitExtra(o);
	}

	protected virtual void OnTriggerExitExtra(Collider o)
	{
	}

	private void OnCollisionEnter(Collision o)
	{
		CollisionEnterExtra(o);
	}

	private void OnCollisionExit(Collision o)
	{
		CollisionExitExtra(o);
	}

	protected virtual void CollisionEnterExtra(Collision o)
	{
	}

	protected virtual void CollisionExitExtra(Collision o)
	{
	}

	protected void CreateHP()
	{
		GameObject gameObject = GameLogic.EffectGet(Utils.GetString("Game/UI/", HPSliderName));
		gameObject.SetParentNormal(GameNode.m_HP);
		HpSlider hpSlider = m_HPSlider = gameObject.GetComponent<HpSlider>();
	}

	protected virtual void OnChangeHP(EntityBase entity, long HP)
	{
	}

	public void ChangeHP(EntityBase entity, long HP)
	{
		if (!GetIsDead())
		{
			ChangeHPMust(entity, HP);
			if ((float)HP < 0f)
			{
				UpdateHittedTime();
			}
		}
	}

	public void ChangeHPMust(EntityBase entity, long HP)
	{
		long num = m_EntityData.ChangeHP(entity, HP);
		if (Type == EntityType.Boss)
		{
			GameLogic.Hold.BattleData.BossChangeHP(num);
		}
		if (GetIsDead())
		{
			DeadCallBack();
		}
		OnChangeHP(entity, num);
		if (entity != null && entity.OnHitAction != null)
		{
			entity.OnHitAction(this, HittedDirection);
		}
		if (GetIsDead())
		{
			if (m_Data.DeadSoundID != 0)
			{
				GameLogic.Hold.Sound.PlayEntityDead(m_Data.DeadSoundID, base.transform.position);
			}
			SetCollider(enable: false);
			if ((bool)entity)
			{
				entity.m_EntityData.ExcuteKillAdd();
			}
		}
	}

	public virtual void DeadCallBack()
	{
		if ((bool)m_HPSlider)
		{
			m_HPSlider.DeInit();
		}
		m_MoveCtrl.ResetRigidBody();
		m_AniCtrl.SendEvent("Dead");
		if (GameLogic.Release.Mode != null && GameLogic.Release.Mode.RoomGenerate != null)
		{
			GameLogic.Release.Mode.RoomGenerate.MonsterDead(this);
		}
		DeInitLogic();
		m_AniCtrl.DeadDown();
		float animationTime = m_AniCtrl.GetAnimationTime("Dead");
		mDeadSeq = DOTween.Sequence().AppendInterval(animationTime * 0.8f).AppendCallback(delegate
		{
			DeInitMesh(showeffect: true);
		});
	}

	public bool GetIsDead()
	{
		if (m_EntityData != null)
		{
			return m_EntityData.CurrentHP <= 0;
		}
		return true;
	}

	protected virtual List<BattleDropData> OnGetGoodList()
	{
		return new List<BattleDropData>();
	}

	public void DivideAction(float x, float z)
	{
		dividemove = new Vector3(x, 0f, z);
		Vector2Int roomXYInside = GameLogic.Release.MapCreatorCtrl.GetRoomXYInside(base.transform.position + dividemove);
		Vector3 worldPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(roomXYInside);
		dividemove = worldPosition - base.transform.position;
		Updater.AddUpdate(Utils.FormatString("{0}.DivideAction", m_EntityData.CharID), OnDivideUpdate);
	}

	private void RemoveDivideUpdate()
	{
		Updater.RemoveUpdate(Utils.FormatString("{0}.DivideAction", m_EntityData.CharID), OnDivideUpdate);
	}

	private void OnDivideUpdate(float delta)
	{
		SetPositionBy(dividemove / divide_maxframe);
		divide_frame++;
		if (divide_frame == divide_maxframe)
		{
			RemoveDivideUpdate();
		}
	}

	public void SetEulerAngles(Vector3 e)
	{
		_eulerAngles = e;
	}

	public void SelfMoveBy(Vector3 pos)
	{
		if (!m_EntityData.IsDizzy())
		{
			SetPositionBy(pos);
		}
	}

	public void SetPosition(Vector3 pos)
	{
		if ((bool)this && (bool)base.transform)
		{
			base.transform.position = pos;
			_position = pos;
		}
	}

	public void SetPositionBy(Vector3 pos)
	{
		if ((bool)this && (bool)base.transform && GetCanPositionBy())
		{
			SetPositionBy_P = GetMoveDistance(pos);
			SetPositionByInternal(SetPositionBy_P);
		}
	}

	protected void SetPositionByInternal(Vector3 pos)
	{
		base.transform.Translate(pos);
		if (this.Event_PositionBy != null)
		{
			this.Event_PositionBy(pos);
		}
		OnSetPositionBy(pos);
		_position = base.transform.position;
	}

	protected virtual bool GetCanPositionBy()
	{
		return true;
	}

	protected virtual void OnSetPositionBy(Vector3 pos)
	{
	}

	private Vector3 GetMoveDistance(Vector3 pos)
	{
		if (IsSelf)
		{
			return pos;
		}
		move_layermask = ((!GetFlying()) ? LayerManager.Move_Ground : LayerManager.Move_Fly);
		move_offset = 0.1f;
		move_dis = pos.magnitude + move_offset;
		Vector3 point = position + move_vec * (mHitCtrl.m_CapsuleCollider.height - 1f) / 2f - pos.normalized * move_offset;
		Vector3 point2 = position - move_vec * (mHitCtrl.m_CapsuleCollider.height - 1f) / 2f - pos.normalized * move_offset;
		Vector3 localScale = base.transform.localScale;
		move_hits = Physics.CapsuleCastAll(point, point2, localScale.x * mHitCtrl.m_CapsuleCollider.radius - move_offset - 0.02f, pos, move_dis, move_layermask);
		int i = 0;
		for (int num = move_hits.Length; i < num; i++)
		{
			move_hit = move_hits[i];
			if (State == EntityState.Hitted)
			{
				return pos.normalized * (move_hit.distance - move_offset);
			}
			Vector3 normal = move_hit.normal;
			float num2 = MathDxx.Abs(normal.x);
			Vector3 normal2 = move_hit.normal;
			if (num2 > MathDxx.Abs(normal2.z))
			{
				pos.x = 0f;
				continue;
			}
			Vector3 normal3 = move_hit.normal;
			float num3 = MathDxx.Abs(normal3.x);
			Vector3 normal4 = move_hit.normal;
			if (num3 < MathDxx.Abs(normal4.z))
			{
				pos.z = 0f;
			}
			else if (MathDxx.Abs(pos.x) > MathDxx.Abs(pos.z))
			{
				pos.z = 0f;
			}
			else
			{
				pos.x = 0f;
			}
		}
		return pos;
	}

	public void SetSuperArmor(bool value)
	{
		mSuperArmor += (value ? 1 : (-1));
	}

	public bool GetSuperArmor()
	{
		return mSuperArmor > 0;
	}

	public EntityPartBodyBase CreatePartBody(int partbodyid, Vector3 pos, float time)
	{
		PartBodyData partBodyData = mPartBodyList[partbodyid];
		if (!partBodyData.CanAdd())
		{
			return null;
		}
		partBodyData.Add();
		EntityPartBodyBase entityPartBodyBase = CreatePartBody(partbodyid, pos);
		entityPartBodyBase.SetParent(this);
		entityPartBodyBase.Init(partbodyid);
		if (time > 0f)
		{
			entityPartBodyBase.SetAliveTime(time);
		}
		entityPartBodyBase.OnRemoveEvent = OnPartBodyRemove;
		return entityPartBodyBase;
	}

	private void OnPartBodyRemove(int partbodyid)
	{
		mPartBodyList[partbodyid].Remove();
	}

	private EntityPartBodyBase CreatePartBody(int partBodyID, Vector3 pos)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>(Utils.GetString("Game/PartBody/PartBodyNode", partBodyID)));
		gameObject.transform.parent = GameNode.m_Battle.transform;
		gameObject.transform.localPosition = pos;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		return gameObject.GetComponent<EntityPartBodyBase>();
	}

	public void AddRotateFollow(EntityBase entity)
	{
		int charID = entity.m_Data.CharID;
		if (!mRotateFollowList.TryGetValue(charID, out RotateFollowData value))
		{
			value = new RotateFollowData(charID);
			value.Init(this, 3f, 3f);
			mRotateFollowList.Add(charID, value);
		}
		value.Add(entity);
	}

	public void RemoveRotateFollow(EntityBase entity)
	{
		int charID = entity.m_Data.CharID;
		if (mRotateFollowList.TryGetValue(charID, out RotateFollowData value))
		{
			value.Remove(entity);
		}
	}

	public Vector3 GetRotateFollowPosition(EntityBase entity)
	{
		int charID = entity.m_Data.CharID;
		if (mRotateFollowList.TryGetValue(charID, out RotateFollowData value))
		{
			return value.GetPosition(entity);
		}
		return position;
	}

	public void SetRotateFollowIndex(int key, int index)
	{
		if (!mRotateIndexList.ContainsKey(key))
		{
			mRotateIndexList.Add(key, index);
		}
		else
		{
			mRotateIndexList[key] = index;
		}
	}

	public int GetRotateFollowIndex(int key)
	{
		int value = 0;
		if (mRotateIndexList.TryGetValue(key, out value))
		{
			return value;
		}
		return value;
	}

	public void AddNewRotateAttribute(GameObject o)
	{
		if (mRotateAttribute == null)
		{
			mRotateAttribute = new RotateBallClass();
			mRotateAttribute.SetRadius(3.5f);
			mRotateAttribute.Init(this, "RotateAttribute", -5f, 180f);
		}
		mRotateAttribute.AddNewRotateAttribute(o);
	}

	public void RemoveRotateAttribute(GameObject o)
	{
		if (mRotateAttribute != null)
		{
			mRotateAttribute.Remove(o);
		}
	}

	public void AddNewRotateShield(GameObject o)
	{
		if (mRotateShield == null)
		{
			mRotateShield = new RotateClass();
			mRotateShield.Init(this, "RotateShield", 1f, 360f);
		}
		mRotateShield.AddNewRotateAttribute(o);
	}

	public void RemoveRotateShield(GameObject o)
	{
		if (mRotateShield != null)
		{
			mRotateShield.Remove(o);
		}
	}

	public void AddNewRotateSword(GameObject o)
	{
		if (mRotateSword == null)
		{
			mRotateSword = new RotateBallClass();
			mRotateSword.SetRadius(0.3f);
			mRotateSword.Init(this, "RotateSword", 4f, 180f);
		}
		mRotateSword.AddNewRotateAttribute(o);
	}

	public void RemoveRotateSword(GameObject o)
	{
		if (mRotateSword != null)
		{
			mRotateSword.Remove(o);
		}
	}

	public void AddSkill(int skillId, params object[] args)
	{
		AddSkillInternal(skillId, args);
		if (IsSelf)
		{
			LocalSave.Instance.BattleIn_UpdateSkill(skillId);
		}
	}

	protected void AddSkillInternal(int skillId, params object[] args)
	{
		if (!skillsList.ContainsKey(skillId))
		{
			Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId);
			if (beanById != null)
			{
				SkillBase skillBase = new SkillBase();
				skillBase.Install(this, beanById, args);
				skillsList.Add(skillId, skillBase);
				skillidList.Add(skillId);
			}
		}
	}

	public void AddSkillAuto(int skillId, params object[] args)
	{
		if (!skillsAutoList.ContainsKey(skillId))
		{
			Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId);
			if (beanById != null)
			{
				SkillBase skillBase = new SkillBase();
				skillBase.Install(this, beanById, args);
				skillsAutoList.Add(skillId, skillBase);
			}
		}
	}

	public void AddSkillAttribute(int skillId, params object[] args)
	{
		Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId);
		if (beanById != null)
		{
			SkillBase skillBase = new SkillBase();
			skillBase.Install(this, beanById, args);
			skillsAttributeList.Add(skillBase);
		}
	}

	public void AddSkillBaby(int skillId, params object[] args)
	{
		Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId);
		if (beanById != null)
		{
			SkillBase skillBase = new SkillBase();
			skillBase.Install(this, beanById, args);
			skillsOverlyingList.Add(skillBase);
		}
	}

	public void AddSkillOverLying(int skillId, params object[] args)
	{
		Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId);
		if (beanById != null)
		{
			SkillBase skillBase = new SkillBase();
			skillBase.Install(this, beanById, args);
			skillsOverlyingList.Add(skillBase);
		}
	}

	public void AddSkillTest(int skillId)
	{
		Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId);
		if (beanById != null)
		{
			SkillBase skillBase = new SkillBase();
			skillBase.Install(this, beanById);
			skillsOverlyingList.Add(skillBase);
			skillidList.Add(skillId);
		}
	}

	public void RemoveSkill(int skillId)
	{
		if (skillsList.TryGetValue(skillId, out SkillBase value))
		{
			value.Uninstall();
			skillsList.Remove(skillId);
			skillidList.Remove(skillId);
		}
	}

	private void UnInstallAllSkills()
	{
		Dictionary<int, SkillBase>.Enumerator enumerator = skillsList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.Uninstall();
		}
		Dictionary<int, SkillBase>.Enumerator enumerator2 = skillsAutoList.GetEnumerator();
		while (enumerator2.MoveNext())
		{
			enumerator2.Current.Value.Uninstall();
		}
		int i = 0;
		for (int count = skillsOverlyingList.Count; i < count; i++)
		{
			skillsOverlyingList[i].Uninstall();
		}
		int j = 0;
		for (int count2 = skillsAttributeList.Count; j < count2; j++)
		{
			skillsAttributeList[j].Uninstall();
		}
		skillsAttributeList.Clear();
		skillsOverlyingList.Clear();
		skillsList.Clear();
		skillidList.Clear();
		skillsAutoList.Clear();
	}

	private void AddInitSkills()
	{
		int i = 0;
		for (int num = m_Data.Skills.Length; i < num; i++)
		{
			AddSkillAuto(m_Data.Skills[i]);
		}
	}

	public List<int> GetSkillList()
	{
		return skillidList;
	}

	public void BattleInInitSkill(int skillId)
	{
		AddSkillInternal(skillId);
	}

	public void SetEntityType(EntityType type)
	{
		m_Type = type;
		UpdateName();
	}

	private void UpdateName()
	{
		if (m_Data != null)
		{
			base.name = Utils.GetString("Entity_", m_Type.ToString(), "_", m_Data.CharID, "_", GameLogic.Random(1000, 9999));
		}
		else
		{
			base.name = Utils.GetString("Entity_", m_Type.ToString(), "_", _name, "_", GameLogic.Random(1000, 9999));
		}
	}

	private void InitBossHP()
	{
		if (Type == EntityType.Boss && !bDivide && !bCall)
		{
			long bossHP = GetBossHP();
			GameLogic.Hold.BattleData.AddBossMaxHP(bossHP);
		}
	}

	private void MissBossHP()
	{
		if (Type == EntityType.Boss && m_Data != null && m_Data.Divide == 0 && DivideID == string.Empty && !bDivide)
		{
			ShowHP(show: false);
		}
	}

	public void SetRoomType(RoomGenerateBase.RoomType type)
	{
		if (type == RoomGenerateBase.RoomType.eBoss && !bCall)
		{
			SetEntityType(EntityType.Boss);
		}
	}

	public void SetEntityDivide(RoomGenerateBase.RoomType type)
	{
		if (type == RoomGenerateBase.RoomType.eBoss)
		{
			SetEntityType(EntityType.Boss);
		}
	}

	protected virtual long GetBossHP()
	{
		throw new Exception(Utils.FormatString("Entity {0} not achieve [GetBossHP]", m_Data.CharID));
	}

	public void WeaponHandUpdate()
	{
		if (m_WeaponHand != null)
		{
			UnityEngine.Object.Destroy(m_WeaponHand);
		}
		if (m_Weapon != null && m_Weapon.m_Data != null)
		{
			GameObject gameObject = GameLogic.EffectGet(Utils.GetString("Game/WeaponHand/WeaponHand", m_Weapon.m_Data.WeaponID));
			if ((bool)gameObject)
			{
				gameObject.transform.parent = WeaponBase.GetWeaponNode(m_Body, m_Weapon.m_Data.WeaponNode);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localRotation = Quaternion.identity;
				m_WeaponHand = gameObject;
				MeshRenderer[] componentsInChildren = m_WeaponHand.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
				m_WeaponHand.ChangeChildLayer(LayerManager.Player);
			}
		}
	}

	public void WeaponHandShow(bool show)
	{
		if (m_WeaponHand != null)
		{
			m_WeaponHand.SetActive(show);
		}
	}
}
