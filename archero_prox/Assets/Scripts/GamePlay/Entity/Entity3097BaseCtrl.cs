




using Dxx.Util;
using UnityEngine;

public class Entity3097BaseCtrl : MonoBehaviour
{
	public MeshRenderer mesh;

	public void SetTexture(string value)
	{
		if (!(mesh == null))
		{
			string path = Utils.FormatString("Game/ModelsTexture/{0}", value);
			Texture texture = ResourceManager.Load<Texture>(path);
			if (texture != null)
			{
				mesh.material.SetTexture("_MainTex", texture);
			}
		}
	}
}
