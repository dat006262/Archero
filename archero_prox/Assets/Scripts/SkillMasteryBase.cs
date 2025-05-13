




public abstract class SkillMasteryBase
{
	protected EntityBase m_Entity;

	protected string mData;

	public void Init(EntityBase entity, string data)
	{
		m_Entity = entity;
		mData = data;
		OnInit();
	}

	protected abstract void OnInit();
}
