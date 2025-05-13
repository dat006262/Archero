




namespace TableTool
{
	public class Stage_Level_chapter11 : LocalBean
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

		protected override bool ReadImpl()
		{
			RoomID = readLocalString();
			Notes = readLocalString();
			Attributes = readArraystring();
			MapAttributes = readArraystring();
			StandardDefence = readLong();
			RoomIDs = readArraystring();
			RoomIDs1 = readArraystring();
			return true;
		}

		public Stage_Level_chapter11 Copy()
		{
			Stage_Level_chapter11 stage_Level_chapter = new Stage_Level_chapter11();
			stage_Level_chapter.RoomID = RoomID;
			stage_Level_chapter.Notes = Notes;
			stage_Level_chapter.Attributes = Attributes;
			stage_Level_chapter.MapAttributes = MapAttributes;
			stage_Level_chapter.StandardDefence = StandardDefence;
			stage_Level_chapter.RoomIDs = RoomIDs;
			stage_Level_chapter.RoomIDs1 = RoomIDs1;
			return stage_Level_chapter;
		}
	}
}
