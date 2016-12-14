﻿using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMCharacterInactiveState : FSMBaseControllerState
	{
		public FSMCharacterInactiveState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			m_Controller.SetAnimation (CEnum.EAnimation.Death);
			m_Controller.SetActive (false);
			m_Controller.SetIsObstacle (false);
			m_Controller.OnReturnObjectManager ();
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
		}

		public override void ExitState()
		{
			base.ExitState ();
			m_Controller.EnableObject ();
			m_Controller.ResetAll ();
		}
	}
}