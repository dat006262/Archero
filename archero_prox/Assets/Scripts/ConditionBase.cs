




using Dxx.Util;

public abstract class ConditionBase
{
	protected float starttime;

	public ConditionBase()
	{
		starttime = Updater.AliveTime;
		Init();
	}

	protected abstract void Init();

	public virtual bool IsEnd()
	{
		return true;
	}
}
