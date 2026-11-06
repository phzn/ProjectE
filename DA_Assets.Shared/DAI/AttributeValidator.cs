using System;
using UnityEngine;

namespace DA_Assets.DAI
{
	// Token: 0x0200003D RID: 61
	public static class AttributeValidator
	{
		// Token: 0x06000130 RID: 304 RVA: 0x00006240 File Offset: 0x00004440
		public static void Validate<T>(params Type[] atts) where T : class
		{
			Type typeFromHandle = typeof(T);
			foreach (Type type in atts)
			{
				if (!typeof(Attribute).IsAssignableFrom(type))
				{
					Debug.LogError("Type '" + type.Name + "' is not an Attribute type.");
				}
				else if (Attribute.GetCustomAttribute(typeFromHandle, type) == null)
				{
					Debug.LogError(string.Concat(new string[] { "Attribute '", type.Name, "' is not applied to '", typeFromHandle.Name, "'." }));
				}
			}
		}
	}
}
