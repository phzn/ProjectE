using System;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Localization.SmartFormat.Core.Settings;
using UnityEngine.Pool;

namespace UnityEngine.Localization.SmartFormat
{
	// Token: 0x0200003D RID: 61
	internal static class FormatItemPool
	{
		// Token: 0x0600023B RID: 571 RVA: 0x000080F2 File Offset: 0x000062F2
		public static LiteralText GetLiteralText(SmartSettings smartSettings, FormatItem parent, int startIndex)
		{
			LiteralText literalText = FormatItemPool.s_LiteralTextPool.Get();
			literalText.Init(smartSettings, parent, startIndex);
			return literalText;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00008107 File Offset: 0x00006307
		public static LiteralText GetLiteralText(SmartSettings smartSettings, FormatItem parent, int startIndex, int endIndex)
		{
			LiteralText literalText = FormatItemPool.s_LiteralTextPool.Get();
			literalText.Init(smartSettings, parent, startIndex, endIndex);
			return literalText;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000811D File Offset: 0x0000631D
		public static LiteralText GetLiteralText(SmartSettings smartSettings, FormatItem parent, string baseString, int startIndex, int endIndex)
		{
			LiteralText literalText = FormatItemPool.s_LiteralTextPool.Get();
			literalText.Init(smartSettings, parent, baseString, startIndex, endIndex);
			return literalText;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00008135 File Offset: 0x00006335
		public static Format GetFormat(SmartSettings smartSettings, string baseString)
		{
			Format format = FormatItemPool.s_FormatPool.Get();
			format.Init(smartSettings, null, baseString, 0, baseString.Length);
			return format;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00008151 File Offset: 0x00006351
		public static Format GetFormat(SmartSettings smartSettings, string baseString, int startIndex, int endIndex)
		{
			Format format = FormatItemPool.s_FormatPool.Get();
			format.Init(smartSettings, null, baseString, startIndex, endIndex);
			return format;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00008168 File Offset: 0x00006368
		public static Format GetFormat(SmartSettings smartSettings, string baseString, int startIndex, int endIndex, bool nested)
		{
			Format format = FormatItemPool.s_FormatPool.Get();
			format.Init(smartSettings, null, baseString, startIndex, endIndex);
			format.HasNested = nested;
			return format;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00008187 File Offset: 0x00006387
		public static Format GetFormat(SmartSettings smartSettings, Placeholder parent, int startIndex)
		{
			Format format = FormatItemPool.s_FormatPool.Get();
			format.Init(smartSettings, parent, startIndex);
			format.parent = parent;
			return format;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x000081A3 File Offset: 0x000063A3
		public static Placeholder GetPlaceholder(SmartSettings smartSettings, Format parent, int startIndex, int nestedDepth)
		{
			Placeholder placeholder = FormatItemPool.s_PlaceholderPool.Get();
			placeholder.Init(smartSettings, parent, startIndex);
			placeholder.NestedDepth = nestedDepth;
			placeholder.FormatterName = "";
			placeholder.FormatterOptions = "";
			return placeholder;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000081D5 File Offset: 0x000063D5
		public static Placeholder GetPlaceholder(SmartSettings smartSettings, Format parent, int startIndex, int nestedDepth, Format itemFormat, int endIndex)
		{
			Placeholder placeholder = FormatItemPool.s_PlaceholderPool.Get();
			placeholder.Init(smartSettings, parent, startIndex, endIndex);
			placeholder.Format = itemFormat;
			placeholder.NestedDepth = nestedDepth;
			placeholder.FormatterName = "";
			placeholder.FormatterOptions = "";
			return placeholder;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00008211 File Offset: 0x00006411
		public static Selector GetSelector(SmartSettings smartSettings, FormatItem parent, string baseString, int startIndex, int endIndex, int operatorStart, int selectorIndex)
		{
			Selector selector = FormatItemPool.s_SelectorPool.Get();
			selector.Init(smartSettings, parent, baseString, startIndex, endIndex);
			selector.operatorStart = operatorStart;
			selector.SelectorIndex = selectorIndex;
			return selector;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00008239 File Offset: 0x00006439
		public static void ReleaseLiteralText(LiteralText literal)
		{
			FormatItemPool.s_LiteralTextPool.Release(literal);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00008246 File Offset: 0x00006446
		public static void ReleaseFormat(Format format)
		{
			FormatItemPool.s_FormatPool.Release(format);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00008253 File Offset: 0x00006453
		public static void ReleasePlaceholder(Placeholder placeholder)
		{
			FormatItemPool.s_PlaceholderPool.Release(placeholder);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00008260 File Offset: 0x00006460
		public static void ReleaseSelector(Selector selector)
		{
			FormatItemPool.s_SelectorPool.Release(selector);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00008270 File Offset: 0x00006470
		public static void Release(FormatItem format)
		{
			LiteralText literalText = format as LiteralText;
			if (literalText != null)
			{
				FormatItemPool.ReleaseLiteralText(literalText);
				return;
			}
			Format format2 = format as Format;
			if (format2 != null)
			{
				FormatItemPool.ReleaseFormat(format2);
				return;
			}
			Placeholder placeholder = format as Placeholder;
			if (placeholder != null)
			{
				FormatItemPool.ReleasePlaceholder(placeholder);
				return;
			}
			Selector selector = format as Selector;
			if (selector == null)
			{
				string text = "Unhandled type ";
				Type type = format.GetType();
				Debug.LogError(text + ((type != null) ? type.ToString() : null));
				return;
			}
			FormatItemPool.ReleaseSelector(selector);
		}

		// Token: 0x04000084 RID: 132
		internal static readonly ObjectPool<LiteralText> s_LiteralTextPool = new ObjectPool<LiteralText>(() => new LiteralText(), null, delegate(LiteralText lt)
		{
			lt.Clear();
		}, null, true, 10, 10000);

		// Token: 0x04000085 RID: 133
		internal static readonly ObjectPool<Format> s_FormatPool = new ObjectPool<Format>(() => new Format(), null, delegate(Format f)
		{
			f.ReleaseToPool();
		}, null, true, 10, 10000);

		// Token: 0x04000086 RID: 134
		internal static readonly ObjectPool<Placeholder> s_PlaceholderPool = new ObjectPool<Placeholder>(() => new Placeholder(), null, delegate(Placeholder p)
		{
			p.ReleaseToPool();
		}, null, true, 10, 10000);

		// Token: 0x04000087 RID: 135
		internal static readonly ObjectPool<Selector> s_SelectorPool = new ObjectPool<Selector>(() => new Selector(), null, delegate(Selector s)
		{
			s.Clear();
		}, null, true, 10, 10000);
	}
}
