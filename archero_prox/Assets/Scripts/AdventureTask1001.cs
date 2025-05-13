




using Dxx.Util;

public class AdventureTask1001 : AdventureTaskBase
{
	private int mHittedCount;

	private int HittedMaxCount = 3;

	public AdventureTask1001()
	{
		Event_OnHitted = OnHitted;
	}

	private void OnHitted(EntityBase source)
	{
		mHittedCount++;
		UpdateUI();
	}

	protected override bool _IsTaskFinish()
	{
		return mHittedCount < HittedMaxCount;
	}

	public override string GetShowTaskString()
	{
		return Utils.FormatString("受击次数 : {0}/{1}", mHittedCount, HittedMaxCount);
	}
}
