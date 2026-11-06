using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000056 RID: 86
	[NullableContext(1)]
	[Nullable(0)]
	internal class NoThrowExpressionVisitor : ExpressionVisitor
	{
		// Token: 0x06000503 RID: 1283 RVA: 0x00014EB3 File Offset: 0x000130B3
		protected override Expression VisitConditional(ConditionalExpression node)
		{
			if (node.IfFalse.NodeType == ExpressionType.Throw)
			{
				return Expression.Condition(node.Test, node.IfTrue, Expression.Constant(NoThrowExpressionVisitor.ErrorResult));
			}
			return base.VisitConditional(node);
		}

		// Token: 0x040001DC RID: 476
		internal static readonly object ErrorResult = new object();
	}
}
