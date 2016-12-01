using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMCharacterMoveState : FSMBaseControllerState
	{

		public FSMCharacterMoveState(IContext context) : base (context)
		{
			
		}

		public override void StartState()
		{
			base.StartState ();
			if (m_Controller.GetUnderControl ()) {
				m_Controller.SetAnimation (CEnum.EAnimation.Move);
			}
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
			m_Controller.UpdateMoveInput ();
			m_Controller.MoveToTarget (m_Controller.GetMovePosition (), dt);
		}

		public override void ExitState()
		{
			base.ExitState ();
		}
	}
}