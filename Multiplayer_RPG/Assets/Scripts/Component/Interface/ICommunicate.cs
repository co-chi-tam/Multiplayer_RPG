﻿using UnityEngine;
using System;
using System.Collections;

namespace SurvivalTest {
	public interface ICommunicate {

		void Chat(string value);
		void SetChat (string value);
		string GetChat();

		void ShowEmotion(string value);
		void SetEmotion (string value);
		string GetEmotion();
	
	}
}

