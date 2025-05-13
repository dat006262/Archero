




using UnityEngine;

public class BodyShaderDefault : BodyShaderBase
{
	private float outlineWidth;

	private Color m_oulineColor;

	private bool bTargetColor;

	private static Color TargetColor = new Color(1f, 0f, 0f);

	private const float TargetWidth = 0.1f;

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
		if ((bool)mMaterial && mMaterial.HasProperty("_Factor"))
		{
			outlineWidth = mMaterial.GetFloat("_Factor");
		}
		if ((bool)mMaterial)
		{
			m_outlineShader = mMaterial.shader;
		}
		if ((bool)mMaterial && mMaterial.HasProperty("_OutLineColor"))
		{
			m_oulineColor = mMaterial.GetColor("_OutLineColor");
		}
	}

	protected override void OnReturnToDefault()
	{
		if ((!BodyMeshRenderer && !BodyMeshRenderer2) || m_outlineShader == null)
		{
			return;
		}
		if (m_Entity.IsElite)
		{
			if ((bool)BodyMeshRenderer)
			{
				BodyMeshRenderer.material = mMaterial;
			}
			else if ((bool)BodyMeshRenderer2)
			{
				BodyMeshRenderer2.material = mMaterial;
			}
			SetLightShader();
			return;
		}
		mMaterial.shader = m_outlineShader;
		if ((bool)BodyMeshRenderer)
		{
			BodyMeshRenderer.sharedMaterial = mMaterial;
			BodyMeshRenderer.sharedMaterial.SetColor("_OutLineColor", (!bTargetColor) ? m_oulineColor : TargetColor);
			BodyMeshRenderer.sharedMaterial.SetFloat("_Factor", (!bTargetColor) ? outlineWidth : 0.1f);
			if (BodyMeshRenderer.sharedMaterial.HasProperty(Property_Brightness))
			{
				BodyMeshRenderer.sharedMaterial.SetFloat(Property_Brightness, Brightness_valueinit);
			}
		}
		else if ((bool)BodyMeshRenderer2)
		{
			BodyMeshRenderer2.sharedMaterial = mMaterial;
			BodyMeshRenderer2.sharedMaterial.SetColor("_OutLineColor", (!bTargetColor) ? m_oulineColor : TargetColor);
			BodyMeshRenderer2.sharedMaterial.SetFloat("_Factor", (!bTargetColor) ? outlineWidth : 0.1f);
			if (BodyMeshRenderer2.sharedMaterial.HasProperty(Property_Brightness))
			{
				BodyMeshRenderer2.sharedMaterial.SetFloat(Property_Brightness, Brightness_valueinit);
			}
		}
	}
}
