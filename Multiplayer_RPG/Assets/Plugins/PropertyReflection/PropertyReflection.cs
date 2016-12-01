using System;
using System.Collections;
using System.Reflection;

namespace DataReflection {
	public class PropertyReflection {

		public virtual void SetField(string name, object value)
		{
			var fieldInfo = this.GetType ().GetField (name);
			fieldInfo.SetValue (this, value);
		}

		public virtual object GetField(string name)
		{
			var fieldInfo = this.GetType ().GetField (name);
			return fieldInfo.GetValue(this);
		}

		public virtual void SetProperty(string name, object value)
		{
			var propertyInfo = this.GetType ().GetProperty (name);
			var newValue = Convert.ChangeType(value, propertyInfo.PropertyType);
			MethodInfo methodInfo = propertyInfo.GetSetMethod();
			methodInfo.Invoke(this, new object[] { newValue });
		}

		public virtual T GetProperty<T>(string name)
		{
			var propertyInfo = this.GetType ().GetProperty (name);
			var propertyValue = propertyInfo.GetValue (this, null);
			var propertyCast = (T)Convert.ChangeType(propertyValue, typeof(T));
			return propertyCast;
		}

	}
}
