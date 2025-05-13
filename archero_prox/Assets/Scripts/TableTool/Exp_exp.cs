




namespace TableTool
{
	public class Exp_exp : LocalBean
	{
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

		public int Exp
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			LevelID = readInt();
			Notes = readLocalString();
			Exp = readInt();
			return true;
		}

		public Exp_exp Copy()
		{
			Exp_exp exp_exp = new Exp_exp();
			exp_exp.LevelID = LevelID;
			exp_exp.Notes = Notes;
			exp_exp.Exp = Exp;
			return exp_exp;
		}
	}
}
