




using DG.Tweening;
using UnityEngine;

public class Bullet1083Ctrl : MonoBehaviour
{
	public Transform lightning;

	public GameObject hit;

	private Vector3 startscale = new Vector3(1f, 0f, 1f);

	private Sequence seq;

	private void OnEnable()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
		seq = DOTween.Sequence();
		hit.SetActive(value: false);
		lightning.localScale = startscale;
		seq.Append(lightning.DOScaleY(1f, 0.1f));
		seq.AppendCallback(delegate
		{
			hit.SetActive(value: true);
		});
		seq.AppendInterval(0.2f);
		seq.Append(lightning.DOScaleX(0f, 0.2f));
	}
}
