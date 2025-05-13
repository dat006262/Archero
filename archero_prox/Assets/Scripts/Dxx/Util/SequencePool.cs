




using DG.Tweening;
using System.Collections.Generic;

namespace Dxx.Util
{
	public class SequencePool
	{
		private List<Sequence> mList = new List<Sequence>();

		public void Add(Sequence seq)
		{
			mList.Add(seq);
		}

		public Sequence Get()
		{
			Sequence sequence = DOTween.Sequence();
			mList.Add(sequence);
			return sequence;
		}

		public void Clear()
		{
			int i = 0;
			for (int count = mList.Count; i < count; i++)
			{
				Sequence sequence = mList[i];
				if (sequence != null)
				{
					sequence.Kill();
					sequence = null;
				}
			}
			mList.Clear();
		}
	}
}
