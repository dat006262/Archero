




using UnityEngine;

public class BodyShaderGold : BodyShaderBase
{
	private static Color mDiffuseColor = new Color(253f / 255f, 148f / 255f, 0f);

	private static Color mOutColor = new Color(49f / 51f, 49f / 51f, 49f / 51f);

	protected override void OnInit()
	{
		if ((bool)BodyMeshRenderer)
		{
			mMaterial = BodyMeshRenderer.material;
		}
		else if ((bool)BodyMeshRenderer2)
		{
			mMaterial = BodyMeshRenderer2.material;
		}
		if ((bool)mMaterial)
		{
			mMaterial.shader = Shader.Find("Custom/OutLight");
			if ((bool)BodyMeshRenderer)
			{
				BodyMeshRenderer.material = mMaterial;
			}
			else if ((bool)BodyMeshRenderer2)
			{
				BodyMeshRenderer2.material = mMaterial;
			}
		}
		m_Entity.PlayEffect(3300001);
	}

	protected override void OnReturnToDefault()
	{
		if ((bool)BodyMeshRenderer || (bool)BodyMeshRenderer2)
		{
			mMaterial.SetColor(Property_TextureColor, mDiffuseColor);
			mMaterial.SetColor(Property_RimColor, mOutColor);
			mMaterial.SetFloat(Property_Brightness, 0.1f);
			if ((bool)BodyMeshRenderer)
			{
				BodyMeshRenderer.material = mMaterial;
			}
			else if ((bool)BodyMeshRenderer2)
			{
				BodyMeshRenderer2.material = mMaterial;
			}
		}
	}
}
