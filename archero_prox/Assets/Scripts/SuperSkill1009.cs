




using UnityEngine;

public class SuperSkill1009 : SuperSkillBase
{
	protected override void OnInit()
	{
	}

	protected override void OnDeInit()
	{
	}

	protected override void OnUseSkill()
	{
		int bulletID = 8902;
		int num = 4;
		float num2 = 360f / (float)num;
		Vector3 pos = base.m_Entity.position + new Vector3(0f, 1f, 0f);
		for (int i = 0; i < num; i++)
		{
			BulletBase bulletBase = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, bulletID, pos, (float)i * num2 + 45f);
			bulletBase.mBulletTransmit.attribute.ReboundWall = new EntityAttributeBase.ValueRange(1, 6, 6);
			bulletBase.mBulletTransmit.AddDebuffs(1029);
			bulletBase.UpdateBulletAttribute();
		}
	}
}
