




using Dxx;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FindPath
{
	private Grid grid = new Grid();

	private Queue<GameObject> mCacheList = new Queue<GameObject>();

	private List<GameObject> mUseList = new List<GameObject>();

	public FindPath()
	{
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
		InitData();
	}

	public void DeInit()
	{
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
	}

	private void OnGotoNextRoom(RoomGenerateBase.Room room)
	{
		InitData();
	}

	private void InitData()
	{
		Init(GameLogic.Release.MapCreatorCtrl.GetFindPathRect());
	}

	public void Init(int[,] rects)
	{
		grid.Init(rects);
	}

	public List<Grid.NodeItem> FindingPath(Vector3 s, Vector3 e)
	{
		Grid.NodeItem item = grid.getItem(s);
		Grid.NodeItem item2 = grid.getItem(e);
		List<Grid.NodeItem> list = ListPool<Grid.NodeItem>.Get();
		HashSet<Grid.NodeItem> hashSet = HashSetPool<Grid.NodeItem>.Get();
		list.Add(item);
		List<Grid.NodeItem> list2 = new List<Grid.NodeItem>();
		while (list.Count > 0)
		{
			Grid.NodeItem nodeItem = list[0];
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (list[i].fCost <= nodeItem.fCost && list[i].hCost < nodeItem.hCost)
				{
					nodeItem = list[i];
				}
			}
			list.Remove(nodeItem);
			hashSet.Add(nodeItem);
			if (nodeItem == item2)
			{
				list2 = generatePath(item, item2);
				break;
			}
			List<Grid.NodeItem> neibourhood = grid.getNeibourhood(nodeItem);
			for (int j = 0; j < neibourhood.Count; j++)
			{
				Grid.NodeItem nodeItem2 = neibourhood[j];
				if (nodeItem2.isWall || hashSet.Contains(nodeItem2))
				{
					continue;
				}
				int num = nodeItem.gCost + getDistanceNodes(nodeItem, nodeItem2);
				if (num < nodeItem2.gCost || !list.Contains(nodeItem2))
				{
					nodeItem2.gCost = num;
					nodeItem2.hCost = getDistanceNodes(nodeItem2, item2);
					nodeItem2.parent = nodeItem;
					if (!list.Contains(nodeItem2))
					{
						list.Add(nodeItem2);
					}
				}
			}
		}
		if (list2.Count == 0)
		{
			list2 = generatePath(item, null);
		}
		ListPool<Grid.NodeItem>.Release(list);
		HashSetPool<Grid.NodeItem>.Release(hashSet);
		return list2;
	}

	private List<Grid.NodeItem> generatePath(Grid.NodeItem startNode, Grid.NodeItem endNode)
	{
		List<Grid.NodeItem> list = new List<Grid.NodeItem>();
		if (endNode != null)
		{
			for (Grid.NodeItem nodeItem = endNode; nodeItem != startNode; nodeItem = nodeItem.parent)
			{
				list.Add(nodeItem);
			}
			list.Reverse();
		}
		grid.updatePath(list);
		return list;
	}

	private int getDistanceNodes(Grid.NodeItem a, Grid.NodeItem b)
	{
		int num = Mathf.Abs(a.x - b.x);
		int num2 = Mathf.Abs(a.y - b.y);
		return (num + num2) * 10;
	}

	private GameObject GetSphere()
	{
		return GameObjectPool.Instantiate("Sphere");
	}

	private void CacheSphere(GameObject o)
	{
		GameObjectPool.Release("Sphere", o);
	}
}
