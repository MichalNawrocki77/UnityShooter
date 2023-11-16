using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I know that making a seperate class for one extension method is an overkill, but just in case It's gonna make more sense in the future I made it.
public static class ExtensionMethods
{
	public static float Remap(this float num,
								float oldMin, float oldMax,
								float newMin, float newMax)
	{
		return ((num - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
	}
}
