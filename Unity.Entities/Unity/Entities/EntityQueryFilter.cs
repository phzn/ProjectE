using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Unity.Entities
{
	// Token: 0x02000221 RID: 545
	internal struct EntityQueryFilter
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x0003F1E6 File Offset: 0x0003D3E6
		public bool RequiresMatchesFilter
		{
			get
			{
				return this.Shared.Count != 0 || this.Changed.Count != 0 || this._UseOrderFiltering > 0U;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x0003F20D File Offset: 0x0003D40D
		// (set) Token: 0x06000D92 RID: 3474 RVA: 0x0003F218 File Offset: 0x0003D418
		public bool UseOrderFiltering
		{
			get
			{
				return this._UseOrderFiltering > 0U;
			}
			internal set
			{
				this._UseOrderFiltering = (value ? 1U : 0U);
			}
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0003F228 File Offset: 0x0003D428
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertValid()
		{
			if (this.Shared.Count < 0 || this.Shared.Count > 2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Shared.Count {0} is out of range [0..{1}]", this.Shared.Count, 2));
			}
			if (this.Changed.Count < 0 || this.Changed.Count > 2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Changed.Count {0} is out of range [0..{1}]", this.Changed.Count, 2));
			}
		}

		// Token: 0x0400059C RID: 1436
		public uint RequiredChangeVersion;

		// Token: 0x0400059D RID: 1437
		public EntityQueryFilter.SharedComponentData Shared;

		// Token: 0x0400059E RID: 1438
		public EntityQueryFilter.ChangedFilter Changed;

		// Token: 0x0400059F RID: 1439
		private uint _UseOrderFiltering;

		// Token: 0x02000222 RID: 546
		public struct SharedComponentData
		{
			// Token: 0x040005A0 RID: 1440
			public const int Capacity = 2;

			// Token: 0x040005A1 RID: 1441
			public int Count;

			// Token: 0x040005A2 RID: 1442
			[FixedBuffer(typeof(int), 2)]
			public EntityQueryFilter.SharedComponentData.<IndexInEntityQuery>e__FixedBuffer IndexInEntityQuery;

			// Token: 0x040005A3 RID: 1443
			[FixedBuffer(typeof(int), 2)]
			public EntityQueryFilter.SharedComponentData.<SharedComponentIndex>e__FixedBuffer SharedComponentIndex;

			// Token: 0x02000223 RID: 547
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 8)]
			public struct <IndexInEntityQuery>e__FixedBuffer
			{
				// Token: 0x040005A4 RID: 1444
				public int FixedElementField;
			}

			// Token: 0x02000224 RID: 548
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 8)]
			public struct <SharedComponentIndex>e__FixedBuffer
			{
				// Token: 0x040005A5 RID: 1445
				public int FixedElementField;
			}
		}

		// Token: 0x02000225 RID: 549
		public struct ChangedFilter
		{
			// Token: 0x040005A6 RID: 1446
			public const int Capacity = 2;

			// Token: 0x040005A7 RID: 1447
			public int Count;

			// Token: 0x040005A8 RID: 1448
			[FixedBuffer(typeof(int), 2)]
			public EntityQueryFilter.ChangedFilter.<IndexInEntityQuery>e__FixedBuffer IndexInEntityQuery;

			// Token: 0x02000226 RID: 550
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 8)]
			public struct <IndexInEntityQuery>e__FixedBuffer
			{
				// Token: 0x040005A9 RID: 1449
				public int FixedElementField;
			}
		}
	}
}
