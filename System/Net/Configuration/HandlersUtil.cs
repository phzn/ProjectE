using System;
using System.Configuration;
using System.Xml;

namespace System.Net.Configuration
{
	// Token: 0x02000764 RID: 1892
	internal class HandlersUtil
	{
		// Token: 0x06003BAF RID: 15279 RVA: 0x0000219B File Offset: 0x0000039B
		private HandlersUtil()
		{
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x000CC96C File Offset: 0x000CAB6C
		internal static string ExtractAttributeValue(string attKey, XmlNode node)
		{
			return HandlersUtil.ExtractAttributeValue(attKey, node, false);
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x000CC978 File Offset: 0x000CAB78
		internal static string ExtractAttributeValue(string attKey, XmlNode node, bool optional)
		{
			if (node.Attributes == null)
			{
				if (optional)
				{
					return null;
				}
				HandlersUtil.ThrowException("Required attribute not found: " + attKey, node);
			}
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(attKey);
			if (xmlNode == null)
			{
				if (optional)
				{
					return null;
				}
				HandlersUtil.ThrowException("Required attribute not found: " + attKey, node);
			}
			string value = xmlNode.Value;
			if (value == string.Empty)
			{
				HandlersUtil.ThrowException((optional ? "Optional" : "Required") + " attribute is empty: " + attKey, node);
			}
			return value;
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x000CC9FC File Offset: 0x000CABFC
		internal static void ThrowException(string msg, XmlNode node)
		{
			if (node != null && node.Name != string.Empty)
			{
				msg = msg + " (node name: " + node.Name + ") ";
			}
			throw new ConfigurationException(msg, node);
		}
	}
}
