using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.Core.Output;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Localization.SmartFormat.Core.Settings;

namespace UnityEngine.Localization.SmartFormat
{
	// Token: 0x02000039 RID: 57
	[Serializable]
	public class SmartFormatter : ISerializationCallbackReceiver
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600020F RID: 527 RVA: 0x0000760C File Offset: 0x0000580C
		// (remove) Token: 0x06000210 RID: 528 RVA: 0x00007644 File Offset: 0x00005844
		public event EventHandler<FormattingErrorEventArgs> OnFormattingFailure;

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00007679 File Offset: 0x00005879
		public List<ISource> SourceExtensions
		{
			get
			{
				return this.m_Sources;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000212 RID: 530 RVA: 0x00007681 File Offset: 0x00005881
		public List<IFormatter> FormatterExtensions
		{
			get
			{
				return this.m_Formatters;
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00007689 File Offset: 0x00005889
		public SmartFormatter()
		{
			this.m_Settings = new SmartSettings();
			this.m_Parser = new Parser(this.m_Settings);
			this.m_Sources = new List<ISource>();
			this.m_Formatters = new List<IFormatter>();
		}

		// Token: 0x06000214 RID: 532 RVA: 0x000076C4 File Offset: 0x000058C4
		public List<string> GetNotEmptyFormatterExtensionNames()
		{
			if (this.m_NotEmptyFormatterExtensionNames != null)
			{
				return this.m_NotEmptyFormatterExtensionNames;
			}
			this.m_NotEmptyFormatterExtensionNames = new List<string>();
			foreach (IFormatter formatter in this.FormatterExtensions)
			{
				if (((formatter != null) ? formatter.Names : null) != null)
				{
					foreach (string text in formatter.Names)
					{
						if (!string.IsNullOrEmpty(text))
						{
							this.m_NotEmptyFormatterExtensionNames.Add(text);
						}
					}
				}
			}
			return this.m_NotEmptyFormatterExtensionNames;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00007770 File Offset: 0x00005970
		public void AddExtensions(params ISource[] sourceExtensions)
		{
			this.SourceExtensions.InsertRange(0, sourceExtensions);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000777F File Offset: 0x0000597F
		public void AddExtensions(params IFormatter[] formatterExtensions)
		{
			this.m_NotEmptyFormatterExtensionNames = null;
			this.FormatterExtensions.InsertRange(0, formatterExtensions);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00007795 File Offset: 0x00005995
		public T GetSourceExtension<T>() where T : class, ISource
		{
			return this.SourceExtensions.OfType<T>().FirstOrDefault<T>();
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000077A7 File Offset: 0x000059A7
		public T GetFormatterExtension<T>() where T : class, IFormatter
		{
			return this.FormatterExtensions.OfType<T>().FirstOrDefault<T>();
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000219 RID: 537 RVA: 0x000077B9 File Offset: 0x000059B9
		// (set) Token: 0x0600021A RID: 538 RVA: 0x000077C1 File Offset: 0x000059C1
		public Parser Parser
		{
			get
			{
				return this.m_Parser;
			}
			set
			{
				this.m_Parser = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600021B RID: 539 RVA: 0x000077CA File Offset: 0x000059CA
		// (set) Token: 0x0600021C RID: 540 RVA: 0x000077D2 File Offset: 0x000059D2
		public SmartSettings Settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000077DB File Offset: 0x000059DB
		public string Format(string format, params object[] args)
		{
			return this.Format(null, args, format);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000077E6 File Offset: 0x000059E6
		public string Format(IList<object> args, string format)
		{
			return this.Format(null, args, format);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000077F1 File Offset: 0x000059F1
		public string Format(IFormatProvider provider, string format, params object[] args)
		{
			return this.Format(provider, args, format);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000077FC File Offset: 0x000059FC
		public string Format(IFormatProvider provider, IList<object> args, string format)
		{
			args = args ?? SmartFormatter.k_Empty;
			StringOutput stringOutput;
			string text;
			using (StringOutputPool.Get(format.Length + args.Count * 8, out stringOutput))
			{
				Format format2 = this.Parser.ParseFormat(format, this.GetNotEmptyFormatterExtensionNames());
				object obj = ((args.Count > 0) ? args[0] : args);
				FormatDetails formatDetails = FormatDetailsPool.Get(this, format2, args, null, provider, stringOutput);
				this.Format(formatDetails, format2, obj);
				FormatDetailsPool.Release(formatDetails);
				FormatItemPool.ReleaseFormat(format2);
				text = stringOutput.ToString();
			}
			return text;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x000078A4 File Offset: 0x00005AA4
		public void FormatInto(IOutput output, string format, params object[] args)
		{
			args = args ?? SmartFormatter.k_Empty;
			Format format2 = this.Parser.ParseFormat(format, this.GetNotEmptyFormatterExtensionNames());
			object obj = ((args.Length != 0) ? args[0] : args);
			FormatDetails formatDetails = FormatDetailsPool.Get(this, format2, args, null, null, output);
			this.Format(formatDetails, format2, obj);
			FormatDetailsPool.Release(formatDetails);
			FormatItemPool.ReleaseFormat(format2);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000078FC File Offset: 0x00005AFC
		public string FormatWithCache(ref FormatCache cache, string format, IList<object> args)
		{
			return this.FormatWithCache(ref cache, format, null, args);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00007908 File Offset: 0x00005B08
		public string FormatWithCache(ref FormatCache cache, string format, IFormatProvider formatProvider, IList<object> args)
		{
			args = args ?? SmartFormatter.k_Empty;
			StringOutput stringOutput;
			string text;
			using (StringOutputPool.Get(format.Length + args.Count * 8, out stringOutput))
			{
				if (cache == null)
				{
					cache = FormatCachePool.Get(this.Parser.ParseFormat(format, this.GetNotEmptyFormatterExtensionNames()));
				}
				object obj = ((args.Count > 0) ? args[0] : args);
				FormatDetails formatDetails = FormatDetailsPool.Get(this, cache.Format, args, cache, formatProvider, stringOutput);
				this.Format(formatDetails, cache.Format, obj);
				FormatDetailsPool.Release(formatDetails);
				text = stringOutput.ToString();
			}
			return text;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x000079C4 File Offset: 0x00005BC4
		public void FormatWithCacheInto(ref FormatCache cache, IOutput output, string format, params object[] args)
		{
			args = args ?? SmartFormatter.k_Empty;
			if (cache == null)
			{
				cache = FormatCachePool.Get(this.Parser.ParseFormat(format, this.GetNotEmptyFormatterExtensionNames()));
			}
			object obj = ((args.Length != 0) ? args[0] : args);
			FormatDetails formatDetails = FormatDetailsPool.Get(this, cache.Format, args, cache, null, output);
			this.Format(formatDetails, cache.Format, obj);
			FormatDetailsPool.Release(formatDetails);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00007A34 File Offset: 0x00005C34
		private void Format(FormatDetails formatDetails, Format format, object current)
		{
			FormattingInfo formattingInfo = FormattingInfoPool.Get(formatDetails, format, current);
			this.Format(formattingInfo);
			FormattingInfoPool.Release(formattingInfo);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00007A58 File Offset: 0x00005C58
		public virtual void Format(FormattingInfo formattingInfo)
		{
			if (formattingInfo.Format == null)
			{
				return;
			}
			this.CheckForExtensions();
			foreach (FormatItem formatItem in formattingInfo.Format.Items)
			{
				LiteralText literalText = formatItem as LiteralText;
				if (literalText != null)
				{
					formattingInfo.Write(literalText.ToString());
				}
				else
				{
					Placeholder placeholder = (Placeholder)formatItem;
					FormattingInfo formattingInfo2 = formattingInfo.CreateChild(placeholder);
					try
					{
						this.EvaluateSelectors(formattingInfo2);
					}
					catch (DataNotReadyException ex)
					{
						if (!string.IsNullOrEmpty(ex.Text))
						{
							formattingInfo.Write(ex.Text);
						}
						continue;
					}
					catch (Exception ex2)
					{
						Format format = placeholder.Format;
						int num = ((format != null) ? format.startIndex : placeholder.Selectors.Last<Selector>().endIndex);
						this.FormatError(formatItem, ex2, num, formattingInfo2);
						continue;
					}
					try
					{
						this.EvaluateFormatters(formattingInfo2);
					}
					catch (Exception ex3)
					{
						Format format2 = placeholder.Format;
						int num2 = ((format2 != null) ? format2.startIndex : placeholder.Selectors.Last<Selector>().endIndex);
						this.FormatError(formatItem, ex3, num2, formattingInfo2);
					}
				}
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00007BAC File Offset: 0x00005DAC
		private void FormatError(FormatItem errorItem, Exception innerException, int startIndex, FormattingInfo formattingInfo)
		{
			EventHandler<FormattingErrorEventArgs> onFormattingFailure = this.OnFormattingFailure;
			if (onFormattingFailure != null)
			{
				onFormattingFailure(this, new FormattingErrorEventArgs(errorItem.RawText, startIndex, this.Settings.FormatErrorAction > ErrorAction.ThrowError));
			}
			switch (this.Settings.FormatErrorAction)
			{
			case ErrorAction.ThrowError:
				throw (innerException as FormattingException) ?? new FormattingException(errorItem, innerException, startIndex);
			case ErrorAction.OutputErrorInResult:
				formattingInfo.FormatDetails.FormattingException = (innerException as FormattingException) ?? new FormattingException(errorItem, innerException, startIndex);
				formattingInfo.Write(innerException.Message);
				formattingInfo.FormatDetails.FormattingException = null;
				return;
			case ErrorAction.Ignore:
				return;
			case ErrorAction.MaintainTokens:
				formattingInfo.Write(formattingInfo.Placeholder.RawText);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00007C68 File Offset: 0x00005E68
		private void CheckForExtensions()
		{
			if (this.SourceExtensions.Count == 0)
			{
				throw new InvalidOperationException("No source extensions are available. Please add at least one source extension, such as the DefaultSource.");
			}
			if (this.FormatterExtensions.Count == 0)
			{
				throw new InvalidOperationException("No formatter extensions are available. Please add at least one formatter extension, such as the DefaultFormatter.");
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00007C9C File Offset: 0x00005E9C
		private void EvaluateSelectors(FormattingInfo formattingInfo)
		{
			if (formattingInfo.Placeholder == null)
			{
				return;
			}
			bool flag = true;
			foreach (Selector selector in formattingInfo.Placeholder.Selectors)
			{
				formattingInfo.Selector = selector;
				formattingInfo.Result = null;
				bool flag2 = this.InvokeSourceExtensions(formattingInfo);
				if (flag2)
				{
					formattingInfo.CurrentValue = formattingInfo.Result;
				}
				if (flag)
				{
					flag = false;
					FormattingInfo formattingInfo2 = formattingInfo;
					while (!flag2 && formattingInfo2.Parent != null)
					{
						formattingInfo2 = formattingInfo2.Parent;
						formattingInfo2.Selector = selector;
						formattingInfo2.Result = null;
						flag2 = this.InvokeSourceExtensions(formattingInfo2);
						if (flag2)
						{
							formattingInfo.CurrentValue = formattingInfo2.Result;
						}
					}
				}
				if (!flag2)
				{
					throw formattingInfo.FormattingException("Could not evaluate the selector \"" + selector.RawText + "\"", selector, -1);
				}
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00007D8C File Offset: 0x00005F8C
		private bool InvokeSourceExtensions(FormattingInfo formattingInfo)
		{
			using (List<ISource>.Enumerator enumerator = this.SourceExtensions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TryEvaluateSelector(formattingInfo))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00007DE8 File Offset: 0x00005FE8
		private void EvaluateFormatters(FormattingInfo formattingInfo)
		{
			if (!this.InvokeFormatterExtensions(formattingInfo))
			{
				throw formattingInfo.FormattingException("No suitable Formatter could be found", formattingInfo.Format, -1);
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00007E08 File Offset: 0x00006008
		private bool InvokeFormatterExtensions(FormattingInfo formattingInfo)
		{
			if (formattingInfo.Placeholder == null)
			{
				return false;
			}
			string formatterName = formattingInfo.Placeholder.FormatterName;
			foreach (IFormatter formatter in this.FormatterExtensions)
			{
				if (formatter.Names.Contains(formatterName) && formatter.TryEvaluateFormat(formattingInfo))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00007E88 File Offset: 0x00006088
		public void OnBeforeSerialize()
		{
			this.m_NotEmptyFormatterExtensionNames = null;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00007E91 File Offset: 0x00006091
		public void OnAfterDeserialize()
		{
			this.m_NotEmptyFormatterExtensionNames = null;
		}

		// Token: 0x0400007A RID: 122
		[SerializeReference]
		private SmartSettings m_Settings;

		// Token: 0x0400007B RID: 123
		[SerializeReference]
		private Parser m_Parser;

		// Token: 0x0400007C RID: 124
		[SerializeReference]
		private List<ISource> m_Sources;

		// Token: 0x0400007D RID: 125
		[SerializeReference]
		private List<IFormatter> m_Formatters;

		// Token: 0x0400007E RID: 126
		private List<string> m_NotEmptyFormatterExtensionNames;

		// Token: 0x0400007F RID: 127
		private static readonly object[] k_Empty = new object[1];
	}
}
