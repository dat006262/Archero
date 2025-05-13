




using System.IO;

namespace GameProtocol
{
	public sealed class CItemUpgarde : CProtocolBase
	{
		public uint m_nTransID;

		public ulong m_nRowID;

		public uint m_nCoins;

		public uint m_nDiamonds;

		public CMaterialItem[] arrayItems;

		public override ushort GetMsgType => 9;

		protected override void OnReadFromStream(BinaryReader reader)
		{
			m_nTransID = reader.ReadUInt32();
			m_nRowID = reader.ReadUInt64();
			m_nCoins = reader.ReadUInt32();
			m_nDiamonds = reader.ReadUInt32();
			ushort num = reader.ReadUInt16();
			arrayItems = new CMaterialItem[num];
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				arrayItems[num2] = new CMaterialItem();
				arrayItems[num2].ReadFromStream(reader);
			}
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nTransID);
			writer.Write(m_nRowID);
			writer.Write(m_nCoins);
			writer.Write(m_nDiamonds);
			ushort num = (ushort)arrayItems.Length;
			writer.Write(num);
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				arrayItems[num2].WriteToStream(writer);
			}
		}
	}
}
