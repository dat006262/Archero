




using PureMVC.Interfaces;
using System;
using System.Diagnostics;
using System.Text;

namespace PureMVC.Patterns
{
	[Serializable]
	public class Notification : INotification
	{
		private StringBuilder strTemp = new StringBuilder();

		public string Name
		{
			get;
			private set;
		}

		public object Body
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public string FileName
		{
			get;
			private set;
		}

		public string FuncName
		{
			get;
			private set;
		}

		public int LineNumber
		{
			get;
			private set;
		}

		public Notification(string name)
			: this(name, null, null)
		{
		}

		public Notification(string name, object body)
			: this(name, body, null)
		{
		}

		public Notification(string name, object body, string type)
		{
			Name = name;
			Body = body;
			Type = type;
		}

		public void getDebugInfo()
		{
			StackTrace stackTrace = new StackTrace(fNeedFileInfo: true);
			StackFrame frame = stackTrace.GetFrame(5);
			if (frame != null)
			{
				FileName = frame.GetFileName();
				FuncName = frame.GetMethod().Name;
				LineNumber = frame.GetFileLineNumber();
			}
		}

		public override string ToString()
		{
			strTemp.Clear();
			strTemp.AppendFormat("Notification Name: {0}", Name);
			strTemp.AppendFormat("{0}Body:{1}", Environment.NewLine, (Body != null) ? Body.ToString() : "null");
			strTemp.AppendFormat("{0}Type:{1}", Environment.NewLine, Type ?? "null");
			return strTemp.ToString();
		}
	}
}
