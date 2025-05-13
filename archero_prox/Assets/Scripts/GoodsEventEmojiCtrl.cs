




using UnityEngine;

public class GoodsEventEmojiCtrl : MonoBehaviour
{
	private const string EventEmojiNear = "EventEmojiNear";

	private TextMesh text;

	private Animation ani;

	private void Awake()
	{
		text = base.transform.Find("child/Text").GetComponent<TextMesh>();
		ani = base.transform.Find("child").GetComponent<Animation>();
		MeshRenderer component = text.GetComponent<MeshRenderer>();
		component.sortingLayerName = "SkillEffect";
		component.sortingOrder = 999;
	}

	private void OnEnable()
	{
		text.text = "?";
		if ((bool)ani)
		{
			ani.enabled = true;
		}
	}

	private void OnDisable()
	{
		if ((bool)ani)
		{
			ani.enabled = false;
		}
	}

	public void Near()
	{
		text.text = "!";
		ani["EventEmojiNear"].time = 0f;
		ani["EventEmojiNear"].speed = 1f;
		ani.Play("EventEmojiNear");
	}

	public void Far()
	{
		text.text = "?";
		ani["EventEmojiNear"].time = ani["EventEmojiNear"].clip.length;
		ani["EventEmojiNear"].speed = -1f;
		ani.Play("EventEmojiNear");
	}
}
