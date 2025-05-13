




using System;
using System.Collections.Generic;

public class TimeClass
{
	private float StartTime;

	private float DelayTime;

	private Action callback;

	private List<TimeClass> m_TimeList = new List<TimeClass>();

	private List<Action> m_TimePerFrameList = new List<Action>();

	private int TimeCount;

	private int TimePerFrameCount;

	private bool IsDelayOver(float CurrentTime)
	{
		return CurrentTime - StartTime >= DelayTime;
	}

	public void StartCallBack(float AliveTime, float DelayTime, Action callback)
	{
		TimeClass timeClass = new TimeClass();
		timeClass.StartTime = AliveTime;
		timeClass.DelayTime = DelayTime;
		timeClass.callback = callback;
		m_TimeList.Add(timeClass);
		TimeCount = m_TimeList.Count;
	}

	public void StartCallBack(float AliveTime, Action callback)
	{
		StartCallBack(AliveTime, 0f, callback);
	}

	public void RemoveCallBack(Action callback)
	{
		int num = 0;
		while (true)
		{
			if (num < TimeCount)
			{
				if (m_TimeList[num].callback == callback)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		m_TimeList.RemoveAt(num);
		TimeCount--;
	}

	public bool IsCallBackOver(Action callback)
	{
		for (int i = 0; i < TimeCount; i++)
		{
			if (m_TimeList[i].callback == callback)
			{
				return false;
			}
		}
		return true;
	}

	public void UpdateTimeClass(float AliveTime)
	{
		if (TimeCount > 0)
		{
			for (int num = TimeCount - 1; num >= 0; num--)
			{
				if (m_TimeList[num].IsDelayOver(AliveTime))
				{
					m_TimeList[num].callback();
					m_TimeList.RemoveAt(num);
					TimeCount--;
				}
			}
		}
		if (TimePerFrameCount > 0)
		{
			for (int i = 0; i < TimePerFrameCount; i++)
			{
				m_TimePerFrameList[i]();
			}
		}
	}

	public void StartPerFrame(Action callback)
	{
		StopPerFrame(callback);
		m_TimePerFrameList.Add(callback);
		TimePerFrameCount++;
	}

	public void StopPerFrame(Action callback)
	{
		if (m_TimePerFrameList.Contains(callback))
		{
			m_TimePerFrameList.Remove(callback);
			TimePerFrameCount--;
		}
	}

	public void Reset()
	{
		m_TimeList.Clear();
		m_TimePerFrameList.Clear();
		TimeCount = 0;
		TimePerFrameCount = 0;
	}
}
