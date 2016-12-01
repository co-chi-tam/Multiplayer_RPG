using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	public class CSurvivalerController : CCharacterController {

		private CUIManager m_UIManager;
		private bool m_TouchedUI;

		protected override void Init ()
		{
			base.Init ();
		}

		protected override void Awake ()
		{
			base.Awake ();
		}

		protected override void Start ()
		{
			base.Start ();
			m_FSMManager.LoadFSM (m_FSMText.text);
			if (this.GetDataUpdate()) {
				m_Data = TinyJSON.JSON.Load (m_DataText.text).Make<CCharacterData> ();
			}
			m_UIManager = CUIManager.GetInstance ();
			m_UIManager.OnEventInputSkill += UpdateBattleInput;
		}

		public override void FixedUpdateBaseTime (float dt)
		{
			base.FixedUpdateBaseTime (dt);
			if (this.GetActive() && this.GetLocalUpdate()) {
				UpdateFSM (dt);
			}
		}

		public override void UpdateFSM(float dt) {
			base.UpdateFSM (dt);
			m_FSMManager.UpdateState (dt);
			m_StateName = m_FSMManager.currentStateName;
		}

		public override void UpdateMoveInput() {
			if (this.GetUnderControl() == false)
				return;
			base.UpdateMoveInput ();
#if UNITY_EDITOR || UNITY_STANDALONE
			if (Input.GetMouseButton (0)) {
				if (EventSystem.current.IsPointerOverGameObject()) 
					return;
				RaycastHit hitInfo;
				var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hitInfo, 100f, 1 << 31)) { // PLANE
					this.SetMovePosition (hitInfo.point);
				}
			}
#elif UNITY_ANDROID
			if (Input.touchCount == 1) {
				var touchPhase = Input.GetTouch (0).phase;
				switch (touchPhase) {
				case TouchPhase.Began:
				m_TouchedUI = !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hitInfo;
					if (Physics.Raycast(ray, out hitInfo, 100f, 1 << 31)) {
					if (m_TouchedUI)
						{
							this.SetMovePosition (hitInfo.point);
						}
					}
				break;
				case TouchPhase.Moved:
					// TODO
				break;
				case TouchPhase.Ended:
					// TODO
				break;
				}
			}
#endif
		}

		public override void UpdateBattleInput(string skillName) {
			if (this.GetUnderControl() == false)
				return;
			base.UpdateBattleInput (skillName);
			var colliders = Physics.OverlapSphere (this.GetPosition (), this.GetSeekRadius (), m_ObjPlayerMask);
			if (colliders.Length > 0 && this.GetTargetAttack() == null) {
				for (int i = colliders.Length - 1; i >= 0; i--) {
					var objCtrl = colliders [i].GetComponent<CObjectController> ();
					if (objCtrl != null && objCtrl != this) {
						if (objCtrl.GetObjectType () != this.GetObjectType ()) {
							this.SetTargetAttack (objCtrl);
							this.SetMovePosition (this.GetPosition());
							break;
						}
					}
				}
			}
		}

		public override void SetUnderControl (bool value)
		{
			base.SetUnderControl (value);
			if (value) {
				CameraController.Instance.target = this.transform;
			}
		}

		public override string GetFSMStateName ()
		{
			base.GetFSMStateName ();
			return m_StateName;
		}

	}
}
