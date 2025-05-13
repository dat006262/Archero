




using DG.Tweening;
using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class BattleMatchDefenceTime_InfoCtrl : MonoBehaviour
{
	private const string aniname = "Card_InfoShow";

	public GameObject child;

	public Animation ani;

	public Text Text_Content;

	public Image Image_Icon;

	private Sequence seq;

	public void ShowInfo(string eventname, object body = null)
	{
		switch (eventname)
		{
		case "MatchDefenceTime_other_learn_skill":
		{
			Image_Icon.enabled = true;
			Text_Content.text = Utils.FormatString("学习了");
			int skillid = (int)body;
			Image_Icon.sprite = SpriteManager.GetSkillIconByID(skillid);
			float num = 0f;
			float preferredWidth = Text_Content.preferredWidth;
			Vector2 sizeDelta = Image_Icon.rectTransform.sizeDelta;
			float num2 = preferredWidth + sizeDelta.x + num;
			float num3 = (0f - num2) / 2f + Text_Content.preferredWidth / 2f;
			float num4 = num3 + Text_Content.preferredWidth / 2f;
			Vector2 sizeDelta2 = Image_Icon.rectTransform.sizeDelta;
			float x = num4 + sizeDelta2.x / 2f + num;
			Text_Content.rectTransform.anchoredPosition = new Vector2(num3, 0f);
			Image_Icon.rectTransform.anchoredPosition = new Vector2(x, 0f);
			break;
		}
		case "MatchDefenceTime_other_dead":
			Image_Icon.enabled = false;
			Text_Content.rectTransform.anchoredPosition = new Vector2(0f, 0f);
			Text_Content.text = Utils.FormatString("死了");
			break;
		case "MatchDefenceTime_other_reborn":
			Image_Icon.enabled = false;
			Text_Content.rectTransform.anchoredPosition = new Vector2(0f, 0f);
			Text_Content.text = Utils.FormatString("复活了");
			break;
		}
		KillSequence();
		seq = DOTween.Sequence();
		seq.AppendInterval(3f);
		seq.AppendCallback(delegate
		{
			show(value: false);
		});
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	private void show(bool value)
	{
		if (value)
		{
			ani["Card_InfoShow"].time = 0f;
			ani["Card_InfoShow"].speed = 1f;
		}
		else
		{
			ani["Card_InfoShow"].time = ani["Card_InfoShow"].clip.length;
			ani["Card_InfoShow"].speed = -1f;
		}
		ani.Play("Card_InfoShow");
	}
}
