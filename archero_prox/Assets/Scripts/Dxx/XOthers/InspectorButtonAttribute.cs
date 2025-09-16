




using System;
using UnityEngine;

namespace Dxx
{
	[AttributeUsage(AttributeTargets.Field)]
	public class InspectorButtonAttribute : PropertyAttribute
	{
		public readonly string MethodName;

		public InspectorButtonAttribute(string MethodName)
		{
			this.MethodName = MethodName;
		}
	}
}
