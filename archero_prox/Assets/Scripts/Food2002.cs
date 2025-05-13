




public class Food2002 : Food2001
{
	protected override void OnEnables()
	{
		base.OnEnables();
		RotateEnable(value: true);
	}

	protected override void OnDropEnd()
	{
		base.OnDropEnd();
		RotateEnable(value: false);
	}

	protected override void OnAbsorb()
	{
		base.OnAbsorb();
		RotateEnable(value: true);
	}
}
