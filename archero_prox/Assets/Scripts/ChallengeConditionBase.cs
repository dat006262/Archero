




using Dxx.Util;

public abstract class ChallengeConditionBase
{
	private int _id;

	protected int result = 1;

	protected string mArg;

	protected ChallengeModeBase mChallenge;

	public int ID => _id;

	public int Result => result;

	public void Init(int id, string arg, ChallengeModeBase challengedata)
	{
		_id = id;
		mArg = arg;
		mChallenge = challengedata;
		OnInit();
	}

	protected abstract void OnInit();

	public void Start()
	{
		OnStart();
	}

	protected virtual void OnStart()
	{
	}

	protected void OnFailure()
	{
		result = 2;
		mChallenge.CheckCondition();
	}

	protected void OnSuccess()
	{
		result = 1;
		mChallenge.CheckCondition();
	}

	public string GetConditionString()
	{
		return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("Challenge_Condition{0}", ID));
	}

	public void DeInit()
	{
		OnDeInit();
	}

	protected abstract void OnDeInit();
}
