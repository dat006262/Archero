




using Dxx.Util;

public class AdventureTask1002 : AdventureTaskBase
{
	private int mCritCount;

	private int CritMaxCount = 20;

	public AdventureTask1002()
	{
		Event_OnCrit = OnCrit;
	}

	private void OnCrit(EntityBase source, int hit)
	{
		mCritCount++;
		UpdateUI();
	}

	protected override bool _IsTaskFinish()
	{
		return mCritCount >= CritMaxCount;
	}

	public override string GetShowTaskString()
	{
		return Utils.FormatString("暴击次数 : {0}/{1}", mCritCount, CritMaxCount);
	}
}
