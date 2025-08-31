




using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace TableTool
{
	public abstract class LocalBean
	{
		private short messageLength;

		private int position;
		
		private int positionWrite;

		private static readonly long t19700101 = new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;

		private static readonly int time_factor = 10000;

		private static readonly Encoding encoding = Encoding.UTF8;

		private FileStream file;

		private byte[] raws;

		private byte[] datas_short = new byte[2];

		private byte[] datas_int = new byte[4];

		private int datas_int_i;

		private byte[] datas_long = new byte[8];

		private byte[] datas_float = new byte[4];

		private int datas_float_i;

		private float datas_float_f;

		private short count_arraystring;

		private short count_arraybool;

		private short count_arrayfloat;

		protected List<byte> byteList;//Use To Create FileStream

		public LocalBean()
		{
			position      = 0;
			positionWrite = 0;
		}

		public int readFromBytes(byte[] raws, int startPos)
		{
			this.raws = raws;
			position = startPos;
			try
			{
				messageLength = readShort();
//				Debug.Log("messageLength"+messageLength);
				if (!ReadImpl())
				{
					return -1;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return position;
		}
		public (byte[],int) writeToBytes(int startPos)
		{
			byteList = new List<byte>();
			positionWrite = startPos;
			{
				writeShort(0);
				List<byte> contentBytes  = WriteImpl();
			
//				Debug.Log("contentBytes"+contentBytes.Count);
				short  network = IPAddress.HostToNetworkOrder((short)contentBytes.Count);;
				byte[] bytes   = BitConverter.GetBytes(network);
				byteList[0] = bytes[0];
				byteList[1] = bytes[1];

			}
			return (byteList.ToArray(),positionWrite);
		}


		public int getLength()
		{
			return messageLength;
		}

		protected void readBytes(byte[] datas, int buffLength)
		{
			int num = 0;
			while (num < buffLength)
			{
				datas[num] = raws[position];
				num++;
				position++;
			}
		}

		protected void writeByte(byte[] datas, int buffLength)
		{
			if (datas == null || buffLength <= 0)
				return;

			// Đảm bảo không vượt quá độ dài của mảng
			int actualCount = Math.Min(buffLength, datas.Length);

			// Tạo mảng con chứa actualCount phần tử đầu tiên
			byte[] subArray = new byte[actualCount];
			Array.Copy(datas, subArray, actualCount);


			byteList.AddRange(datas);

			// Cập nhật vị trí ghi
			positionWrite += datas.Length;

		}
		protected short readShort()
		{
			readBytes(datas_short, 2);
			short network = BitConverter.ToInt16(datas_short, 0);
			return IPAddress.NetworkToHostOrder(network);
		}
		protected void writeShort(short value)
		{
		
			short  network = IPAddress.HostToNetworkOrder(value);
			byte[] bytes   = BitConverter.GetBytes(network);  
			writeByte(bytes,2);
		}




		protected bool readBool()
		{
			short num = readShort();
			if (num == 1)
			{
				return true;
			}
			return false;
		}
		protected void writeBool( bool value)
		{
			writeShort( (short)(value ? 1 : 0));
		}


		
		protected int readInt()
		{
			readBytes(datas_int, 4);
			datas_int_i = BitConverter.ToInt32(datas_int, 0);
			datas_int_i = IPAddress.NetworkToHostOrder(datas_int_i);
			return datas_int_i;
		}
		protected void writeInt( int value)
		{
			int    network = IPAddress.HostToNetworkOrder(value);
			byte[] bytes   = BitConverter.GetBytes(network);
		//	Debug.Log("Gia tri int luu"+value+ "Length"+ bytes.Length+ "bytes[0]"+bytes[0]+ "bytes[1]"+bytes[1]+ "bytes[2]"+bytes[2]+ "bytes[3]"+bytes[3] );;;;
			//byteList.AddRange(bytes);

			writeByte(bytes,4);
		}



		protected long readLong()
		{
			readBytes(datas_long, 8);
			long network = BitConverter.ToInt64(datas_long, 0);
			return IPAddress.NetworkToHostOrder(network);
		}
		protected void writeLong( long value)
		{
			long   network = IPAddress.HostToNetworkOrder(value);
			byte[] bytes   = BitConverter.GetBytes(network);       // Chuyển thành mảng byte
			writeByte(bytes,8);
		}




		protected DateTime readDate()
		{
			long num = readLong();
			num *= time_factor;
			num += t19700101;
			return new DateTime(num);
		}
		
		protected void writeDate( DateTime date)
		{
			long ticks = (date.Ticks - t19700101) / time_factor;
			writeLong( ticks);
		}


		protected float readFloat()
		{
			readBytes(datas_float, 4);
			datas_float_i = BitConverter.ToInt32(datas_float, 0);
			datas_float_i = IPAddress.NetworkToHostOrder(datas_float_i);
			byte[] bytes = BitConverter.GetBytes(datas_float_i);
			datas_float_f = BitConverter.ToSingle(bytes, 0);
			return datas_float_f;
		}

		protected void writeFloat(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			int intValue = BitConverter.ToInt32(bytes, 0);
			intValue = IPAddress.HostToNetworkOrder(intValue);
			byte[] networkBytes = BitConverter.GetBytes(intValue);
			writeByte(networkBytes,8);

		}



		protected double readDouble()
		{
			byte[] array = new byte[8];
			readBytes(array, 8);
			long network = BitConverter.ToInt64(array, 0);
			network = IPAddress.NetworkToHostOrder(network);
			return BitConverter.Int64BitsToDouble(network);
		}
	
		
	
		protected void writeDouble(double value)
		{
			long bits = BitConverter.DoubleToInt64Bits(value);
			writeLong(bits);
		}
		


		
		protected int[] readArrayint()
		{
			short num = readShort();
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = readInt();
			}
			return array;
		}

		protected void writeArrayInt(int[] array)
		{
			writeShort( (short)array.Length);
			foreach (int val in array)
				writeInt(val);
		}

		protected string[] readArraystring()
		{
			count_arraystring = readShort();
			string[] array = new string[count_arraystring];
			for (int i = 0; i < count_arraystring; i++)
			{
				array[i] = readLocalString();
			}
			return array;
		}

		protected void writeArrayString( string[] array)
		{
			writeShort( (short)array.Length);
			foreach (string str in array)
				writeLocalString( str);
		}

		protected double[] readArraydouble()
		{
			short num = readShort();
			double[] array = new double[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = readDouble();
			}
			return array;
		}
		protected void writeArrayDouble( double[] array)
		{
			writeShort((short)array.Length);
			foreach (double val in array)
				writeDouble(val);
		}



		protected bool[] readArraybool()
		{
			count_arraybool = readShort();
			bool[] array = new bool[count_arraybool];
			for (int i = 0; i < count_arraybool; i++)
			{
				array[i] = readBool();
			}
			return array;
		}

		protected void writeArrayBool( bool[] array)
		{
			writeShort((short)array.Length);
			foreach (bool val in array)
				writeBool(val);
		}

		protected float[] readArrayfloat()
		{
			count_arrayfloat = readShort();
			float[] array = new float[count_arrayfloat];
			for (int i = 0; i < count_arrayfloat; i++)
			{
				array[i] = readFloat();
			}
			return array;
		}
		protected void writeArrayFloat( float[] array)
		{
			writeShort((short)array.Length);
			foreach (float val in array)
				writeFloat(val);
		}

		protected short[] readArrayshort()
		{
			short num = readShort();
			short[] array = new short[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = readShort();
			}
			return array;
		}
		protected void writeArrayShort(short[] array)
		{
			writeShort( (short)array.Length);
			foreach (short val in array)
				writeShort(val);
		}


		protected long[] readArraylong()
		{
			short num = readShort();
			long[] array = new long[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = readLong();
			}
			return array;
		}
		protected void writeArrayLong(  long[] array)
		{
			writeShort((short)array.Length);
			foreach (long val in array)
				writeLong(val);
		}


		protected string readLocalString()
		{
			short num = readShort();
			byte[] array = new byte[num - 2];
			readBytes(array, num - 2);
			try
			{
				return encoding.GetString(array);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("get string ecode error " + ex.Message);
				return string.Empty;
			}
		}
		protected void writeLocalString(string value)
		{
			if (value == null)
				value = string.Empty;

			byte[] stringBytes = encoding.GetBytes(value);

			short totalLength = (short)(stringBytes.Length + 2);

			writeShort(totalLength);
			writeByte(stringBytes, stringBytes.Length);
		}



		protected string readCommonString()
		{
			string key = readLocalString();
			return toCommonString(key);
		}

		protected string toCommonString(string key)
		{
			if (key != null)
			{
				return key;
			}
			return key;
		}

		protected abstract bool ReadImpl();

		protected virtual List<byte> WriteImpl()
		{
			

			return byteList;
		}

	}
}
