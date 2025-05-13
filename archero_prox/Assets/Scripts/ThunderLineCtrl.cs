




using Dxx.Util;
using UnityEngine;

public class ThunderLineCtrl : MonoBehaviour
{
	private Transform child;

	private EntityBase from;

	private EntityBase to;

	private Vector3 frompos;

	private Vector3 topos;

	private MeshRenderer[] meshes;

	private void Awake()
	{
		child = base.transform.Find("line");
		meshes = GetComponentsInChildren<MeshRenderer>();
		int i = 0;
		for (int num = meshes.Length; i < num; i++)
		{
			meshes[i].sortingLayerName = "BulletEffect";
		}
	}

	private void Update()
	{
		UpdateEntity();
	}

	private void UpdateEntity()
	{
		if ((bool)from && (bool)to && (bool)child && (bool)from.m_Body && (bool)to.m_Body)
		{
			frompos = from.m_Body.DeadNode.transform.position;
			topos = to.m_Body.DeadNode.transform.position;
			float num = Vector3.Distance(frompos, topos);
			float angle = Utils.getAngle(frompos - topos);
			base.transform.rotation = Quaternion.Euler(0f, angle, 0f);
			base.transform.position = frompos;
			base.transform.LookAt(to.m_Body.DeadNode.transform);
			Transform transform = child;
			Vector3 localScale = child.localScale;
			float x = localScale.x;
			float y = num;
			Vector3 localScale2 = child.localScale;
			transform.localScale = new Vector3(x, y, localScale2.z);
			child.localPosition = new Vector3(0f, 0f, num / 2f);
		}
	}

	public void UpdateEntity(EntityBase from, EntityBase to)
	{
		this.from = from;
		this.to = to;
		UpdateEntity();
	}
}
