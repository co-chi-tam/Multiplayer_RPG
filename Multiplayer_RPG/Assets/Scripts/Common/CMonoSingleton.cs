using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CMonoSingleton<T>: CBaseMonoBehaviour, ITask where T : MonoBehaviour {

		#region Singleton

		protected static T m_Instance;
		private static object m_SingletonObject = new object();
		public static T Instance {
			get { 
				lock (m_SingletonObject) {
					if (m_Instance == null) {
						var resourceLoads = Resources.LoadAll<T> ("");
						GameObject go = null;
						if (resourceLoads.Length == 0) {
							go = new GameObject ();
							m_Instance = go.AddComponent<T> ();
						} else {
							go = Instantiate (resourceLoads [0].gameObject);
							m_Instance = go.GetComponent<T> ();
						}
						go.SetActive (true);
						go.name = typeof(T).Name;
					}
					return m_Instance;
				}
			}
		}

		public static T GetInstance() {
			return Instance;
		}

		#endregion

		#region Implementation Monobehaviour

		protected override void Awake() {
			base.Awake ();
			m_Instance = this as T;
		}

		#endregion

		#region ITask

		public virtual bool OnTask() {
			return true;
		}

		public virtual float OnTaskProcess() {
			return 1f;
		}

		public virtual string GetTaskName() {
			return typeof(T).Name;
		}

		#endregion

	}
}
