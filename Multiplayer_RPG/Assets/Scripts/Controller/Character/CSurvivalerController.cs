using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	public class CSurvivalerController : CCharacterController {

		private bool m_TouchedUI;
		private CSkillTreeComponent m_SkillTree;

		protected override void Init ()
		{
			base.Init ();
		}

		protected override void Awake ()
		{
			base.Awake ();
			var skillStree = Resources.Load<TextAsset> ("Data/SkillTree/SkillTreeData");
			var skillData = TinyJSON.JSON.Load (skillStree.text).Make<CSkillNodeData> ();
			this.m_SkillTree = new CSkillTreeComponent (skillData);
			this.m_SkillTree.GenerateMapSkillTree (null);
		}

		protected override void Start ()
		{
			base.Start ();
			if (this.GetDataUpdate()) {
				m_Data = TinyJSON.JSON.Load (m_DataText.text).Make<CCharacterData> ();
			} 
			var fsmJson = Resources.Load <TextAsset> (m_Data.fsmPath);
			m_FSMManager.LoadFSM (fsmJson.text);

			this.m_UIManager.RegisterUIInfo (this, true, true);
			#if UNITY_ANDROID
			Input.simulateMouseWithTouches = true;
			Application.targetFrameRate = 30;
			#endif
			// TEST
			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "MainScene") {
				this.m_UIManager.RegisterUIControl (true, UpdateSkillInput, Talk);
			}
		}

		public override void FixedUpdateBaseTime (float dt)
		{
			base.FixedUpdateBaseTime (dt);
			if (this.GetActive()) {
				UpdateFSM (dt);
			}
		}

		public override void UpdateFSM(float dt) {
			base.UpdateFSM (dt);
			m_FSMManager.UpdateState (dt);
		}

		public override void UpdateMoveInput(float dt) {
			if (this.GetUnderControl() == false)
				return;
			base.UpdateMoveInput (dt);
#if UNITY_EDITOR || UNITY_STANDALONE
			if (Input.GetMouseButton (0)) {
				if (EventSystem.current.IsPointerOverGameObject()) 
					return;
				SelectionObject();
			}
#elif UNITY_ANDROID
			if (Input.touchCount == 1) {
				var touchPoint = Input.GetTouch (0);
				m_TouchedUI = CUtil.IsPointerOverUIObject (touchPoint.position);
				if (m_TouchedUI == false) {	
					SelectionObject();
				}
			}
#endif
		}

		private void SelectionObject() {
			RaycastHit hitInfo;
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hitInfo, 100f, m_ObjPlayerMask)) { // Object layermask
				var objCtrl = hitInfo.collider.GetComponent<CObjectController> ();
				if (objCtrl != null && objCtrl != this && objCtrl.GetObjectType () != this.GetObjectType ()) {
					this.SetAnimation (CEnum.EAnimation.Attack_1);
					this.SetCurrentSkill (CEnum.EAnimation.Attack_1);
					this.SetTargetInteract (objCtrl);
					this.SetMovePosition (objCtrl.GetPosition());
					this.SetDidAttack(false);
				} else {
					this.SetCurrentSkill (CEnum.EAnimation.Idle);	
					this.SetTargetInteract (null);
					this.SetMovePosition (hitInfo.point);
					this.SetDidAttack(false);
				}
			}
		}

		public override void UpdateSkillInput(CEnum.EAnimation skill) {
			base.UpdateSkillInput (skill);
			this.SetAnimation (skill);
			this.SetCurrentSkill (skill);
			if (this.GetOtherInteractive() == false || 
				(this.GetTargetInteract() != null && this.GetTargetInteract().GetActive() == true))
				return;
			var colliders = Physics.OverlapSphere (this.GetPosition (), this.GetSeekRadius (), m_ObjPlayerMask);
			if (colliders.Length > 0) {
				for (int i = colliders.Length - 1; i >= 0; i--) {
					var objCtrl = colliders [i].GetComponent<CObjectController> ();
					if (objCtrl != null && objCtrl != this) {
						if (objCtrl.GetObjectType () != this.GetObjectType ()) {
							this.SetTargetInteract (objCtrl);
							this.SetMovePosition (this.GetPosition());
							this.SetCurrentSkill (skill);
							break;
						}
					}
				}
			}
		}

		public override void SetUnderControl (bool value)
		{
			base.SetUnderControl (value);
			if (value && this.GetLocalUpdate()) {
				CameraController.Instance.target = this.transform;
				this.m_UIManager.RegisterUIControl (this, UpdateSkillInput, Talk);
			}
		}

		public override void SetActive (bool value)
		{
//			base.SetActive (value);
			m_Active = value;
		}
	}
}
