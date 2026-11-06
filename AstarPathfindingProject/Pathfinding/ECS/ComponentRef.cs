using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.ECS
{
	// Token: 0x0200030F RID: 783
	public ref struct ComponentRef<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06001320 RID: 4896 RVA: 0x00073B25 File Offset: 0x00071D25
		public unsafe ComponentRef(byte* ptr)
		{
			this.ptr = ptr;
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06001321 RID: 4897 RVA: 0x00073B2E File Offset: 0x00071D2E
		public unsafe ref T value
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref *(T*)this.ptr;
			}
		}

		// Token: 0x04000E07 RID: 3591
		private unsafe byte* ptr;
	}
}
