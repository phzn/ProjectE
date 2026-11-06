using System;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000D7 RID: 215
	internal static class SpaceFillingCurves
	{
		// Token: 0x060006CC RID: 1740 RVA: 0x000288DB File Offset: 0x00026ADB
		private static uint Part1By1(uint x)
		{
			x &= 65535U;
			x = (x ^ (x << 8)) & 16711935U;
			x = (x ^ (x << 4)) & 252645135U;
			x = (x ^ (x << 2)) & 858993459U;
			x = (x ^ (x << 1)) & 1431655765U;
			return x;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0002891B File Offset: 0x00026B1B
		private static uint Compact1By1(uint x)
		{
			x &= 1431655765U;
			x = (x ^ (x >> 1)) & 858993459U;
			x = (x ^ (x >> 2)) & 252645135U;
			x = (x ^ (x >> 4)) & 16711935U;
			x = (x ^ (x >> 8)) & 65535U;
			return x;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0002895B File Offset: 0x00026B5B
		public static uint EncodeMorton2D(uint2 coord)
		{
			return (SpaceFillingCurves.Part1By1(coord.y) << 1) + SpaceFillingCurves.Part1By1(coord.x);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00028976 File Offset: 0x00026B76
		public static uint2 DecodeMorton2D(uint code)
		{
			return math.uint2(SpaceFillingCurves.Compact1By1(code), SpaceFillingCurves.Compact1By1(code >> 1));
		}
	}
}
