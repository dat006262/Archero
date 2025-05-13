




using UnityEngine;

public class GoodsCreateBase : MonoBehaviour
{
	private void Awake()
	{
		OnAwake();
	}

	protected virtual void OnAwake()
	{
	}

	private void Start()
	{
		OnStart();
	}

	protected virtual void OnStart()
	{
	}

	public void Init()
	{
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	private void Update()
	{
		OnUpdate();
	}

	protected virtual void OnUpdate()
	{
	}

	private void OnTriggerEnter(Collider o)
	{
		TriggerEnter(o);
	}

	protected virtual void TriggerEnter(Collider o)
	{
	}

	protected void Cache()
	{
		GameLogic.Release.GoodsCreate.Cache(base.gameObject);
	}
}
