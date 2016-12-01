using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DataReflection;

namespace SurvivalTest {
	[Serializable]
	public class CMapData : CBaseData {

		public string mapName;
		public CMapObject[] mapObjects;

		public CMapData () : base()
		{
			this.id = string.Empty;
			this.mapObjects = null;
		}

		public class CMapObject {

			public string dataPath;
			public string position;

			public CMapObject ()
			{
				this.dataPath = string.Empty;
				this.position = string.Empty;
			}
		}
	}
}
