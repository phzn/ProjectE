using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000069 RID: 105
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct StringReference
	{
		// Token: 0x170000D1 RID: 209
		public char this[int i]
		{
			get
			{
				return this._chars[i];
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x000182C7 File Offset: 0x000164C7
		public char[] Chars
		{
			get
			{
				return this._chars;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x000182CF File Offset: 0x000164CF
		public int StartIndex
		{
			get
			{
				return this._startIndex;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x000182D7 File Offset: 0x000164D7
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000182DF File Offset: 0x000164DF
		public StringReference(char[] chars, int startIndex, int length)
		{
			this._chars = chars;
			this._startIndex = startIndex;
			this._length = length;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000182F6 File Offset: 0x000164F6
		public override string ToString()
		{
			return new string(this._chars, this._startIndex, this._length);
		}

		// Token: 0x0400021D RID: 541
		private readonly char[] _chars;

		// Token: 0x0400021E RID: 542
		private readonly int _startIndex;

		// Token: 0x0400021F RID: 543
		private readonly int _length;
	}
}
