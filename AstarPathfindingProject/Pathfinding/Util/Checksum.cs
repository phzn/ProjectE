using System;

namespace Pathfinding.Util
{
	// Token: 0x0200029F RID: 671
	internal class Checksum
	{
		// Token: 0x060010D1 RID: 4305 RVA: 0x00065718 File Offset: 0x00063918
		public static uint GetChecksum(byte[] arr, uint hash = 0U)
		{
			hash ^= 2166136261U;
			if (arr == null)
			{
				return hash - 1U;
			}
			for (int i = 0; i < arr.Length; i++)
			{
				hash = (hash ^ (uint)arr[i]) * 16777619U;
			}
			return hash;
		}
	}
}
