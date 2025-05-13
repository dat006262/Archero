




using UnityEngine;

public class GuideArrowCtrl : MonoBehaviour
{
	private Animation ani;

	private Vector3 pos;

	private bool bShow = true;

	private float mindis = 5f;

	private void Awake()
	{
		ani = base.transform.Find("arrowparent/arrow").GetComponent<Animation>();
	}

	private void Start()
	{
		pos = base.transform.position;
		bShow = true;
	}

	private void Update()
	{
		if ((bool)GameLogic.Self)
		{
			float num = Vector3.Distance(GameLogic.Self.position, pos);
			if (num < mindis && bShow)
			{
				ani.Play("guidearrow_miss");
				bShow = false;
			}
			else if (num > mindis + 1f && !bShow)
			{
				ani.Play("guidearrow_show");
				bShow = true;
			}
		}
	}
}
