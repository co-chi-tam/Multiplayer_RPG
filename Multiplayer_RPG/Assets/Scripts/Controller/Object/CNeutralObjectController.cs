using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	public class CNeutralObjectController : CObjectController, IItem  {

		#region Properties

		protected CObjectData m_Data;

		#endregion

		#region MonoBehaviour

		public override void Init ()
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
			var fsmJson = Resources.Load <TextAsset> (m_Data.fsmPath);
			m_FSMManager.LoadFSM (fsmJson.text);
			this.SetActive (true);
			this.SetEnable (true);
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

		#endregion

		#region Main methods

		protected override void OnLoadData () {
			base.OnLoadData ();
			this.m_Data = new CObjectData ();
			if (this.GetDataUpdate ()) {
				m_Data = TinyJSON.JSON.Load (m_DataText.text).Make<CObjectData> ();
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

		public override CObjectData GetData ()
		{
			base.GetData ();
			return m_Data;
		}

		public override void SetData (CObjectData value)
		{
			base.SetData (value);
			m_Data = value;
		}

		public override string GetName ()
		{
			base.GetName ();
			return m_Data.name;
		}

		public override void SetName (string value)
		{
			base.SetName (value);
			m_Data.name = value;
		}

		public override CEnum.EObjectType GetObjectType ()
		{
			base.GetObjectType ();
			return m_Data.objectType;
		}

		public override void SetObjectType (CEnum.EObjectType objectType)
		{
			base.SetObjectType (objectType);
			m_Data.objectType = objectType;	
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

		public override int GetCurrentHealth ()
		{
			base.GetCurrentHealth ();
			return m_Data.currentHealth;
		}

		public override void SetCurrentHealth (int value)
		{
			base.SetCurrentHealth (value);
			m_Data.currentHealth = value;
		}

		public override int GetMaxHealth ()
		{
			base.GetMaxHealth ();
			return m_Data.maxHealth;
		}

		public override string GetFSMName ()
		{
			base.GetFSMName ();
			return m_Data.fsmPath;
		}

		public override void SetFSMName (string value)
		{
			base.SetFSMName (value);
			m_Data.fsmPath = value;
		}

		public virtual int GetInventorySlot () {
			return 0;
		}

		public virtual void SetInventorySlot (int value) {

		}

		public virtual CEnum.EItemSlot GetItemSlot() {
			return CEnum.EItemSlot.Inventory;
		}

		public virtual void SetItemSlot(CEnum.EItemSlot value) {
			
		}

		public virtual int GetCurrentAmount() {
			return 0;
		}

		public virtual void SetCurrentAmount(int value) {
			
		}

		public virtual int GetMaxAmount() {
			return 0;
		}

		#endregion

	}
}
