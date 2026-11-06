using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000214 RID: 532
	public static class PointerType
	{
		// Token: 0x06000F74 RID: 3956 RVA: 0x000397E4 File Offset: 0x000379E4
		internal static string GetPointerType(int pointerId)
		{
			bool flag = pointerId == PointerId.mousePointerId;
			string text;
			if (flag)
			{
				text = PointerType.mouse;
			}
			else
			{
				bool flag2 = pointerId == PointerId.penPointerIdBase;
				if (flag2)
				{
					text = PointerType.pen;
				}
				else
				{
					text = PointerType.touch;
				}
			}
			return text;
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00039824 File Offset: 0x00037A24
		internal static bool IsDirectManipulationDevice(string pointerType)
		{
			return pointerType == PointerType.touch || pointerType == PointerType.pen;
		}

		// Token: 0x040006F6 RID: 1782
		public static readonly string mouse = "mouse";

		// Token: 0x040006F7 RID: 1783
		public static readonly string touch = "touch";

		// Token: 0x040006F8 RID: 1784
		public static readonly string pen = "pen";

		// Token: 0x040006F9 RID: 1785
		public static readonly string unknown = "";
	}
}
