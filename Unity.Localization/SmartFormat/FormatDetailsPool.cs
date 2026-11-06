using System;
using System.Collections.Generic;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.Core.Output;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Pool;

namespace UnityEngine.Localization.SmartFormat
{
	// Token: 0x0200003C RID: 60
	internal static class FormatDetailsPool
	{
		// Token: 0x06000237 RID: 567 RVA: 0x00008077 File Offset: 0x00006277
		public static FormatDetails Get(SmartFormatter formatter, Format originalFormat, IList<object> originalArgs, FormatCache formatCache, IFormatProvider provider, IOutput output)
		{
			FormatDetails formatDetails = FormatDetailsPool.s_Pool.Get();
			formatDetails.Init(formatter, originalFormat, originalArgs, formatCache, provider, output);
			return formatDetails;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00008091 File Offset: 0x00006291
		public static PooledObject<FormatDetails> Get(SmartFormatter formatter, Format originalFormat, object[] originalArgs, FormatCache formatCache, IFormatProvider provider, IOutput output, out FormatDetails value)
		{
			PooledObject<FormatDetails> pooledObject = FormatDetailsPool.s_Pool.Get(out value);
			value.Init(formatter, originalFormat, originalArgs, formatCache, provider, output);
			return pooledObject;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x000080AF File Offset: 0x000062AF
		public static void Release(FormatDetails toRelease)
		{
			FormatDetailsPool.s_Pool.Release(toRelease);
		}

		// Token: 0x04000083 RID: 131
		internal static readonly ObjectPool<FormatDetails> s_Pool = new ObjectPool<FormatDetails>(() => new FormatDetails(), null, delegate(FormatDetails fd)
		{
			fd.Clear();
		}, null, true, 10, 10000);
	}
}
