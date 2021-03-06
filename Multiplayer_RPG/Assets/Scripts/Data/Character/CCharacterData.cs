﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	[Serializable]
	public class CCharacterData : CObjectData {

		public CEnum.EClassType classType;
		public int[] attackableObjectTypes;

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
			this.classType = CEnum.EClassType.None;

			this.attackableObjectTypes = new int[0];

			this.currentSanity = 0;
			this.maxSanity = 0;
			this.currentHunger = 0;
			this.maxHunger = 0;

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
