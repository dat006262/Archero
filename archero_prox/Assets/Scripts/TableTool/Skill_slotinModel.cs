




using System.Collections.Generic;

namespace TableTool
{
	public class Skill_slotinModel : LocalModel<Skill_slotin, int>
	{
		private const string _Filename = "Skill_slotin";

		protected override string Filename => "Skill_slotin";

		protected override int GetBeanKey(Skill_slotin bean)
		{
			return bean.SkillID;
		}

		public List<int> GetSkillsByStage(int stage)
		{
			List<int> list = new List<int>();
			IList<Skill_slotin> allBeans = GetAllBeans();
			int i = 0;
			for (int count = allBeans.Count; i < count; i++)
			{
				Skill_slotin skill_slotin = allBeans[i];
				if (skill_slotin.UnlockStage == stage && !is_have_same_skill(list, skill_slotin.SkillID))
				{
					list.Add(skill_slotin.SkillID);
				}
			}
			return list;
		}

		private bool is_have_same_skill(List<int> list, int skillid)
		{
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(list[i]);
				Skill_skill beanById2 = LocalModelManager.Instance.Skill_skill.GetBeanById(skillid);
				if (beanById != null && beanById2 != null && beanById.SkillIcon == beanById2.SkillIcon)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsWeaponSkillID(int id)
		{
			switch (id)
			{
			case 1000031:
			case 1000032:
			case 1000033:
			case 1000036:
				return true;
			default:
				return false;
			}
		}
	}
}
