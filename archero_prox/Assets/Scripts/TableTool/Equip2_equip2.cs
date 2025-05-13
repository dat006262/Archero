




namespace TableTool
{
	public class Equip2_equip2 : LocalBean
	{
		public int Id
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public int Position
		{
			get;
			private set;
		}

		public int Type
		{
			get;
			private set;
		}

		public int Icon
		{
			get;
			private set;
		}

		public int DropIcon
		{
			get;
			private set;
		}

		public string BaseAttributes
		{
			get;
			private set;
		}

		public int LevelBaseAttributes
		{
			get;
			private set;
		}

		public string[] AdditionSkills
		{
			get;
			private set;
		}

		public int[] UnlockCondition
		{
			get;
			private set;
		}

		public int[] SuperID
		{
			get;
			private set;
		}

		public int[] CoinCost
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			Id = readInt();
			Name = readLocalString();
			Position = readInt();
			Type = readInt();
			Icon = readInt();
			DropIcon = readInt();
			BaseAttributes = readLocalString();
			LevelBaseAttributes = readInt();
			AdditionSkills = readArraystring();
			UnlockCondition = readArrayint();
			SuperID = readArrayint();
			CoinCost = readArrayint();
			return true;
		}

		public Equip2_equip2 Copy()
		{
			Equip2_equip2 equip2_equip = new Equip2_equip2();
			equip2_equip.Id = Id;
			equip2_equip.Name = Name;
			equip2_equip.Position = Position;
			equip2_equip.Type = Type;
			equip2_equip.Icon = Icon;
			equip2_equip.DropIcon = DropIcon;
			equip2_equip.BaseAttributes = BaseAttributes;
			equip2_equip.LevelBaseAttributes = LevelBaseAttributes;
			equip2_equip.AdditionSkills = AdditionSkills;
			equip2_equip.UnlockCondition = UnlockCondition;
			equip2_equip.SuperID = SuperID;
			equip2_equip.CoinCost = CoinCost;
			return equip2_equip;
		}
	}
}
