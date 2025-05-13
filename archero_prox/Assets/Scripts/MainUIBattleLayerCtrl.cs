




using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class MainUIBattleLayerCtrl : MonoBehaviour
{
	public const string BoxAniString = "BoxChestRotating";

	public Text Text_Stage;

	public CurrencyExpCtrl mExpCtrl;

	public ButtonCtrl Button_Layer;

	public RedNodeCtrl mRedCtrl;

	public RectTransform BoxTran;

	public Animation BoxAni;

	public Text Text_StageCount;

	public Action OnLayerClick;

	private bool bEnable;

	private int mMax;

	private void Awake()
	{
		Button_Layer.SetDepondNet(value: true);
		Button_Layer.onClick = delegate
		{
			if (OnLayerClick != null)
			{
				OnLayerClick();
			}
		};
		DOTween.Sequence().AppendInterval(GameLogic.Random(0f, 1f)).AppendCallback(delegate
		{
			RectTransform boxTran = BoxTran;
			Vector2 anchoredPosition = BoxTran.anchoredPosition;
			boxTran.DOAnchorPosY(anchoredPosition.y + 7f, 1.4f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
		});
	}

	public void SetLayer(int current, int max)
	{
		mMax = max;
		bEnable = (current >= max);
		UpdateStageCount();
		int openCount = LocalModelManager.Instance.Box_ChapterBox.GetOpenCount(current, LocalSave.Instance.mStage.BoxLayerID);
		mRedCtrl.SetType(RedNodeType.eRedCount);
		mRedCtrl.Value = openCount;
		BoxAni.enabled = (openCount > 0);
		if (BoxAni.enabled)
		{
			BoxAni.Play("BoxChestRotating");
		}
		else
		{
			BoxAni.transform.localRotation = Quaternion.identity;
			BoxAni.transform.localScale = Vector3.one;
		}
		UpdateNet();
	}

	private void UpdateStageCount()
	{
		string stageLayer = GameLogic.Hold.Language.GetStageLayer(mMax);
		LocalSave.Instance.mStage.GetLayerBoxStageLayer(mMax, out int stage, out int layer);
		Text_StageCount.text = GameLogic.Hold.Language.GetLanguageByTID("Main_StageCount", Utils.FormatString("{0}-{1}", stage, layer));
		if (mMax > 999999)
		{
			Text_StageCount.gameObject.SetActive(value: false);
		}
	}

	public void UpdateNet()
	{
	}

	public void OnLanguageChange()
	{
		Text_Stage.text = GameLogic.Hold.Language.GetLanguageByTID("Main_StageChest");
		UpdateStageCount();
	}
}
