




using Dxx.Util;
using TableTool;

public class SuperSkillBase
{
	private float mLastUseTime = -10000f;

	public int SkillID
	{
		get;
		private set;
	}

	public Skill_super m_Data
	{
		get;
		private set;
	}

	public EntityHero m_Entity
	{
		get;
		private set;
	}

	public bool CanUseSkill => Updater.AliveTime - mLastUseTime >= m_Data.CD;

	public void Init(EntityHero entity)
	{
		m_Entity = entity;
		string text = GetType().ToString();
		text = text.Substring(text.Length - 4, 4);
		SkillID = int.Parse(text);
		m_Data = LocalModelManager.Instance.Skill_super.GetBeanById(SkillID);
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	public void DeInit()
	{
		OnDeInit();
	}

	protected virtual void OnDeInit()
	{
	}

	public void UseSkill()
	{
		if (CanUseSkill)
		{
			mLastUseTime = Updater.AliveTime;
			OnUseSkill();
		}
	}

	protected virtual void OnUseSkill()
	{
	}
}
