using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D8 RID: 216
	[NullableContext(1)]
	[Nullable(0)]
	internal class QueryScanFilter : PathFilter
	{
		// Token: 0x06000C0D RID: 3085 RVA: 0x0002FDD3 File Offset: 0x0002DFD3
		public QueryScanFilter(QueryExpression expression)
		{
			this.Expression = expression;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0002FDE2 File Offset: 0x0002DFE2
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			foreach (JToken jtoken in current)
			{
				JContainer jcontainer = jtoken as JContainer;
				if (jcontainer != null)
				{
					foreach (JToken jtoken2 in jcontainer.DescendantsAndSelf())
					{
						if (this.Expression.IsMatch(root, jtoken2, settings))
						{
							yield return jtoken2;
						}
					}
					IEnumerator<JToken> enumerator2 = null;
				}
				else if (this.Expression.IsMatch(root, jtoken, settings))
				{
					yield return jtoken;
				}
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040003E2 RID: 994
		internal QueryExpression Expression;
	}
}
