




using System;
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

		public LocalBean()
		{
			position = 0;
		}

		public int readFromBytes(byte[] raws, int startPos)
		{
			this.raws = raws;
			position = startPos;
			try
			{
				messageLength = readShort();
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

		protected short readShort()
		{
			readBytes(datas_short, 2);
			short network = BitConverter.ToInt16(datas_short, 0);
			return IPAddress.NetworkToHostOrder(network);
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

		protected int readInt()
		{
			readBytes(datas_int, 4);
			datas_int_i = BitConverter.ToInt32(datas_int, 0);
			datas_int_i = IPAddress.NetworkToHostOrder(datas_int_i);
			return datas_int_i;
		}

		protected long readLong()
		{
			readBytes(datas_long, 8);
			long network = BitConverter.ToInt64(datas_long, 0);
			return IPAddress.NetworkToHostOrder(network);
		}

		protected DateTime readDate()
		{
			long num = readLong();
			num *= time_factor;
			num += t19700101;
			return new DateTime(num);
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

		protected double readDouble()
		{
			byte[] array = new byte[8];
			readBytes(array, 8);
			long network = BitConverter.ToInt64(array, 0);
			network = IPAddress.NetworkToHostOrder(network);
			return BitConverter.Int64BitsToDouble(network);
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
	}
}
