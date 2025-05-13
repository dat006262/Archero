




public class Weapon1015 : WeaponSprintBase
{
	protected override void OnInit()
	{
		if (m_Entity.IsElite)
		{
			distance = 4f;
		}
		else
		{
			distance = 2f;
		}
		delaytime = 0.4f;
		base.OnInit();
	}
}
