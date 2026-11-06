using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000075 RID: 117
	public static class IBindingExtensions
	{
		// Token: 0x06000549 RID: 1353 RVA: 0x0001490C File Offset: 0x00012B0C
		public static bool IsBound(this IBindable control)
		{
			return ((control != null) ? control.binding : null) != null;
		}
	}
}
