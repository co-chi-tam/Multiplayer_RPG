using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMSkillMoveState : FSMBaseControllerState
	{

		public FSMSkillMoveState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
			m_Controller.MoveToTarget (m_Controller.GetMovePosition (), dt);
		}

		public override void ExitState()
		{
			base.ExitState ();
		}
	}
}