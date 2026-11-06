using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x0200007B RID: 123
	[Serializable]
	public class XmlSource : ISource
	{
		// Token: 0x0600033C RID: 828 RVA: 0x0000C8CE File Offset: 0x0000AACE
		public XmlSource(SmartFormatter formatter)
		{
			formatter.Parser.AddAlphanumericSelectors();
			formatter.Parser.AddAdditionalSelectorChars("_");
			formatter.Parser.AddOperators(".");
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000C904 File Offset: 0x0000AB04
		public bool TryEvaluateSelector(ISelectorInfo selectorInfo)
		{
			XElement xelement = selectorInfo.CurrentValue as XElement;
			if (xelement != null)
			{
				string selector = selectorInfo.SelectorText;
				List<XElement> list = (from x in xelement.Elements()
					where x.Name.LocalName == selector
					select x).ToList<XElement>();
				if (list.Any<XElement>())
				{
					selectorInfo.Result = list;
					return true;
				}
			}
			return false;
		}
	}
}
