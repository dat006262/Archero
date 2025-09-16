




using UnityEngine;

public class BoxRedAniCtrl : MonoBehaviour
{
	private GameObject Obj_RedNode;

	private Animator BoxAni;

	private ButtonCtrl Button_Box;

	private void Awake()
	{
		Obj_RedNode = base.transform.Find("Button_Box/fg/Image_RedNode").gameObject;
		BoxAni = GetComponent<Animator>();
		Button_Box = base.transform.Find("Button_Box").GetComponent<ButtonCtrl>();
		Button_Box.onClick = delegate
		{
			BoxAniPlay(play: false);
			WindowUI.ShowWindow(WindowID.WindowID_BoxOpen);
		};
	}

	private void OnEnable()
	{
		if ((bool)BoxAni)
		{
			BoxAni.enabled = true;
		}
	}

	private void OnDisable()
	{
		if ((bool)BoxAni)
		{
			BoxAni.enabled = false;
		}
	}

	public void UpdateCanOpen(int count)
	{
	}

	public void UpdateBox()
	{
	}

	private void BoxAniPlay(bool play)
	{
		if ((bool)BoxAni)
		{
			BoxAni.enabled = play;
			if (!play)
			{
				BoxAni.transform.localRotation = Quaternion.identity;
				BoxAni.transform.localScale = Vector3.one;
			}
		}
	}
}
