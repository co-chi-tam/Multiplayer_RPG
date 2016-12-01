using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	public class CUIManager : CMonoSingleton<CUIManager> {

		public Action<string> OnEventInputSkill;

		protected override void Awake ()
		{
			base.Awake ();
		}

		public override void FixedUpdateBaseTime (float dt)
		{
			base.FixedUpdateBaseTime (dt);
			if (Input.GetKeyDown (KeyCode.A)) {
				PressedBasicSkill ();
			}
		}

		public void PressedBasicSkill() {
			if (OnEventInputSkill != null) {
				OnEventInputSkill ("BASIC");
			}
		}

	}
}
