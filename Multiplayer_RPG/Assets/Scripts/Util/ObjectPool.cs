using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPool
{
	public class ObjectPool<T> where T : class
	{

		#region Properties

		// Using list
		private LinkedList<T> m_ListUsing;
		// Waiint list
		private Stack<T> m_ListWaiting;

		#endregion

		#region Contructor

		public ObjectPool()
		{
			m_ListUsing = new LinkedList<T>();
			m_ListWaiting = new Stack<T>();
		}

		#endregion

		#region Main methods

		// Find Using item
		public T FindUsingItem(Func<T, bool> onCondition) {
			if (onCondition != null) {
				foreach (var item in m_ListUsing) {
					if (onCondition (item)) {
						return item;
					}
				}
			}
			return default (T);
		}

		// Find Using item
		public T FindWaitingItem(Func<T, bool> onCondition) {
			if (onCondition != null) {
				foreach (var item in m_ListWaiting) {
					if (onCondition (item)) {
						return item;
					}
				}
			}
			return default (T);
		}

		// Get Object on free
		public T Get()
		{
			if (CountWaiting () > 0) {
				T tmp = m_ListWaiting.Pop ();
				m_ListUsing.AddFirst (tmp);
				return tmp;
			} 
			return default (T);
		}

		// Get Object on free
		public bool Get(ref T value)
		{
			if (CountWaiting() > 0)
			{
				value = m_ListWaiting.Pop();
				m_ListUsing.AddFirst(value);
				return true;
			}
			return false;
		}

		// Return to reuse
		public void Set(T item)
		{
			if (item == null) return;
			if (m_ListUsing.Contains(item)) {
				m_ListUsing.Remove(item);
			}
			m_ListWaiting.Push(item);
		}

		// Return to reuse
		public void Set(int index)
		{
			T tmp = m_ListUsing.ElementAt (index);
			if (tmp == null) return;
			m_ListUsing.Remove(tmp);
			m_ListWaiting.Push(tmp);
		}

		// Contains Using
		public bool ContainsUsing(T value) {
			return m_ListUsing.Contains (value);
		}

		// Contains Waiting
		public bool ContainsWaiting(T value) {
			return m_ListWaiting.Contains (value);
		}

		// Count using list
		public int Count() {
			return m_ListUsing.Count;
		}

		// Count waiting list
		public int CountWaiting() {
			return m_ListWaiting.Count;
		}

		// Get element at index.
		public T ElementUsingAtIndex(int index) {
			if (index > m_ListUsing.Count - 1)
				return default (T);
			return m_ListUsing.ElementAt (index);
		}

		#endregion

	}
}