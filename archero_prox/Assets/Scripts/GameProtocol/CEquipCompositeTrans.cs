




using System.IO;

namespace GameProtocol
{
	public sealed class CEquipCompositeTrans : CProtocolBase
	{
		public uint m_nTransID;

		public CEquipmentItem[] m_arrCompositeInfo;

		public override ushort GetMsgType => 18;

		protected override void OnReadFromStream(BinaryReader reader)
		{
			m_nTransID = reader.ReadUInt32();
			ushort num = reader.ReadUInt16();
			m_arrCompositeInfo = new CEquipmentItem[num];
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				m_arrCompositeInfo[num2] = new CEquipmentItem();
				m_arrCompositeInfo[num2].ReadFromStream(reader);
			}
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nTransID);
			writer.Write((ushort)m_arrCompositeInfo.Length);
			for (ushort num = 0; num < (ushort)m_arrCompositeInfo.Length; num = (ushort)(num + 1))
			{
				m_arrCompositeInfo[num].WriteToStream(writer);
			}
		}
	}
}
