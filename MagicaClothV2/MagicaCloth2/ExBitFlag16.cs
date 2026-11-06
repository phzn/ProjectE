using System;
using System.Runtime.CompilerServices;

namespace MagicaCloth2
{
	// Token: 0x0200010B RID: 267
	[Serializable]
	public struct ExBitFlag16
	{
		// Token: 0x06000508 RID: 1288 RVA: 0x0002D8D8 File Offset: 0x0002BAD8
		public ExBitFlag16(ushort initialValue = 0)
		{
			this.Value = initialValue;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0002D8E1 File Offset: 0x0002BAE1
		public void Clear()
		{
			this.Value = 0;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0002D8EA File Offset: 0x0002BAEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetFlag(ushort flag, bool sw)
		{
			if (sw)
			{
				this.Value |= flag;
				return;
			}
			this.Value &= ~flag;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0002D90F File Offset: 0x0002BB0F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsSet(ushort flag)
		{
			return (this.Value & flag) > 0;
		}

		// Token: 0x0400089D RID: 2205
		public ushort Value;
	}
}
