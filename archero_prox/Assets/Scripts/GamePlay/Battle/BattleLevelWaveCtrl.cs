




using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class BattleLevelWaveCtrl : MonoBehaviour
{
	public GameObject child;

	public Text Text_Wave;

	private BattleLevelWaveData mData;

	private float starttime;

	public void SetActive(bool value)
	{
		child.SetActive(value);
	}

	public void SetInfo(BattleLevelWaveData data)
	{
		mData = data;
		starttime = Updater.AliveTime;
		SetActive(data.showui);
		if (data.showui)
		{
			Update();
		}
	}

	private void set_time(int time)
	{
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("battle_level_wave");
		string languageByTID2 = GameLogic.Hold.Language.GetLanguageByTID("battle_level_nexttime");
		Text_Wave.text = Utils.FormatString("{0}:{1}/{2}     {3}:{4}", languageByTID, mData.currentwave, mData.maxwave, languageByTID2, GameLogic.Hold.Language.GetSecond(time));
		if (time < 0)
		{
			mData.currentwave++;
			if (mData.is_last_wave())
			{
				mData.showui = false;
				SetActive(value: false);
			}
			else
			{
				SetInfo(mData);
			}
		}
	}

	private void Update()
	{
		if (mData.showui)
		{
			float num = Updater.AliveTime - starttime;
			float value = (float)mData.lasttime - num;
			set_time(MathDxx.CeilToInt(value));
		}
	}

	public void Deinit()
	{
	}
}
