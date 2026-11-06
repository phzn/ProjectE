using System;
using System.Threading;
using System.Threading.Tasks;

namespace System.Xml.Linq
{
	// Token: 0x02000041 RID: 65
	internal struct ElementWriter
	{
		// Token: 0x06000258 RID: 600 RVA: 0x0000B99D File Offset: 0x00009B9D
		public ElementWriter(XmlWriter writer)
		{
			this._writer = writer;
			this._resolver = default(NamespaceResolver);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000B9B4 File Offset: 0x00009BB4
		public void WriteElement(XElement e)
		{
			this.PushAncestors(e);
			XElement xelement = e;
			XNode xnode = e;
			for (;;)
			{
				e = xnode as XElement;
				if (e != null)
				{
					this.WriteStartElement(e);
					if (e.content == null)
					{
						this.WriteEndElement();
					}
					else
					{
						string text = e.content as string;
						if (text == null)
						{
							xnode = ((XNode)e.content).next;
							continue;
						}
						this._writer.WriteString(text);
						this.WriteFullEndElement();
					}
				}
				else
				{
					xnode.WriteTo(this._writer);
				}
				while (xnode != xelement && xnode == xnode.parent.content)
				{
					xnode = xnode.parent;
					this.WriteFullEndElement();
				}
				if (xnode == xelement)
				{
					break;
				}
				xnode = xnode.next;
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000BA64 File Offset: 0x00009C64
		public async Task WriteElementAsync(XElement e, CancellationToken cancellationToken)
		{
			this.PushAncestors(e);
			XElement root = e;
			XNode i = e;
			for (;;)
			{
				e = i as XElement;
				if (e != null)
				{
					await this.WriteStartElementAsync(e, cancellationToken).ConfigureAwait(false);
					if (e.content == null)
					{
						await this.WriteEndElementAsync(cancellationToken).ConfigureAwait(false);
					}
					else
					{
						string text = e.content as string;
						if (text == null)
						{
							i = ((XNode)e.content).next;
							continue;
						}
						cancellationToken.ThrowIfCancellationRequested();
						await this._writer.WriteStringAsync(text).ConfigureAwait(false);
						await this.WriteFullEndElementAsync(cancellationToken).ConfigureAwait(false);
					}
				}
				else
				{
					await i.WriteToAsync(this._writer, cancellationToken).ConfigureAwait(false);
				}
				while (i != root && i == i.parent.content)
				{
					i = i.parent;
					await this.WriteFullEndElementAsync(cancellationToken).ConfigureAwait(false);
				}
				if (i == root)
				{
					break;
				}
				i = i.next;
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000BABC File Offset: 0x00009CBC
		private string GetPrefixOfNamespace(XNamespace ns, bool allowDefaultNamespace)
		{
			string namespaceName = ns.NamespaceName;
			if (namespaceName.Length == 0)
			{
				return string.Empty;
			}
			string prefixOfNamespace = this._resolver.GetPrefixOfNamespace(ns, allowDefaultNamespace);
			if (prefixOfNamespace != null)
			{
				return prefixOfNamespace;
			}
			if (namespaceName == "http://www.w3.org/XML/1998/namespace")
			{
				return "xml";
			}
			if (namespaceName == "http://www.w3.org/2000/xmlns/")
			{
				return "xmlns";
			}
			return null;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000BB10 File Offset: 0x00009D10
		private void PushAncestors(XElement e)
		{
			for (;;)
			{
				e = e.parent as XElement;
				if (e == null)
				{
					break;
				}
				XAttribute xattribute = e.lastAttr;
				if (xattribute != null)
				{
					do
					{
						xattribute = xattribute.next;
						if (xattribute.IsNamespaceDeclaration)
						{
							this._resolver.AddFirst((xattribute.Name.NamespaceName.Length == 0) ? string.Empty : xattribute.Name.LocalName, XNamespace.Get(xattribute.Value));
						}
					}
					while (xattribute != e.lastAttr);
				}
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000BB8C File Offset: 0x00009D8C
		private void PushElement(XElement e)
		{
			this._resolver.PushScope();
			XAttribute xattribute = e.lastAttr;
			if (xattribute != null)
			{
				do
				{
					xattribute = xattribute.next;
					if (xattribute.IsNamespaceDeclaration)
					{
						this._resolver.Add((xattribute.Name.NamespaceName.Length == 0) ? string.Empty : xattribute.Name.LocalName, XNamespace.Get(xattribute.Value));
					}
				}
				while (xattribute != e.lastAttr);
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000BC00 File Offset: 0x00009E00
		private void WriteEndElement()
		{
			this._writer.WriteEndElement();
			this._resolver.PopScope();
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000BC18 File Offset: 0x00009E18
		private async Task WriteEndElementAsync(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await this._writer.WriteEndElementAsync().ConfigureAwait(false);
			this._resolver.PopScope();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000BC68 File Offset: 0x00009E68
		private void WriteFullEndElement()
		{
			this._writer.WriteFullEndElement();
			this._resolver.PopScope();
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000BC80 File Offset: 0x00009E80
		private async Task WriteFullEndElementAsync(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await this._writer.WriteFullEndElementAsync().ConfigureAwait(false);
			this._resolver.PopScope();
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000BCD0 File Offset: 0x00009ED0
		private void WriteStartElement(XElement e)
		{
			this.PushElement(e);
			XNamespace xnamespace = e.Name.Namespace;
			this._writer.WriteStartElement(this.GetPrefixOfNamespace(xnamespace, true), e.Name.LocalName, xnamespace.NamespaceName);
			XAttribute xattribute = e.lastAttr;
			if (xattribute != null)
			{
				do
				{
					xattribute = xattribute.next;
					xnamespace = xattribute.Name.Namespace;
					string localName = xattribute.Name.LocalName;
					string namespaceName = xnamespace.NamespaceName;
					this._writer.WriteAttributeString(this.GetPrefixOfNamespace(xnamespace, false), localName, (namespaceName.Length == 0 && localName == "xmlns") ? "http://www.w3.org/2000/xmlns/" : namespaceName, xattribute.Value);
				}
				while (xattribute != e.lastAttr);
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000BD84 File Offset: 0x00009F84
		private async Task WriteStartElementAsync(XElement e, CancellationToken cancellationToken)
		{
			this.PushElement(e);
			XNamespace xnamespace = e.Name.Namespace;
			await this._writer.WriteStartElementAsync(this.GetPrefixOfNamespace(xnamespace, true), e.Name.LocalName, xnamespace.NamespaceName).ConfigureAwait(false);
			XAttribute a = e.lastAttr;
			if (a != null)
			{
				do
				{
					a = a.next;
					xnamespace = a.Name.Namespace;
					string localName = a.Name.LocalName;
					string namespaceName = xnamespace.NamespaceName;
					await this._writer.WriteAttributeStringAsync(this.GetPrefixOfNamespace(xnamespace, false), localName, (namespaceName.Length == 0 && localName == "xmlns") ? "http://www.w3.org/2000/xmlns/" : namespaceName, a.Value).ConfigureAwait(false);
				}
				while (a != e.lastAttr);
			}
		}

		// Token: 0x0400014C RID: 332
		private XmlWriter _writer;

		// Token: 0x0400014D RID: 333
		private NamespaceResolver _resolver;
	}
}
