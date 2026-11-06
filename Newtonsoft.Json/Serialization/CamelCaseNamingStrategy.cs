using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000071 RID: 113
	public class CamelCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x060005F6 RID: 1526 RVA: 0x00018AED File Offset: 0x00016CED
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00018B03 File Offset: 0x00016D03
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
			: this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00018B14 File Offset: 0x00016D14
		public CamelCaseNamingStrategy()
		{
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00018B1C File Offset: 0x00016D1C
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToCamelCase(name);
		}
	}
}
