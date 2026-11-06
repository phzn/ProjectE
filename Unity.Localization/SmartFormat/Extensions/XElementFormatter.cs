using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x0200007A RID: 122
	[Serializable]
	public class XElementFormatter : FormatterBase
	{
		// Token: 0x06000339 RID: 825 RVA: 0x0000C82C File Offset: 0x0000AA2C
		public XElementFormatter()
		{
			base.Names = this.DefaultNames;
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000C840 File Offset: 0x0000AA40
		public override string[] DefaultNames
		{
			get
			{
				return new string[] { "xelement", "xml", "x", "" };
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000C868 File Offset: 0x0000AA68
		public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			Format format = formattingInfo.Format;
			object currentValue = formattingInfo.CurrentValue;
			XElement xelement = null;
			if (format != null && format.HasNested)
			{
				return false;
			}
			IList<XElement> list = currentValue as IList<XElement>;
			if (list != null && list.Count > 0)
			{
				xelement = list[0];
			}
			XElement xelement2 = xelement ?? (currentValue as XElement);
			if (xelement2 != null)
			{
				formattingInfo.Write(xelement2.Value);
				return true;
			}
			return false;
		}
	}
}
