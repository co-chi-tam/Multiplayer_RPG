using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CGameManager : CMonoSingleton<CGameManager> {

		public CEnum.EGameMode GameMode = CEnum.EGameMode.Survial;

		protected override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (this.gameObject);
			CSceneManager.Instance.OnRegisterTask (this);
		}

		protected override void Start ()
		{
			base.Start ();
		}

		public void ChangeModeGame(int value) {
			this.GameMode = (CEnum.EGameMode)value;
		}
	
	}
}
