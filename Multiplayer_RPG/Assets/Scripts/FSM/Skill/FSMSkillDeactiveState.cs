﻿using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMSkillDeactiveState : FSMBaseControllerState
	{
		public FSMSkillDeactiveState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			if (m_Controller.OnEndAction != null) {
				m_Controller.OnEndAction ();
			}
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
		}

		public override void ExitState()
		{
			base.ExitState ();
		}
	}
}