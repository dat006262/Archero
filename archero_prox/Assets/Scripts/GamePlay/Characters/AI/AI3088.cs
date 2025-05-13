




public class AI3088 : AIBase
{
	private int ran;

	protected override void OnInit()
	{
		ran = GameLogic.Random(2, 5);
		for (int i = 0; i < ran; i++)
		{
			AddAction(new AIMove1002(m_Entity, 800, 1500));
		}
		AddAction(GetActionDelegate(delegate
		{
			for (int j = 0; j < 4; j++)
			{
				BulletBase bulletBase = GameLogic.Release.Bullet.CreateBullet(m_Entity, m_Entity.m_Data.WeaponID, m_Entity.m_Body.EffectMask.transform.position, (float)j * 90f);
				bulletBase.transform.SetParent(m_Entity.transform);
			}
		}));
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}
}
