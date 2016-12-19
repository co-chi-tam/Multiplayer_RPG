using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public interface ISkill {

		void Init ();
		void SetActive (bool value);
		void SetEnable (bool value);
		void SetStartPosition (Vector3 position);
		void SetPosition (Vector3 position);
		void SetTargetInteract (CObjectController value);
		void SetMovePosition (Vector3 position);
		void RemoveAllStartActionListener ();
		void AddStartActionListener (Action<object> value);
		void RemoveAllEndActionListener ();
		void AddEndActionListener (Action<object> value);
	
	}
}

