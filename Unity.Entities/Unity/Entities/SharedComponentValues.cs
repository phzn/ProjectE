using System;

namespace Unity.Entities
{
	// Token: 0x0200031D RID: 797
	internal struct SharedComponentValues
	{
		// Token: 0x170001AD RID: 429
		public unsafe ref int this[int i]
		{
			get
			{
				return ref this.firstIndex[i * this.stride / 4];
			}
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x000509B0 File Offset: 0x0004EBB0
		public unsafe static implicit operator SharedComponentValues(int* p)
		{
			return new SharedComponentValues
			{
				firstIndex = p,
				stride = 4
			};
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x000509D8 File Offset: 0x0004EBD8
		public unsafe bool EqualTo(SharedComponentValues otherValues, int sharedComponentCount)
		{
			for (int i = 0; i < sharedComponentCount; i++)
			{
				if (*otherValues[i] != *this[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x00050A08 File Offset: 0x0004EC08
		public unsafe void CopyTo(int* dest, int startIndex, int count)
		{
			for (int i = 0; i < count; i++)
			{
				dest[i] = *this[startIndex + i];
			}
		}

		// Token: 0x040007C4 RID: 1988
		public unsafe int* firstIndex;

		// Token: 0x040007C5 RID: 1989
		public int stride;
	}
}
