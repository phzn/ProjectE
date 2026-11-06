using System;
using UnityEngine.Localization.SmartFormat.Utilities;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x02000074 RID: 116
	public class CustomPluralRuleProvider : IFormatProvider
	{
		// Token: 0x06000315 RID: 789 RVA: 0x0000BE9B File Offset: 0x0000A09B
		public CustomPluralRuleProvider(PluralRules.PluralRuleDelegate pluralRule)
		{
			this._pluralRule = pluralRule;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000BEAA File Offset: 0x0000A0AA
		public object GetFormat(Type formatType)
		{
			if (!(formatType == typeof(CustomPluralRuleProvider)))
			{
				return null;
			}
			return this;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000BEC1 File Offset: 0x0000A0C1
		public PluralRules.PluralRuleDelegate GetPluralRule()
		{
			return this._pluralRule;
		}

		// Token: 0x040000C8 RID: 200
		private readonly PluralRules.PluralRuleDelegate _pluralRule;
	}
}
