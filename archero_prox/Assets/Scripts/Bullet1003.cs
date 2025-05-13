




using UnityEngine;

public class Bullet1003 : BulletBase
{
	protected override bool bFlyCantHit => true;

	protected override void AwakeInit()
	{
		Parabola_MaxHeight = 4f;
	}

	public override void SetTarget(EntityBase entity, int size = 1)
	{
		base.SetTarget(entity, size);
		if (PosFromStart2Target < 3f)
		{
			PosFromStart2Target = 3f;
		}
	}

	protected override void OnInit()
	{
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void UpdateProcess()
	{
		base.UpdateProcess();
		if (base.childMesh != null)
		{
			Transform childMesh = base.childMesh;
			Vector3 localEulerAngles = base.childMesh.localEulerAngles;
			childMesh.localRotation = Quaternion.Euler(0f, 0f, localEulerAngles.z + 20f);
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		base.Speed += 0.1f;
	}
}
