using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200000C RID: 12
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public class FcuPropertyHeader : PropertyHeader
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002A6E File Offset: 0x00000C6E
		public FcuPropertyHeader(FcuLocKey label, FcuLocKey tooltip)
			: base(new GUIContent(FcuPropertyHeader.GetLabelFromEnum(label), FcuPropertyHeader.GetLabelFromEnum(tooltip)))
		{
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002A87 File Offset: 0x00000C87
		private static string GetLabelFromEnum(FcuLocKey value)
		{
			return value.Localize(Array.Empty<object>());
		}
	}
}
