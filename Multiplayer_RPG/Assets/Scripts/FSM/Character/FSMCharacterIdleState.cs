using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMCharacterIdleState : FSMBaseControllerState
	{
		public FSMCharacterIdleState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			m_Controller.SetAnimation (CEnum.EAnimation.Idle);
			m_Controller.ResetPerAction ();
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
			m_Controller.UpdateMoveInput ();
		}

		public override void ExitState()
		{
			base.ExitState ();
		}
	}
}