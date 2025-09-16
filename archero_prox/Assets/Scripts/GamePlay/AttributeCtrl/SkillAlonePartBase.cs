




using Dxx.Util;
using System.Collections.Generic;
using TableTool;

public class SkillAlonePartBase : SkillAloneBase
{
	private const string String_CallID = "CallID";

	private const string String_Time = "Time";

	private const string String_Weight = "Weight";

	private int partid;

	private float time;

	private int weight = 1;

	private List<Goods_goods.GoodData> mAttrs = new List<Goods_goods.GoodData>();

	protected override void OnInstall()
	{
		int num = base.m_SkillData.Args.Length;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				Excute(base.m_SkillData.Args[i]);
			}
		}
		m_Entity.m_EntityData.AddDeadCall(new DeadCallData(partid, DeadAction, weight));
	}

	protected override void OnUninstall()
	{
	}

	private void Excute(string str)
	{
		bool flag = true;
		Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str);
		switch (goodData.goodType)
		{
		case "CallID":
			partid = (int)goodData.value;
			break;
		case "Time":
			time = (float)goodData.value / 1000f;
			break;
		case "Weight":
			weight = (int)goodData.value;
			break;
		default:
			flag = false;
			break;
		}
		if (!flag)
		{
			mAttrs.Add(goodData);
		}
	}

	private void DeadAction(EntityBase entity)
	{
		if (entity == null)
		{
			return;
		}
		EntityPartBodyBase entityPartBodyBase = m_Entity.CreatePartBody(partid, entity.position, time);
		if (entityPartBodyBase == null)
		{
			return;
		}
		entityPartBodyBase.SetEntityType(EntityType.PartBody);
		OnDeadAction(entity, entityPartBodyBase);
		if (entityPartBodyBase.m_EntityData != null)
		{
			int i = 0;
			for (int count = mAttrs.Count; i < count; i++)
			{
				entityPartBodyBase.m_EntityData.ExcuteAttributes(mAttrs[i]);
			}
		}
	}

	protected virtual void OnDeadAction(EntityBase deadentity, EntityPartBodyBase partbody)
	{
	}
}
