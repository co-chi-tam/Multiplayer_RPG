using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SequenceList<T> where T : class {

	#region Properties

	private LinkedList<T> m_Queue;

	public T this[int index] {
		get { 
			var count = m_Queue.Count;
			var node = m_Queue.GetEnumerator ();
			var i = 0;
			while (node.MoveNext ()) {
				if (i == count - index - 1) {
					return node.Current;
				}
				i++;
			}
			return null;
		}
	}

	public int Count {
		get { return m_Queue.Count; }
	}

	#endregion

	#region Contructor

	public SequenceList ()
	{
		m_Queue = new LinkedList<T> ();
	}

	#endregion

	#region Main methods

	public T Pop(int index) {
		var tmp = this [index];
		if (m_Queue.Remove (tmp)) {
			Enqueue (tmp);
		}
		return tmp;
	}

	public T Peek() {
		if (m_Queue.Last == null)
			return default(T);
		return m_Queue.Last.Value;
	}

	public void AddFirst(T value) {
		m_Queue.AddFirst (value);
	}

	public void AddLast(T value) {
		m_Queue.AddLast (value);
	}

	public void Enqueue(T value) {
		m_Queue.AddFirst (value);
	}

	public T Dequeue(bool continueQueue = true) {
		var lastQueue = m_Queue.Last.Value;
		m_Queue.RemoveLast ();
		if (continueQueue == true) {
			Enqueue (lastQueue);
		}
		return lastQueue;
	}

	public bool Remove(T value) {
		return m_Queue.Remove (value);
	}

	public bool Remove(int index) {
		var value = this [index];
		return m_Queue.Remove (value);
	}

	public bool Contain(T value) {
		return m_Queue.Contains (value);
	}

	public void Clear() {
		m_Queue.Clear ();
	}

	#endregion

}
