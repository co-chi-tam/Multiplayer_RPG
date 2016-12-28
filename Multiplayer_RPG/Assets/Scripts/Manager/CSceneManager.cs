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
		[SerializeField]	private Texture2D m_LoadingTexture;
		[SerializeField]	private Vector2 m_LoadingGrid = Vector2.one;
		[Range(0.25f, 5f)]
		[SerializeField]	private float m_ScreenLoadingTime = 1f;

		public UnityEvent OnSceneStartLoad;
		public Func<bool> OnSceneProcess;
		public UnityEvent OnSceneLoaded;

		private Texture2D m_LoadingBackgroundTexture;
		private Rect m_FullScreenRect;
		private Rect m_LoadingSpriteRect;
		private bool m_LevelWasLoaded;
		private bool m_ScreenProcessing;
		private float m_LoadingX;
		private float m_LoadingY;

		#endregion

		#region Monobehaviour

		protected override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (this.gameObject);
			m_FullScreenRect = new Rect (0f, 0f, Screen.width, Screen.height);
			m_LoadingSpriteRect = new Rect (0, 0, (Screen.width / 100f) * 10f, (Screen.width / 100f) * 10f);
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
		}

		protected override void Start ()
		{
			base.Start ();
			OnResetLoadingScreen ();
		}

		protected virtual void OnGUI() {
			if (Event.current.type.Equals (EventType.Repaint) && m_ScreenProcessing) {
				// Background
				GUI.DrawTexture (m_FullScreenRect, m_LoadingBackgroundTexture, ScaleMode.StretchToFill);
				// Loading
				m_LoadingX = ((int)(Time.time * 10f) / m_LoadingGrid.x) % 1f;
				m_LoadingY = 0f;
				GUI.DrawTextureWithTexCoords (m_LoadingSpriteRect, 
					m_LoadingTexture, 
					new Rect (m_LoadingX, m_LoadingY, 1f / m_LoadingGrid.x, 1f / m_LoadingGrid.y));
				if (m_LevelWasLoaded) {
					this.OnDrawBackgroundScreen ();
				} else {
					var onProcess = true;
					if (this.OnSceneProcess != null) {
						onProcess &= this.OnSceneProcess ();
					}
					m_LevelWasLoaded = onProcess;
				}
			}
		}

		#endregion 

		#region Main methods

		public virtual void OnRegisterTask(ITask task) {
			this.OnSceneProcess += task.OnTasked;	
		}

		public void OnDrawBackgroundScreen() {
			var currentColor = m_LoadingBackgroundTexture.GetPixels () [0];
			currentColor.a -= 1f / m_ScreenLoadingTime * Time.deltaTime;
			m_LoadingBackgroundTexture.SetPixels (new Color[] { currentColor });
			m_LoadingBackgroundTexture.Apply ();
			m_ScreenProcessing = currentColor.a > 0f;
		}

		public void OnResetLoadingScreen() {
			m_LoadingBackgroundTexture = new Texture2D (1, 1);
			m_LoadingBackgroundTexture.SetPixels (new Color[] { m_ScreenLoadingColor });
			m_LoadingBackgroundTexture.Apply ();
			m_ScreenProcessing = true;
		}

		public virtual void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
		{
			this.OnResetLoadingScreen ();
			this.OnSceneLoaded.Invoke ();
		}

		public virtual void LoadScene(string name) {
			this.OnResetLoadingScreen ();
			SceneManager.LoadScene (name);
			this.OnSceneStartLoad.Invoke ();
			m_LevelWasLoaded = this.OnSceneProcess == null;
		}

		public virtual void LoadSceneAsync(string name) {
			this.OnResetLoadingScreen ();
			SceneManager.LoadSceneAsync (name);
			this.OnSceneStartLoad.Invoke ();
			m_LevelWasLoaded = this.OnSceneProcess == null;
		}
	
		#endregion

	}
}
