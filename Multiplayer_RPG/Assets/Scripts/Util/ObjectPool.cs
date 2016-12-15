using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPool
{
	public class ObjectPool<T> where T : class
	{

		// Using list
		private LinkedList<T> m_ListUsing;
		// Waiint list
		private Stack<T> m_ListWaiting;

		public ObjectPool()
		{
			m_ListUsing = new LinkedList<T>();
			m_ListWaiting = new Stack<T>();
		}

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

		// Create object in waiting list
		public void Create(T item)
		{
			if (item == null) {
				return;
			}
			if (m_ListWaiting.Contains (item) == false) {
				m_ListWaiting.Push (item);
			} 
		}

		// Add existed object 
		public void Add(T item) {
			if (item == null || m_ListWaiting.Contains (item) || m_ListUsing.Contains (item)) {
				return;
			}
			m_ListWaiting.Push (item);
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
	}
}