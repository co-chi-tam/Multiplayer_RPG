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

		private Queue<ITask> m_SceneTask = new Queue<ITask> ();

		#endregion

		#region Monobehaviour

		protected override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (this.gameObject);
		}

		protected override void Start ()
		{
			base.Start ();
			this.OnResetLoadingScreen ();
			this.OnSceneStartLoad.Invoke ();
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
		}

		protected override void Update ()
		{
			base.Update ();
			if (m_SceneTask.Count > 0) {
				var task = m_SceneTask.Peek ();
				if (task.OnTask ()) {
					m_SceneTask.Dequeue ();
				}
				if (m_SceneTask.Count == 0) {
					this.OnSceneLoaded.Invoke ();
				}
			}
		}

		#endregion 

		#region Main methods

		public virtual void OnRegisterTask(ITask task) {
			m_SceneTask.Enqueue (task);
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

	}
}
