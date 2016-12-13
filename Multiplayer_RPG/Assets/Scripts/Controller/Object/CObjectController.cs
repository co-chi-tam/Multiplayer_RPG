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
		[SerializeField]	protected TextAsset m_DataText;
		[SerializeField]	protected AnimatorCustom m_AnimatorController;

		// Manager
		protected FSMManager m_FSMManager;
		protected CObjectManager m_ObjectManager;

		protected Vector3 m_StartPosition;
		protected Vector3 m_MovePosition;
		protected Vector2 m_TouchPosition;
		protected string m_ChatString;
		protected string m_EmotionName;

		// Smoothy
		protected float m_WaitingPerAction = 0f;

		// Controller
		protected bool m_UnderControl = true;
		protected bool m_LocalUpdate = true;
		protected bool m_DataUpdate = true;
		protected bool m_OtherInteractive = true;

		protected CBattlableComponent m_BattleComponent;

		#endregion

		#region Implementation Monobehaviour

		public override void Init ()
		{
			base.Init ();
		}

		protected override void Awake ()
		{
			base.Awake ();
			m_FSMManager 		= new FSMManager ();
			this.OnLoadData ();
		}

		protected override void Start ()
		{
			base.Start ();

			this.OnRegisterAnimation ();
			this.OnRegisterFSM ();
			this.OnRegisterComponent ();

			this.m_ObjectManager = CObjectManager.GetInstance ();
		}

		protected override void Update ()
		{
			base.Update ();
			var health = 0;
			if (m_BattleComponent.CalculateHealth (this.GetCurrentHealth (), out health)) {
				this.SetCurrentHealth (health);
			}
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
			this.m_BattleComponent = new CBattlableComponent (this);
		}

		protected virtual void OnRegisterFSM() {
			var waitingState 	= new FSMWaitingState (this);

			m_FSMManager.RegisterState ("WaitingState", 		waitingState);

			m_FSMManager.RegisterCondition ("IsDeath",			IsDeath);
			m_FSMManager.RegisterCondition ("IsActive",			this.GetActive);
			m_FSMManager.RegisterCondition ("DidEndWaiting", 	DidEndWaiting);
		}

		protected virtual void OnRegisterAnimation() {
			m_AnimatorController.RegisterAnimation ("Death", InactiveObject);
			m_AnimatorController.RegisterAnimation ("Inactive", InactiveObject);
		}

		protected virtual void OnRegisterInventory() {
		
		}

		protected virtual void OnLoadData() {
			
		}

		public virtual void ResetAll() {
			if (this.GetActive () == false)
				return;
			this.SetMovePosition (this.GetPosition());
			this.SetStartPosition (this.GetPosition ());
			this.SetCurrentHealth (this.GetMaxHealth ());
			this.ActiveObject ();
		}

		public virtual void ResetPerAction() {
			m_WaitingPerAction = this.GetAttackSpeed();
		}

		public virtual void InteractAnObject() {
			
		}

		public virtual void ApplyDamage(IBattlable attacker, int damage, CEnum.EElementType damageType) {
			m_BattleComponent.ApplyDamage (damage, damageType);
		}

		public virtual void ApplyBuff(IBattlable buffer, int buff, CEnum.EStatusType statusType) {
			m_BattleComponent.ApplyBuff (buff, statusType);
		}

		public virtual void FindTargetInteract() { 
			
		}

		public virtual void FindMovePosition() { 

		}

		public virtual void MoveToTarget(Vector3 target, float dt) {
			
		}

		public virtual void LookAtTarget(Vector3 target) {

		}

		public virtual void UpdateTouchInput(float dt) {
		
		}

		public virtual void UpdateSkillInput(CEnum.EAnimation skill) {
			
		}

		public virtual void UpdateSelectionObject(Vector3 originPoint, Vector3 directionPoint) {

		}

		public virtual void DestroySelf() {
			this.ResetAll();
			DestroyImmediate (this.gameObject);
		}

		public virtual void Chat(string value) {
			
		}

		public virtual void ShowEmotion (string value) {
		
		}

		public virtual void InactiveObject(string animationName) {
			var childCount = m_Transform.childCount;
			for (int i = 0; i < childCount; i++) {
				var child = m_Transform.GetChild (i);
				child.gameObject.SetActive (false);
			}
			this.m_CapsuleCollider.enabled = false;
		}

		public virtual void ActiveObject() {
			var childCount = m_Transform.childCount;
			for (int i = 0; i < childCount; i++) {
				var child = m_Transform.GetChild (i);
				child.gameObject.SetActive (true);
			}
			this.m_CapsuleCollider.enabled = true;
		}

		public virtual void OnReturnObjectManager() {
			this.m_ObjectManager.SetObject (this.GetName (), this);
		}

		public virtual void SpawnResources() {

		}

		#endregion

		#region FSM

		internal virtual bool IsDeath() {
			return this.GetCurrentHealth() <= 0;
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
			return m_FSMManager.currentStateName;
		}

		public virtual void SetFSMStateName(string value) {
			m_FSMManager.SetState (value);
		}

		public virtual string GetFSMName() {
			return string.Empty;
		}

		public virtual void SetAvatar(string value) {
			
		}

		public virtual string GetAvatar() {
			return string.Empty;
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
			m_AnimatorController.SetInteger ("AnimParam", (int)value);
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
			m_CapsuleCollider.enabled = value;
		}

		public virtual bool GetIsObstacle() {
			return m_CapsuleCollider.enabled;
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

		public virtual Vector3 GetOriginTouchPoint() {
			return Vector3.zero;
		}

		public virtual void SetOriginTouchPoint(Vector3 position) {
			
		}

		public virtual Vector3 GetDirectionTouchPoint() {
			return Vector3.zero;
		}

		public virtual void SetDirectionTouchPoint(Vector3 position) {

		}

		public virtual CEnum.EAnimation GetCurrentSkill() {
			return CEnum.EAnimation.Idle;
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

		public virtual void SetChat(string value) {
			m_ChatString = value;
		}

		public virtual string GetChat() {
			return m_ChatString;
		}

		public virtual void SetEmotion(string value) {
			m_EmotionName = value;
		}

		public virtual string GetEmotion() {
			return m_EmotionName;
		}

		#endregion

	}
}
