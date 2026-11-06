using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Xml.Linq
{
	/// <summary>Represents a node that can contain other nodes.</summary>
	// Token: 0x0200001D RID: 29
	public abstract class XContainer : XNode
	{
		// Token: 0x06000108 RID: 264 RVA: 0x000059FB File Offset: 0x00003BFB
		internal XContainer()
		{
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005A04 File Offset: 0x00003C04
		internal XContainer(XContainer other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (other.content is string)
			{
				this.content = other.content;
				return;
			}
			XNode xnode = (XNode)other.content;
			if (xnode != null)
			{
				do
				{
					xnode = xnode.next;
					this.AppendNodeSkipNotify(xnode.CloneNode());
				}
				while (xnode != other.content);
			}
		}

		/// <summary>Gets the first child node of this node.</summary>
		/// <returns>An <see cref="T:System.Xml.Linq.XNode" /> containing the first child node of the <see cref="T:System.Xml.Linq.XContainer" />.</returns>
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00005A6C File Offset: 0x00003C6C
		public XNode FirstNode
		{
			get
			{
				XNode lastNode = this.LastNode;
				if (lastNode == null)
				{
					return null;
				}
				return lastNode.next;
			}
		}

		/// <summary>Gets the last child node of this node.</summary>
		/// <returns>An <see cref="T:System.Xml.Linq.XNode" /> containing the last child node of the <see cref="T:System.Xml.Linq.XContainer" />.</returns>
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00005A8C File Offset: 0x00003C8C
		public XNode LastNode
		{
			get
			{
				if (this.content == null)
				{
					return null;
				}
				XNode xnode = this.content as XNode;
				if (xnode != null)
				{
					return xnode;
				}
				string text = this.content as string;
				if (text != null)
				{
					if (text.Length == 0)
					{
						return null;
					}
					XText xtext = new XText(text);
					xtext.parent = this;
					xtext.next = xtext;
					Interlocked.CompareExchange<object>(ref this.content, xtext, text);
				}
				return (XNode)this.content;
			}
		}

		/// <summary>Adds the specified content as children of this <see cref="T:System.Xml.Linq.XContainer" />.</summary>
		/// <param name="content">A content object containing simple content or a collection of content objects to be added.</param>
		// Token: 0x0600010C RID: 268 RVA: 0x00005AFC File Offset: 0x00003CFC
		public void Add(object content)
		{
			if (base.SkipNotify())
			{
				this.AddContentSkipNotify(content);
				return;
			}
			if (content == null)
			{
				return;
			}
			XNode xnode = content as XNode;
			if (xnode != null)
			{
				this.AddNode(xnode);
				return;
			}
			string text = content as string;
			if (text != null)
			{
				this.AddString(text);
				return;
			}
			XAttribute xattribute = content as XAttribute;
			if (xattribute != null)
			{
				this.AddAttribute(xattribute);
				return;
			}
			XStreamingElement xstreamingElement = content as XStreamingElement;
			if (xstreamingElement != null)
			{
				this.AddNode(new XElement(xstreamingElement));
				return;
			}
			object[] array = content as object[];
			if (array != null)
			{
				foreach (object obj in array)
				{
					this.Add(obj);
				}
				return;
			}
			IEnumerable enumerable = content as IEnumerable;
			if (enumerable != null)
			{
				foreach (object obj2 in enumerable)
				{
					this.Add(obj2);
				}
				return;
			}
			this.AddString(XContainer.GetStringValue(content));
		}

		/// <summary>Adds the specified content as children of this <see cref="T:System.Xml.Linq.XContainer" />.</summary>
		/// <param name="content">A parameter list of content objects.</param>
		// Token: 0x0600010D RID: 269 RVA: 0x00005C04 File Offset: 0x00003E04
		public void Add(params object[] content)
		{
			this.Add(content);
		}

		/// <summary>Adds the specified content as the first children of this document or element.</summary>
		/// <param name="content">A content object containing simple content or a collection of content objects to be added.</param>
		// Token: 0x0600010E RID: 270 RVA: 0x00005C10 File Offset: 0x00003E10
		public void AddFirst(object content)
		{
			new Inserter(this, null).Add(content);
		}

		/// <summary>Adds the specified content as the first children of this document or element.</summary>
		/// <param name="content">A parameter list of content objects.</param>
		/// <exception cref="T:System.InvalidOperationException">The parent is <see langword="null" />.</exception>
		// Token: 0x0600010F RID: 271 RVA: 0x00005C2D File Offset: 0x00003E2D
		public void AddFirst(params object[] content)
		{
			this.AddFirst(content);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlWriter" /> that can be used to add nodes to the <see cref="T:System.Xml.Linq.XContainer" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> that is ready to have content written to it.</returns>
		// Token: 0x06000110 RID: 272 RVA: 0x00005C38 File Offset: 0x00003E38
		public XmlWriter CreateWriter()
		{
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.ConformanceLevel = ((this is XDocument) ? ConformanceLevel.Document : ConformanceLevel.Fragment);
			return XmlWriter.Create(new XNodeBuilder(this), xmlWriterSettings);
		}

		/// <summary>Returns a collection of the descendant nodes for this document or element, in document order.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XNode" /> containing the descendant nodes of the <see cref="T:System.Xml.Linq.XContainer" />, in document order.</returns>
		// Token: 0x06000111 RID: 273 RVA: 0x00005C69 File Offset: 0x00003E69
		public IEnumerable<XNode> DescendantNodes()
		{
			return this.GetDescendantNodes(false);
		}

		/// <summary>Returns a collection of the descendant elements for this document or element, in document order.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> containing the descendant elements of the <see cref="T:System.Xml.Linq.XContainer" />.</returns>
		// Token: 0x06000112 RID: 274 RVA: 0x00005C72 File Offset: 0x00003E72
		public IEnumerable<XElement> Descendants()
		{
			return this.GetDescendants(null, false);
		}

		/// <summary>Returns a filtered collection of the descendant elements for this document or element, in document order. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</summary>
		/// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> to match.</param>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> containing the descendant elements of the <see cref="T:System.Xml.Linq.XContainer" /> that match the specified <see cref="T:System.Xml.Linq.XName" />.</returns>
		// Token: 0x06000113 RID: 275 RVA: 0x00005C7C File Offset: 0x00003E7C
		public IEnumerable<XElement> Descendants(XName name)
		{
			if (!(name != null))
			{
				return XElement.EmptySequence;
			}
			return this.GetDescendants(name, false);
		}

		/// <summary>Gets the first (in document order) child element with the specified <see cref="T:System.Xml.Linq.XName" />.</summary>
		/// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> to match.</param>
		/// <returns>A <see cref="T:System.Xml.Linq.XElement" /> that matches the specified <see cref="T:System.Xml.Linq.XName" />, or <see langword="null" />.</returns>
		// Token: 0x06000114 RID: 276 RVA: 0x00005C98 File Offset: 0x00003E98
		public XElement Element(XName name)
		{
			XNode xnode = this.content as XNode;
			if (xnode != null)
			{
				XElement xelement;
				for (;;)
				{
					xnode = xnode.next;
					xelement = xnode as XElement;
					if (xelement != null && xelement.name == name)
					{
						break;
					}
					if (xnode == this.content)
					{
						goto IL_0039;
					}
				}
				return xelement;
			}
			IL_0039:
			return null;
		}

		/// <summary>Returns a collection of the child elements of this element or document, in document order.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> containing the child elements of this <see cref="T:System.Xml.Linq.XContainer" />, in document order.</returns>
		// Token: 0x06000115 RID: 277 RVA: 0x00005CDF File Offset: 0x00003EDF
		public IEnumerable<XElement> Elements()
		{
			return this.GetElements(null);
		}

		/// <summary>Returns a filtered collection of the child elements of this element or document, in document order. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</summary>
		/// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> to match.</param>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> containing the children of the <see cref="T:System.Xml.Linq.XContainer" /> that have a matching <see cref="T:System.Xml.Linq.XName" />, in document order.</returns>
		// Token: 0x06000116 RID: 278 RVA: 0x00005CE8 File Offset: 0x00003EE8
		public IEnumerable<XElement> Elements(XName name)
		{
			if (!(name != null))
			{
				return XElement.EmptySequence;
			}
			return this.GetElements(name);
		}

		/// <summary>Returns a collection of the child nodes of this element or document, in document order.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XNode" /> containing the contents of this <see cref="T:System.Xml.Linq.XContainer" />, in document order.</returns>
		// Token: 0x06000117 RID: 279 RVA: 0x00005D00 File Offset: 0x00003F00
		public IEnumerable<XNode> Nodes()
		{
			XNode i = this.LastNode;
			if (i != null)
			{
				do
				{
					i = i.next;
					yield return i;
				}
				while (i.parent == this && i != this.content);
			}
			yield break;
		}

		/// <summary>Removes the child nodes from this document or element.</summary>
		// Token: 0x06000118 RID: 280 RVA: 0x00005D10 File Offset: 0x00003F10
		public void RemoveNodes()
		{
			if (base.SkipNotify())
			{
				this.RemoveNodesSkipNotify();
				return;
			}
			while (this.content != null)
			{
				string text = this.content as string;
				if (text != null)
				{
					if (text.Length > 0)
					{
						this.ConvertTextToNode();
					}
					else if (this is XElement)
					{
						base.NotifyChanging(this, XObjectChangeEventArgs.Value);
						if (text != this.content)
						{
							throw new InvalidOperationException("This operation was corrupted by external code.");
						}
						this.content = null;
						base.NotifyChanged(this, XObjectChangeEventArgs.Value);
					}
					else
					{
						this.content = null;
					}
				}
				XNode xnode = this.content as XNode;
				if (xnode != null)
				{
					XNode next = xnode.next;
					base.NotifyChanging(next, XObjectChangeEventArgs.Remove);
					if (xnode != this.content || next != xnode.next)
					{
						throw new InvalidOperationException("This operation was corrupted by external code.");
					}
					if (next != xnode)
					{
						xnode.next = next.next;
					}
					else
					{
						this.content = null;
					}
					next.parent = null;
					next.next = null;
					base.NotifyChanged(next, XObjectChangeEventArgs.Remove);
				}
			}
		}

		/// <summary>Replaces the children nodes of this document or element with the specified content.</summary>
		/// <param name="content">A content object containing simple content or a collection of content objects that replace the children nodes.</param>
		// Token: 0x06000119 RID: 281 RVA: 0x00005E14 File Offset: 0x00004014
		public void ReplaceNodes(object content)
		{
			content = XContainer.GetContentSnapshot(content);
			this.RemoveNodes();
			this.Add(content);
		}

		/// <summary>Replaces the children nodes of this document or element with the specified content.</summary>
		/// <param name="content">A parameter list of content objects.</param>
		// Token: 0x0600011A RID: 282 RVA: 0x00005E2B File Offset: 0x0000402B
		public void ReplaceNodes(params object[] content)
		{
			this.ReplaceNodes(content);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000043E9 File Offset: 0x000025E9
		internal virtual void AddAttribute(XAttribute a)
		{
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000043E9 File Offset: 0x000025E9
		internal virtual void AddAttributeSkipNotify(XAttribute a)
		{
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005E34 File Offset: 0x00004034
		internal void AddContentSkipNotify(object content)
		{
			if (content == null)
			{
				return;
			}
			XNode xnode = content as XNode;
			if (xnode != null)
			{
				this.AddNodeSkipNotify(xnode);
				return;
			}
			string text = content as string;
			if (text != null)
			{
				this.AddStringSkipNotify(text);
				return;
			}
			XAttribute xattribute = content as XAttribute;
			if (xattribute != null)
			{
				this.AddAttributeSkipNotify(xattribute);
				return;
			}
			XStreamingElement xstreamingElement = content as XStreamingElement;
			if (xstreamingElement != null)
			{
				this.AddNodeSkipNotify(new XElement(xstreamingElement));
				return;
			}
			object[] array = content as object[];
			if (array != null)
			{
				foreach (object obj in array)
				{
					this.AddContentSkipNotify(obj);
				}
				return;
			}
			IEnumerable enumerable = content as IEnumerable;
			if (enumerable != null)
			{
				foreach (object obj2 in enumerable)
				{
					this.AddContentSkipNotify(obj2);
				}
				return;
			}
			this.AddStringSkipNotify(XContainer.GetStringValue(content));
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005F2C File Offset: 0x0000412C
		internal void AddNode(XNode n)
		{
			this.ValidateNode(n, this);
			if (n.parent != null)
			{
				n = n.CloneNode();
			}
			else
			{
				XNode xnode = this;
				while (xnode.parent != null)
				{
					xnode = xnode.parent;
				}
				if (n == xnode)
				{
					n = n.CloneNode();
				}
			}
			this.ConvertTextToNode();
			this.AppendNode(n);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005F80 File Offset: 0x00004180
		internal void AddNodeSkipNotify(XNode n)
		{
			this.ValidateNode(n, this);
			if (n.parent != null)
			{
				n = n.CloneNode();
			}
			else
			{
				XNode xnode = this;
				while (xnode.parent != null)
				{
					xnode = xnode.parent;
				}
				if (n == xnode)
				{
					n = n.CloneNode();
				}
			}
			this.ConvertTextToNode();
			this.AppendNodeSkipNotify(n);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005FD4 File Offset: 0x000041D4
		internal void AddString(string s)
		{
			this.ValidateString(s);
			if (this.content != null)
			{
				if (s.Length > 0)
				{
					this.ConvertTextToNode();
					XText xtext = this.content as XText;
					if (xtext != null && !(xtext is XCData))
					{
						XText xtext2 = xtext;
						xtext2.Value += s;
						return;
					}
					this.AppendNode(new XText(s));
				}
				return;
			}
			if (s.Length > 0)
			{
				this.AppendNode(new XText(s));
				return;
			}
			if (!(this is XElement))
			{
				this.content = s;
				return;
			}
			base.NotifyChanging(this, XObjectChangeEventArgs.Value);
			if (this.content != null)
			{
				throw new InvalidOperationException("This operation was corrupted by external code.");
			}
			this.content = s;
			base.NotifyChanged(this, XObjectChangeEventArgs.Value);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006090 File Offset: 0x00004290
		internal void AddStringSkipNotify(string s)
		{
			this.ValidateString(s);
			if (this.content == null)
			{
				this.content = s;
				return;
			}
			if (s.Length > 0)
			{
				string text = this.content as string;
				if (text != null)
				{
					this.content = text + s;
					return;
				}
				XText xtext = this.content as XText;
				if (xtext != null && !(xtext is XCData))
				{
					XText xtext2 = xtext;
					xtext2.text += s;
					return;
				}
				this.AppendNodeSkipNotify(new XText(s));
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006110 File Offset: 0x00004310
		internal void AppendNode(XNode n)
		{
			bool flag = base.NotifyChanging(n, XObjectChangeEventArgs.Add);
			if (n.parent != null)
			{
				throw new InvalidOperationException("This operation was corrupted by external code.");
			}
			this.AppendNodeSkipNotify(n);
			if (flag)
			{
				base.NotifyChanged(n, XObjectChangeEventArgs.Add);
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006148 File Offset: 0x00004348
		internal void AppendNodeSkipNotify(XNode n)
		{
			n.parent = this;
			if (this.content == null || this.content is string)
			{
				n.next = n;
			}
			else
			{
				XNode xnode = (XNode)this.content;
				n.next = xnode.next;
				xnode.next = n;
			}
			this.content = n;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000061A0 File Offset: 0x000043A0
		internal override void AppendText(StringBuilder sb)
		{
			string text = this.content as string;
			if (text != null)
			{
				sb.Append(text);
				return;
			}
			XNode xnode = (XNode)this.content;
			if (xnode != null)
			{
				do
				{
					xnode = xnode.next;
					xnode.AppendText(sb);
				}
				while (xnode != this.content);
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000061EC File Offset: 0x000043EC
		private string GetTextOnly()
		{
			if (this.content == null)
			{
				return null;
			}
			string text = this.content as string;
			if (text == null)
			{
				XNode xnode = (XNode)this.content;
				for (;;)
				{
					xnode = xnode.next;
					if (xnode.NodeType != XmlNodeType.Text)
					{
						break;
					}
					text += ((XText)xnode).Value;
					if (xnode == this.content)
					{
						return text;
					}
				}
				return null;
			}
			return text;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000624C File Offset: 0x0000444C
		private string CollectText(ref XNode n)
		{
			string text = "";
			while (n != null && n.NodeType == XmlNodeType.Text)
			{
				text += ((XText)n).Value;
				n = ((n != this.content) ? n.next : null);
			}
			return text;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000629C File Offset: 0x0000449C
		internal bool ContentsEqual(XContainer e)
		{
			if (this.content == e.content)
			{
				return true;
			}
			string textOnly = this.GetTextOnly();
			if (textOnly != null)
			{
				return textOnly == e.GetTextOnly();
			}
			XNode xnode = this.content as XNode;
			XNode xnode2 = e.content as XNode;
			if (xnode != null && xnode2 != null)
			{
				xnode = xnode.next;
				xnode2 = xnode2.next;
				while (!(this.CollectText(ref xnode) != e.CollectText(ref xnode2)))
				{
					if (xnode == null && xnode2 == null)
					{
						return true;
					}
					if (xnode == null || xnode2 == null || !xnode.DeepEquals(xnode2))
					{
						break;
					}
					xnode = ((xnode != this.content) ? xnode.next : null);
					xnode2 = ((xnode2 != e.content) ? xnode2.next : null);
				}
			}
			return false;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006354 File Offset: 0x00004554
		internal int ContentsHashCode()
		{
			string textOnly = this.GetTextOnly();
			if (textOnly != null)
			{
				return textOnly.GetHashCode();
			}
			int num = 0;
			XNode xnode = this.content as XNode;
			if (xnode != null)
			{
				do
				{
					xnode = xnode.next;
					string text = this.CollectText(ref xnode);
					if (text.Length > 0)
					{
						num ^= text.GetHashCode();
					}
					if (xnode == null)
					{
						break;
					}
					num ^= xnode.GetDeepHashCode();
				}
				while (xnode != this.content);
			}
			return num;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000063BC File Offset: 0x000045BC
		internal void ConvertTextToNode()
		{
			string text = this.content as string;
			if (!string.IsNullOrEmpty(text))
			{
				XText xtext = new XText(text);
				xtext.parent = this;
				xtext.next = xtext;
				this.content = xtext;
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000063F9 File Offset: 0x000045F9
		internal IEnumerable<XNode> GetDescendantNodes(bool self)
		{
			if (self)
			{
				yield return this;
			}
			XNode i = this;
			for (;;)
			{
				XContainer xcontainer = i as XContainer;
				XNode firstNode;
				if (xcontainer != null && (firstNode = xcontainer.FirstNode) != null)
				{
					i = firstNode;
				}
				else
				{
					while (i != null && i != this && i == i.parent.content)
					{
						i = i.parent;
					}
					if (i == null || i == this)
					{
						break;
					}
					i = i.next;
				}
				yield return i;
			}
			yield break;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006410 File Offset: 0x00004610
		internal IEnumerable<XElement> GetDescendants(XName name, bool self)
		{
			if (self)
			{
				XElement xelement = (XElement)this;
				if (name == null || xelement.name == name)
				{
					yield return xelement;
				}
			}
			XNode i = this;
			XContainer xcontainer = this;
			for (;;)
			{
				if (xcontainer != null && xcontainer.content is XNode)
				{
					i = ((XNode)xcontainer.content).next;
				}
				else
				{
					while (i != this && i == i.parent.content)
					{
						i = i.parent;
					}
					if (i == this)
					{
						break;
					}
					i = i.next;
				}
				XElement e = i as XElement;
				if (e != null && (name == null || e.name == name))
				{
					yield return e;
				}
				xcontainer = e;
				e = null;
			}
			yield break;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000642E File Offset: 0x0000462E
		private IEnumerable<XElement> GetElements(XName name)
		{
			XNode i = this.content as XNode;
			if (i != null)
			{
				do
				{
					i = i.next;
					XElement xelement = i as XElement;
					if (xelement != null && (name == null || xelement.name == name))
					{
						yield return xelement;
					}
				}
				while (i.parent == this && i != this.content);
			}
			yield break;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006448 File Offset: 0x00004648
		internal static string GetStringValue(object value)
		{
			string text = value as string;
			if (text != null)
			{
				return text;
			}
			if (value is double)
			{
				text = XmlConvert.ToString((double)value);
			}
			else if (value is float)
			{
				text = XmlConvert.ToString((float)value);
			}
			else if (value is decimal)
			{
				text = XmlConvert.ToString((decimal)value);
			}
			else if (value is bool)
			{
				text = XmlConvert.ToString((bool)value);
			}
			else if (value is DateTime)
			{
				text = XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.RoundtripKind);
			}
			else if (value is DateTimeOffset)
			{
				text = XmlConvert.ToString((DateTimeOffset)value);
			}
			else if (value is TimeSpan)
			{
				text = XmlConvert.ToString((TimeSpan)value);
			}
			else
			{
				if (value is XObject)
				{
					throw new ArgumentException("An XObject cannot be used as a value.");
				}
				text = value.ToString();
			}
			if (text == null)
			{
				throw new ArgumentException("The argument cannot be converted to a string.");
			}
			return text;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000652C File Offset: 0x0000472C
		internal void ReadContentFrom(XmlReader r)
		{
			if (r.ReadState != ReadState.Interactive)
			{
				throw new InvalidOperationException("The XmlReader state should be Interactive.");
			}
			XContainer.ContentReader contentReader = new XContainer.ContentReader(this);
			while (contentReader.ReadContentFrom(this, r) && r.Read())
			{
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006568 File Offset: 0x00004768
		internal void ReadContentFrom(XmlReader r, LoadOptions o)
		{
			if ((o & (LoadOptions.SetBaseUri | LoadOptions.SetLineInfo)) == LoadOptions.None)
			{
				this.ReadContentFrom(r);
				return;
			}
			if (r.ReadState != ReadState.Interactive)
			{
				throw new InvalidOperationException("The XmlReader state should be Interactive.");
			}
			XContainer.ContentReader contentReader = new XContainer.ContentReader(this, r, o);
			while (contentReader.ReadContentFrom(this, r, o) && r.Read())
			{
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000065B4 File Offset: 0x000047B4
		internal async Task ReadContentFromAsync(XmlReader r, CancellationToken cancellationToken)
		{
			if (r.ReadState != ReadState.Interactive)
			{
				throw new InvalidOperationException("The XmlReader state should be Interactive.");
			}
			XContainer.ContentReader cr = new XContainer.ContentReader(this);
			bool flag;
			do
			{
				cancellationToken.ThrowIfCancellationRequested();
				flag = cr.ReadContentFrom(this, r);
				if (flag)
				{
					flag = await r.ReadAsync().ConfigureAwait(false);
				}
			}
			while (flag);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006608 File Offset: 0x00004808
		internal async Task ReadContentFromAsync(XmlReader r, LoadOptions o, CancellationToken cancellationToken)
		{
			if ((o & (LoadOptions.SetBaseUri | LoadOptions.SetLineInfo)) == LoadOptions.None)
			{
				await this.ReadContentFromAsync(r, cancellationToken).ConfigureAwait(false);
			}
			else
			{
				if (r.ReadState != ReadState.Interactive)
				{
					throw new InvalidOperationException("The XmlReader state should be Interactive.");
				}
				XContainer.ContentReader cr = new XContainer.ContentReader(this, r, o);
				bool flag;
				do
				{
					cancellationToken.ThrowIfCancellationRequested();
					flag = cr.ReadContentFrom(this, r, o);
					if (flag)
					{
						flag = await r.ReadAsync().ConfigureAwait(false);
					}
				}
				while (flag);
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006664 File Offset: 0x00004864
		internal void RemoveNode(XNode n)
		{
			bool flag = base.NotifyChanging(n, XObjectChangeEventArgs.Remove);
			if (n.parent != this)
			{
				throw new InvalidOperationException("This operation was corrupted by external code.");
			}
			XNode xnode = (XNode)this.content;
			while (xnode.next != n)
			{
				xnode = xnode.next;
			}
			if (xnode == n)
			{
				this.content = null;
			}
			else
			{
				if (this.content == n)
				{
					this.content = xnode;
				}
				xnode.next = n.next;
			}
			n.parent = null;
			n.next = null;
			if (flag)
			{
				base.NotifyChanged(n, XObjectChangeEventArgs.Remove);
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000066F8 File Offset: 0x000048F8
		private void RemoveNodesSkipNotify()
		{
			XNode xnode = this.content as XNode;
			if (xnode != null)
			{
				do
				{
					XNode next = xnode.next;
					xnode.parent = null;
					xnode.next = null;
					xnode = next;
				}
				while (xnode != this.content);
			}
			this.content = null;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000043E9 File Offset: 0x000025E9
		internal virtual void ValidateNode(XNode node, XNode previous)
		{
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000043E9 File Offset: 0x000025E9
		internal virtual void ValidateString(string s)
		{
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000673C File Offset: 0x0000493C
		internal void WriteContentTo(XmlWriter writer)
		{
			if (this.content != null)
			{
				string text = this.content as string;
				if (text != null)
				{
					if (this is XDocument)
					{
						writer.WriteWhitespace(text);
						return;
					}
					writer.WriteString(text);
					return;
				}
				else
				{
					XNode xnode = (XNode)this.content;
					do
					{
						xnode = xnode.next;
						xnode.WriteTo(writer);
					}
					while (xnode != this.content);
				}
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000679C File Offset: 0x0000499C
		internal async Task WriteContentToAsync(XmlWriter writer, CancellationToken cancellationToken)
		{
			if (this.content != null)
			{
				string text = this.content as string;
				if (text != null)
				{
					cancellationToken.ThrowIfCancellationRequested();
					Task task;
					if (this is XDocument)
					{
						task = writer.WriteWhitespaceAsync(text);
					}
					else
					{
						task = writer.WriteStringAsync(text);
					}
					await task.ConfigureAwait(false);
				}
				else
				{
					XNode i = (XNode)this.content;
					do
					{
						i = i.next;
						await i.WriteToAsync(writer, cancellationToken).ConfigureAwait(false);
					}
					while (i != this.content);
					i = null;
				}
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000067F0 File Offset: 0x000049F0
		private static void AddContentToList(List<object> list, object content)
		{
			IEnumerable enumerable = ((content is string) ? null : (content as IEnumerable));
			if (enumerable == null)
			{
				list.Add(content);
				return;
			}
			foreach (object obj in enumerable)
			{
				if (obj != null)
				{
					XContainer.AddContentToList(list, obj);
				}
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006860 File Offset: 0x00004A60
		internal static object GetContentSnapshot(object content)
		{
			if (content is string || !(content is IEnumerable))
			{
				return content;
			}
			List<object> list = new List<object>();
			XContainer.AddContentToList(list, content);
			return list;
		}

		// Token: 0x0400008B RID: 139
		internal object content;

		// Token: 0x0200001E RID: 30
		private sealed class ContentReader
		{
			// Token: 0x0600013A RID: 314 RVA: 0x00006880 File Offset: 0x00004A80
			public ContentReader(XContainer rootContainer)
			{
				this._currentContainer = rootContainer;
			}

			// Token: 0x0600013B RID: 315 RVA: 0x0000688F File Offset: 0x00004A8F
			public ContentReader(XContainer rootContainer, XmlReader r, LoadOptions o)
			{
				this._currentContainer = rootContainer;
				this._baseUri = (((o & LoadOptions.SetBaseUri) != LoadOptions.None) ? r.BaseURI : null);
				this._lineInfo = (((o & LoadOptions.SetLineInfo) != LoadOptions.None) ? (r as IXmlLineInfo) : null);
			}

			// Token: 0x0600013C RID: 316 RVA: 0x000068C8 File Offset: 0x00004AC8
			public bool ReadContentFrom(XContainer rootContainer, XmlReader r)
			{
				switch (r.NodeType)
				{
				case XmlNodeType.Element:
				{
					NamespaceCache namespaceCache = this._eCache;
					XElement xelement = new XElement(namespaceCache.Get(r.NamespaceURI).GetName(r.LocalName));
					if (r.MoveToFirstAttribute())
					{
						do
						{
							XElement xelement2 = xelement;
							namespaceCache = this._aCache;
							xelement2.AppendAttributeSkipNotify(new XAttribute(namespaceCache.Get((r.Prefix.Length == 0) ? string.Empty : r.NamespaceURI).GetName(r.LocalName), r.Value));
						}
						while (r.MoveToNextAttribute());
						r.MoveToElement();
					}
					this._currentContainer.AddNodeSkipNotify(xelement);
					if (!r.IsEmptyElement)
					{
						this._currentContainer = xelement;
						return true;
					}
					return true;
				}
				case XmlNodeType.Text:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					this._currentContainer.AddStringSkipNotify(r.Value);
					return true;
				case XmlNodeType.CDATA:
					this._currentContainer.AddNodeSkipNotify(new XCData(r.Value));
					return true;
				case XmlNodeType.EntityReference:
					if (!r.CanResolveEntity)
					{
						throw new InvalidOperationException("The XmlReader cannot resolve entity references.");
					}
					r.ResolveEntity();
					return true;
				case XmlNodeType.ProcessingInstruction:
					this._currentContainer.AddNodeSkipNotify(new XProcessingInstruction(r.Name, r.Value));
					return true;
				case XmlNodeType.Comment:
					this._currentContainer.AddNodeSkipNotify(new XComment(r.Value));
					return true;
				case XmlNodeType.DocumentType:
					this._currentContainer.AddNodeSkipNotify(new XDocumentType(r.LocalName, r.GetAttribute("PUBLIC"), r.GetAttribute("SYSTEM"), r.Value));
					return true;
				case XmlNodeType.EndElement:
					if (this._currentContainer.content == null)
					{
						this._currentContainer.content = string.Empty;
					}
					if (this._currentContainer == rootContainer)
					{
						return false;
					}
					this._currentContainer = this._currentContainer.parent;
					return true;
				case XmlNodeType.EndEntity:
					return true;
				}
				throw new InvalidOperationException(global::SR.Format("The XmlReader should not be on a node of type {0}.", r.NodeType));
			}

			// Token: 0x0600013D RID: 317 RVA: 0x00006AE0 File Offset: 0x00004CE0
			public bool ReadContentFrom(XContainer rootContainer, XmlReader r, LoadOptions o)
			{
				XNode xnode = null;
				string baseURI = r.BaseURI;
				switch (r.NodeType)
				{
				case XmlNodeType.Element:
				{
					NamespaceCache namespaceCache = this._eCache;
					XElement xelement = new XElement(namespaceCache.Get(r.NamespaceURI).GetName(r.LocalName));
					if (this._baseUri != null && this._baseUri != baseURI)
					{
						xelement.SetBaseUri(baseURI);
					}
					if (this._lineInfo != null && this._lineInfo.HasLineInfo())
					{
						xelement.SetLineInfo(this._lineInfo.LineNumber, this._lineInfo.LinePosition);
					}
					if (r.MoveToFirstAttribute())
					{
						do
						{
							namespaceCache = this._aCache;
							XAttribute xattribute = new XAttribute(namespaceCache.Get((r.Prefix.Length == 0) ? string.Empty : r.NamespaceURI).GetName(r.LocalName), r.Value);
							if (this._lineInfo != null && this._lineInfo.HasLineInfo())
							{
								xattribute.SetLineInfo(this._lineInfo.LineNumber, this._lineInfo.LinePosition);
							}
							xelement.AppendAttributeSkipNotify(xattribute);
						}
						while (r.MoveToNextAttribute());
						r.MoveToElement();
					}
					this._currentContainer.AddNodeSkipNotify(xelement);
					if (r.IsEmptyElement)
					{
						goto IL_032F;
					}
					this._currentContainer = xelement;
					if (this._baseUri != null)
					{
						this._baseUri = baseURI;
						goto IL_032F;
					}
					goto IL_032F;
				}
				case XmlNodeType.Text:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					if ((this._baseUri != null && this._baseUri != baseURI) || (this._lineInfo != null && this._lineInfo.HasLineInfo()))
					{
						xnode = new XText(r.Value);
						goto IL_032F;
					}
					this._currentContainer.AddStringSkipNotify(r.Value);
					goto IL_032F;
				case XmlNodeType.CDATA:
					xnode = new XCData(r.Value);
					goto IL_032F;
				case XmlNodeType.EntityReference:
					if (!r.CanResolveEntity)
					{
						throw new InvalidOperationException("The XmlReader cannot resolve entity references.");
					}
					r.ResolveEntity();
					goto IL_032F;
				case XmlNodeType.ProcessingInstruction:
					xnode = new XProcessingInstruction(r.Name, r.Value);
					goto IL_032F;
				case XmlNodeType.Comment:
					xnode = new XComment(r.Value);
					goto IL_032F;
				case XmlNodeType.DocumentType:
					xnode = new XDocumentType(r.LocalName, r.GetAttribute("PUBLIC"), r.GetAttribute("SYSTEM"), r.Value);
					goto IL_032F;
				case XmlNodeType.EndElement:
				{
					if (this._currentContainer.content == null)
					{
						this._currentContainer.content = string.Empty;
					}
					XElement xelement2 = this._currentContainer as XElement;
					if (xelement2 != null && this._lineInfo != null && this._lineInfo.HasLineInfo())
					{
						xelement2.SetEndElementLineInfo(this._lineInfo.LineNumber, this._lineInfo.LinePosition);
					}
					if (this._currentContainer == rootContainer)
					{
						return false;
					}
					if (this._baseUri != null && this._currentContainer.HasBaseUri)
					{
						this._baseUri = this._currentContainer.parent.BaseUri;
					}
					this._currentContainer = this._currentContainer.parent;
					goto IL_032F;
				}
				case XmlNodeType.EndEntity:
					goto IL_032F;
				}
				throw new InvalidOperationException(global::SR.Format("The XmlReader should not be on a node of type {0}.", r.NodeType));
				IL_032F:
				if (xnode != null)
				{
					if (this._baseUri != null && this._baseUri != baseURI)
					{
						xnode.SetBaseUri(baseURI);
					}
					if (this._lineInfo != null && this._lineInfo.HasLineInfo())
					{
						xnode.SetLineInfo(this._lineInfo.LineNumber, this._lineInfo.LinePosition);
					}
					this._currentContainer.AddNodeSkipNotify(xnode);
				}
				return true;
			}

			// Token: 0x0400008C RID: 140
			private readonly NamespaceCache _eCache;

			// Token: 0x0400008D RID: 141
			private readonly NamespaceCache _aCache;

			// Token: 0x0400008E RID: 142
			private readonly IXmlLineInfo _lineInfo;

			// Token: 0x0400008F RID: 143
			private XContainer _currentContainer;

			// Token: 0x04000090 RID: 144
			private string _baseUri;
		}
	}
}
