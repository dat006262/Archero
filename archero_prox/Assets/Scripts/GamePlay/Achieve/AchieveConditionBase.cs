




using TableTool;

public abstract class AchieveConditionBase
{
	private int _id;

	protected string[] mArgs;

	private bool m_isunlock;

	private bool m_isfinish;

	protected LocalSave.AchieveDataOne mData;

	public int ID => _id;

	public bool IsUnlock => m_isunlock;

	public void Init(LocalSave.AchieveDataOne data)
	{
		mData = data;
		_id = data.achieveid;
		mArgs = data.mData.CondTypeArgs;
		Achieve_Achieve beanById = LocalModelManager.Instance.Achieve_Achieve.GetBeanById(_id);
		int unlockType = beanById.UnlockType;
		if (unlockType != 1)
		{
		}
		OnInit();
	}

	protected abstract void OnInit();

	public int GetMax()
	{
		return mData.maxcount;
	}

	public int GetCurrent()
	{
		return mData.currentcount;
	}

	public string GetBattleMaxString()
	{
		return OnGetBattleMaxString();
	}

	protected virtual string OnGetBattleMaxString()
	{
		return string.Empty;
	}

	public void Excute()
	{
		OnExcute();
	}

	protected abstract void OnExcute();

	public void SetFinish()
	{
		m_isfinish = true;
	}

	public bool IsFinish()
	{
		if (m_isfinish)
		{
			return true;
		}
		return OnIsFinish();
	}

	protected virtual bool OnIsFinish()
	{
		return false;
	}

	public string GetConditionString()
	{
		return OnGetConditionString();
	}

	protected abstract string OnGetConditionString();
}
