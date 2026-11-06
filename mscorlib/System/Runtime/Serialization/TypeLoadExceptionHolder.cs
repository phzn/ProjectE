using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000668 RID: 1640
	internal class TypeLoadExceptionHolder
	{
		// Token: 0x06003D52 RID: 15698 RVA: 0x000D470B File Offset: 0x000D290B
		internal TypeLoadExceptionHolder(string typeName)
		{
			this.m_typeName = typeName;
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06003D53 RID: 15699 RVA: 0x000D471A File Offset: 0x000D291A
		internal string TypeName
		{
			get
			{
				return this.m_typeName;
			}
		}

		// Token: 0x04002778 RID: 10104
		private string m_typeName;
	}
}
