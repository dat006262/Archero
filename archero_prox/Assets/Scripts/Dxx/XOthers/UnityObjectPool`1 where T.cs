




using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dxx
{
	public class UnityObjectPool<T> where T : UnityEngine.Object
	{
		protected Stack<T> m_Stack = new Stack<T>();

		protected Func<T> m_actionCreate;

		protected Action<T> m_ActionOnGet;

		protected Action<T> m_ActionOnRelease;

		private T origin;

		public int countAll
		{
			get;
			private set;
		}

		public int countActive => countAll - countInactive;

		public int countInactive => m_Stack.Count;

		public UnityObjectPool(Func<T> actionCreate, Action<T> actionOnGet, Action<T> actionOnRelease)
		{
			m_actionCreate = actionCreate;
			m_ActionOnGet = actionOnGet;
			m_ActionOnRelease = actionOnRelease;
		}

		public T Get()
		{
			T val;
			if (m_Stack.Count == 0)
			{
				val = m_actionCreate();
				countAll++;
			}
			else
			{
				val = m_Stack.Pop();
			}
			if (m_ActionOnGet != null)
			{
				m_ActionOnGet(val);
			}
			return val;
		}

		public void Release(T element)
		{
			if (m_Stack.Count > 0 && object.ReferenceEquals(m_Stack.Peek(), element))
			{
				UnityEngine.Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
			}
			if (m_ActionOnRelease != null)
			{
				m_ActionOnRelease(element);
			}
			m_Stack.Push(element);
		}
	}
}
