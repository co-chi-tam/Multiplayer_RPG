using UnityEngine;
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
			m_Controller.DisableObject ("Inactive");
			m_Controller.SetIsObstacle (false);
			m_Controller.OnEndAction.Invoke (null);
			m_Controller.OnReturnObjectManager ();
			m_Controller.SpawnResourceMaterials ();
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