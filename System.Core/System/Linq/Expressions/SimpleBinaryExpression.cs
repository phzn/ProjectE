using System;

namespace System.Linq.Expressions
{
	// Token: 0x0200020E RID: 526
	internal class SimpleBinaryExpression : BinaryExpression
	{
		// Token: 0x06000CEA RID: 3306 RVA: 0x0002D176 File Offset: 0x0002B376
		internal SimpleBinaryExpression(ExpressionType nodeType, Expression left, Expression right, Type type)
			: base(left, right)
		{
			this.NodeType = nodeType;
			this.Type = type;
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x0002D18F File Offset: 0x0002B38F
		public sealed override ExpressionType NodeType { get; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0002D197 File Offset: 0x0002B397
		public sealed override Type Type { get; }
	}
}
