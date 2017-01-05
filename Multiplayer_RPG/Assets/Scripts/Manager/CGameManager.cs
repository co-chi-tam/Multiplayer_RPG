using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CGameManager : CMonoSingleton<CGameManager> {

		public CEnum.EGameMode GameMode = CEnum.EGameMode.Survial;

		protected CMapManager m_MapManager;

		protected override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (this.gameObject);
		}

		protected override void Start ()
		{
			base.Start ();
			CSceneManager.Instance.OnRegisterTask (this);
			CSceneManager.Instance.OnSceneLoaded.AddListener (() => {
				var sceneName = CSceneManager.Instance.GetSceneName();
				if (sceneName == "MainScene") {
					LoadMap();
				}
			});
		}

		public void ChangeModeGame(int value) {
			this.GameMode = (CEnum.EGameMode)value;
		}

		public virtual void LoadMap() {
			m_MapManager = new CMapManager ();
			m_MapManager.LoadMap ("WorldMap0001", (mapData) => {
				var mapObjects = mapData.mapObjects;
				for (int i = 0; i < mapObjects.Length; i++) {
					var objDataText = CResourceManager.Instance.LoadResourceOrAsset<TextAsset> (mapObjects[i].dataPath);
					if (objDataText != null) {
						var objData = TinyJSON.JSON.Load (objDataText.text).Make<CCharacterData> ();
						var objModel = CResourceManager.Instance.LoadResourceOrAsset<CObjectController>(objData.modelPath);
						var objController = GameObject.Instantiate(objModel);
						objController.SetPosition (mapObjects[i].position.ToV3());
					}
				}
			});
		}
	
	}
}
