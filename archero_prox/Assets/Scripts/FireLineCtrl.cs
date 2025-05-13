




using UnityEngine;

public class FireLineCtrl : MonoBehaviour
{
	private LineRenderer child;

	private Transform endeffect;

	private const float textureLengthScale = 3f;

	private const float textureScrollSpeed = 8f;

	private void Awake()
	{
		child = base.transform.Find("child").GetComponent<LineRenderer>();
		child.sortingLayerName = "Hit";
		endeffect = base.transform.Find("FrostBeamImpact");
	}

	public void UpdateLine(Vector3 startpos, Vector3 endpos)
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
