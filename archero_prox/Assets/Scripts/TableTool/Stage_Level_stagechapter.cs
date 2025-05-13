




namespace TableTool
{
	public class Stage_Level_stagechapter : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public int TiledID
		{
			get;
			private set;
		}

		public int GameType
		{
			get;
			private set;
		}

		public int[] GameArgs
		{
			get;
			private set;
		}

		public string[] StyleSequence
		{
			get;
			private set;
		}

		public string StageLevel
		{
			get;
			private set;
		}

		public string OpenCondition
		{
			get;
			private set;
		}

		public int ArgsOpen
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

		public string[] GoldTurn
		{
			get;
			private set;
		}

		public int[] DropAddCond
		{
			get;
			private set;
		}

		public int DropAddProb
		{
			get;
			private set;
		}

		public int AdProb
		{
			get;
			private set;
		}

		public string[] AdTurn
		{
			get;
			private set;
		}

		public string[] ScrollRate
		{
			get;
			private set;
		}

		public string[] ScrollRateBoss
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Notes = readLocalString();
			TiledID = readInt();
			GameType = readInt();
			GameArgs = readArrayint();
			StyleSequence = readArraystring();
			StageLevel = readLocalString();
			OpenCondition = readLocalString();
			ArgsOpen = readInt();
			GoldRate = readFloat();
			EquipDropID = readInt();
			EquipProb = readInt();
			IntegralRate = readFloat();
			ExpBase = readInt();
			ExpAdd = readInt();
			GoldTurn = readArraystring();
			DropAddCond = readArrayint();
			DropAddProb = readInt();
			AdProb = readInt();
			AdTurn = readArraystring();
			ScrollRate = readArraystring();
			ScrollRateBoss = readArraystring();
			return true;
		}

		public Stage_Level_stagechapter Copy()
		{
			Stage_Level_stagechapter stage_Level_stagechapter = new Stage_Level_stagechapter();
			stage_Level_stagechapter.ID = ID;
			stage_Level_stagechapter.Notes = Notes;
			stage_Level_stagechapter.TiledID = TiledID;
			stage_Level_stagechapter.GameType = GameType;
			stage_Level_stagechapter.GameArgs = GameArgs;
			stage_Level_stagechapter.StyleSequence = StyleSequence;
			stage_Level_stagechapter.StageLevel = StageLevel;
			stage_Level_stagechapter.OpenCondition = OpenCondition;
			stage_Level_stagechapter.ArgsOpen = ArgsOpen;
			stage_Level_stagechapter.GoldRate = GoldRate;
			stage_Level_stagechapter.EquipDropID = EquipDropID;
			stage_Level_stagechapter.EquipProb = EquipProb;
			stage_Level_stagechapter.IntegralRate = IntegralRate;
			stage_Level_stagechapter.ExpBase = ExpBase;
			stage_Level_stagechapter.ExpAdd = ExpAdd;
			stage_Level_stagechapter.GoldTurn = GoldTurn;
			stage_Level_stagechapter.DropAddCond = DropAddCond;
			stage_Level_stagechapter.DropAddProb = DropAddProb;
			stage_Level_stagechapter.AdProb = AdProb;
			stage_Level_stagechapter.AdTurn = AdTurn;
			stage_Level_stagechapter.ScrollRate = ScrollRate;
			stage_Level_stagechapter.ScrollRateBoss = ScrollRateBoss;
			return stage_Level_stagechapter;
		}
	}
}
