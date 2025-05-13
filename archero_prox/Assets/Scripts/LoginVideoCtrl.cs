




using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LoginVideoCtrl : MonoBehaviour
{
	public RawImage image;

	public VideoPlayer mPlayer;

	public Action OnPlayEnd;

	private void Start()
	{
		mPlayer.loopPointReached += OnLoopPointReached;
	}

	private void OnLoopPointReached(VideoPlayer video)
	{
		OnPlayEnd();
	}

	private void Update()
	{
		if ((bool)mPlayer && (bool)mPlayer.texture && (bool)image)
		{
			image.texture = mPlayer.texture;
		}
	}
}
