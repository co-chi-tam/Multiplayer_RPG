using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	[Serializable]
	public class CSkillNodeData : CObjectData
	{
		public int currentEXP;
		public int expPerLevel;
		public int currentLevel;
		public int maxLevel;
		public int unlockLevel;
		public CSkillNodeData[] childSkills;

		public CSkillNodeData () : base()
		{
			this.id = string.Empty;
			this.name = string.Empty;
			this.currentEXP = 0;
			this.expPerLevel = 0;
			this.currentLevel = 0;
			this.maxLevel = 0;
			this.unlockLevel = 0;
			this.childSkills = null;
		}

	}
}

