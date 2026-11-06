using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Xml.Linq
{
	/// <summary>Represents an XML element.  See XElement Class Overview and the Remarks section on this page for usage information and examples.</summary>
	// Token: 0x0200002F RID: 47
	[XmlSchemaProvider(null, IsAny = true)]
	public class XElement : XContainer, IXmlSerializable
	{
		/// <summary>Gets an empty collection of elements.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> that contains an empty collection.</returns>
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00008EFD File Offset: 0x000070FD
		public static IEnumerable<XElement> EmptySequence
		{
			get
			{
				return Array.Empty<XElement>();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XElement" /> class with the specified name.</summary>
		/// <param name="name">An <see cref="T:System.Xml.Linq.XName" /> that contains the name of the element.</param>
		// Token: 0x060001BB RID: 443 RVA: 0x00008F04 File Offset: 0x00007104
		public XElement(XName name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.name = name;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XElement" /> class with the specified name and content.</summary>
		/// <param name="name">An <see cref="T:System.Xml.Linq.XName" /> that contains the element name.</param>
		/// <param name="content">The contents of the element.</param>
		// Token: 0x060001BC RID: 444 RVA: 0x00008F27 File Offset: 0x00007127
		public XElement(XName name, object content)
			: this(name)
		{
			base.AddContentSkipNotify(content);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XElement" /> class with the specified name and content.</summary>
		/// <param name="name">An <see cref="T:System.Xml.Linq.XName" /> that contains the element name.</param>
		/// <param name="content">The initial content of the element.</param>
		// Token: 0x060001BD RID: 445 RVA: 0x00008F37 File Offset: 0x00007137
		public XElement(XName name, params object[] content)
			: this(name, content)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XElement" /> class from another <see cref="T:System.Xml.Linq.XElement" /> object.</summary>
		/// <param name="other">An <see cref="T:System.Xml.Linq.XElement" /> object to copy from.</param>
		// Token: 0x060001BE RID: 446 RVA: 0x00008F44 File Offset: 0x00007144
		public XElement(XElement other)
			: base(other)
		{
			this.name = other.name;
			XAttribute next = other.lastAttr;
			if (next != null)
			{
				do
				{
					next = next.next;
					this.AppendAttributeSkipNotify(new XAttribute(next));
				}
				while (next != other.lastAttr);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XElement" /> class from an <see cref="T:System.Xml.Linq.XStreamingElement" /> object.</summary>
		/// <param name="other">An <see cref="T:System.Xml.Linq.XStreamingElement" /> that contains unevaluated queries that will be iterated for the contents of this <see cref="T:System.Xml.Linq.XElement" />.</param>
		// Token: 0x060001BF RID: 447 RVA: 0x00008F8A File Offset: 0x0000718A
		public XElement(XStreamingElement other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			this.name = other.name;
			base.AddContentSkipNotify(other.content);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008FB8 File Offset: 0x000071B8
		internal XElement()
			: this("default")
		{
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00008FCA File Offset: 0x000071CA
		internal XElement(XmlReader r)
			: this(r, LoadOptions.None)
		{
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00007A9D File Offset: 0x00005C9D
		private XElement(XElement.AsyncConstructionSentry s)
		{
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00008FD4 File Offset: 0x000071D4
		internal XElement(XmlReader r, LoadOptions o)
		{
			this.ReadElementFrom(r, o);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008FE4 File Offset: 0x000071E4
		internal static async Task<XElement> CreateAsync(XmlReader r, CancellationToken cancellationToken)
		{
			XElement xe = new XElement(default(XElement.AsyncConstructionSentry));
			await xe.ReadElementFromAsync(r, LoadOptions.None, cancellationToken).ConfigureAwait(false);
			return xe;
		}

		/// <summary>Serialize this element to a file.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that contains the name of the file.</param>
		// Token: 0x060001C5 RID: 453 RVA: 0x0000902F File Offset: 0x0000722F
		public void Save(string fileName)
		{
			this.Save(fileName, base.GetSaveOptionsFromAnnotations());
		}

		/// <summary>Serialize this element to a file, optionally disabling formatting.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that contains the name of the file.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> that specifies formatting behavior.</param>
		// Token: 0x060001C6 RID: 454 RVA: 0x00009040 File Offset: 0x00007240
		public void Save(string fileName, SaveOptions options)
		{
			XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
			using (XmlWriter xmlWriter = XmlWriter.Create(fileName, xmlWriterSettings))
			{
				this.Save(xmlWriter);
			}
		}

		/// <summary>Gets the first attribute of this element.</summary>
		/// <returns>An <see cref="T:System.Xml.Linq.XAttribute" /> that contains the first attribute of this element.</returns>
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00009080 File Offset: 0x00007280
		public XAttribute FirstAttribute
		{
			get
			{
				if (this.lastAttr == null)
				{
					return null;
				}
				return this.lastAttr.next;
			}
		}

		/// <summary>Gets a value indicating whether this element has at least one attribute.</summary>
		/// <returns>
		///   <see langword="true" /> if this element has at least one attribute; otherwise <see langword="false" />.</returns>
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00009097 File Offset: 0x00007297
		public bool HasAttributes
		{
			get
			{
				return this.lastAttr != null;
			}
		}

		/// <summary>Gets a value indicating whether this element has at least one child element.</summary>
		/// <returns>
		///   <see langword="true" /> if this element has at least one child element; otherwise <see langword="false" />.</returns>
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x000090A4 File Offset: 0x000072A4
		public bool HasElements
		{
			get
			{
				XNode xnode = this.content as XNode;
				if (xnode != null)
				{
					while (!(xnode is XElement))
					{
						xnode = xnode.next;
						if (xnode == this.content)
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
		}

		/// <summary>Gets a value indicating whether this element contains no content.</summary>
		/// <returns>
		///   <see langword="true" /> if this element contains no content; otherwise <see langword="false" />.</returns>
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001CA RID: 458 RVA: 0x000090DB File Offset: 0x000072DB
		public bool IsEmpty
		{
			get
			{
				return this.content == null;
			}
		}

		/// <summary>Gets the last attribute of this element.</summary>
		/// <returns>An <see cref="T:System.Xml.Linq.XAttribute" /> that contains the last attribute of this element.</returns>
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000090E6 File Offset: 0x000072E6
		public XAttribute LastAttribute
		{
			get
			{
				return this.lastAttr;
			}
		}

		/// <summary>Gets or sets the name of this element.</summary>
		/// <returns>An <see cref="T:System.Xml.Linq.XName" /> that contains the name of this element.</returns>
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001CC RID: 460 RVA: 0x000090EE File Offset: 0x000072EE
		// (set) Token: 0x060001CD RID: 461 RVA: 0x000090F6 File Offset: 0x000072F6
		public XName Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				bool flag = base.NotifyChanging(this, XObjectChangeEventArgs.Name);
				this.name = value;
				if (flag)
				{
					base.NotifyChanged(this, XObjectChangeEventArgs.Name);
				}
			}
		}

		/// <summary>Gets the node type for this node.</summary>
		/// <returns>The node type. For <see cref="T:System.Xml.Linq.XElement" /> objects, this value is <see cref="F:System.Xml.XmlNodeType.Element" />.</returns>
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001CE RID: 462 RVA: 0x0000912E File Offset: 0x0000732E
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Element;
			}
		}

		/// <summary>Gets or sets the concatenated text contents of this element.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains all of the text content of this element. If there are multiple text nodes, they will be concatenated.</returns>
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00009134 File Offset: 0x00007334
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00009175 File Offset: 0x00007375
		public string Value
		{
			get
			{
				if (this.content == null)
				{
					return string.Empty;
				}
				string text = this.content as string;
				if (text != null)
				{
					return text;
				}
				StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
				this.AppendText(stringBuilder);
				return StringBuilderCache.GetStringAndRelease(stringBuilder);
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.RemoveNodes();
				base.Add(value);
			}
		}

		/// <summary>Returns a collection of elements that contain this element, and the ancestors of this element.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> of elements that contain this element, and the ancestors of this element.</returns>
		// Token: 0x060001D1 RID: 465 RVA: 0x00009192 File Offset: 0x00007392
		public IEnumerable<XElement> AncestorsAndSelf()
		{
			return base.GetAncestors(null, true);
		}

		/// <summary>Returns a filtered collection of elements that contain this element, and the ancestors of this element. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</summary>
		/// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> to match.</param>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> that contain this element, and the ancestors of this element. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</returns>
		// Token: 0x060001D2 RID: 466 RVA: 0x0000919C File Offset: 0x0000739C
		public IEnumerable<XElement> AncestorsAndSelf(XName name)
		{
			if (!(name != null))
			{
				return XElement.EmptySequence;
			}
			return base.GetAncestors(name, true);
		}

		/// <summary>Returns the <see cref="T:System.Xml.Linq.XAttribute" /> of this <see cref="T:System.Xml.Linq.XElement" /> that has the specified <see cref="T:System.Xml.Linq.XName" />.</summary>
		/// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> of the <see cref="T:System.Xml.Linq.XAttribute" /> to get.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XAttribute" /> that has the specified <see cref="T:System.Xml.Linq.XName" />; <see langword="null" /> if there is no attribute with the specified name.</returns>
		// Token: 0x060001D3 RID: 467 RVA: 0x000091B8 File Offset: 0x000073B8
		public XAttribute Attribute(XName name)
		{
			XAttribute next = this.lastAttr;
			if (next != null)
			{
				for (;;)
				{
					next = next.next;
					if (next.name == name)
					{
						break;
					}
					if (next == this.lastAttr)
					{
						goto IL_002A;
					}
				}
				return next;
			}
			IL_002A:
			return null;
		}

		/// <summary>Returns a collection of attributes of this element.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XAttribute" /> of attributes of this element.</returns>
		// Token: 0x060001D4 RID: 468 RVA: 0x000091F0 File Offset: 0x000073F0
		public IEnumerable<XAttribute> Attributes()
		{
			return this.GetAttributes(null);
		}

		/// <summary>Returns a filtered collection of attributes of this element. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</summary>
		/// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> to match.</param>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XAttribute" /> that contains the attributes of this element. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</returns>
		// Token: 0x060001D5 RID: 469 RVA: 0x000091F9 File Offset: 0x000073F9
		public IEnumerable<XAttribute> Attributes(XName name)
		{
			if (!(name != null))
			{
				return XAttribute.EmptySequence;
			}
			return this.GetAttributes(name);
		}

		/// <summary>Returns a collection of nodes that contain this element, and all descendant nodes of this element, in document order.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XNode" /> that contain this element, and all descendant nodes of this element, in document order.</returns>
		// Token: 0x060001D6 RID: 470 RVA: 0x00009211 File Offset: 0x00007411
		public IEnumerable<XNode> DescendantNodesAndSelf()
		{
			return base.GetDescendantNodes(true);
		}

		/// <summary>Returns a collection of elements that contain this element, and all descendant elements of this element, in document order.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> of elements that contain this element, and all descendant elements of this element, in document order.</returns>
		// Token: 0x060001D7 RID: 471 RVA: 0x0000921A File Offset: 0x0000741A
		public IEnumerable<XElement> DescendantsAndSelf()
		{
			return base.GetDescendants(null, true);
		}

		/// <summary>Returns a filtered collection of elements that contain this element, and all descendant elements of this element, in document order. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</summary>
		/// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> to match.</param>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> that contain this element, and all descendant elements of this element, in document order. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</returns>
		// Token: 0x060001D8 RID: 472 RVA: 0x00009224 File Offset: 0x00007424
		public IEnumerable<XElement> DescendantsAndSelf(XName name)
		{
			if (!(name != null))
			{
				return XElement.EmptySequence;
			}
			return base.GetDescendants(name, true);
		}

		/// <summary>Gets the default <see cref="T:System.Xml.Linq.XNamespace" /> of this <see cref="T:System.Xml.Linq.XElement" />.</summary>
		/// <returns>An <see cref="T:System.Xml.Linq.XNamespace" /> that contains the default namespace of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		// Token: 0x060001D9 RID: 473 RVA: 0x00009240 File Offset: 0x00007440
		public XNamespace GetDefaultNamespace()
		{
			string namespaceOfPrefixInScope = this.GetNamespaceOfPrefixInScope("xmlns", null);
			if (namespaceOfPrefixInScope == null)
			{
				return XNamespace.None;
			}
			return XNamespace.Get(namespaceOfPrefixInScope);
		}

		/// <summary>Gets the namespace associated with a particular prefix for this <see cref="T:System.Xml.Linq.XElement" />.</summary>
		/// <param name="prefix">A string that contains the namespace prefix to look up.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XNamespace" /> for the namespace associated with the prefix for this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		// Token: 0x060001DA RID: 474 RVA: 0x0000926C File Offset: 0x0000746C
		public XNamespace GetNamespaceOfPrefix(string prefix)
		{
			if (prefix == null)
			{
				throw new ArgumentNullException("prefix");
			}
			if (prefix.Length == 0)
			{
				throw new ArgumentException(global::SR.Format("'{0}' is an invalid prefix.", prefix));
			}
			if (prefix == "xmlns")
			{
				return XNamespace.Xmlns;
			}
			string namespaceOfPrefixInScope = this.GetNamespaceOfPrefixInScope(prefix, null);
			if (namespaceOfPrefixInScope != null)
			{
				return XNamespace.Get(namespaceOfPrefixInScope);
			}
			if (prefix == "xml")
			{
				return XNamespace.Xml;
			}
			return null;
		}

		/// <summary>Gets the prefix associated with a namespace for this <see cref="T:System.Xml.Linq.XElement" />.</summary>
		/// <param name="ns">An <see cref="T:System.Xml.Linq.XNamespace" /> to look up.</param>
		/// <returns>A <see cref="T:System.String" /> that contains the namespace prefix.</returns>
		// Token: 0x060001DB RID: 475 RVA: 0x000092DC File Offset: 0x000074DC
		public string GetPrefixOfNamespace(XNamespace ns)
		{
			if (ns == null)
			{
				throw new ArgumentNullException("ns");
			}
			string namespaceName = ns.NamespaceName;
			bool flag = false;
			XElement xelement = this;
			XAttribute next;
			for (;;)
			{
				next = xelement.lastAttr;
				if (next != null)
				{
					bool flag2 = false;
					do
					{
						next = next.next;
						if (next.IsNamespaceDeclaration)
						{
							if (next.Value == namespaceName && next.Name.NamespaceName.Length != 0 && (!flag || this.GetNamespaceOfPrefixInScope(next.Name.LocalName, xelement) == null))
							{
								goto IL_0072;
							}
							flag2 = true;
						}
					}
					while (next != xelement.lastAttr);
					flag = flag || flag2;
				}
				xelement = xelement.parent as XElement;
				if (xelement == null)
				{
					goto Block_8;
				}
			}
			IL_0072:
			return next.Name.LocalName;
			Block_8:
			if (namespaceName == "http://www.w3.org/XML/1998/namespace")
			{
				if (!flag || this.GetNamespaceOfPrefixInScope("xml", null) == null)
				{
					return "xml";
				}
			}
			else if (namespaceName == "http://www.w3.org/2000/xmlns/")
			{
				return "xmlns";
			}
			return null;
		}

		/// <summary>Loads an <see cref="T:System.Xml.Linq.XElement" /> from a file.</summary>
		/// <param name="uri">A URI string referencing the file to load into a new <see cref="T:System.Xml.Linq.XElement" />.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XElement" /> that contains the contents of the specified file.</returns>
		// Token: 0x060001DC RID: 476 RVA: 0x000093B5 File Offset: 0x000075B5
		public static XElement Load(string uri)
		{
			return XElement.Load(uri, LoadOptions.None);
		}

		/// <summary>Loads an <see cref="T:System.Xml.Linq.XElement" /> from a file, optionally preserving white space, setting the base URI, and retaining line information.</summary>
		/// <param name="uri">A URI string referencing the file to load into an <see cref="T:System.Xml.Linq.XElement" />.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies white space behavior, and whether to load base URI and line information.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XElement" /> that contains the contents of the specified file.</returns>
		// Token: 0x060001DD RID: 477 RVA: 0x000093C0 File Offset: 0x000075C0
		public static XElement Load(string uri, LoadOptions options)
		{
			XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
			XElement xelement;
			using (XmlReader xmlReader = XmlReader.Create(uri, xmlReaderSettings))
			{
				xelement = XElement.Load(xmlReader, options);
			}
			return xelement;
		}

		/// <summary>Creates a new <see cref="T:System.Xml.Linq.XElement" /> instance by using the specified stream.</summary>
		/// <param name="stream">The stream that contains the XML data.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XElement" /> object used to read the data that is contained in the stream.</returns>
		// Token: 0x060001DE RID: 478 RVA: 0x00009404 File Offset: 0x00007604
		public static XElement Load(Stream stream)
		{
			return XElement.Load(stream, LoadOptions.None);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.Linq.XElement" /> instance by using the specified stream, optionally preserving white space, setting the base URI, and retaining line information.</summary>
		/// <param name="stream">The stream containing the XML data.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> object that specifies whether to load base URI and line information.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XElement" /> object used to read the data that the stream contains.</returns>
		// Token: 0x060001DF RID: 479 RVA: 0x00009410 File Offset: 0x00007610
		public static XElement Load(Stream stream, LoadOptions options)
		{
			XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
			XElement xelement;
			using (XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings))
			{
				xelement = XElement.Load(xmlReader, options);
			}
			return xelement;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009454 File Offset: 0x00007654
		public static async Task<XElement> LoadAsync(Stream stream, LoadOptions options, CancellationToken cancellationToken)
		{
			XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
			xmlReaderSettings.Async = true;
			XElement xelement;
			using (XmlReader r = XmlReader.Create(stream, xmlReaderSettings))
			{
				xelement = await XElement.LoadAsync(r, options, cancellationToken).ConfigureAwait(false);
			}
			return xelement;
		}

		/// <summary>Loads an <see cref="T:System.Xml.Linq.XElement" /> from a <see cref="T:System.IO.TextReader" />.</summary>
		/// <param name="textReader">A <see cref="T:System.IO.TextReader" /> that will be read for the <see cref="T:System.Xml.Linq.XElement" /> content.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XElement" /> that contains the XML that was read from the specified <see cref="T:System.IO.TextReader" />.</returns>
		// Token: 0x060001E1 RID: 481 RVA: 0x000094A7 File Offset: 0x000076A7
		public static XElement Load(TextReader textReader)
		{
			return XElement.Load(textReader, LoadOptions.None);
		}

		/// <summary>Loads an <see cref="T:System.Xml.Linq.XElement" /> from a <see cref="T:System.IO.TextReader" />, optionally preserving white space and retaining line information.</summary>
		/// <param name="textReader">A <see cref="T:System.IO.TextReader" /> that will be read for the <see cref="T:System.Xml.Linq.XElement" /> content.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies white space behavior, and whether to load base URI and line information.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XElement" /> that contains the XML that was read from the specified <see cref="T:System.IO.TextReader" />.</returns>
		// Token: 0x060001E2 RID: 482 RVA: 0x000094B0 File Offset: 0x000076B0
		public static XElement Load(TextReader textReader, LoadOptions options)
		{
			XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
			XElement xelement;
			using (XmlReader xmlReader = XmlReader.Create(textReader, xmlReaderSettings))
			{
				xelement = XElement.Load(xmlReader, options);
			}
			return xelement;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000094F4 File Offset: 0x000076F4
		public static async Task<XElement> LoadAsync(TextReader textReader, LoadOptions options, CancellationToken cancellationToken)
		{
			XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
			xmlReaderSettings.Async = true;
			XElement xelement;
			using (XmlReader r = XmlReader.Create(textReader, xmlReaderSettings))
			{
				xelement = await XElement.LoadAsync(r, options, cancellationToken).ConfigureAwait(false);
			}
			return xelement;
		}

		/// <summary>Loads an <see cref="T:System.Xml.Linq.XElement" /> from an <see cref="T:System.Xml.XmlReader" />.</summary>
		/// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> that will be read for the content of the <see cref="T:System.Xml.Linq.XElement" />.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XElement" /> that contains the XML that was read from the specified <see cref="T:System.Xml.XmlReader" />.</returns>
		// Token: 0x060001E4 RID: 484 RVA: 0x00009547 File Offset: 0x00007747
		public static XElement Load(XmlReader reader)
		{
			return XElement.Load(reader, LoadOptions.None);
		}

		/// <summary>Loads an <see cref="T:System.Xml.Linq.XElement" /> from an <see cref="T:System.Xml.XmlReader" />, optionally preserving white space, setting the base URI, and retaining line information.</summary>
		/// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> that will be read for the content of the <see cref="T:System.Xml.Linq.XElement" />.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies white space behavior, and whether to load base URI and line information.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XElement" /> that contains the XML that was read from the specified <see cref="T:System.Xml.XmlReader" />.</returns>
		// Token: 0x060001E5 RID: 485 RVA: 0x00009550 File Offset: 0x00007750
		public static XElement Load(XmlReader reader, LoadOptions options)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader.MoveToContent() != XmlNodeType.Element)
			{
				throw new InvalidOperationException(global::SR.Format("The XmlReader must be on a node of type {0} instead of a node of type {1}.", XmlNodeType.Element, reader.NodeType));
			}
			XElement xelement = new XElement(reader, options);
			reader.MoveToContent();
			if (!reader.EOF)
			{
				throw new InvalidOperationException("The XmlReader state should be EndOfFile after this operation.");
			}
			return xelement;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000095B6 File Offset: 0x000077B6
		public static Task<XElement> LoadAsync(XmlReader reader, LoadOptions options, CancellationToken cancellationToken)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<XElement>(cancellationToken);
			}
			return XElement.LoadAsyncInternal(reader, options, cancellationToken);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000095E0 File Offset: 0x000077E0
		private static async Task<XElement> LoadAsyncInternal(XmlReader reader, LoadOptions options, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<XmlNodeType>.ConfiguredTaskAwaiter configuredTaskAwaiter = reader.MoveToContentAsync().ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<XmlNodeType>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<XmlNodeType>.ConfiguredTaskAwaiter);
			}
			if (configuredTaskAwaiter.GetResult() != XmlNodeType.Element)
			{
				throw new InvalidOperationException(global::SR.Format("The XmlReader must be on a node of type {0} instead of a node of type {1}.", XmlNodeType.Element, reader.NodeType));
			}
			XElement e = new XElement(default(XElement.AsyncConstructionSentry));
			await e.ReadElementFromAsync(reader, options, cancellationToken).ConfigureAwait(false);
			cancellationToken.ThrowIfCancellationRequested();
			await reader.MoveToContentAsync().ConfigureAwait(false);
			if (!reader.EOF)
			{
				throw new InvalidOperationException("The XmlReader state should be EndOfFile after this operation.");
			}
			return e;
		}

		/// <summary>Load an <see cref="T:System.Xml.Linq.XElement" /> from a string that contains XML.</summary>
		/// <param name="text">A <see cref="T:System.String" /> that contains XML.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XElement" /> populated from the string that contains XML.</returns>
		// Token: 0x060001E8 RID: 488 RVA: 0x00009633 File Offset: 0x00007833
		public static XElement Parse(string text)
		{
			return XElement.Parse(text, LoadOptions.None);
		}

		/// <summary>Load an <see cref="T:System.Xml.Linq.XElement" /> from a string that contains XML, optionally preserving white space and retaining line information.</summary>
		/// <param name="text">A <see cref="T:System.String" /> that contains XML.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies white space behavior, and whether to load base URI and line information.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XElement" /> populated from the string that contains XML.</returns>
		// Token: 0x060001E9 RID: 489 RVA: 0x0000963C File Offset: 0x0000783C
		public static XElement Parse(string text, LoadOptions options)
		{
			XElement xelement;
			using (StringReader stringReader = new StringReader(text))
			{
				XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
				using (XmlReader xmlReader = XmlReader.Create(stringReader, xmlReaderSettings))
				{
					xelement = XElement.Load(xmlReader, options);
				}
			}
			return xelement;
		}

		/// <summary>Removes nodes and attributes from this <see cref="T:System.Xml.Linq.XElement" />.</summary>
		// Token: 0x060001EA RID: 490 RVA: 0x0000969C File Offset: 0x0000789C
		public void RemoveAll()
		{
			this.RemoveAttributes();
			base.RemoveNodes();
		}

		/// <summary>Removes the attributes of this <see cref="T:System.Xml.Linq.XElement" />.</summary>
		// Token: 0x060001EB RID: 491 RVA: 0x000096AC File Offset: 0x000078AC
		public void RemoveAttributes()
		{
			if (base.SkipNotify())
			{
				this.RemoveAttributesSkipNotify();
				return;
			}
			while (this.lastAttr != null)
			{
				XAttribute next = this.lastAttr.next;
				base.NotifyChanging(next, XObjectChangeEventArgs.Remove);
				if (this.lastAttr == null || next != this.lastAttr.next)
				{
					throw new InvalidOperationException("This operation was corrupted by external code.");
				}
				if (next != this.lastAttr)
				{
					this.lastAttr.next = next.next;
				}
				else
				{
					this.lastAttr = null;
				}
				next.parent = null;
				next.next = null;
				base.NotifyChanged(next, XObjectChangeEventArgs.Remove);
			}
		}

		/// <summary>Replaces the child nodes and the attributes of this element with the specified content.</summary>
		/// <param name="content">The content that will replace the child nodes and attributes of this element.</param>
		// Token: 0x060001EC RID: 492 RVA: 0x00009748 File Offset: 0x00007948
		public void ReplaceAll(object content)
		{
			content = XContainer.GetContentSnapshot(content);
			this.RemoveAll();
			base.Add(content);
		}

		/// <summary>Replaces the child nodes and the attributes of this element with the specified content.</summary>
		/// <param name="content">A parameter list of content objects.</param>
		// Token: 0x060001ED RID: 493 RVA: 0x0000975F File Offset: 0x0000795F
		public void ReplaceAll(params object[] content)
		{
			this.ReplaceAll(content);
		}

		/// <summary>Replaces the attributes of this element with the specified content.</summary>
		/// <param name="content">The content that will replace the attributes of this element.</param>
		// Token: 0x060001EE RID: 494 RVA: 0x00009768 File Offset: 0x00007968
		public void ReplaceAttributes(object content)
		{
			content = XContainer.GetContentSnapshot(content);
			this.RemoveAttributes();
			base.Add(content);
		}

		/// <summary>Replaces the attributes of this element with the specified content.</summary>
		/// <param name="content">A parameter list of content objects.</param>
		// Token: 0x060001EF RID: 495 RVA: 0x0000977F File Offset: 0x0000797F
		public void ReplaceAttributes(params object[] content)
		{
			this.ReplaceAttributes(content);
		}

		/// <summary>Outputs this <see cref="T:System.Xml.Linq.XElement" /> to the specified <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="stream">The stream to output this <see cref="T:System.Xml.Linq.XElement" /> to.</param>
		// Token: 0x060001F0 RID: 496 RVA: 0x00009788 File Offset: 0x00007988
		public void Save(Stream stream)
		{
			this.Save(stream, base.GetSaveOptionsFromAnnotations());
		}

		/// <summary>Outputs this <see cref="T:System.Xml.Linq.XElement" /> to the specified <see cref="T:System.IO.Stream" />, optionally specifying formatting behavior.</summary>
		/// <param name="stream">The stream to output this <see cref="T:System.Xml.Linq.XElement" /> to.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> object that specifies formatting behavior.</param>
		// Token: 0x060001F1 RID: 497 RVA: 0x00009798 File Offset: 0x00007998
		public void Save(Stream stream, SaveOptions options)
		{
			XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
			using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
			{
				this.Save(xmlWriter);
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000097D8 File Offset: 0x000079D8
		public async Task SaveAsync(Stream stream, SaveOptions options, CancellationToken cancellationToken)
		{
			XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
			xmlWriterSettings.Async = true;
			using (XmlWriter w = XmlWriter.Create(stream, xmlWriterSettings))
			{
				await this.SaveAsync(w, cancellationToken).ConfigureAwait(false);
			}
			XmlWriter w = null;
		}

		/// <summary>Serialize this element to a <see cref="T:System.IO.TextWriter" />.</summary>
		/// <param name="textWriter">A <see cref="T:System.IO.TextWriter" /> that the <see cref="T:System.Xml.Linq.XElement" /> will be written to.</param>
		// Token: 0x060001F3 RID: 499 RVA: 0x00009833 File Offset: 0x00007A33
		public void Save(TextWriter textWriter)
		{
			this.Save(textWriter, base.GetSaveOptionsFromAnnotations());
		}

		/// <summary>Serialize this element to a <see cref="T:System.IO.TextWriter" />, optionally disabling formatting.</summary>
		/// <param name="textWriter">The <see cref="T:System.IO.TextWriter" /> to output the XML to.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> that specifies formatting behavior.</param>
		// Token: 0x060001F4 RID: 500 RVA: 0x00009844 File Offset: 0x00007A44
		public void Save(TextWriter textWriter, SaveOptions options)
		{
			XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
			using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, xmlWriterSettings))
			{
				this.Save(xmlWriter);
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00009884 File Offset: 0x00007A84
		public async Task SaveAsync(TextWriter textWriter, SaveOptions options, CancellationToken cancellationToken)
		{
			XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
			xmlWriterSettings.Async = true;
			using (XmlWriter w = XmlWriter.Create(textWriter, xmlWriterSettings))
			{
				await this.SaveAsync(w, cancellationToken).ConfigureAwait(false);
			}
			XmlWriter w = null;
		}

		/// <summary>Serialize this element to an <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="writer">A <see cref="T:System.Xml.XmlWriter" /> that the <see cref="T:System.Xml.Linq.XElement" /> will be written to.</param>
		// Token: 0x060001F6 RID: 502 RVA: 0x000098DF File Offset: 0x00007ADF
		public void Save(XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.WriteStartDocument();
			this.WriteTo(writer);
			writer.WriteEndDocument();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00009902 File Offset: 0x00007B02
		public Task SaveAsync(XmlWriter writer, CancellationToken cancellationToken)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			return this.SaveAsyncInternal(writer, cancellationToken);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000992C File Offset: 0x00007B2C
		private async Task SaveAsyncInternal(XmlWriter writer, CancellationToken cancellationToken)
		{
			await writer.WriteStartDocumentAsync().ConfigureAwait(false);
			await this.WriteToAsync(writer, cancellationToken).ConfigureAwait(false);
			cancellationToken.ThrowIfCancellationRequested();
			await writer.WriteEndDocumentAsync().ConfigureAwait(false);
		}

		/// <summary>Sets the value of an attribute, adds an attribute, or removes an attribute.</summary>
		/// <param name="name">An <see cref="T:System.Xml.Linq.XName" /> that contains the name of the attribute to change.</param>
		/// <param name="value">The value to assign to the attribute. The attribute is removed if the value is <see langword="null" />. Otherwise, the value is converted to its string representation and assigned to the <see cref="P:System.Xml.Linq.XAttribute.Value" /> property of the attribute.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="value" /> is an instance of <see cref="T:System.Xml.Linq.XObject" />.</exception>
		// Token: 0x060001F9 RID: 505 RVA: 0x00009980 File Offset: 0x00007B80
		public void SetAttributeValue(XName name, object value)
		{
			XAttribute xattribute = this.Attribute(name);
			if (value == null)
			{
				if (xattribute != null)
				{
					this.RemoveAttribute(xattribute);
					return;
				}
			}
			else
			{
				if (xattribute != null)
				{
					xattribute.Value = XContainer.GetStringValue(value);
					return;
				}
				this.AppendAttribute(new XAttribute(name, value));
			}
		}

		/// <summary>Sets the value of a child element, adds a child element, or removes a child element.</summary>
		/// <param name="name">An <see cref="T:System.Xml.Linq.XName" /> that contains the name of the child element to change.</param>
		/// <param name="value">The value to assign to the child element. The child element is removed if the value is <see langword="null" />. Otherwise, the value is converted to its string representation and assigned to the <see cref="P:System.Xml.Linq.XElement.Value" /> property of the child element.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="value" /> is an instance of <see cref="T:System.Xml.Linq.XObject" />.</exception>
		// Token: 0x060001FA RID: 506 RVA: 0x000099C0 File Offset: 0x00007BC0
		public void SetElementValue(XName name, object value)
		{
			XElement xelement = base.Element(name);
			if (value == null)
			{
				if (xelement != null)
				{
					base.RemoveNode(xelement);
					return;
				}
			}
			else
			{
				if (xelement != null)
				{
					xelement.Value = XContainer.GetStringValue(value);
					return;
				}
				base.AddNode(new XElement(name, XContainer.GetStringValue(value)));
			}
		}

		/// <summary>Sets the value of this element.</summary>
		/// <param name="value">The value to assign to this element. The value is converted to its string representation and assigned to the <see cref="P:System.Xml.Linq.XElement.Value" /> property.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="value" /> is an <see cref="T:System.Xml.Linq.XObject" />.</exception>
		// Token: 0x060001FB RID: 507 RVA: 0x00009A05 File Offset: 0x00007C05
		public void SetValue(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.Value = XContainer.GetStringValue(value);
		}

		/// <summary>Write this element to an <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="writer">An <see cref="T:System.Xml.XmlWriter" /> into which this method will write.</param>
		// Token: 0x060001FC RID: 508 RVA: 0x00009A24 File Offset: 0x00007C24
		public override void WriteTo(XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			new ElementWriter(writer).WriteElement(this);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00009A50 File Offset: 0x00007C50
		public override Task WriteToAsync(XmlWriter writer, CancellationToken cancellationToken)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			return new ElementWriter(writer).WriteElementAsync(this, cancellationToken);
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.String" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.String" />.</param>
		/// <returns>A <see cref="T:System.String" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		// Token: 0x060001FE RID: 510 RVA: 0x00009A8B File Offset: 0x00007C8B
		[CLSCompliant(false)]
		public static explicit operator string(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return element.Value;
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Boolean" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Boolean" />.</param>
		/// <returns>A <see cref="T:System.Boolean" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element does not contain a valid <see cref="T:System.Boolean" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> parameter is <see langword="null" />.</exception>
		// Token: 0x060001FF RID: 511 RVA: 0x00009A98 File Offset: 0x00007C98
		[CLSCompliant(false)]
		public static explicit operator bool(XElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return XmlConvert.ToBoolean(element.Value.ToLowerInvariant());
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Nullable`1" /> of <see cref="T:System.Boolean" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Nullable`1" /> of <see cref="T:System.Boolean" />.</param>
		/// <returns>A <see cref="T:System.Nullable`1" /> of <see cref="T:System.Boolean" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element is not <see langword="null" /> and does not contain a valid <see cref="T:System.Boolean" /> value.</exception>
		// Token: 0x06000200 RID: 512 RVA: 0x00009AB8 File Offset: 0x00007CB8
		[CLSCompliant(false)]
		public static explicit operator bool?(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new bool?(XmlConvert.ToBoolean(element.Value.ToLowerInvariant()));
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to an <see cref="T:System.Int32" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Int32" />.</param>
		/// <returns>A <see cref="T:System.Int32" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element does not contain a valid <see cref="T:System.Int32" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06000201 RID: 513 RVA: 0x00009AE7 File Offset: 0x00007CE7
		[CLSCompliant(false)]
		public static explicit operator int(XElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return XmlConvert.ToInt32(element.Value);
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Nullable`1" /> of <see cref="T:System.Int32" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Nullable`1" /> of <see cref="T:System.Int32" />.</param>
		/// <returns>A <see cref="T:System.Nullable`1" /> of <see cref="T:System.Int32" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element is not <see langword="null" /> and does not contain a valid <see cref="T:System.Int32" /> value.</exception>
		// Token: 0x06000202 RID: 514 RVA: 0x00009B04 File Offset: 0x00007D04
		[CLSCompliant(false)]
		public static explicit operator int?(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new int?(XmlConvert.ToInt32(element.Value));
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.UInt32" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.UInt32" />.</param>
		/// <returns>A <see cref="T:System.UInt32" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element does not contain a valid <see cref="T:System.UInt32" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06000203 RID: 515 RVA: 0x00009B2E File Offset: 0x00007D2E
		[CLSCompliant(false)]
		public static explicit operator uint(XElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return XmlConvert.ToUInt32(element.Value);
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Nullable`1" /> of <see cref="T:System.UInt32" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Nullable`1" /> of <see cref="T:System.UInt32" />.</param>
		/// <returns>A <see cref="T:System.Nullable`1" /> of <see cref="T:System.UInt32" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element is not <see langword="null" /> and does not contain a valid <see cref="T:System.UInt32" /> value.</exception>
		// Token: 0x06000204 RID: 516 RVA: 0x00009B4C File Offset: 0x00007D4C
		[CLSCompliant(false)]
		public static explicit operator uint?(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new uint?(XmlConvert.ToUInt32(element.Value));
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to an <see cref="T:System.Int64" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Int64" />.</param>
		/// <returns>A <see cref="T:System.Int64" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element does not contain a valid <see cref="T:System.Int64" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06000205 RID: 517 RVA: 0x00009B76 File Offset: 0x00007D76
		[CLSCompliant(false)]
		public static explicit operator long(XElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return XmlConvert.ToInt64(element.Value);
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Nullable`1" /> of <see cref="T:System.Int64" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Nullable`1" /> of <see cref="T:System.Int64" />.</param>
		/// <returns>A <see cref="T:System.Nullable`1" /> of <see cref="T:System.Int64" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element is not <see langword="null" /> and does not contain a valid <see cref="T:System.Int64" /> value.</exception>
		// Token: 0x06000206 RID: 518 RVA: 0x00009B94 File Offset: 0x00007D94
		[CLSCompliant(false)]
		public static explicit operator long?(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new long?(XmlConvert.ToInt64(element.Value));
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.UInt64" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.UInt64" />.</param>
		/// <returns>A <see cref="T:System.UInt64" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element does not contain a valid <see cref="T:System.UInt64" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06000207 RID: 519 RVA: 0x00009BBE File Offset: 0x00007DBE
		[CLSCompliant(false)]
		public static explicit operator ulong(XElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return XmlConvert.ToUInt64(element.Value);
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Nullable`1" /> of <see cref="T:System.UInt64" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Nullable`1" /> of <see cref="T:System.UInt64" />.</param>
		/// <returns>A <see cref="T:System.Nullable`1" /> of <see cref="T:System.UInt64" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element is not <see langword="null" /> and does not contain a valid <see cref="T:System.UInt64" /> value.</exception>
		// Token: 0x06000208 RID: 520 RVA: 0x00009BDC File Offset: 0x00007DDC
		[CLSCompliant(false)]
		public static explicit operator ulong?(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new ulong?(XmlConvert.ToUInt64(element.Value));
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Single" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Single" />.</param>
		/// <returns>A <see cref="T:System.Single" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element does not contain a valid <see cref="T:System.Single" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06000209 RID: 521 RVA: 0x00009C06 File Offset: 0x00007E06
		[CLSCompliant(false)]
		public static explicit operator float(XElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return XmlConvert.ToSingle(element.Value);
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Nullable`1" /> of <see cref="T:System.Single" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Nullable`1" /> of <see cref="T:System.Single" />.</param>
		/// <returns>A <see cref="T:System.Nullable`1" /> of <see cref="T:System.Single" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element is not <see langword="null" /> and does not contain a valid <see cref="T:System.Single" /> value.</exception>
		// Token: 0x0600020A RID: 522 RVA: 0x00009C24 File Offset: 0x00007E24
		[CLSCompliant(false)]
		public static explicit operator float?(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new float?(XmlConvert.ToSingle(element.Value));
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Double" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Double" />.</param>
		/// <returns>A <see cref="T:System.Double" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element does not contain a valid <see cref="T:System.Double" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> parameter is <see langword="null" />.</exception>
		// Token: 0x0600020B RID: 523 RVA: 0x00009C4E File Offset: 0x00007E4E
		[CLSCompliant(false)]
		public static explicit operator double(XElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return XmlConvert.ToDouble(element.Value);
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Nullable`1" /> of <see cref="T:System.Double" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Nullable`1" /> of <see cref="T:System.Double" />.</param>
		/// <returns>A <see cref="T:System.Nullable`1" /> of <see cref="T:System.Double" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element is not <see langword="null" /> and does not contain a valid <see cref="T:System.Double" /> value.</exception>
		// Token: 0x0600020C RID: 524 RVA: 0x00009C6C File Offset: 0x00007E6C
		[CLSCompliant(false)]
		public static explicit operator double?(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new double?(XmlConvert.ToDouble(element.Value));
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Decimal" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Decimal" />.</param>
		/// <returns>A <see cref="T:System.Decimal" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element does not contain a valid <see cref="T:System.Decimal" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> parameter is <see langword="null" />.</exception>
		// Token: 0x0600020D RID: 525 RVA: 0x00009C96 File Offset: 0x00007E96
		[CLSCompliant(false)]
		public static explicit operator decimal(XElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return XmlConvert.ToDecimal(element.Value);
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Nullable`1" /> of <see cref="T:System.Decimal" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Nullable`1" /> of <see cref="T:System.Decimal" />.</param>
		/// <returns>A <see cref="T:System.Nullable`1" /> of <see cref="T:System.Decimal" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element is not <see langword="null" /> and does not contain a valid <see cref="T:System.Decimal" /> value.</exception>
		// Token: 0x0600020E RID: 526 RVA: 0x00009CB4 File Offset: 0x00007EB4
		[CLSCompliant(false)]
		public static explicit operator decimal?(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new decimal?(XmlConvert.ToDecimal(element.Value));
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.DateTime" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.DateTime" />.</param>
		/// <returns>A <see cref="T:System.DateTime" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element does not contain a valid <see cref="T:System.DateTime" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> parameter is <see langword="null" />.</exception>
		// Token: 0x0600020F RID: 527 RVA: 0x00009CDE File Offset: 0x00007EDE
		[CLSCompliant(false)]
		public static explicit operator DateTime(XElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return DateTime.Parse(element.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Nullable`1" /> of <see cref="T:System.DateTime" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Nullable`1" /> of <see cref="T:System.DateTime" />.</param>
		/// <returns>A <see cref="T:System.Nullable`1" /> of <see cref="T:System.DateTime" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element is not <see langword="null" /> and does not contain a valid <see cref="T:System.DateTime" /> value.</exception>
		// Token: 0x06000210 RID: 528 RVA: 0x00009D04 File Offset: 0x00007F04
		[CLSCompliant(false)]
		public static explicit operator DateTime?(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new DateTime?(DateTime.Parse(element.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind));
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XAttribute" /> to a <see cref="T:System.DateTimeOffset" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.DateTimeOffset" />.</param>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element does not contain a valid <see cref="T:System.DateTimeOffset" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06000211 RID: 529 RVA: 0x00009D38 File Offset: 0x00007F38
		[CLSCompliant(false)]
		public static explicit operator DateTimeOffset(XElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return XmlConvert.ToDateTimeOffset(element.Value);
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Nullable`1" /> of <see cref="T:System.DateTimeOffset" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to an <see cref="T:System.Nullable`1" /> of <see cref="T:System.DateTimeOffset" />.</param>
		/// <returns>A <see cref="T:System.Nullable`1" /> of <see cref="T:System.DateTimeOffset" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element is not <see langword="null" /> and does not contain a valid <see cref="T:System.DateTimeOffset" /> value.</exception>
		// Token: 0x06000212 RID: 530 RVA: 0x00009D54 File Offset: 0x00007F54
		[CLSCompliant(false)]
		public static explicit operator DateTimeOffset?(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new DateTimeOffset?(XmlConvert.ToDateTimeOffset(element.Value));
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.TimeSpan" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.TimeSpan" />.</param>
		/// <returns>A <see cref="T:System.TimeSpan" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element does not contain a valid <see cref="T:System.TimeSpan" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06000213 RID: 531 RVA: 0x00009D7E File Offset: 0x00007F7E
		[CLSCompliant(false)]
		public static explicit operator TimeSpan(XElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return XmlConvert.ToTimeSpan(element.Value);
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Nullable`1" /> of <see cref="T:System.TimeSpan" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Nullable`1" /> of <see cref="T:System.TimeSpan" />.</param>
		/// <returns>A <see cref="T:System.Nullable`1" /> of <see cref="T:System.TimeSpan" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element is not <see langword="null" /> and does not contain a valid <see cref="T:System.TimeSpan" /> value.</exception>
		// Token: 0x06000214 RID: 532 RVA: 0x00009D9C File Offset: 0x00007F9C
		[CLSCompliant(false)]
		public static explicit operator TimeSpan?(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new TimeSpan?(XmlConvert.ToTimeSpan(element.Value));
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Guid" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Guid" />.</param>
		/// <returns>A <see cref="T:System.Guid" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element does not contain a valid <see cref="T:System.Guid" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06000215 RID: 533 RVA: 0x00009DC6 File Offset: 0x00007FC6
		[CLSCompliant(false)]
		public static explicit operator Guid(XElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return XmlConvert.ToGuid(element.Value);
		}

		/// <summary>Cast the value of this <see cref="T:System.Xml.Linq.XElement" /> to a <see cref="T:System.Nullable`1" /> of <see cref="T:System.Guid" />.</summary>
		/// <param name="element">The <see cref="T:System.Xml.Linq.XElement" /> to cast to <see cref="T:System.Nullable`1" /> of <see cref="T:System.Guid" />.</param>
		/// <returns>A <see cref="T:System.Nullable`1" /> of <see cref="T:System.Guid" /> that contains the content of this <see cref="T:System.Xml.Linq.XElement" />.</returns>
		/// <exception cref="T:System.FormatException">The element is not <see langword="null" /> and does not contain a valid <see cref="T:System.Guid" /> value.</exception>
		// Token: 0x06000216 RID: 534 RVA: 0x00009DE4 File Offset: 0x00007FE4
		[CLSCompliant(false)]
		public static explicit operator Guid?(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new Guid?(XmlConvert.ToGuid(element.Value));
		}

		/// <summary>Gets an XML schema definition that describes the XML representation of this object.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method.</returns>
		// Token: 0x06000217 RID: 535 RVA: 0x00009E0E File Offset: 0x0000800E
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		/// <summary>Generates an object from its XML representation.</summary>
		/// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> from which the object is deserialized.</param>
		// Token: 0x06000218 RID: 536 RVA: 0x00009E14 File Offset: 0x00008014
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (this.parent != null || this.annotations != null || this.content != null || this.lastAttr != null)
			{
				throw new InvalidOperationException("This instance cannot be deserialized.");
			}
			if (reader.MoveToContent() != XmlNodeType.Element)
			{
				throw new InvalidOperationException(global::SR.Format("The XmlReader must be on a node of type {0} instead of a node of type {1}.", XmlNodeType.Element, reader.NodeType));
			}
			this.ReadElementFrom(reader, LoadOptions.None);
		}

		/// <summary>Converts an object into its XML representation.</summary>
		/// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> to which this object is serialized.</param>
		// Token: 0x06000219 RID: 537 RVA: 0x00007F94 File Offset: 0x00006194
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			this.WriteTo(writer);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00009E8C File Offset: 0x0000808C
		internal override void AddAttribute(XAttribute a)
		{
			if (this.Attribute(a.Name) != null)
			{
				throw new InvalidOperationException("Duplicate attribute.");
			}
			if (a.parent != null)
			{
				a = new XAttribute(a);
			}
			this.AppendAttribute(a);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00009EBE File Offset: 0x000080BE
		internal override void AddAttributeSkipNotify(XAttribute a)
		{
			if (this.Attribute(a.Name) != null)
			{
				throw new InvalidOperationException("Duplicate attribute.");
			}
			if (a.parent != null)
			{
				a = new XAttribute(a);
			}
			this.AppendAttributeSkipNotify(a);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00009EF0 File Offset: 0x000080F0
		internal void AppendAttribute(XAttribute a)
		{
			bool flag = base.NotifyChanging(a, XObjectChangeEventArgs.Add);
			if (a.parent != null)
			{
				throw new InvalidOperationException("This operation was corrupted by external code.");
			}
			this.AppendAttributeSkipNotify(a);
			if (flag)
			{
				base.NotifyChanged(a, XObjectChangeEventArgs.Add);
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00009F27 File Offset: 0x00008127
		internal void AppendAttributeSkipNotify(XAttribute a)
		{
			a.parent = this;
			if (this.lastAttr == null)
			{
				a.next = a;
			}
			else
			{
				a.next = this.lastAttr.next;
				this.lastAttr.next = a;
			}
			this.lastAttr = a;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00009F68 File Offset: 0x00008168
		private bool AttributesEqual(XElement e)
		{
			XAttribute next = this.lastAttr;
			XAttribute next2 = e.lastAttr;
			if (next != null && next2 != null)
			{
				for (;;)
				{
					next = next.next;
					next2 = next2.next;
					if (next.name != next2.name || next.value != next2.value)
					{
						break;
					}
					if (next == this.lastAttr)
					{
						goto Block_3;
					}
				}
				return false;
				Block_3:
				return next2 == e.lastAttr;
			}
			return next == null && next2 == null;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00009FDB File Offset: 0x000081DB
		internal override XNode CloneNode()
		{
			return new XElement(this);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00009FE4 File Offset: 0x000081E4
		internal override bool DeepEquals(XNode node)
		{
			XElement xelement = node as XElement;
			return xelement != null && this.name == xelement.name && base.ContentsEqual(xelement) && this.AttributesEqual(xelement);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000A020 File Offset: 0x00008220
		private IEnumerable<XAttribute> GetAttributes(XName name)
		{
			XAttribute a = this.lastAttr;
			if (a != null)
			{
				do
				{
					a = a.next;
					if (name == null || a.name == name)
					{
						yield return a;
					}
				}
				while (a.parent == this && a != this.lastAttr);
			}
			yield break;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000A038 File Offset: 0x00008238
		private string GetNamespaceOfPrefixInScope(string prefix, XElement outOfScope)
		{
			for (XElement xelement = this; xelement != outOfScope; xelement = xelement.parent as XElement)
			{
				XAttribute next = xelement.lastAttr;
				if (next != null)
				{
					for (;;)
					{
						next = next.next;
						if (next.IsNamespaceDeclaration && next.Name.LocalName == prefix)
						{
							break;
						}
						if (next == xelement.lastAttr)
						{
							goto IL_0040;
						}
					}
					return next.Value;
				}
				IL_0040:;
			}
			return null;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000A098 File Offset: 0x00008298
		internal override int GetDeepHashCode()
		{
			int num = this.name.GetHashCode();
			num ^= base.ContentsHashCode();
			XAttribute next = this.lastAttr;
			if (next != null)
			{
				do
				{
					next = next.next;
					num ^= next.GetDeepHashCode();
				}
				while (next != this.lastAttr);
			}
			return num;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000A0DE File Offset: 0x000082DE
		private void ReadElementFrom(XmlReader r, LoadOptions o)
		{
			this.ReadElementFromImpl(r, o);
			if (!r.IsEmptyElement)
			{
				r.Read();
				base.ReadContentFrom(r, o);
			}
			r.Read();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000A108 File Offset: 0x00008308
		private async Task ReadElementFromAsync(XmlReader r, LoadOptions o, CancellationToken cancellationTokentoken)
		{
			this.ReadElementFromImpl(r, o);
			if (!r.IsEmptyElement)
			{
				cancellationTokentoken.ThrowIfCancellationRequested();
				await r.ReadAsync().ConfigureAwait(false);
				await base.ReadContentFromAsync(r, o, cancellationTokentoken).ConfigureAwait(false);
			}
			cancellationTokentoken.ThrowIfCancellationRequested();
			await r.ReadAsync().ConfigureAwait(false);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000A164 File Offset: 0x00008364
		private void ReadElementFromImpl(XmlReader r, LoadOptions o)
		{
			if (r.ReadState != ReadState.Interactive)
			{
				throw new InvalidOperationException("The XmlReader state should be Interactive.");
			}
			this.name = XNamespace.Get(r.NamespaceURI).GetName(r.LocalName);
			if ((o & LoadOptions.SetBaseUri) != LoadOptions.None)
			{
				string baseURI = r.BaseURI;
				if (!string.IsNullOrEmpty(baseURI))
				{
					base.SetBaseUri(baseURI);
				}
			}
			IXmlLineInfo xmlLineInfo = null;
			if ((o & LoadOptions.SetLineInfo) != LoadOptions.None)
			{
				xmlLineInfo = r as IXmlLineInfo;
				if (xmlLineInfo != null && xmlLineInfo.HasLineInfo())
				{
					base.SetLineInfo(xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
				}
			}
			if (r.MoveToFirstAttribute())
			{
				do
				{
					XAttribute xattribute = new XAttribute(XNamespace.Get((r.Prefix.Length == 0) ? string.Empty : r.NamespaceURI).GetName(r.LocalName), r.Value);
					if (xmlLineInfo != null && xmlLineInfo.HasLineInfo())
					{
						xattribute.SetLineInfo(xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
					}
					this.AppendAttributeSkipNotify(xattribute);
				}
				while (r.MoveToNextAttribute());
				r.MoveToElement();
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000A258 File Offset: 0x00008458
		internal void RemoveAttribute(XAttribute a)
		{
			bool flag = base.NotifyChanging(a, XObjectChangeEventArgs.Remove);
			if (a.parent != this)
			{
				throw new InvalidOperationException("This operation was corrupted by external code.");
			}
			XAttribute xattribute = this.lastAttr;
			XAttribute next;
			while ((next = xattribute.next) != a)
			{
				xattribute = next;
			}
			if (xattribute == a)
			{
				this.lastAttr = null;
			}
			else
			{
				if (this.lastAttr == a)
				{
					this.lastAttr = xattribute;
				}
				xattribute.next = a.next;
			}
			a.parent = null;
			a.next = null;
			if (flag)
			{
				base.NotifyChanged(a, XObjectChangeEventArgs.Remove);
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000A2E4 File Offset: 0x000084E4
		private void RemoveAttributesSkipNotify()
		{
			if (this.lastAttr != null)
			{
				XAttribute xattribute = this.lastAttr;
				do
				{
					XAttribute next = xattribute.next;
					xattribute.parent = null;
					xattribute.next = null;
					xattribute = next;
				}
				while (xattribute != this.lastAttr);
				this.lastAttr = null;
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000A327 File Offset: 0x00008527
		internal void SetEndElementLineInfo(int lineNumber, int linePosition)
		{
			base.AddAnnotation(new LineInfoEndElementAnnotation(lineNumber, linePosition));
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000A336 File Offset: 0x00008536
		internal override void ValidateNode(XNode node, XNode previous)
		{
			if (node is XDocument)
			{
				throw new ArgumentException(global::SR.Format("A node of type {0} cannot be added to content.", XmlNodeType.Document));
			}
			if (node is XDocumentType)
			{
				throw new ArgumentException(global::SR.Format("A node of type {0} cannot be added to content.", XmlNodeType.DocumentType));
			}
		}

		// Token: 0x040000F9 RID: 249
		internal XName name;

		// Token: 0x040000FA RID: 250
		internal XAttribute lastAttr;

		// Token: 0x02000030 RID: 48
		private struct AsyncConstructionSentry
		{
		}
	}
}
