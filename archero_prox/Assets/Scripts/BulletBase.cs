




using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class BulletBase : PauseObject
{
	public class BulletLine
	{
		private GameObject mBulletLine;

		private BulletLineCtrl mLineCtrl;

		private BulletBase mBullet;

		private BulletBase mLastBullet;

		public void Init(BulletBase bullet, BulletBase lastbullet)
		{
			mBullet = bullet;
			mLastBullet = lastbullet;
			CreateBulletLine();
		}

		private void CreateBulletLine()
		{
			mBulletLine = GameLogic.EffectGet("Effect/Attributes/BulletLine");
			mBulletLine.transform.SetParent(GameNode.m_PoolParent.transform);
			mLineCtrl = mBulletLine.GetComponent<BulletLineCtrl>();
			mLineCtrl.Init(mBullet, mLastBullet);
			mLineCtrl.mOverDistanceEvent = DeInit;
		}

		public void DeInit()
		{
			if ((bool)mLineCtrl)
			{
				mLineCtrl.mOverDistanceEvent = null;
				mLineCtrl.Cache();
				mLineCtrl = null;
			}
		}
	}

	protected class TrailCtrl
	{
		private struct TrailWidth
		{
			public float startWidth;

			public float endWidth;
		}

		public bool bShow;

		private GameObject trail;

		private List<TrailRenderer> mTrailRenderers = new List<TrailRenderer>();

		private List<float> mTrailTime = new List<float>();

		private List<MeshRenderer> mTrailMeshs = new List<MeshRenderer>();

		private List<ParticleSystem> mTrailParticles = new List<ParticleSystem>();

		private List<TrailWidth> mTrailsWidth = new List<TrailWidth>();

		public TrailCtrl(Transform trail)
		{
			if ((bool)trail)
			{
				this.trail = trail.gameObject;
				InitTrailRenderer();
				InitTrailMesh();
				InitParticles();
			}
		}

		private void InitTrailRenderer()
		{
			TrailRenderer[] componentsInChildren = trail.GetComponentsInChildren<TrailRenderer>(includeInactive: true);
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				TrailRenderer trailRenderer = componentsInChildren[i];
				mTrailTime.Add(trailRenderer.time);
				trailRenderer.sortingLayerName = "Hit";
				mTrailRenderers.Add(trailRenderer);
				mTrailsWidth.Add(new TrailWidth
				{
					startWidth = trailRenderer.startWidth,
					endWidth = trailRenderer.endWidth
				});
			}
		}

		private void TrailRendererShow(bool show)
		{
			int i = 0;
			for (int count = mTrailRenderers.Count; i < count; i++)
			{
				if (mTrailRenderers[i] != null)
				{
					mTrailRenderers[i].Clear();
				}
			}
		}

		private void InitTrailMesh()
		{
			MeshRenderer[] componentsInChildren = trail.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				MeshRenderer meshRenderer = componentsInChildren[i];
				meshRenderer.sortingLayerName = "BulletEffect";
				meshRenderer.sortingOrder = -1;
				mTrailMeshs.Add(meshRenderer);
			}
		}

		private void TrailMeshShow(bool show)
		{
			int i = 0;
			for (int count = mTrailMeshs.Count; i < count; i++)
			{
				if (mTrailMeshs[i] != null)
				{
					mTrailMeshs[i].enabled = show;
				}
			}
		}

		private void InitParticles()
		{
			ParticleSystem[] componentsInChildren = trail.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				mTrailParticles.Add(componentsInChildren[i]);
			}
		}

		private void TrailParticlesShow(bool show)
		{
			int i = 0;
			for (int count = mTrailParticles.Count; i < count; i++)
			{
				ParticleSystem particleSystem = mTrailParticles[i];
				if ((bool)particleSystem)
				{
					particleSystem.Clear();
					particleSystem.SetParticles(null, 0);
				}
			}
		}

		public void TrailShow(bool show)
		{
			TrailRendererShow(show);
			TrailMeshShow(show);
			TrailParticlesShow(show);
			if (trail != null)
			{
				trail.SetActive(show);
			}
		}

		public float GetTrailTime()
		{
			if (mTrailRenderers.Count > 0)
			{
				return mTrailRenderers[0].time;
			}
			return 0f;
		}

		public void SetTrailTime(float ratio)
		{
			int i = 0;
			for (int count = mTrailRenderers.Count; i < count; i++)
			{
				if (mTrailTime.Count > i)
				{
					mTrailRenderers[i].time = mTrailTime[i] * ratio;
				}
			}
		}

		public void Clear()
		{
		}

		public void UpdateTrailWidthScale(float scale)
		{
			int i = 0;
			for (int count = mTrailRenderers.Count; i < count; i++)
			{
				TrailRenderer trailRenderer = mTrailRenderers[i];
				TrailWidth trailWidth = mTrailsWidth[i];
				trailRenderer.startWidth = trailWidth.startWidth * scale;
				TrailRenderer trailRenderer2 = mTrailRenderers[i];
				TrailWidth trailWidth2 = mTrailsWidth[i];
				trailRenderer2.endWidth = trailWidth2.endWidth * scale;
			}
		}
	}

	public class TriggerData
	{
		private const float delaytime = 1f;

		private float lastintime;

		public GameObject target;

		public Collider collider;

		private int lastinframe;

		public int currentframe;

		public bool GetCanHit()
		{
			if (Time.frameCount == currentframe)
			{
				if (lastinframe != currentframe - 1)
				{
					lastinframe = currentframe;
					lastintime = Updater.AliveTime;
					return true;
				}
				lastinframe = currentframe;
				if (Updater.AliveTime - lastintime > 1f)
				{
					lastintime += 1f;
					return true;
				}
			}
			return false;
		}
	}

	public Action OnBulletCache;

	protected Transform mTransform;

	protected GameObject mGameObject;

	public const float g = 9.8f;

	private int BulletID;

	protected string ClassName;

	protected bool bInit;

	protected bool bbMoveEnable = true;

	protected bool bFlyRotate = true;

	public Weapon_weapon m_Data;

	public BulletTransmit mBulletTransmit;

	private bool bBoxEnable = true;

	protected BoxCollider[] boxList;

	protected int boxListCount;

	protected SphereCollider[] sphereList;

	protected int sphereListCount;

	protected CapsuleCollider[] capsuleList;

	protected int capsuleListCount;

	private int CurrentFrameCount;

	private GameObject AttackSoundObj;

	private GameObject trailattrobj;

	private GameObject headattrobj;

	protected Action OnHitSelf;

	protected Action<Collider> HitWallAction;

	private Sequence seq_flyhit;

	protected SequencePool mSeqPool = new SequencePool();

	protected float moveX;

	protected float moveY;

	protected float bulletAngle;

	protected Vector3 moveDirection;

	private Vector3 raycastPoint;

	protected Vector3 StartPosition;

	protected float StartPositionY;

	protected float PosFromStart2Target = 5f;

	protected Transform shadow;

	protected GameObject shadowGameObject;

	protected Vector3 shadow_initpos;

	private float mDistance;

	protected float LifeTime;

	protected float CreateTime;

	protected float RemoveTime;

	private float mSpeed;

	protected Transform childMesh;

	protected MeshRenderer childMeshRender;

	private Transform rotateTran;

	protected Vector3 childMesh_initpos;

	private TrailRenderer[] trails;

	private GameObject lastwall;

	private ActionBasic action = new ActionBasic();

	protected List<EntityBase> mHitList = new List<EntityBase>();

	protected GameObject mHitWall;

	protected Action<bool> OnTrailShowEvent;

	private bool bDelayCache;

	protected ConditionBase mCondition;

	protected float[] mArgs;

	protected EntityBase Target;

	protected Vector3 TargetPosition;

	[NonSerialized]
	public BulletBase mLastBullet;

	private BulletLine mBulletLine;

	protected Transform mBulletModel;

	private int mArrowEjectCount;

	private int mArrowEjectMaxCount;

	private float currentHitRatio = 1f;

	private float catapult_x;

	private float catapult_z;

	private float catapult_alpha;

	private float catapult_scale;

	protected Action<float> meshAlphaAction;

	protected int mReboundWallCount;

	protected int mReboundWallMaxCount;

	private SphereCollider mReboundSphere;

	private BulletBase HitCreate2_Bullet;

	private GameObject HitSputter_o;

	private List<EntityBase> HitSputter_list;

	private float HitSputter_hitratio = 0.5f;

	private int HitSputter_i;

	private int HitSputter_imax;

	protected bool bLight45;

	protected float mSpeedRatio = 1f;

	protected TrailCtrl mTrailCtrl;

	private Dictionary<GameObject, TriggerData> mTriggerList = new Dictionary<GameObject, TriggerData>();

	private Dictionary<GameObject, TriggerData>.Enumerator mTriggerListIter;

	private int TriggerTest_Interval;

	private int TriggerTest_TriggerFrame;

	private int TriggerTest_Boxi;

	private int TriggerTest_Spherei;

	private int TriggerTest_Capsulei;

	private RaycastHit[] TriggerTest_Hits;

	private RaycastHit TriggerTest_Hit;

	private float TriggerTest_Min;

	private float TriggerTest_MoveDis;

	private Vector3 TriggerTest_CurrentPos;

	private float TriggerTest_BeforeHit;

	private Vector3 TriggerTest_vec = new Vector3(0f, 1f, 0f);

	private Collider minCollider;

	private float tempdis;

	private float tempmin;

	private float mindis;

	private int mInitFrameCount;

	private List<Collider> mColliders = new List<Collider>();

	private bool canhitted;

	private HitStruct target_hs;

	private int TriggerExtra_hit;

	private bool TriggerExtra_bEject;

	private bool TriggerExtra_bThroughEnemy;

	private RaycastHit HitWall_hit;

	private Vector3 HitWall_dir = default(Vector3);

	private bool bShowBullet = true;

	private bool bGetTrackTarget;

	private EntityBase mTrackTarget;

	protected bool bExcuteReboundWall;

	private float BulletRayCast_cudris;

	protected Vector3 Parabola_position = default(Vector3);

	private Vector3 OnMove_vec = default(Vector3);

	protected float Parabola_MaxHeight = 2f;

	protected AnimationCurve Parabola_Curve;

	private Keyframe beforeframe;

	private Keyframe afterframe = default(Keyframe);

	private AnimationCurve Horizontal_Curve;

	private Vector3 Horizontal_vec = default(Vector3);

	private float bulletscale;

	protected bool bMoveEnable
	{
		get
		{
			return bbMoveEnable;
		}
		set
		{
			bbMoveEnable = value;
		}
	}

	protected virtual bool bFlyCantHit => false;

	public EntityBase m_Entity
	{
		get;
		private set;
	}

	protected float CurrentDistance
	{
		get;
		set;
	}

	protected float Distance
	{
		get
		{
			return mDistance;
		}
		set
		{
			mDistance = value;
		}
	}

	protected float Speed
	{
		get
		{
			return mSpeed;
		}
		set
		{
			mSpeed = value;
		}
	}

	protected float FrameDistance => Speed * mSpeedRatio * Updater.delta;

	public int bulletids
	{
		get;
		private set;
	}

	protected virtual bool bZScale => false;

	private void Awake()
	{
		mTransform = base.transform;
		mGameObject = base.gameObject;
		boxList = GetComponents<BoxCollider>();
		boxListCount = boxList.Length;
		sphereList = GetComponents<SphereCollider>();
		sphereListCount = sphereList.Length;
		capsuleList = GetComponents<CapsuleCollider>();
		capsuleListCount = capsuleList.Length;
		AwakeInit();
	}

	protected virtual void AwakeInit()
	{
	}

	private void Start()
	{
	}

	protected virtual void StartInit()
	{
	}

	private void DeInitData()
	{
		mCondition = null;
		mLastBullet = null;
		if (mBulletLine != null)
		{
			mBulletLine.DeInit();
			mBulletLine = null;
		}
		mHitWall = null;
		OnDeInit();
		action.DeInit();
		BoxEnable(enable: false);
		bMoveEnable = false;
		TrailShow(show: false);
		ShadowShow(show: false);
		FlyOver();
		Updater.RemoveUpdate("BulletBase_Skill.Catapult", OnCatapult);
	}

	public void DeInit()
	{
		bInit = false;
		CacheLater();
	}

	protected virtual void OnDeInit()
	{
		KillSequence();
		if (OnBulletCache != null)
		{
			OnBulletCache();
		}
	}

	public void Init(EntityBase entity, int BulletID)
	{
		this.BulletID = BulletID;
		m_Data = LocalModelManager.Instance.Weapon_weapon.GetBeanById(BulletID);
		Init_Model();
		Transform trail = null;
		if ((bool)mBulletModel)
		{
			shadow = mBulletModel.Find("shadow");
			if ((bool)shadow)
			{
				shadowGameObject = shadow.gameObject;
			}
			childMesh = mBulletModel.Find("child");
			if (childMesh != null)
			{
				rotateTran = childMesh.Find("rotate");
				childMesh_initpos = childMesh.localPosition;
				childMeshRender = childMesh.GetComponentInChildren<MeshRenderer>();
			}
			trail = mBulletModel.Find("trail");
		}
		bulletids = GameLogic.GetBulletID();
		mTrailCtrl = new TrailCtrl(trail);
		HitWallAction = HitWall;
		TriggerTest_TriggerFrame = 0;
		mTriggerList.Clear();
		if (m_Data.AliveTime > 0 && m_Data.bCache)
		{
			mCondition = AIMoveBase.GetConditionTime(m_Data.AliveTime);
		}
		mInitFrameCount = 0;
		bInit = true;
		currentHitRatio = 1f;
		mHitList.Clear();
		m_Entity = entity;
		bDelayCache = false;
		bGetTrackTarget = false;
		action.ActionClear();
		action.Init();
		Target = null;
		if ((bool)childMesh)
		{
			childMesh.gameObject.SetActive(value: true);
			childMesh.localPosition = childMesh_initpos;
		}
		CurrentDistance = 0f;
		bMoveEnable = true;
		BoxEnable(!bFlyCantHit);
		mSpeed = m_Data.Speed;
		if ((bool)m_Entity)
		{
			mSpeed *= m_Entity.m_EntityData.BulletSpeed;
		}
		ShadowShow(show: true);
		mDistance = m_Data.Distance;
		Vector3 eulerAngles = mTransform.eulerAngles;
		bulletAngle = eulerAngles.y;
		UpdateMoveDirection();
		Vector3 position = mTransform.position;
		float x = position.x;
		Vector3 position2 = mTransform.position;
		StartPosition = new Vector3(x, 0f, position2.z);
		Vector3 position3 = mTransform.position;
		StartPositionY = position3.y;
		if (m_Data.CreateSoundID != 0)
		{
			AttackSoundObj = GameLogic.Hold.Sound.PlayBulletCreate(m_Data.CreateSoundID, mTransform.position);
		}
		BulletParabolaInit();
		BulletHorizontalInit();
		CreateBulletEffect();
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	public bool GetInit()
	{
		return bInit;
	}

	public EntityBase GetEntity()
	{
		return m_Entity;
	}

	protected void UpdateMoveDirection()
	{
		moveX = MathDxx.Sin(bulletAngle);
		moveY = MathDxx.Cos(bulletAngle);
		if (bZScale)
		{
			moveY *= 1.23f;
		}
		if (bFlyRotate)
		{
			Transform transform = mTransform;
			Vector3 eulerAngles = mTransform.eulerAngles;
			float x = eulerAngles.x;
			float angle = Utils.getAngle(moveX, moveY);
			Vector3 eulerAngles2 = mTransform.eulerAngles;
			transform.rotation = Quaternion.Euler(x, angle, eulerAngles2.z);
		}
		moveDirection = new Vector3(moveX, 0f, moveY);
	}

	protected Vector3 GetMoveDirection(float angle)
	{
		float x = MathDxx.Sin(angle);
		float num = MathDxx.Cos(angle);
		if (bZScale)
		{
			num *= 1.23f;
		}
		return new Vector3(x, 0f, num);
	}

	private void CreateBulletEffect()
	{
		if (m_Data.CreatePath != string.Empty)
		{
			Transform transform = GameLogic.EffectGet(Utils.GetString("Game/BulletCreate/BulletCreate_", m_Data.CreatePath)).transform;
			transform.parent = mTransform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			transform.parent = GameNode.m_PoolParent;
		}
	}

	public virtual void SetBulletAttribute(BulletTransmit bullet)
	{
		mBulletTransmit = bullet;
		int i = 0;
		for (int num = m_Data.Attributes.Length; i < num; i++)
		{
			mBulletTransmit.attribute.Excute(m_Data.Attributes[i]);
		}
		TrailShow(show: true);
		UpdateBulletAttribute();
		OnSetBulletAttribute();
	}

	public void UpdateBulletAttribute()
	{
		mReboundWallCount = mBulletTransmit.attribute.ReboundWall.Value;
		mReboundWallMaxCount = mReboundWallCount;
		mArrowEjectCount = mBulletTransmit.attribute.ArrowEject.Value;
		mArrowEjectMaxCount = mArrowEjectCount;
	}

	protected virtual void OnSetBulletAttribute()
	{
	}

	public void SetArgs(params float[] args)
	{
		mArgs = args;
		OnSetArgs();
	}

	protected virtual void OnSetArgs()
	{
	}

	public virtual void SetTarget(EntityBase entity, int size = 1)
	{
		if ((bool)entity)
		{
			Target = entity;
			List<Vector2Int> roundEmpty = GameLogic.Release.MapCreatorCtrl.GetRoundEmpty(Target.position, size);
			if (roundEmpty.Count == 0)
			{
				TargetPosition = Target.position;
			}
			else
			{
				int index = GameLogic.Random(0, roundEmpty.Count);
				TargetPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(roundEmpty[index]);
			}
			PosFromStart2Target = Vector3.Distance(StartPosition, TargetPosition);
			if (PosFromStart2Target < 0.01f)
			{
				PosFromStart2Target = 0.01f;
			}
		}
		UpdateParabolaArgs();
	}

	public void SetPosFromTarget(float dis)
	{
		PosFromStart2Target = dis;
		UpdateParabolaArgs();
	}

	protected void RotateDeal()
	{
		if (!bMoveEnable)
		{
			return;
		}
		if (m_Data.LookCamera != 0 && (bool)childMesh)
		{
			childMesh.rotation = Quaternion.Euler(-35f, 0f, 0f);
		}
		if (m_Data.RotateSpeed > 0f)
		{
			if ((bool)rotateTran)
			{
				Transform transform = rotateTran;
				Vector3 localEulerAngles = rotateTran.localEulerAngles;
				float x = localEulerAngles.x;
				Vector3 localEulerAngles2 = rotateTran.localEulerAngles;
				float y = localEulerAngles2.y + m_Data.RotateSpeed;
				Vector3 localEulerAngles3 = rotateTran.localEulerAngles;
				transform.localRotation = Quaternion.Euler(x, y, localEulerAngles3.z);
			}
			else if ((bool)childMesh)
			{
				Transform transform2 = childMesh;
				Vector3 localEulerAngles4 = childMesh.localEulerAngles;
				float x2 = localEulerAngles4.x;
				Vector3 localEulerAngles5 = childMesh.localEulerAngles;
				float y2 = localEulerAngles5.y + m_Data.RotateSpeed;
				Vector3 localEulerAngles6 = childMesh.localEulerAngles;
				transform2.localRotation = Quaternion.Euler(x2, y2, localEulerAngles6.z);
			}
		}
	}

	protected void FlyOver()
	{
	}

	protected virtual void Cache()
	{
		if ((bool)this && (bool)mGameObject)
		{
			DeInitData();
			if ((bool)m_Entity && m_Entity.IsSelf)
			{
				GameLogic.Release.PlayerBullet.Cache(m_Data.WeaponID, mGameObject);
			}
			else
			{
				GameLogic.BulletCache(m_Data.WeaponID, mGameObject);
			}
		}
	}

	private void CacheLater()
	{
		BulletCache();
	}

	public void BulletCache()
	{
		Cache();
	}

	public void BulletDestroy()
	{
		overDistance();
	}

	protected virtual void overDistance()
	{
		if (m_Data.DeadSoundID != 0)
		{
			GameLogic.Hold.Sound.PlayBulletDead(m_Data.DeadSoundID, mTransform.position);
		}
		OnOverDistance();
		ShowDeadEffect();
		HitHero(null, null);
	}

	protected virtual void OnOverDistance()
	{
	}

	protected void ShowDeadEffect()
	{
		if (m_Data.DeadEffectID != 0)
		{
			PlayEffect(m_Data.DeadEffectID, mTransform.position, mTransform.rotation);
		}
	}

	protected void HitHero(EntityBase entity, Collider o)
	{
		if (childMesh != null)
		{
			childMesh.localPosition = childMesh_initpos;
		}
		OnHitHero(entity);
		if ((bool)entity)
		{
			entity.AddHatredTarget(m_Entity);
		}
		if ((bool)entity)
		{
			Transform ketNode = entity.GetKetNode(m_Data.DeadNode);
			if ((bool)ketNode)
			{
				mTransform.SetParent(ketNode);
				mTransform.localPosition = Vector3.zero;
			}
		}
		if (!m_Data.bThroughEntity || !(entity != null))
		{
			BoxEnable(enable: false);
			bMoveEnable = false;
			TrailShow(show: false);
			ShadowShow(show: false);
			FlyOver();
			DeInitDelay(m_Data.DeadDelay);
		}
	}

	private void DeInitDelay(float deaddelay)
	{
		TrailShow(show: false);
		if (deaddelay > 0f)
		{
			action.ActionClear();
			action.AddAction(new ActionBasic.ActionWait
			{
				waitTime = deaddelay / 1000f
			});
			action.AddAction(new ActionBasic.ActionDelegate
			{
				action = DeInit
			});
		}
		else
		{
			DeInit();
		}
	}

	protected virtual void OnHitHero(EntityBase entity)
	{
	}

	protected void ShadowDeal()
	{
		if ((bool)shadow)
		{
			shadow.localPosition = shadow_initpos;
			Transform transform = shadow;
			Vector3 position = shadow.position;
			float x = position.x;
			Vector3 position2 = shadow.position;
			transform.position = new Vector3(x, 0.1f, position2.z);
			Transform transform2 = shadow;
			Vector3 eulerAngles = mTransform.eulerAngles;
			transform2.rotation = Quaternion.Euler(90f, eulerAngles.y, 0f);
		}
	}

	private void ShadowShow(bool show)
	{
		if ((bool)shadowGameObject)
		{
			shadowGameObject.SetActive(show);
		}
	}

	protected void SetBoxEnableOnce(float starttime)
	{
		BoxEnable(enable: false);
		Sequence s = mSeqPool.Get();
		s.AppendInterval(starttime).AppendCallback(delegate
		{
			BoxEnable(enable: true);
		}).AppendInterval(0.03f)
			.AppendCallback(delegate
			{
				BoxEnable(enable: false);
			});
	}

	private void KillSequence()
	{
		mSeqPool.Clear();
	}

	protected virtual void BoxEnable(bool enable)
	{
		bBoxEnable = enable;
		if (boxListCount > 0)
		{
			for (int i = 0; i < boxListCount; i++)
			{
				if ((bool)boxList[i])
				{
					boxList[i].enabled = enable;
				}
			}
		}
		if (sphereList.Length > 0)
		{
			for (int j = 0; j < sphereList.Length; j++)
			{
				if ((bool)sphereList[j])
				{
					sphereList[j].enabled = enable;
				}
			}
		}
		if (capsuleList.Length <= 0)
		{
			return;
		}
		for (int k = 0; k < capsuleList.Length; k++)
		{
			if ((bool)capsuleList[k])
			{
				capsuleList[k].enabled = enable;
			}
		}
	}

	public void SetRadius(float radius)
	{
		if (sphereList.Length > 0)
		{
			for (int i = 0; i < sphereList.Length; i++)
			{
				if ((bool)sphereList[i])
				{
					sphereList[i].radius = radius;
				}
			}
		}
		if (capsuleList.Length <= 0)
		{
			return;
		}
		for (int j = 0; j < capsuleList.Length; j++)
		{
			if ((bool)capsuleList[j])
			{
				capsuleList[j].radius = radius;
			}
		}
	}

	public float[] GetBuffArgs()
	{
		return OnGetBuffArg();
	}

	protected virtual float[] OnGetBuffArg()
	{
		return new float[0];
	}

	public GameObject PlayEffect(int fxId, Vector3 pos, Quaternion rota)
	{
		Fx_fx beanById = LocalModelManager.Instance.Fx_fx.GetBeanById(fxId);
		if (beanById == null)
		{
			SdkManager.Bugly_Report("BulletBase", Utils.FormatString("PlayEffect[{0}] is null.", fxId));
		}
		Transform transform = GameLogic.Release.MapEffect.Get(beanById.Path).transform;
		transform.SetParent(GameNode.m_PoolParent);
		transform.position = pos;
		transform.rotation = rota;
		return transform.gameObject;
	}

	public void SetLastBullet(BulletBase o)
	{
		mLastBullet = o;
		CreateBulletLine();
	}

	private void CreateBulletLine()
	{
		if (m_Entity.m_EntityData.GetBulletLine() && (bool)mLastBullet && mLastBullet.GetInit())
		{
			mBulletLine = new BulletLine();
			mBulletLine.Init(this, mLastBullet);
		}
	}

	private void Init_Model()
	{
		if (!(m_Data.ModelID != string.Empty))
		{
			return;
		}
		Transform exists = mTransform.Find(m_Data.ModelID);
		if (!exists)
		{
			string text = Utils.FormatString("Game/BulletModels/{0}", m_Data.ModelID);
			GameObject gameObject = GameLogic.EffectGet(text);
			GameObject gameObject2 = null;
			if (!gameObject)
			{
				SdkManager.Bugly_Report("BulletBase_Model", Utils.FormatString("Init_Model BulletID:{0} path:{1} model:{2} is not found!!!", m_Data.WeaponID, text, m_Data.ModelID));
				gameObject = ResourceManager.Load<GameObject>("Game/BulletModels/Empty");
				gameObject2 = UnityEngine.Object.Instantiate(gameObject);
			}
			else
			{
				gameObject2 = gameObject;
			}
			if ((bool)gameObject2)
			{
				mBulletModel = gameObject2.transform;
				mBulletModel.name = m_Data.ModelID;
				mBulletModel.SetParentNormal(mTransform);
				mBulletModel.localScale = Vector3.one * m_Data.ModelScale;
				Init_ModelScale();
				Renderer[] componentsInChildren = mBulletModel.GetComponentsInChildren<Renderer>(includeInactive: true);
				int i = 0;
				for (int num = componentsInChildren.Length; i < num; i++)
				{
					componentsInChildren[i].sortingLayerName = "BulletEffect";
				}
			}
		}
		else
		{
			mBulletModel = exists;
			mBulletModel.gameObject.SetActive(value: true);
		}
	}

	protected void BulletModelShow(bool value)
	{
		if ((bool)mBulletModel)
		{
			mBulletModel.gameObject.SetActive(value);
		}
	}

	private void Init_ModelScale()
	{
		TrailRenderer[] componentsInChildren = mBulletModel.GetComponentsInChildren<TrailRenderer>(includeInactive: true);
		if (componentsInChildren != null && componentsInChildren.Length > 0)
		{
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				componentsInChildren[i].startWidth *= m_Data.ModelScale;
				componentsInChildren[i].endWidth *= m_Data.ModelScale;
			}
		}
	}

	public bool ExcuteArrowEject(EntityBase entity)
	{
		if (mArrowEjectCount < 1)
		{
			return false;
		}
		EntityBase entityBase = GameLogic.Release.Entity.FindArrowEject(entity);
		if (entityBase == null)
		{
			return false;
		}
		OnArrowEject(entityBase);
		currentHitRatio *= GameConfig.GetArrowEject();
		if (mArrowEjectCount == mArrowEjectMaxCount)
		{
			mSpeed *= GameConfig.GetArrowEject();
		}
		mBulletTransmit.ArrowEjectAction(GameConfig.GetArrowEject());
		mArrowEjectCount--;
		Transform transform = mTransform;
		Vector3 position = entity.position;
		float x = position.x;
		Vector3 position2 = mTransform.position;
		float y = position2.y;
		Vector3 position3 = entity.position;
		transform.position = new Vector3(x, y, position3.z);
		Vector3 position4 = entityBase.position;
		float x2 = position4.x;
		Vector3 position5 = mTransform.position;
		float x3 = x2 - position5.x;
		Vector3 position6 = entityBase.position;
		float z = position6.z;
		Vector3 position7 = mTransform.position;
		float y2 = z - position7.z;
		bulletAngle = Utils.getAngle(x3, y2);
		UpdateMoveDirection();
		return true;
	}

	protected virtual void OnArrowEject(EntityBase nextentity)
	{
	}

	protected void Catapult()
	{
		if (meshAlphaAction == null)
		{
			SdkManager.Bugly_Report("BulletBase_Skill.Catapult", Utils.FormatString("子弹ID:{0} 弹飞效果 必须有meshAlphaAction", BulletID));
		}
		Vector3 eulerAngles = mTransform.eulerAngles;
		float min = eulerAngles.y + 120f;
		Vector3 eulerAngles2 = mTransform.eulerAngles;
		float angle = UnityEngine.Random.Range(min, eulerAngles2.y + 240f);
		catapult_x = MathDxx.Sin(angle);
		catapult_z = MathDxx.Cos(angle);
		catapult_alpha = 1f;
		catapult_scale = 1f;
		bMoveEnable = false;
		BoxEnable(enable: false);
		Updater.AddUpdate("BulletBase_Skill.Catapult", OnCatapult);
	}

	private void OnCatapult(float delta)
	{
		if (!this || !mTransform)
		{
			Updater.RemoveUpdate("BulletBase_Skill.Catapult", OnCatapult);
			return;
		}
		mTransform.position += new Vector3(catapult_x, 0f, catapult_z) * Updater.delta * 6f;
		catapult_alpha -= Updater.delta * 4f;
		catapult_scale -= Updater.delta * 0.6f;
		mTransform.localScale = Vector3.one * catapult_scale;
		if (catapult_alpha < 0.4f)
		{
			catapult_alpha = 0f;
			DeInit();
		}
		if (meshAlphaAction != null)
		{
			meshAlphaAction(catapult_alpha);
		}
	}

	protected void ExcuteReboundWall(Collider o)
	{
		if (o.gameObject == mHitWall)
		{
			return;
		}
		if (mReboundWallCount < 1)
		{
			PlayHitWallSound();
			if (HitWallAction != null)
			{
				HitWallAction(o);
			}
			return;
		}
		if (m_Entity.IsSelf)
		{
			currentHitRatio *= GameConfig.GetReboundHit();
		}
		if (mReboundWallCount == mReboundWallMaxCount)
		{
			mSpeed *= GameConfig.GetReboundSpeed();
		}
		mReboundWallCount--;
		mHitList.Clear();
		mHitWall = o.gameObject;
		mReboundSphere = null;
		if (sphereList.Length > 0)
		{
			mReboundSphere = sphereList[0];
		}
		float num = bulletAngle = Utils.ExcuteReboundWallSkill(bulletAngle, mTransform.position, mReboundSphere, o);
		if (m_Data.Ballistic == 1)
		{
			mTransform.position = raycastPoint;
		}
		bExcuteReboundWall = true;
		UpdateMoveDirection();
	}

	public void AddCantHit(EntityBase entity)
	{
		mHitList.Add(entity);
	}

	private void OnHitEvent(EntityBase entity, float hittedAngle)
	{
		HitCreate2(entity, hittedAngle);
		HitSputter(entity, hittedAngle);
	}

	private void HitCreate2(EntityBase entity, float hittedAngle)
	{
		if (mBulletTransmit.GetHitCreate2())
		{
			for (int i = 0; i < 2; i++)
			{
				Transform transform = GameLogic.BulletGet(3001).transform;
				transform.SetParent(GameNode.m_PoolParent);
				Transform transform2 = transform;
				Vector3 position = entity.position;
				float x = position.x;
				Vector3 position2 = entity.position;
				transform2.position = new Vector3(x, 1f, position2.z);
				transform.localRotation = Quaternion.Euler(0f, hittedAngle - 90f + (float)i * 180f, 0f);
				transform.localScale = Vector3.one;
				transform.SetParent(GameNode.m_PoolParent);
				HitCreate2_Bullet = transform.GetComponent<BulletBase>();
				HitCreate2_Bullet.Init(m_Entity, 3001);
				HitCreate2_Bullet.AddCantHit(entity);
				HitCreate2_Bullet.SetBulletAttribute(new BulletTransmit(m_Entity, 3001, clear: true));
				HitCreate2_Bullet.mBulletTransmit.AddAttackRatio(mBulletTransmit.mHitCreate2Percent);
			}
		}
	}

	private void HitSputter(EntityBase entity, float hittedAngle)
	{
		if (mBulletTransmit.GetHitSputter())
		{
			HitSputter_o = GameLogic.EffectGet("Effect/Attributes/Bullet_Crescent");
			HitSputter_o.transform.position = entity.position;
			HitSputter_o.transform.localRotation = Quaternion.Euler(0f, hittedAngle, 0f);
			mBulletTransmit.mHitSputter = 0;
			HitSputter_list = GameLogic.Release.Entity.GetSectorEntities(entity, 4f, hittedAngle, 90f, sameteam: true);
			HitSputter_imax = HitSputter_list.Count;
			for (HitSputter_i = 0; HitSputter_i < HitSputter_imax; HitSputter_i++)
			{
				HitStruct attackStruct = mBulletTransmit.GetAttackStruct();
				long beforehit = MathDxx.CeilToInt((float)attackStruct.before_hit * HitSputter_hitratio);
				GameLogic.SendHit_Skill(HitSputter_list[HitSputter_i], beforehit);
			}
		}
	}

	public bool GetLight45()
	{
		return bLight45;
	}

	private void UpdateSpeedRatio()
	{
		if (!GameLogic.IsSameTeam(m_Entity, GameLogic.Self))
		{
			mSpeedRatio = GameLogic.Self.m_EntityData.GetBulletSpeedRatio(this);
		}
	}

	protected void TrailShow(bool show)
	{
		if (mTrailCtrl != null && mBulletTransmit != null && mTrailCtrl.bShow != show)
		{
			mTrailCtrl.bShow = show;
			ThroughTrailShow(show);
			TrailAttrShow(show);
			HeadAttrShow(show);
			if (mBulletTransmit.trailType != 0 || mBulletTransmit.headType != 0)
			{
				show = false;
			}
			mTrailCtrl.TrailShow(show);
			if (OnTrailShowEvent != null)
			{
				OnTrailShowEvent(show);
			}
		}
	}

	protected void ThroughTrailShow(bool show)
	{
		if (mBulletTransmit.GetThroughEnemy())
		{
			OnThroughTrailShow(show);
		}
	}

	protected virtual void OnThroughTrailShow(bool show)
	{
	}

	protected virtual Transform GetTrailAttParent()
	{
		return mTransform;
	}

	protected void TrailAttrShow(bool show)
	{
		if (trailattrobj != null && !show)
		{
			GameLogic.EffectCache(trailattrobj);
			trailattrobj = null;
		}
		else if (trailattrobj == null && show && mBulletTransmit.trailType != 0)
		{
			trailattrobj = GameLogic.EffectGet(EntityData.ElementData[mBulletTransmit.trailType].TrailPath);
			trailattrobj.transform.SetParent(GetTrailAttParent());
			trailattrobj.transform.localPosition = Vector3.zero;
			trailattrobj.transform.localRotation = Quaternion.identity;
			Vector3 localScale = Vector3.one;
			int num = 101;
			Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById(m_Data.WeaponID);
			if (beanById != null)
			{
				num = beanById.Type;
			}
			switch (m_Data.WeaponID)
			{
			case 8001:
				num = 104;
				break;
			case 8002:
				num = 103;
				break;
			case 1001:
				num = 101;
				break;
			}
			switch (num)
			{
			case 101:
				localScale = new Vector3(1f, 1f, 1f);
				break;
			case 103:
				localScale = new Vector3(1.5f, 1f, 0.4f);
				break;
			case 104:
				localScale = new Vector3(2f, 1f, 0.4f);
				break;
			}
			trailattrobj.transform.localScale = localScale;
		}
	}

	protected void HeadAttrShow(bool show)
	{
		if (headattrobj != null && !show)
		{
			GameLogic.EffectCache(headattrobj);
			headattrobj = null;
		}
		else if (headattrobj == null && show && mBulletTransmit.headType != 0)
		{
			headattrobj = GameLogic.EffectGet(EntityData.ElementData[mBulletTransmit.headType].HeadPath);
			headattrobj.transform.SetParent(mTransform);
			headattrobj.transform.localPosition = Vector3.zero;
			headattrobj.transform.localScale = Vector3.one;
			headattrobj.transform.localRotation = Quaternion.identity;
		}
	}

	private void TriggerUpdateList(Collider o)
	{
		if (!mTriggerList.TryGetValue(o.gameObject, out TriggerData value))
		{
			value = new TriggerData();
			value.target = o.gameObject;
			mTriggerList.Add(o.gameObject, value);
		}
		value.currentframe = Time.frameCount;
		value.collider = o;
	}

	private void TriggerListCheck()
	{
		mTriggerListIter = mTriggerList.GetEnumerator();
		while (mTriggerListIter.MoveNext())
		{
			if (mTriggerListIter.Current.Value.GetCanHit())
			{
				TriggerEnter1(mTriggerListIter.Current.Value.collider);
			}
		}
	}

	protected void TriggerTest()
	{
		if (FrameDistance > 0f)
		{
			TriggerTest_Interval = (int)(0.1f / FrameDistance);
		}
		else
		{
			TriggerTest_Interval = 0;
		}
		bExcuteReboundWall = false;
		if (Time.frameCount - TriggerTest_TriggerFrame > TriggerTest_Interval)
		{
			TriggerTest_TriggerFrame = Time.frameCount;
			TriggerTest_Base();
		}
		if (bMoveEnable && !bExcuteReboundWall)
		{
			OnUpdate();
			OnBulletTrack();
		}
	}

	protected void TriggerTest_Base()
	{
		TriggerTest_MoveDis = FrameDistance;
		TriggerTest_BeforeHit = TriggerTest_MoveDis;
		if (CurrentFrameCount == Time.frameCount)
		{
			return;
		}
		mColliders.Clear();
		TriggerTest_CurrentPos = mTransform.position;
		if (mInitFrameCount == 0)
		{
			mInitFrameCount = 1;
			TriggerTest_BeforeHit = 0f;
		}
		else
		{
			TriggerTest_BeforeHit = TriggerTest_MoveDis;
		}
		if (m_Data.Ballistic == 1)
		{
			if (!bMoveEnable)
			{
				return;
			}
			TriggerTest_BeforeHit = TriggerTest_MoveDis;
			TriggerTest_BeforeHit = MathDxx.Clamp(TriggerTest_BeforeHit, 0f, 0.8f);
			TriggerTest_Hits = Physics.RaycastAll(TriggerTest_CurrentPos - moveDirection.normalized * TriggerTest_BeforeHit, moveDirection, FrameDistance + TriggerTest_BeforeHit, m_Data.GetLayer());
		}
		else if (boxListCount > 0)
		{
			if (!boxList[0].enabled)
			{
				return;
			}
			int i = 0;
			for (int num = boxList.Length; i < num; i++)
			{
				Vector3 vector = mTransform.TransformPoint(new Vector3(0f, 0f, -1f) * TriggerTest_BeforeHit + boxList[i].center);
				Vector3 center = vector;
				Vector3 localScale = mTransform.localScale;
				RaycastHit[] array = Physics.BoxCastAll(center, localScale.x * boxList[i].size / 2f, moveDirection, mTransform.rotation, FrameDistance + TriggerTest_BeforeHit, m_Data.GetLayer());
				if (i == 0)
				{
					TriggerTest_Hits = array;
					continue;
				}
				RaycastHit[] array2 = new RaycastHit[TriggerTest_Hits.Length + array.Length];
				for (int j = 0; j < TriggerTest_Hits.Length; j++)
				{
					array2[j] = TriggerTest_Hits[j];
				}
				for (int k = 0; k < array.Length; k++)
				{
					array2[TriggerTest_Hits.Length + k] = array[k];
				}
				TriggerTest_Hits = array2;
			}
		}
		else if (sphereListCount > 0)
		{
			if (!sphereList[0].enabled)
			{
				return;
			}
			Vector3 origin = TriggerTest_CurrentPos - moveDirection * TriggerTest_BeforeHit;
			Vector3 localScale2 = mTransform.localScale;
			TriggerTest_Hits = Physics.SphereCastAll(origin, localScale2.x * sphereList[0].radius, moveDirection, FrameDistance + TriggerTest_BeforeHit, m_Data.GetLayer());
		}
		else if (capsuleListCount > 0)
		{
			if (!capsuleList[0].enabled)
			{
				return;
			}
			Vector3 point = TriggerTest_CurrentPos + TriggerTest_vec * (capsuleList[0].height - 1f) / 2f - moveDirection * TriggerTest_BeforeHit;
			Vector3 point2 = TriggerTest_CurrentPos - TriggerTest_vec * (capsuleList[0].height - 1f) / 2f - moveDirection * TriggerTest_BeforeHit;
			Vector3 localScale3 = mTransform.localScale;
			TriggerTest_Hits = Physics.CapsuleCastAll(point, point2, localScale3.x * capsuleList[0].radius, moveDirection, FrameDistance + TriggerTest_BeforeHit, m_Data.GetLayer());
		}
		if (TriggerTest_Hits == null || TriggerTest_Hits.Length <= 0)
		{
			return;
		}
		minCollider = null;
		mindis = 2.14748365E+09f;
		int l = 0;
		for (int num2 = TriggerTest_Hits.Length; l < num2; l++)
		{
			TriggerTest_Hit = TriggerTest_Hits[l];
			if ((bool)TriggerTest_Hit.collider.gameObject && (((bool)m_Entity && TriggerTest_Hit.collider.gameObject != m_Entity.gameObject) || !m_Entity))
			{
				mColliders.Add(TriggerTest_Hit.collider);
				tempdis = TriggerTest_Hit.distance;
				tempmin = MathDxx.Abs(tempdis - TriggerTest_BeforeHit);
				if (tempmin <= mindis)
				{
					mindis = tempmin;
					minCollider = TriggerTest_Hit.collider;
					raycastPoint = TriggerTest_Hit.point;
				}
			}
		}
		if (m_Data.bMoreHit)
		{
			int m = 0;
			for (int count = mColliders.Count; m < count; m++)
			{
				TriggerUpdateList(mColliders[m]);
			}
			TriggerListCheck();
		}
		else if (minCollider != null)
		{
			TriggerEnter1(minCollider);
		}
		CurrentFrameCount = Time.frameCount;
	}

	private void TriggerEnter1(Collider o)
	{
		GameObject gameObject = null;
		if ((bool)o)
		{
			gameObject = o.gameObject;
		}
		if ((bool)gameObject && (gameObject.layer == LayerManager.Bullet2Map || gameObject.layer == LayerManager.MapOutWall))
		{
			if (m_Data.bThroughWall)
			{
				return;
			}
			if (mBulletTransmit.attribute.ReboundWall.Enable)
			{
				ExcuteReboundWall(o);
				return;
			}
			PlayHitWallSound();
			if (HitWallAction != null)
			{
				HitWallAction(o);
			}
			return;
		}
		if (gameObject.layer == LayerManager.BulletResist && !m_Data.bThroughEntity)
		{
			EntityParentBase component = o.GetComponent<EntityParentBase>();
			if ((bool)component && !component.IsSelf(m_Entity))
			{
				overDistance();
			}
		}
		TriggerExtra(o);
	}

	protected virtual void TriggerExtra(Collider o)
	{
		EntityBase entityBase = null;
		GameObject gameObject = null;
		if ((bool)o)
		{
			gameObject = o.gameObject;
		}
		if ((bool)gameObject && (gameObject.layer == LayerManager.Player || gameObject.layer == LayerManager.Fly))
		{
			entityBase = GameLogic.Release.Entity.GetEntityByChild(gameObject);
			if (entityBase == m_Entity)
			{
				if (OnHitSelf != null)
				{
					OnHitSelf();
				}
				return;
			}
		}
		if (!entityBase || GameLogic.IsSameTeam(entityBase, m_Entity) || mHitList.Contains(entityBase))
		{
			return;
		}
		bool enable = mBulletTransmit.attribute.ArrowEject.Enable;
		if (enable)
		{
			mHitList.Clear();
		}
		if (!m_Data.bMoreHit)
		{
			mHitList.Add(entityBase);
		}
		TriggerExtra_bThroughEnemy = mBulletTransmit.GetThroughEnemy();
		canhitted = entityBase.GetHittedData(this).GetCanHitted();
		if (!canhitted && !m_Data.bThroughEntity && !TriggerExtra_bThroughEnemy)
		{
			HitHero(entityBase, o);
		}
		if (canhitted)
		{
			target_hs = mBulletTransmit.GetAttackStruct();
			TriggerExtra_hit = (int)((float)target_hs.before_hit * currentHitRatio);
			target_hs.before_hit = MathDxx.Clamp(TriggerExtra_hit, TriggerExtra_hit, -1);
			mBulletTransmit.AddDebuffsToTarget(entityBase);
			GameLogic.SendHit_Bullet(entityBase, m_Entity, target_hs.before_hit, target_hs.type, new HitBulletStruct
			{
				bullet = this,
				weapon = m_Data
			});
			TriggerExtra_bEject = false;
			if (enable)
			{
				TriggerExtra_bEject = ExcuteArrowEject(entityBase);
			}
			if (TriggerExtra_bThroughEnemy)
			{
				currentHitRatio *= mBulletTransmit.mThroughRatio;
			}
			else if (!TriggerExtra_bEject)
			{
				HitHero(entityBase, o);
			}
			OnHitEvent(entityBase, bulletAngle);
		}
	}

	private void HitWall(Collider o)
	{
		if (!bMoveEnable)
		{
			return;
		}
		if (m_Data.bCache)
		{
			if (m_Data.Speed > 0f)
			{
				SdkManager.Bugly_Report("BulletBase", Utils.FormatString("飞行子弹{0} 使用了 1击中不回收子弹！！！", m_Data.WeaponID));
			}
		}
		else if (m_Data.Ballistic != 2)
		{
			bMoveEnable = false;
			ShowDeadEffect();
			OnHitWall();
			DeInitDelay(m_Data.DeadDelay);
		}
	}

	private void PlayHitWallSound()
	{
		if (m_Data.HitWallSoundID != 0)
		{
			GameLogic.Hold.Sound.PlayBulletHitWall(m_Data.HitWallSoundID, mTransform.position);
		}
	}

	protected virtual void OnHitWall()
	{
	}

	protected override void UpdateProcess()
	{
		if (GameLogic.Hold.BattleData.Challenge_MonsterHide() && !m_Entity.IsSelf)
		{
			Vector3 position = mTransform.position;
			float x = position.x;
			Vector3 position2 = mTransform.position;
			bool flag = Vector3.Distance(new Vector3(x, 0f, position2.z), GameLogic.Self.position) <= GameLogic.Hold.BattleData.Challenge_MonsterHideRange();
			if (bShowBullet != flag)
			{
				bShowBullet = flag;
				int layer = (!flag) ? LayerManager.Hide : LayerManager.Bullet;
				mTransform.ChangeChildLayer(layer);
			}
		}
		UpdateSpeedRatio();
		UpdateScale();
		TriggerTest();
		ShadowDeal();
		RotateDeal();
		CheckFar();
		if (mCondition != null && mCondition.IsEnd())
		{
			overDistance();
			mCondition = null;
		}
	}

	private void CheckFar()
	{
		if (GameLogic.Release.Game.RoomState != RoomState.Runing)
		{
			return;
		}
		Vector3 position = mTransform.position;
		if (!(MathDxx.Abs(position.x) > 30f))
		{
			Vector3 position2 = mTransform.position;
			if (!(MathDxx.Abs(position2.z) > 30f))
			{
				return;
			}
		}
		overDistance();
	}

	private void BulletParabolaInit()
	{
		if (m_Data.Ballistic == 2)
		{
			Parabola_Curve = LocalModelManager.Instance.Curve_curve.GetCurve(100002);
		}
	}

	protected virtual void OnUpdate()
	{
		switch (m_Data.Ballistic)
		{
		case 0:
			BulletStraight();
			break;
		case 1:
			BulletRayCast();
			break;
		case 2:
			BulletParabola();
			break;
		case 3:
			BulletHorizontal();
			break;
		}
	}

	protected void OnBulletTrack()
	{
		if (mBulletTransmit != null && mBulletTransmit.attribute != null && mBulletTransmit.attribute.ArrowTrack.Enable && (bool)m_Entity)
		{
			if (!bGetTrackTarget)
			{
				bGetTrackTarget = true;
				mTrackTarget = GameLogic.Release.Entity.FindTargetExclude(null);
			}
			if ((bool)mTrackTarget && !mTrackTarget.GetIsDead())
			{
				bulletAngle = Utils.getAngle(mTrackTarget.position - mTransform.position);
				UpdateMoveDirection();
			}
		}
	}

	private void BulletRayCast()
	{
		if (bMoveEnable && !bExcuteReboundWall)
		{
			BulletRayCast_cudris = FrameDistance;
			mTransform.position += moveDirection * BulletRayCast_cudris;
			CurrentDistance += BulletRayCast_cudris;
			if (CurrentDistance >= Distance)
			{
				overDistance();
			}
		}
	}

	protected void BulletStraight()
	{
		if (bMoveEnable && !bExcuteReboundWall)
		{
			OnMove();
			if (CurrentDistance >= Distance)
			{
				overDistance();
			}
		}
	}

	private void BulletParabola()
	{
		if (!bMoveEnable || bExcuteReboundWall)
		{
			return;
		}
		float frameDistance = FrameDistance;
		CurrentDistance += frameDistance;
		float num = CurrentDistance / PosFromStart2Target;
		ref Vector3 parabola_position = ref Parabola_position;
		Vector3 position = mTransform.position;
		parabola_position.x = position.x + moveX * frameDistance;
		Parabola_position.y = Parabola_Curve.Evaluate(CurrentDistance / PosFromStart2Target) * Parabola_MaxHeight;
		ref Vector3 parabola_position2 = ref Parabola_position;
		Vector3 position2 = mTransform.position;
		parabola_position2.z = position2.z + moveY * frameDistance;
		mTransform.position = Parabola_position;
		if (num >= 1f)
		{
			if (m_Data.DeadSoundID != 0)
			{
				GameLogic.Hold.Sound.PlayBulletDead(m_Data.DeadSoundID, mTransform.position);
			}
			ParabolaOver();
		}
	}

	protected virtual void ParabolaOver()
	{
		if (bFlyCantHit)
		{
			BoxEnable(enable: true);
			bMoveEnable = false;
			TrailShow(show: false);
			ShadowShow(show: false);
			FlyOver();
			ShowDeadEffect();
			if ((bool)childMesh)
			{
				childMesh.gameObject.SetActive(value: false);
			}
			Sequence s = mSeqPool.Get();
			s.AppendInterval(0.1f).AppendCallback(delegate
			{
				HitHero(null, null);
			});
		}
		else
		{
			overDistance();
		}
	}

	protected virtual void OnMove()
	{
		float frameDistance = FrameDistance;
		OnMove(frameDistance);
	}

	protected void OnMove(float dis)
	{
		OnMove_vec.x = moveX;
		OnMove_vec.y = 0f;
		OnMove_vec.z = moveY;
		mTransform.position += OnMove_vec * dis;
		CurrentDistance += dis;
	}

	protected void UpdateParabolaArgs()
	{
		if (m_Data.Ballistic == 2)
		{
			beforeframe = Parabola_Curve.keys[0];
			afterframe.time = 0f;
			afterframe.value = StartPositionY / Parabola_MaxHeight;
			afterframe.outTangent = beforeframe.outTangent;
			Parabola_Curve.MoveKey(0, afterframe);
		}
		CreateTime = Updater.AliveTime;
		switch (m_Data.Ballistic)
		{
		case 0:
		case 1:
			LifeTime = mDistance / Speed;
			break;
		case 2:
			LifeTime = PosFromStart2Target / Speed;
			break;
		}
		RemoveTime = CreateTime + LifeTime;
	}

	private void BulletHorizontalInit()
	{
		if (m_Data.Ballistic == 3)
		{
			Horizontal_Curve = LocalModelManager.Instance.Curve_curve.GetCurve(100003);
		}
	}

	private void BulletHorizontal()
	{
		if (bMoveEnable && !bExcuteReboundWall)
		{
			float frameDistance = FrameDistance;
			CurrentDistance += frameDistance;
			float num = CurrentDistance / PosFromStart2Target;
			ref Vector3 horizontal_vec = ref Horizontal_vec;
			Vector3 position = mTransform.position;
			horizontal_vec.x = position.x + moveX * FrameDistance;
			Horizontal_vec.y = Horizontal_Curve.Evaluate(num) * StartPositionY;
			ref Vector3 horizontal_vec2 = ref Horizontal_vec;
			Vector3 position2 = mTransform.position;
			horizontal_vec2.z = position2.z + moveY * FrameDistance;
			mTransform.position = Horizontal_vec;
			if (num >= 1f)
			{
				overDistance();
			}
		}
	}

	private void UpdateScale()
	{
		if ((bool)m_Entity && m_Entity.m_EntityData.GetBulletScale() && !(CurrentDistance > 7f))
		{
			if (CurrentDistance <= 5f)
			{
				bulletscale = (5f - CurrentDistance) / 5f * 0.5f + 1f;
			}
			else
			{
				bulletscale = 1f;
			}
			mTransform.localScale = Vector3.one * bulletscale;
			mBulletTransmit.AddAttackRatio(bulletscale);
		}
	}
}
