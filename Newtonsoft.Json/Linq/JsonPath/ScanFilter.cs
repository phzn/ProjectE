using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000DA RID: 218
	[NullableContext(2)]
	[Nullable(0)]
	internal class ScanFilter : PathFilter
	{
		// Token: 0x06000C12 RID: 3090 RVA: 0x0002FE27 File Offset: 0x0002E027
		public ScanFilter(string name)
		{
			this.Name = name;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0002FE36 File Offset: 0x0002E036
		[NullableContext(1)]
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			foreach (JToken c in current)
			{
				if (this.Name == null)
				{
					yield return c;
				}
				JToken value = c;
				for (;;)
				{
					JContainer jcontainer = value as JContainer;
					value = PathFilter.GetNextScanValue(c, jcontainer, value);
					if (value == null)
					{
						break;
					}
					JProperty jproperty = value as JProperty;
					if (jproperty != null)
					{
						if (jproperty.Name == this.Name)
						{
							yield return jproperty.Value;
						}
					}
					else if (this.Name == null)
					{
						yield return value;
					}
				}
				value = null;
				c = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040003E4 RID: 996
		internal string Name;
	}
}
