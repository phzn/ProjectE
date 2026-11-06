using System;

namespace Unity.Serialization.Json
{
	// Token: 0x02000043 RID: 67
	internal struct Token
	{
		// Token: 0x0600016E RID: 366 RVA: 0x00006288 File Offset: 0x00004488
		public override string ToString()
		{
			return string.Format("Type=[{0}] Range=[{1}..{2}] Parent=[{3}]", new object[] { this.Type, this.Start, this.End, this.Parent });
		}

		// Token: 0x0400009E RID: 158
		public TokenType Type;

		// Token: 0x0400009F RID: 159
		public int Parent;

		// Token: 0x040000A0 RID: 160
		public int Start;

		// Token: 0x040000A1 RID: 161
		public int End;
	}
}
