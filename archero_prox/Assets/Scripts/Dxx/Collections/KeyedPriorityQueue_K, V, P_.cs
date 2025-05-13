




using System;
using System.Collections.Generic;

namespace Dxx.Collections
{
	public class KeyedPriorityQueue<K, V, P>
	{
		public delegate void KeyedPriorityQueueHeadChangeDelegate(KeyedPriorityQueue<K, V, P> q, V oriHead, V newHead);

		public delegate int CompareDelegate(P p1, P p2);

		private class Node
		{
			public K key;

			public V value;

			public P priority;

			public int index;
		}

		public CompareDelegate priorityComparer;

		private Comparer<P> mComparer = Comparer<P>.Default;

		private Dictionary<K, Node> mDict = new Dictionary<K, Node>();

		private List<Node> mHeap = new List<Node>();

		private int mCount;

		public int Count => mCount;

		public event KeyedPriorityQueueHeadChangeDelegate onHeadChanged;

		public KeyedPriorityQueue()
		{
			mHeap.Add(null);
		}

		public void Enqueue(K key, V value, P priority)
		{
			if (mDict.ContainsKey(key))
			{
				throw new ArgumentException("An element with the same key already exists in the KeyedPriorityQueue");
			}
			V oriHead = (mCount <= 0) ? default(V) : mHeap[1].value;
			int num = ++mCount;
			if (mCount == mHeap.Count)
			{
				mHeap.Add(new Node());
			}
			Node node = mHeap[mCount];
			node.key = key;
			node.value = value;
			node.priority = priority;
			int num2 = num >> 1;
			while (num2 > 0 && Compare(priority, mHeap[num2].priority) < 0)
			{
				mHeap[num] = mHeap[num2];
				mHeap[num].index = num;
				num = num2;
				num2 = num >> 1;
			}
			mHeap[num] = node;
			mDict.Add(key, node);
			node.index = num;
			if (num == 1 && this.onHeadChanged != null)
			{
				this.onHeadChanged(this, oriHead, mHeap[1].value);
			}
		}

		public V Dequeue()
		{
			if (mCount <= 0)
			{
				throw new InvalidOperationException("Empty Queue");
			}
			Node node = mHeap[1];
			mHeap[1] = mHeap[mCount];
			mHeap[1].index = 1;
			mHeap[mCount] = node;
			mCount--;
			mDict.Remove(node.key);
			Heapify(1);
			V value = node.value;
			node.key = default(K);
			node.value = default(V);
			node.priority = default(P);
			return value;
		}

		public V Dequeue(out K key, out P priority)
		{
			if (mCount <= 0)
			{
				throw new InvalidOperationException("Empty Queue");
			}
			Node node = mHeap[1];
			mHeap[1] = mHeap[mCount];
			mHeap[1].index = 1;
			mHeap[mCount] = node;
			mCount--;
			mDict.Remove(node.key);
			Heapify(1);
			key = node.key;
			priority = node.priority;
			V value = node.value;
			node.key = default(K);
			node.value = default(V);
			node.priority = default(P);
			return value;
		}

		public V Peek()
		{
			if (mCount <= 0)
			{
				throw new InvalidOperationException("Empty Queue");
			}
			Node node = mHeap[1];
			return node.value;
		}

		public V Peek(out K key, out P priority)
		{
			if (mCount <= 0)
			{
				throw new InvalidOperationException("Empty Queue");
			}
			Node node = mHeap[1];
			key = node.key;
			priority = node.priority;
			return node.value;
		}

		public bool RemoveFromQueue(K key)
		{
			if (mCount <= 0)
			{
				return false;
			}
			if (!mDict.TryGetValue(key, out Node value))
			{
				return false;
			}
			mDict.Remove(key);
			int index = value.index;
			mHeap[index] = mHeap[mCount];
			mHeap[mCount] = value;
			mHeap[index].index = index;
			mCount--;
			Heapify(index);
			HeapUp(index);
			if (index == 1 && this.onHeadChanged != null)
			{
				this.onHeadChanged(this, value.value, (mCount <= 0) ? default(V) : mHeap[1].value);
			}
			value.key = default(K);
			value.value = default(V);
			value.priority = default(P);
			return true;
		}

		public bool TryGetItem(K key, out V value)
		{
			value = default(V);
			if (mCount <= 0)
			{
				return false;
			}
			P priority;
			return TryGetItem(key, out value, out priority);
		}

		public bool TryGetItem(K key, out V value, out P priority)
		{
			if (mCount <= 0)
			{
				value = default(V);
				priority = default(P);
				return false;
			}
			if (mDict.TryGetValue(key, out Node value2))
			{
				value = value2.value;
				priority = value2.priority;
				return true;
			}
			value = default(V);
			priority = default(P);
			return false;
		}

		public bool Contains(K key)
		{
			return mDict.ContainsKey(key);
		}

		public void Clear()
		{
			mHeap.Clear();
			mDict.Clear();
			mHeap.Add(null);
			mCount = 0;
		}

		private int Compare(P p1, P p2)
		{
			if (priorityComparer != null)
			{
				return priorityComparer(p1, p2);
			}
			return mComparer.Compare(p1, p2);
		}

		private void Heapify(int i)
		{
			int num = i << 1;
			int num2 = num + 1;
			int num3 = i;
			if (num <= mCount && Compare(mHeap[num].priority, mHeap[num3].priority) < 0)
			{
				num3 = num;
			}
			if (num2 <= mCount && Compare(mHeap[num2].priority, mHeap[num3].priority) < 0)
			{
				num3 = num2;
			}
			if (num3 != i)
			{
				Node node = mHeap[num3];
				mHeap[num3] = mHeap[i];
				mHeap[num3].index = num3;
				mHeap[i] = node;
				node.index = i;
				Heapify(num3);
			}
		}

		private int HeapUp(int i)
		{
			Node node = mHeap[i];
			P priority = node.priority;
			int num = i >> 1;
			while (num > 0 && Compare(priority, mHeap[num].priority) < 0)
			{
				mHeap[i] = mHeap[num];
				mHeap[i].index = i;
				i = num;
				num = i >> 1;
			}
			mHeap[i] = node;
			node.index = i;
			return i;
		}
	}
}
