using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x02000077 RID: 119
	[Serializable]
	public class TemplateFormatter : FormatterBase
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000C2CC File Offset: 0x0000A4CC
		private IDictionary<string, Format> Templates
		{
			get
			{
				if (this.m_TemplatesDict == null)
				{
					IEqualityComparer<string> caseSensitivityComparer = this.Formatter.Settings.GetCaseSensitivityComparer();
					this.m_TemplatesDict = new Dictionary<string, Format>(caseSensitivityComparer);
					foreach (TemplateFormatter.Template template in this.m_Templates)
					{
						if (!string.IsNullOrEmpty(template.name))
						{
							try
							{
								this.m_TemplatesDict[template.name] = this.Formatter.Parser.ParseFormat(template.text, this.Formatter.GetNotEmptyFormatterExtensionNames());
							}
							catch (Exception ex)
							{
								Debug.LogException(ex);
							}
						}
					}
				}
				return this.m_TemplatesDict;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000C39C File Offset: 0x0000A59C
		// (set) Token: 0x06000327 RID: 807 RVA: 0x0000C3B2 File Offset: 0x0000A5B2
		public SmartFormatter Formatter
		{
			get
			{
				return this.m_Formatter ?? LocalizationSettings.StringDatabase.SmartFormatter;
			}
			set
			{
				this.m_Formatter = value;
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000C3BB File Offset: 0x0000A5BB
		public TemplateFormatter()
		{
			base.Names = this.DefaultNames;
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000C3DA File Offset: 0x0000A5DA
		public override string[] DefaultNames
		{
			get
			{
				return new string[] { "template", "t" };
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			string text = formattingInfo.FormatterOptions;
			if (text == string.Empty)
			{
				if (formattingInfo.Format.HasNested)
				{
					return false;
				}
				text = formattingInfo.Format.RawText;
			}
			Format format;
			if (this.Templates.TryGetValue(text, out format))
			{
				formattingInfo.Write(format, formattingInfo.CurrentValue);
				return true;
			}
			if (base.Names.Contains(formattingInfo.Placeholder.FormatterName))
			{
				throw new FormatException(string.Concat(new string[]
				{
					"Formatter '",
					formattingInfo.Placeholder.FormatterName,
					"' found no registered template named '",
					text,
					"'"
				}));
			}
			return false;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000C4A4 File Offset: 0x0000A6A4
		public void Register(string templateName, string template)
		{
			Format format = this.Formatter.Parser.ParseFormat(template, this.Formatter.GetNotEmptyFormatterExtensionNames());
			this.Templates.Add(templateName, format);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000C4DB File Offset: 0x0000A6DB
		public bool Remove(string templateName)
		{
			return this.Templates.Remove(templateName);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000C4E9 File Offset: 0x0000A6E9
		public override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();
			this.m_TemplatesDict = null;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000C4F8 File Offset: 0x0000A6F8
		public void Clear()
		{
			this.Templates.Clear();
		}

		// Token: 0x040000CE RID: 206
		[SerializeField]
		private List<TemplateFormatter.Template> m_Templates = new List<TemplateFormatter.Template>();

		// Token: 0x040000CF RID: 207
		private IDictionary<string, Format> m_TemplatesDict;

		// Token: 0x040000D0 RID: 208
		[NonSerialized]
		private SmartFormatter m_Formatter;

		// Token: 0x0200013C RID: 316
		[Serializable]
		private class Template
		{
			// Token: 0x17000185 RID: 389
			// (get) Token: 0x060007A6 RID: 1958 RVA: 0x00018EF6 File Offset: 0x000170F6
			// (set) Token: 0x060007A7 RID: 1959 RVA: 0x00018EFE File Offset: 0x000170FE
			public Format Format { get; set; }

			// Token: 0x040002CF RID: 719
			public string name;

			// Token: 0x040002D0 RID: 720
			public string text;
		}
	}
}
