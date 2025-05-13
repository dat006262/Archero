




using Dxx.UI;
using UnityEngine;
using UnityEngine.UI;

public class Test_GridInfinity : MonoBehaviour
{
	public InfinityScrollGroup infinity;

	public GameObject copyItem;

	public int initItemCount;

	public int initDisplayCount;

	public InputField inputItemIndex;

	private void Start()
	{
		infinity.RegUpdateCallback<Button>(UpdateChildCallbak);
		infinity.Init(initDisplayCount, initItemCount, copyItem);
	}

	public void OnClickDisplayCountPlus()
	{
	}

	public void OnClickDisplayCountMinus()
	{
	}

	public void OnClickScrollTo()
	{
		int result = 0;
		if (!string.IsNullOrEmpty(inputItemIndex.text) && int.TryParse(inputItemIndex.text, out result))
		{
			infinity.ScrollToItem(result);
		}
	}

	public void OnClickRefresh()
	{
		infinity.RefreshAll();
	}

	public void OnClickItemCountPlus()
	{
		infinity.SetItemCount(++initItemCount);
	}

	public void OnClickItemCountMinus()
	{
		infinity.SetItemCount(--initItemCount);
	}

	private void UpdateChildCallbak(int index, Button obj)
	{
		Text component = obj.transform.Find("Text").GetComponent<Text>();
		component.text = $"{index:D4}";
		UnityEngine.Debug.LogFormat("UpdateChildCallbak: {0}", index);
	}
}
