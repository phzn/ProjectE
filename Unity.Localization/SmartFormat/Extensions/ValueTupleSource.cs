using System;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.Utilities;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x02000079 RID: 121
	[Serializable]
	public class ValueTupleSource : ISource
	{
		// Token: 0x06000337 RID: 823 RVA: 0x0000C72C File Offset: 0x0000A92C
		public ValueTupleSource(SmartFormatter formatter)
		{
			this.m_Formatter = formatter;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000C73C File Offset: 0x0000A93C
		public bool TryEvaluateSelector(ISelectorInfo selectorInfo)
		{
			FormattingInfo formattingInfo = selectorInfo as FormattingInfo;
			if (formattingInfo == null)
			{
				return false;
			}
			if (formattingInfo.CurrentValue == null || !formattingInfo.CurrentValue.IsValueTuple())
			{
				return false;
			}
			object currentValue = formattingInfo.CurrentValue;
			foreach (object obj in formattingInfo.CurrentValue.GetValueTupleItemObjectsFlattened())
			{
				if (this.m_Formatter == null)
				{
					SmartFormatter smartFormatter = LocalizationSettings.StringDatabase.SmartFormatter;
				}
				foreach (ISource source in this.m_Formatter.SourceExtensions)
				{
					formattingInfo.CurrentValue = obj;
					if (source.TryEvaluateSelector(formattingInfo))
					{
						formattingInfo.CurrentValue = currentValue;
						return true;
					}
				}
			}
			formattingInfo.CurrentValue = currentValue;
			return false;
		}

		// Token: 0x040000D3 RID: 211
		private SmartFormatter m_Formatter;
	}
}
