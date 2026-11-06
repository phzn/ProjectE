using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D5 RID: 213
	[NullableContext(1)]
	[Nullable(0)]
	internal class CompositeExpression : QueryExpression
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x0002F86B File Offset: 0x0002DA6B
		// (set) Token: 0x06000C01 RID: 3073 RVA: 0x0002F873 File Offset: 0x0002DA73
		public List<QueryExpression> Expressions { get; set; }

		// Token: 0x06000C02 RID: 3074 RVA: 0x0002F87C File Offset: 0x0002DA7C
		public CompositeExpression(QueryOperator @operator)
			: base(@operator)
		{
			this.Expressions = new List<QueryExpression>();
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0002F890 File Offset: 0x0002DA90
		public override bool IsMatch(JToken root, JToken t, [Nullable(2)] JsonSelectSettings settings)
		{
			QueryOperator @operator = this.Operator;
			if (@operator == QueryOperator.And)
			{
				using (List<QueryExpression>.Enumerator enumerator = this.Expressions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsMatch(root, t, settings))
						{
							return false;
						}
					}
				}
				return true;
			}
			if (@operator != QueryOperator.Or)
			{
				throw new ArgumentOutOfRangeException();
			}
			using (List<QueryExpression>.Enumerator enumerator = this.Expressions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsMatch(root, t, settings))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
