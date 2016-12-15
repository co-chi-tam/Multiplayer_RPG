using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CGameManager : CMonoSingleton<CGameManager> {

		public CEnum.EGameMode GameMode = CEnum.EGameMode.Survial;

//		private CObjectManager m_ObjectManager;

		protected override void Awake ()
		{
			base.Awake ();
		}

		protected override void Start ()
		{
			base.Start ();
//			m_ObjectManager = CObjectManager.GetInstance ();
		}
	
	}
}
