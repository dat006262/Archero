




using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SkillAloneAttrGoodBase : MonoBehaviour
{
	public class EffectClass
	{
		public GameObject o;

		public SkillAloneAttrGoodBase[] list;

		public bool bGotoRoomDeInit;

		public void OnGotoNextRoom(RoomGenerateBase.Room room)
		{
			int i = 0;
			for (int num = list.Length; i < num; i++)
			{
				list[i].OntoNextRoom(room);
			}
		}

		public void Init(EntityBase entity, params float[] args)
		{
			int i = 0;
			for (int num = list.Length; i < num; i++)
			{
				list[i].Init(entity, args);
			}
		}

		public void DeInit()
		{
			if (list != null)
			{
				int i = 0;
				for (int num = list.Length; i < num; i++)
				{
					list[i].DeInit();
				}
			}
		}
	}

	private static Dictionary<GameObject, EffectClass> mList = new Dictionary<GameObject, EffectClass>();

	private static List<GameObject> mRemoveList = new List<GameObject>();

	protected EntityBase m_Entity;

	protected float[] args;

	public bool bGotoRoomDeInit = true;

	private ParticleSystem[] particles;

	private MeshRenderer[] meshes;

	private Sequence seq;

	public static SkillAloneAttrGoodBase[] Add(EntityBase entity, GameObject o, bool bGotoRoomDeInit, params float[] args)
	{
		if (o == null)
		{
			return null;
		}
		if (mList.ContainsKey(o))
		{
			return null;
		}
		EffectClass effectClass = new EffectClass();
		effectClass.o = o;
		effectClass.list = o.GetComponentsInChildren<SkillAloneAttrGoodBase>(includeInactive: true);
		effectClass.bGotoRoomDeInit = bGotoRoomDeInit;
		effectClass.Init(entity, args);
		mList.Add(o, effectClass);
		return effectClass.list;
	}

	public static bool Remove(GameObject o)
	{
		if (o == null)
		{
			return false;
		}
		if (mList.TryGetValue(o, out EffectClass value))
		{
			value.DeInit();
			return true;
		}
		return false;
	}

	public static void OnGotoNextRoom(RoomGenerateBase.Room room)
	{
		mRemoveList.Clear();
		Dictionary<GameObject, EffectClass>.Enumerator enumerator = mList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.bGotoRoomDeInit)
			{
				mRemoveList.Add(enumerator.Current.Key);
			}
			else
			{
				enumerator.Current.Value.OnGotoNextRoom(room);
			}
		}
		int i = 0;
		for (int count = mRemoveList.Count; i < count; i++)
		{
			GameObject o = mRemoveList[i];
			Remove(o);
			GameLogic.EffectCache(o);
		}
	}

	public static void InitData()
	{
		mList.Clear();
	}

	public static void DeInitData()
	{
		Dictionary<GameObject, EffectClass>.Enumerator enumerator = mList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			GameLogic.EffectCache(enumerator.Current.Key);
		}
		mList.Clear();
	}

	public void Init(EntityBase entity, params float[] args)
	{
		m_Entity = entity;
		this.args = args;
		particles = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
		meshes = GetComponentsInChildren<MeshRenderer>(includeInactive: true);
		int i = 0;
		for (int num = meshes.Length; i < num; i++)
		{
			meshes[i].sortingLayerName = "Hit";
		}
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	public void DeInit()
	{
		KillSequence();
		OnDeInit();
	}

	protected virtual void OnDeInit()
	{
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	public void OntoNextRoom(RoomGenerateBase.Room room)
	{
		KillSequence();
		base.gameObject.SetActive(value: false);
		seq = DOTween.Sequence().AppendInterval(0.2f).AppendCallback(delegate
		{
			base.gameObject.SetActive(value: true);
		});
		if (particles != null)
		{
			int i = 0;
			for (int num = particles.Length; i < num; i++)
			{
				particles[i].Clear();
			}
		}
	}

	private void OnTriggerEnter(Collider o)
	{
		if (o.gameObject.layer == LayerManager.Player)
		{
			EntityBase entityByChild = GameLogic.Release.Entity.GetEntityByChild(o.gameObject);
			if (!GameLogic.IsSameTeam(entityByChild, m_Entity))
			{
				TriggerEnter(entityByChild);
			}
		}
	}

	protected virtual void TriggerEnter(EntityBase entity)
	{
		Attack(entity, 1f);
	}

	protected void Attack(EntityBase entity, float hitratio)
	{
		int attack = 20;
		if (m_Entity.m_Weapon != null)
		{
			attack = m_Entity.m_Weapon.m_Data.Attack;
		}
		long beforehit = (long)((float)(-m_Entity.m_EntityData.GetAttack(attack)) * hitratio);
		GameLogic.SendHit_Skill(entity, beforehit);
	}

	protected void AttackByArg(EntityBase entity)
	{
		float hitratio = 1f;
		if (args != null && args.Length > 0)
		{
			hitratio = args[0];
		}
		Attack(entity, hitratio);
	}
}
