using System;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Pool;

namespace UnityEngine.Localization.SmartFormat
{
	// Token: 0x0200003F RID: 63
	internal static class ParsingErrorsPool
	{
		// Token: 0x06000250 RID: 592 RVA: 0x00008445 File Offset: 0x00006645
		public static ParsingErrors Get(Format format)
		{
			ParsingErrors parsingErrors = ParsingErrorsPool.s_Pool.Get();
			parsingErrors.Init(format);
			return parsingErrors;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00008458 File Offset: 0x00006658
		public static void Release(ParsingErrors toRelease)
		{
			ParsingErrorsPool.s_Pool.Release(toRelease);
		}

		// Token: 0x04000089 RID: 137
		internal static readonly ObjectPool<ParsingErrors> s_Pool = new ObjectPool<ParsingErrors>(() => new ParsingErrors(), null, delegate(ParsingErrors pe)
		{
			pe.Clear();
		}, null, true, 10, 10000);
	}
}
