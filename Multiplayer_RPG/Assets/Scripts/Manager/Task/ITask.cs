using System;
using System.Collections;

namespace SurvivalTest {
	public interface ITask {

		bool OnTask();
		float OnTaskProcess();
		string GetTaskName();
	
	}
}

