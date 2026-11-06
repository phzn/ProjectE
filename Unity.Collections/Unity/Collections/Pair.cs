using System;

namespace Unity.Collections
{
	// Token: 0x02000049 RID: 73
	internal struct Pair<Key, Value>
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x000068FE File Offset: 0x00004AFE
		public Pair(Key k, Value v)
		{
			this.key = k;
			this.value = v;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000690E File Offset: 0x00004B0E
		public override string ToString()
		{
			return string.Format("{0} = {1}", this.key, this.value);
		}

		// Token: 0x040000BA RID: 186
		public Key key;

		// Token: 0x040000BB RID: 187
		public Value value;
	}
}
