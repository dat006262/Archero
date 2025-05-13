




namespace TableTool
{
	public class Equip_equip : LocalBean
	{
		public bool Install;

		public string primaryKey;

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

		public int PropType
		{
			get;
			private set;
		}

		public int Overlying
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

		public int EquipIcon
		{
			get;
			private set;
		}

		public int Quality
		{
			get;
			private set;
		}

		public string[] Attributes
		{
			get;
			private set;
		}

		public int[] AttributesUp
		{
			get;
			private set;
		}

		public int[] Skills
		{
			get;
			private set;
		}

		public string[] SkillsUp
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

		public string InitialPower
		{
			get;
			private set;
		}

		public string AddPower
		{
			get;
			private set;
		}

		public string Powerratio
		{
			get;
			private set;
		}

		public int[] SuperID
		{
			get;
			private set;
		}

		public int BreakNeed
		{
			get;
			private set;
		}

		public int MaxLevel
		{
			get;
			private set;
		}

		public int UpgradeNeed
		{
			get;
			private set;
		}

		public int Score
		{
			get;
			private set;
		}

		public int SellPrice
		{
			get;
			private set;
		}

		public string[] CritSellProb
		{
			get;
			private set;
		}

		public float[] SellDiamond
		{
			get;
			private set;
		}

		public int[] CardCost
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
			PropType = readInt();
			Overlying = readInt();
			Position = readInt();
			Type = readInt();
			EquipIcon = readInt();
			Quality = readInt();
			Attributes = readArraystring();
			AttributesUp = readArrayint();
			Skills = readArrayint();
			SkillsUp = readArraystring();
			AdditionSkills = readArraystring();
			UnlockCondition = readArrayint();
			InitialPower = readLocalString();
			AddPower = readLocalString();
			Powerratio = readLocalString();
			SuperID = readArrayint();
			BreakNeed = readInt();
			MaxLevel = readInt();
			UpgradeNeed = readInt();
			Score = readInt();
			SellPrice = readInt();
			CritSellProb = readArraystring();
			SellDiamond = readArrayfloat();
			CardCost = readArrayint();
			CoinCost = readArrayint();
			return true;
		}

		public Equip_equip Copy()
		{
			Equip_equip equip_equip = new Equip_equip();
			equip_equip.Id = Id;
			equip_equip.Name = Name;
			equip_equip.PropType = PropType;
			equip_equip.Overlying = Overlying;
			equip_equip.Position = Position;
			equip_equip.Type = Type;
			equip_equip.EquipIcon = EquipIcon;
			equip_equip.Quality = Quality;
			equip_equip.Attributes = Attributes;
			equip_equip.AttributesUp = AttributesUp;
			equip_equip.Skills = Skills;
			equip_equip.SkillsUp = SkillsUp;
			equip_equip.AdditionSkills = AdditionSkills;
			equip_equip.UnlockCondition = UnlockCondition;
			equip_equip.InitialPower = InitialPower;
			equip_equip.AddPower = AddPower;
			equip_equip.Powerratio = Powerratio;
			equip_equip.SuperID = SuperID;
			equip_equip.BreakNeed = BreakNeed;
			equip_equip.MaxLevel = MaxLevel;
			equip_equip.UpgradeNeed = UpgradeNeed;
			equip_equip.Score = Score;
			equip_equip.SellPrice = SellPrice;
			equip_equip.CritSellProb = CritSellProb;
			equip_equip.SellDiamond = SellDiamond;
			equip_equip.CardCost = CardCost;
			equip_equip.CoinCost = CoinCost;
			return equip_equip;
		}
	}
}
