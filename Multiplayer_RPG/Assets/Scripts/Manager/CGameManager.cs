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
//			m_MapManager = new CMapManager ();
//			m_MapManager.LoadMap ("WorldMap0001", (mapData) => {
//				var mapObjects = mapData.mapObjects;
//				for (int i = 0; i < mapObjects.Length; i++) {
//					
//				}
//			});
		}

		public void ChangeModeGame(int value) {
			this.GameMode = (CEnum.EGameMode)value;
		}
	
	}
}
