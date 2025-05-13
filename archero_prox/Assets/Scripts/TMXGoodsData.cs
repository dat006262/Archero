




public class TMXGoodsData
{
	public TMXGoodsType Type;

	public TMXGoodsParentType ParentType;

	private int TypeId;

	private int GoodsID;

	public int x;

	public int y;

	public TMXGoodsData()
	{
	}

	public TMXGoodsData(int goodsid, int typeid)
	{
		SetGoodsId(goodsid);
		Init(typeid);
	}

	public void SetGoodsId(int goodsid)
	{
		GoodsID = goodsid;
	}

	public void Init(int typeid)
	{
		TypeId = typeid;
		Type = (TMXGoodsType)typeid;
		if (typeid < 100)
		{
			ParentType = TMXGoodsParentType.Obstacle_GroundUp;
		}
		else if (typeid < 200)
		{
			ParentType = TMXGoodsParentType.Obstacle_GroundDown;
		}
		else if (typeid < 300)
		{
			ParentType = TMXGoodsParentType.Through_Trap;
		}
		else if (typeid < 400)
		{
			ParentType = TMXGoodsParentType.Food;
		}
		else if (typeid < 900)
		{
			ParentType = TMXGoodsParentType.Equip;
		}
	}

	public bool IsEmpty()
	{
		return ParentType != TMXGoodsParentType.Obstacle_GroundUp && ParentType != TMXGoodsParentType.Obstacle_GroundDown;
	}
}
