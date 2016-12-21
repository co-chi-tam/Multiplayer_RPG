using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CSceneManager : CMonoSingleton<CSceneManager> {

		#region Properties

		[SerializeField]	private Color m_ScreenLoadingColor = Color.white;
		[Range(0.25f, 5f)]
		[SerializeField]	private float m_ScreenLoadingTime = 1f;

		public UnityEvent OnSceneStartLoad;
		public UnityEvent OnSceneLoaded;

		private Texture2D m_LoadingScreenTexture;
		private Rect m_FullScreenRect;
		private bool m_LevelWasLoaded;

		#endregion

		#region Monobehaviour

		protected override void Awake ()
		{
			base.Awake ();
			m_FullScreenRect = new Rect (0f, 0f, Screen.width, Screen.height);
			m_LevelWasLoaded = true;
			OnResetLoadingScreen ();
			DontDestroyOnLoad (this.gameObject);
		}

		protected override void Start ()
		{
			base.Start ();
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
		}

		protected virtual void OnGUI() {
			if (Event.current.type.Equals (EventType.Repaint)) {
				GUI.DrawTexture (m_FullScreenRect, m_LoadingScreenTexture, ScaleMode.StretchToFill);
				if (m_LevelWasLoaded) {
					var currentColor = m_LoadingScreenTexture.GetPixels () [0];
					currentColor.a -= 1f / m_ScreenLoadingTime * Time.deltaTime;
					m_LoadingScreenTexture.SetPixels (new Color[] { currentColor });
					m_LoadingScreenTexture.Apply ();
					m_LevelWasLoaded = currentColor.a > 0f;
				}
			}
		}

		#endregion 

		#region Main methods

		private void OnResetLoadingScreen() {
			m_LoadingScreenTexture = new Texture2D (1, 1);
			m_LoadingScreenTexture.SetPixels (new Color[] { m_ScreenLoadingColor });
			m_LoadingScreenTexture.Apply ();
		}

		public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
		{
			OnResetLoadingScreen ();
			m_LevelWasLoaded = true;
			OnSceneLoaded.Invoke ();
		}

		public void LoadScene(string name) {
			OnResetLoadingScreen ();
			m_LevelWasLoaded = false;
			SceneManager.LoadScene (name);
			OnSceneStartLoad.Invoke ();
		}

		public void LoadSceneAsync(string name) {
			OnResetLoadingScreen ();
			m_LevelWasLoaded = false;
			SceneManager.LoadSceneAsync (name);
			OnSceneStartLoad.Invoke ();
		}
	
		#endregion

	}
}
