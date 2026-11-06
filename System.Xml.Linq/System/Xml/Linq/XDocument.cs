using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Xml.Linq
{
	/// <summary>Represents an XML document. For the components and usage of an <see cref="T:System.Xml.Linq.XDocument" /> object, see XDocument Class Overview.</summary>
	// Token: 0x02000027 RID: 39
	public class XDocument : XContainer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XDocument" /> class.</summary>
		// Token: 0x0600016E RID: 366 RVA: 0x00007A9D File Offset: 0x00005C9D
		public XDocument()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XDocument" /> class with the specified content.</summary>
		/// <param name="content">A parameter list of content objects to add to this document.</param>
		// Token: 0x0600016F RID: 367 RVA: 0x00007AA5 File Offset: 0x00005CA5
		public XDocument(params object[] content)
			: this()
		{
			base.AddContentSkipNotify(content);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XDocument" /> class with the specified <see cref="T:System.Xml.Linq.XDeclaration" /> and content.</summary>
		/// <param name="declaration">An <see cref="T:System.Xml.Linq.XDeclaration" /> for the document.</param>
		/// <param name="content">The content of the document.</param>
		// Token: 0x06000170 RID: 368 RVA: 0x00007AB4 File Offset: 0x00005CB4
		public XDocument(XDeclaration declaration, params object[] content)
			: this(content)
		{
			this._declaration = declaration;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XDocument" /> class from an existing <see cref="T:System.Xml.Linq.XDocument" /> object.</summary>
		/// <param name="other">The <see cref="T:System.Xml.Linq.XDocument" /> object that will be copied.</param>
		// Token: 0x06000171 RID: 369 RVA: 0x00007AC4 File Offset: 0x00005CC4
		public XDocument(XDocument other)
			: base(other)
		{
			if (other._declaration != null)
			{
				this._declaration = new XDeclaration(other._declaration);
			}
		}

		/// <summary>Gets or sets the XML declaration for this document.</summary>
		/// <returns>An <see cref="T:System.Xml.Linq.XDeclaration" /> that contains the XML declaration for this document.</returns>
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00007AE6 File Offset: 0x00005CE6
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00007AEE File Offset: 0x00005CEE
		public XDeclaration Declaration
		{
			get
			{
				return this._declaration;
			}
			set
			{
				this._declaration = value;
			}
		}

		/// <summary>Gets the Document Type Definition (DTD) for this document.</summary>
		/// <returns>A <see cref="T:System.Xml.Linq.XDocumentType" /> that contains the DTD for this document.</returns>
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00007AF7 File Offset: 0x00005CF7
		public XDocumentType DocumentType
		{
			get
			{
				return this.GetFirstNode<XDocumentType>();
			}
		}

		/// <summary>Gets the node type for this node.</summary>
		/// <returns>The node type. For <see cref="T:System.Xml.Linq.XDocument" /> objects, this value is <see cref="F:System.Xml.XmlNodeType.Document" />.</returns>
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00007AFF File Offset: 0x00005CFF
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Document;
			}
		}

		/// <summary>Gets the root element of the XML Tree for this document.</summary>
		/// <returns>The root <see cref="T:System.Xml.Linq.XElement" /> of the XML tree.</returns>
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00007B03 File Offset: 0x00005D03
		public XElement Root
		{
			get
			{
				return this.GetFirstNode<XElement>();
			}
		}

		/// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from a file.</summary>
		/// <param name="uri">A URI string that references the file to load into a new <see cref="T:System.Xml.Linq.XDocument" />.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> that contains the contents of the specified file.</returns>
		// Token: 0x06000177 RID: 375 RVA: 0x00007B0B File Offset: 0x00005D0B
		public static XDocument Load(string uri)
		{
			return XDocument.Load(uri, LoadOptions.None);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from a file, optionally preserving white space, setting the base URI, and retaining line information.</summary>
		/// <param name="uri">A URI string that references the file to load into a new <see cref="T:System.Xml.Linq.XDocument" />.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies white space behavior, and whether to load base URI and line information.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> that contains the contents of the specified file.</returns>
		// Token: 0x06000178 RID: 376 RVA: 0x00007B14 File Offset: 0x00005D14
		public static XDocument Load(string uri, LoadOptions options)
		{
			XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
			XDocument xdocument;
			using (XmlReader xmlReader = XmlReader.Create(uri, xmlReaderSettings))
			{
				xdocument = XDocument.Load(xmlReader, options);
			}
			return xdocument;
		}

		/// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> instance by using the specified stream.</summary>
		/// <param name="stream">The stream that contains the XML data.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> object that reads the data that is contained in the stream.</returns>
		// Token: 0x06000179 RID: 377 RVA: 0x00007B58 File Offset: 0x00005D58
		public static XDocument Load(Stream stream)
		{
			return XDocument.Load(stream, LoadOptions.None);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> instance by using the specified stream, optionally preserving white space, setting the base URI, and retaining line information.</summary>
		/// <param name="stream">The stream containing the XML data.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies whether to load base URI and line information.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> object that reads the data that is contained in the stream.</returns>
		// Token: 0x0600017A RID: 378 RVA: 0x00007B64 File Offset: 0x00005D64
		public static XDocument Load(Stream stream, LoadOptions options)
		{
			XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
			XDocument xdocument;
			using (XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings))
			{
				xdocument = XDocument.Load(xmlReader, options);
			}
			return xdocument;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007BA8 File Offset: 0x00005DA8
		public static async Task<XDocument> LoadAsync(Stream stream, LoadOptions options, CancellationToken cancellationToken)
		{
			XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
			xmlReaderSettings.Async = true;
			XDocument xdocument;
			using (XmlReader r = XmlReader.Create(stream, xmlReaderSettings))
			{
				xdocument = await XDocument.LoadAsync(r, options, cancellationToken).ConfigureAwait(false);
			}
			return xdocument;
		}

		/// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from a <see cref="T:System.IO.TextReader" />.</summary>
		/// <param name="textReader">A <see cref="T:System.IO.TextReader" /> that contains the content for the <see cref="T:System.Xml.Linq.XDocument" />.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> that contains the contents of the specified <see cref="T:System.IO.TextReader" />.</returns>
		// Token: 0x0600017C RID: 380 RVA: 0x00007BFB File Offset: 0x00005DFB
		public static XDocument Load(TextReader textReader)
		{
			return XDocument.Load(textReader, LoadOptions.None);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from a <see cref="T:System.IO.TextReader" />, optionally preserving white space, setting the base URI, and retaining line information.</summary>
		/// <param name="textReader">A <see cref="T:System.IO.TextReader" /> that contains the content for the <see cref="T:System.Xml.Linq.XDocument" />.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies white space behavior, and whether to load base URI and line information.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> that contains the XML that was read from the specified <see cref="T:System.IO.TextReader" />.</returns>
		// Token: 0x0600017D RID: 381 RVA: 0x00007C04 File Offset: 0x00005E04
		public static XDocument Load(TextReader textReader, LoadOptions options)
		{
			XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
			XDocument xdocument;
			using (XmlReader xmlReader = XmlReader.Create(textReader, xmlReaderSettings))
			{
				xdocument = XDocument.Load(xmlReader, options);
			}
			return xdocument;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007C48 File Offset: 0x00005E48
		public static async Task<XDocument> LoadAsync(TextReader textReader, LoadOptions options, CancellationToken cancellationToken)
		{
			XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
			xmlReaderSettings.Async = true;
			XDocument xdocument;
			using (XmlReader r = XmlReader.Create(textReader, xmlReaderSettings))
			{
				xdocument = await XDocument.LoadAsync(r, options, cancellationToken).ConfigureAwait(false);
			}
			return xdocument;
		}

		/// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from an <see cref="T:System.Xml.XmlReader" />.</summary>
		/// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> that contains the content for the <see cref="T:System.Xml.Linq.XDocument" />.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> that contains the contents of the specified <see cref="T:System.Xml.XmlReader" />.</returns>
		// Token: 0x0600017F RID: 383 RVA: 0x00007C9B File Offset: 0x00005E9B
		public static XDocument Load(XmlReader reader)
		{
			return XDocument.Load(reader, LoadOptions.None);
		}

		/// <summary>Loads an <see cref="T:System.Xml.Linq.XDocument" /> from an <see cref="T:System.Xml.XmlReader" />, optionally setting the base URI, and retaining line information.</summary>
		/// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> that will be read for the content of the <see cref="T:System.Xml.Linq.XDocument" />.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies whether to load base URI and line information.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> that contains the XML that was read from the specified <see cref="T:System.Xml.XmlReader" />.</returns>
		// Token: 0x06000180 RID: 384 RVA: 0x00007CA4 File Offset: 0x00005EA4
		public static XDocument Load(XmlReader reader, LoadOptions options)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader.ReadState == ReadState.Initial)
			{
				reader.Read();
			}
			XDocument xdocument = XDocument.InitLoad(reader, options);
			xdocument.ReadContentFrom(reader, options);
			if (!reader.EOF)
			{
				throw new InvalidOperationException("The XmlReader state should be EndOfFile after this operation.");
			}
			if (xdocument.Root == null)
			{
				throw new InvalidOperationException("The root element is missing.");
			}
			return xdocument;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007D03 File Offset: 0x00005F03
		public static Task<XDocument> LoadAsync(XmlReader reader, LoadOptions options, CancellationToken cancellationToken)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<XDocument>(cancellationToken);
			}
			return XDocument.LoadAsyncInternal(reader, options, cancellationToken);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007D2C File Offset: 0x00005F2C
		private static async Task<XDocument> LoadAsyncInternal(XmlReader reader, LoadOptions options, CancellationToken cancellationToken)
		{
			if (reader.ReadState == ReadState.Initial)
			{
				await reader.ReadAsync().ConfigureAwait(false);
			}
			XDocument d = XDocument.InitLoad(reader, options);
			await d.ReadContentFromAsync(reader, options, cancellationToken).ConfigureAwait(false);
			if (!reader.EOF)
			{
				throw new InvalidOperationException("The XmlReader state should be EndOfFile after this operation.");
			}
			if (d.Root == null)
			{
				throw new InvalidOperationException("The root element is missing.");
			}
			return d;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007D80 File Offset: 0x00005F80
		private static XDocument InitLoad(XmlReader reader, LoadOptions options)
		{
			XDocument xdocument = new XDocument();
			if ((options & LoadOptions.SetBaseUri) != LoadOptions.None)
			{
				string baseURI = reader.BaseURI;
				if (!string.IsNullOrEmpty(baseURI))
				{
					xdocument.SetBaseUri(baseURI);
				}
			}
			if ((options & LoadOptions.SetLineInfo) != LoadOptions.None)
			{
				IXmlLineInfo xmlLineInfo = reader as IXmlLineInfo;
				if (xmlLineInfo != null && xmlLineInfo.HasLineInfo())
				{
					xdocument.SetLineInfo(xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
				}
			}
			if (reader.NodeType == XmlNodeType.XmlDeclaration)
			{
				xdocument.Declaration = new XDeclaration(reader);
			}
			return xdocument;
		}

		/// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from a string.</summary>
		/// <param name="text">A string that contains XML.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> populated from the string that contains XML.</returns>
		// Token: 0x06000184 RID: 388 RVA: 0x00007DEE File Offset: 0x00005FEE
		public static XDocument Parse(string text)
		{
			return XDocument.Parse(text, LoadOptions.None);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from a string, optionally preserving white space, setting the base URI, and retaining line information.</summary>
		/// <param name="text">A string that contains XML.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies white space behavior, and whether to load base URI and line information.</param>
		/// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> populated from the string that contains XML.</returns>
		// Token: 0x06000185 RID: 389 RVA: 0x00007DF8 File Offset: 0x00005FF8
		public static XDocument Parse(string text, LoadOptions options)
		{
			XDocument xdocument;
			using (StringReader stringReader = new StringReader(text))
			{
				XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
				using (XmlReader xmlReader = XmlReader.Create(stringReader, xmlReaderSettings))
				{
					xdocument = XDocument.Load(xmlReader, options);
				}
			}
			return xdocument;
		}

		/// <summary>Outputs this <see cref="T:System.Xml.Linq.XDocument" /> to the specified <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="stream">The stream to output this <see cref="T:System.Xml.Linq.XDocument" /> to.</param>
		// Token: 0x06000186 RID: 390 RVA: 0x00007E58 File Offset: 0x00006058
		public void Save(Stream stream)
		{
			this.Save(stream, base.GetSaveOptionsFromAnnotations());
		}

		/// <summary>Outputs this <see cref="T:System.Xml.Linq.XDocument" /> to the specified <see cref="T:System.IO.Stream" />, optionally specifying formatting behavior.</summary>
		/// <param name="stream">The stream to output this <see cref="T:System.Xml.Linq.XDocument" /> to.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> that specifies formatting behavior.</param>
		// Token: 0x06000187 RID: 391 RVA: 0x00007E68 File Offset: 0x00006068
		public void Save(Stream stream, SaveOptions options)
		{
			XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
			if (this._declaration != null && !string.IsNullOrEmpty(this._declaration.Encoding))
			{
				try
				{
					xmlWriterSettings.Encoding = Encoding.GetEncoding(this._declaration.Encoding);
				}
				catch (ArgumentException)
				{
				}
			}
			using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
			{
				this.Save(xmlWriter);
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007EE8 File Offset: 0x000060E8
		public async Task SaveAsync(Stream stream, SaveOptions options, CancellationToken cancellationToken)
		{
			XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
			xmlWriterSettings.Async = true;
			if (this._declaration != null && !string.IsNullOrEmpty(this._declaration.Encoding))
			{
				try
				{
					xmlWriterSettings.Encoding = Encoding.GetEncoding(this._declaration.Encoding);
				}
				catch (ArgumentException)
				{
				}
			}
			using (XmlWriter w = XmlWriter.Create(stream, xmlWriterSettings))
			{
				await this.WriteToAsync(w, cancellationToken).ConfigureAwait(false);
			}
			XmlWriter w = null;
		}

		/// <summary>Serialize this <see cref="T:System.Xml.Linq.XDocument" /> to a <see cref="T:System.IO.TextWriter" />.</summary>
		/// <param name="textWriter">A <see cref="T:System.IO.TextWriter" /> that the <see cref="T:System.Xml.Linq.XDocument" /> will be written to.</param>
		// Token: 0x06000189 RID: 393 RVA: 0x00007F43 File Offset: 0x00006143
		public void Save(TextWriter textWriter)
		{
			this.Save(textWriter, base.GetSaveOptionsFromAnnotations());
		}

		/// <summary>Serialize this <see cref="T:System.Xml.Linq.XDocument" /> to a <see cref="T:System.IO.TextWriter" />, optionally disabling formatting.</summary>
		/// <param name="textWriter">The <see cref="T:System.IO.TextWriter" /> to output the XML to.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> that specifies formatting behavior.</param>
		// Token: 0x0600018A RID: 394 RVA: 0x00007F54 File Offset: 0x00006154
		public void Save(TextWriter textWriter, SaveOptions options)
		{
			XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
			using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, xmlWriterSettings))
			{
				this.Save(xmlWriter);
			}
		}

		/// <summary>Serialize this <see cref="T:System.Xml.Linq.XDocument" /> to an <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="writer">A <see cref="T:System.Xml.XmlWriter" /> that the <see cref="T:System.Xml.Linq.XDocument" /> will be written to.</param>
		// Token: 0x0600018B RID: 395 RVA: 0x00007F94 File Offset: 0x00006194
		public void Save(XmlWriter writer)
		{
			this.WriteTo(writer);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00007FA0 File Offset: 0x000061A0
		public async Task SaveAsync(TextWriter textWriter, SaveOptions options, CancellationToken cancellationToken)
		{
			XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
			xmlWriterSettings.Async = true;
			using (XmlWriter w = XmlWriter.Create(textWriter, xmlWriterSettings))
			{
				await this.WriteToAsync(w, cancellationToken).ConfigureAwait(false);
			}
			XmlWriter w = null;
		}

		/// <summary>Serialize this <see cref="T:System.Xml.Linq.XDocument" /> to a file, overwriting an existing file, if it exists.</summary>
		/// <param name="fileName">A string that contains the name of the file.</param>
		// Token: 0x0600018D RID: 397 RVA: 0x00007FFB File Offset: 0x000061FB
		public void Save(string fileName)
		{
			this.Save(fileName, base.GetSaveOptionsFromAnnotations());
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000800A File Offset: 0x0000620A
		public Task SaveAsync(XmlWriter writer, CancellationToken cancellationToken)
		{
			return this.WriteToAsync(writer, cancellationToken);
		}

		/// <summary>Serialize this <see cref="T:System.Xml.Linq.XDocument" /> to a file, optionally disabling formatting.</summary>
		/// <param name="fileName">A string that contains the name of the file.</param>
		/// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> that specifies formatting behavior.</param>
		// Token: 0x0600018F RID: 399 RVA: 0x00008014 File Offset: 0x00006214
		public void Save(string fileName, SaveOptions options)
		{
			XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
			if (this._declaration != null && !string.IsNullOrEmpty(this._declaration.Encoding))
			{
				try
				{
					xmlWriterSettings.Encoding = Encoding.GetEncoding(this._declaration.Encoding);
				}
				catch (ArgumentException)
				{
				}
			}
			using (XmlWriter xmlWriter = XmlWriter.Create(fileName, xmlWriterSettings))
			{
				this.Save(xmlWriter);
			}
		}

		/// <summary>Write this document to an <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="writer">An <see cref="T:System.Xml.XmlWriter" /> into which this method will write.</param>
		// Token: 0x06000190 RID: 400 RVA: 0x00008094 File Offset: 0x00006294
		public override void WriteTo(XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (this._declaration != null && this._declaration.Standalone == "yes")
			{
				writer.WriteStartDocument(true);
			}
			else if (this._declaration != null && this._declaration.Standalone == "no")
			{
				writer.WriteStartDocument(false);
			}
			else
			{
				writer.WriteStartDocument();
			}
			base.WriteContentTo(writer);
			writer.WriteEndDocument();
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008112 File Offset: 0x00006312
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
			return this.WriteToAsyncInternal(writer, cancellationToken);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000813C File Offset: 0x0000633C
		private async Task WriteToAsyncInternal(XmlWriter writer, CancellationToken cancellationToken)
		{
			Task task;
			if (this._declaration != null && this._declaration.Standalone == "yes")
			{
				task = writer.WriteStartDocumentAsync(true);
			}
			else if (this._declaration != null && this._declaration.Standalone == "no")
			{
				task = writer.WriteStartDocumentAsync(false);
			}
			else
			{
				task = writer.WriteStartDocumentAsync();
			}
			await task.ConfigureAwait(false);
			await base.WriteContentToAsync(writer, cancellationToken).ConfigureAwait(false);
			await writer.WriteEndDocumentAsync().ConfigureAwait(false);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000818F File Offset: 0x0000638F
		internal override void AddAttribute(XAttribute a)
		{
			throw new ArgumentException("An attribute cannot be added to content.");
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000818F File Offset: 0x0000638F
		internal override void AddAttributeSkipNotify(XAttribute a)
		{
			throw new ArgumentException("An attribute cannot be added to content.");
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000819B File Offset: 0x0000639B
		internal override XNode CloneNode()
		{
			return new XDocument(this);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000081A4 File Offset: 0x000063A4
		internal override bool DeepEquals(XNode node)
		{
			XDocument xdocument = node as XDocument;
			return xdocument != null && base.ContentsEqual(xdocument);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000081C4 File Offset: 0x000063C4
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000081CC File Offset: 0x000063CC
		private T GetFirstNode<T>() where T : XNode
		{
			XNode xnode = this.content as XNode;
			if (xnode != null)
			{
				T t;
				for (;;)
				{
					xnode = xnode.next;
					t = xnode as T;
					if (t != null)
					{
						break;
					}
					if (xnode == this.content)
					{
						goto IL_0035;
					}
				}
				return t;
			}
			IL_0035:
			return default(T);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00008218 File Offset: 0x00006418
		internal static bool IsWhitespace(string s)
		{
			foreach (char c in s)
			{
				if (c != ' ' && c != '\t' && c != '\r' && c != '\n')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008258 File Offset: 0x00006458
		internal override void ValidateNode(XNode node, XNode previous)
		{
			XmlNodeType nodeType = node.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Element:
				this.ValidateDocument(previous, XmlNodeType.DocumentType, XmlNodeType.None);
				return;
			case XmlNodeType.Attribute:
				return;
			case XmlNodeType.Text:
				this.ValidateString(((XText)node).Value);
				return;
			case XmlNodeType.CDATA:
				throw new ArgumentException(global::SR.Format("A node of type {0} cannot be added to content.", XmlNodeType.CDATA));
			default:
				if (nodeType == XmlNodeType.Document)
				{
					throw new ArgumentException(global::SR.Format("A node of type {0} cannot be added to content.", XmlNodeType.Document));
				}
				if (nodeType != XmlNodeType.DocumentType)
				{
					return;
				}
				this.ValidateDocument(previous, XmlNodeType.None, XmlNodeType.Element);
				return;
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000082E4 File Offset: 0x000064E4
		private void ValidateDocument(XNode previous, XmlNodeType allowBefore, XmlNodeType allowAfter)
		{
			XNode xnode = this.content as XNode;
			if (xnode != null)
			{
				if (previous == null)
				{
					allowBefore = allowAfter;
				}
				for (;;)
				{
					xnode = xnode.next;
					XmlNodeType nodeType = xnode.NodeType;
					if (nodeType == XmlNodeType.Element || nodeType == XmlNodeType.DocumentType)
					{
						if (nodeType != allowBefore)
						{
							break;
						}
						allowBefore = XmlNodeType.None;
					}
					if (xnode == previous)
					{
						allowBefore = allowAfter;
					}
					if (xnode == this.content)
					{
						return;
					}
				}
				throw new InvalidOperationException("This operation would create an incorrectly structured document.");
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000833F File Offset: 0x0000653F
		internal override void ValidateString(string s)
		{
			if (!XDocument.IsWhitespace(s))
			{
				throw new ArgumentException("Non-whitespace characters cannot be added to content.");
			}
		}

		// Token: 0x040000C8 RID: 200
		private XDeclaration _declaration;
	}
}
