using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200009F RID: 159
	public class SnakeCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x06000822 RID: 2082 RVA: 0x00022FDA File Offset: 0x000211DA
		public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00022FF0 File Offset: 0x000211F0
		public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
			: this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00023001 File Offset: 0x00021201
		public SnakeCaseNamingStrategy()
		{
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00023009 File Offset: 0x00021209
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToSnakeCase(name);
		}
	}
}
