using UnityEngine;
using System;
using System.Collections;

namespace SurvivalTest {
	public class CResourceManager : CMonoSingleton <CResourceManager> {

		private AssetBundle m_AssetResourceLoader = null;
		private string m_ResourceUrl = "https://www.dropbox.com/s/075ukwtqvzdokcj/bundle-contain.sd?dl=1";
		private int m_Version = 1;

		protected override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (this.gameObject);
			CSceneManager.Instance.OnRegisterTask (this);
		}

		protected override void Start ()
		{
			base.Start ();
			StartCoroutine (HandleResourceLoader(m_ResourceUrl, m_Version, (x) => {
				this.OnLoadResourceComplete (x); 
			}, (e) => {
				this.OnLoadResourceFail(e);
			}));
		}

		protected virtual void OnLoadResourceComplete(AssetBundle bundle) {
			m_AssetResourceLoader = bundle;
		}

		protected virtual void OnLoadResourceFail(string error) {
			m_AssetResourceLoader = null;
			Caching.CleanCache();
			CLog.Debug ("Fail " + error);
		}

		public virtual bool OnResourceLoaded() {
			return m_AssetResourceLoader != null;
		}

		public virtual T LoadAsset<T>(string name) where T : UnityEngine.Object {
			var lowerCaseName = name.ToLower ();
			return m_AssetResourceLoader.LoadAsset<T> (lowerCaseName);
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
			while (!Caching.ready)
				yield return null;	
			var www = WWW.LoadFromCacheOrDownload (url, version);
			yield return www;
			if (www.error != null) {
				if (onError != null) {
					onError ("WWW download had an error:" + www.error);
				}
			}
			if (onComplete != null) {
				onComplete (www.assetBundle);
			}
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

		public override bool OnTasked() {
			base.OnTasked ();
			return this.OnResourceLoaded ();
		}

	}
}
