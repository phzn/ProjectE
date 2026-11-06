using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000CD RID: 205
	[NullableContext(1)]
	[Nullable(0)]
	internal class ArrayMultipleIndexFilter : PathFilter
	{
		// Token: 0x06000BD3 RID: 3027 RVA: 0x0002E4BC File Offset: 0x0002C6BC
		public ArrayMultipleIndexFilter(List<int> indexes)
		{
			this.Indexes = indexes;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0002E4CB File Offset: 0x0002C6CB
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			foreach (JToken t in current)
			{
				foreach (int num in this.Indexes)
				{
					JToken tokenIndex = PathFilter.GetTokenIndex(t, settings, num);
					if (tokenIndex != null)
					{
						yield return tokenIndex;
					}
				}
				List<int>.Enumerator enumerator2 = default(List<int>.Enumerator);
				t = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040003C5 RID: 965
		internal List<int> Indexes;
	}
}
