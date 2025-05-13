




namespace TableTool
{
	public class Skill_slotoutcost : LocalBean
	{
		public int Id
		{
			get;
			private set;
		}

		public int UpperLimit
		{
			get;
			private set;
		}

		public int LowerLimit
		{
			get;
			private set;
		}

		public int CoinCost
		{
			get;
			private set;
		}

		public int TimeCost
		{
			get;
			private set;
		}

		public int NeedLevel
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			Id = readInt();
			UpperLimit = readInt();
			LowerLimit = readInt();
			CoinCost = readInt();
			TimeCost = readInt();
			NeedLevel = readInt();
			return true;
		}

		public Skill_slotoutcost Copy()
		{
			Skill_slotoutcost skill_slotoutcost = new Skill_slotoutcost();
			skill_slotoutcost.Id = Id;
			skill_slotoutcost.UpperLimit = UpperLimit;
			skill_slotoutcost.LowerLimit = LowerLimit;
			skill_slotoutcost.CoinCost = CoinCost;
			skill_slotoutcost.TimeCost = TimeCost;
			skill_slotoutcost.NeedLevel = NeedLevel;
			return skill_slotoutcost;
		}
	}
}
