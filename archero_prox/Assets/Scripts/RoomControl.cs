




using TableTool;
using UnityEngine;

public class RoomControl : RoomControlBase
{
	private static Color[] ColorLights = new Color[5]
	{
		Color.white,
		new Color(247f / 255f, 1f, 0f, 1f),
		new Color(0f, 1f, 244f / 255f, 1f),
		new Color(1f, 0.6f, 0f, 1f),
		new Color(0f, 57f / 85f, 1f, 1f)
	};

	private static Color[] ColorShadows = new Color[5]
	{
		Color.white,
		new Color(7f / 51f, 73f / 255f, 7f / 255f, 1f),
		new Color(7f / 51f, 73f / 255f, 7f / 255f, 1f),
		new Color(43f / 85f, 88f / 255f, 0.0117647061f, 1f),
		new Color(2f / 51f, 33f / 85f, 94f / 255f, 1f)
	};

	private const string DoorAnimationName = "MapDoor_Miss";

	private RoomGenerateBase.Room room;

	private RoomGenerateBase.Room nextRoom;

	private RoomGateCtrl mGateCtrl;

	private GameObject layerObj;

	private GameObject bossObj;

	protected override void OnAwake()
	{
		layerObj = base.transform.Find("WallUp/Layer/Map_LayerIcon").gameObject;
		bossObj = base.transform.Find("WallUp/Layer/BossParent").gameObject;
		mGateCtrl = base.transform.Find("WallUp/gate").GetComponent<RoomGateCtrl>();
	}

	protected override void OnInit(object data = null)
	{
		OpenDoor(value: false);
		GameLogic.Hold.Guide.GuideBattleNext();
		if (data != null)
		{
			Mode_LevelData mode_LevelData = (Mode_LevelData)data;
			if (mode_LevelData != null)
			{
				room = mode_LevelData.room;
				nextRoom = mode_LevelData.nextroom;
			}
		}
		if (room != null)
		{
			SetLayer(room.RoomID);
		}
		DoorDownShow();
		ExcuteLayer();
		OnInitStage();
	}

	protected override void OnLayerShow(bool value)
	{
		layerObj.SetActive(value);
		bossObj.SetActive(value: false);
	}

	protected override void OnSetText(string value)
	{
		SetLayer(value);
	}

	protected override void OnOpenDoor(bool show)
	{
		m_bOpenDoor = show;
		mGateCtrl.OpenDoor(show);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.G))
		{
			OnOpenDoor(show: true);
		}
	}

	private void DoorDownShow()
	{
	}

	private void ExcuteLayer()
	{
		if (nextRoom != null)
		{
			bool flag = false;
			if (room != null)
			{
				flag = (room.RoomID == 0);
			}
			layerObj.SetActive(!nextRoom.IsBossRoom && !flag);
			bossObj.SetActive(nextRoom.IsBossRoom);
		}
		else
		{
			layerObj.SetActive(room == null || room.RoomID > 0);
			bossObj.SetActive(value: false);
		}
	}

	protected override void OnClearGoods()
	{
		base.GoodsParent.DestroyChildren();
	}

	protected override void OnClearGoodsDrop()
	{
		base.GoodsDropParent.DestroyChildren();
	}

	protected override Transform OnGetGoodsDropParent()
	{
		return base.GoodsDropParent;
	}

	private void OnInitStage()
	{
		int styleID = LocalModelManager.Instance.Stage_Level_stagechapter.GetStyleID();
		if (styleID == 3)
		{
			InitStage3();
		}
	}

	private void InitStage3()
	{
		if (room != null)
		{
			Transform transform = base.transform.Find("WallUp/nowater");
			if ((bool)transform)
			{
				transform.gameObject.SetActive(!GameLogic.Release.MapCreatorCtrl.GetStage3MiddleWater());
			}
		}
	}
}
