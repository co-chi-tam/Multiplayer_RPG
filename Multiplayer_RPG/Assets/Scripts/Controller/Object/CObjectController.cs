using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	[RequireComponent(typeof(CapsuleCollider))]
	public class CObjectController : CBaseController, IContext, IMovable, IBattlable, IStatus {

		#region Properties

		public Action OnStartAction;
		public Action OnEndAction;

		[SerializeField]	protected CapsuleCollider m_CapsuleCollider;

		protected FSMManager m_FSMManager;

		protected Vector3 m_StartPosition;
		protected Vector3 m_MovePosition;
		protected string m_TalkString;

		// Smoothy
		protected float m_WaitingPerAction = 0f;

		// Controller
		protected bool m_UnderControl = true;
		protected bool m_LocalUpdate = true;
		protected bool m_DataUpdate = true;
		protected bool m_OtherInteractive = true;

		#endregion

		#region Implementation Monobehaviour

		protected override void Init ()
		{
			base.Init ();
		}

		protected override void Awake ()
		{
			base.Awake ();
			m_FSMManager 		= new FSMManager ();
		}

		protected override void Start ()
		{
			base.Start ();
			OnRegisterAnimation ();
			OnRegisterFSM ();
			OnRegisterComponent ();
		}

		protected override void OnDrawGizmos ()
		{
			base.OnDrawGizmos ();
			if (Application.isPlaying == false)
				return;
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere (this.GetPosition(), this.GetSize());
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere (this.GetPosition(), this.GetAttackRange());
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere (this.GetPosition(), this.GetSeekRadius());
		}

		#endregion

		#region Main methods

		public virtual void UpdateFSM(float dt) {

		}

		protected virtual void OnRegisterComponent() {

		}

		protected virtual void OnRegisterFSM() {
			var waitingState 	= new FSMWaitingState (this);

			m_FSMManager.RegisterState ("WaitingState", 		waitingState);

			m_FSMManager.RegisterCondition ("IsActive",			this.GetActive);
			m_FSMManager.RegisterCondition ("DidEndWaiting", 	DidEndWaiting);
		}

		protected virtual void OnRegisterAnimation() {

		}

		public virtual void ResetBaseGame() {

		}

		public virtual void InteractAnObject() {
			
		}

		public virtual void ApplyDamage(IBattlable attacker, int damage, CEnum.EElementType damageType) {

		}

		public virtual void ApplyBuff(IBattlable buffer, int buff, CEnum.EStatusType statusType) {

		}

		public virtual void FindTargetInteract() { 
			
		}

		public virtual void FindMovePosition() { 

		}

		public virtual void MoveToTarget(Vector3 target, float dt) {
			
		}

		public virtual void LookAtTarget(Vector3 target) {

		}

		public virtual void UpdateMoveInput(float dt) {
		
		}

		public virtual void UpdateSkillInput(CEnum.EAnimation skill) {
			
		}

		public virtual void ResetPerAction() {
			m_WaitingPerAction = this.GetAttackSpeed();
			this.SetCurrentSkill (CEnum.EAnimation.Idle);
		}

		public virtual void DestroySelf() {
			DestroyImmediate (this.gameObject);
		}

		public virtual void Talk(string value) {
			
		}

		public virtual void InactiveObject(string animationName) {
		
		}

		public virtual void ActiveObject() {
		
		}

		#endregion

		#region FSM

		internal virtual bool IsDeath() {
			return false;
		}

		internal virtual bool DidEndWaiting() {
			m_WaitingPerAction -= Time.deltaTime;
			return m_WaitingPerAction <= 0f;
		}

		#endregion

		#region Getter && Setter

		public virtual void SetData(CObjectData value) {
			
		}

		public virtual CObjectData GetData() {
			return null;
		}

		public virtual string GetFSMStateName() {
			return string.Empty;
		}

		public virtual void SetFSMStateName(string value) {

		}

		public virtual string GetFSMName() {
			return string.Empty;
		}

		public virtual void SetName(string value) {
			gameObject.name = value;
		}

		public virtual string GetName() {
			return gameObject.name;
		}

		public override void SetActive (bool value)
		{
			base.SetActive (value);
		}

		public override bool GetActive ()
		{
			return base.GetActive ();
		}

		public virtual void SetUnderControl (bool value)
		{
			m_UnderControl = value;
		}

		public virtual bool GetUnderControl ()
		{
			return m_UnderControl;
		}

		public virtual void SetLocalUpdate (bool value)
		{
			m_LocalUpdate = value;
		}

		public virtual bool GetLocalUpdate ()
		{
			return m_LocalUpdate;
		}

		public virtual void SetDataUpdate (bool value)
		{
			m_DataUpdate = value;
		}

		public virtual bool GetDataUpdate ()
		{
			return m_DataUpdate;
		}

		public virtual void SetOtherInteractive (bool value)
		{
			m_OtherInteractive = value;
		}

		public virtual bool GetOtherInteractive ()
		{
			return m_OtherInteractive;
		}

		public virtual void SetAnimation(CEnum.EAnimation value) {
			
		}

		public virtual CEnum.EAnimation GetAnimation() {
			return CEnum.EAnimation.Idle;
		}

		public virtual void SetAnimationTime(float value) {

		}

		public virtual float GetAnimationTime() {
			return 0f;
		}

		public virtual void SetTargetInteract(CObjectController value) {
			
		}

		public virtual CObjectController GetTargetInteract() {
			return null;
		}

		public virtual CEnum.EObjectType GetObjectType() {
			return CEnum.EObjectType.None;
		}

		public virtual void SetObjectType(CEnum.EObjectType objectType) {
			
		}

		public virtual CObjectController GetOwner() {
			return null;
		}

		public virtual void SetOwner(CObjectController value) {
			
		}

		public virtual void SetDidAttack(bool value) {
			
		}

		public virtual bool GetDidAttack() {
			return false;
		}

		public override string GetID() {
			base.GetID ();
			return this.gameObject.GetInstanceID () + "";
		}

		public override void SetID (string value)
		{
			base.SetID (value);
		}

		public virtual float GetMoveSpeed() {
			return 0f;
		}

		public virtual float GetSeekRadius() {
			return 0f;
		}

		public virtual float GetDistanceToTarget() {
			return 0f;
		}

		public virtual void SetIsObstacle(bool value) {
			
		}

		public virtual bool GetIsObstacle() {
			return false;
		}

		public virtual int GetPhysicDefend() {
			return 0;
		}

		public virtual int GetCurrentHealth() {
			return 0;
		}

		public virtual int GetMaxHealth() {
			return 0;
		}

		public virtual void SetCurrentHealth(int value) {

		}

		public virtual int GetAttackDamage() {
			return 0;
		}

		public virtual float GetAttackRange() {
			return 0f;
		}

		public virtual float GetAttackSpeed() {
			return 0f;
		}

		public virtual float GetSize() {
			return m_CapsuleCollider.radius;
		}

		public virtual float GetHeight() {
			return m_CapsuleCollider.height / 2f;
		}

		public virtual Vector3 GetMovePosition() {
			return m_MovePosition;
		}

		public virtual void SetMovePosition(Vector3 value) {
			m_MovePosition = value;
		}

		public virtual Vector3 GetStartPosition() {
			return m_StartPosition;
		}

		public virtual void SetStartPosition(Vector3 position) {
			m_StartPosition = position;
		}

		public virtual CEnum.EAnimation GetCurrentSkill() {
			return CEnum.EAnimation.Attack_1;
		}

		public virtual void SetCurrentSkill(CEnum.EAnimation value) {
		
		}

		public virtual string GetToken() {
			return string.Empty;
		}

		public virtual void SetToken(string value) {
			
		}

		public virtual object GetController() {
			return this;
		}

		public virtual void SetTalk(string value) {
			m_TalkString = value;
		}

		public virtual string GetTalk() {
			return m_TalkString;
		}

		#endregion

	}
}
