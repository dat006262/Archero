




using System.Collections.Generic;
using UnityEngine;

public class Grid
{
	public class NodeItem
	{
		public bool isWall;

		public int x;

		public int y;

		public int gCost;

		public int hCost;

		public NodeItem parent;

		public int fCost => gCost + hCost;

		public NodeItem(bool isWall, int x, int y)
		{
			this.isWall = isWall;
			this.x = x;
			this.y = y;
		}
	}

	public Transform player;

	public Transform destPos;

	private NodeItem[,] grid;

	private int w;

	private int h;

	private GameObject WallRange;

	private GameObject PathRange;

	private List<GameObject> pathObj = new List<GameObject>();

	public void Init(int[,] list)
	{
		w = list.GetLength(0);
		h = list.GetLength(1);
		grid = new NodeItem[w, h];
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				bool isWall = list[i, j] > 0;
				grid[i, j] = new NodeItem(isWall, i, j);
			}
		}
	}

	public NodeItem getItem(Vector3 position)
	{
		if ((bool)GameLogic.Release && GameLogic.Release.MapCreatorCtrl != null)
		{
			Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(position);
			if (grid != null)
			{
				if (grid.GetLength(0) > roomXY.x && grid.GetLength(1) > roomXY.y)
				{
					return grid[roomXY.x, roomXY.y];
				}
				return new NodeItem(isWall: false, 5, 0);
			}
			return new NodeItem(isWall: false, 5, 0);
		}
		if (grid != null && grid.GetLength(0) > 5 && grid.GetLength(1) > 0)
		{
			return grid[5, 0];
		}
		return new NodeItem(isWall: false, 5, 0);
	}

	public List<NodeItem> getNeibourhood(NodeItem node)
	{
		List<NodeItem> list = new List<NodeItem>();
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if ((i != 0 || j != 0) && (i == 0 || j == 0))
				{
					int num = node.x + i;
					int num2 = node.y + j;
					if (num < w && num >= 0 && num2 < h && num2 >= 0)
					{
						list.Add(grid[num, num2]);
					}
				}
			}
		}
		return list;
	}

	public void updatePath(List<NodeItem> lines)
	{
	}
}
