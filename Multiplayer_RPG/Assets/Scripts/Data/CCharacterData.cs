using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	[Serializable]
	public class CCharacterData : CObjectData {

		public string modelPath;
		public string avatarPath;

		public int[] attackableObjectTypes;

		public int currentHealth;
		public int maxHealth;
		public int currentSanity;
		public int maxSanity;
		public int currentHunger;
		public int maxHunger;

		public float attackRange;
		public float attackSpeed;
		public int attackDamage;

		public int physicDefend;

		public float moveSpeed;

		public float seekRadius;

		public string token;

		public CCharacterData () : base ()
		{
			this.modelPath = string.Empty;
			this.avatarPath = string.Empty;

			this.attackableObjectTypes = null;

			this.currentHealth = 0;
			this.maxHealth = 0;

			this.attackRange = 0f;
			this.attackSpeed = 0f;
			this.attackDamage = 0;

			this.physicDefend = 0;

			this.moveSpeed = 0f;

			this.seekRadius = 0f;

			this.token = string.Empty;
		}

	}
}
