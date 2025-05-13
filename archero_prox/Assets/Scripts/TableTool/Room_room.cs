




namespace TableTool
{
	public class Room_room : LocalBean
	{
		public int RoomID
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

		public float[] GoodsOffset
		{
			get;
			private set;
		}

		public int Shape
		{
			get;
			private set;
		}

		public float[] CameraRound
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			RoomID = readInt();
			Notes = readLocalString();
			Difficult = readInt();
			GoodsOffset = readArrayfloat();
			Shape = readInt();
			CameraRound = readArrayfloat();
			return true;
		}

		public Room_room Copy()
		{
			Room_room room_room = new Room_room();
			room_room.RoomID = RoomID;
			room_room.Notes = Notes;
			room_room.Difficult = Difficult;
			room_room.GoodsOffset = GoodsOffset;
			room_room.Shape = Shape;
			room_room.CameraRound = CameraRound;
			return room_room;
		}
	}
}
