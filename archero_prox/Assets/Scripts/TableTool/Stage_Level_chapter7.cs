




using System.Collections.Generic;

namespace TableTool
{
	[System.Serializable]
	public class Stage_Level_chapter7 : LocalBean
	{
		public string RoomID
	;
		public string Notes
	;
		public string[] Attributes
	;
		public string[] MapAttributes
	;
		public long StandardDefence
	;
		public string[] RoomIDs
	;
		public string[] RoomIDs1
	;
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

		protected override List<byte> WriteImpl()
		{
			writeLocalString(RoomID);
			writeLocalString(Notes);
			writeArrayString(Attributes);
			writeArrayString(MapAttributes);
			writeLong(StandardDefence);
			writeArrayString(RoomIDs);
			writeArrayString(RoomIDs1);
			
			return byteList;
		}

		public Stage_Level_chapter7 Copy()
		{
			Stage_Level_chapter7 stage_Level_chapter = new Stage_Level_chapter7();
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
