using System;
using System.Reflection;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200002E RID: 46
	[AttributeUsage(AttributeTargets.Method)]
	public class GUITargetAttribute : Attribute
	{
		// Token: 0x06000373 RID: 883 RVA: 0x0000C493 File Offset: 0x0000A693
		public GUITargetAttribute()
		{
			this.displayMask = -1;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000C4A4 File Offset: 0x0000A6A4
		public GUITargetAttribute(int displayIndex)
		{
			this.displayMask = 1 << displayIndex;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000C4BA File Offset: 0x0000A6BA
		public GUITargetAttribute(int displayIndex, int displayIndex1)
		{
			this.displayMask = (1 << displayIndex) | (1 << displayIndex1);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000C4D8 File Offset: 0x0000A6D8
		public GUITargetAttribute(int displayIndex, int displayIndex1, params int[] displayIndexList)
		{
			this.displayMask = (1 << displayIndex) | (1 << displayIndex1);
			for (int i = 0; i < displayIndexList.Length; i++)
			{
				this.displayMask |= 1 << displayIndexList[i];
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000C528 File Offset: 0x0000A728
		[RequiredByNativeCode]
		private static int GetGUITargetAttrValue(Type klass, string methodName)
		{
			MethodInfo method = klass.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			bool flag = method != null;
			if (flag)
			{
				object[] customAttributes = method.GetCustomAttributes(true);
				bool flag2 = customAttributes != null;
				if (flag2)
				{
					for (int i = 0; i < customAttributes.Length; i++)
					{
						bool flag3 = customAttributes[i].GetType() != typeof(GUITargetAttribute);
						if (!flag3)
						{
							GUITargetAttribute guitargetAttribute = customAttributes[i] as GUITargetAttribute;
							return guitargetAttribute.displayMask;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x040000E4 RID: 228
		internal int displayMask;
	}
}
