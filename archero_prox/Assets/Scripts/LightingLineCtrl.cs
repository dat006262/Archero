




using UnityEngine;

public class LightingLineCtrl : MonoBehaviour
{
	private LineRenderer child;

	private Transform endeffect;

	private ParticleSystem[] ps;

	private const float textureLengthScale = 3f;

	private const float textureScrollSpeed = 8f;

	private Transform ball_child;

	private EntityBase target;

	private void Awake()
	{
		child = base.transform.Find("child").GetComponent<LineRenderer>();
		child.sortingLayerName = "Hit";
		endeffect = base.transform.Find("break");
		ps = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
	}

	public void Init(Transform child, EntityBase target)
	{
		ball_child = child;
		this.target = target;
		UpdateLine(ball_child.position, target.m_Body.EffectMask.transform.position);
	}

	private void Update()
	{
		if ((bool)target && (bool)ball_child)
		{
			UpdateLine(ball_child.position, target.m_Body.EffectMask.transform.position);
		}
	}

	private void UpdateLine(Vector3 startpos, Vector3 endpos)
	{
		child.positionCount = 2;
		child.SetPosition(0, startpos);
		child.SetPosition(1, endpos);
		float num = Vector3.Distance(startpos, endpos);
		child.material.mainTextureScale = new Vector2(num / 3f, 1f);
		child.material.mainTextureOffset -= new Vector2(Time.deltaTime * 8f, 0f);
		endeffect.position = endpos;
	}
}
