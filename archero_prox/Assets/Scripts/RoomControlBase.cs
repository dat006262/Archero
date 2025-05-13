




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class RoomControlBase : MonoBehaviour
{
	public class Mode_LevelData
	{
		public RoomGenerateBase.Room room;

		public RoomGenerateBase.Room nextroom;
	}

	protected bool m_bOpenDoor;

	protected object mInitData;

	private Transform cloudparent;

	private Transform cloud01;

	private Transform cloud02;

	private const float CloudMinDistance = 4f;

	private bool bCloudInit;

	private float mCloud01MoveTime;

	private float mCloud02MoveTime;

	private float cloud01y = -100f;

	private float cloud02y = -200f;

	private float randomheight;

	private Transform Collider_Parent;

	private Transform Collider_Door;

	private Transform Collider_Up;

	private Transform Collider_UpSide;

	private Transform Collider_Left;

	private Transform Collider_Right;

	private Transform Collider_Down;

	private Transform _GoodsDropParent;

	private Transform _GoodsParent;

	private TextMesh[] texts_layer;

	private MeshRenderer textMeshrenderer;

	protected Transform GoodsDropParent
	{
		get
		{
			if (_GoodsDropParent == null)
			{
				GameObject gameObject = new GameObject("GoodsDropParent");
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localRotation = Quaternion.identity;
				_GoodsDropParent = gameObject.transform;
			}
			return _GoodsDropParent;
		}
	}

	public Transform GoodsParent
	{
		get
		{
			if (_GoodsParent == null)
			{
				GameObject gameObject = new GameObject("GoodsParent");
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localRotation = Quaternion.identity;
				_GoodsParent = gameObject.transform;
			}
			return _GoodsParent;
		}
	}

	private void Awake()
	{
		ColliderAwake();
		CloudAwake();
		LayerAwake();
		OnAwake();
	}

	protected virtual void OnAwake()
	{
	}

	public void Init(object data = null)
	{
		mInitData = data;
		InitCloud();
		OnInit(data);
	}

	protected virtual void OnInit(object data = null)
	{
	}

	private void OnEnable()
	{
		OnEnabled();
	}

	protected virtual void OnEnabled()
	{
	}

	private void OnDisable()
	{
		OnDisabled();
	}

	protected virtual void OnDisabled()
	{
	}

	public void OpenDoor(bool value)
	{
		Collider_OpenDoor(value);
		OnOpenDoor(value);
	}

	protected virtual void OnOpenDoor(bool value)
	{
	}

	public bool IsDoorOpen()
	{
		return m_bOpenDoor;
	}

	public void LayerShow(bool value)
	{
		OnLayerShow(value);
	}

	protected virtual void OnLayerShow(bool value)
	{
	}

	public void SetText(string value)
	{
		OnSetText(value);
	}

	protected virtual void OnSetText(string value)
	{
	}

	public void Clear()
	{
		ClearGoods();
		ClearGoodsDrop();
	}

	public void ClearGoods()
	{
		OnClearGoods();
	}

	protected virtual void OnClearGoods()
	{
	}

	public void ClearGoodsDrop()
	{
		OnClearGoodsDrop();
	}

	protected virtual void OnClearGoodsDrop()
	{
	}

	public Transform GetGoodsDropParent()
	{
		return OnGetGoodsDropParent();
	}

	protected virtual Transform OnGetGoodsDropParent()
	{
		return null;
	}

	protected virtual void OnReceiveEvent(string eventName, object data)
	{
	}

	public void SendEvent(string eventName, object data = null)
	{
		OnReceiveEvent(eventName, data);
	}

	private void CloudAwake()
	{
		cloudparent = base.transform.Find("Cloud");
		if ((bool)cloudparent)
		{
			cloud01 = cloudparent.Find("cloud01");
			cloud02 = cloudparent.Find("cloud02");
		}
	}

	private void InitCloud()
	{
		if (!bCloudInit)
		{
			bCloudInit = true;
			randomheight = 9.5f;
			Mode_LevelData mode_LevelData = mInitData as Mode_LevelData;
			if (mode_LevelData != null && mode_LevelData.room != null)
			{
				randomheight = (float)mode_LevelData.room.RoomHeight / 2.2f;
			}
			if (GameLogic.Random(0, 100) < 50)
			{
				mCloud01MoveTime = GameLogic.Random(70f, 90f);
				mCloud02MoveTime = GameLogic.Random(120f, 140f);
			}
			else
			{
				mCloud02MoveTime = GameLogic.Random(70f, 90f);
				mCloud01MoveTime = GameLogic.Random(120f, 140f);
			}
			if ((bool)cloud01)
			{
				RandomCloudY(ref cloud01y, cloud02y);
				cloud01.localPosition = new Vector3(15f, 0f, cloud01y);
				cloud01.DOLocalMoveX(-15f, mCloud01MoveTime).OnStepComplete(delegate
				{
					RandomCloudY(ref cloud01y, cloud02y);
					cloud01.localPosition = new Vector3(15f, 0f, cloud01y);
				}).SetLoops(-1)
					.SetEase(Ease.Linear);
			}
			if ((bool)cloud02)
			{
				float num = GameLogic.Random(-5f, 5f);
				float num2 = (num + 15f) / 30f;
				RandomCloudY(ref cloud02y, cloud01y);
				cloud02.localPosition = new Vector3(num, 0f, cloud02y);
				cloud02.DOLocalMoveX(-15f, mCloud02MoveTime * num2).SetEase(Ease.Linear).OnComplete(delegate
				{
					Transform transform = cloud02;
					Vector3 localPosition = cloud02.localPosition;
					transform.localPosition = new Vector3(15f, 0f, localPosition.z);
					cloud02.DOLocalMoveX(-15f, mCloud02MoveTime).OnStepComplete(delegate
					{
						RandomCloudY(ref cloud02y, cloud01y);
						cloud02.localPosition = new Vector3(-15f, 0f, cloud02y);
					}).SetLoops(-1)
						.SetEase(Ease.Linear);
				});
			}
		}
	}

	private void RandomCloudY(ref float clouda, float cloudb)
	{
		clouda = GameLogic.Random(0f - randomheight, randomheight);
		while (MathDxx.Abs(clouda - cloudb) < 4f)
		{
			clouda = GameLogic.Random(0f - randomheight, randomheight);
		}
	}

	private void CloudUpdate()
	{
	}

	private void ColliderAwake()
	{
		Collider_Parent = base.transform.Find("Collider");
		if ((bool)Collider_Parent)
		{
			Collider_Door = Collider_Parent.Find("door");
			Collider_Up = Collider_Parent.Find("up");
			Collider_UpSide = Collider_Parent.Find("upside");
			Collider_Left = Collider_Parent.Find("left");
			Collider_Right = Collider_Parent.Find("right");
			Collider_Down = Collider_Parent.Find("down");
		}
	}

	private void Collider_OpenDoor(bool open)
	{
		if ((bool)Collider_Door)
		{
			Collider_Door.gameObject.SetActive(open);
		}
		if ((bool)Collider_Up)
		{
			Collider_Up.gameObject.SetActive(!open);
		}
		if ((bool)Collider_UpSide)
		{
			Collider_UpSide.gameObject.SetActive(open);
		}
	}

	private void LayerAwake()
	{
		texts_layer = base.transform.GetComponentsInChildren<TextMesh>(includeInactive: true);
		int i = 0;
		for (int num = texts_layer.Length; i < num; i++)
		{
			textMeshrenderer = texts_layer[i].GetComponent<MeshRenderer>();
			textMeshrenderer.sortingLayerName = "Player";
			textMeshrenderer.sortingOrder = 990 + i;
		}
	}

	protected void SetLayer(int layer)
	{
		SetLayer(layer.ToString());
	}

	protected void SetLayer(string value)
	{
		int i = 0;
		for (int num = texts_layer.Length; i < num; i++)
		{
			texts_layer[i].text = value;
		}
	}
}
