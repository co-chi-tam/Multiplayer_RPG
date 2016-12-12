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
				this.m_UIManager.RegisterUIControl (true, this.UpdateSkillInput, this.Chat, this.ShowEmotion);
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
				if (objCtrl != null && objCtrl != this && objCtrl.GetObjectType () != this.GetObjectType ()) {
					this.SetTargetInteract (objCtrl);
					this.SetCurrentSkill (CEnum.EAnimation.Attack_1);
					this.SetMovePosition (this.GetPosition ());
					this.SetDidAttack(false);
				} else {	
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
