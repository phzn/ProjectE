using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000B6 RID: 182
	internal static class RenderPassEventsEnumValues
	{
		// Token: 0x060005EA RID: 1514 RVA: 0x00024140 File Offset: 0x00022340
		static RenderPassEventsEnumValues()
		{
			Array array = Enum.GetValues(typeof(RenderPassEvent));
			RenderPassEventsEnumValues.values = new int[array.Length];
			int num = 0;
			foreach (object obj in array)
			{
				int num2 = (int)obj;
				RenderPassEventsEnumValues.values[num] = num2;
				num++;
			}
		}

		// Token: 0x040004B3 RID: 1203
		public static int[] values;
	}
}
