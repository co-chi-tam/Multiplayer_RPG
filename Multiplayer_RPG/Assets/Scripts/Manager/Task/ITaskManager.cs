using System;

namespace SurvivalTest {
	public interface ITaskManager
	{

		void OnRegisterTask (ITask task);
		void OnUnregisterTask (ITask task);
		void RemoveAllTask();
	
	}
}

