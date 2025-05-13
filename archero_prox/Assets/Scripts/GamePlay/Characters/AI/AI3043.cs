




public class AI3043 : AIBase
{
	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		if (m_Entity.IsElite)
		{
			m_Entity.AddSkill(1100014);
		}
	}

	protected override void OnInit()
	{
		AddAction(new AIMove1034(m_Entity));
		AddAction(new AIMove1034(m_Entity));
		AddAction(new AIMove1034(m_Entity));
		AddAction(new AIMove1034(m_Entity));
		AddAction(GetActionWait(string.Empty, 500));
		AddAction(GetActionAttackSpecial(string.Empty, m_Entity.m_Data.WeaponID, rotate: false));
	}
}
