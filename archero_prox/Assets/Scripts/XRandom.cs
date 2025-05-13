




using Dxx.Util;
using System;

public class XRandom
{
	private long seed;

	private const long multiplier = 25214903917L;

	private const long addend = 11L;

	private const long mask = 281474976710655L;

	private const double DOUBLE_UNIT = 1.1102230246251565E-16;

	private const string BadBound = "bound must be positive";

	private static long _seedUniquifier = 8682522807148012L;

	private double nextNextGaussian;

	private bool haveNextNextGaussian;

	public XRandom()
		: this(seedUniquifier() ^ nanoTime())
	{
	}

	public XRandom(long seed)
	{
		if (GetType() == typeof(XRandom))
		{
			this.seed = initialScramble(seed);
			return;
		}
		this.seed = 0L;
		setSeed(seed);
	}

	public static long nanoTime()
	{
		return DateTime.Now.Ticks * 100;
	}

	private static long seedUniquifier()
	{
		long seedUniquifier;
		long num;
		do
		{
			seedUniquifier = _seedUniquifier;
			num = seedUniquifier * 181783497276652981L;
		}
		while (_seedUniquifier != seedUniquifier);
		_seedUniquifier = num;
		return num;
	}

	private static long initialScramble(long seed)
	{
		return (seed ^ 0x5DEECE66D) & 0xFFFFFFFFFFFF;
	}

	public void setSeed(long seed)
	{
		lock (this)
		{
			this.seed = initialScramble(seed);
			haveNextNextGaussian = false;
		}
	}

	protected int next(int bits)
	{
		bool flag = false;
		long value;
		do
		{
			long num = seed;
			value = ((num * 25214903917L + 11) & 0xFFFFFFFFFFFF);
			if (seed == num)
			{
				seed = value;
				flag = true;
			}
		}
		while (!flag);
		return (int)move_fill_0(value, 48 - bits);
	}

	public void nextBytes(byte[] bytes)
	{
		int num = 0;
		int num2 = bytes.Length;
		while (num < num2)
		{
			int num3 = nextInt();
			int num4 = Math.Min(num2 - num, 4);
			while (num4-- > 0)
			{
				bytes[num++] = (byte)num3;
				num3 >>= 8;
			}
		}
	}

	public int nextInt()
	{
		return next(32);
	}

	public int nextInt(int bound)
	{
		if (bound <= 0)
		{
			throw new ArgumentException("bound must be positive");
		}
		int num = next(31);
		int num2 = bound - 1;
		if ((bound & num2) == 0)
		{
			num = (int)((long)bound * (long)num >> 31);
		}
		else
		{
			int num3 = num;
			while (num3 - (num = num3 % bound) + num2 < 0)
			{
				num3 = next(31);
			}
		}
		return num;
	}

	public int nextInt(int min, int max)
	{
		if (min >= max)
		{
			return min;
		}
		return MathDxx.Abs(nextInt()) % (max - min) + min;
	}

	public long nextLong()
	{
		return ((long)next(32) << 32) + next(32);
	}

	public bool nextBoolean()
	{
		return next(1) != 0;
	}

	public float nextFloat()
	{
		return (float)next(24) / 16777216f;
	}

	public double nextDouble()
	{
		return (double)(((long)next(26) << 27) + next(27)) * 1.1102230246251565E-16;
	}

	public double nextGaussian()
	{
		lock (this)
		{
			if (haveNextNextGaussian)
			{
				haveNextNextGaussian = false;
				return nextNextGaussian;
			}
			double num;
			double num2;
			double num3;
			do
			{
				num = 2.0 * nextDouble() - 1.0;
				num2 = 2.0 * nextDouble() - 1.0;
				num3 = num * num + num2 * num2;
			}
			while (num3 >= 1.0 || num3 == 0.0);
			double num4 = Math.Sqrt(-2.0 * Math.Log(num3) / num3);
			nextNextGaussian = num2 * num4;
			haveNextNextGaussian = true;
			return num * num4;
		}
	}

	public static long move_fill_0(long value, int bits)
	{
		long num = long.MaxValue;
		for (int i = 0; i < bits; i++)
		{
			value >>= 1;
			value &= num;
		}
		return value;
	}
}
