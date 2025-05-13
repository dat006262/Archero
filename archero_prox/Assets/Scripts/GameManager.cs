




using UnityEngine;

public class GameManager
{
	public enum GameState
	{
		eMain,
		eGaming
	}

	private RoomState roomState;

	private int talentId;

	private int skillId;

	private int sickId;

	private GameObject MoveJoy => GameLogic.Release.Mode.GetMoveJoy();

	public RoomState RoomState => roomState;

	public GameState gameState
	{
		get;
		private set;
	}

	public GameManager()
	{
		Init();
	}

	public void Release()
	{
	}

	private void Init()
	{
	}

	public void SetRoomState(RoomState state)
	{
		roomState = state;
	}

	public void SetRunning()
	{
		SetRoomState(RoomState.Runing);
	}

	public void SetGameState(GameState state)
	{
		gameState = state;
		switch (gameState)
		{
		case GameState.eMain:
			WindowUI.GameOver();
			break;
		case GameState.eGaming:
			WindowUI.GameBegin();
			break;
		}
	}

	public void StartGame()
	{
	}

	public void EndGame()
	{
		CameraControlM.Instance.CameraPositionZero();
		RemoveJoy();
	}

	public void ShowJoy(bool show)
	{
		if ((bool)MoveJoy)
		{
			MoveJoy.SetActive(show);
		}
	}

	public void RemoveJoy()
	{
		if ((bool)MoveJoy)
		{
			UnityEngine.Object.Destroy(MoveJoy.gameObject);
		}
	}

	public void JoyEnable(bool enable)
	{
		if ((bool)GameLogic.Self)
		{
			if (enable)
			{
				GameLogic.Self.m_MoveCtrl.RegisterJoyEvent();
				GameLogic.Self.m_AttackCtrl.RegisterJoyEvent();
				ShowJoy(show: true);
			}
			else
			{
				GameLogic.Self.m_MoveCtrl.RemoveJoyEvent();
				GameLogic.Self.m_AttackCtrl.RemoveJoyEvent();
				GameLogic.Self.m_MoveCtrl.OnMoveEnd();
				GameLogic.Self.m_AttackCtrl.OnMoveEnd();
				ShowJoy(show: false);
			}
		}
	}

	public void SaveHeirChooseData(int talentId, int skillId, int sickId)
	{
		this.talentId = talentId;
		this.skillId = skillId;
		this.sickId = sickId;
	}
}
