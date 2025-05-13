




namespace TableTool
{
	public class Character_LevelModel : LocalModel<Character_Level, int>
	{
		private const string _Filename = "Character_Level";

		private int maxLevel;

		protected override string Filename => "Character_Level";

		protected override int GetBeanKey(Character_Level bean)
		{
			return bean.ID;
		}

		public void Init()
		{
			maxLevel = GetAllBeans().Count;
		}

		public int GetExp(int level)
		{
			return GetBeanById(level)?.Exp ?? 1;
		}

		public int GetMaxLevel()
		{
			return maxLevel;
		}

		public int GetLevelUpCount(int addexp)
		{
			int num = LocalSave.Instance.GetLevel();
			int num2 = (int)LocalSave.Instance.GetExp();
			int exp = LocalModelManager.Instance.Character_Level.GetExp(num);
			int num3 = 0;
			while (num2 + addexp >= exp)
			{
				num++;
				num3++;
				addexp -= exp - num2;
				num2 = 0;
				exp = LocalModelManager.Instance.Character_Level.GetExp(num);
			}
			return num3;
		}
	}
}
