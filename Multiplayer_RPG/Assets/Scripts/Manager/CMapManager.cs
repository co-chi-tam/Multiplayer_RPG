using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CMapManager {

//		private List<GameObject> m_Objects;

		public CMapManager ()
		{
//			this.m_Objects = new List<GameObject> ();
		}

		public void LoadMap(string dataPath, Action<CMapData> complete) {
			var mapJsonText = CResourceManager.Instance.LoadResourceOrAsset<TextAsset> (dataPath);
			if (mapJsonText != null) {
				var mapData = TinyJSON.JSON.Load (mapJsonText.text).Make<CMapData> ();
				if (complete != null) {
					complete (mapData);
				} 
			}
		} 

	}
}
