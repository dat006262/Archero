




namespace TableTool
{
	public class Operation_move : LocalBean
	{
		public int MoveStateID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public int AttackRemove
		{
			get;
			private set;
		}

		public string[] Args
		{
			get;
			private set;
		}

		public string Args_note
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			MoveStateID = readInt();
			Notes = readLocalString();
			AttackRemove = readInt();
			Args = readArraystring();
			Args_note = readLocalString();
			return true;
		}

		public Operation_move Copy()
		{
			Operation_move operation_move = new Operation_move();
			operation_move.MoveStateID = MoveStateID;
			operation_move.Notes = Notes;
			operation_move.AttackRemove = AttackRemove;
			operation_move.Args = Args;
			operation_move.Args_note = Args_note;
			return operation_move;
		}
	}
}
