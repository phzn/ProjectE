using System;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Pool;

namespace UnityEngine.Localization.SmartFormat
{
	// Token: 0x0200003E RID: 62
	internal static class FormattingInfoPool
	{
		// Token: 0x0600024B RID: 587 RVA: 0x000083C1 File Offset: 0x000065C1
		public static FormattingInfo Get(FormatDetails formatDetails, Format format, object currentValue)
		{
			FormattingInfo formattingInfo = FormattingInfoPool.s_Pool.Get();
			formattingInfo.Init(formatDetails, format, currentValue);
			return formattingInfo;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x000083D6 File Offset: 0x000065D6
		public static FormattingInfo Get(FormattingInfo parent, FormatDetails formatDetails, Format format, object currentValue)
		{
			FormattingInfo formattingInfo = FormattingInfoPool.s_Pool.Get();
			formattingInfo.Init(parent, formatDetails, format, currentValue);
			return formattingInfo;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x000083EC File Offset: 0x000065EC
		public static FormattingInfo Get(FormattingInfo parent, FormatDetails formatDetails, Placeholder placeholder, object currentValue)
		{
			FormattingInfo formattingInfo = FormattingInfoPool.s_Pool.Get();
			formattingInfo.Init(parent, formatDetails, placeholder, currentValue);
			return formattingInfo;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00008402 File Offset: 0x00006602
		public static void Release(FormattingInfo toRelease)
		{
			FormattingInfoPool.s_Pool.Release(toRelease);
		}

		// Token: 0x04000088 RID: 136
		internal static readonly ObjectPool<FormattingInfo> s_Pool = new ObjectPool<FormattingInfo>(() => new FormattingInfo(), null, delegate(FormattingInfo fi)
		{
			fi.ReleaseToPool();
		}, null, true, 10, 10000);
	}
}
