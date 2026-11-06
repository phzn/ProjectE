using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000074 RID: 116
	public class DefaultNamingStrategy : NamingStrategy
	{
		// Token: 0x0600063F RID: 1599 RVA: 0x0001AA7F File Offset: 0x00018C7F
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return name;
		}
	}
}
