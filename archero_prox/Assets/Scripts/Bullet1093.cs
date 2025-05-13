




using UnityEngine;

public class Bullet1093 : Bullet1076
{
	protected override void OnUpdate()
	{
		base.OnUpdate();
		Transform transform = base.transform;
		Vector3 eulerAngles = base.transform.eulerAngles;
		transform.rotation = Quaternion.Euler(0f, eulerAngles.y + 5f, 0f);
	}
}
