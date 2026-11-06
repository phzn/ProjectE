using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D4 RID: 212
	[NullableContext(1)]
	[Nullable(0)]
	internal abstract class QueryExpression
	{
		// Token: 0x06000BFD RID: 3069 RVA: 0x0002F851 File Offset: 0x0002DA51
		public QueryExpression(QueryOperator @operator)
		{
			this.Operator = @operator;
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0002F860 File Offset: 0x0002DA60
		public bool IsMatch(JToken root, JToken t)
		{
			return this.IsMatch(root, t, null);
		}

		// Token: 0x06000BFF RID: 3071
		public abstract bool IsMatch(JToken root, JToken t, [Nullable(2)] JsonSelectSettings settings);

		// Token: 0x040003DD RID: 989
		internal QueryOperator Operator;
	}
}
