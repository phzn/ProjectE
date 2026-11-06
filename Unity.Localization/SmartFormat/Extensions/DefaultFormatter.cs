using System;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x0200006C RID: 108
	[Serializable]
	public class DefaultFormatter : FormatterBase
	{
		// Token: 0x060002DC RID: 732 RVA: 0x0000AE4B File Offset: 0x0000904B
		public DefaultFormatter()
		{
			base.Names = this.DefaultNames;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000AE5F File Offset: 0x0000905F
		public override string[] DefaultNames
		{
			get
			{
				return new string[] { "default", "d", "" };
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000AE80 File Offset: 0x00009080
		public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			Format format = formattingInfo.Format;
			object obj = formattingInfo.CurrentValue;
			if (format != null && format.HasNested)
			{
				formattingInfo.Write(format, obj);
				return true;
			}
			if (obj == null)
			{
				obj = "";
			}
			IFormatProvider provider = formattingInfo.FormatDetails.Provider;
			string text2;
			if (provider != null)
			{
				ICustomFormatter customFormatter = provider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
				if (customFormatter != null)
				{
					string text = ((format != null) ? format.GetLiteralText() : null);
					text2 = customFormatter.Format(text, obj, provider);
					goto IL_00A0;
				}
			}
			IFormattable formattable = obj as IFormattable;
			if (formattable != null)
			{
				string text3 = ((format != null) ? format.ToString() : null);
				text2 = formattable.ToString(text3, provider);
			}
			else
			{
				text2 = obj.ToString();
			}
			IL_00A0:
			if (formattingInfo.Alignment > 0)
			{
				int num = formattingInfo.Alignment - text2.Length;
				if (num > 0)
				{
					formattingInfo.Write(new string(' ', num));
				}
			}
			formattingInfo.Write(text2);
			if (formattingInfo.Alignment < 0)
			{
				int num2 = -formattingInfo.Alignment - text2.Length;
				if (num2 > 0)
				{
					formattingInfo.Write(new string(' ', num2));
				}
			}
			return true;
		}
	}
}
