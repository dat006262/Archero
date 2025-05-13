




using DG.Tweening;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
	public string AniString;

	public string AniString2;

	private Animation ani;

	private float time;

	private void Start()
	{
		ani = GetComponent<Animation>();
		float num = 0.5f;
		ani[AniString].speed = num;
		ani[AniString2].speed = num;
		time = ani[AniString].length / num;
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.A))
		{
			ani.Play(AniString);
			DOTween.Sequence().AppendInterval(time).AppendCallback(delegate
			{
				ani.Play(AniString2);
			});
		}
	}
}
