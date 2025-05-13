




using Newtonsoft.Json;
using System;
using System.IO;

[Serializable]
public abstract class CProtocolBase : IProtocol
{
	[JsonIgnore]
	private string _strUserID = string.Empty;

	[JsonIgnore]
	private ushort _nSoftVersion;

	[JsonIgnore]
	private string _strDeviceID = string.Empty;

	[JsonIgnore]
	private string _strAccessToken = string.Empty;

	public abstract ushort GetMsgType
	{
		get;
	}

	[JsonIgnore]
	public string m_strUserID
	{
		get
		{
			if (_strUserID == string.Empty)
			{
				_strUserID = LocalSave.Instance.GetUserID();
			}
			return _strUserID;
		}
		set
		{
			_strUserID = value;
		}
	}

	[JsonIgnore]
	public ushort m_nSoftVersion
	{
		get
		{
			if (_nSoftVersion == 0)
			{
				_nSoftVersion = PlatformHelper.GetAppVersionCode();
			}
			return _nSoftVersion;
		}
		set
		{
			_nSoftVersion = value;
		}
	}

	[JsonIgnore]
	public string m_strDeviceID
	{
		get
		{
			if (_strDeviceID == string.Empty)
			{
				_strDeviceID = PlatformHelper.GetUUID();
			}
			return _strDeviceID;
		}
		set
		{
			_strDeviceID = value;
		}
	}

	[JsonIgnore]
	public string m_strAccessToken
	{
		get
		{
			if (_strAccessToken == string.Empty)
			{
				_strAccessToken = LocalSave.Instance.GetServerUserID().ToString();
			}
			return _strAccessToken;
		}
		set
		{
			_strAccessToken = value;
		}
	}

	public void ReadFromStream(BinaryReader reader)
	{
		m_strUserID = reader.ReadString();
		m_nSoftVersion = reader.ReadUInt16();
		m_strDeviceID = reader.ReadString();
		m_strAccessToken = reader.ReadString();
		OnReadFromStream(reader);
	}

	protected abstract void OnReadFromStream(BinaryReader reader);

	public void WriteToStream(BinaryWriter writer)
	{
		writer.Write(m_strUserID);
		writer.Write(m_nSoftVersion);
		writer.Write(m_strDeviceID);
		writer.Write(m_strAccessToken);
		OnWriteToStream(writer);
	}

	protected abstract void OnWriteToStream(BinaryWriter writer);

	public virtual byte[] buildPacket()
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
}
