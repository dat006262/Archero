




namespace TableTool
{
	public class Drop_Drop : LocalBean
	{
		public int DropID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public int DropType
		{
			get;
			private set;
		}

		public string[] Prob
		{
			get;
			private set;
		}

		public string[] Rand1
		{
			get;
			private set;
		}

		public string[] Rand2
		{
			get;
			private set;
		}

		public string[] Rand3
		{
			get;
			private set;
		}

		public string[] Rand4
		{
			get;
			private set;
		}

		public string[] Rand5
		{
			get;
			private set;
		}

		public string[] Fixed
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			DropID = readInt();
			Notes = readLocalString();
			DropType = readInt();
			Prob = readArraystring();
			Rand1 = readArraystring();
			Rand2 = readArraystring();
			Rand3 = readArraystring();
			Rand4 = readArraystring();
			Rand5 = readArraystring();
			Fixed = readArraystring();
			return true;
		}

		public Drop_Drop Copy()
		{
			Drop_Drop drop_Drop = new Drop_Drop();
			drop_Drop.DropID = DropID;
			drop_Drop.Notes = Notes;
			drop_Drop.DropType = DropType;
			drop_Drop.Prob = Prob;
			drop_Drop.Rand1 = Rand1;
			drop_Drop.Rand2 = Rand2;
			drop_Drop.Rand3 = Rand3;
			drop_Drop.Rand4 = Rand4;
			drop_Drop.Rand5 = Rand5;
			drop_Drop.Fixed = Fixed;
			return drop_Drop;
		}
	}
}
