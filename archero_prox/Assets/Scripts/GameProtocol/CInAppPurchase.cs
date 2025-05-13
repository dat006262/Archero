




using Newtonsoft.Json;
using System;
using System.IO;

namespace GameProtocol
{
	[Serializable]
	public sealed class CInAppPurchase : CProtocolBase
	{
		public uint m_nTransID;

		public ushort m_nPlatformIndex;

		public string m_nProductID;

		public string m_strReceiptData;

		[JsonIgnore]
		public override ushort GetMsgType => 15;

		protected override void OnReadFromStream(BinaryReader reader)
		{
			m_nTransID = reader.ReadUInt32();
			m_nPlatformIndex = reader.ReadUInt16();
			m_nProductID = reader.ReadString();
			m_strReceiptData = reader.ReadString();
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nTransID);
			writer.Write(m_nPlatformIndex);
			writer.Write(m_nProductID);
			writer.Write(m_strReceiptData);
		}
	}
}
