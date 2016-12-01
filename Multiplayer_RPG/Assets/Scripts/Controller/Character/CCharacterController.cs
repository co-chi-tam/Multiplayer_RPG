using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {

	[RequireComponent(typeof(NavMeshAgent))]
	public partial class CCharacterController : CObjectController {

		#region Properties

		[SerializeField]	protected string m_StateName;
		[SerializeField]	protected AnimatorCustom m_AnimatorController;
		[SerializeField]	protected CObjectController m_TargetAttack;
		[SerializeField]	protected TextAsset m_FSMText;
		[SerializeField]	protected TextAsset m_DataText;
		[SerializeField]	protected LayerMask m_ObjPlayerMask;

		protected NavMeshAgent m_NavMeshAgent;
		protected CEnum.EAnimation m_CurrentAnimation = CEnum.EAnimation.Idle;
		protected CCharacterData m_Data;

		protected bool m_DidAttack = false;

		#endregion

		#region Implementation Monobehaviour

		protected override void Awake ()
		{
			base.Awake ();
			m_NavMeshAgent = this.GetComponent<NavMeshAgent> ();
			m_Data = new CCharacterData ();
		}

		protected override void Start ()
		{
			base.Start ();
			this.SetActive (true);
			this.SetMovePosition (this.GetPosition());
			this.SetStartPosition (this.GetPosition ());
		}

		protected override void OnRegisterComponent() {
			base.OnRegisterComponent ();
			m_MovableComponent = new CMovableComponent (this, m_NavMeshAgent);
			m_MovableComponent.currentTransform = m_Transform;
			m_BattleComponent = new CBattlableComponent (this);
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

		public virtual void DeactiveObject(string animationName) {
			this.gameObject.SetActive (false);
		}

		#endregion

		#region Getter && Setter

		public override string GetID ()
		{
			base.GetID ();
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

		public override void SetCurrentMana (int value)
		{
			base.SetCurrentMana (value);
			m_Data.currentMana = value;
		}

		public override int GetCurrentMana ()
		{
			base.GetCurrentMana ();
			return m_Data.currentMana;
		}

		public override int GetMaxMana ()
		{
			base.GetMaxMana ();
			return m_Data.maxMana;
		}

		public override int GetPhysicDefend() {
			base.GetPhysicDefend ();
			return m_Data.physicDefend;
		}

		public override int GetMagicDefend() {
			base.GetMagicDefend ();
			return m_Data.magicDefend;
		}

		public override int GetPureDamage() {
			base.GetPureDamage ();
			return m_Data.pureDamage;
		}

		public override int GetPhysicDamage() {
			base.GetPhysicDamage ();
			return m_Data.physicDamage;
		}

		public override int GetMagicDamage() {
			base.GetMagicDamage ();
			return m_Data.magicDamage;
		}

		public override void SetActiveSkill (int index)
		{
			base.SetActiveSkill (index);
			SetAnimation ((CEnum.EAnimation)index);
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
			return this.GetSize() + m_Data.attackRange;
		}

		public override float GetDistanceToTarget ()
		{
			base.GetDistanceToTarget ();
			if (m_TargetAttack == null)
				return 0.1f;
			return this.GetAttackRange () + m_TargetAttack.GetSize();
		}

		public override void SetTargetAttack(CObjectController value) {
			base.SetTargetAttack (value);
			m_TargetAttack = value;
		}

		public override CObjectController GetTargetAttack() {
			base.GetTargetAttack ();
			if (m_TargetAttack == null || m_TargetAttack.GetActive () == false)
				return null;
			return m_TargetAttack;
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
				return Vector3.zero;
			return m_MovableComponent.targetPosition;
		}

		public override void SetMovePosition(Vector3 value) {
			base.SetMovePosition (value);
			if (m_MovableComponent == null)
				return;
			m_MovableComponent.targetPosition = value;
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
