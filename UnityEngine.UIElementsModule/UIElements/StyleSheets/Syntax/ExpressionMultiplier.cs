using System;

namespace UnityEngine.UIElements.StyleSheets.Syntax
{
	// Token: 0x020004AD RID: 1197
	internal struct ExpressionMultiplier
	{
		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x0600252A RID: 9514 RVA: 0x0009CE68 File Offset: 0x0009B068
		// (set) Token: 0x0600252B RID: 9515 RVA: 0x0009CE80 File Offset: 0x0009B080
		public ExpressionMultiplierType type
		{
			get
			{
				return this.m_Type;
			}
			set
			{
				this.SetType(value);
			}
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x0009CE8C File Offset: 0x0009B08C
		public ExpressionMultiplier(ExpressionMultiplierType type = ExpressionMultiplierType.None)
		{
			this.m_Type = type;
			this.min = (this.max = 1);
			this.SetType(type);
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x0009CEBC File Offset: 0x0009B0BC
		private void SetType(ExpressionMultiplierType value)
		{
			this.m_Type = value;
			switch (value)
			{
			case ExpressionMultiplierType.ZeroOrMore:
				this.min = 0;
				this.max = 100;
				return;
			case ExpressionMultiplierType.OneOrMore:
			case ExpressionMultiplierType.OneOrMoreComma:
			case ExpressionMultiplierType.GroupAtLeastOne:
				this.min = 1;
				this.max = 100;
				return;
			case ExpressionMultiplierType.ZeroOrOne:
				this.min = 0;
				this.max = 1;
				return;
			}
			this.min = (this.max = 1);
		}

		// Token: 0x0400120E RID: 4622
		public const int Infinity = 100;

		// Token: 0x0400120F RID: 4623
		private ExpressionMultiplierType m_Type;

		// Token: 0x04001210 RID: 4624
		public int min;

		// Token: 0x04001211 RID: 4625
		public int max;
	}
}
