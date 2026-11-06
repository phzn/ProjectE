using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200007C RID: 124
	public class Box : VisualElement
	{
		// Token: 0x0600055E RID: 1374 RVA: 0x0001507A File Offset: 0x0001327A
		public Box()
		{
			base.AddToClassList(Box.ussClassName);
		}

		// Token: 0x0400021E RID: 542
		public static readonly string ussClassName = "unity-box";

		// Token: 0x0200007D RID: 125
		public new class UxmlFactory : UxmlFactory<Box>
		{
		}
	}
}
