using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CEventListenerComponent : Component {
			
		protected IEventListener m_Listener;

		protected Dictionary<string, CEventListener> m_ListenMap;

		public CEventListenerComponent (IEventListener listener) : base()
		{
			this.m_Listener = listener;
			this.m_ListenMap = new Dictionary<string, CEventListener> ();
		}

		public virtual void InvokeEventListener(string name, object value) {
			if (this.m_ListenMap.ContainsKey (name) == false) {
				this.m_ListenMap [name] = new CEventListener ();
			}
			this.m_ListenMap [name].Invoke (value);
		}

		public virtual void AddEventListener(string name, Action<object> onEvent) {
			if (this.m_ListenMap.ContainsKey (name) == false) {
				this.m_ListenMap [name] = new CEventListener ();
			}
			this.m_ListenMap [name].AddListener (onEvent);
		}

		public virtual void RemoveEventListener(string name, Action<object> onEvent) {
			if (this.m_ListenMap.ContainsKey (name) == false) {
				this.m_ListenMap [name] = new CEventListener ();
			}
			this.m_ListenMap [name].RemoveListener (onEvent);
		}

		public virtual void RemoveAllEventListener(string name) {
			if (this.m_ListenMap.ContainsKey (name) == false) {
				this.m_ListenMap [name] = new CEventListener ();
			}
			this.m_ListenMap [name].RemoveAllListener ();
		}

	}
}
