




using DG.Tweening;
using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenOneEquipCtrl : MonoBehaviour
{
	public GameObject infoparent;

	public Text Text_Title;

	public Text Text_Name;

	public Text Text_Info;

	public Transform equipparent;

	public Image Image_Icon;

	public Image Image_BG;

	public GameObject mAddParent;

	public Text Text_Count;

	public Image Image_White;

	public GameObject fx_open;

	private Sequence seq;

	private LocalSave.EquipOne equipdata;

	public Sequence Init(LocalSave.EquipOne equip, int count)
	{
		DeInit();
		equipdata = equip;
		Text_Count.text = Utils.FormatString("x{0}", count);
		mAddParent.SetActive(value: false);
		fx_open.SetActive(value: false);
		equipparent.localScale = Vector3.zero;
		int num = 1;
		num = ((count <= 5) ? 1 : ((count > 10) ? 3 : 2));
		Text_Title.color = GetColor(num);
		Text_Count.color = Text_Title.color;
		Text_Title.text = Utils.FormatString("{0}!", GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("BoxOpenOne_EquipTitle{0}", num)));
		Text_Name.text = equip.NameString;
		Text_Info.text = equip.InfoString;
		Text_Title.transform.localScale = Vector3.zero;
		Text_Name.transform.localScale = Vector3.zero;
		Text_Info.transform.localScale = Vector3.zero;
		Image_BG.sprite = SpriteManager.GetCharUI(Utils.FormatString("CharUI_Quality{0}", equipdata.Quality));
		Image_Icon.sprite = equipdata.Icon;
		Image_White.color = new Color(1f, 1f, 1f, 0.7f);
		seq = DOTween.Sequence();
		seq.Append(Text_Title.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
		seq.AppendInterval(0.3f);
		seq.Append(Text_Name.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
		seq.Append(Text_Info.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
		seq.AppendCallback(delegate
		{
			fx_open.SetActive(value: true);
		});
		seq.Append(equipparent.DOScale(1f, 0.3f));
		seq.AppendInterval(0.3f);
		seq.AppendCallback(delegate
		{
			Image_White.DOColor(new Color(1f, 1f, 1f, 0f), 0.3f);
		});
		seq.AppendInterval(0.3f);
		return seq;
	}

	public void DeInit()
	{
		fx_open.SetActive(value: false);
	}

	private Color GetColor(int quality)
	{
		return LocalSave.QualityColors[quality];
	}
}
