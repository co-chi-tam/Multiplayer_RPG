using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	[Serializable]
	public class CCharacterData : CObjectData {

		public string modelPath;
		public string avatarPath;

		public int currentHealth;
		public int maxHealth;

		public int currentMana;
		public int maxMana;

		public float attackRange;

		public int pureDamage;

		public int physicDamage;
		public int physicDefend;

		public int magicDamage;
		public int magicDefend;

		public float moveSpeed;

		public float seekRadius;

		public string token;

		public CCharacterData () : base ()
		{
			this.modelPath = string.Empty;
			this.avatarPath = string.Empty;

			this.currentHealth = 0;
			this.maxHealth = 0;

			this.currentMana = 0;
			this.maxMana = 0;

			this.attackRange = 0;

			this.pureDamage = 0;

			this.physicDamage = 0;
			this.physicDefend = 0;

			this.magicDamage = 0;
			this.magicDefend = 0;

			this.moveSpeed = 0f;

			this.seekRadius = 0f;

			this.token = string.Empty;
		}

	}
}
