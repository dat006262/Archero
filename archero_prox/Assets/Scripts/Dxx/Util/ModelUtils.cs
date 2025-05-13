




using UnityEngine;

namespace Dxx.Util
{
	public class ModelUtils
	{
		public static GameObject GenerateModel(string bodyPath, string weaponPath)
		{
			GameObject gameObject = Object.Instantiate(ResourceManager.Load<GameObject>(bodyPath));
			if (!string.IsNullOrEmpty(weaponPath))
			{
				GameObject gameObject2 = Object.Instantiate(ResourceManager.Load<GameObject>(weaponPath));
				if (gameObject2 != null)
				{
					BodyMask component = gameObject.GetComponent<BodyMask>();
					if (component != null && component.LeftWeapon != null)
					{
						gameObject2.SetParentNormal(component.LeftWeapon);
						MeshRenderer componentInChildren = gameObject2.transform.GetComponentInChildren<MeshRenderer>();
						if ((bool)componentInChildren && (bool)componentInChildren.material && componentInChildren.material.HasProperty("_Factor"))
						{
							componentInChildren.material.SetFloat("_Factor", 0f);
						}
					}
				}
			}
			return gameObject;
		}
	}
}
