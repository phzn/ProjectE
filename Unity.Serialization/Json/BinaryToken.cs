using System;

namespace Unity.Serialization.Json
{
	// Token: 0x0200004E RID: 78
	internal struct BinaryToken
	{
		// Token: 0x060001AD RID: 429 RVA: 0x00007798 File Offset: 0x00005998
		public override string ToString()
		{
			return string.Format("Type=[{0}] HandleIndex=[{1}] Position=[{2}] Parent=[{3}] Length=[{4}]", new object[] { this.Type, this.HandleIndex, this.Position, this.Parent, this.Length });
		}

		// Token: 0x040000D6 RID: 214
		public TokenType Type;

		// Token: 0x040000D7 RID: 215
		public int HandleIndex;

		// Token: 0x040000D8 RID: 216
		public int Position;

		// Token: 0x040000D9 RID: 217
		public int Parent;

		// Token: 0x040000DA RID: 218
		public int Length;

		// Token: 0x040000DB RID: 219
		public bool LastCharacterIsEscaped;
	}
}
