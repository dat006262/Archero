




using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using TableTool;

namespace GameProtocol
{
	public class CRespFirstRewardInfo : IProtocol
	{
		public string m_strProductId;

		public string m_strRewardinfo;

		private string[] mInfoList;

		public ushort GetMsgType => 17;

		public bool IsValid
		{
			get
			{
				mInfoList = null;
				if (string.IsNullOrEmpty(m_strProductId))
				{
					return false;
				}
				if (string.IsNullOrEmpty(m_strRewardinfo))
				{
					return false;
				}
				try
				{
					JArray jArray = JArray.Parse(m_strRewardinfo);
					if (jArray == null)
					{
						return false;
					}
					mInfoList = jArray.ToObject<string[]>();
					if (mInfoList == null || mInfoList.Length == 0)
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
				return true;
			}
		}

		public void ReadFromStream(BinaryReader reader)
		{
			m_strProductId = reader.ReadString();
			m_strRewardinfo = reader.ReadString();
		}

		public void WriteToStream(BinaryWriter writer)
		{
			writer.Write(m_strProductId);
			writer.Write(m_strRewardinfo);
		}

		public byte[] buildPacket()
		{
			BinaryWriter writer = ProtocolBuffer.Writer;
			writer.Write((byte)13);
			writer.Write(GetMsgType);
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer2 = new CustomBinaryWriter(memoryStream);
			WriteToStream(writer2);
			ushort value = (ushort)memoryStream.ToArray().Length;
			writer.Write(value);
			writer.Write(memoryStream.ToArray());
			return ProtocolBuffer.CacheStream.ToArray();
		}

		public List<Drop_DropModel.DropData> GetList()
		{
			List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
			if (!IsValid)
			{
				return list;
			}
			if (mInfoList == null || mInfoList.Length == 0)
			{
				return list;
			}
			int i = 0;
			for (int num = mInfoList.Length; i < num; i++)
			{
				string text = mInfoList[i];
				if (string.IsNullOrEmpty(text))
				{
					continue;
				}
				string[] array = text.Split(',');
				if (array.Length == 3)
				{
					int result = 0;
					int.TryParse(array[0], out result);
					int result2 = 0;
					int.TryParse(array[1], out result2);
					int result3 = 0;
					int.TryParse(array[2], out result3);
					if (result != 0 && result2 != 0 && result3 != 0)
					{
						Drop_DropModel.DropData dropData = new Drop_DropModel.DropData();
						dropData.type = (PropType)result;
						dropData.id = result2;
						dropData.count = result3;
						list.Add(dropData);
					}
				}
			}
			return list;
		}
	}
}
