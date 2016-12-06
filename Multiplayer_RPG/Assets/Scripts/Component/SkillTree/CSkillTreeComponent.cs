using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CSkillTreeComponent : CComponent
	{

		public Action<CSkillNodeData, int> OnLevelUp;

		protected CSkillNodeData m_SkillRoot;
		protected Dictionary<string, CSkillNodeData> m_SkillMap;

		public CSkillTreeComponent (string json) : base ()
		{
			this.m_SkillRoot = TinyJSON.JSON.Load (json).Make<CSkillNodeData> ();
			this.m_SkillMap = new Dictionary<string, CSkillNodeData> ();
		}

		public CSkillTreeComponent (CSkillNodeData root) : base ()
		{
			this.m_SkillRoot = root;
			this.m_SkillMap = new Dictionary<string, CSkillNodeData> ();
		}

		public void AddExp(string id, int exp) {
			if (this.m_SkillMap.ContainsKey (id)) {
				var skill = this.m_SkillMap [id];
				skill.currentEXP += exp;
				while (skill.currentEXP >= skill.expPerLevel * (skill.currentLevel - 1)	
				    && skill.currentLevel < skill.maxLevel						
					&& skill.currentLevel > 0) {
					skill.currentLevel += 1;
					skill.currentEXP -= skill.expPerLevel * (skill.currentLevel - 1);
					if (OnLevelUp != null) {
						OnLevelUp (skill, skill.currentLevel);
					}
				}
			} else {
				Debug.Log ("SKILL ID: " + id + " NOT FOUND");
			}
		}

		public void AddLevel(string id, int level, int exp) {
			if (this.m_SkillMap.ContainsKey (id)) {
				var skill = this.m_SkillMap [id];
				if (level <= skill.maxLevel) {
					skill.currentLevel = level;
					skill.currentEXP = exp;
					if (OnLevelUp != null) {
						OnLevelUp (skill, skill.currentLevel);
					}
				}
			} else {
				Debug.Log ("SKILL ID: " + id + " NOT FOUND");
			}
		}

		public void GenerateMapSkillTree(Action<CSkillNodeData, CSkillNodeData> onChildLoaded) {
			GenerateMapSkillTree (m_SkillRoot, onChildLoaded);
		}

		public void GenerateMapSkillTree(CSkillNodeData parent, Action<CSkillNodeData, CSkillNodeData> onChildLoaded) {
			if (parent == null)
				return;
			m_SkillMap.Add (parent.id, parent);
			LoadMapSkillTree (parent, (p, c) => {
				m_SkillMap.Add (c.id, c);
				if (onChildLoaded != null) {
					onChildLoaded (p, c);
				}
			}, null);
		}

		public void LoadMapSkillTree(Action<CSkillNodeData, CSkillNodeData> onChildLoaded, Func<CSkillNodeData, CSkillNodeData, bool> condition) {
			LoadMapSkillTree (m_SkillRoot, onChildLoaded, condition);
		}

		public void LoadMapSkillTree(CSkillNodeData parent, Action<CSkillNodeData, CSkillNodeData> onChildLoaded, Func<CSkillNodeData, CSkillNodeData, bool> condition) {
			if (parent == null)
				return;
			for (int i = 0; i < parent.childSkills.Length; i++) {
				var child = parent.childSkills [i];
				if (child != null) {
					if (onChildLoaded != null) {
						if (condition != null) {
							if (condition (parent, child)) {
								onChildLoaded (parent, child);
							}
						} else {
							onChildLoaded (parent, child);
						}
					}
					LoadMapSkillTree (child, onChildLoaded, condition);
				}
			}
		}

		public CSkillNodeData GetRoot() {
			return m_SkillRoot;
		}
	
		public CSkillNodeData GetSkill(string id) {
			CSkillNodeData parent = null;
			if (m_SkillRoot.id == id) {
				return m_SkillRoot;
			} else {
				for (int i = 0; i < m_SkillRoot.childSkills.Length; i++) {
					if (m_SkillRoot.childSkills [i] != null) {
						parent = FindChild (m_SkillRoot, m_SkillRoot.childSkills [i], (p, c) => {
							return c != null && c.id == id && p.currentLevel >= c.unlockLevel;
						});
						if (parent != null) {
							return parent;						
						}
					}
				}
			}
			return null;
		}

		public CSkillNodeData GetSkill(Func<CSkillNodeData, CSkillNodeData, bool> condition) {
			CSkillNodeData parent = null;
			if (condition != null && m_SkillRoot != null) {
				if (condition(null, m_SkillRoot)) {
					return m_SkillRoot;
				} else {
					for (int i = 0; i < m_SkillRoot.childSkills.Length; i++) {
						if (m_SkillRoot.childSkills [i] != null) {
							parent = FindChild (m_SkillRoot, m_SkillRoot.childSkills [i], condition);
						}
						if (parent != null) {
							return parent;						
						}
					}
				}
			}
			return null;
		}

		private CSkillNodeData FindChild(CSkillNodeData grand, CSkillNodeData parent, Func<CSkillNodeData, CSkillNodeData, bool> condition) {
			if (condition != null && parent != null) {
				if (condition(grand, parent)) {
					return parent;
				} else {
					for (int i = 0; i < parent.childSkills.Length; i++) {
						return FindChild (parent, parent.childSkills [i], condition);
					}
				}
			}
			return null;
		}

	}
}

