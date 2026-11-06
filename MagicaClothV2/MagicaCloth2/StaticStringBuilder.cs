using System;
using System.Text;

namespace MagicaCloth2
{
	// Token: 0x02000109 RID: 265
	public class StaticStringBuilder
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0002D76B File Offset: 0x0002B96B
		public static StringBuilder Instance
		{
			get
			{
				return StaticStringBuilder.stringBuilder;
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0002D772 File Offset: 0x0002B972
		public static void Clear()
		{
			StaticStringBuilder.stringBuilder.Length = 0;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0002D780 File Offset: 0x0002B980
		public static StringBuilder Append(params object[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				StaticStringBuilder.stringBuilder.Append(args[i]);
			}
			return StaticStringBuilder.stringBuilder;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0002D7B0 File Offset: 0x0002B9B0
		public static StringBuilder AppendLine(params object[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				StaticStringBuilder.stringBuilder.Append(args[i]);
			}
			StaticStringBuilder.stringBuilder.Append("\n");
			return StaticStringBuilder.stringBuilder;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0002D7EE File Offset: 0x0002B9EE
		public static StringBuilder AppendLine()
		{
			StaticStringBuilder.stringBuilder.Append("\n");
			return StaticStringBuilder.stringBuilder;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0002D808 File Offset: 0x0002BA08
		public static string AppendToString(params object[] args)
		{
			StaticStringBuilder.stringBuilder.Length = 0;
			for (int i = 0; i < args.Length; i++)
			{
				StaticStringBuilder.stringBuilder.Append(args[i]);
			}
			return StaticStringBuilder.stringBuilder.ToString();
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0002D846 File Offset: 0x0002BA46
		public static string ToString()
		{
			return StaticStringBuilder.stringBuilder.ToString();
		}

		// Token: 0x0400089A RID: 2202
		private static StringBuilder stringBuilder = new StringBuilder(1024);
	}
}
