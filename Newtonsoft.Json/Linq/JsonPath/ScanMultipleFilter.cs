using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000DB RID: 219
	[NullableContext(1)]
	[Nullable(0)]
	internal class ScanMultipleFilter : PathFilter
	{
		// Token: 0x06000C14 RID: 3092 RVA: 0x0002FE4D File Offset: 0x0002E04D
		public ScanMultipleFilter(List<string> names)
		{
			this._names = names;
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0002FE5C File Offset: 0x0002E05C
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			foreach (JToken c in current)
			{
				JToken value = c;
				for (;;)
				{
					JContainer jcontainer = value as JContainer;
					value = PathFilter.GetNextScanValue(c, jcontainer, value);
					if (value == null)
					{
						break;
					}
					JProperty property = value as JProperty;
					if (property != null)
					{
						foreach (string text in this._names)
						{
							if (property.Name == text)
							{
								yield return property.Value;
							}
						}
						List<string>.Enumerator enumerator2 = default(List<string>.Enumerator);
					}
					property = null;
				}
				value = null;
				c = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040003E5 RID: 997
		private List<string> _names;
	}
}
