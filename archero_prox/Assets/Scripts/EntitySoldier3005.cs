




public class EntitySoldier3005 : EntityMonsterBase
{
	protected override void StartInit()
	{
		base.StartInit();
		InitWeapon(m_Data.WeaponID);
	}
}
