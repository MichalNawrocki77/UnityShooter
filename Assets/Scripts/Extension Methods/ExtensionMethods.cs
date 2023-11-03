using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
	public static float Remap(this float num,
								float oldMin, float oldMax,
								float newMin, float newMax)
	{
		return ((num - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
	}
}
