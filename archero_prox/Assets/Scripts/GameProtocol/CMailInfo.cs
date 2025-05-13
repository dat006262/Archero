




using Newtonsoft.Json;
using System.IO;

namespace GameProtocol
{
	public sealed class CMailInfo
	{
		public enum eMailType
		{
			ENormalMailType = 1,
			EReimburseMailType,
			eForcePopType,
			EInvalidMailType
		}

		public bool IsReaded;

		public bool IsGot;

		public uint m_nMailID;

		public string m_strTitle;

		public string m_strContent;

		public ushort m_nMailType;

		public ulong m_i64PubTime;

		public ushort m_nCoins;

		public ushort m_nDiamond;

		public bool m_bIsReaded;

		public bool m_bIsGot;

		[JsonIgnore]
		public bool IsHaveReward => (m_nCoins > 0 || m_nDiamond > 0) && m_nMailType == 2;

		[JsonIgnore]
		public bool IsShowRed => !IsReaded || (IsHaveReward && !IsGot);

		public void ReadFromStream(BinaryReader reader)
		{
			m_nMailID = reader.ReadUInt32();
			m_strTitle = reader.ReadString();
			m_strContent = reader.ReadString();
			m_nMailType = reader.ReadUInt16();
			m_i64PubTime = reader.ReadUInt64();
			m_nCoins = reader.ReadUInt16();
			m_nDiamond = reader.ReadUInt16();
		}

		public void WriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nMailID);
			writer.Write(m_strTitle);
			writer.Write(m_strContent);
			writer.Write(m_nMailType);
			writer.Write(m_i64PubTime);
			writer.Write(m_nCoins);
			writer.Write(m_nDiamond);
		}

		private bool checkValid()
		{
			return m_nMailType < 4;
		}

		public byte[] buildPacket()
		{
			return null;
		}
	}
}
