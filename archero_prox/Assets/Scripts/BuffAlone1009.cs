




using System.Collections.Generic;

public class BuffAlone1009 : BuffAloneBase
{
	private float range;

	protected override void OnStart()
	{
		range = buff_data.Args[0];
	}

	protected override void OnRemove()
	{
	}

	protected override void ExcuteBuff(BuffData data)
	{
		List<EntityBase> roundSelfEntities = GameLogic.Release.Entity.GetRoundSelfEntities(m_Entity, range, sameteam: false);
		int i = 0;
		for (int count = roundSelfEntities.Count; i < count; i++)
		{
			EntityBase target = roundSelfEntities[i];
			GameLogic.SendBuff(target, m_Entity, 1010);
		}
	}
}
