




namespace TableTool
{
	public class Stage_Level_activitylevel : LocalBean
	{
		public string RoomID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public string[] Attributes
		{
			get;
			private set;
		}

		public string[] MapAttributes
		{
			get;
			private set;
		}

		public long StandardDefence
		{
			get;
			private set;
		}

		public string[] RoomIDs
		{
			get;
			private set;
		}

		public string[] RoomIDs1
		{
			get;
			private set;
		}

		public string[] RoomIDs2
		{
			get;
			private set;
		}

		public string[] Args
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			RoomID = readLocalString();
			Notes = readLocalString();
			Attributes = readArraystring();
			MapAttributes = readArraystring();
			StandardDefence = readLong();
			RoomIDs = readArraystring();
			RoomIDs1 = readArraystring();
			RoomIDs2 = readArraystring();
			Args = readArraystring();
			return true;
		}

		public Stage_Level_activitylevel Copy()
		{
			Stage_Level_activitylevel stage_Level_activitylevel = new Stage_Level_activitylevel();
			stage_Level_activitylevel.RoomID = RoomID;
			stage_Level_activitylevel.Notes = Notes;
			stage_Level_activitylevel.Attributes = Attributes;
			stage_Level_activitylevel.MapAttributes = MapAttributes;
			stage_Level_activitylevel.StandardDefence = StandardDefence;
			stage_Level_activitylevel.RoomIDs = RoomIDs;
			stage_Level_activitylevel.RoomIDs1 = RoomIDs1;
			stage_Level_activitylevel.RoomIDs2 = RoomIDs2;
			stage_Level_activitylevel.Args = Args;
			return stage_Level_activitylevel;
		}
	}
}
