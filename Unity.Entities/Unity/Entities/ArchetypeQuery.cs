using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x0200022B RID: 555
	[GenerateTestsForBurstCompatibility]
	internal struct ArchetypeQuery : IEquatable<ArchetypeQuery>
	{
		// Token: 0x06000DBF RID: 3519 RVA: 0x00041CD0 File Offset: 0x0003FED0
		public unsafe bool Equals(ArchetypeQuery other)
		{
			return this.AnyCount == other.AnyCount && this.AllCount == other.AllCount && this.NoneCount == other.NoneCount && this.DisabledCount == other.DisabledCount && this.AbsentCount == other.AbsentCount && this.PresentCount == other.PresentCount && (this.AnyCount <= 0 || UnsafeUtility.MemCmp((void*)this.Any, (void*)other.Any, (long)(4 * this.AnyCount)) == 0 || UnsafeUtility.MemCmp((void*)this.AnyAccessMode, (void*)other.AnyAccessMode, (long)this.AnyCount) == 0) && (this.AllCount <= 0 || UnsafeUtility.MemCmp((void*)this.All, (void*)other.All, (long)(4 * this.AllCount)) == 0 || UnsafeUtility.MemCmp((void*)this.AllAccessMode, (void*)other.AllAccessMode, (long)this.AllCount) == 0) && (this.NoneCount <= 0 || UnsafeUtility.MemCmp((void*)this.None, (void*)other.None, (long)(4 * this.NoneCount)) == 0 || UnsafeUtility.MemCmp((void*)this.NoneAccessMode, (void*)other.NoneAccessMode, (long)this.NoneCount) == 0) && (this.DisabledCount <= 0 || UnsafeUtility.MemCmp((void*)this.Disabled, (void*)other.Disabled, (long)(4 * this.DisabledCount)) == 0 || UnsafeUtility.MemCmp((void*)this.DisabledAccessMode, (void*)other.DisabledAccessMode, (long)this.DisabledCount) == 0) && (this.AbsentCount <= 0 || UnsafeUtility.MemCmp((void*)this.Absent, (void*)other.Absent, (long)(4 * this.AbsentCount)) == 0 || UnsafeUtility.MemCmp((void*)this.AbsentAccessMode, (void*)other.AbsentAccessMode, (long)this.AbsentCount) == 0) && (this.PresentCount <= 0 || UnsafeUtility.MemCmp((void*)this.Present, (void*)other.Present, (long)(4 * this.PresentCount)) == 0 || UnsafeUtility.MemCmp((void*)this.PresentAccessMode, (void*)other.PresentAccessMode, (long)this.PresentCount) == 0) && this.Options == other.Options;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00041ED4 File Offset: 0x000400D4
		public unsafe override int GetHashCode()
		{
			int num = this.AnyCount + 1;
			num = (397 * num) ^ (this.AllCount + 1);
			num = (397 * num) ^ (this.NoneCount + 1);
			num = (397 * num) ^ (this.DisabledCount + 1);
			num = (397 * num) ^ (this.AbsentCount + 1);
			num = (397 * num) ^ (this.PresentCount + 1);
			num = (397 * num) ^ (int)this.Options;
			num = (int)math.hash((void*)this.Any, 4 * this.AnyCount, (uint)num);
			num = (int)math.hash((void*)this.All, 4 * this.AllCount, (uint)num);
			num = (int)math.hash((void*)this.None, 4 * this.NoneCount, (uint)num);
			num = (int)math.hash((void*)this.Disabled, 4 * this.DisabledCount, (uint)num);
			num = (int)math.hash((void*)this.Absent, 4 * this.AbsentCount, (uint)num);
			num = (int)math.hash((void*)this.Present, 4 * this.PresentCount, (uint)num);
			num = (int)math.hash((void*)this.AnyAccessMode, this.AnyCount, (uint)num);
			num = (int)math.hash((void*)this.AllAccessMode, this.AllCount, (uint)num);
			num = (int)math.hash((void*)this.NoneAccessMode, this.NoneCount, (uint)num);
			num = (int)math.hash((void*)this.DisabledAccessMode, this.DisabledCount, (uint)num);
			num = (int)math.hash((void*)this.AbsentAccessMode, this.AbsentCount, (uint)num);
			return (int)math.hash((void*)this.PresentAccessMode, this.PresentCount, (uint)num);
		}

		// Token: 0x040005BE RID: 1470
		public unsafe TypeIndex* Any;

		// Token: 0x040005BF RID: 1471
		public unsafe byte* AnyAccessMode;

		// Token: 0x040005C0 RID: 1472
		public int AnyCount;

		// Token: 0x040005C1 RID: 1473
		public unsafe TypeIndex* All;

		// Token: 0x040005C2 RID: 1474
		public unsafe byte* AllAccessMode;

		// Token: 0x040005C3 RID: 1475
		public int AllCount;

		// Token: 0x040005C4 RID: 1476
		public ulong AllBloomFilterMask;

		// Token: 0x040005C5 RID: 1477
		public unsafe TypeIndex* None;

		// Token: 0x040005C6 RID: 1478
		public unsafe byte* NoneAccessMode;

		// Token: 0x040005C7 RID: 1479
		public int NoneCount;

		// Token: 0x040005C8 RID: 1480
		public unsafe TypeIndex* Disabled;

		// Token: 0x040005C9 RID: 1481
		public unsafe byte* DisabledAccessMode;

		// Token: 0x040005CA RID: 1482
		public int DisabledCount;

		// Token: 0x040005CB RID: 1483
		public unsafe TypeIndex* Absent;

		// Token: 0x040005CC RID: 1484
		public unsafe byte* AbsentAccessMode;

		// Token: 0x040005CD RID: 1485
		public int AbsentCount;

		// Token: 0x040005CE RID: 1486
		public unsafe TypeIndex* Present;

		// Token: 0x040005CF RID: 1487
		public unsafe byte* PresentAccessMode;

		// Token: 0x040005D0 RID: 1488
		public int PresentCount;

		// Token: 0x040005D1 RID: 1489
		public EntityQueryOptions Options;
	}
}
