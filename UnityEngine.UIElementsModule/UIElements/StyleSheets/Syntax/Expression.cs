using System;

namespace UnityEngine.UIElements.StyleSheets.Syntax
{
	// Token: 0x020004A8 RID: 1192
	internal class Expression
	{
		// Token: 0x06002529 RID: 9513 RVA: 0x0009CE35 File Offset: 0x0009B035
		public Expression(ExpressionType type)
		{
			this.type = type;
			this.combinator = ExpressionCombinator.None;
			this.multiplier = new ExpressionMultiplier(ExpressionMultiplierType.None);
			this.subExpressions = null;
			this.keyword = null;
		}

		// Token: 0x040011E8 RID: 4584
		public ExpressionType type;

		// Token: 0x040011E9 RID: 4585
		public ExpressionMultiplier multiplier;

		// Token: 0x040011EA RID: 4586
		public DataType dataType;

		// Token: 0x040011EB RID: 4587
		public ExpressionCombinator combinator;

		// Token: 0x040011EC RID: 4588
		public Expression[] subExpressions;

		// Token: 0x040011ED RID: 4589
		public string keyword;
	}
}
