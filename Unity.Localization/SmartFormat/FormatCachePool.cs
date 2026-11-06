using System;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Pool;

namespace UnityEngine.Localization.SmartFormat
{
	// Token: 0x0200003B RID: 59
	internal static class FormatCachePool
	{
		// Token: 0x06000233 RID: 563 RVA: 0x0000800C File Offset: 0x0000620C
		public static FormatCache Get(Format format)
		{
			FormatCache formatCache = FormatCachePool.s_Pool.Get();
			formatCache.Format = format;
			return formatCache;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000801F File Offset: 0x0000621F
		public static PooledObject<FormatCache> Get(Format format, out FormatCache value)
		{
			PooledObject<FormatCache> pooledObject = FormatCachePool.s_Pool.Get(out value);
			value.Format = format;
			return pooledObject;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00008034 File Offset: 0x00006234
		public static void Release(FormatCache toRelease)
		{
			FormatCachePool.s_Pool.Release(toRelease);
		}

		// Token: 0x04000082 RID: 130
		internal static readonly ObjectPool<FormatCache> s_Pool = new ObjectPool<FormatCache>(() => new FormatCache(), null, delegate(FormatCache fc)
		{
			FormatItemPool.ReleaseFormat(fc.Format);
			fc.Format = null;
			fc.Table = null;
			fc.CachedObjects.Clear();
			fc.VariableTriggers.Clear();
			fc.LocalVariables = null;
		}, null, true, 10, 10000);
	}
}
