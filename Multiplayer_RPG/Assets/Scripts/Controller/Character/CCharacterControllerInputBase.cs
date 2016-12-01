﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {

	public partial class CCharacterController {

		protected CMovableComponent m_MovableComponent;

		public override void UpdateMoveInput() {
			if (this.GetUnderControl () == false)
				return;
			base.UpdateMoveInput ();
		}

		public override void UpdateBattleInput(string skillName) {
			if (this.GetUnderControl () == false)
				return;
			base.UpdateBattleInput (skillName);
		}

		public override void FindMovePosition() {
			if (this.GetUnderControl () == false)
				return;
			base.FindMovePosition ();
			// FIND RANDOM POSITION
			var randomAround = UnityEngine.Random.insideUnitCircle * this.GetSeekRadius ();
			var randomPos = new Vector3 (randomAround.x, 0f, randomAround.y) + this.GetStartPosition();
			this.SetMovePosition (randomPos);
		}
	
	}
}
