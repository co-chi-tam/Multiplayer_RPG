using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMBaseControllerState : FSMBaseState
	{
		protected CObjectController m_Controller;

		public FSMBaseControllerState(IContext context) : base (context)
		{
			m_Controller = context as CObjectController;
		}

		public override void StartState()
		{
			base.StartState ();
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