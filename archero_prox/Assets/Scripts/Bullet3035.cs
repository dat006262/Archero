




using Dxx.Util;
using UnityEngine;

public class Bullet3035 : BulletBase
{
	private BulletBombDodge_effect effectctrl;

	private Transform effect;

	private const float textureLengthScale = 3f;

	private const float textureScrollSpeed = 8f;

	private const float width = 13f;

	private int layerMask;

	protected override void OnInit()
	{
		base.OnInit();
		effect = mBulletModel.Find("effect");
		effectctrl = mBulletModel.GetComponent<BulletBombDodge_effect>();
		if (m_Data.bThroughInsideWall)
		{
			layerMask = 1 << LayerManager.MapOutWall;
		}
		else
		{
			layerMask = ((1 << LayerManager.Stone) | (1 << LayerManager.MapOutWall));
		}
		CheckBulletLength();
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
		effect.gameObject.SetActive(value: false);
	}

	public void CheckBulletLength()
	{
		float x = MathDxx.Sin(bulletAngle);
		float z = MathDxx.Cos(bulletAngle);
		RaycastHit[] array = Physics.RaycastAll(direction: new Vector3(x, 0f, z), origin: mTransform.position, maxDistance: 100f, layerMask: layerMask);
		float num = 100f;
		int i = 0;
		for (int num2 = array.Length; i < num2; i++)
		{
			RaycastHit raycastHit = array[i];
			if (((raycastHit.collider.gameObject.layer == LayerManager.Stone && !m_Data.bThroughWall) || raycastHit.collider.gameObject.layer == LayerManager.MapOutWall) && num > raycastHit.distance)
			{
				num = raycastHit.distance;
			}
		}
		float num3 = num / 13f;
		if ((bool)effect)
		{
			effect.localScale = new Vector3(num3, 1f, 1f);
		}
		effectctrl.SetScale(num3);
		boxList[0].center = new Vector3(0f, 0f, num / 2f);
		BoxCollider obj = boxList[0];
		Vector3 size = boxList[0].size;
		float x2 = size.x;
		Vector3 size2 = boxList[0].size;
		obj.size = new Vector3(x2, size2.y, num);
		effect.gameObject.SetActive(value: true);
	}
}
