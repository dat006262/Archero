




namespace TableTool
{
	public class Stage_Level_activity : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int Type
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public int Difficult
		{
			get;
			private set;
		}

		public string StageLevel
		{
			get;
			private set;
		}

		public int MaxLayer
		{
			get;
			private set;
		}

		public string[] StyleSequence
		{
			get;
			private set;
		}

		public int LevelCondition
		{
			get;
			private set;
		}

		public int[] TimeCondition
		{
			get;
			private set;
		}

		public int Number
		{
			get;
			private set;
		}

		public int[] Power
		{
			get;
			private set;
		}

		public int[] Price
		{
			get;
			private set;
		}

		public float GoldRate
		{
			get;
			private set;
		}

		public int EquipDropID
		{
			get;
			private set;
		}

		public int EquipProb
		{
			get;
			private set;
		}

		public float IntegralRate
		{
			get;
			private set;
		}

		public int[] Reward
		{
			get;
			private set;
		}

		public string[] Args
		{
			get;
			private set;
		}

		public string StandardRoom
		{
			get;
			private set;
		}

		public float Integral_Ratio
		{
			get;
			private set;
		}

		public int ExpBase
		{
			get;
			private set;
		}

		public int ExpAdd
		{
			get;
			private set;
		}

		public bool Unlock => LocalSave.Instance.GetLevel() >= LevelCondition;

		protected override bool ReadImpl()
		{
			ID = readInt();
			Type = readInt();
			Notes = readLocalString();
			Difficult = readInt();
			StageLevel = readLocalString();
			MaxLayer = readInt();
			StyleSequence = readArraystring();
			LevelCondition = readInt();
			TimeCondition = readArrayint();
			Number = readInt();
			Power = readArrayint();
			Price = readArrayint();
			GoldRate = readFloat();
			EquipDropID = readInt();
			EquipProb = readInt();
			IntegralRate = readFloat();
			Reward = readArrayint();
			Args = readArraystring();
			StandardRoom = readLocalString();
			Integral_Ratio = readFloat();
			ExpBase = readInt();
			ExpAdd = readInt();
			return true;
		}

		public Stage_Level_activity Copy()
		{
			Stage_Level_activity stage_Level_activity = new Stage_Level_activity();
			stage_Level_activity.ID = ID;
			stage_Level_activity.Type = Type;
			stage_Level_activity.Notes = Notes;
			stage_Level_activity.Difficult = Difficult;
			stage_Level_activity.StageLevel = StageLevel;
			stage_Level_activity.MaxLayer = MaxLayer;
			stage_Level_activity.StyleSequence = StyleSequence;
			stage_Level_activity.LevelCondition = LevelCondition;
			stage_Level_activity.TimeCondition = TimeCondition;
			stage_Level_activity.Number = Number;
			stage_Level_activity.Power = Power;
			stage_Level_activity.Price = Price;
			stage_Level_activity.GoldRate = GoldRate;
			stage_Level_activity.EquipDropID = EquipDropID;
			stage_Level_activity.EquipProb = EquipProb;
			stage_Level_activity.IntegralRate = IntegralRate;
			stage_Level_activity.Reward = Reward;
			stage_Level_activity.Args = Args;
			stage_Level_activity.StandardRoom = StandardRoom;
			stage_Level_activity.Integral_Ratio = Integral_Ratio;
			stage_Level_activity.ExpBase = ExpBase;
			stage_Level_activity.ExpAdd = ExpAdd;
			return stage_Level_activity;
		}

		public GameMode GetMode()
		{
			return (GameMode)Type;
		}
	}
}
