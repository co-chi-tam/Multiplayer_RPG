using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CResourceManager : CMonoSingleton <CResourceManager> {

		private Dictionary<string, AssetBundle> m_AssetResourceLoader = null;
		private string m_ServerCheckUrl = "https://www.dropbox.com/s/ivrd48us6z2hyts/cactus_leafy_go.go?dl=1";
		private int m_Version = 0;
		private string m_VersionStr = "v0.0";

		private CWWW m_ServerRequest;
		private float m_Processing = 0f;
		private bool m_Handling = false;
		private float m_TimeOut = 300f;

		protected override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (this.gameObject);
			m_AssetResourceLoader = new Dictionary<string, AssetBundle> ();
			m_VersionStr = PlayerPrefs.GetString ("VERSION_LOCAL", "v0.0");
		}

		protected override void Start ()
		{
			base.Start ();
			// Register task
			CSceneManager.Instance.OnRegisterTask (this);
			// Test: Check server sersion.
			m_ServerRequest = new CWWW ();
			m_ServerRequest.Get ("https://www.google.com.vn", (result) => {
				m_ServerRequest.HandleCoroutine (HandleResourceLoader(m_ServerCheckUrl, m_Version, (x) => {
					this.OnLoadResourceComplete (m_VersionStr, x); 
				}, (e) => {
					this.OnLoadResourceFail(m_VersionStr, e);
				}));
			}, (error) => {
				this.OnLoadResourceFail(m_VersionStr, error);
			});
		}

		protected virtual void OnLoadResourceComplete(string version, AssetBundle bundle) {
			if (m_AssetResourceLoader.ContainsKey (version) == false) {
				m_AssetResourceLoader.Add (version, bundle);
			}
		}

		protected virtual void OnLoadResourceFail(string version, string error) {
			Caching.CleanCache();
			m_AssetResourceLoader.Clear();
			CSceneManager.Instance.OnUnregisterTask (this);
			CLog.Debug ("Fail version: {0} - error {1}", version, error);
		}

		public virtual bool OnResourceLoaded() {
			return m_AssetResourceLoader.Count > 0;
		}

		public virtual T LoadAsset<T>(string name) where T : UnityEngine.Object {
			var lowerCaseName = name.ToLower ();
			foreach (var item in m_AssetResourceLoader) {
				if (item.Value.Contains (name)) {
					return item.Value.LoadAsset<T> (lowerCaseName);
				}
			}
			return default (T);
		}

		public virtual T LoadResourceOrAsset<T>(string name) where T : UnityEngine.Object {
			var resourceLoader = Resources.LoadAll<T>("");
			for (int i = 0; i < resourceLoader.Length; i++) {
				if (resourceLoader[i].name == name) {
					return resourceLoader[i] as T;
				}
			}
			if (m_AssetResourceLoader != null) {
				var assetObject = this.LoadAsset<T> (name);
				if (assetObject != null) {
					return assetObject;
				}
			}
			return default (T);
		}

		private IEnumerator HandleResourceLoader(string url, int version, Action<AssetBundle> onComplete, Action<string> onError) {
			while (!Caching.ready) // Caching already
				yield return null;	
			if (m_Handling == true) // Handle request
				yield break;
			m_Handling = true;
			var www = WWW.LoadFromCacheOrDownload (url, version);
			var timeProcessing = 0f;
			while (www.isDone == false && timeProcessing <= m_TimeOut) {
				timeProcessing += Time.deltaTime;
				m_Processing = www.progress;
				yield return WaitHelper.WaitFixedUpdate;
			}
			if (www.isDone == false && timeProcessing >= m_TimeOut) {
				if (onError != null) {
					onError ("WWW download had an error: Request time out....");
				}
				m_Handling = false;
				yield break;
			}
			yield return www;
			if (www.error != null) {
				if (onError != null) {
					onError ("WWW download had an error: " + www.error);
				}
			} else {
				if (onComplete != null && www.assetBundle != null) {
					onComplete (www.assetBundle);
				}
			}
			m_Handling = false;
			m_Processing = 1f;
			www.Dispose ();
		}

		private GameObject ReloadGameObject(GameObject gObj) {
			gObj.transform.position = Vector3.zero;
			gObj.transform.rotation = Quaternion.identity;
			gObj.transform.localScale = Vector3.one;
			var render = gObj.GetComponent<Renderer> ();
			if (render != null) {
				render.material.shader = Shader.Find (render.material.shader.name);
			}
			return gObj;
		}

		public override bool OnTask() {
			base.OnTask ();
			return this.OnResourceLoaded ();
		}

		public override float OnTaskProcess ()
		{
			base.OnTaskProcess ();
			return m_Processing;
		}

	}
}
