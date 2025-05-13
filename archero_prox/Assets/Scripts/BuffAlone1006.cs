




public class BuffAlone1006 : BuffAloneBase
{
	protected override void OnStart()
	{
	}

	protected override void OnRemove()
	{
	}

	protected override float OnValue(float value)
	{
		if (args.Length > 0)
		{
			return value * args[0];
		}
		return value;
	}
}
