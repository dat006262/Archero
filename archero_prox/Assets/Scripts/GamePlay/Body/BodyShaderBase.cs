




using Dxx.Util;
using UnityEngine;
using UnityEngine.Rendering;

public class BodyShaderBase
{
	protected EntityBase m_Entity;

	protected BodyMask m_Body;

	protected SkinnedMeshRenderer BodyMeshRenderer;

	protected MeshRenderer BodyMeshRenderer2;

	protected Shader m_outlineShader;

	protected Shader m_alphaShader;

	protected Shader m_deadShader;

	protected Material mMaterial;

	protected int Property_Brightness;

	private int Property_EmissionColor;

	protected int Property_RimColor;

	protected int Property_TextureColor;

	protected float Brightness_valueinit;

	public void Init(EntityBase entity)
	{
		m_Entity = entity;
		m_Body = m_Entity.m_Body;
		BodyMeshRenderer = m_Body.Body.GetComponent<SkinnedMeshRenderer>();
		if ((bool)BodyMeshRenderer)
		{
			BodyMeshRenderer.sortingLayerName = "Player";
			BodyMeshRenderer.shadowCastingMode = ShadowCastingMode.On;
		}
		else
		{
			BodyMeshRenderer2 = m_Body.Body.GetComponent<MeshRenderer>();
			if (BodyMeshRenderer2 != null)
			{
				BodyMeshRenderer2.sortingLayerName = "Player";
				BodyMeshRenderer2.shadowCastingMode = ShadowCastingMode.On;
			}
		}
		m_alphaShader = Shader.Find("Custom/OutLight");
		m_deadShader = Shader.Find("Custom/DeadShader");
		Property_Brightness = Shader.PropertyToID("_RimPower");
		Property_EmissionColor = Shader.PropertyToID("_EmissionColor");
		Property_RimColor = Shader.PropertyToID("_RimColor");
		Property_TextureColor = Shader.PropertyToID("_TextureColor");
		OnInit();
		if ((bool)mMaterial)
		{
			if (m_Entity.IsElite)
			{
				mMaterial.shader = m_alphaShader;
				mMaterial.SetColor(Property_RimColor, new Color(0f, 22f / 85f, 1f));
				mMaterial.SetFloat(Property_Brightness, 1.23f);
				SetLightShader();
			}
			if (mMaterial.HasProperty(Property_Brightness))
			{
				Brightness_valueinit = mMaterial.GetFloat(Property_Brightness);
			}
		}
	}

	protected virtual void OnInit()
	{
	}

	protected void SetLightShader()
	{
		if (!BodyMeshRenderer && !BodyMeshRenderer2)
		{
			return;
		}
		if ((bool)BodyMeshRenderer)
		{
			if (BodyMeshRenderer.material.HasProperty(Property_RimColor))
			{
				BodyMeshRenderer.material.SetColor(Property_RimColor, new Color(0f, 22f / 85f, 1f));
			}
			if (BodyMeshRenderer.material.HasProperty(Property_Brightness))
			{
				BodyMeshRenderer.material.SetFloat(Property_Brightness, 1.23f);
			}
		}
		else if ((bool)BodyMeshRenderer2)
		{
			if (BodyMeshRenderer2.material.HasProperty(Property_RimColor))
			{
				BodyMeshRenderer2.material.SetColor(Property_RimColor, new Color(0f, 22f / 85f, 1f));
			}
			if (BodyMeshRenderer2.material.HasProperty(Property_Brightness))
			{
				BodyMeshRenderer2.material.SetFloat(Property_Brightness, 1.23f);
			}
		}
		int i = 0;
		for (int count = m_Body.Body_Extra.Count; i < count; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = m_Body.Body_Extra[i];
			if ((bool)skinnedMeshRenderer)
			{
				skinnedMeshRenderer.material = mMaterial;
				if (skinnedMeshRenderer.material.HasProperty(Property_RimColor))
				{
					skinnedMeshRenderer.material.SetColor(Property_RimColor, new Color(0f, 22f / 85f, 1f));
				}
				if (skinnedMeshRenderer.material.HasProperty(Property_Brightness))
				{
					skinnedMeshRenderer.material.SetFloat(Property_Brightness, 1.23f);
				}
			}
		}
	}

	public void ReturnToDefault()
	{
		OnReturnToDefault();
	}

	protected virtual void OnReturnToDefault()
	{
	}

	public void SetHitted()
	{
		if ((bool)BodyMeshRenderer || (bool)BodyMeshRenderer2)
		{
			if ((bool)BodyMeshRenderer)
			{
				BodyMeshRenderer.material.shader = m_alphaShader;
				BodyMeshRenderer.material.SetColor(Property_RimColor, Color.white);
				BodyMeshRenderer.material.SetColor(Property_RimColor, Color.white);
			}
			else if ((bool)BodyMeshRenderer2)
			{
				BodyMeshRenderer2.material.shader = m_alphaShader;
				BodyMeshRenderer2.material.SetColor(Property_RimColor, Color.white);
				BodyMeshRenderer2.material.SetColor(Property_RimColor, Color.white);
			}
		}
	}

	public void OnUpdateHitted(float value)
	{
		if ((bool)BodyMeshRenderer || (bool)BodyMeshRenderer2)
		{
			if ((bool)BodyMeshRenderer)
			{
				BodyMeshRenderer.material.SetFloat(Property_Brightness, value);
			}
			else if ((bool)BodyMeshRenderer2)
			{
				BodyMeshRenderer2.material.SetFloat(Property_Brightness, value);
			}
		}
	}

	public void SetElement(Color color)
	{
		if ((bool)BodyMeshRenderer || (bool)BodyMeshRenderer2)
		{
			if ((bool)BodyMeshRenderer)
			{
				BodyMeshRenderer.material.shader = m_alphaShader;
				BodyMeshRenderer.material.SetFloat(Property_Brightness, 1f);
				BodyMeshRenderer.material.SetColor(Property_RimColor, color);
			}
			else if ((bool)BodyMeshRenderer2)
			{
				BodyMeshRenderer2.material.shader = m_alphaShader;
				BodyMeshRenderer2.material.SetFloat(Property_Brightness, 1f);
				BodyMeshRenderer2.material.SetColor(Property_RimColor, color);
			}
		}
	}

	public void SetTexture(string textureid)
	{
		string path = Utils.FormatString("Game/ModelsTexture/{0}", textureid);
		Texture texture = ResourceManager.Load<Texture>(path);
		if (!(texture != null))
		{
			return;
		}
		if ((bool)mMaterial)
		{
			mMaterial.SetTexture("_MainTex", texture);
		}
		if (m_Body.Body_Extra != null)
		{
			int i = 0;
			for (int count = m_Body.Body_Extra.Count; i < count; i++)
			{
				m_Body.Body_Extra[i].material.SetTexture("_MainTex", texture);
			}
		}
	}

	public void SetOrder(int order)
	{
		if ((bool)BodyMeshRenderer)
		{
			BodyMeshRenderer.sortingOrder = order;
		}
		else if ((bool)BodyMeshRenderer2)
		{
			BodyMeshRenderer2.sortingOrder = order;
		}
	}

	public void SetStrengh()
	{
		if ((bool)mMaterial && mMaterial.HasProperty("_MainColor"))
		{
			mMaterial.SetColor("_MainColor", new Color(33f / 85f, 33f / 85f, 33f / 85f));
		}
	}
}
