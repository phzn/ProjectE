using System;

namespace Unity.Entities
{
	// Token: 0x0200010C RID: 268
	internal struct ECBChunk
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x000175B1 File Offset: 0x000157B1
		internal int Capacity
		{
			get
			{
				return this.Size - this.Used;
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000175C0 File Offset: 0x000157C0
		internal int Bump(int size)
		{
			int used = this.Used;
			this.Used += size;
			return used;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x000175D8 File Offset: 0x000157D8
		internal unsafe int BaseSortKey
		{
			get
			{
				fixed (ECBChunk* ptr = &this)
				{
					ECBChunk* ptr2 = ptr;
					if (this.Used < sizeof(BasicCommand))
					{
						return -1;
					}
					BasicCommand* ptr3 = (BasicCommand*)(ptr2 + 1);
					return ptr3->SortKey;
				}
			}
		}

		// Token: 0x040002FF RID: 767
		internal int Used;

		// Token: 0x04000300 RID: 768
		internal int Size;

		// Token: 0x04000301 RID: 769
		internal unsafe ECBChunk* Next;

		// Token: 0x04000302 RID: 770
		internal unsafe ECBChunk* Prev;
	}
}
