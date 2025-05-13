




using System;
using System.Collections.Generic;
using UnityEngine;

public class KeySpeedAnimation
{
	public class KeyInfo
	{
		public float spdFrom;

		public float spdTo;

		public float time;
	}

	private List<KeyInfo> keyList = new List<KeyInfo>();

	private int phaseIndex;

	private float timeCount;

	private float speed;

	private Action<int, float, float> onUpdate;

	public KeySpeedAnimation(List<KeyInfo> keyList, Action<int, float, float> onUpdate)
	{
		this.keyList = keyList;
		this.onUpdate = onUpdate;
		Reset();
	}

	public void Reset()
	{
		phaseIndex = 0;
		speed = 0f;
		timeCount = 0f;
	}

	public bool Update(float deltaTime)
	{
		if (phaseIndex >= keyList.Count)
		{
			return true;
		}
		timeCount += deltaTime;
		KeyInfo keyInfo = keyList[phaseIndex];
		if (timeCount < keyInfo.time)
		{
			speed = Mathf.Lerp(keyInfo.spdFrom, keyInfo.spdTo, timeCount / keyInfo.time);
			if (onUpdate != null)
			{
				onUpdate(phaseIndex, speed, deltaTime);
			}
		}
		else
		{
			timeCount -= keyInfo.time;
			phaseIndex++;
		}
		return false;
	}

	public float GetTotalLength()
	{
		float num = 0f;
		for (int i = 0; i < keyList.Count; i++)
		{
			float spdFrom = keyList[i].spdFrom;
			float time = keyList[i].time;
			float num2 = (keyList[i].spdTo - keyList[i].spdFrom) / keyList[i].time;
			num += spdFrom * time + 0.5f * num2 * time * time;
		}
		return num;
	}
}
