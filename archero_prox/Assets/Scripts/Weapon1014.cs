




public class Weapon1014 : WeaponSprintBase
{
	protected override void OnInit()
	{
		base.OnInit();
		if (m_Entity.IsElite)
		{
			distance = 4f;
		}
		else
		{
			distance = 2f;
		}
	}
}
