




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyFlyCtrl
{
	public class CurrencyUseStruct
	{
		public CurrencyType type;

		public long count;

		public Vector3 endpos;

		public Action callback;
	}

	public class CurrencyGetStruct
	{
		public CurrencyType type;

		public long count;

		public Vector3 startpos;
	}

	public class CurrencyUseAction : ActionBasic.ActionUIBase
	{
		public GameObject gameobject;

		public Vector3 endpos;

		private Image image;

		private Vector3 startpos;

		private Vector3 startoffsetpos;

		private float offsets = 70f;

		private float offsettime;

		private float flytime;

		private float alphastarttime;

		private float starttime;

		private AnimationCurve curve;

		private bool bMoveOffset = true;

		protected override void OnInit()
		{
			image = gameobject.GetComponentInChildren<Image>();
			alphastarttime = GameLogic.Random(0f, 0.2f);
			offsettime = GameLogic.Random(0.5f, 0.8f);
			flytime = GameLogic.Random(0.6f, 0.9f);
			startpos = gameobject.transform.position;
			startoffsetpos = startpos + new Vector3(GameLogic.Random(0f - offsets, offsets), GameLogic.Random(0f - offsets, offsets), 0f);
			curve = LocalModelManager.Instance.Curve_curve.GetCurve(100008);
			starttime = Updater.AliveTime;
			bMoveOffset = true;
		}

		protected override void OnUpdate()
		{
			if (!gameobject)
			{
				End();
			}
			else if (bMoveOffset)
			{
				UpdateAlpha();
				UpdateOffset();
			}
			else
			{
				UpdateFly();
			}
		}

		private void UpdateAlpha()
		{
			float num = Updater.AliveTime - starttime;
			if (num > alphastarttime)
			{
				float num2 = num - alphastarttime;
				Image obj = image;
				Color color = image.color;
				float r = color.r;
				Color color2 = image.color;
				float g = color2.g;
				Color color3 = image.color;
				obj.color = new Color(r, g, color3.b, num2 / 0.3f);
			}
		}

		private void UpdateOffset()
		{
			float num = Updater.AliveTime - starttime;
			float value = num / (offsettime - 0.1f);
			value = Mathf.Clamp01(value);
			gameobject.transform.position = startpos + (startoffsetpos - startpos) * curve.Evaluate(value);
			if (num >= offsettime)
			{
				starttime = Updater.AliveTime;
				bMoveOffset = false;
			}
		}

		private void UpdateFly()
		{
			float num = Updater.AliveTime - starttime;
			float value = num / flytime;
			value = Mathf.Clamp01(value);
			gameobject.transform.position = curve.Evaluate(value) * (endpos - startoffsetpos) + startoffsetpos;
			if (value == 1f)
			{
				if ((bool)gameobject)
				{
					UnityEngine.Object.Destroy(gameobject);
				}
				End();
			}
		}
	}

	private class CurrencyFlyData
	{
		public string path;

		public float range;
	}

	private ActionBasic action = new ActionBasic();

	private static Dictionary<CurrencyType, CurrencyFlyData> mList = new Dictionary<CurrencyType, CurrencyFlyData>
	{
		{
			CurrencyType.Gold,
			new CurrencyFlyData
			{
				path = "CurrencyFly_Gold",
				range = (float)GameLogic.Width / 7f
			}
		},
		{
			CurrencyType.Diamond,
			new CurrencyFlyData
			{
				path = "CurrencyFly_Diamond",
				range = (float)GameLogic.Width / 7f
			}
		},
		{
			CurrencyType.LevelExp,
			new CurrencyFlyData
			{
				path = "CurrencyFly_LevelExp",
				range = (float)GameLogic.Width / 7f
			}
		},
		{
			CurrencyType.BattleGold,
			new CurrencyFlyData
			{
				path = "CurrencyFly_Gold",
				range = (float)GameLogic.Width / 7f
			}
		},
		{
			CurrencyType.Key,
			new CurrencyFlyData
			{
				path = "CurrencyFly_Key",
				range = (float)GameLogic.Width / 7f
			}
		},
		{
			CurrencyType.BattleDiamond,
			new CurrencyFlyData
			{
				path = "CurrencyFly_Diamond",
				range = (float)GameLogic.Width / 7f
			}
		}
	};

	public void UseAction(string typename, Transform parent, Vector3 startpos, Vector3 endpos, long count, Action callback)
	{
		if ((bool)parent && action.GetActionCount() <= 0)
		{
			action.Init();
			ActionBasic.ActionParallel actionParallel = new ActionBasic.ActionParallel();
			List<ActionBasic.ActionBase> list = new List<ActionBasic.ActionBase>();
			if (count < 1)
			{
				count = 1L;
			}
			else if (count > 10)
			{
				count = 10L;
			}
			for (int i = 0; i < count; i++)
			{
				list.Add(new CurrencyUseAction
				{
					gameobject = GetGameobject(typename, parent, startpos),
					endpos = endpos
				});
			}
			actionParallel.list = list;
			action.AddAction(actionParallel);
			if (callback != null)
			{
				action.AddActionDelegate(callback);
			}
		}
	}

	private GameObject GetGameobject(string name, Transform parent, Vector3 startpos)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>(Utils.GetString("UIPanel/CurrencyUI/", name)));
		gameObject.transform.SetParent(parent);
		gameObject.transform.position = startpos;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.rotation = Quaternion.identity;
		Animation componentInChildren = gameObject.GetComponentInChildren<Animation>();
		if ((bool)componentInChildren)
		{
			componentInChildren["CurrencyFlyRotate"].time = GameLogic.Random(0f, componentInChildren["CurrencyFlyRotate"].length);
		}
		return gameObject;
	}

	public void DeInit()
	{
		action.DeInit();
	}

	public static void UseCurrency(CurrencyType type, long count, Vector3 endpos, Action callback)
	{
		Facade.Instance.SendNotification("UseCurrency", new CurrencyUseStruct
		{
			type = type,
			count = count,
			endpos = endpos,
			callback = callback
		});
	}

	public static void GetCurrency(CurrencyType type, int count, Vector3 startpos)
	{
		Facade.Instance.SendNotification("GetCurrency", new CurrencyGetStruct
		{
			type = type,
			count = count,
			startpos = startpos
		});
	}

	public static void PlayKeyUse(long count, Vector3 startpos, Vector3 endpos, Action onFinish)
	{
		WindowUI.ShowMask(value: true);
		Transform t = GetObject("CurrencyFly_Key", startpos, GameNode.m_FrontEvent);
		t.DOMove(endpos, 1f).SetUpdate(isIndependentUpdate: true).SetEase(Ease.OutQuad)
			.OnComplete(delegate
			{
				WindowUI.ShowMask(value: false);
				UnityEngine.Object.Destroy(t.gameObject);
				if (onFinish != null)
				{
					onFinish();
				}
			});
	}

	private static Transform GetObject(string path, Vector3 startpos, Transform parent)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/CurrencyUI/{0}", path)));
		gameObject.SetParentNormal(parent);
		RectTransform rectTransform = gameObject.transform as RectTransform;
		rectTransform.position = startpos;
		return rectTransform;
	}

	public static void PlayGet(CurrencyType type, long allcount, Action<long> OnOverOne = null, Action onFinish = null, bool mask = true)
	{
		PlayGet(type, allcount, new Vector3((float)GameLogic.Width / 2f, (float)GameLogic.Height / 2f, 0f), OnOverOne, onFinish, mask);
	}

	public static void PlayGet(CurrencyType type, long allcount, Vector3 startpos, Action<long> OnOverOne = null, Action onFinish = null, bool mask = true)
	{
		if (allcount <= 0)
		{
			return;
		}
		IMediator mediator = Facade.Instance.RetrieveMediator("CurrencyModuleMediator");
		if (mediator != null)
		{
			Vector3 endpos = new Vector3(GameLogic.Width, GameLogic.Height, 0f);
			switch (type)
			{
			case CurrencyType.Gold:
				endpos = (Vector3)mediator.GetEvent("GetEvent_GetGoldPosition");
				break;
			case CurrencyType.Diamond:
				endpos = (Vector3)mediator.GetEvent("GetEvent_GetDiamondPosition");
				break;
			case CurrencyType.Key:
				endpos = (Vector3)mediator.GetEvent("GetEvent_GetKeyPosition");
				break;
			}
			PlayGet(type, allcount, startpos, endpos, OnOverOne, onFinish, mask);
		}
	}

	public static void PlayGet(CurrencyType type, long allcount, Vector3 startpos, Vector3 endpos, Action<long> OnOverOne, Action onFinish, bool mask)
	{
		if (allcount > 0)
		{
			PlayFlyAnimation(type, allcount, startpos, endpos, OnOverOne, onFinish, mask);
		}
	}

	public static void PlayFlyAnimation(CurrencyType type, long allcount, Vector3 startpos, Vector3 endpos, Action<long> OnOverOne, Action onFinish, bool mask)
	{
		if (allcount == 0 || !mList.ContainsKey(type))
		{
			return;
		}
		startpos = new Vector3(startpos.x, startpos.y, 0f);
		endpos = new Vector3(endpos.x, endpos.y, 0f);
		int count = (int)allcount;
		if (count > 20)
		{
			count = 20;
		}
		long beforecount = 0L;
		float range = mList[type].range;
		Vector3 vector = endpos - startpos;
		float angle = Utils.getAngle(new Vector2(vector.x, vector.y));
		if (mask)
		{
			WindowUI.ShowMask(value: true);
		}
		for (int i = 0; i < count; i++)
		{
			int index = i;
			if (!mList.ContainsKey(type))
			{
				SdkManager.Bugly_Report("CurrencyFlyCtrl", Utils.FormatString("PlayFlyAnimation {0} is not int mList.", type));
			}
			Transform t = GetObject(mList[type].path, startpos, GameNode.m_Front3);
			Vector3 b = new Vector3(GameLogic.Random(0f - range, range), GameLogic.Random(0f - range, range), 0f);
			Vector3 endValue = startpos + b;
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			CanvasGroup component = t.GetComponent<CanvasGroup>();
			component.alpha = 0f;
			sequence.AppendInterval((float)index * 0.03f);
			sequence.Append(t.DOMove(endValue, 0.2f).SetUpdate(isIndependentUpdate: true));
			sequence.Join(component.DOFade(1f, 0.4f).SetUpdate(isIndependentUpdate: true));
			sequence.Append(t.DOMove(endpos, 0.7f).SetUpdate(isIndependentUpdate: true).SetEase(Ease.InBack)
				.OnComplete(delegate
				{
					UnityEngine.Object.Destroy(t.gameObject);
					long num = (long)((float)allcount / (float)count * ((float)index + 1f));
					long num2 = num - beforecount;
					beforecount = num;
					if (type != CurrencyType.Gold)
					{
						if (type != CurrencyType.Diamond)
						{
							if (type == CurrencyType.Key)
							{
								LocalSave.Instance.Modify_Key(num2);
							}
						}
						else
						{
							LocalSave.Instance.Modify_ShowDiamond(num2);
						}
					}
					else
					{
						LocalSave.Instance.Modify_ShowGold(num2);
					}
					if (OnOverOne != null)
					{
						OnOverOne(num2);
					}
				}));
			if (index == count - 1)
			{
				sequence.AppendCallback(delegate
				{
					if (mask)
					{
						WindowUI.ShowMask(value: false);
					}
					if (onFinish != null)
					{
						onFinish();
					}
				});
			}
		}
	}
}
