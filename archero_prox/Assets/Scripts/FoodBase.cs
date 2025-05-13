




using Dxx.Util;
using TableTool;
using UnityEngine;

public class FoodBase : PauseObject
{
	public int FoodID;

	protected string ClassName;

	protected int ClassID;

	private int GoodId;

	protected object data;

	protected BoxCollider m_Box;

	protected Equip_equip m_Equip;

	protected GoodsDrop m_GoodsDrop;

	protected Vector3 EndPosition;

	protected bool bAbsorbImme;

	protected Animator Ani_Rotate;

	private GameObject trail;

	private bool bTrailShow;

	protected MeshRenderer[] meshes;

	private float flyStartDelayTime;

	protected float flyTime = 0.17f;

	protected float flyDelayTime = 0.3f;

	protected float flySpeed = 1f;

	private Vector3 mflyspeed;

	private Vector3 mflydir;

	private static AnimationCurve _curve;

	private bool bStartAbsorb;

	private float mAbsorbStartTime;

	private float mAbsoryUpdateTime;

	private EntityHero AbsorbEntity;

	private float flypercent;

	private float lastDis;

	private float tempdis;

	private const float maxspeed = 1f;

	private const float maxdis = 0.7f;

	private float foodAngle;

	protected bool bFlyRotate = true;

	private float startscalez;

	public Goods_food m_Data
	{
		get;
		protected set;
	}

	private static AnimationCurve curve
	{
		get
		{
			if (_curve == null)
			{
				_curve = LocalModelManager.Instance.Curve_curve.GetCurve(100021);
			}
			return _curve;
		}
	}

	private void Awake()
	{
		m_GoodsDrop = base.gameObject.AddComponent<GoodsDrop>();
		m_GoodsDrop.SetDropEnd(DropEnd);
		InitTrail();
		OnAwakeInit();
	}

	protected virtual void OnAwakeInit()
	{
		if (GetType() == typeof(FoodBase))
		{
			ClassID = FoodID;
		}
		else
		{
			ClassName = GetType().ToString();
			ClassName = ClassName.Substring(ClassName.Length - 4, 4);
			ClassID = int.Parse(ClassName);
			FoodID = ClassID;
		}
		m_Data = LocalModelManager.Instance.Goods_food.GetBeanById(ClassID);
		m_Box = GetComponent<BoxCollider>();
		meshes = base.transform.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
		for (int i = 0; i < meshes.Length; i++)
		{
			meshes[i].sortingLayerName = "Hit";
			meshes[i].sortingOrder = ClassID;
		}
		Transform transform = base.transform.Find("child/rotate");
		if ((bool)transform)
		{
			Ani_Rotate = transform.GetComponent<Animator>();
			Transform transform2 = transform.Find("GameObject/mesh");
			if ((bool)transform2 && (bool)Ani_Rotate)
			{
				transform2.parent.localRotation = Quaternion.Euler(0f, GameLogic.Random(0f, 360f), 0f);
			}
		}
	}

	private void Start()
	{
		StartInit();
	}

	protected virtual void StartInit()
	{
	}

	public void Init(object data)
	{
		this.data = data;
		flyStartDelayTime = 0f;
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	protected virtual void OnEnables()
	{
	}

	private void OnEnable()
	{
		if ((bool)Ani_Rotate)
		{
			Ani_Rotate.enabled = true;
		}
		OnEnables();
	}

	private void OnDisable()
	{
		if ((bool)Ani_Rotate)
		{
			Ani_Rotate.enabled = false;
		}
	}

	public object GetData()
	{
		return data;
	}

	private void InitTrail()
	{
		Transform transform = base.transform.Find("child/trail");
		if ((bool)transform)
		{
			trail = transform.gameObject;
			bTrailShow = true;
			TrailShow(show: false);
		}
	}

	private void OnDestroy()
	{
		OnDeInit();
	}

	protected virtual void OnDeInit()
	{
	}

	protected void RotateEnable(bool value)
	{
		if ((bool)Ani_Rotate)
		{
			Ani_Rotate.enabled = value;
		}
	}

	private void TrailShow(bool show)
	{
		if (bTrailShow != show)
		{
			bTrailShow = show;
			if ((bool)trail)
			{
				trail.SetActive(show);
			}
		}
	}

	private void DropEnd()
	{
		OnDropEnd();
	}

	protected virtual void OnDropEnd()
	{
	}

	public bool GetAbsorbImme()
	{
		return bAbsorbImme;
	}

	public void GetGoods(EntityBase entity)
	{
		OnGetGoods(entity);
	}

	protected virtual void OnGetGoods(EntityBase entity)
	{
		m_Data.GetGoods(entity);
	}

	protected virtual void OnGetGoodsEnd()
	{
	}

	public virtual void SetEndPosition(Vector3 startpos, Vector3 endpos)
	{
		EndPosition = endpos;
		m_GoodsDrop.DropInitLast(startpos, endpos);
	}

	public Vector3 GetEndPosition()
	{
		return EndPosition;
	}

	protected virtual void OnAbsorb()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.gameObject.layer == LayerManager.PlayerAbsorb || other.gameObject.layer == LayerManager.PlayerAbsorbImme) && ((other.gameObject.layer == LayerManager.PlayerAbsorb && !bAbsorbImme) || (other.gameObject.layer == LayerManager.PlayerAbsorbImme && bAbsorbImme)))
		{
			OnAbsorb();
			BeAbsorb(GameLogic.Self);
		}
		ChildTriggerEnter(other.gameObject);
	}

	private void OnTriggerExit(Collider other)
	{
		ChildTriggetExit(other.gameObject);
	}

	public virtual void ChildTriggerEnter(GameObject o)
	{
	}

	public virtual void ChildTriggetExit(GameObject o)
	{
	}

	public virtual void SetEquip(Equip_equip equip)
	{
		m_Equip = equip;
	}

	private void BeAbsorb(EntityHero _entity)
	{
		if (!bStartAbsorb && (bool)_entity)
		{
			lastDis = 2.14748365E+09f;
			AbsorbEntity = _entity;
			bStartAbsorb = true;
			startscalez = 0f;
			SetTrailScaleZ(startscalez);
			mAbsorbStartTime = Updater.AliveTime;
			mAbsoryUpdateTime = mAbsorbStartTime;
			OnAbsorbStart();
		}
	}

	protected virtual void OnAbsorbStart()
	{
	}

	private void SetTrailScaleZ(float scalez)
	{
		if ((bool)trail)
		{
			scalez = MathDxx.Clamp01(scalez);
			trail.transform.localScale = new Vector3(1f, 1f, scalez);
		}
	}

	private void Absorbing()
	{
		if (!bStartAbsorb || !AbsorbEntity || AbsorbEntity.GetIsDead())
		{
			return;
		}
		if (ClassID == 1001)
		{
		}
		mAbsoryUpdateTime += Updater.delta;
		if (mAbsoryUpdateTime - mAbsorbStartTime < flyStartDelayTime)
		{
			return;
		}
		if (mAbsoryUpdateTime - mAbsorbStartTime < flyTime + flyStartDelayTime)
		{
			flypercent = (mAbsoryUpdateTime - mAbsorbStartTime - flyStartDelayTime) / flyTime;
			Transform transform = base.transform;
			Vector3 position = base.transform.position;
			float x = position.x;
			float y = curve.Evaluate(flypercent) * 1.7f;
			Vector3 position2 = base.transform.position;
			transform.position = new Vector3(x, y, position2.z);
		}
		else if (mAbsoryUpdateTime - mAbsorbStartTime > flyDelayTime + flyStartDelayTime)
		{
			TrailShow(show: true);
			foodAngle = Utils.getAngle(AbsorbEntity.position - base.transform.position);
			if (bFlyRotate)
			{
				base.transform.localRotation = Quaternion.Euler(0f, foodAngle, 0f);
			}
			mflydir = AbsorbEntity.transform.position - base.transform.position;
			mflyspeed = mflydir.normalized * (mAbsoryUpdateTime - mAbsorbStartTime - flyDelayTime - flyStartDelayTime) * 2.5f * flySpeed;
			if (mflyspeed.magnitude > 1f)
			{
				mflyspeed = mflyspeed.normalized * 1f;
			}
			startscalez += mflyspeed.magnitude / 2f;
			SetTrailScaleZ(startscalez);
			base.transform.position += mflyspeed;
			tempdis = Vector3.Distance(AbsorbEntity.position, base.transform.position);
			if (tempdis <= 0.7f)
			{
				base.transform.position = AbsorbEntity.position;
				TrailShow(show: false);
				AbsorbEntity.AbsorbFoods(this);
				AbsorbEnd();
			}
			lastDis = tempdis;
		}
	}

	protected override void UpdateProcess()
	{
		Absorbing();
	}

	public bool GetAbsorbing()
	{
		return bStartAbsorb;
	}

	public void AbsorbEnd()
	{
		OnGetGoodsEnd();
		bStartAbsorb = false;
		GameLogic.EffectCache(base.gameObject);
	}
}
