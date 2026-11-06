using System;

namespace System.Linq.Expressions
{
	// Token: 0x02000293 RID: 659
	internal class TypedParameterExpression : ParameterExpression
	{
		// Token: 0x0600130F RID: 4879 RVA: 0x0003C739 File Offset: 0x0003A939
		internal TypedParameterExpression(Type type, string name)
			: base(name)
		{
			this.Type = type;
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001310 RID: 4880 RVA: 0x0003C749 File Offset: 0x0003A949
		public sealed override Type Type { get; }
	}
}
