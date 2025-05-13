




public class AI3049 : AIBase
{
	private ActionBattle action = new ActionBattle();

	private int bulletid;

	protected override void OnInit()
	{
		bulletid = m_Entity.m_Data.WeaponID;
		action.Init(m_Entity);
		AddAction(GetActionWait(string.Empty, 3000));
		AddAction(GetActionDelegate(delegate
		{
			CreateBullets();
		}));
	}

	protected override void OnAIDeInit()
	{
		action.DeInit();
	}

	private void CreateBullets()
	{
		int num = 10;
		float num2 = 360f / (float)num;
		for (int i = 0; i < num; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(m_Entity, bulletid, m_Entity.m_Body.LeftBullet.transform.position, num2 * (float)i);
		}
	}
}
