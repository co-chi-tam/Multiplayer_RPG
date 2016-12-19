using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CSkillComponent : CComponent
	{

		protected ISkill m_Skill;

		public CSkillComponent (ISkill value) : base()
		{
			this.m_Skill = value;
		}
	
	}
}

