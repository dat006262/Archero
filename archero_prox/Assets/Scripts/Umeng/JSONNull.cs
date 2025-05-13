




using System.IO;

namespace Umeng
{
	public class JSONNull : JSONNode
	{
		public override JSONNodeType Tag => JSONNodeType.NullValue;

		public override bool IsNull => true;

		public override string Value
		{
			get
			{
				return "null";
			}
			set
			{
			}
		}

		public override bool AsBool
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public override string ToString()
		{
			return "null";
		}

		internal override string ToString(string aIndent, string aPrefix)
		{
			return "null";
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			return obj is JSONNull;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write((byte)5);
		}
	}
}
