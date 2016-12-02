﻿using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	
	[RequireComponent(typeof(NetworkIdentity))]
	public class CEntity : NetworkBehaviour {
	
		#region Properties

		// Sync Object
		protected IStatus m_ObjectSyn;

		// Network
		protected float m_FixedTimeSync = 0.1f;
		protected CNetworkManager m_NetworkManager;
		// Sync time
		protected CountdownTime m_CountDownFixedTimeSync;
		// Transform
		protected Vector3 m_MovePosition;
		protected Vector3 m_Position;
		protected Vector3 m_Rotation;
		// Animation
		protected int m_Animation;
		protected float m_AnimationTime;
		// Info
		public string uID;
		// Control data
		protected string m_FSMStateName;
		public CCharacterData controlData;

		#endregion

		#region Implementation MonoBehaviour

		protected virtual void OnEnable() {
			
		}

		protected virtual void OnDisable() {

		}

		// Init to first spawn object
		public virtual void Init() {
			// Move Positio
			m_ObjectSyn.SetMovePosition (m_MovePosition);
			// Transfrom
			m_ObjectSyn.SetPosition (m_Position);
			m_ObjectSyn.SetStartPosition (m_Position);
			m_ObjectSyn.SetRotation (m_Rotation);
			// Animation
			m_ObjectSyn.SetAnimation ((CEnum.EAnimation) m_Animation);
			m_ObjectSyn.SetAnimationTime (m_AnimationTime);
			// Info
			m_ObjectSyn.SetID(uID);
		}

		// Awake GameObject
		protected virtual void Awake() {
			this.controlData = new CCharacterData ();
			this.uID = string.Empty;
		}

		// Start GameObject
		protected virtual void Start () {
			m_NetworkManager = CNetworkManager.GetInstance ();
			m_CountDownFixedTimeSync = new CountdownTime (m_FixedTimeSync, true);
			OnCreateControlObject ();
		}

		// Active on Server
		public virtual void OnServerLoadedObject ()
		{
			m_ObjectSyn.SetLocalUpdate (true);
			m_ObjectSyn.SetOtherInteractive (true);
			m_ObjectSyn.SetDataUpdate (false);
			Init ();
		}

		// Active On local and is local player
		public virtual void OnLocalPlayerLoadedObject  ()
		{
			m_ObjectSyn.SetLocalUpdate (true);
			m_ObjectSyn.SetOtherInteractive (false);
			m_ObjectSyn.SetDataUpdate (false);
			m_NetworkManager.OnClientRegisterEntity (this);
			Init ();
		}

		// Active On local and is client
		public virtual void OnClientLoadedObject ()
		{
			m_ObjectSyn.SetLocalUpdate (true);
			m_ObjectSyn.SetOtherInteractive (false);
			m_ObjectSyn.SetDataUpdate (false);
			m_NetworkManager.OnClientRegisterEntity (this);
			Init ();
		}

		// Mono Update
		protected virtual void Update () {
			this.OnServerUpdateBaseTime (Time.deltaTime);
			this.OnClientUpdateBaseTime (Time.deltaTime);
		}

		// Mono FixedUpdate
		protected virtual void FixedUpdate() {
			this.OnServerFixedUpdateBaseTime (Time.fixedDeltaTime);
			this.OnClientFixedUpdateBaseTime (Time.fixedDeltaTime);
		}

		// Mono Destroy
		[ClientCallback]
		protected virtual void OnDestroy() {

		}

		// Mono Application Quit
		[ClientCallback]
		public virtual void OnApplicationQuit() {
			m_NetworkManager.StopClient ();
		}

		// Mono Application Focus
		[ClientCallback]
		public virtual void OnApplicationFocus(bool value) {
		
		}

		#endregion

		#region Server

		// On Server Update Base time
		[ServerCallback]
		public virtual void OnServerUpdateBaseTime(float dt) {

		}

		// On Server FixedUpdate Base time
		[ServerCallback]
		public virtual void OnServerFixedUpdateBaseTime(float dt) {
			var onTime = 0f;
			if (m_CountDownFixedTimeSync.UpdateTime (dt, out onTime)) {
				// Update sync data
				m_MovePosition = m_ObjectSyn.GetMovePosition ();
				m_Position = m_ObjectSyn.GetPosition ();
				m_Rotation = m_ObjectSyn.GetRotation ();
				// Server sync data
				OnServerFixedUpdateSynData (dt);
				// Update client
				RpcFixedUpdateClientSyncTime (onTime);
			}
		}

		[ServerCallback]
		public virtual void OnServerFixedUpdateSynData(float dt) {
			// Update Info
			RpcUpdateInfo (m_ObjectSyn.GetID()); 
			// Update control Data
			RpcUpdateControlData (this.m_ObjectSyn.GetActive(),
				this.controlData.modelPath, 
				this.controlData.currentHealth, this.controlData.maxHealth, 
				this.controlData.moveSpeed, this.controlData.seekRadius,
				(int)this.controlData.objectType,
				this.m_ObjectSyn.GetFSMStateName());
			// Update transform
			RpcUpdateTransform (m_ObjectSyn.GetMovePosition(), m_ObjectSyn.GetPosition (), m_ObjectSyn.GetRotation());
			// Update animation
			RpcUpdateAnimation ((int) m_ObjectSyn.GetAnimation (), m_ObjectSyn.GetAnimationTime ());
		}

		// On Server Destroy Object 
		[ServerCallback]
		public virtual void OnServerDestroyObject() {
			m_ObjectSyn.OnDestroyObject ();
		}

		#endregion

		#region Client

		// On Client Update
		[ClientCallback]
		public virtual void OnClientUpdateBaseTime(float dt) {
			
		}

		// On Client Fixed Update
		[ClientCallback]
		public virtual void OnClientFixedUpdateBaseTime(float dt) {
			
		}

		[ClientCallback]
		public virtual void OnClientFixedUpdateSyncTime(float dt) {
		
		}

		// On Clients Network Destroy
		public override void OnNetworkDestroy ()
		{
			base.OnNetworkDestroy ();
			m_ObjectSyn.OnDestroyObject ();
		}

		public virtual void OnClientUpdateTransform() {
			if (m_ObjectSyn == null)
				return;
			// Move Position
			m_ObjectSyn.SetMovePosition (m_MovePosition);
			// Transform
			if (m_Position != m_ObjectSyn.GetPosition()) {
				var lerpPosition = Vector3.Lerp (m_ObjectSyn.GetPosition (), m_Position, 0.5f);
				m_ObjectSyn.SetPosition (lerpPosition);
			}
			if (m_Rotation != m_ObjectSyn.GetRotation ()) {
				var lerpRotation = Vector3.Lerp (m_ObjectSyn.GetRotation (), m_Rotation, 0.5f);
				m_ObjectSyn.SetRotation (lerpRotation);
			}
		}

		public virtual void OnClientUpdateAnimation() {
			if (m_ObjectSyn == null)
				return;
			var animation = (CEnum.EAnimation)m_Animation;
			if (animation != m_ObjectSyn.GetAnimation ()) {
				m_ObjectSyn.SetAnimation (animation);
				m_ObjectSyn.SetAnimationTime (m_AnimationTime);
			}
		}

		#endregion

		#region Main methods

		public virtual void OnCreateControlObject() {
			if (m_ObjectSyn == null) {
				StartCoroutine (HandleOnCreateControlObject());
			}
		}

		private IEnumerator HandleOnCreateControlObject() {
			while (string.IsNullOrEmpty (controlData.modelPath)) {
				yield return WaitHelper.WaitFixedUpdate;
			}
			yield return null;
			var goObj = Instantiate (Resources.Load <GameObject> (controlData.modelPath));
			m_ObjectSyn = goObj.GetComponent<IStatus> ();
			m_ObjectSyn.SetData (controlData);
			if (this.isServer) {
				OnServerLoadedObject ();
			} else {
				if (this.isLocalPlayer) {
					OnLocalPlayerLoadedObject ();
				} else {
					OnClientLoadedObject ();
				}
			}
		}

		#endregion

		#region Command

		#endregion

		#region RPC

		// RPC Entity info
		[ClientRpc]
		internal virtual void RpcUpdateInfo(string id) {
			this.SetID (id);
		}

		// RPC Control Data
		[ClientRpc]
		internal virtual void RpcUpdateControlData(bool active,
			string modelPath, 
			int currentHealth, int maxHealth, 
			float moveSpeed, float seekRadius,
			int objectType,
			string fsmStateName) {
			if (m_ObjectSyn != null) {
				m_ObjectSyn.SetActive (active);
				if (fsmStateName != m_ObjectSyn.GetFSMStateName ()) {
					m_ObjectSyn.SetFSMStateName (fsmStateName);
				}
			}
			this.controlData.modelPath = modelPath;
			this.controlData.currentHealth = currentHealth;
			this.controlData.maxHealth = maxHealth;
			this.controlData.moveSpeed = moveSpeed;
			this.controlData.seekRadius = seekRadius;
			this.controlData.objectType = (CEnum.EObjectType) objectType;
		}

		// RPC Update transform
		[ClientRpc]
		internal virtual void RpcUpdateTransform(Vector3 movePosition, Vector3 position, Vector3 rotation) {
			this.m_MovePosition = movePosition;
			this.m_Position = position;
			this.m_Rotation = rotation;
		}

		// RPC Update Animation
		[ClientRpc]
		internal virtual void RpcUpdateAnimation(int anim, float animTime) {
			this.m_Animation = anim;
			this.m_AnimationTime = animTime;
		}

		// RPC On Client Fixed Update Sync time
		[ClientRpc]
		internal virtual void RpcFixedUpdateClientSyncTime(float dt) {
			// Transform
			OnClientUpdateTransform();
			// Animation
			OnClientUpdateAnimation();
			// Client Update  
			OnClientFixedUpdateSyncTime(dt);
		}

		#endregion

		#region Getter && Setter

		public virtual void SetPosition(Vector3 position) {
			this.m_Position = position;
		}

		public virtual string GetID() {
			return uID;
		}

		public virtual void SetID(string value) {
			this.uID = value;
			if (m_ObjectSyn == null)
				return;
			m_ObjectSyn.SetID (value);
		}

		public virtual object GetController() {
			if (m_ObjectSyn == null)
				return null;
			return m_ObjectSyn.GetController ();
		}

		#endregion

	}

}
