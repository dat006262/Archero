




using DG.Tweening;
using Dxx.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdHarvestBattleCtrl : MonoBehaviour
{
	public GameObject child;

	public GameObject entitychild;

	private EntityHero m_Entity;

	private SequencePool mSeqPool = new SequencePool();

	private List<Vector3> mPosList = new List<Vector3>
	{
		new Vector3(8f, 0f, 0f),
		new Vector3(8f, 0f, 1.5f),
		new Vector3(8f, 0f, -1.5f),
		new Vector3(8f, 0f, 0f)
	};

	private List<int> entityids = new List<int>
	{
		3115,
		3116,
		3117
	};

	public void Init()
	{
		StartCoroutine("initie");
	}

	public IEnumerator initie()
	{
		yield return null;
		yield return null;
		GameObject o = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("Game/Player/PlayerNode"));
		o.transform.parent = entitychild.transform;
		int playerid = 1001;
		GameLogic.SelfAttribute.Init();
		EntityHero player = o.GetComponent<EntityHero>();
		player.Init(playerid);
		player.transform.localPosition = new Vector3(-3.3f, 0f, 0f);
		m_Entity = player;
		m_Entity.SetCollider(enable: false);
		m_Entity.ShowHP(show: false);
		m_Entity.transform.ChangeChildLayer(LayerManager.Player);
		Sequence s = mSeqPool.Get();
		s.AppendCallback(delegate
		{
			if (GameLogic.Release.Entity.GetActiveEntityCount() < 4)
			{
				int index = GameLogic.Random(0, entityids.Count);
				EntityBase entityBase = GameLogic.Release.MapCreatorCtrl.CreateEntity(new MapCreator.CreateData
				{
					entityid = entityids[index]
				});
				entityBase.transform.parent = entitychild.transform;
				entityBase.transform.localPosition = new Vector3(6.8f, 0f, GameLogic.Random(-1.5f, 2f));
				entityBase.m_Body.SetIsVislble(value: true);
				entityBase.ShowHP(show: false);
			}
		});
		s.AppendInterval(0.5f);
		s.SetLoops(-1);
	}

	public void DeInit()
	{
		StopCoroutine("initie");
		entitychild.transform.DestroyChildren();
		mSeqPool.Clear();
		m_Entity.DeInit();
		GameLogic.Release.Release();
		if (m_Entity != null)
		{
			UnityEngine.Object.Destroy(m_Entity.gameObject);
		}
	}
}
