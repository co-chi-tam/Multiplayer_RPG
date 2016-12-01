using System;
using System.Collections;

namespace SurvivalTest {
	public interface IMovable {

		string GetID();
		float GetMoveSpeed();
		float GetDistanceToTarget();
		void SetIsObstacle(bool value);
		bool GetIsObstacle();

	}
}
