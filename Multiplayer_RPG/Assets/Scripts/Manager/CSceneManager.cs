using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CSceneManager : CMonoSingleton<CSceneManager> {

		#region Properties

		public UnityEvent OnSceneStartLoad;
		public UnityEvent OnSceneLoaded;

		private LinkedList<ITask> m_SceneTask = new LinkedList<ITask> ();

		#endregion

		#region Monobehaviour

		protected override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (this.gameObject);
			this.OnResetLoadingScreen ();
			this.OnSceneStartLoad.Invoke ();
		}

		protected override void Start ()
		{
			base.Start ();
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
		}

		protected override void Update ()
		{
			base.Update ();
			if (m_SceneTask.Count > 0) {
				var task = m_SceneTask.First.Value;
				if (task.OnTask ()) {
					m_SceneTask.RemoveFirst();
				}
				if (m_SceneTask.Count == 0) {
					this.OnSceneLoaded.Invoke ();
				}
			} 
		}

		#endregion 

		#region Main methods

		public virtual void OnRegisterTask(ITask task) {
			m_SceneTask.AddLast (task);
		}

		public virtual void OnUnregisterTask(ITask task) {
			m_SceneTask.Remove (task);
		}

		public virtual void RemoveAllTask() {
			m_SceneTask.Clear ();
		}

		public void OnResetLoadingScreen() {
			
		}

		public virtual void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
		{
			this.OnResetLoadingScreen ();
			if (m_SceneTask.Count == 0) {
				this.OnSceneLoaded.Invoke ();
			}
		}

		public virtual void LoadScene(string name) {
			this.OnResetLoadingScreen ();
			SceneManager.LoadScene (name);
			this.OnSceneStartLoad.Invoke ();
		}

		public virtual void LoadSceneAsync(string name) {
			this.OnResetLoadingScreen ();
			SceneManager.LoadSceneAsync (name);
			this.OnSceneStartLoad.Invoke ();
		}
	
		#endregion

		#region Getter && Setter

		public string GetSceneName() {
			return SceneManager.GetActiveScene ().name;
		}

		#endregion

	}
}
