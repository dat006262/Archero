




using System.Collections.Generic;

public class RedLinesCtrl
{
	private List<RedLineCtrl> list = new List<RedLineCtrl>();

	public void Init(EntityBase entity, bool throughwall, int ReboundCount, int count, float perangle)
	{
		list.Clear();
		float num = perangle * (float)(count - 1) / 2f;
		for (int i = 0; i < count; i++)
		{
			RedLineCtrl redLineCtrl = new RedLineCtrl();
			float offsetangle = num - perangle * (float)i;
			redLineCtrl.Init(entity, throughwall, ReboundCount, offsetangle);
			list.Add(redLineCtrl);
		}
	}

	public void DeInit()
	{
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			list[i].DeInit();
		}
		list.Clear();
	}

	public void Update()
	{
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			list[i].Update();
		}
	}
}
