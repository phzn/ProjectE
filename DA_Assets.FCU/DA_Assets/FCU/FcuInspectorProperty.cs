using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200000B RID: 11
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class FcuInspectorProperty : CustomInspectorProperty
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00002A43 File Offset: 0x00000C43
		public FcuInspectorProperty(ComponentType type, FcuLocKey label, FcuLocKey tooltip, float minValue = 0f, float maxValue = 1f)
			: base(type, new GUIContent(FcuInspectorProperty.GetLabelFromEnum(label), FcuInspectorProperty.GetLabelFromEnum(tooltip)), minValue, maxValue)
		{
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002A61 File Offset: 0x00000C61
		private static string GetLabelFromEnum(FcuLocKey value)
		{
			return value.Localize(Array.Empty<object>());
		}
	}
}
