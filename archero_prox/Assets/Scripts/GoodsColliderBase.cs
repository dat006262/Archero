




using UnityEngine;

public class GoodsColliderBase : MonoBehaviour
{
	private GoodsBase m_Goods;

	private BoxCollider box;

	private float lasttime;

	private RaycastHit[] TriggerTest_Hits;

	private RaycastHit TriggerTest_Hit;

	private int TriggerTest_i;

	private int TriggerTest_Max;

	private Vector3 dir = new Vector3(0f, 1f, 0f);

	private void Awake()
	{
		box = GetComponent<BoxCollider>();
	}

	public void SetGoods(GoodsBase good)
	{
		m_Goods = good;
	}

	private void Update()
	{
		if (Time.time != lasttime)
		{
			lasttime = Time.time;
			TriggerTest();
		}
	}

	private void TriggerTest()
	{
		TriggerTest_Hits = Physics.BoxCastAll(base.transform.position, box.size / 2f, dir, base.transform.rotation, 0f, 1 << LayerManager.Player);
		TriggerTest_Max = TriggerTest_Hits.Length;
		for (TriggerTest_i = 0; TriggerTest_i < TriggerTest_Max; TriggerTest_i++)
		{
			m_Goods.ChildTriggerEnter(TriggerTest_Hits[TriggerTest_i].collider.gameObject);
		}
	}
}
