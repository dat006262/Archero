




using UnityEngine;

public class Bullet5017 : Bullet8001
{
	private float angle;

	protected override void OnRotate()
	{
		angle += m_Data.RotateSpeed;
		childMesh.localRotation = Quaternion.Euler(angle, 0f, 0f);
	}
}
