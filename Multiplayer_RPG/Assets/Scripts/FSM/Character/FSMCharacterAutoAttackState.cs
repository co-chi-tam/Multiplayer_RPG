using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMCharacterAutoAttackState : FSMBaseControllerState
	{
		public FSMCharacterAutoAttackState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			if (m_Controller.GetUnderControl ()) {
				m_Controller.InteractAnObject ();
			}
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
			var target = m_Controller.GetTargetInteract ();
			if (target != null) { 
				m_Controller.LookAtTarget (target.GetPosition ());
			}
		}

		public override void ExitState()
		{
			base.ExitState ();
			m_Controller.SetDidAttack (false);
		}
	}
}