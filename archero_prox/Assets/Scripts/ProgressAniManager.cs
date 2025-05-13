




using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;

public class ProgressAniManager
{
	public class ProgressTransfer
	{
		private float _currentvalue;

		public float changevalue;

		public float maxvalue;

		public int currentlevel;

		public bool islevelup;

		public bool isend;

		public Dictionary<int, float> levellist;

		public float currentvalue
		{
			get
			{
				return _currentvalue;
			}
			set
			{
				if (value != 0f)
				{
					float num = value - _currentvalue;
					changevalue += num;
				}
				_currentvalue = value;
			}
		}

		public float percent
		{
			get
			{
				float num = getcurrentmax();
				if (num > 0f)
				{
					return currentvalue / num;
				}
				return 1f;
			}
		}

		public ProgressTransfer(int currentlevel, float currentvalue)
		{
			init(currentlevel, currentvalue);
		}

		public void init(int currentlevel, float currentvalue)
		{
			this.currentlevel = currentlevel;
			this.currentvalue = currentvalue;
			init();
		}

		public void init()
		{
			islevelup = false;
			isend = true;
			clear();
		}

		public void clear()
		{
			changevalue = 0f;
		}

		public void levelup()
		{
			currentlevel++;
		}

		private float getcurrentmax()
		{
			if (levellist.TryGetValue(currentlevel, out float value))
			{
				return value * 100f;
			}
			return -1f;
		}

		public override string ToString()
		{
			return Utils.FormatString("level:{0} value:{1} percent:{2} islevelup:{3}", currentlevel, currentvalue, percent, islevelup);
		}
	}

	public int currentlevel;

	public float currentvalue;

	public Dictionary<int, float> levellist;

	public float progresstime = 1f;

	private float addvalue;

	private float currentaddvalue;

	private ProgressTransfer mTransfer = new ProgressTransfer(1, 0f);

	private Sequence seq;

	private Action<ProgressTransfer> updatecallback;

	public void Init(int currentlevel, float currentvalue, Dictionary<int, float> levellist)
	{
		this.currentlevel = currentlevel;
		this.currentvalue = currentvalue;
		this.levellist = levellist;
		mTransfer.currentvalue = this.currentvalue;
		mTransfer.currentlevel = this.currentlevel;
		mTransfer.levellist = this.levellist;
		mTransfer.init();
	}

	public void Deinit()
	{
		KillSequence();
	}

	public void SetUpdate(Action<ProgressTransfer> updatecallback)
	{
		this.updatecallback = updatecallback;
	}

	public Sequence Play(float add)
	{
		add *= 100f;
		currentaddvalue -= mTransfer.changevalue;
		currentaddvalue += add;
		addvalue = currentaddvalue;
		currentlevel = mTransfer.currentlevel;
		currentvalue = mTransfer.currentvalue;
		mTransfer.clear();
		KillSequence();
		seq = DOTween.Sequence();
		float num = getcurrentmax();
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		while (currentvalue + addvalue >= num)
		{
			num3 = 1f;
			num2 = currentvalue / num;
			currentlevel++;
			num5 = (num3 - num2) * progresstime;
			num4 += num5;
			if (seq != null)
			{
				int num6 = currentlevel;
				seq.Append(DOTween.To(() => mTransfer.currentvalue, delegate(float x)
				{
					mTransfer.currentvalue = x;
				}, num, num5).OnUpdate(delegate
				{
					mTransfer.islevelup = false;
					mTransfer.isend = false;
					if (updatecallback != null)
					{
						updatecallback(mTransfer);
					}
				}));
				seq.AppendCallback(delegate
				{
					mTransfer.islevelup = true;
					mTransfer.isend = false;
					mTransfer.levelup();
					if (updatecallback != null)
					{
						updatecallback(mTransfer);
					}
				});
				seq.AppendCallback(delegate
				{
					mTransfer.islevelup = false;
					mTransfer.currentvalue = 0f;
					if (updatecallback != null)
					{
						updatecallback(mTransfer);
					}
				});
			}
			addvalue -= num - currentvalue;
			currentvalue = 0f;
			num = getcurrentmax();
		}
		if (currentlevel >= GameLogic.Self.m_EntityData.MaxLevel)
		{
			seq.AppendCallback(delegate
			{
				mTransfer.currentvalue = 1f;
				mTransfer.islevelup = false;
				mTransfer.isend = false;
				if (updatecallback != null)
				{
					updatecallback(mTransfer);
				}
			});
		}
		else
		{
			num2 = currentvalue / num;
			num3 = (currentvalue + addvalue) / num;
			num5 = (num3 - num2) * progresstime;
			num4 += num5;
			if (seq != null)
			{
				seq.Append(DOTween.To(() => mTransfer.currentvalue, delegate(float x)
				{
					mTransfer.currentvalue = x;
				}, currentvalue + addvalue, num5).OnUpdate(delegate
				{
					mTransfer.islevelup = false;
					mTransfer.isend = false;
					if (updatecallback != null)
					{
						updatecallback(mTransfer);
					}
				}));
				seq.AppendCallback(delegate
				{
					mTransfer.islevelup = false;
					mTransfer.isend = true;
					if (updatecallback != null)
					{
						updatecallback(mTransfer);
					}
				});
			}
		}
		return seq;
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	private float getcurrentmax()
	{
		if (levellist.TryGetValue(currentlevel, out float value))
		{
			return value * 100f;
		}
		return -1f;
	}
}
