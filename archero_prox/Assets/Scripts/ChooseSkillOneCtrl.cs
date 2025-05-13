




using PureMVC.Patterns;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSkillOneCtrl : MonoBehaviour
{
	private int skillid;

	private Text text;

	private Image image;

	private int index;

	private RectTransform rectt;

	private float allspeed;

	private int endindex;

	private float endposy;

	private bool bStart;

	private Action mRandomName;

	private int mColumn;

	private int mCount;

	private const float time = 0.1f;

	private float movetime;

	private bool bLast;

	private bool bRevert;

	private int mRevertState;

	private float mEndPosY;

	private void Awake()
	{
		index = int.Parse(base.name);
		image = base.transform.GetComponent<Image>();
		rectt = (base.transform as RectTransform);
	}

	public void Init(int skillid, Text text)
	{
		allspeed = 0f;
		this.skillid = skillid;
		int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(skillid).SkillIcon;
		image.sprite = SpriteManager.GetSkillIcon(skillIcon);
		this.text = text;
		Modify();
	}

	private void Modify()
	{
		float y = (1 - index) * 180;
		RectTransform rectTransform = rectt;
		Vector2 anchoredPosition = rectt.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(anchoredPosition.x, y);
	}

	public void OnClick()
	{
		if (endindex == 1)
		{
			GameLogic.Self.LearnSkill(skillid);
			LocalSave.Instance.BattleIn_UpdateLearnSkill(skillid);
			Facade.Instance.SendNotification("BATTLE_CHOOSESKILL_SKILL_CHOOSE", skillid);
		}
	}

	private void Update()
	{
		ModifyPositionY();
		float num = Time.unscaledDeltaTime / 0.1f * 180f;
		if (bStart)
		{
			Vector2 anchoredPosition = rectt.anchoredPosition;
			float y = anchoredPosition.y;
			Vector2 anchoredPosition2 = rectt.anchoredPosition;
			float num2 = anchoredPosition2.y - num;
			RectTransform rectTransform = rectt;
			Vector2 anchoredPosition3 = rectt.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(anchoredPosition3.x, num2);
			ModifyPositionY();
			if (y * num2 <= 0f)
			{
				GameLogic.Hold.Sound.PlayUI(1000005);
			}
			allspeed += num;
			while (allspeed >= 180f)
			{
				allspeed -= 180f;
				mCount--;
			}
			if (Time.frameCount % 3 == 0)
			{
				mRandomName();
			}
			if (mCount <= 0)
			{
				bStart = false;
				bLast = true;
			}
		}
		else if (bLast)
		{
			if (endindex == 1)
			{
				bLast = false;
				bRevert = true;
				text.text = GameLogic.Hold.Language.GetSkillName(skillid);
				GameLogic.Hold.Sound.PlayUI(1000006);
				if (mColumn == 2)
				{
					Facade.Instance.SendNotification("BATTLE_CHOOSESKILL_ACTION_END");
				}
			}
			else
			{
				bLast = false;
				RectTransform rectTransform2 = rectt;
				Vector2 anchoredPosition4 = rectt.anchoredPosition;
				rectTransform2.anchoredPosition = new Vector2(anchoredPosition4.x, endposy);
			}
		}
		else
		{
			if (!bRevert)
			{
				return;
			}
			if (mRevertState == 0)
			{
				RectTransform rectTransform3 = rectt;
				Vector2 anchoredPosition5 = rectt.anchoredPosition;
				float x = anchoredPosition5.x;
				Vector2 anchoredPosition6 = rectt.anchoredPosition;
				rectTransform3.anchoredPosition = new Vector2(x, anchoredPosition6.y - num);
				Vector2 anchoredPosition7 = rectt.anchoredPosition;
				if (anchoredPosition7.y < endposy - 54.0000038f)
				{
					mRevertState = 1;
				}
			}
			else if (mRevertState == 1)
			{
				num *= 0.5f;
				Vector2 anchoredPosition8 = rectt.anchoredPosition;
				float num3 = anchoredPosition8.y + num;
				if (num3 >= endposy || num < 0.001f)
				{
					num3 = endposy;
					bRevert = false;
					mRevertState = 0;
				}
				RectTransform rectTransform4 = rectt;
				Vector2 anchoredPosition9 = rectt.anchoredPosition;
				rectTransform4.anchoredPosition = new Vector2(anchoredPosition9.x, num3);
			}
		}
	}

	private void ModifyPositionY()
	{
		while (true)
		{
			Vector2 anchoredPosition = rectt.anchoredPosition;
			if (anchoredPosition.y <= -180f)
			{
				RectTransform rectTransform = rectt;
				Vector2 anchoredPosition2 = rectt.anchoredPosition;
				float x = anchoredPosition2.x;
				Vector2 anchoredPosition3 = rectt.anchoredPosition;
				rectTransform.anchoredPosition = new Vector2(x, anchoredPosition3.y + 540f);
				continue;
			}
			break;
		}
	}

	public void AddAction(int column, int count, Action randomname)
	{
		mRandomName = randomname;
		mColumn = column;
		mCount = count;
		bStart = true;
		bLast = false;
		bRevert = false;
		mRevertState = 0;
		movetime = 0f;
		mEndPosY = 0f;
		endindex = (mCount + index) % 3;
		endposy = (1 - endindex) * 180;
	}
}
