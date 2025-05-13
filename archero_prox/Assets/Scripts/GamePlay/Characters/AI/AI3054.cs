




public class AI3054 : AIBase
{
	private int bulletid;

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		bulletid = m_Entity.m_Data.WeaponID;
		if (m_Entity.IsElite)
		{
			bulletid = 1096;
		}
	}

	protected override void OnInit()
	{
		AddAction(new AIMove1052(m_Entity, 4));
		AddAction(GetActionAttack(string.Empty, bulletid));
	}
}
