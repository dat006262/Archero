




using Dxx.Util;
using TableTool;
using UnityEngine;

public class SkillAloneBase : AttributeCtrlBase
{
	protected EntityBabyBase CreateBaby(int babyID)
	{
		if (!m_Entity)
		{
			return null;
		}
		GameObject gameObject = Object.Instantiate(ResourceManager.Load<GameObject>(Utils.FormatString("Game/Baby/BabyNode{0}", babyID)));
		gameObject.transform.parent = GameNode.m_Battle.transform;
		Vector3 position = m_Entity.position;
		float x = position.x + Random.Range(-2f, 2f);
		Vector3 position2 = m_Entity.position;
		float z = position2.z + Random.Range(-2f, 2f);
		gameObject.transform.localPosition = new Vector3(x, 0f, z);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		return gameObject.GetComponent<EntityBabyBase>();
	}

	public static long GetAttack(EntityBase entity, string att)
	{
		Goods_goods.GoodData goodData = Goods_goods.GetGoodData(att);
		return GetAttack(entity, goodData);
	}

	private static long GetAttack(EntityBase entity, Goods_goods.GoodData data)
	{
		long result = 0L;
		switch (data.goodType)
		{
		case "Attack":
			result = data.value;
			break;
		case "Attack%":
			if ((bool)entity)
			{
				result = (long)((float)(entity.m_EntityData.GetAttackBase() * data.value) / 100f);
			}
			break;
		}
		return result;
	}
}
