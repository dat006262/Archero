




using Dxx.Util;
using System.IO;

namespace GameProtocol
{
	public sealed class CRespItemPacket : IProtocol
	{
		public const ushort MsgType = 7;

		public CCommonRespMsg m_commMsg;

		public CEquipmentItem[] m_arrayEquipItems;

		public ushort GetMsgType => 7;

		public void ReadFromStream(BinaryReader reader)
		{
			try
			{
				m_commMsg = new CCommonRespMsg();
				m_commMsg.ReadFromStream(reader);
				ushort num = reader.ReadUInt16();
				m_arrayEquipItems = new CEquipmentItem[num];
				for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
				{
					m_arrayEquipItems[num2] = new CEquipmentItem();
					m_arrayEquipItems[num2].ReadFromStream(reader);
				}
				LocalSave.Instance.SetEquips(this);
			}
			catch
			{
				Debugger.Log("!!!!!!!!!!!!!!!!!!!! resp equips error");
				SdkManager.Bugly_Report("CRespItemPacket", Utils.FormatString("ReadFromStream error"));
			}
		}

		public void WriteToStream(BinaryWriter writer)
		{
			m_commMsg.WriteToStream(writer);
			ushort num = (ushort)((m_arrayEquipItems != null) ? m_arrayEquipItems.Length : 0);
			writer.Write(num);
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				m_arrayEquipItems[num2].WriteToStream(writer);
			}
		}

		public byte[] buildPacket()
		{
			BinaryWriter writer = ProtocolBuffer.Writer;
			writer.Write((byte)13);
			writer.Write((ushort)7);
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer2 = new CustomBinaryWriter(memoryStream);
			WriteToStream(writer2);
			ushort value = (ushort)memoryStream.ToArray().Length;
			writer.Write(value);
			writer.Write(memoryStream.ToArray());
			return ProtocolBuffer.CacheStream.ToArray();
		}
	}
}
