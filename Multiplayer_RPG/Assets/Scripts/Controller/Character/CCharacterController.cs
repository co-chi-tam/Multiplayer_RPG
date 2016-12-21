using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	[RequireComponent(typeof(NavMeshAgent))]
	public partial class CCharacterController : CObjectController, IInventory {

		#region Properties

		[SerializeField]	protected CObjectController m_TargetInteract;
		[SerializeField]	protected LayerMask m_ObjPlayerMask;

		// Data
		protected CCharacterData m_Data;

		// Manager
		protected CUIManager m_UIManager;

		// Touch
		protected Vector3 m_OriginTouchPoint;
		protected Vector3 m_DirectionTouchPoint;

		// Component
		protected CInventoryComponent m_InventoryComponent;

		// Common
		protected NavMeshAgent m_NavMeshAgent;
		protected CEnum.EAnimation m_CurrentSkill = CEnum.EAnimation.Attack_1;
		protected bool m_DidAttack = false;

		#endregion

		#region Implementation Monobehaviour

		public override void Init ()
		{
			base.Init ();
		}

		protected override void Awake ()
		{
			base.Awake ();
			this.m_NavMeshAgent = this.GetComponent<NavMeshAgent> ();
		}

		protected override void Start ()
		{
			base.Start ();
			this.SetActive (true);
			this.SetEnable (true);
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
			OnUpdateExecuteCommand ();
		}

		protected override void OnRegisterComponent() {
			base.OnRegisterComponent ();
			// Movable
			this.m_MovableComponent = new CMovableComponent (this, m_NavMeshAgent);
			this.m_MovableComponent.currentTransform = m_Transform;
			this.m_MovableComponent.targetPosition = m_MovePosition;
			// Inventory
			m_InventoryComponent = new CInventoryComponent (this, 6);
		}

		protected override void OnLoadData ()
		{
			base.OnLoadData ();
			this.m_Data = new CCharacterData ();
			if (this.GetDataUpdate ()) {
				this.m_Data = TinyJSON.JSON.Load (m_DataText.text).Make<CCharacterData> ();
			}
		}

		public override void OnDestroyObject ()
		{
			// Inventory items
			var inventoryItems = m_InventoryComponent.GetInventoryItems ();
			for (int i = 0; i < inventoryItems.Length; i++) {
				var item = inventoryItems [i];
				if (item != null) {
					this.m_ObjectManager.SetObject (item.GetName (), item.GetController () as CBaseController);
				}
			}
			// Equipment items
			var equipItems = m_InventoryComponent.GetEquipmentItems ();
			for (int i = 0; i < equipItems.Length; i++) {
				var item = equipItems [i];
				if (item != null) {
					this.m_ObjectManager.SetObject (item.GetName (), item.GetController () as CBaseController);
				}
			}
			base.OnDestroyObject ();
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

		public override void ResetAll ()
		{
			if (this.GetActive () == false)
				return;
			base.ResetAll ();
			this.SetAnimation (CEnum.EAnimation.Idle);
			this.SetCurrentSkill (CEnum.EAnimation.Idle);
			this.SetTargetInteract (null);
		} 

		public override void ResetPerAction() {
			base.ResetPerAction ();
			this.SetDidAttack (false);
		}

		public override void UpdateSelectionObject (Vector3 originPoint, Vector3 directionPoint)
		{
			base.UpdateSelectionObject (originPoint, directionPoint);
			m_OriginTouchPoint = originPoint;
			m_DirectionTouchPoint = directionPoint;
		}

		public virtual void AddInventoryItem(IItem value) {
			if (this.GetOtherInteractive () == false)
				return;
			if (this.GetObjectType () != CEnum.EObjectType.Survivaler)
				return;
			if (this.m_InventoryComponent.AddInventoryItem (value, (x) => {
				this.m_EventComponent.InvokeEventListener ("AddInventoryItem", x);
			}, (x) => {
				var itemController = value.GetController() as CObjectController;
				this.m_ObjectManager.SetObject(itemController.GetName(), itemController); 
				this.m_EventComponent.InvokeEventListener ("UpdateInventoryItem", x);
			})) {
				this.m_UIManager.OnAddItemInventory.Invoke ();
				this.m_UIManager.LoadInventoryItems (m_InventoryComponent.GetInventoryItems (), this.ExecuteInventoryItem);
			}
		}

		public virtual void AddEquipmentItem(CEnum.EItemSlot slot, IItem value) {
		
		}

		public override void SpawnResourceMaterials ()
		{
			if (this.GetOtherInteractive () == false)
				return;
			base.SpawnResourceMaterials ();
			if (m_Data == null || m_Data.resourceMaterials.Length == 0)
				return;
			for (int i = 0; i < m_Data.resourceMaterials.Length; i++) {
				var itemData = m_Data.resourceMaterials [i];
				var randomCircle = UnityEngine.Random.insideUnitCircle; 
				var randomAround = randomCircle * this.GetSize ();
				var randomPosition = new Vector3 (randomAround.x, 0f, randomAround.y) + this.GetPosition ();
				if ((Mathf.Abs(randomCircle.x) * 100f) <= itemData.rate) {
					continue;
				}
				this.m_ObjectManager.GetObjectModified (itemData.name, (obj) => {
					var objectSpawned = obj as CNeutralObjectController;
					objectSpawned.Init ();
					objectSpawned.SetActive (true);
					objectSpawned.SetEnable (true);
					objectSpawned.SetStartPosition (randomPosition);
					objectSpawned.SetPosition (randomPosition);
					objectSpawned.SetCurrentAmount (itemData.currentAmount);
					return objectSpawned;
				});
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
			if (m_Data != null)
				return m_Data.name;
			return base.GetName ();
		}

		public override void SetAvatar (string value)
		{
			base.SetAvatar (value);
			m_Data.avatarPath = value;
		}

		public override string GetAvatar ()
		{
			base.GetAvatar ();
			return m_Data.avatarPath;
		}

		public override void SetCurrentHealth (int value)
		{
			base.SetCurrentHealth (value);
			m_Data.currentHealth = value;
			m_EventComponent.InvokeEventListener("HealthChange", (float)value / this.GetMaxHealth ());
		}

		public override int GetCurrentHealth ()
		{
			base.GetCurrentHealth ();
			return m_Data.currentHealth;
		}

		public override void SetMaxHealth (int value)
		{
			base.SetCurrentHealth (value);
			m_Data.maxHealth = value;
		}

		public override int GetMaxHealth ()
		{
			base.GetMaxHealth ();
			return m_Data.maxHealth;
		}

		public override void SetCurrentSanity (int value)
		{
			base.SetCurrentSanity (value);
			m_Data.currentSanity = value;
		}

		public override int GetCurrentSanity ()
		{
			base.GetCurrentSanity ();
			return m_Data.currentSanity;
		}

		public override int GetMaxSanity ()
		{
			base.GetMaxSanity ();
			return m_Data.maxSanity;
		}

		public override void SetCurrentHunger (int value)
		{
			base.SetCurrentHunger (value);
			m_Data.currentHunger = value;
		}

		public override int GetCurrentHunger ()
		{
			base.GetCurrentHunger ();
			return m_Data.currentHunger;
		}

		public override int GetMaxHunger ()
		{
			base.GetMaxHunger ();
			return m_Data.maxHunger;
		}

		public override int GetPhysicDefend() {
			base.GetPhysicDefend ();
			return m_Data.physicDefend;
		}

		public override int GetAttackDamage() {
			base.GetAttackDamage ();
			return m_Data.attackDamage;
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

		public override void SetInventoryItem(int slot, IItem value) {
			base.SetInventoryItem (slot, value);
			m_InventoryComponent.SetInventoryItem (slot, value);
			this.m_UIManager.LoadInventoryItems (m_InventoryComponent.GetInventoryItems (), this.ExecuteInventoryItem);
		}

		public override IItem[] GetInventoryItems() {
			base.GetInventoryItems();
			return m_InventoryComponent.GetInventoryItems ();
		}

		public override void SetEquipmentItem(int slot, IItem value) {
			base.SetEquipmentItem (slot, value);
			m_InventoryComponent.SetEquipmentItem ((CEnum.EItemSlot)slot, value);
		}

		public override IItem[] GetEquipmentItems() {
			base.GetEquipmentItems();
			return m_InventoryComponent.GetEquipmentItems ();
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

		public override CEnum.EClassType GetClassType ()
		{
			base.GetClassType ();
			return m_Data.classType;
		}

		public override void SetClassType (CEnum.EClassType value)
		{
			base.SetClassType (value);
			m_Data.classType = value;
		}

		public override Vector3 GetMovePosition() {
			if (m_MovableComponent == null)
				return base.GetMovePosition();
			var position = m_MovableComponent.targetPosition;
			position.y = 0f;
			return position;
		}

		public override void SetMovePosition(Vector3 value) {
			base.SetMovePosition (value);
			if (m_MovableComponent == null)
				return;
			value.y = 0f;
			m_MovableComponent.targetPosition = value;
		}

		public override Vector3 GetOriginTouchPoint() {
			base.GetOriginTouchPoint ();
			return m_OriginTouchPoint;
		}

		public override void SetOriginTouchPoint(Vector3 position) {
			base.SetOriginTouchPoint (position);
			m_OriginTouchPoint = position;
		}

		public override Vector3 GetDirectionTouchPoint() {
			base.GetDirectionTouchPoint ();
			return m_DirectionTouchPoint;
		}

		public override void SetDirectionTouchPoint(Vector3 position) {
			base.SetDirectionTouchPoint (position);
			m_DirectionTouchPoint = position;
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
