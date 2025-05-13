




using System.Collections.Generic;

public class AdditionAttribute
{
	private float pValue;

	private List<float> list = new List<float>();

	public float Value => pValue;

	public AdditionAttribute(float value)
	{
		UpdateAttribute(value);
	}

	public AdditionAttribute()
	{
	}

	public void UpdateAttribute(float value)
	{
		if (value > 0f)
		{
			list.Add(value);
		}
		else
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				if (list[i] == 0f - value)
				{
					list.RemoveAt(i);
					break;
				}
			}
		}
		UpdateAttribute();
	}

	private void UpdateAttribute()
	{
		pValue = 1f;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			pValue *= 1f - list[i];
		}
		pValue = 1f - pValue;
	}
}
