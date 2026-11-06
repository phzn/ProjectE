using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D7 RID: 215
	[NullableContext(1)]
	[Nullable(0)]
	internal class QueryFilter : PathFilter
	{
		// Token: 0x06000C0B RID: 3083 RVA: 0x0002FD9F File Offset: 0x0002DF9F
		public QueryFilter(QueryExpression expression)
		{
			this.Expression = expression;
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0002FDAE File Offset: 0x0002DFAE
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			foreach (JToken jtoken in current)
			{
				foreach (JToken jtoken2 in ((IEnumerable<JToken>)jtoken))
				{
					if (this.Expression.IsMatch(root, jtoken2, settings))
					{
						yield return jtoken2;
					}
				}
				IEnumerator<JToken> enumerator2 = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040003E1 RID: 993
		internal QueryExpression Expression;
	}
}
