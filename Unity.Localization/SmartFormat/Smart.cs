using System;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Extensions;

namespace UnityEngine.Localization.SmartFormat
{
	// Token: 0x02000037 RID: 55
	public static class Smart
	{
		// Token: 0x06000200 RID: 512 RVA: 0x00007456 File Offset: 0x00005656
		public static string Format(string format, params object[] args)
		{
			return Smart.Default.Format(format, args);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00007464 File Offset: 0x00005664
		public static string Format(IFormatProvider provider, string format, params object[] args)
		{
			return Smart.Default.Format(provider, format, args);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00007473 File Offset: 0x00005673
		public static string Format(string format, object arg0, object arg1, object arg2)
		{
			return Smart.Format(format, new object[] { arg0, arg1, arg2 });
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000748D File Offset: 0x0000568D
		public static string Format(string format, object arg0, object arg1)
		{
			return Smart.Format(format, new object[] { arg0, arg1 });
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000074A3 File Offset: 0x000056A3
		public static string Format(string format, object arg0)
		{
			return Smart.Default.Format(format, new object[] { arg0 });
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000205 RID: 517 RVA: 0x000074BA File Offset: 0x000056BA
		// (set) Token: 0x06000206 RID: 518 RVA: 0x000074C1 File Offset: 0x000056C1
		public static SmartFormatter Default { get; set; } = Smart.CreateDefaultSmartFormat();

		// Token: 0x06000207 RID: 519 RVA: 0x000074CC File Offset: 0x000056CC
		public static SmartFormatter CreateDefaultSmartFormat()
		{
			SmartFormatter smartFormatter = new SmartFormatter();
			ListFormatter listFormatter = new ListFormatter(smartFormatter);
			smartFormatter.AddExtensions(new ISource[]
			{
				listFormatter,
				new PersistentVariablesSource(smartFormatter),
				new DictionarySource(smartFormatter),
				new ValueTupleSource(smartFormatter),
				new XmlSource(smartFormatter),
				new ReflectionSource(smartFormatter),
				new DefaultSource(smartFormatter)
			});
			smartFormatter.AddExtensions(new IFormatter[]
			{
				listFormatter,
				new PluralLocalizationFormatter(),
				new ConditionalFormatter(),
				new TimeFormatter(),
				new XElementFormatter(),
				new ChooseFormatter(),
				new SubStringFormatter(),
				new IsMatchFormatter(),
				new DefaultFormatter()
			});
			return smartFormatter;
		}
	}
}
