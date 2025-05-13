




using System;
using System.Collections.Generic;
using UnityEngine;

public class GameThreeActionCtrl : ActionBasic
{
	public class ActionUp : ActionUIBase
	{
		public List<Transform> list;

		private int frame = 10;

		private int currentframe;

		private float Speed = 50f;

		protected override void OnInit()
		{
			currentframe = 0;
		}

		protected override void OnUpdate()
		{
			if (currentframe < frame)
			{
				float speed = (1f - ((float)currentframe + 1f) / (float)frame) * Speed;
				DoPosition(speed);
				currentframe++;
			}
			else
			{
				End();
			}
		}

		private void DoPosition(float speed)
		{
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				Vector3 localPosition = list[i].localPosition;
				float num = localPosition.y + speed;
				Transform transform = list[i];
				Vector3 localPosition2 = list[i].localPosition;
				float x = localPosition2.x;
				float y = num;
				Vector3 localPosition3 = list[i].localPosition;
				transform.localPosition = new Vector3(x, y, localPosition3.z);
			}
		}
	}

	public class ActionDown : ActionUIBase
	{
		public List<Transform> list;

		private int frame = 10;

		private int currentframe;

		private float Speed = 50f;

		protected override void OnInit()
		{
			currentframe = 0;
		}

		protected override void OnUpdate()
		{
			if (currentframe < frame)
			{
				float speed = (0f - (float)currentframe / (float)frame) * Speed;
				DoPosition(speed);
				currentframe++;
			}
			else
			{
				End();
			}
		}

		private void DoPosition(float speed)
		{
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				Vector3 localPosition = list[i].localPosition;
				float num = localPosition.y + speed;
				Transform transform = list[i];
				Vector3 localPosition2 = list[i].localPosition;
				float x = localPosition2.x;
				float y = num;
				Vector3 localPosition3 = list[i].localPosition;
				transform.localPosition = new Vector3(x, y, localPosition3.z);
			}
		}
	}

	public class ActionShowSieve : ActionUIBase
	{
		public GameObject sieve;

		public bool show;

		protected override void OnInit()
		{
			sieve.SetActive(show);
			End();
		}
	}

	public class ActionRandomSieve : ActionUIBase
	{
		public List<Transform> list;

		public List<Transform> shadowlist;

		public Transform sieve;

		private Transform transform1;

		private Transform transform2;

		private Transform shadow1;

		private Transform shadow2;

		private int moveCount = 8;

		private int currentCount;

		private int moveframe = 12;

		private int currentframe;

		private float movestartx;

		private float moveendx;

		private int currentstate;

		private float y;

		private float yValue = 5f;

		protected override void OnInit()
		{
		}

		protected override void OnUpdate()
		{
			if (currentCount < moveCount)
			{
				if (currentstate == 0)
				{
					currentframe = 0;
					RandomTransform(out transform1, out this.transform2, out shadow1, out shadow2);
					Vector3 localPosition = transform1.localPosition;
					movestartx = localPosition.x;
					Vector3 localPosition2 = this.transform2.localPosition;
					moveendx = localPosition2.x;
					currentstate = 1;
				}
				else if (currentstate == 1)
				{
					if (currentframe < moveframe / 2)
					{
						y = (float)(-currentframe) * yValue;
					}
					else
					{
						y = (float)(-(moveframe - currentframe - 1)) * yValue;
					}
					Transform transform = transform1;
					float x = movestartx + (moveendx - movestartx) * ((float)currentframe + 1f) / (float)moveframe;
					float num = y;
					Vector3 localPosition3 = transform1.localPosition;
					transform.localPosition = new Vector3(x, num, localPosition3.z);
					Transform transform2 = this.transform2;
					float x2 = moveendx + (movestartx - moveendx) * ((float)currentframe + 1f) / (float)moveframe;
					float num2 = 0f - y;
					Vector3 localPosition4 = this.transform2.localPosition;
					transform2.localPosition = new Vector3(x2, num2, localPosition4.z);
					Transform transform3 = shadow1;
					Vector3 localPosition5 = transform1.localPosition;
					float x3 = localPosition5.x;
					Vector3 localPosition6 = shadow1.localPosition;
					float num3 = localPosition6.y;
					Vector3 localPosition7 = shadow1.localPosition;
					transform3.localPosition = new Vector3(x3, num3, localPosition7.z);
					Transform transform4 = shadow2;
					Vector3 localPosition8 = this.transform2.localPosition;
					float x4 = localPosition8.x;
					Vector3 localPosition9 = shadow2.localPosition;
					float num4 = localPosition9.y;
					Vector3 localPosition10 = shadow2.localPosition;
					transform4.localPosition = new Vector3(x4, num4, localPosition10.z);
					currentframe++;
					if (currentframe == moveframe)
					{
						currentstate = 2;
					}
				}
				else if (currentstate == 2)
				{
					sieve.localPosition = list[1].localPosition;
					currentCount++;
					currentstate = 0;
				}
			}
			else
			{
				End();
			}
		}

		private void RandomTransform(out Transform transform1, out Transform transform2, out Transform shadow1, out Transform shadow2)
		{
			int num = GameLogic.Random(0, list.Count);
			transform1 = list[num];
			shadow1 = shadowlist[num];
			int num2;
			for (num2 = GameLogic.Random(0, list.Count); num2 == num; num2 = GameLogic.Random(0, list.Count))
			{
			}
			transform2 = list[num2];
			shadow2 = shadowlist[num2];
		}
	}

	public void DoAction(List<Transform> list, List<Transform> shadowlist, GameObject sieve, Action callback)
	{
		ActionClear();
		AddAction(new ActionShowMaskUI
		{
			show = true
		});
		AddAction(new ActionShowSieve
		{
			sieve = sieve,
			show = true
		});
		AddAction(new ActionWaitIgnoreTime
		{
			waitTime = 0.5f
		});
		AddAction(new ActionUp
		{
			list = list
		});
		AddAction(new ActionWaitIgnoreTime
		{
			waitTime = 0.5f
		});
		AddAction(new ActionDown
		{
			list = list
		});
		AddAction(new ActionShowSieve
		{
			sieve = sieve,
			show = false
		});
		AddAction(new ActionRandomSieve
		{
			list = list,
			shadowlist = shadowlist,
			sieve = sieve.transform
		});
		AddAction(new ActionShowSieve
		{
			sieve = sieve,
			show = true
		});
		AddAction(new ActionShowMaskUI
		{
			show = false
		});
		AddAction(new ActionDelegate
		{
			action = callback
		});
	}

	public void OnClickOne(Transform transform, Transform sieve, Action<bool> callback)
	{
		ActionClear();
		AddAction(new ActionUp
		{
			list = new List<Transform>
			{
				transform
			}
		});
		ActionDelegate actionDelegate = new ActionDelegate
		{
			actionbool = callback
		};
		ActionDelegate actionDelegate2 = actionDelegate;
		Vector3 localPosition = transform.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = sieve.localPosition;
		actionDelegate2.resultbool = (x == localPosition2.x);
		AddAction(actionDelegate);
	}

	protected override void OnUpdate(float delta)
	{
		if (actionList.Count > 0)
		{
			ActionBase actionBase = actionList[0];
			actionBase.Init();
			actionBase.Update();
			if (actionBase.IsEnd)
			{
				actionList.RemoveAt(0);
			}
		}
	}
}
