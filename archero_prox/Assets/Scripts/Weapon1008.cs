




using UnityEngine;

public class Weapon1008 : WeaponBase
{
	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 2; i++)
		{
			action.AddActionDelegate(delegate
			{
				if (m_Entity.IsElite)
				{
					for (int j = 0; j < 3; j++)
					{
						CreateBulletOverride(Vector3.zero, GameLogic.Random(0f - m_Data.RandomAngle, m_Data.RandomAngle) + (float)j * 45f - 45f);
					}
				}
				else
				{
					CreateBulletOverride(Vector3.zero, GameLogic.Random(0f - m_Data.RandomAngle, m_Data.RandomAngle));
				}
			});
			action.AddActionWait(0.2f);
		}
	}
}
