




using UnityEngine;

public class MeshLayerHelper : MonoBehaviour
{
	public string LayerName = "Hit";

	public int order;

	public bool changechild;

	private void Start()
	{
		if (!changechild)
		{
			MeshRenderer component = GetComponent<MeshRenderer>();
			if ((bool)component)
			{
				component.sortingLayerName = LayerName;
			}
		}
		else
		{
			MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>(includeInactive: true);
			if (componentsInChildren != null)
			{
				int i = 0;
				for (int num = componentsInChildren.Length; i < num; i++)
				{
					if (componentsInChildren[i] != null)
					{
						componentsInChildren[i].sortingLayerName = LayerName;
						componentsInChildren[i].sortingOrder = order;
					}
				}
			}
		}
		UnityEngine.Object.Destroy(this);
	}
}
