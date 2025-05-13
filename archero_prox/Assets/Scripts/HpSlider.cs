




using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpSlider : MonoBehaviour
{
	private Transform mTransform;

	private GameObject child;

	public Text Text_HP;

	public Transform Image_Fg;

	public Transform Image_Fg_Reduce;

	public Transform Image_Fg_Blue;

	public Transform Image_Bg;

	public Transform Image_MPFG;

	public Transform Image_MP_Reduce;

	public Image Line;

	public Transform LineParent;

	private EntityBase entity;

	private bool bReducingHP;

	private float minReduceScale = 0.008f;

	private float reducesHP;

	private bool bReducingMP;

	private float reducesMP;

	private bool bUpdateLine;

	private float bReducingHP_PosX;

	private Color LineColor = new Color(1f, 1f, 1f, 0f);

	private const int LineFrame = 8;

	private int LineIndex;

	private RectTransform HP_Parent;

	private RectTransform HP_BG;

	private RectTransform HP_FG_Reduce;

	private RectTransform HP_FG;

	private RectTransform HP_FG_Blue;

	private float FG_Width;

	private float maxHP;

	private long mPerHP = 200L;

	private const float perHPWidth = 30f;

	private List<Image> mHPLineList = new List<Image>();

	private Queue<Image> mHPLineCacheList = new Queue<Image>();

	private int ShowHPCount = 1;

	private void Awake()
	{
		mTransform = base.transform;
		child = mTransform.Find("HPSlider").gameObject;
		HP_Parent = (child.transform as RectTransform);
		HP_BG = (child.transform.Find("HP_BG") as RectTransform);
		HP_FG_Reduce = (child.transform.Find("HP_FG_Reduce") as RectTransform);
		HP_FG = (child.transform.Find("HP_FG") as RectTransform);
		HP_FG_Blue = (child.transform.Find("HP_FG_Blue") as RectTransform);
		Vector2 sizeDelta = HP_FG.sizeDelta;
		FG_Width = sizeDelta.x;
	}

	private void LateUpdate()
	{
		if ((bool)entity && !entity.GetIsDead())
		{
			Vector3 vector = Utils.World2Screen(entity.position);
			float x = vector.x;
			float y = vector.y;
			Vector3 localPosition = entity.m_Body.HPMask.transform.localPosition;
			float y2 = y + localPosition.y * 23f;
			mTransform.position = new Vector3(x, y2, 0f);
		}
	}

	private void Update()
	{
		if (bReducingHP)
		{
			Vector3 localScale = Image_Fg_Reduce.localScale;
			float x = localScale.x;
			Vector3 localScale2 = Image_Fg.localScale;
			reducesHP = (x - localScale2.x) / 70f;
			reducesHP = ((!(reducesHP < minReduceScale)) ? reducesHP : minReduceScale);
			Vector3 localScale3 = Image_Fg_Reduce.localScale;
			float num = localScale3.x - reducesHP;
			Vector3 localScale4 = Image_Fg.localScale;
			if (num < localScale4.x)
			{
				Vector3 localScale5 = Image_Fg_Reduce.localScale;
				float x2 = localScale5.x;
				Vector3 localScale6 = Image_Fg.localScale;
				reducesHP = x2 - localScale6.x;
				Image_Fg_Reduce.localScale = Image_Fg.localScale;
				bReducingHP = false;
				bUpdateLine = true;
				return;
			}
			Transform image_Fg_Reduce = Image_Fg_Reduce;
			Vector3 localScale7 = Image_Fg_Reduce.localScale;
			image_Fg_Reduce.localScale = new Vector3(localScale7.x - reducesHP, 1f, 1f);
		}
		if (bReducingMP)
		{
			Vector3 localScale8 = Image_MP_Reduce.localScale;
			float x3 = localScale8.x;
			Vector3 localScale9 = Image_MPFG.localScale;
			reducesMP = (x3 - localScale9.x) / 70f;
			reducesMP = ((!(reducesMP < minReduceScale)) ? reducesMP : minReduceScale);
			Vector3 localScale10 = Image_MP_Reduce.localScale;
			float num2 = localScale10.x - reducesMP;
			Vector3 localScale11 = Image_MPFG.localScale;
			if (num2 < localScale11.x)
			{
				Vector3 localScale12 = Image_MP_Reduce.localScale;
				float x4 = localScale12.x;
				Vector3 localScale13 = Image_MPFG.localScale;
				reducesMP = x4 - localScale13.x;
				Image_MP_Reduce.localScale = Image_MPFG.localScale;
				bReducingMP = false;
				return;
			}
			Transform image_MP_Reduce = Image_MP_Reduce;
			Vector3 localScale14 = Image_MP_Reduce.localScale;
			image_MP_Reduce.localScale = new Vector3(localScale14.x - reducesMP, 1f, 1f);
		}
		LineAnimation();
	}

	public void UpdateHP()
	{
		Vector3 localScale = Image_Fg.localScale;
		if (localScale.x > entity.m_EntityData.GetHPPercent())
		{
			bReducingHP = true;
			LineAnimationStart();
		}
		Transform image_Fg = Image_Fg;
		float hPPercent = entity.m_EntityData.GetHPPercent();
		Vector3 localScale2 = Image_Fg.localScale;
		float y = localScale2.y;
		Vector3 localScale3 = Image_Fg.localScale;
		image_Fg.localScale = new Vector3(hPPercent, y, localScale3.z);
		UpdateHPText();
		UpdateShield();
	}

	private void UpdateHPText()
	{
		if ((bool)Text_HP)
		{
			Text_HP.text = entity.m_EntityData.CurrentHP.ToString();
		}
	}

	public void UpdateShield()
	{
		long num = entity.m_EntityData.MaxHP;
		float num2 = (float)entity.m_EntityData.Shield_CurrentHitValue / (float)num;
		Transform image_Fg_Blue = Image_Fg_Blue;
		float x = num2;
		Vector3 localScale = Image_Fg_Blue.localScale;
		float y = localScale.y;
		Vector3 localScale2 = Image_Fg_Blue.localScale;
		image_Fg_Blue.localScale = new Vector3(x, y, localScale2.z);
	}

	private void OnMaxHPUpdateInternal()
	{
		long num = entity.m_EntityData.MaxHP;
		long num2 = num / mPerHP;
		if (num2 > mHPLineList.Count)
		{
			long num3 = mHPLineList.Count;
			for (long num4 = num2; num3 < num4; num3++)
			{
				mHPLineList.Add(GetHPLine());
			}
		}
		else if (num2 < mHPLineList.Count)
		{
			int num5 = mHPLineList.Count - 1;
			while (num5 >= num2 && num5 < mHPLineList.Count)
			{
				CacheHPLine(mHPLineList[num5]);
				mHPLineList.RemoveAt(num5);
				num5--;
			}
		}
		float num6 = num;
		if (num6 > maxHP)
		{
			num6 = maxHP;
		}
		float num7 = (float)entity.m_EntityData.attribute.GetHPBase() / 4f;
		float num8 = num6 / num7 * 30f;
		float num9 = 3f;
		RectTransform hP_Parent = HP_Parent;
		float x = num8;
		Vector2 sizeDelta = HP_Parent.sizeDelta;
		hP_Parent.sizeDelta = new Vector2(x, sizeDelta.y);
		RectTransform hP_FG = HP_FG;
		float x2 = num8 - num9 * 2f;
		Vector2 sizeDelta2 = HP_FG.sizeDelta;
		hP_FG.sizeDelta = new Vector2(x2, sizeDelta2.y);
		HP_FG.anchoredPosition = new Vector2(num9, 0f);
		HP_FG_Reduce.anchoredPosition = new Vector2(num9, 0f);
		HP_FG_Blue.anchoredPosition = new Vector2(num9, 0f);
		RectTransform hP_FG_Blue = HP_FG_Blue;
		float x3 = num8 - num9 * 2f;
		Vector2 sizeDelta3 = HP_FG.sizeDelta;
		hP_FG_Blue.sizeDelta = new Vector2(x3, sizeDelta3.y);
		HP_FG_Blue.localScale = new Vector3(0f, 1f, 1f);
		RectTransform hP_FG_Reduce = HP_FG_Reduce;
		float x4 = num8 - num9 * 2f;
		Vector2 sizeDelta4 = HP_FG_Reduce.sizeDelta;
		hP_FG_Reduce.sizeDelta = new Vector2(x4, sizeDelta4.y);
		RectTransform hP_BG = HP_BG;
		float x5 = num8;
		Vector2 sizeDelta5 = HP_BG.sizeDelta;
		hP_BG.sizeDelta = new Vector2(x5, sizeDelta5.y);
		for (int i = 0; i < num2; i++)
		{
			float num10 = (float)(i + 1) / ((float)num / (float)mPerHP);
			float x6 = num10 * num8 - num8 / 2f;
			Image image = mHPLineList[i];
			image.transform.localPosition = new Vector3(x6, 0f, 0f);
			bool flag = (i + 1) % 5 == 0;
		}
		UpdateHP();
	}

	private void OnMaxHPUpdate(long before, long after)
	{
		GameLogic.ShowHPMaxChange(after - before);
		OnMaxHPUpdateInternal();
	}

	private Image GetHPLine()
	{
		if (mHPLineCacheList.Count > 0)
		{
			Image image = mHPLineCacheList.Dequeue();
			image.gameObject.SetActive(value: true);
			return image;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("Game/UI/HPLine"));
		gameObject.transform.SetParent(LineParent);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		return gameObject.GetComponent<Image>();
	}

	private void CacheHPLine(Image t)
	{
		mHPLineCacheList.Enqueue(t);
		t.gameObject.SetActive(value: false);
	}

	public void Init(EntityBase entity)
	{
		this.entity = entity;
		maxHP = (float)entity.m_EntityData.MaxHP / 4f * 7f;
		if (entity.IsSelf)
		{
			entity.OnMaxHpUpdate = (Action<long, long>)Delegate.Combine(entity.OnMaxHpUpdate, new Action<long, long>(OnMaxHPUpdate));
			OnMaxHPUpdateInternal();
		}
		UpdateHP();
	}

	public void DeInit()
	{
		EntityBase entityBase = entity;
		entityBase.OnMaxHpUpdate = (Action<long, long>)Delegate.Remove(entityBase.OnMaxHpUpdate, new Action<long, long>(OnMaxHPUpdate));
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void ShowHP(bool show)
	{
		ShowHPCount += (show ? 1 : (-1));
		child.gameObject.SetActive(ShowHPCount > 0);
	}

	private void LineAnimationStart()
	{
		if ((bool)Line)
		{
			bReducingHP_PosX = entity.m_EntityData.GetHPPercent() * 90f - 45f;
			Line.transform.localPosition = new Vector3(bReducingHP_PosX, 0f, 0f);
			Line.color = Color.white;
			Line.transform.localScale = new Vector3(1f, 3f, 1f);
			LineIndex = 8;
		}
	}

	private void LineAnimation()
	{
		if (bUpdateLine && (bool)Line)
		{
			Transform transform = Line.transform;
			Vector3 localScale = Line.transform.localScale;
			float x = localScale.x;
			Vector3 localScale2 = Line.transform.localScale;
			float y = localScale2.y + 2f;
			Vector3 localScale3 = Line.transform.localScale;
			transform.localScale = new Vector3(x, y, localScale3.z);
			Line.color = new Color(1f, 1f, 1f, (float)LineIndex / 8f);
			LineIndex--;
			if (LineIndex < 0)
			{
				LineAniationEnd();
			}
		}
	}

	private void LineAniationEnd()
	{
		if ((bool)Line)
		{
			bUpdateLine = false;
			Line.color = LineColor;
			Line.transform.localScale = Vector3.one;
		}
	}
}
