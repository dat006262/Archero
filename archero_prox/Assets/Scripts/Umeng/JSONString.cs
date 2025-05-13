




using System.IO;

namespace Umeng
{
	public class JSONString : JSONNode
	{
		private string m_Data;

		public override JSONNodeType Tag => JSONNodeType.String;

		public override bool IsString => true;

		public override string Value
		{
			get
			{
				return m_Data;
			}
			set
			{
				m_Data = value;
			}
		}

		public JSONString(string aData)
		{
			m_Data = aData;
		}

		public override string ToString()
		{
			return "\"" + JSONNode.Escape(m_Data) + "\"";
		}

		internal override string ToString(string aIndent, string aPrefix)
		{
			return "\"" + JSONNode.Escape(m_Data) + "\"";
		}

		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write((byte)3);
			aWriter.Write(m_Data);
		}
	}
}
