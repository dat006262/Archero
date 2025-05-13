




using System.Collections.Generic;

namespace TableTool
{
	public class Room_level : LocalBean
	{
		private List<string[]> mList = new List<string[]>();

		public int LevelID
		{
			get;
			private set;
		}

		public string Notes
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

		public string[] RoomIDs3
		{
			get;
			private set;
		}

		public string[] RoomIDs4
		{
			get;
			private set;
		}

		public string[] RoomIDs5
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			LevelID = readInt();
			Notes = readLocalString();
			RoomIDs = readArraystring();
			RoomIDs1 = readArraystring();
			RoomIDs2 = readArraystring();
			RoomIDs3 = readArraystring();
			RoomIDs4 = readArraystring();
			RoomIDs5 = readArraystring();
			return true;
		}

		public Room_level Copy()
		{
			Room_level room_level = new Room_level();
			room_level.LevelID = LevelID;
			room_level.Notes = Notes;
			room_level.RoomIDs = RoomIDs;
			room_level.RoomIDs1 = RoomIDs1;
			room_level.RoomIDs2 = RoomIDs2;
			room_level.RoomIDs3 = RoomIDs3;
			room_level.RoomIDs4 = RoomIDs4;
			room_level.RoomIDs5 = RoomIDs5;
			return room_level;
		}

		public string[] GetList(int layer, int count)
		{
			if (mList.Count == 0)
			{
				mList.Add(RoomIDs1);
				mList.Add(RoomIDs2);
				mList.Add(RoomIDs3);
				mList.Add(RoomIDs4);
				mList.Add(RoomIDs5);
			}
			if (count < mList.Count && mList[count].Length > 0)
			{
				return mList[count];
			}
			return RoomIDs;
		}
	}
}
