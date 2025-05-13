




using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSortCtrl : MonoBehaviour
{
	public ButtonCtrl Button_Sort;

	public Text Text_Sort;

	private const int sorttypecount = 2;

	private static string[] sortstrings = new string[2]
	{
		"EquipUI_Sort_Quality",
		"EquipUI_Sort_Position"
	};

	public Action<List<LocalSave.EquipOne>> OnButtonClick;

	private Func<List<LocalSave.EquipOne>>[] sorts = new Func<List<LocalSave.EquipOne>>[2];

	private EquipType mEquipType = EquipType.eAll;

	private int mSortType
	{
		get
		{
			return PlayerPrefsEncrypt.GetInt("charui_sort_local");
		}
		set
		{
			PlayerPrefsEncrypt.SetInt("charui_sort_local", value);
		}
	}

	private void Awake()
	{
		sorts[0] = delegate
		{
			List<LocalSave.EquipOne> props2 = LocalSave.Instance.GetProps(mEquipType, havewear: false);
			props2.Sort(delegate(LocalSave.EquipOne a, LocalSave.EquipOne b)
			{
				if (a.PropType < b.PropType)
				{
					return -1;
				}
				if (a.PropType > b.PropType)
				{
					return 1;
				}
				if (a.data.Quality > b.data.Quality)
				{
					return -1;
				}
				if (a.data.Quality < b.data.Quality)
				{
					return 1;
				}
				return (a.data.Id < b.data.Id) ? (-1) : 1;
			});
			return props2;
		};
		sorts[1] = delegate
		{
			List<LocalSave.EquipOne> props = LocalSave.Instance.GetProps(mEquipType, havewear: false);
			props.Sort(delegate(LocalSave.EquipOne a, LocalSave.EquipOne b)
			{
				if (a.PropType < b.PropType)
				{
					return -1;
				}
				if (a.PropType > b.PropType)
				{
					return 1;
				}
				if (a.data.Position < b.data.Position)
				{
					return -1;
				}
				if (a.data.Position > b.data.Position)
				{
					return 1;
				}
				if (a.data.Quality > b.data.Quality)
				{
					return -1;
				}
				if (a.data.Quality < b.data.Quality)
				{
					return 1;
				}
				return (a.data.Id < b.data.Id) ? (-1) : 1;
			});
			return props;
		};
		Button_Sort.onClick = delegate
		{
			if (OnButtonClick != null)
			{
				mSortType++;
				mSortType %= 2;
				OnLanguageChange();
				OnButtonClick(sorts[mSortType]());
			}
		};
	}

	public List<LocalSave.EquipOne> GetList(EquipType type)
	{
		mEquipType = type;
		return sorts[mSortType]();
	}

	public void OnLanguageChange()
	{
		Text_Sort.text = GameLogic.Hold.Language.GetLanguageByTID(sortstrings[mSortType]);
	}
}
