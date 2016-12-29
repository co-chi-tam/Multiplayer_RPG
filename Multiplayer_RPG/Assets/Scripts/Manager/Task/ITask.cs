using System;
using System.Collections;

namespace SurvivalTest {
	public interface ITask {

		void StartTask();
		void EndTask();

		bool OnTask();
		float OnTaskProcess();
		string GetTaskName();
	
	}
}

