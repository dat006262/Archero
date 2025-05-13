




using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AdInsideUICtrl : MediatorCtrlBase
{
	public ButtonCtrl Button_shadow;

	public RawImage image;

	public VideoPlayer mPlayer;

	public AudioSource mAudioSource;

	public AdInsideTimeCtrl mTimeCtrl;

	private AdInsideProxy.Transfer mTransfer;

	private bool bSoundOpen;

	private bool bMusicOpen;

	private float updatetime;

	protected override void OnInit()
	{
		Button_shadow.onClick = delegate
		{
			RateUrlManager.OpenAdUrl();
		};
	}

	private void OnLoopPointReached(VideoPlayer video)
	{
		WindowUI.CloseWindow(WindowID.WindowID_AdInside);
		if (mTransfer != null)
		{
			mTransfer.finish_callback();
		}
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("AdInsideProxy");
		if (proxy == null || proxy.Data == null || !(proxy.Data is AdInsideProxy.Transfer))
		{
			SdkManager.Bugly_Report("AdInsideUICtrl", "AdInsideProxy is invalid.");
			OnLoopPointReached(null);
			return;
		}
		updatetime = 0f;
		GameLogic.Hold.Sound.PauseBackgroundMusic();
		mTransfer = (proxy.Data as AdInsideProxy.Transfer);
		mTimeCtrl.SetMax((float)mPlayer.clip.length);
		InitUI();
	}

	private IEnumerator play_video()
	{
		mPlayer.EnableAudioTrack(0, enabled: true);
		mPlayer.SetTargetAudioSource(0, mAudioSource);
		mPlayer.Prepare();
		while (!mPlayer.isPrepared)
		{
			yield return null;
		}
		mPlayer.Play();
		mAudioSource.Play();
		while (mPlayer.isPlaying)
		{
			yield return null;
		}
	}

	private void InitUI()
	{
		mPlayer.loopPointReached += OnLoopPointReached;
		StartCoroutine(play_video());
	}

	private void Update()
	{
		if ((bool)mPlayer && (bool)mPlayer.texture && (bool)image)
		{
			updatetime += Time.unscaledDeltaTime;
			image.texture = mPlayer.texture;
			mTimeCtrl.SetCurrent((float)mPlayer.time);
		}
	}

	protected override void OnClose()
	{
		GameLogic.Hold.Sound.ResumeBackgroundMusic();
		mPlayer.loopPointReached -= OnLoopPointReached;
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
	}

	public override void OnLanguageChange()
	{
	}
}
