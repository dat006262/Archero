




using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeadGoodCtrl
{
	private Action callback;

	private bool bStart;

	private int currentFrame;

	private int currentindex;

	private List<BattleDropData> goodslist;

	private List<Vector2Int> Goldslist;

	private Vector2Int GoldsCenter;

	private Transform MapGoodsDrop;

	private Vector3 pos;

	public void Init()
	{
		Updater.AddUpdate("DeadGoodCtrl", OnUpdate);
	}

	public void DeInit()
	{
		bStart = false;
		Updater.RemoveUpdate("DeadGoodCtrl", OnUpdate);
	}

	public void StartDrop(Vector3 pos, List<BattleDropData> goodslist, int radius, Transform MapGoodsDrop, Action callback)
	{
		this.callback = callback;
		this.pos = pos;
		this.goodslist = goodslist;
		this.MapGoodsDrop = MapGoodsDrop;
		GoldsCenter = default(Vector2Int);
		Goldslist = GameLogic.Release.MapCreatorCtrl.GetNearEmptyList(pos, ref GoldsCenter, radius);
		bStart = true;
		currentFrame = 0;
		currentindex = 0;
		if (goodslist.Count == 0)
		{
			bStart = false;
		}
	}

	private void OnUpdate(float delta)
	{
		if (!bStart)
		{
			return;
		}
		if (currentFrame % 3 == 0)
		{
			if (goodslist == null || currentindex >= goodslist.Count || currentindex < 0)
			{
				if (callback != null)
				{
					callback();
				}
				bStart = false;
				return;
			}
			string text = string.Empty;
			switch (goodslist[currentindex].type)
			{
			case FoodType.eHP:
			case FoodType.eGold:
			case FoodType.eExp:
			{
				int childtype = (int)goodslist[currentindex].childtype;
				text = childtype.ToString();
				break;
			}
			case FoodType.eEquip:
			{
				LocalSave.EquipOne equipOne = goodslist[currentindex].data as LocalSave.EquipOne;
				text = "1010101";
				break;
			}
			}
			GameObject gameObject = GameLogic.EffectGet(Utils.GetString("Game/Food/", text));
			gameObject.transform.parent = MapGoodsDrop;
			gameObject.transform.position = new Vector3(pos.x, 0f, pos.z);
			gameObject.transform.localScale = Vector3.one;
			FoodBase component = gameObject.GetComponent<FoodBase>();
			component.Init(goodslist[currentindex].data);
			Vector3 randomEndPosition = GetRandomEndPosition(Goldslist, GoldsCenter);
			component.SetEndPosition(pos, randomEndPosition);
			currentindex++;
		}
		currentFrame++;
	}

	private Vector3 GetRandomEndPosition(List<Vector2Int> Goldslist, Vector2Int GoldsCenter)
	{
		float num = 0.45f;
		Vector3 worldPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(GoldsCenter);
		Vector2Int v;
		Vector3 a;
		if (UnityEngine.Random.Range(0, 100) < 30 && GameLogic.Release.MapCreatorCtrl.IsEmpty(GoldsCenter))
		{
			v = GoldsCenter;
			a = new Vector3(worldPosition.x + UnityEngine.Random.Range(0f - num, num), 0f, worldPosition.z + UnityEngine.Random.Range(0f - num, num));
		}
		else
		{
			int index = UnityEngine.Random.Range(0, Goldslist.Count);
			v = Goldslist[index];
			worldPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(v);
			a = new Vector3(worldPosition.x + UnityEngine.Random.Range(0f - num, num), 0f, worldPosition.z + UnityEngine.Random.Range(0f - num, num));
		}
		Vector3 vector = Vector3.zero;
		if (v.x == 0)
		{
			vector += new Vector3(0.15f, 0f, 0f);
		}
		else if (v.x == GameLogic.Release.MapCreatorCtrl.width - 1)
		{
			vector += new Vector3(-0.15f, 0f, 0f);
		}
		if (v.y == 0)
		{
			vector += new Vector3(0f, 0f, -0.2f);
		}
		else if (v.y == GameLogic.Release.MapCreatorCtrl.height - 1)
		{
			vector += new Vector3(0f, 0f, 0.2f);
		}
		return a + vector;
	}
}
