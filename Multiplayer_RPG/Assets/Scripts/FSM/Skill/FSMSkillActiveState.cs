using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMSkillActiveState : FSMBaseControllerState
	{
		public FSMSkillActiveState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			if (m_Controller.OnStartAction != null) {
				m_Controller.OnStartAction ();
			}
			m_Controller.OnStartAction = null;
			m_Controller.ResetPerAction ();
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