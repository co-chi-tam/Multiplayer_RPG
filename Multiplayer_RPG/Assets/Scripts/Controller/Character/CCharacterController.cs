using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {

	[RequireComponent(typeof(NavMeshAgent))]
	public partial class CCharacterController : CObjectController {

		#region Properties

		[SerializeField]	protected AnimatorCustom m_AnimatorController;
		[SerializeField]	protected CObjectController m_TargetInteract;
		[SerializeField]	protected TextAsset m_DataText;
		[SerializeField]	protected LayerMask m_ObjPlayerMask;

		protected NavMeshAgent m_NavMeshAgent;
		protected CEnum.EAnimation m_CurrentAnimation = CEnum.EAnimation.Idle;
		protected CEnum.EAnimation m_CurrentSkill = CEnum.EAnimation.Attack_1;
		protected CCharacterData m_Data;

		protected CUIManager m_UIManager;
		protected bool m_DidAttack = false;

		#endregion

		#region Implementation Monobehaviour

		protected override void Init ()
		{
			base.Init ();
		}

		protected override void Awake ()
		{
			base.Awake ();
			this.m_NavMeshAgent = this.GetComponent<NavMeshAgent> ();
			this.m_Data = new CCharacterData ();
		}

		protected override void Start ()
		{
			base.Start ();
			this.SetActive (true);
			this.SetMovePosition (this.GetPosition());
			this.SetStartPosition (this.GetPosition ());
			this.m_UIManager = CUIManager.GetInstance ();
		}

		protected override void Update ()
		{
			base.Update ();
			var health = 0;
			if (m_BattleComponent.CalculateHealth (this.GetCurrentHealth (), out health)) {
				this.SetCurrentHealth (health);
			}
		}

		protected override void OnRegisterComponent() {
			base.OnRegisterComponent ();
			this.m_MovableComponent = new CMovableComponent (this, m_NavMeshAgent);
			this.m_MovableComponent.currentTransform = m_Transform;
			this.m_MovableComponent.targetPosition = m_MovePosition;
			this.m_BattleComponent = new CBattlableComponent (this);
		}

		#endregion

		#region Main methods

		public override void MoveToTarget(Vector3 target, float dt) {
			base.MoveToTarget (target, dt);
			m_MovableComponent.targetPosition = target;
			m_MovableComponent.MoveForwardToTarget (dt);
		}

		public override void LookAtTarget(Vector3 target) {
			base.LookAtTarget (target);
			m_MovableComponent.LookForwardToTarget (target);
		} 

		public override void InactiveObject(string animationName) {
			base.InactiveObject (animationName);
			var childCount = m_Transform.childCount;
			for (int i = 0; i < childCount; i++) {
				var child = m_Transform.GetChild (i);
				child.gameObject.SetActive (false);
			}
		}

		public override void ActiveObject() {
			base.ActiveObject ();
			var childCount = m_Transform.childCount;
			for (int i = 0; i < childCount; i++) {
				var child = m_Transform.GetChild (i);
				child.gameObject.SetActive (true);
			}
		}

		#endregion

		#region Getter && Setter

		public override string GetID ()
		{
			base.GetID ();
			if (m_Data == null)
				return base.GetID ();
			return m_Data.id;
		}

		public override void SetID (string value)
		{
			base.SetID (value);
			m_Data.id = value;
		}

		public override void SetData (CObjectData value)
		{
			base.SetData (value);
			m_Data = value as CCharacterData;
		}

		public override CObjectData GetData ()
		{
			base.GetData ();
			return m_Data;
		}

		public override void SetName (string value)
		{
			base.SetName (value);
			m_Data.name = value;
		}

		public override string GetName ()
		{
			base.GetName ();
			return m_Data.name;
		}

		public override void SetCurrentHealth (int value)
		{
			base.SetCurrentHealth (value);
			m_Data.currentHealth = value;
		}

		public override int GetCurrentHealth ()
		{
			base.GetCurrentHealth ();
			return m_Data.currentHealth;
		}

		public override int GetMaxHealth ()
		{
			base.GetMaxHealth ();
			return m_Data.maxHealth;
		}

		public override int GetPhysicDefend() {
			base.GetPhysicDefend ();
			return m_Data.physicDefend;
		}

		public override int GetAttackDamage() {
			base.GetAttackDamage ();
			return m_Data.attackDamage;
		}

		public override void SetAnimation(CEnum.EAnimation anim) {
			base.SetAnimation (anim);
			m_CurrentAnimation = anim;
			m_AnimatorController.SetInteger ("AnimParam", (int)anim);
		}

		public override CEnum.EAnimation GetAnimation() {
			return m_CurrentAnimation;
		}

		public override void SetAnimationTime (float value)
		{
			base.SetAnimationTime (value);
			m_AnimatorController.SetTime (value);
		}

		public override float GetAnimationTime ()
		{
			base.GetAnimationTime ();
			return m_AnimatorController.GetTime ();
		}

		public override float GetMoveSpeed ()
		{
			base.GetMoveSpeed ();
			return m_Data.moveSpeed;
		}

		public override float GetSeekRadius ()
		{
			base.GetSeekRadius ();
			return m_Data.seekRadius + this.GetSize();
		}

		public override float GetAttackRange ()
		{
			base.GetAttackRange ();
			if (m_Data == null)
				return base.GetAttackRange ();		
			return this.GetSize() + m_Data.attackRange;
		}

		public override float GetAttackSpeed ()
		{
			base.GetAttackSpeed ();
			if (m_Data == null)
				return base.GetAttackSpeed ();		
			return m_Data.attackSpeed;
		}

		public override float GetDistanceToTarget ()
		{
			base.GetDistanceToTarget ();
			if (m_TargetInteract == null)
				return 0.1f;
			return this.GetAttackRange () + m_TargetInteract.GetSize();
		}

		public override void SetTargetInteract(CObjectController value) {
			base.SetTargetInteract (value);
			m_TargetInteract = value;
		}

		public override CObjectController GetTargetInteract() {
			base.GetTargetInteract ();
			if (m_TargetInteract == null || m_TargetInteract.GetActive () == false)
				return null;
			return m_TargetInteract;
		}

		public override void SetDidAttack(bool value) {
			base.SetDidAttack (value);
			m_DidAttack = value;
		}

		public override bool GetDidAttack() {
			base.GetDidAttack ();
			return m_DidAttack;
		}

		public override CEnum.EObjectType GetObjectType ()
		{
			return m_Data.objectType;
		}

		public override void SetObjectType (CEnum.EObjectType objectType)
		{
			base.SetObjectType (objectType);
			m_Data.objectType = objectType;
		}

		public override Vector3 GetMovePosition() {
			if (m_MovableComponent == null)
				return base.GetMovePosition();
			return m_MovableComponent.targetPosition;
		}

		public override void SetMovePosition(Vector3 value) {
			base.SetMovePosition (value);
			if (m_MovableComponent == null)
				return;
			m_MovableComponent.targetPosition = value;
		}

		public override void SetCurrentSkill (CEnum.EAnimation value)
		{
			base.SetCurrentSkill (value);
			switch (value) {
			case CEnum.EAnimation.Idle:
			case CEnum.EAnimation.Attack_1:
			case CEnum.EAnimation.Attack_2:
			case CEnum.EAnimation.Attack_3:
			case CEnum.EAnimation.Attack_4:
			case CEnum.EAnimation.Attack_5:
			case CEnum.EAnimation.Attack_6:
			case CEnum.EAnimation.Attack_7:
			case CEnum.EAnimation.Attack_8:
			case CEnum.EAnimation.Attack_9:
			case CEnum.EAnimation.Attack_10:
				m_CurrentSkill = value;
				break;
			default:
				m_CurrentSkill = CEnum.EAnimation.Idle;
				break;
			}
		}

		public override CEnum.EAnimation GetCurrentSkill ()
		{
			base.GetCurrentSkill ();
			return m_CurrentSkill;
		}

		public override string GetToken() {
			base.GetToken ();
			return m_Data.token;
		}

		public override void SetToken(string value) {
			base.SetToken (value);
			m_Data.token = value;
		}

		#endregion

	}
}
