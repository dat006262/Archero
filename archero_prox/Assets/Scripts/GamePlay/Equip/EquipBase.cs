




using Dxx.Util;
using TableTool;
using UnityEngine;

public class EquipBase : PauseObject
{
	protected string ClassName;

	protected int ClassID;

	private int GoodId;

	protected BoxCollider m_Box;

	protected Equip_equip m_Equip;

	protected GoodsDrop m_GoodsDrop;

	protected Vector3 EndPosition;

	protected bool bAbsorbImme;

	private Animator Ani_Rotate;

	private bool bStartAbsorb;

	[SerializeField]
	private float mAbsorbTime;

	private EntityHero AbsorbEntity;

	public Goods_food m_Data
	{
		get;
		protected set;
	}

	private void Awake()
	{
		ClassName = GetType().ToString();
		ClassName = ClassName.Substring(ClassName.Length - 7, 7);
		ClassID = int.Parse(ClassName);
		m_Data = LocalModelManager.Instance.Goods_food.GetBeanById(ClassID);
		m_Box = GetComponent<BoxCollider>();
		Transform transform = base.transform.Find("child/rotate");
		if ((bool)transform)
		{
			Ani_Rotate = transform.GetComponent<Animator>();
		}
		AwakeInit();
	}

	protected virtual void AwakeInit()
	{
		m_GoodsDrop = base.gameObject.AddComponent<GoodsDrop>();
		m_GoodsDrop.SetDropEnd(DropEnd);
	}

	private void Start()
	{
		StartInit();
	}

	protected virtual void StartInit()
	{
	}

	protected virtual void Init()
	{
	}

	private void OnEnable()
	{
		if ((bool)Ani_Rotate)
		{
			Ani_Rotate.enabled = true;
		}
		Init();
	}

	private void OnDisable()
	{
		if ((bool)Ani_Rotate)
		{
			Ani_Rotate.enabled = false;
		}
	}

	private void OnDestroy()
	{
		OnDeInit();
	}

	protected virtual void OnDeInit()
	{
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
		m_Data.GetGoods(entity);
		OnGetGoods();
	}

	protected virtual void OnGetGoods()
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
		if ((other.gameObject.layer == LayerManager.PlayerAbsorb && !bAbsorbImme) || (other.gameObject.layer == LayerManager.PlayerAbsorbImme && bAbsorbImme))
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
		if (!bStartAbsorb && (bool)_entity && !_entity.GetIsDead())
		{
			AbsorbEntity = _entity;
			bStartAbsorb = true;
			mAbsorbTime = Updater.AliveTime;
		}
	}

	private void Absorbing()
	{
		if (!bStartAbsorb)
		{
			return;
		}
		if (!AbsorbEntity || AbsorbEntity.GetIsDead())
		{
			bStartAbsorb = false;
		}
		else if (Updater.AliveTime - mAbsorbTime < 0.17f)
		{
			Transform transform = base.transform;
			Vector3 position = base.transform.position;
			float x = position.x;
			Vector3 position2 = base.transform.position;
			float y = position2.y + Updater.delta * 10f;
			Vector3 position3 = base.transform.position;
			transform.position = new Vector3(x, y, position3.z);
		}
		else if (Updater.AliveTime - mAbsorbTime > 0.3f)
		{
			Vector3 vector = (AbsorbEntity.transform.position - base.transform.position).normalized * (Updater.AliveTime - mAbsorbTime - 0.3f) * 2.5f;
			if (vector.magnitude > 2.5f)
			{
				vector = vector.normalized * 2.5f;
			}
			base.transform.position += vector;
			if (Vector3.Distance(AbsorbEntity.position, base.transform.position) <= 1f)
			{
				base.transform.position = AbsorbEntity.position;
				AbsorbEntity.AbsorbEquips(this);
				AbsorbEnd();
			}
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
		bStartAbsorb = false;
		GameLogic.EffectCache(base.gameObject);
	}
}
