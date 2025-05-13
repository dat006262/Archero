




using Dxx.Util;

public class ConditionTime : ConditionBase
{
	public float time;

	protected override void Init()
	{
	}

	public override bool IsEnd()
	{
		return Updater.AliveTime - starttime >= time;
	}
}
