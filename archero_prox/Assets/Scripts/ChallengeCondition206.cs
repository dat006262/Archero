




using Dxx.Util;

public class ChallengeCondition206 : ChallengeConditionBase
{
	private float time = 0.3f;

	protected override void OnInit()
	{
		mChallenge.BombermanEnable = true;
		if (!float.TryParse(mArg, out time))
		{
			SdkManager.Bugly_Report("ChallengeCondition206", Utils.FormatString("[{0}] is not a int value.", mArg));
		}
		mChallenge.BombermanTime = time;
	}

	protected override void OnDeInit()
	{
	}
}
