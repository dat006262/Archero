




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopOneEquipExp : ShopOneBase
{
	public const float itemwidth = 235f;

	public Text Text_Title;

	public Text Text_Content;

	public GameObject goldparent;

	private List<ShopItemEquipExp> mList = new List<ShopItemEquipExp>();

	private GameObject _itemgold;

	private LocalUnityObjctPool mPool;

	private string oncestring;

	private string timestring;

	private int lasttime;

	private float m_flasttime;

	private GameObject itemgold
	{
		get
		{
			if (_itemgold == null)
			{
				_itemgold = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/ShopUI/ShopItemEquipExpOne"));
				_itemgold.SetParentNormal(goldparent);
			}
			return _itemgold;
		}
	}

	protected override void OnAwake()
	{
		if (mPool == null)
		{
			mPool = LocalUnityObjctPool.Create(base.gameObject);
			mPool.CreateCache<ShopItemEquipExp>(itemgold);
			itemgold.SetActive(value: false);
		}
	}

	protected override void OnInit()
	{
		mPool.Collect<ShopItemEquipExp>();
		mList.Clear();
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_equipexp_title");
		oncestring = GameLogic.Hold.Language.GetLanguageByTID("shopui_equipexp_buy_once");
		timestring = GameLogic.Hold.Language.GetLanguageByTID("battle_level_nexttime");
		lasttime = GameLogic.Random(500, 5000);
		m_flasttime = lasttime;
		Update();
		int num = 3;
		float num2 = (float)(num - 1) * 235f;
		for (int i = 0; i < num; i++)
		{
			ShopItemEquipExp shopItemEquipExp = mPool.DeQueue<ShopItemEquipExp>();
			shopItemEquipExp.gameObject.SetParentNormal(goldparent);
			RectTransform rectTransform = shopItemEquipExp.transform as RectTransform;
			rectTransform.anchoredPosition = new Vector2((0f - num2) / 2f + 235f * (float)i, 0f);
			shopItemEquipExp.Init(i);
			shopItemEquipExp.OnClickButton = OnOpenWindowSure;
			mList.Add(shopItemEquipExp);
		}
	}

	protected override void OnDeinit()
	{
	}

	private void Update()
	{
		m_flasttime -= Time.deltaTime;
		string second3String = Utils.GetSecond3String((int)m_flasttime);
		Text_Content.text = Utils.FormatString("{0}  {1}:{2}", oncestring, timestring, second3String);
	}

	private void OnOpenWindowSure(int index, ShopItemEquipExp item)
	{
	}

	private void OnClickEquipExp(int index, ShopItemEquipExp item)
	{
	}

	public override void OnLanguageChange()
	{
		OnInit();
	}

	public override void UpdateNet()
	{
		int i = 0;
		for (int count = mList.Count; i < count; i++)
		{
			mList[i].UpdateNet();
		}
	}
}
