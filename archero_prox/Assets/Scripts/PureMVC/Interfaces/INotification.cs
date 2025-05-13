




namespace PureMVC.Interfaces
{
	public interface INotification
	{
		string Name
		{
			get;
		}

		object Body
		{
			get;
			set;
		}

		string Type
		{
			get;
			set;
		}

		string FileName
		{
			get;
		}

		string FuncName
		{
			get;
		}

		int LineNumber
		{
			get;
		}

		new string ToString();
	}
}
