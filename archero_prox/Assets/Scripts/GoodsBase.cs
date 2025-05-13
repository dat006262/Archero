




using TableTool;
using UnityEngine;

public class GoodsBase : PauseObject
{
	protected string ClassName;

	protected int ClassID;

	private int GoodId;

	private TMXGoodsData GoodData;

	protected BoxCollider m_Box;

	protected Equip_equip m_Equip;

	protected GoodsDrop m_GoodsDrop;

	protected Vector3 EndPosition;

	protected bool bAbsorbImme;

	private Animator Ani_Rotate;

	public Goods_goods m_Data
	{
		get;
		protected set;
	}

	private void Awake()
	{
		ClassName = GetType().ToString();
		ClassName = ClassName.Substring(ClassName.Length - 4, 4);
		ClassID = int.Parse(ClassName);
		m_Data = LocalModelManager.Instance.Goods_goods.GetBeanById(ClassID);
		m_Box = GetComponent<BoxCollider>();
		GoodData = new TMXGoodsData();
		GoodData.SetGoodsId(ClassID);
		GoodData.Init(m_Data.GoodsType);
		Transform transform = base.transform.Find("child/rotate");
		if ((bool)transform)
		{
			Ani_Rotate = transform.GetComponent<Animator>();
		}
		AwakeInit();
	}

	protected virtual void AwakeInit()
	{
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
		Init();
	}

	private void OnDisable()
	{
		OnDeInit();
	}

	protected virtual void OnDeInit()
	{
	}

	public bool GetAbsorbImme()
	{
		return bAbsorbImme;
	}

	protected virtual void OnAbsorb()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
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
}
