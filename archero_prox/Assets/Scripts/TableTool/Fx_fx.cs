




namespace TableTool
{
	public class Fx_fx : LocalBean
	{
		public int FxID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public string Path
		{
			get;
			private set;
		}

		public int Node
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			FxID = readInt();
			Notes = readLocalString();
			Path = readLocalString();
			Node = readInt();
			return true;
		}

		public Fx_fx Copy()
		{
			Fx_fx fx_fx = new Fx_fx();
			fx_fx.FxID = FxID;
			fx_fx.Notes = Notes;
			fx_fx.Path = Path;
			fx_fx.Node = Node;
			return fx_fx;
		}
	}
}
