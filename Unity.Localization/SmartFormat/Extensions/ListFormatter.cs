using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Localization.SmartFormat.Core.Settings;
using UnityEngine.Pool;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x02000071 RID: 113
	[Serializable]
	public class ListFormatter : FormatterBase, ISource, IFormatterLiteralExtractor
	{
		// Token: 0x060002EA RID: 746 RVA: 0x0000B2C9 File Offset: 0x000094C9
		public ListFormatter(SmartFormatter formatter)
		{
			formatter.Parser.AddOperators("[]()");
			this.m_SmartSettings = formatter.Settings;
			base.Names = this.DefaultNames;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000B2F9 File Offset: 0x000094F9
		public override string[] DefaultNames
		{
			get
			{
				return new string[] { "list", "l", "" };
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000B31C File Offset: 0x0000951C
		public bool TryEvaluateSelector(ISelectorInfo selectorInfo)
		{
			object currentValue = selectorInfo.CurrentValue;
			string selectorText = selectorInfo.SelectorText;
			IList list = currentValue as IList;
			if (list == null)
			{
				return false;
			}
			int num;
			if ((selectorInfo.SelectorIndex != 0 || selectorInfo.SelectorOperator.Length != 0) && int.TryParse(selectorText, out num) && num < list.Count)
			{
				selectorInfo.Result = list[num];
				return true;
			}
			if (selectorText.Equals("index", StringComparison.OrdinalIgnoreCase))
			{
				if (selectorInfo.SelectorIndex == 0)
				{
					selectorInfo.Result = ListFormatter.CollectionIndex;
					return true;
				}
				if (0 <= ListFormatter.CollectionIndex && ListFormatter.CollectionIndex < list.Count)
				{
					selectorInfo.Result = list[ListFormatter.CollectionIndex];
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000B3CF File Offset: 0x000095CF
		// (set) Token: 0x060002EE RID: 750 RVA: 0x0000B3D6 File Offset: 0x000095D6
		private static int CollectionIndex { get; set; } = -1;

		// Token: 0x060002EF RID: 751 RVA: 0x0000B3E0 File Offset: 0x000095E0
		public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			Format format = formattingInfo.Format;
			object currentValue = formattingInfo.CurrentValue;
			IEnumerable enumerable = currentValue as IEnumerable;
			if (enumerable == null)
			{
				return false;
			}
			if (currentValue is string)
			{
				return false;
			}
			if (currentValue is IFormattable)
			{
				return false;
			}
			if (format == null)
			{
				return false;
			}
			IList<Format> list = format.Split('|', 4);
			if (list.Count < 2)
			{
				return false;
			}
			Format format2 = list[0];
			string text = ((list.Count >= 2) ? list[1].GetLiteralText() : "");
			string text2 = ((list.Count >= 3) ? list[2].GetLiteralText() : text);
			string text3 = ((list.Count >= 4) ? list[3].GetLiteralText() : text2);
			if (!format2.HasNested)
			{
				Format format3 = FormatItemPool.GetFormat(this.m_SmartSettings, format2.baseString, format2.startIndex, format2.endIndex, true);
				Placeholder placeholder = FormatItemPool.GetPlaceholder(this.m_SmartSettings, format3, format2.startIndex, 0, format2, format2.endIndex);
				format3.Items.Add(placeholder);
				format2 = format3;
			}
			List<object> list2 = null;
			ICollection collection = currentValue as ICollection;
			if (collection == null)
			{
				list2 = CollectionPool<List<object>, object>.Get();
				foreach (object obj in enumerable)
				{
					list2.Add(obj);
				}
				collection = list2;
			}
			int collectionIndex = ListFormatter.CollectionIndex;
			ListFormatter.CollectionIndex = -1;
			foreach (object obj2 in collection)
			{
				ListFormatter.CollectionIndex++;
				if (text != null && ListFormatter.CollectionIndex != 0)
				{
					if (ListFormatter.CollectionIndex < collection.Count - 1)
					{
						formattingInfo.Write(text);
					}
					else if (ListFormatter.CollectionIndex == 1)
					{
						formattingInfo.Write(text3);
					}
					else
					{
						formattingInfo.Write(text2);
					}
				}
				formattingInfo.Write(format2, obj2);
			}
			ListFormatter.CollectionIndex = collectionIndex;
			if (list2 != null)
			{
				CollectionPool<List<object>, object>.Release(list2);
			}
			return true;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000B60C File Offset: 0x0000980C
		public void WriteAllLiterals(IFormattingInfo formattingInfo)
		{
			Format format = formattingInfo.Format;
			if (format == null)
			{
				return;
			}
			IList<Format> list = format.Split('|', 4);
			if (list.Count < 2)
			{
				return;
			}
			Format format2 = list[0];
			for (int i = 0; i < Math.Min(list.Count, 4); i++)
			{
				formattingInfo.Write(list[i], null);
			}
			if (!format2.HasNested)
			{
				Format format3 = FormatItemPool.GetFormat(this.m_SmartSettings, format2.baseString, format2.startIndex, format2.endIndex, true);
				Placeholder placeholder = FormatItemPool.GetPlaceholder(this.m_SmartSettings, format3, format2.startIndex, 0, format2, format2.endIndex);
				format3.Items.Add(placeholder);
				format2 = format3;
			}
			formattingInfo.Write(format2, null);
		}

		// Token: 0x040000C0 RID: 192
		[SerializeReference]
		[HideInInspector]
		private SmartSettings m_SmartSettings;
	}
}
