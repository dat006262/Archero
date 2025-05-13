




public class Weapon5033 : WeaponSprintBase
{
	private float movedis;

	protected override void OnInit()
	{
		distance = 8f;
		delaytime = 0.2f;
		movedis = 0f;
		base.OnInit();
	}

	protected override void OnUpdateMove(float currentdis)
	{
		movedis += currentdis;
	}
}
