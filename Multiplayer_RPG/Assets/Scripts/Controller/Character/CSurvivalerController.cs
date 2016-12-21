using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	public class CSurvivalerController : CCharacterController {

		#region Properties

		private bool m_TouchedUI;
		private CSkillTreeComponent m_SkillTree;

		#endregion

		#region MonoBehaviour

		public override void Init ()
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

			var fsmJson = Resources.Load <TextAsset> (m_Data.fsmPath);
			m_FSMManager.LoadFSM (fsmJson.text);

			this.m_UIManager.RegisterUIInfo (this, true, true);
			#if UNITY_ANDROID
			Input.simulateMouseWithTouches = true;
			Application.targetFrameRate = 30;
			#endif
			if (CGameManager.Instance.GameMode == CEnum.EGameMode.Survial) {
				this.m_UIManager.RegisterUIControl (true, this.UpdateSkillInput, this.Chat, this.ShowEmotion);
				this.m_UIManager.RegisterUIStatus (this);
			}
		}

		public override void FixedUpdateBaseTime (float dt)
		{
			base.FixedUpdateBaseTime (dt);
			if (this.GetActive()) {
				UpdateFSM (dt);
			}
		}

		#endregion

		#region Main methods

		public override void UpdateFSM(float dt) {
			base.UpdateFSM (dt);
			m_FSMManager.UpdateState (dt);
		}

		public override void UpdateTouchInput(float dt) {
			base.UpdateTouchInput (dt);
			if (this.GetUnderControl() == false)
				return;
#if UNITY_EDITOR || UNITY_STANDALONE
			if (Input.GetMouseButton (0)) {
				if (EventSystem.current.IsPointerOverGameObject()) { return; }
				var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				UpdateSelectionObject(ray.origin, ray.direction);
			}
#elif UNITY_ANDROID
			if (Input.touchCount == 1) {	
				var touchPoint = Input.GetTouch (0);
				m_TouchedUI = CUtil.IsPointerOverUIObject (touchPoint.position);
				if (m_TouchedUI) { return; }
				var ray = Camera.main.ScreenPointToRay (touchPoint.position);
				UpdateSelectionObject(ray.origin, ray.direction);
			}
#endif
		}

		public override void UpdateSelectionObject(Vector3 originPoint, Vector3 directionPoint) {
			base.UpdateSelectionObject (originPoint, directionPoint);
			if (this.GetOtherInteractive() == false)
				return;
			RaycastHit hitInfo;
			if (Physics.Raycast (originPoint, directionPoint, out hitInfo, 100f, m_ObjPlayerMask)) { // Object layermask
				var objCtrl = hitInfo.collider.GetComponent<CObjectController> ();
				if (objCtrl != null 
					&& objCtrl != this 
					&& Array.IndexOf (m_Data.attackableObjectTypes, (int)objCtrl.GetObjectType ()) != -1) {
					this.SetMovePosition (objCtrl.GetPosition ());
					this.SetTargetInteract (objCtrl);
					this.SetCurrentSkill (CEnum.EAnimation.Attack_1);
				} else {	
					this.SetTargetInteract (null);
					this.SetCurrentSkill (CEnum.EAnimation.Idle);
					this.SetMovePosition (hitInfo.point);
				}
			}
		}

		public override void UpdateSkillInput(CEnum.EAnimation skill) {
			this.SetCurrentSkill (skill);
			if (this.GetOtherInteractive() == false || 
				(this.GetTargetInteract() != null && this.GetTargetInteract().GetActive() == true))
				return;
			base.UpdateSkillInput (skill);
			RaycastHit hitInfo;
			if (Physics.Raycast (this.GetPosition (), m_Transform.forward, out hitInfo, this.GetSeekRadius (), m_ObjPlayerMask)) {
				var objCtrl = hitInfo.collider.GetComponent<CObjectController> ();
				if (objCtrl != null 
					&& objCtrl != this 
					&& Array.IndexOf (m_Data.attackableObjectTypes, (int)objCtrl.GetObjectType ()) != -1) {
					var direction = objCtrl.GetPosition () - this.GetPosition ();
					var frontPosition = objCtrl.GetPosition() - direction.normalized * (objCtrl.GetSize () + this.GetAttackRange () - this.GetSize());
					this.SetMovePosition (frontPosition);
					this.SetTargetInteract (objCtrl);
					this.SetCurrentSkill (skill);
				}
			}
		}

		#endregion

		#region Gett && Setter

		public override void SetUnderControl (bool value)
		{
			base.SetUnderControl (value);
			if (value && this.GetLocalUpdate()) {
				CameraController.Instance.target = this.transform;
				this.m_UIManager.RegisterUIControl (this, this.UpdateSkillInput, this.Chat, this.ShowEmotion);
			}
		}

		public override void SetActive (bool value)
		{
//			base.SetActive (value);
			m_Active = value;
		}

		#endregion

	}
}
