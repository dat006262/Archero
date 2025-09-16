




using System;
using UnityEngine;

namespace Dxx
{
	public static class ExtensionMethods
	{
		public static int ToInt(this float @this)
		{
			return Convert.ToInt32(@this);
		}

		public static float ToFloat(this int @this)
		{
			return Convert.ToSingle(@this);
		}

		public static bool Contains(this LayerMask mask, int layer)
		{
			return (mask.value & (1 << layer)) > 0;
		}

		public static bool Contains(this LayerMask mask, GameObject gameobject)
		{
			return (mask.value & (1 << gameobject.layer)) > 0;
		}
	}
}
