using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMCharacterAttackState : FSMBaseControllerState
	{

		public FSMCharacterAttackState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			if (m_Controller.GetUnderControl ()) {
				m_Controller.SetAnimation (CEnum.EAnimation.Attack_2);
			}
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
			m_Controller.UpdateMoveInput ();
			var target = m_Controller.GetTargetAttack ();
			if (target != null) { 
				m_Controller.LookAtTarget (target.GetPosition ());
			}
		}

		public override void ExitState()
		{
			base.ExitState ();
			m_Controller.SetTargetAttack (null);
			m_Controller.SetDidAttack (false);
		}
	}
}