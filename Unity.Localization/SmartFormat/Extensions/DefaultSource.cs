using System;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Formatting;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x0200006D RID: 109
	[Serializable]
	public class DefaultSource : ISource
	{
		// Token: 0x060002DF RID: 735 RVA: 0x0000AF8E File Offset: 0x0000918E
		public DefaultSource(SmartFormatter formatter)
		{
			formatter.Parser.AddOperators(",");
			formatter.Parser.AddAdditionalSelectorChars("-");
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000AFB8 File Offset: 0x000091B8
		public bool TryEvaluateSelector(ISelectorInfo selectorInfo)
		{
			object currentValue = selectorInfo.CurrentValue;
			string selectorText = selectorInfo.SelectorText;
			FormatDetails formatDetails = selectorInfo.FormatDetails;
			int num;
			if (int.TryParse(selectorText, out num))
			{
				if (selectorInfo.SelectorIndex == 0 && num < formatDetails.OriginalArgs.Count && selectorInfo.SelectorOperator == "")
				{
					selectorInfo.Result = formatDetails.OriginalArgs[num];
					return true;
				}
				if (selectorInfo.SelectorOperator == ",")
				{
					if (selectorInfo.Placeholder != null)
					{
						selectorInfo.Placeholder.Alignment = num;
					}
					selectorInfo.Result = currentValue;
					return true;
				}
			}
			return false;
		}
	}
}
