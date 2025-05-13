




using DG.Tweening;
using Dxx.Util;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class GameTurnTableOneCtrl : MonoBehaviour
{
	public Transform child;

	public Image Image_Icon;

	public Text Text_Value;

	public TurnTableData mData
	{
		get;
		private set;
	}

	private void Awake()
	{
	}

	public void Init(TurnTableData data)
	{
		mData = data;
		child.localScale = Vector3.one;
		Text_Value.text = string.Empty;
		if (LocalSave.QualityColors.ContainsKey(data.quality))
		{
			Text_Value.color = LocalSave.QualityColors[data.quality];
		}
		switch (data.type)
		{
		case TurnTableType.BigEquip:
		case TurnTableType.SmallEquip:
		case TurnTableType.Boss:
		case TurnTableType.Hitted:
			break;
		case TurnTableType.ExpBig:
			Image_Icon.sprite = SpriteManager.GetBattle("GameTurn_ExpBig");
			break;
		case TurnTableType.ExpSmall:
			Image_Icon.sprite = SpriteManager.GetBattle("GameTurn_ExpSmall");
			break;
		case TurnTableType.Empty:
			Image_Icon.sprite = SpriteManager.GetBattle("GameTurn_Empty");
			break;
		case TurnTableType.Gold:
			Image_Icon.sprite = SpriteManager.GetUICommon("Currency_Gold");
			Text_Value.text = Utils.FormatString("x{0}", data.value);
			break;
		case TurnTableType.Diamond:
			Image_Icon.sprite = SpriteManager.GetUICommon("Currency_Diamond");
			Text_Value.text = Utils.FormatString("x{0}", data.value);
			break;
		case TurnTableType.HPAdd:
			Image_Icon.sprite = SpriteManager.GetSkillIcon((int)data.value);
			break;
		case TurnTableType.PlayerSkill:
		{
			int skillIcon2 = LocalModelManager.Instance.Skill_skill.GetBeanById((int)data.value).SkillIcon;
			Image_Icon.sprite = SpriteManager.GetSkillIcon(skillIcon2);
			break;
		}
		case TurnTableType.EventSkill:
		{
			Room_eventgameturn beanById = LocalModelManager.Instance.Room_eventgameturn.GetBeanById((int)data.value);
			int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(beanById.GetID).SkillIcon;
			Image_Icon.sprite = SpriteManager.GetSkillIcon(skillIcon);
			break;
		}
		case TurnTableType.Get:
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			sequence.Append(child.DOScale(1.3f, 0.18f));
			sequence.Append(child.DOScale(1f, 0.18f));
			sequence.AppendCallback(delegate
			{
				Image_Icon.sprite = SpriteManager.GetBattle("GameTurn_Tick");
			});
			break;
		}
		case TurnTableType.Reward_Gold2:
		case TurnTableType.Reward_Gold3:
		case TurnTableType.Reward_Item2:
		case TurnTableType.Reward_Item3:
		case TurnTableType.Reward_All2:
		case TurnTableType.Reward_All3:
			Image_Icon.sprite = SpriteManager.GetBattle(data.type.ToString());
			break;
		}
	}
}
