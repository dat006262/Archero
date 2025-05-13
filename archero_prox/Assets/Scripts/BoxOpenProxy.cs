




using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using TableTool;

public class BoxOpenProxy : Proxy
{
	public class Transfer
	{
		public EquipSource source;

		public List<Drop_DropModel.DropData> list;

		public int[] diamonds;

		public int count = 1;

		public Action retry_callback;

		public int GetCurrentDiamond()
		{
			if (diamonds.Length > 0)
			{
				if (count < diamonds.Length)
				{
					return diamonds[count];
				}
				return diamonds[diamonds.Length - 1];
			}
			SdkManager.Bugly_Report("BoxOpenProxy", "diamonds is null");
			return 0;
		}

		public int GetStartDiamond()
		{
			if (diamonds.Length > 0)
			{
				return diamonds[0];
			}
			return 0;
		}

		public void AddCount()
		{
			count++;
		}
	}

	public new const string NAME = "BoxOpenProxy";

	public BoxOpenProxy(object data)
		: base("BoxOpenProxy", data)
	{
	}
}
