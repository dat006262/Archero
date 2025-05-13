




public class AI2008 : AIBabyBase
{
	protected override void OnInit()
	{
		m_Entity.m_EntityData.Modify_ThroughEnemy(1, 1f);
		m_Entity.ChangeWeapon(m_Entity.m_Data.WeaponID);
		base.OnInit();
	}
}
