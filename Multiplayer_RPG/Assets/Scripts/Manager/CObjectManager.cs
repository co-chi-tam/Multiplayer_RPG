using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

namespace SurvivalTest {
	public class CObjectManager : CMonoSingleton<CObjectManager> {

		private Dictionary<string, ObjectPool<CBaseController>> m_ObjectPools;

		protected override void Awake ()
		{
			base.Awake ();
			m_ObjectPools = new Dictionary<string, ObjectPool<CBaseController>> ();
		}

		public CBaseController GetObject(string path) {
			if (m_ObjectPools.ContainsKey (path)) {
				var objGet = m_ObjectPools [path].Get ();
				if (objGet != null) {
					return objGet;
				}
			} else {
				m_ObjectPools [path] = new ObjectPool<CBaseController> ();
			}
			var resourceLoad = Resources.Load<CBaseController> (path);
			var newObj = Instantiate (resourceLoad);
			m_ObjectPools [path].Create (newObj);
			var objAlready = m_ObjectPools [path].Get();
			return objAlready;
		}

		public void SetObject(string path, CBaseController obj) {
			if (m_ObjectPools.ContainsKey (path)) {
				m_ObjectPools [path].Set (obj);
			} else {
				m_ObjectPools [path] = new ObjectPool<CBaseController> ();
				m_ObjectPools [path].Add (obj);
			}
			obj.gameObject.SetActive (false);
		}
	
	}
}

