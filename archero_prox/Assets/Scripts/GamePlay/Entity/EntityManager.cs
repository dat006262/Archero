




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
	public class DivideTransfer
	{
		public string divedeid;

		public int charid;

		public EntityType entitytype;
	}

	public class DivideData
	{
		public string DivideID;

		public int charid;

		public EntityType entitytype;

		public int count;

		public List<int> goodlist;
	}

	public const float Ground_Bottom_Y = -1f;

	private EntityHero _Self;

	private List<EntityBase> m_HeroList = new List<EntityBase>();

	private List<EntityBase> m_EntityList = new List<EntityBase>();

	private Dictionary<GameObject, EntityBase> mObj2EntityList = new Dictionary<GameObject, EntityBase>();

	private List<EntityTowerBase> m_TowerList = new List<EntityTowerBase>();

	private float find_minDis;

	private float find_minDis1;

	private EntityBase find_target;

	private EntityBase find_mintarget;

	private EntityBase find_temp;

	private int find_i;

	private int find_imax;

	private List<EntityBase> findcanattacklist = new List<EntityBase>();

	private List<EntityBase> mCanHitList = new List<EntityBase>();

	private List<EntityBase> mRangeList = new List<EntityBase>();

	private const float ArrowEjectDistance = 7.5f;

	private EntityBase eject_temp;

	private EntityBase entitybase_temp;

	private List<EntityBase> rounds_list = new List<EntityBase>();

	private EntityBase rounds_temp;

	private List<EntityBase> round_list = new List<EntityBase>();

	private EntityBase round_temp;

	private float near_min;

	private float near_dis;

	private EntityBase near_entity;

	private EntityBase near_entitytemp;

	private EntityBase entity_random;

	private EntityBase entity_randomtemp;

	private List<EntityBase> Sector_list = new List<EntityBase>();

	private EntityBase Sector_e;

	private EntityBase entity_child;

	private List<EntityBabyBase> m_BabyList = new List<EntityBabyBase>();

	private Dictionary<string, DivideData> mDivideList = new Dictionary<string, DivideData>();

	private List<EntityPartBodyBase> m_PartBodyList = new List<EntityPartBodyBase>();

	public EntityHero Self => _Self;

	public void SetSelf(EntityHero self)
	{
		_Self = self;
		AddHero(self);
	}

	public void AddHero(EntityBase hero)
	{
		m_HeroList.Add(hero);
	}

	public void Add(EntityBase entity)
	{
		if (!m_EntityList.Contains(entity))
		{
			m_EntityList.Add(entity);
		}
	}

	public void Remove(EntityBase entity)
	{
		if (m_EntityList.Contains(entity))
		{
			entity.DeInit();
		}
		else if (entity == _Self)
		{
			_Self.DeInit();
		}
	}

	public void RemoveLogic(EntityBase entity)
	{
		if (m_EntityList.Contains(entity))
		{
			m_EntityList.Remove(entity);
		}
	}

	public void AddTower(EntityTowerBase tower)
	{
		if (!m_TowerList.Contains(tower))
		{
			m_TowerList.Add(tower);
		}
		else
		{
			SdkManager.Bugly_Report("EntityManager.cs", Utils.FormatString("AddTower is already contains {0}", tower.name));
		}
	}

	public void RemoveTower(EntityTowerBase tower)
	{
		if (m_TowerList.Contains(tower))
		{
			m_TowerList.Remove(tower);
			tower.DeInit();
		}
	}

	private List<EntityBase> GetTargetList(EntityBase self, bool issameteam)
	{
		bool flag = GameLogic.IsSameTeam(self, GameLogic.Self);
		if (flag == issameteam)
		{
			return m_HeroList;
		}
		return m_EntityList;
	}

	public EntityBase FindTargetExclude(EntityBase exclude)
	{
		find_minDis = 2.14748365E+09f;
		find_minDis1 = 2.14748365E+09f;
		find_target = null;
		find_mintarget = null;
		find_i = 0;
		for (find_imax = m_EntityList.Count; find_i < find_imax; find_i++)
		{
			find_temp = m_EntityList[find_i];
			if ((bool)find_temp && find_temp.gameObject.activeInHierarchy && find_temp != exclude && !find_temp.GetIsDead() && find_temp.GetIsInCamera() && !GameLogic.IsSameTeam(find_temp, GameLogic.Self) && find_temp.GetColliderEnable())
			{
				float num = Vector3.Distance(find_temp.transform.position, Self.position);
				if (num < find_minDis && GameLogic.GetCanHit(Self, find_temp))
				{
					find_target = find_temp;
					find_minDis = num;
				}
				if (num < find_minDis1)
				{
					find_mintarget = find_temp;
					find_minDis1 = num;
				}
			}
		}
		return find_target;
	}

	public EntityBase FindCanAttackRandom(EntityBase self)
	{
		List<EntityBase> targetList = GetTargetList(self, issameteam: false);
		findcanattacklist.Clear();
		find_minDis = 2.14748365E+09f;
		find_minDis1 = 2.14748365E+09f;
		find_target = null;
		find_mintarget = null;
		find_i = 0;
		for (find_imax = targetList.Count; find_i < find_imax; find_i++)
		{
			find_temp = targetList[find_i];
			if ((bool)find_temp && find_temp.gameObject.activeInHierarchy && !find_temp.GetIsDead() && find_temp.GetIsInCamera() && find_temp.GetColliderEnable() && find_temp.GetMeshShow())
			{
				findcanattacklist.Add(find_temp);
			}
		}
		if (findcanattacklist.Count == 0)
		{
			return null;
		}
		int index = GameLogic.Random(0, findcanattacklist.Count);
		return findcanattacklist[index];
	}

	public EntityBase FindTargetInCamera()
	{
		find_minDis = 2.14748365E+09f;
		find_minDis1 = 2.14748365E+09f;
		find_target = null;
		find_mintarget = null;
		find_i = 0;
		for (find_imax = m_EntityList.Count; find_i < find_imax; find_i++)
		{
			find_temp = m_EntityList[find_i];
			if ((bool)find_temp && find_temp.gameObject.activeInHierarchy && !find_temp.GetIsDead() && find_temp.GetIsInCamera() && !GameLogic.IsSameTeam(find_temp, GameLogic.Self) && find_temp.GetColliderEnable() && find_temp.GetMeshShow())
			{
				Vector3 position = find_temp.position;
				if (position.y >= -1f)
				{
					float num = Vector3.Distance(find_temp.position, Self.position);
					if (num < find_minDis && GameLogic.GetCanHit(Self, find_temp))
					{
						find_target = find_temp;
						find_minDis = num;
					}
					if (num < find_minDis1)
					{
						find_mintarget = find_temp;
						find_minDis1 = num;
					}
				}
			}
		}
		if (find_target != null)
		{
		}
		if (find_target == null && find_mintarget != null)
		{
			Vector3 position2 = find_mintarget.position;
			if (position2.y >= -1f)
			{
				return find_mintarget;
			}
		}
		return find_target;
	}

	public EntityBase FindArrowEject(EntityBase entity)
	{
		mCanHitList.Clear();
		mRangeList.Clear();
		int i = 0;
		for (int count = m_EntityList.Count; i < count; i++)
		{
			eject_temp = m_EntityList[i];
			float num = Vector3.Distance(eject_temp.transform.position, entity.position);
			if ((bool)eject_temp && eject_temp != entity && eject_temp.gameObject.activeInHierarchy && eject_temp.Type == entity.Type && !eject_temp.GetIsDead() && eject_temp.GetIsInCamera() && num < 7.5f)
			{
				if (GameLogic.GetCanHit(entity, eject_temp))
				{
					mCanHitList.Add(eject_temp);
				}
				mRangeList.Add(eject_temp);
			}
		}
		if (mCanHitList.Count > 0)
		{
			return mCanHitList[Random.Range(0, mCanHitList.Count)];
		}
		if (mRangeList.Count > 0)
		{
			return mRangeList[Random.Range(0, mRangeList.Count)];
		}
		return null;
	}

	public int GetEntityCount()
	{
		return m_EntityList.Count;
	}

	public int GetActiveEntityCount()
	{
		int num = 0;
		int i = 0;
		for (int count = m_EntityList.Count; i < count; i++)
		{
			if ((bool)m_EntityList[i] && m_EntityList[i].gameObject.activeInHierarchy && !m_EntityList[i].GetIsDead())
			{
				num++;
			}
		}
		return num;
	}

	public List<EntityBase> GetEntities()
	{
		return m_EntityList;
	}

	public EntityBase GetEntityBase(GameObject o)
	{
		int i = 0;
		for (int count = m_EntityList.Count; i < count; i++)
		{
			entitybase_temp = m_EntityList[i];
			if (entitybase_temp.gameObject == o)
			{
				return entitybase_temp;
			}
		}
		return null;
	}

	public List<EntityBase> GetRoundEntities(EntityBase entity, float range, bool haveself)
	{
		rounds_list = new List<EntityBase>();
		int i = 0;
		for (int count = m_EntityList.Count; i < count; i++)
		{
			rounds_temp = m_EntityList[i];
			if ((bool)rounds_temp && rounds_temp.gameObject.activeInHierarchy && ((!haveself && rounds_temp != entity) || haveself) && !rounds_temp.GetIsDead() && Vector3.Distance(rounds_temp.position, entity.position) < range)
			{
				rounds_list.Add(rounds_temp);
			}
		}
		return rounds_list;
	}

	public List<EntityBase> GetRoundSelfEntities(EntityBase self, float range, bool sameteam)
	{
		round_list.Clear();
		int i = 0;
		for (int count = m_EntityList.Count; i < count; i++)
		{
			round_temp = m_EntityList[i];
			if ((bool)round_temp && (bool)round_temp.gameObject && round_temp != self && round_temp.gameObject.activeInHierarchy && !round_temp.GetIsDead() && GameLogic.IsSameTeam(round_temp, self) == sameteam && Vector3.Distance(round_temp.position, self.position) < range)
			{
				round_list.Add(round_temp);
			}
		}
		return round_list;
	}

	public EntityBase GetNearEntity(EntityBase self, float range, bool sameteam)
	{
		List<EntityBase> targetList = GetTargetList(self, sameteam);
		near_min = 2.14748365E+09f;
		near_entity = null;
		int i = 0;
		for (int count = targetList.Count; i < count; i++)
		{
			near_entitytemp = targetList[i];
			near_dis = Vector3.Distance(near_entitytemp.position, self.position);
			if ((bool)near_entitytemp && near_entitytemp != self && near_entitytemp.gameObject.activeInHierarchy && !near_entitytemp.GetIsDead() && near_dis < range && near_dis < near_min)
			{
				near_entity = near_entitytemp;
				near_min = near_dis;
			}
		}
		return near_entity;
	}

	public EntityBase GetNearTarget(EntityBase self)
	{
		List<EntityBase> targetList = GetTargetList(self, issameteam: false);
		near_min = 2.14748365E+09f;
		near_entity = null;
		int i = 0;
		for (int count = targetList.Count; i < count; i++)
		{
			near_entitytemp = targetList[i];
			near_dis = Vector3.Distance(near_entitytemp.position, self.position);
			if ((bool)near_entitytemp && near_entitytemp != self && near_entitytemp.gameObject.activeInHierarchy && near_dis < near_min && !near_entitytemp.GetIsDead())
			{
				Vector3 position = near_entitytemp.position;
				if (position.y >= 0f && near_entitytemp.GetMeshShow())
				{
					near_entity = near_entitytemp;
					near_min = near_dis;
				}
			}
		}
		return near_entity;
	}

	public EntityBase GetNearEntity(BulletBase bullet, bool sameteam)
	{
		List<EntityBase> targetList = GetTargetList(bullet.m_Entity, sameteam);
		near_min = 2.14748365E+09f;
		near_entity = null;
		int i = 0;
		for (int count = targetList.Count; i < count; i++)
		{
			near_entitytemp = targetList[i];
			near_dis = Vector3.Distance(near_entitytemp.position, bullet.transform.position);
			if ((bool)near_entitytemp && near_entitytemp.gameObject.activeInHierarchy && !near_entitytemp.GetIsDead() && near_dis < near_min)
			{
				near_entity = near_entitytemp;
				near_min = near_dis;
			}
		}
		return near_entity;
	}

	public EntityBase GetRandomEntity(EntityBase self, float range, bool sameteam)
	{
		List<EntityBase> targetList = GetTargetList(self, sameteam);
		entity_random = null;
		int num = GameLogic.Random(0, targetList.Count);
		int i = 0;
		for (int count = targetList.Count; i < count; i++)
		{
			entity_randomtemp = targetList[(num + i) % count];
			float num2 = Vector3.Distance(entity_randomtemp.position, self.position);
			if ((bool)entity_randomtemp && entity_randomtemp != self && entity_randomtemp.gameObject.activeInHierarchy && !entity_randomtemp.GetIsDead() && num2 < range)
			{
				entity_random = entity_randomtemp;
				break;
			}
		}
		return entity_random;
	}

	public List<EntityBase> GetSectorEntities(EntityBase self, float range, float middleangle, float offsetangle, bool sameteam)
	{
		List<EntityBase> targetList = GetTargetList(self, sameteam);
		Sector_list.Clear();
		int i = 0;
		for (int count = targetList.Count; i < count; i++)
		{
			Sector_e = targetList[i];
			float num = Vector3.Distance(Sector_e.position, self.position);
			if ((bool)Sector_e && Sector_e != self && Sector_e.gameObject.activeInHierarchy && !Sector_e.GetIsDead() && num < range)
			{
				Vector3 position = Sector_e.position;
				float x = position.x;
				Vector3 position2 = self.position;
				float x2 = x - position2.x;
				Vector3 position3 = Sector_e.position;
				float z = position3.z;
				Vector3 position4 = self.position;
				float angle = Utils.getAngle(x2, z - position4.z);
				if (MathDxx.Abs(angle - middleangle) <= offsetangle || MathDxx.Abs(angle - middleangle + 360f) <= offsetangle || MathDxx.Abs(angle - middleangle - 360f) <= offsetangle)
				{
					Sector_list.Add(Sector_e);
				}
			}
		}
		return Sector_list;
	}

	public EntityBase GetEntityByChild(GameObject o)
	{
		if (mObj2EntityList.TryGetValue(o, out entity_child))
		{
			return entity_child;
		}
		entity_child = o.GetComponent<EntityBase>();
		mObj2EntityList.Add(o, entity_child);
		return entity_child;
	}

	public void MonstersClear()
	{
		for (int num = m_EntityList.Count - 1; num >= 0; num--)
		{
			m_EntityList[num].DeInit();
		}
		for (int num2 = m_TowerList.Count - 1; num2 >= 0; num2--)
		{
			RemoveTower(m_TowerList[num2]);
		}
		m_TowerList.Clear();
		m_EntityList.Clear();
	}

	public void DeInit()
	{
		MonstersClear();
		if ((bool)Self)
		{
			Self.DeInit();
			UnityEngine.Object.Destroy(Self.gameObject);
		}
		m_HeroList.Clear();
		mCanHitList.Clear();
		mRangeList.Clear();
		Sector_list.Clear();
		mObj2EntityList.Clear();
		round_list.Clear();
		rounds_list.Clear();
		DeInitBabies();
		DeInitPartBodies();
	}

	public bool IsSelfObject(GameObject o)
	{
		return o == Self.gameObject;
	}

	public void SetBaby(EntityBabyBase baby)
	{
		m_BabyList.Add(baby);
	}

	private void DeInitBabies()
	{
		for (int num = m_BabyList.Count - 1; num >= 0; num--)
		{
			m_BabyList[num].DeInit();
		}
		m_BabyList.Clear();
	}

	public void RemoveBaby(EntityBabyBase baby)
	{
		baby.DeInit();
		m_BabyList.Remove(baby);
	}

	public void AddDivide(string divideid, DivideTransfer transfer)
	{
		if (mDivideList.TryGetValue(divideid, out DivideData value))
		{
			value.count++;
			return;
		}
		value = new DivideData();
		value.DivideID = divideid;
		value.count = 1;
		value.charid = transfer.charid;
		value.entitytype = transfer.entitytype;
		mDivideList.Add(divideid, value);
	}

	public void RemoveDivide(string divideid)
	{
		if (mDivideList.TryGetValue(divideid, out DivideData value))
		{
			value.count--;
		}
	}

	public bool GetDivideDead(string divideid, out List<int> goodlist, out EntityType entitytype)
	{
		if (mDivideList.TryGetValue(divideid, out DivideData value))
		{
			if (value.count == 0)
			{
				goodlist = value.goodlist;
				mDivideList.Remove(divideid);
				entitytype = value.entitytype;
				return true;
			}
			goodlist = null;
			entitytype = EntityType.Invalid;
			return false;
		}
		goodlist = null;
		entitytype = EntityType.Invalid;
		return true;
	}

	public void SetPartBody(EntityPartBodyBase partbody)
	{
		m_PartBodyList.Add(partbody);
	}

	private void DeInitPartBodies()
	{
		for (int num = m_PartBodyList.Count - 1; num >= 0; num--)
		{
			m_PartBodyList[num].DeInit();
		}
		m_PartBodyList.Clear();
	}

	public void RemovePartBody(EntityPartBodyBase partbody, bool gotonextroom = false)
	{
		partbody.bGotoRoomRemove = gotonextroom;
		partbody.DeInit();
		m_PartBodyList.Remove(partbody);
	}
}
