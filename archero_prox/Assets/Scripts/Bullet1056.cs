




using Dxx.Util;
using UnityEngine;

public class Bullet1056 : BulletBase
{
	private GameObject mStartObj;

	private GameObject mEndObj;

	private LineRenderer line;

	private const float textureLengthScale = 3f;

	private const float textureScrollSpeed = 8f;

	private float range;

	private float hitratio;

	protected override void OnInit()
	{
		base.OnInit();
		mStartObj = mBulletModel.Find("FireBeamStart").gameObject;
		mEndObj = mBulletModel.Find("FireBeamEnd").gameObject;
		line = mBulletModel.Find("Fire Beam").GetComponent<LineRenderer>();
		line.sortingLayerName = "Player";
		line.sortingOrder = 0;
		CheckBulletLength();
	}

	public void InitData(float range, float hitratio)
	{
		this.range = range;
		this.hitratio = hitratio;
		mBulletTransmit.SetAttack((long)((float)base.m_Entity.m_EntityData.GetAttackBase() * hitratio));
	}

	public void CheckBulletLength()
	{
		float x = MathDxx.Sin(bulletAngle);
		float z = MathDxx.Cos(bulletAngle);
		Vector3 a = new Vector3(x, 0f, z);
		mStartObj.transform.position = mTransform.position;
		Vector3 vector = mTransform.position + a * range;
		mEndObj.transform.position = vector;
		line.positionCount = 2;
		line.SetPosition(0, mTransform.position);
		line.SetPosition(1, vector);
		float num = Vector3.Distance(mTransform.position, vector);
		line.material.mainTextureScale = new Vector2(num / 3f, 1f);
		line.material.mainTextureOffset -= new Vector2(Time.deltaTime * 8f, 0f);
		boxList[0].center = new Vector3(0f, 0f, range / 2f);
		BoxCollider obj = boxList[0];
		Vector3 size = boxList[0].size;
		float x2 = size.x;
		Vector3 size2 = boxList[0].size;
		obj.size = new Vector3(x2, size2.y, range);
	}

	protected override void OnUpdate()
	{
		bulletAngle += 5f;
		UpdateMoveDirection();
		CheckBulletLength();
	}
}
