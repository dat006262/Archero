




using DG.Tweening;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenGetCtrl : MonoBehaviour
{
	public const float ItemWidth = 150f;

	public const float ItemHeight = 150f;

	public const int LineCount = 4;

	public GameObject child;

	public Text Text_Title;

	public Transform getparent;

	private GameObject _copyitem;

	private LocalUnityObjctPool mPool;

	private GameObject copyitem
	{
		get
		{
			if (_copyitem == null)
			{
				_copyitem = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/BoxGetUI/BoxGetResult"));
				_copyitem.SetParentNormal(getparent);
				_copyitem.SetActive(value: false);
			}
			return _copyitem;
		}
	}

	private void Awake()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<BoxOpenGetOne>(copyitem);
	}

	public Sequence Init(List<Drop_DropModel.DropData> list)
	{
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("BoxOpen_GetTitle");
		Text_Title.transform.localScale = Vector3.one * 1.6f;
		Sequence sequence = DOTween.Sequence();
		sequence.Append(Text_Title.transform.DOScale(1f, 0.3f));
		mPool.Collect<BoxOpenGetOne>();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			int index = i;
			sequence.AppendCallback(delegate
			{
				BoxOpenGetOne boxOpenGetOne = mPool.DeQueue<BoxOpenGetOne>();
				RectTransform rectTransform = boxOpenGetOne.transform as RectTransform;
				rectTransform.SetParentNormal(getparent);
				if (count <= 4)
				{
					float x = (float)index * 150f - 75f * (float)(count - 1);
					rectTransform.anchoredPosition = new Vector2(x, 0f);
				}
				else
				{
					rectTransform.anchoredPosition = new Vector2((float)(index % 4) * 150f - 225f, (float)(index / 4) * -150f);
				}
				boxOpenGetOne.Init(list[index]);
				boxOpenGetOne.mCanvasGroup.alpha = 0f;
				rectTransform.localScale = Vector3.zero;
				DOTween.Sequence().Append(boxOpenGetOne.mCanvasGroup.DOFade(1f, 0.3f)).Join(rectTransform.DOScale(1f, 0.3f));
			});
			sequence.AppendInterval(0.2f);
		}
		return sequence;
	}

	public void Show(bool value)
	{
		child.SetActive(value);
	}
}
