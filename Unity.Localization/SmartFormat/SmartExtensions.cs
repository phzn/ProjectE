using System;
using System.IO;
using System.Text;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.Core.Output;

namespace UnityEngine.Localization.SmartFormat
{
	// Token: 0x02000038 RID: 56
	public static class SmartExtensions
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000758C File Offset: 0x0000578C
		public static void AppendSmart(this StringBuilder sb, string format, params object[] args)
		{
			StringOutput stringOutput = new StringOutput(sb);
			Smart.Default.FormatInto(stringOutput, format, args);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000075AD File Offset: 0x000057AD
		public static void AppendLineSmart(this StringBuilder sb, string format, params object[] args)
		{
			sb.AppendSmart(format, args);
			sb.AppendLine();
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000075C0 File Offset: 0x000057C0
		public static void WriteSmart(this TextWriter writer, string format, params object[] args)
		{
			TextWriterOutput textWriterOutput = new TextWriterOutput(writer);
			Smart.Default.FormatInto(textWriterOutput, format, args);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000075E1 File Offset: 0x000057E1
		public static void WriteLineSmart(this TextWriter writer, string format, params object[] args)
		{
			writer.WriteSmart(format, args);
			writer.WriteLine();
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000075F1 File Offset: 0x000057F1
		public static string FormatSmart(this string format, params object[] args)
		{
			return Smart.Format(format, args);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x000075FA File Offset: 0x000057FA
		public static string FormatSmart(this string format, ref FormatCache cache, params object[] args)
		{
			return Smart.Default.FormatWithCache(ref cache, format, args);
		}
	}
}
