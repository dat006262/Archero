




using System.Collections.Generic;
using System.Text;

public class TGAUtils
{
	public static string DictionaryToJson(Dictionary<string, object> dict)
	{
		StringBuilder stringBuilder = new StringBuilder("{");
		foreach (KeyValuePair<string, object> item in dict)
		{
			if (item.Value is string)
			{
				stringBuilder.AppendFormat("\"{0}\":\"{1}\",", item.Key, item.Value);
			}
			else
			{
				stringBuilder.AppendFormat("\"{0}\":{1},", item.Key, item.Value);
			}
		}
		stringBuilder[stringBuilder.Length - 1] = '}';
		return stringBuilder.ToString();
	}
}
