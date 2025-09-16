




using UnityEngine;

public class BattleResourceCreator : CInstance<BattleResourceCreator>
{
	private GameObject get_gameobject(string path)
	{
		return Object.Instantiate(ResourceManager.Load<GameObject>(path));
	}

	public T Get<T>(string path) where T : Component
	{
		GameObject gameObject = get_gameobject(path);
		return gameObject.GetComponent<T>();
	}

	public Entity3097BaseCtrl Get3097Base(Transform parent = null)
	{
		Entity3097BaseCtrl entity3097BaseCtrl = Get<Entity3097BaseCtrl>("Effect/Monster/3097_base");
		if (parent != null)
		{
			entity3097BaseCtrl.transform.SetParentNormal(parent);
			Transform transform = entity3097BaseCtrl.transform;
			Vector3 position = entity3097BaseCtrl.transform.position;
			float x = position.x;
			Vector3 position2 = entity3097BaseCtrl.transform.position;
			transform.position = new Vector3(x, 0f, position2.z);
		}
		return entity3097BaseCtrl;
	}

	public GameObject GetFootCircle(Transform parent = null)
	{
		GameObject gameObject = get_gameobject("Game/Player/FootCircle");
		if (parent != null)
		{
			gameObject.SetParentNormal(parent);
		}
		return gameObject;
	}

	public GameObject GetHead_Pet(Transform parent = null)
	{
		GameObject gameObject = get_gameobject("Game/UI/Head_Pet");
		if (parent != null)
		{
			gameObject.SetParentNormal(parent);
		}
		return gameObject;
	}

	public GameObject GetFoodEquipEffect_Equip(Transform parent = null)
	{
		GameObject gameObject = get_gameobject("Game/Food/effect_equip");
		if (parent != null)
		{
			gameObject.SetParentNormal(parent);
		}
		return gameObject;
	}

	public GameObject GetFoodEquipEffect_EquipExp(Transform parent = null)
	{
		GameObject gameObject = get_gameobject("Game/Food/effect_equipexp");
		if (parent != null)
		{
			gameObject.SetParentNormal(parent);
		}
		return gameObject;
	}
}
