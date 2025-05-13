




using Dxx.Util;
using UnityEngine;

public class SkillAlone1016 : SkillAloneBase
{
	private GameObject obj;

	protected override void OnInstall()
	{
		string text = base.m_SkillData.Args[0];
		float num = float.Parse(base.m_SkillData.Args[1]);
		int num2 = int.Parse(base.m_SkillData.Args[2]);
		obj = GameLogic.EffectGet(Utils.GetString("Game/SkillPrefab/SkillAlone", text));
		SkillAloneAttrGoodBase.Add(m_Entity, obj, false, num, num2);
		m_Entity.AddNewRotateAttribute(obj);
	}

	protected override void OnUninstall()
	{
		SkillAloneAttrGoodBase.Remove(obj);
		m_Entity.RemoveRotateAttribute(obj);
	}
}
