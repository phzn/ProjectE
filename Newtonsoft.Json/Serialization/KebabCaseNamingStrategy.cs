using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000097 RID: 151
	public class KebabCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x060007FC RID: 2044 RVA: 0x00022B69 File Offset: 0x00020D69
		public KebabCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00022B7F File Offset: 0x00020D7F
		public KebabCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
			: this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00022B90 File Offset: 0x00020D90
		public KebabCaseNamingStrategy()
		{
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00022B98 File Offset: 0x00020D98
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToKebabCase(name);
		}
	}
}
