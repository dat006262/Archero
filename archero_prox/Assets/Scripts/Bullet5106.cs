




using UnityEngine;

public class Bullet5106 : BulletBase
{
	private ConditionBase mCondition1;

	private ConditionBase mCondition2;

	protected int DelayTimeEnable = 100;

	protected override void OnInit()
	{
		base.OnInit();
		BoxEnable(enable: false);
		mCondition1 = AIMoveBase.GetConditionTime(DelayTimeEnable);
		mCondition2 = AIMoveBase.GetConditionTime(DelayTimeEnable + 100);
	}

	protected override void OnUpdate()
	{
		if (mCondition1 != null && mCondition1.IsEnd())
		{
			BoxEnable(enable: true);
			mCondition1 = null;
			create_bullet();
		}
		if (mCondition2 != null && mCondition2.IsEnd())
		{
			BoxEnable(enable: false);
			mCondition2 = null;
		}
	}

	private void create_bullet()
	{
		int num = 3;
		float num2 = GameLogic.Random(0f, 360f);
		for (int i = 0; i < num; i++)
		{
			BulletManager bullet = GameLogic.Release.Bullet;
			EntityBase entity = base.m_Entity;
			Vector3 position = base.transform.position;
			float x = position.x;
			Vector3 position2 = base.transform.position;
			bullet.CreateBullet(entity, 5107, new Vector3(x, 0.5f, position2.z), num2 + (float)i * 360f / (float)num);
		}
	}
}
