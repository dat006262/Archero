




namespace TableTool
{
	public class Room_colorstyle : LocalBean
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

		public int StyleID
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Notes = readLocalString();
			StyleID = readInt();
			return true;
		}

		public Room_colorstyle Copy()
		{
			Room_colorstyle room_colorstyle = new Room_colorstyle();
			room_colorstyle.ID = ID;
			room_colorstyle.Notes = Notes;
			room_colorstyle.StyleID = StyleID;
			return room_colorstyle;
		}
	}
}
