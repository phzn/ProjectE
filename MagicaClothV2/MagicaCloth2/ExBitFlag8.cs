using System;
using System.Runtime.CompilerServices;

namespace MagicaCloth2
{
	// Token: 0x0200010C RID: 268
	[Serializable]
	public struct ExBitFlag8
	{
		// Token: 0x0600050C RID: 1292 RVA: 0x0002D91C File Offset: 0x0002BB1C
		public ExBitFlag8(byte initialValue = 0)
		{
			this.Value = initialValue;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0002D925 File Offset: 0x0002BB25
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear()
		{
			this.Value = 0;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0002D92E File Offset: 0x0002BB2E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetFlag(byte flag, bool sw)
		{
			if (sw)
			{
				this.Value |= flag;
				return;
			}
			this.Value &= ~flag;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0002D953 File Offset: 0x0002BB53
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsSet(byte flag)
		{
			return (this.Value & flag) > 0;
		}

		// Token: 0x0400089E RID: 2206
		public byte Value;
	}
}
