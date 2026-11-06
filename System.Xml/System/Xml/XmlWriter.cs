using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	/// <summary>Represents a writer that provides a fast, non-cached, forward-only way to generate streams or files that contain XML data.</summary>
	// Token: 0x02000179 RID: 377
	public abstract class XmlWriter : IDisposable
	{
		/// <summary>Gets the <see cref="T:System.Xml.XmlWriterSettings" /> object used to create this <see cref="T:System.Xml.XmlWriter" /> instance.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlWriterSettings" /> object used to create this writer instance. If this writer was not created using the <see cref="Overload:System.Xml.XmlWriter.Create" /> method, this property returns <see langword="null" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000D47 RID: 3399 RVA: 0x0001DA42 File Offset: 0x0001BC42
		public virtual XmlWriterSettings Settings
		{
			get
			{
				return null;
			}
		}

		/// <summary>When overridden in a derived class, writes the XML declaration with the version "1.0".</summary>
		/// <exception cref="T:System.InvalidOperationException">This is not the first write method called after the constructor.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D48 RID: 3400
		public abstract void WriteStartDocument();

		/// <summary>When overridden in a derived class, writes the XML declaration with the version "1.0" and the standalone attribute.</summary>
		/// <param name="standalone">If <see langword="true" />, it writes "standalone=yes"; if <see langword="false" />, it writes "standalone=no".</param>
		/// <exception cref="T:System.InvalidOperationException">This is not the first write method called after the constructor. </exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D49 RID: 3401
		public abstract void WriteStartDocument(bool standalone);

		/// <summary>When overridden in a derived class, closes any open elements or attributes and puts the writer back in the Start state.</summary>
		/// <exception cref="T:System.ArgumentException">The XML document is invalid.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D4A RID: 3402
		public abstract void WriteEndDocument();

		/// <summary>When overridden in a derived class, writes the DOCTYPE declaration with the specified name and optional attributes.</summary>
		/// <param name="name">The name of the DOCTYPE. This must be non-empty.</param>
		/// <param name="pubid">If non-null it also writes PUBLIC "pubid" "sysid" where <paramref name="pubid" /> and <paramref name="sysid" /> are replaced with the value of the given arguments.</param>
		/// <param name="sysid">If <paramref name="pubid" /> is <see langword="null" /> and <paramref name="sysid" /> is non-null it writes SYSTEM "sysid" where <paramref name="sysid" /> is replaced with the value of this argument.</param>
		/// <param name="subset">If non-null it writes [subset] where subset is replaced with the value of this argument.</param>
		/// <exception cref="T:System.InvalidOperationException">This method was called outside the prolog (after the root element). </exception>
		/// <exception cref="T:System.ArgumentException">The value for <paramref name="name" /> would result in invalid XML.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D4B RID: 3403
		public abstract void WriteDocType(string name, string pubid, string sysid, string subset);

		/// <summary>When overridden in a derived class, writes the specified start tag and associates it with the given namespace.</summary>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="ns">The namespace URI to associate with the element. If this namespace is already in scope and has an associated prefix, the writer automatically writes that prefix also.</param>
		/// <exception cref="T:System.InvalidOperationException">The writer is closed.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">There is a character in the buffer that is a valid XML character but is not valid for the output encoding. For example, if the output encoding is ASCII, you should only use characters from the range of 0 to 127 for element and attribute names. The invalid character might be in the argument of this method or in an argument of previous methods that were writing to the buffer. Such characters are escaped by character entity references when possible (for example, in text nodes or attribute values). However, the character entity reference is not allowed in element and attribute names, comments, processing instructions, or CDATA sections.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D4C RID: 3404 RVA: 0x00057E6E File Offset: 0x0005606E
		public void WriteStartElement(string localName, string ns)
		{
			this.WriteStartElement(null, localName, ns);
		}

		/// <summary>When overridden in a derived class, writes the specified start tag and associates it with the given namespace and prefix.</summary>
		/// <param name="prefix">The namespace prefix of the element.</param>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="ns">The namespace URI to associate with the element.</param>
		/// <exception cref="T:System.InvalidOperationException">The writer is closed.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">There is a character in the buffer that is a valid XML character but is not valid for the output encoding. For example, if the output encoding is ASCII, you should only use characters from the range of 0 to 127 for element and attribute names. The invalid character might be in the argument of this method or in an argument of previous methods that were writing to the buffer. Such characters are escaped by character entity references when possible (for example, in text nodes or attribute values). However, the character entity reference is not allowed in element and attribute names, comments, processing instructions, or CDATA sections.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D4D RID: 3405
		public abstract void WriteStartElement(string prefix, string localName, string ns);

		/// <summary>When overridden in a derived class, writes out a start tag with the specified local name.</summary>
		/// <param name="localName">The local name of the element.</param>
		/// <exception cref="T:System.InvalidOperationException">The writer is closed.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">There is a character in the buffer that is a valid XML character but is not valid for the output encoding. For example, if the output encoding is ASCII, you should only use characters from the range of 0 to 127 for element and attribute names. The invalid character might be in the argument of this method or in an argument of previous methods that were writing to the buffer. Such characters are escaped by character entity references when possible (for example, in text nodes or attribute values). However, the character entity reference is not allowed in element and attribute names, comments, processing instructions, or CDATA sections. </exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D4E RID: 3406 RVA: 0x00057E79 File Offset: 0x00056079
		public void WriteStartElement(string localName)
		{
			this.WriteStartElement(null, localName, null);
		}

		/// <summary>When overridden in a derived class, closes one element and pops the corresponding namespace scope.</summary>
		/// <exception cref="T:System.InvalidOperationException">This results in an invalid XML document.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D4F RID: 3407
		public abstract void WriteEndElement();

		/// <summary>When overridden in a derived class, closes one element and pops the corresponding namespace scope.</summary>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D50 RID: 3408
		public abstract void WriteFullEndElement();

		/// <summary>When overridden in a derived class, writes an attribute with the specified local name, namespace URI, and value.</summary>
		/// <param name="localName">The local name of the attribute.</param>
		/// <param name="ns">The namespace URI to associate with the attribute.</param>
		/// <param name="value">The value of the attribute.</param>
		/// <exception cref="T:System.InvalidOperationException">The state of writer is not <see langword="WriteState.Element" /> or writer is closed. </exception>
		/// <exception cref="T:System.ArgumentException">The <see langword="xml:space" /> or <see langword="xml:lang" /> attribute value is invalid. </exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D51 RID: 3409 RVA: 0x00057E84 File Offset: 0x00056084
		public void WriteAttributeString(string localName, string ns, string value)
		{
			this.WriteStartAttribute(null, localName, ns);
			this.WriteString(value);
			this.WriteEndAttribute();
		}

		/// <summary>When overridden in a derived class, writes out the attribute with the specified local name and value.</summary>
		/// <param name="localName">The local name of the attribute.</param>
		/// <param name="value">The value of the attribute.</param>
		/// <exception cref="T:System.InvalidOperationException">The state of writer is not <see langword="WriteState.Element" /> or writer is closed. </exception>
		/// <exception cref="T:System.ArgumentException">The <see langword="xml:space" /> or <see langword="xml:lang" /> attribute value is invalid. </exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D52 RID: 3410 RVA: 0x00057E9C File Offset: 0x0005609C
		public void WriteAttributeString(string localName, string value)
		{
			this.WriteStartAttribute(null, localName, null);
			this.WriteString(value);
			this.WriteEndAttribute();
		}

		/// <summary>When overridden in a derived class, writes out the attribute with the specified prefix, local name, namespace URI, and value.</summary>
		/// <param name="prefix">The namespace prefix of the attribute.</param>
		/// <param name="localName">The local name of the attribute.</param>
		/// <param name="ns">The namespace URI of the attribute.</param>
		/// <param name="value">The value of the attribute.</param>
		/// <exception cref="T:System.InvalidOperationException">The state of writer is not <see langword="WriteState.Element" /> or writer is closed. </exception>
		/// <exception cref="T:System.ArgumentException">The <see langword="xml:space" /> or <see langword="xml:lang" /> attribute value is invalid. </exception>
		/// <exception cref="T:System.Xml.XmlException">The <paramref name="localName" /> or <paramref name="ns" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D53 RID: 3411 RVA: 0x00057EB4 File Offset: 0x000560B4
		public void WriteAttributeString(string prefix, string localName, string ns, string value)
		{
			this.WriteStartAttribute(prefix, localName, ns);
			this.WriteString(value);
			this.WriteEndAttribute();
		}

		/// <summary>Writes the start of an attribute with the specified local name and namespace URI.</summary>
		/// <param name="localName">The local name of the attribute.</param>
		/// <param name="ns">The namespace URI of the attribute.</param>
		/// <exception cref="T:System.Text.EncoderFallbackException">There is a character in the buffer that is a valid XML character but is not valid for the output encoding. For example, if the output encoding is ASCII, you should only use characters from the range of 0 to 127 for element and attribute names. The invalid character might be in the argument of this method or in an argument of previous methods that were writing to the buffer. Such characters are escaped by character entity references when possible (for example, in text nodes or attribute values). However, the character entity reference is not allowed in element and attribute names, comments, processing instructions, or CDATA sections.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D54 RID: 3412 RVA: 0x00057ECD File Offset: 0x000560CD
		public void WriteStartAttribute(string localName, string ns)
		{
			this.WriteStartAttribute(null, localName, ns);
		}

		/// <summary>When overridden in a derived class, writes the start of an attribute with the specified prefix, local name, and namespace URI.</summary>
		/// <param name="prefix">The namespace prefix of the attribute.</param>
		/// <param name="localName">The local name of the attribute.</param>
		/// <param name="ns">The namespace URI for the attribute.</param>
		/// <exception cref="T:System.Text.EncoderFallbackException">There is a character in the buffer that is a valid XML character but is not valid for the output encoding. For example, if the output encoding is ASCII, you should only use characters from the range of 0 to 127 for element and attribute names. The invalid character might be in the argument of this method or in an argument of previous methods that were writing to the buffer. Such characters are escaped by character entity references when possible (for example, in text nodes or attribute values). However, the character entity reference is not allowed in element and attribute names, comments, processing instructions, or CDATA sections. </exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D55 RID: 3413
		public abstract void WriteStartAttribute(string prefix, string localName, string ns);

		/// <summary>Writes the start of an attribute with the specified local name.</summary>
		/// <param name="localName">The local name of the attribute.</param>
		/// <exception cref="T:System.InvalidOperationException">The writer is closed.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">There is a character in the buffer that is a valid XML character but is not valid for the output encoding. For example, if the output encoding is ASCII, you should only use characters from the range of 0 to 127 for element and attribute names. The invalid character might be in the argument of this method or in an argument of previous methods that were writing to the buffer. Such characters are escaped by character entity references when possible (for example, in text nodes or attribute values). However, the character entity reference is not allowed in element and attribute names, comments, processing instructions, or CDATA sections.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D56 RID: 3414 RVA: 0x00057ED8 File Offset: 0x000560D8
		public void WriteStartAttribute(string localName)
		{
			this.WriteStartAttribute(null, localName, null);
		}

		/// <summary>When overridden in a derived class, closes the previous <see cref="M:System.Xml.XmlWriter.WriteStartAttribute(System.String,System.String)" /> call.</summary>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D57 RID: 3415
		public abstract void WriteEndAttribute();

		/// <summary>When overridden in a derived class, writes out a &lt;![CDATA[...]]&gt; block containing the specified text.</summary>
		/// <param name="text">The text to place inside the CDATA block.</param>
		/// <exception cref="T:System.ArgumentException">The text would result in a non-well formed XML document.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D58 RID: 3416
		public abstract void WriteCData(string text);

		/// <summary>When overridden in a derived class, writes out a comment &lt;!--...--&gt; containing the specified text.</summary>
		/// <param name="text">Text to place inside the comment.</param>
		/// <exception cref="T:System.ArgumentException">The text would result in a non-well-formed XML document.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D59 RID: 3417
		public abstract void WriteComment(string text);

		/// <summary>When overridden in a derived class, writes out a processing instruction with a space between the name and text as follows: &lt;?name text?&gt;.</summary>
		/// <param name="name">The name of the processing instruction.</param>
		/// <param name="text">The text to include in the processing instruction.</param>
		/// <exception cref="T:System.ArgumentException">The text would result in a non-well formed XML document.
		///         <paramref name="name" /> is either <see langword="null" /> or <see langword="String.Empty" />.This method is being used to create an XML declaration after <see cref="M:System.Xml.XmlWriter.WriteStartDocument" /> has already been called. </exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D5A RID: 3418
		public abstract void WriteProcessingInstruction(string name, string text);

		/// <summary>When overridden in a derived class, writes out an entity reference as <see langword="&amp;name;" />.</summary>
		/// <param name="name">The name of the entity reference.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="name" /> is either <see langword="null" /> or <see langword="String.Empty" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D5B RID: 3419
		public abstract void WriteEntityRef(string name);

		/// <summary>When overridden in a derived class, forces the generation of a character entity for the specified Unicode character value.</summary>
		/// <param name="ch">The Unicode character for which to generate a character entity.</param>
		/// <exception cref="T:System.ArgumentException">The character is in the surrogate pair character range, <see langword="0xd800" /> - <see langword="0xdfff" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D5C RID: 3420
		public abstract void WriteCharEntity(char ch);

		/// <summary>When overridden in a derived class, writes out the given white space.</summary>
		/// <param name="ws">The string of white space characters.</param>
		/// <exception cref="T:System.ArgumentException">The string contains non-white space characters.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D5D RID: 3421
		public abstract void WriteWhitespace(string ws);

		/// <summary>When overridden in a derived class, writes the given text content.</summary>
		/// <param name="text">The text to write.</param>
		/// <exception cref="T:System.ArgumentException">The text string contains an invalid surrogate pair.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D5E RID: 3422
		public abstract void WriteString(string text);

		/// <summary>When overridden in a derived class, generates and writes the surrogate character entity for the surrogate character pair.</summary>
		/// <param name="lowChar">The low surrogate. This must be a value between 0xDC00 and 0xDFFF.</param>
		/// <param name="highChar">The high surrogate. This must be a value between 0xD800 and 0xDBFF.</param>
		/// <exception cref="T:System.ArgumentException">An invalid surrogate character pair was passed.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D5F RID: 3423
		public abstract void WriteSurrogateCharEntity(char lowChar, char highChar);

		/// <summary>When overridden in a derived class, writes text one buffer at a time.</summary>
		/// <param name="buffer">Character array containing the text to write.</param>
		/// <param name="index">The position in the buffer indicating the start of the text to write.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> or <paramref name="count" /> is less than zero.-or-The buffer length minus <paramref name="index" /> is less than <paramref name="count" />; the call results in surrogate pair characters being split or an invalid surrogate pair being written.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="buffer" /> parameter value is not valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D60 RID: 3424
		public abstract void WriteChars(char[] buffer, int index, int count);

		/// <summary>When overridden in a derived class, writes raw markup manually from a character buffer.</summary>
		/// <param name="buffer">Character array containing the text to write.</param>
		/// <param name="index">The position within the buffer indicating the start of the text to write.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> or <paramref name="count" /> is less than zero. -or-The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D61 RID: 3425
		public abstract void WriteRaw(char[] buffer, int index, int count);

		/// <summary>When overridden in a derived class, writes raw markup manually from a string.</summary>
		/// <param name="data">String containing the text to write.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="data" /> is either <see langword="null" /> or <see langword="String.Empty" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D62 RID: 3426
		public abstract void WriteRaw(string data);

		/// <summary>When overridden in a derived class, encodes the specified binary bytes as Base64 and writes out the resulting text.</summary>
		/// <param name="buffer">Byte array to encode.</param>
		/// <param name="index">The position in the buffer indicating the start of the bytes to write.</param>
		/// <param name="count">The number of bytes to write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="buffer" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> or <paramref name="count" /> is less than zero. -or-The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D63 RID: 3427
		public abstract void WriteBase64(byte[] buffer, int index, int count);

		/// <summary>When overridden in a derived class, encodes the specified binary bytes as <see langword="BinHex" /> and writes out the resulting text.</summary>
		/// <param name="buffer">Byte array to encode.</param>
		/// <param name="index">The position in the buffer indicating the start of the bytes to write.</param>
		/// <param name="count">The number of bytes to write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The writer is closed or in error state.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> or <paramref name="count" /> is less than zero. -or-The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D64 RID: 3428 RVA: 0x00057EE3 File Offset: 0x000560E3
		public virtual void WriteBinHex(byte[] buffer, int index, int count)
		{
			BinHexEncoder.Encode(buffer, index, count, this);
		}

		/// <summary>When overridden in a derived class, gets the state of the writer.</summary>
		/// <returns>One of the <see cref="T:System.Xml.WriteState" /> values.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000D65 RID: 3429
		public abstract WriteState WriteState { get; }

		/// <summary>When overridden in a derived class, closes this stream and the underlying stream.</summary>
		/// <exception cref="T:System.InvalidOperationException">A call is made to write more output after <see langword="Close" /> has been called or the result of this call is an invalid XML document.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D66 RID: 3430 RVA: 0x0000B528 File Offset: 0x00009728
		public virtual void Close()
		{
		}

		/// <summary>When overridden in a derived class, flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.</summary>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D67 RID: 3431
		public abstract void Flush();

		/// <summary>When overridden in a derived class, returns the closest prefix defined in the current namespace scope for the namespace URI.</summary>
		/// <param name="ns">The namespace URI whose prefix you want to find.</param>
		/// <returns>The matching prefix or <see langword="null" /> if no matching namespace URI is found in the current scope.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="ns" /> is either <see langword="null" /> or <see langword="String.Empty" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D68 RID: 3432
		public abstract string LookupPrefix(string ns);

		/// <summary>When overridden in a derived class, gets an <see cref="T:System.Xml.XmlSpace" /> representing the current <see langword="xml:space" /> scope.</summary>
		/// <returns>An <see langword="XmlSpace" /> representing the current <see langword="xml:space" /> scope.Value Meaning 
		///             <see langword="None" />
		///           This is the default if no <see langword="xml:space" /> scope exists.
		///             <see langword="Default" />
		///           The current scope is <see langword="xml:space" />="default".
		///             <see langword="Preserve" />
		///           The current scope is <see langword="xml:space" />="preserve".</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000D69 RID: 3433 RVA: 0x0001222F File Offset: 0x0001042F
		public virtual XmlSpace XmlSpace
		{
			get
			{
				return XmlSpace.Default;
			}
		}

		/// <summary>When overridden in a derived class, gets the current <see langword="xml:lang" /> scope.</summary>
		/// <returns>The current <see langword="xml:lang" /> scope.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x0001E51E File Offset: 0x0001C71E
		public virtual string XmlLang
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>When overridden in a derived class, writes out the specified name, ensuring it is a valid NmToken according to the W3C XML 1.0 recommendation (http://www.w3.org/TR/1998/REC-xml-19980210#NT-Name).</summary>
		/// <param name="name">The name to write.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="name" /> is not a valid NmToken; or <paramref name="name" /> is either <see langword="null" /> or <see langword="String.Empty" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D6B RID: 3435 RVA: 0x00057EEE File Offset: 0x000560EE
		public virtual void WriteNmToken(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
			}
			this.WriteString(XmlConvert.VerifyNMTOKEN(name, ExceptionType.ArgumentException));
		}

		/// <summary>When overridden in a derived class, writes out the specified name, ensuring it is a valid name according to the W3C XML 1.0 recommendation (http://www.w3.org/TR/1998/REC-xml-19980210#NT-Name).</summary>
		/// <param name="name">The name to write.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="name" /> is not a valid XML name; or <paramref name="name" /> is either <see langword="null" /> or <see langword="String.Empty" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D6C RID: 3436 RVA: 0x00057F18 File Offset: 0x00056118
		public virtual void WriteName(string name)
		{
			this.WriteString(XmlConvert.VerifyQName(name, ExceptionType.ArgumentException));
		}

		/// <summary>When overridden in a derived class, writes out the namespace-qualified name. This method looks up the prefix that is in scope for the given namespace.</summary>
		/// <param name="localName">The local name to write.</param>
		/// <param name="ns">The namespace URI for the name.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="localName" /> is either <see langword="null" /> or <see langword="String.Empty" />.
		///         <paramref name="localName" /> is not a valid name. </exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D6D RID: 3437 RVA: 0x00057F28 File Offset: 0x00056128
		public virtual void WriteQualifiedName(string localName, string ns)
		{
			if (ns != null && ns.Length > 0)
			{
				string text = this.LookupPrefix(ns);
				if (text == null)
				{
					throw new ArgumentException(Res.GetString("The '{0}' namespace is not defined.", new object[] { ns }));
				}
				this.WriteString(text);
				this.WriteString(":");
			}
			this.WriteString(localName);
		}

		/// <summary>Writes the object value.</summary>
		/// <param name="value">The object value to write.
		///       Note   With the release of the .NET Framework 3.5, this method accepts <see cref="T:System.DateTimeOffset" /> as a parameter.</param>
		/// <exception cref="T:System.ArgumentException">An invalid value was specified.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The writer is closed or in error state.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D6E RID: 3438 RVA: 0x00057F7F File Offset: 0x0005617F
		public virtual void WriteValue(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value, null));
		}

		/// <summary>Writes a <see cref="T:System.String" /> value.</summary>
		/// <param name="value">The <see cref="T:System.String" /> value to write.</param>
		/// <exception cref="T:System.ArgumentException">An invalid value was specified.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D6F RID: 3439 RVA: 0x00057FA1 File Offset: 0x000561A1
		public virtual void WriteValue(string value)
		{
			if (value == null)
			{
				return;
			}
			this.WriteString(value);
		}

		/// <summary>Writes a <see cref="T:System.Boolean" /> value.</summary>
		/// <param name="value">The <see cref="T:System.Boolean" /> value to write.</param>
		/// <exception cref="T:System.ArgumentException">An invalid value was specified.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D70 RID: 3440 RVA: 0x00057FAE File Offset: 0x000561AE
		public virtual void WriteValue(bool value)
		{
			this.WriteString(XmlConvert.ToString(value));
		}

		/// <summary>Writes a <see cref="T:System.DateTime" /> value.</summary>
		/// <param name="value">The <see cref="T:System.DateTime" /> value to write.</param>
		/// <exception cref="T:System.ArgumentException">An invalid value was specified.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D71 RID: 3441 RVA: 0x00057FBC File Offset: 0x000561BC
		public virtual void WriteValue(DateTime value)
		{
			this.WriteString(XmlConvert.ToString(value, XmlDateTimeSerializationMode.RoundtripKind));
		}

		/// <summary>Writes a <see cref="T:System.DateTimeOffset" /> value.</summary>
		/// <param name="value">The <see cref="T:System.DateTimeOffset" /> value to write.</param>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D72 RID: 3442 RVA: 0x00057FCB File Offset: 0x000561CB
		public virtual void WriteValue(DateTimeOffset value)
		{
			if (value.Offset != TimeSpan.Zero)
			{
				this.WriteValue(value.LocalDateTime);
				return;
			}
			this.WriteValue(value.UtcDateTime);
		}

		/// <summary>Writes a <see cref="T:System.Double" /> value.</summary>
		/// <param name="value">The <see cref="T:System.Double" /> value to write.</param>
		/// <exception cref="T:System.ArgumentException">An invalid value was specified.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D73 RID: 3443 RVA: 0x00057FFB File Offset: 0x000561FB
		public virtual void WriteValue(double value)
		{
			this.WriteString(XmlConvert.ToString(value));
		}

		/// <summary>Writes a single-precision floating-point number.</summary>
		/// <param name="value">The single-precision floating-point number to write.</param>
		/// <exception cref="T:System.ArgumentException">An invalid value was specified.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D74 RID: 3444 RVA: 0x00058009 File Offset: 0x00056209
		public virtual void WriteValue(float value)
		{
			this.WriteString(XmlConvert.ToString(value));
		}

		/// <summary>Writes a <see cref="T:System.Decimal" /> value.</summary>
		/// <param name="value">The <see cref="T:System.Decimal" /> value to write.</param>
		/// <exception cref="T:System.ArgumentException">An invalid value was specified.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D75 RID: 3445 RVA: 0x00058017 File Offset: 0x00056217
		public virtual void WriteValue(decimal value)
		{
			this.WriteString(XmlConvert.ToString(value));
		}

		/// <summary>Writes a <see cref="T:System.Int32" /> value.</summary>
		/// <param name="value">The <see cref="T:System.Int32" /> value to write.</param>
		/// <exception cref="T:System.ArgumentException">An invalid value was specified.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D76 RID: 3446 RVA: 0x00058025 File Offset: 0x00056225
		public virtual void WriteValue(int value)
		{
			this.WriteString(XmlConvert.ToString(value));
		}

		/// <summary>Writes a <see cref="T:System.Int64" /> value.</summary>
		/// <param name="value">The <see cref="T:System.Int64" /> value to write.</param>
		/// <exception cref="T:System.ArgumentException">An invalid value was specified.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D77 RID: 3447 RVA: 0x00058033 File Offset: 0x00056233
		public virtual void WriteValue(long value)
		{
			this.WriteString(XmlConvert.ToString(value));
		}

		/// <summary>When overridden in a derived class, writes out all the attributes found at the current position in the <see cref="T:System.Xml.XmlReader" />.</summary>
		/// <param name="reader">The <see langword="XmlReader" /> from which to copy the attributes.</param>
		/// <param name="defattr">
		///       <see langword="true" /> to copy the default attributes from the <see langword="XmlReader" />; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="reader" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.Xml.XmlException">The reader is not positioned on an <see langword="element" />, <see langword="attribute" /> or <see langword="XmlDeclaration" /> node. </exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D78 RID: 3448 RVA: 0x00058044 File Offset: 0x00056244
		public virtual void WriteAttributes(XmlReader reader, bool defattr)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader.NodeType == XmlNodeType.Element || reader.NodeType == XmlNodeType.XmlDeclaration)
			{
				if (reader.MoveToFirstAttribute())
				{
					this.WriteAttributes(reader, defattr);
					reader.MoveToElement();
					return;
				}
			}
			else
			{
				if (reader.NodeType != XmlNodeType.Attribute)
				{
					throw new XmlException("The current position on the Reader is neither an element nor an attribute.", string.Empty);
				}
				do
				{
					if (defattr || !reader.IsDefaultInternal)
					{
						this.WriteStartAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);
						while (reader.ReadAttributeValue())
						{
							if (reader.NodeType == XmlNodeType.EntityReference)
							{
								this.WriteEntityRef(reader.Name);
							}
							else
							{
								this.WriteString(reader.Value);
							}
						}
						this.WriteEndAttribute();
					}
				}
				while (reader.MoveToNextAttribute());
			}
		}

		/// <summary>When overridden in a derived class, copies everything from the reader to the writer and moves the reader to the start of the next sibling.</summary>
		/// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> to read from.</param>
		/// <param name="defattr">
		///       <see langword="true" /> to copy the default attributes from the <see langword="XmlReader" />; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="reader" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="reader" /> contains invalid characters.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D79 RID: 3449 RVA: 0x00058104 File Offset: 0x00056304
		public virtual void WriteNode(XmlReader reader, bool defattr)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			bool canReadValueChunk = reader.CanReadValueChunk;
			int num = ((reader.NodeType == XmlNodeType.None) ? (-1) : reader.Depth);
			do
			{
				switch (reader.NodeType)
				{
				case XmlNodeType.Element:
					this.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
					this.WriteAttributes(reader, defattr);
					if (reader.IsEmptyElement)
					{
						this.WriteEndElement();
					}
					break;
				case XmlNodeType.Text:
					if (canReadValueChunk)
					{
						if (this.writeNodeBuffer == null)
						{
							this.writeNodeBuffer = new char[1024];
						}
						int num2;
						while ((num2 = reader.ReadValueChunk(this.writeNodeBuffer, 0, 1024)) > 0)
						{
							this.WriteChars(this.writeNodeBuffer, 0, num2);
						}
					}
					else
					{
						this.WriteString(reader.Value);
					}
					break;
				case XmlNodeType.CDATA:
					this.WriteCData(reader.Value);
					break;
				case XmlNodeType.EntityReference:
					this.WriteEntityRef(reader.Name);
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.XmlDeclaration:
					this.WriteProcessingInstruction(reader.Name, reader.Value);
					break;
				case XmlNodeType.Comment:
					this.WriteComment(reader.Value);
					break;
				case XmlNodeType.DocumentType:
					this.WriteDocType(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value);
					break;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					this.WriteWhitespace(reader.Value);
					break;
				case XmlNodeType.EndElement:
					this.WriteFullEndElement();
					break;
				}
			}
			while (reader.Read() && (num < reader.Depth || (num == reader.Depth && reader.NodeType == XmlNodeType.EndElement)));
		}

		/// <summary>Copies everything from the <see cref="T:System.Xml.XPath.XPathNavigator" /> object to the writer. The position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> remains unchanged.</summary>
		/// <param name="navigator">The <see cref="T:System.Xml.XPath.XPathNavigator" /> to copy from.</param>
		/// <param name="defattr">
		///       <see langword="true" /> to copy the default attributes; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="navigator" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D7A RID: 3450 RVA: 0x000582C4 File Offset: 0x000564C4
		public virtual void WriteNode(XPathNavigator navigator, bool defattr)
		{
			if (navigator == null)
			{
				throw new ArgumentNullException("navigator");
			}
			int num = 0;
			navigator = navigator.Clone();
			for (;;)
			{
				IL_0018:
				bool flag = false;
				switch (navigator.NodeType)
				{
				case XPathNodeType.Root:
					flag = true;
					break;
				case XPathNodeType.Element:
					this.WriteStartElement(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI);
					if (navigator.MoveToFirstAttribute())
					{
						do
						{
							IXmlSchemaInfo schemaInfo = navigator.SchemaInfo;
							if (defattr || schemaInfo == null || !schemaInfo.IsDefault)
							{
								this.WriteStartAttribute(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI);
								this.WriteString(navigator.Value);
								this.WriteEndAttribute();
							}
						}
						while (navigator.MoveToNextAttribute());
						navigator.MoveToParent();
					}
					if (navigator.MoveToFirstNamespace(XPathNamespaceScope.Local))
					{
						this.WriteLocalNamespaces(navigator);
						navigator.MoveToParent();
					}
					flag = true;
					break;
				case XPathNodeType.Text:
					this.WriteString(navigator.Value);
					break;
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
					this.WriteWhitespace(navigator.Value);
					break;
				case XPathNodeType.ProcessingInstruction:
					this.WriteProcessingInstruction(navigator.LocalName, navigator.Value);
					break;
				case XPathNodeType.Comment:
					this.WriteComment(navigator.Value);
					break;
				}
				if (flag)
				{
					if (navigator.MoveToFirstChild())
					{
						num++;
						continue;
					}
					if (navigator.NodeType == XPathNodeType.Element)
					{
						if (navigator.IsEmptyElement)
						{
							this.WriteEndElement();
						}
						else
						{
							this.WriteFullEndElement();
						}
					}
				}
				while (num != 0)
				{
					if (navigator.MoveToNext())
					{
						goto IL_0018;
					}
					num--;
					navigator.MoveToParent();
					if (navigator.NodeType == XPathNodeType.Element)
					{
						this.WriteFullEndElement();
					}
				}
				break;
			}
		}

		/// <summary>Writes an element with the specified local name and value.</summary>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="value">The value of the element.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="localName" /> value is <see langword="null" /> or an empty string.-or-The parameter values are not valid.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">There is a character in the buffer that is a valid XML character but is not valid for the output encoding. For example, if the output encoding is ASCII, you should only use characters from the range of 0 to 127 for element and attribute names. The invalid character might be in the argument of this method or in an argument of previous methods that were writing to the buffer. Such characters are escaped by character entity references when possible (for example, in text nodes or attribute values). However, the character entity reference is not allowed in element and attribute names, comments, processing instructions, or CDATA sections.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D7B RID: 3451 RVA: 0x00058447 File Offset: 0x00056647
		public void WriteElementString(string localName, string value)
		{
			this.WriteElementString(localName, null, value);
		}

		/// <summary>Writes an element with the specified local name, namespace URI, and value.</summary>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="ns">The namespace URI to associate with the element.</param>
		/// <param name="value">The value of the element.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="localName" /> value is <see langword="null" /> or an empty string.-or-The parameter values are not valid.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">There is a character in the buffer that is a valid XML character but is not valid for the output encoding. For example, if the output encoding is ASCII, you should only use characters from the range of 0 to 127 for element and attribute names. The invalid character might be in the argument of this method or in an argument of previous methods that were writing to the buffer. Such characters are escaped by character entity references when possible (for example, in text nodes or attribute values). However, the character entity reference is not allowed in element and attribute names, comments, processing instructions, or CDATA sections.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D7C RID: 3452 RVA: 0x00058452 File Offset: 0x00056652
		public void WriteElementString(string localName, string ns, string value)
		{
			this.WriteStartElement(localName, ns);
			if (value != null && value.Length != 0)
			{
				this.WriteString(value);
			}
			this.WriteEndElement();
		}

		/// <summary>Writes an element with the specified prefix, local name, namespace URI, and value.</summary>
		/// <param name="prefix">The prefix of the element.</param>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="ns">The namespace URI of the element.</param>
		/// <param name="value">The value of the element.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="localName" /> value is <see langword="null" /> or an empty string.-or-The parameter values are not valid.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">There is a character in the buffer that is a valid XML character but is not valid for the output encoding. For example, if the output encoding is ASCII, you should only use characters from the range of 0 to 127 for element and attribute names. The invalid character might be in the argument of this method or in an argument of previous methods that were writing to the buffer. Such characters are escaped by character entity references when possible (for example, in text nodes or attribute values). However, the character entity reference is not allowed in element and attribute names, comments, processing instructions, or CDATA sections.</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D7D RID: 3453 RVA: 0x00058474 File Offset: 0x00056674
		public void WriteElementString(string prefix, string localName, string ns, string value)
		{
			this.WriteStartElement(prefix, localName, ns);
			if (value != null && value.Length != 0)
			{
				this.WriteString(value);
			}
			this.WriteEndElement();
		}

		/// <summary>Releases all resources used by the current instance of the <see cref="T:System.Xml.XmlWriter" /> class.</summary>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D7E RID: 3454 RVA: 0x0005849A File Offset: 0x0005669A
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Xml.XmlWriter" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		// Token: 0x06000D7F RID: 3455 RVA: 0x000584A3 File Offset: 0x000566A3
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.WriteState != WriteState.Closed)
			{
				this.Close();
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x000584B8 File Offset: 0x000566B8
		private void WriteLocalNamespaces(XPathNavigator nsNav)
		{
			string localName = nsNav.LocalName;
			string value = nsNav.Value;
			if (nsNav.MoveToNextNamespace(XPathNamespaceScope.Local))
			{
				this.WriteLocalNamespaces(nsNav);
			}
			if (localName.Length == 0)
			{
				this.WriteAttributeString(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/", value);
				return;
			}
			this.WriteAttributeString("xmlns", localName, "http://www.w3.org/2000/xmlns/", value);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlWriter" /> instance using the specified filename.</summary>
		/// <param name="outputFileName">The file to which you want to write. The <see cref="T:System.Xml.XmlWriter" /> creates a file at the specified path and writes to it in XML 1.0 text syntax. The <paramref name="outputFileName" /> must be a file system path.</param>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="url" /> value is <see langword="null" />.</exception>
		// Token: 0x06000D81 RID: 3457 RVA: 0x00058514 File Offset: 0x00056714
		public static XmlWriter Create(string outputFileName)
		{
			return XmlWriter.Create(outputFileName, null);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlWriter" /> instance using the filename and <see cref="T:System.Xml.XmlWriterSettings" /> object.</summary>
		/// <param name="outputFileName">The file to which you want to write. The <see cref="T:System.Xml.XmlWriter" /> creates a file at the specified path and writes to it in XML 1.0 text syntax. The <paramref name="outputFileName" /> must be a file system path.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlWriterSettings" /> object used to configure the new <see cref="T:System.Xml.XmlWriter" /> instance. If this is <see langword="null" />, a <see cref="T:System.Xml.XmlWriterSettings" /> with default settings is used.If the <see cref="T:System.Xml.XmlWriter" /> is being used with the <see cref="M:System.Xml.Xsl.XslCompiledTransform.Transform(System.String,System.Xml.XmlWriter)" /> method, you should use the <see cref="P:System.Xml.Xsl.XslCompiledTransform.OutputSettings" /> property to obtain an <see cref="T:System.Xml.XmlWriterSettings" /> object with the correct settings. This ensures that the created <see cref="T:System.Xml.XmlWriter" /> object has the correct output settings.</param>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="url" /> value is <see langword="null" />.</exception>
		// Token: 0x06000D82 RID: 3458 RVA: 0x0005851D File Offset: 0x0005671D
		public static XmlWriter Create(string outputFileName, XmlWriterSettings settings)
		{
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			return settings.CreateWriter(outputFileName);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlWriter" /> instance using the specified stream.</summary>
		/// <param name="output">The stream to which you want to write. The <see cref="T:System.Xml.XmlWriter" /> writes XML 1.0 text syntax and appends it to the specified stream.</param>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="stream" /> value is <see langword="null" />.</exception>
		// Token: 0x06000D83 RID: 3459 RVA: 0x00058530 File Offset: 0x00056730
		public static XmlWriter Create(Stream output)
		{
			return XmlWriter.Create(output, null);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlWriter" /> instance using the stream and <see cref="T:System.Xml.XmlWriterSettings" /> object.</summary>
		/// <param name="output">The stream to which you want to write. The <see cref="T:System.Xml.XmlWriter" /> writes XML 1.0 text syntax and appends it to the specified stream.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlWriterSettings" /> object used to configure the new <see cref="T:System.Xml.XmlWriter" /> instance. If this is <see langword="null" />, a <see cref="T:System.Xml.XmlWriterSettings" /> with default settings is used.If the <see cref="T:System.Xml.XmlWriter" /> is being used with the <see cref="M:System.Xml.Xsl.XslCompiledTransform.Transform(System.String,System.Xml.XmlWriter)" /> method, you should use the <see cref="P:System.Xml.Xsl.XslCompiledTransform.OutputSettings" /> property to obtain an <see cref="T:System.Xml.XmlWriterSettings" /> object with the correct settings. This ensures that the created <see cref="T:System.Xml.XmlWriter" /> object has the correct output settings.</param>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="stream" /> value is <see langword="null" />.</exception>
		// Token: 0x06000D84 RID: 3460 RVA: 0x00058539 File Offset: 0x00056739
		public static XmlWriter Create(Stream output, XmlWriterSettings settings)
		{
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			return settings.CreateWriter(output);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlWriter" /> instance using the specified <see cref="T:System.IO.TextWriter" />.</summary>
		/// <param name="output">The <see cref="T:System.IO.TextWriter" /> to which you want to write. The <see cref="T:System.Xml.XmlWriter" /> writes XML 1.0 text syntax and appends it to the specified <see cref="T:System.IO.TextWriter" />.</param>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="text" /> value is <see langword="null" />.</exception>
		// Token: 0x06000D85 RID: 3461 RVA: 0x0005854C File Offset: 0x0005674C
		public static XmlWriter Create(TextWriter output)
		{
			return XmlWriter.Create(output, null);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlWriter" /> instance using the <see cref="T:System.IO.TextWriter" /> and <see cref="T:System.Xml.XmlWriterSettings" /> objects.</summary>
		/// <param name="output">The <see cref="T:System.IO.TextWriter" /> to which you want to write. The <see cref="T:System.Xml.XmlWriter" /> writes XML 1.0 text syntax and appends it to the specified <see cref="T:System.IO.TextWriter" />.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlWriterSettings" /> object used to configure the new <see cref="T:System.Xml.XmlWriter" /> instance. If this is <see langword="null" />, a <see cref="T:System.Xml.XmlWriterSettings" /> with default settings is used.If the <see cref="T:System.Xml.XmlWriter" /> is being used with the <see cref="M:System.Xml.Xsl.XslCompiledTransform.Transform(System.String,System.Xml.XmlWriter)" /> method, you should use the <see cref="P:System.Xml.Xsl.XslCompiledTransform.OutputSettings" /> property to obtain an <see cref="T:System.Xml.XmlWriterSettings" /> object with the correct settings. This ensures that the created <see cref="T:System.Xml.XmlWriter" /> object has the correct output settings.</param>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="text" /> value is <see langword="null" />.</exception>
		// Token: 0x06000D86 RID: 3462 RVA: 0x00058555 File Offset: 0x00056755
		public static XmlWriter Create(TextWriter output, XmlWriterSettings settings)
		{
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			return settings.CreateWriter(output);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlWriter" /> instance using the specified <see cref="T:System.Text.StringBuilder" />.</summary>
		/// <param name="output">The <see cref="T:System.Text.StringBuilder" /> to which to write to. Content written by the <see cref="T:System.Xml.XmlWriter" /> is appended to the <see cref="T:System.Text.StringBuilder" />.</param>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="builder" /> value is <see langword="null" />.</exception>
		// Token: 0x06000D87 RID: 3463 RVA: 0x00058568 File Offset: 0x00056768
		public static XmlWriter Create(StringBuilder output)
		{
			return XmlWriter.Create(output, null);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlWriter" /> instance using the <see cref="T:System.Text.StringBuilder" /> and <see cref="T:System.Xml.XmlWriterSettings" /> objects.</summary>
		/// <param name="output">The <see cref="T:System.Text.StringBuilder" /> to which to write to. Content written by the <see cref="T:System.Xml.XmlWriter" /> is appended to the <see cref="T:System.Text.StringBuilder" />.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlWriterSettings" /> object used to configure the new <see cref="T:System.Xml.XmlWriter" /> instance. If this is <see langword="null" />, a <see cref="T:System.Xml.XmlWriterSettings" /> with default settings is used.If the <see cref="T:System.Xml.XmlWriter" /> is being used with the <see cref="M:System.Xml.Xsl.XslCompiledTransform.Transform(System.String,System.Xml.XmlWriter)" /> method, you should use the <see cref="P:System.Xml.Xsl.XslCompiledTransform.OutputSettings" /> property to obtain an <see cref="T:System.Xml.XmlWriterSettings" /> object with the correct settings. This ensures that the created <see cref="T:System.Xml.XmlWriter" /> object has the correct output settings.</param>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="builder" /> value is <see langword="null" />.</exception>
		// Token: 0x06000D88 RID: 3464 RVA: 0x00058571 File Offset: 0x00056771
		public static XmlWriter Create(StringBuilder output, XmlWriterSettings settings)
		{
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			return settings.CreateWriter(new StringWriter(output, CultureInfo.InvariantCulture));
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlWriter" /> instance using the specified <see cref="T:System.Xml.XmlWriter" /> object.</summary>
		/// <param name="output">The <see cref="T:System.Xml.XmlWriter" /> object that you want to use as the underlying writer.</param>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object that is wrapped around the specified <see cref="T:System.Xml.XmlWriter" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="writer" /> value is <see langword="null" />.</exception>
		// Token: 0x06000D89 RID: 3465 RVA: 0x0005859C File Offset: 0x0005679C
		public static XmlWriter Create(XmlWriter output)
		{
			return XmlWriter.Create(output, null);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlWriter" /> instance using the specified <see cref="T:System.Xml.XmlWriter" /> and <see cref="T:System.Xml.XmlWriterSettings" /> objects.</summary>
		/// <param name="output">The <see cref="T:System.Xml.XmlWriter" /> object that you want to use as the underlying writer.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlWriterSettings" /> object used to configure the new <see cref="T:System.Xml.XmlWriter" /> instance. If this is <see langword="null" />, a <see cref="T:System.Xml.XmlWriterSettings" /> with default settings is used.If the <see cref="T:System.Xml.XmlWriter" /> is being used with the <see cref="M:System.Xml.Xsl.XslCompiledTransform.Transform(System.String,System.Xml.XmlWriter)" /> method, you should use the <see cref="P:System.Xml.Xsl.XslCompiledTransform.OutputSettings" /> property to obtain an <see cref="T:System.Xml.XmlWriterSettings" /> object with the correct settings. This ensures that the created <see cref="T:System.Xml.XmlWriter" /> object has the correct output settings.</param>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object that is wrapped around the specified <see cref="T:System.Xml.XmlWriter" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="writer" /> value is <see langword="null" />.</exception>
		// Token: 0x06000D8A RID: 3466 RVA: 0x000585A5 File Offset: 0x000567A5
		public static XmlWriter Create(XmlWriter output, XmlWriterSettings settings)
		{
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			return settings.CreateWriter(output);
		}

		/// <summary>Asynchronously writes the XML declaration with the version "1.0".</summary>
		/// <returns>The task that represents the asynchronous <see langword="WriteStartDocument" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D8B RID: 3467 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteStartDocumentAsync()
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes the XML declaration with the version "1.0" and the standalone attribute.</summary>
		/// <param name="standalone">If <see langword="true" />, it writes "standalone=yes"; if <see langword="false" />, it writes "standalone=no".</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteStartDocument" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D8C RID: 3468 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteStartDocumentAsync(bool standalone)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously closes any open elements or attributes and puts the writer back in the Start state.</summary>
		/// <returns>The task that represents the asynchronous <see langword="WriteEndDocument" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D8D RID: 3469 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteEndDocumentAsync()
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes the DOCTYPE declaration with the specified name and optional attributes.</summary>
		/// <param name="name">The name of the DOCTYPE. This must be non-empty.</param>
		/// <param name="pubid">If non-null it also writes PUBLIC "pubid" "sysid" where <paramref name="pubid" /> and <paramref name="sysid" /> are replaced with the value of the given arguments.</param>
		/// <param name="sysid">If <paramref name="pubid" /> is <see langword="null" /> and <paramref name="sysid" /> is non-null it writes SYSTEM "sysid" where <paramref name="sysid" /> is replaced with the value of this argument.</param>
		/// <param name="subset">If non-null it writes [subset] where subset is replaced with the value of this argument.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteDocType" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D8E RID: 3470 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes the specified start tag and associates it with the given namespace and prefix.</summary>
		/// <param name="prefix">The namespace prefix of the element.</param>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="ns">The namespace URI to associate with the element.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteStartElement" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D8F RID: 3471 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteStartElementAsync(string prefix, string localName, string ns)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously closes one element and pops the corresponding namespace scope.</summary>
		/// <returns>The task that represents the asynchronous <see langword="WriteEndElement" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D90 RID: 3472 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteEndElementAsync()
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously closes one element and pops the corresponding namespace scope.</summary>
		/// <returns>The task that represents the asynchronous <see langword="WriteFullEndElement" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D91 RID: 3473 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteFullEndElementAsync()
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes out the attribute with the specified prefix, local name, namespace URI, and value.</summary>
		/// <param name="prefix">The namespace prefix of the attribute.</param>
		/// <param name="localName">The local name of the attribute.</param>
		/// <param name="ns">The namespace URI of the attribute.</param>
		/// <param name="value">The value of the attribute.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteAttributeString" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D92 RID: 3474 RVA: 0x000585B8 File Offset: 0x000567B8
		public Task WriteAttributeStringAsync(string prefix, string localName, string ns, string value)
		{
			Task task = this.WriteStartAttributeAsync(prefix, localName, ns);
			if (task.IsSuccess())
			{
				return this.WriteStringAsync(value).CallTaskFuncWhenFinish(new Func<Task>(this.WriteEndAttributeAsync));
			}
			return this.WriteAttributeStringAsyncHelper(task, value);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x000585FC File Offset: 0x000567FC
		private async Task WriteAttributeStringAsyncHelper(Task task, string value)
		{
			await task.ConfigureAwait(false);
			await this.WriteStringAsync(value).ConfigureAwait(false);
			await this.WriteEndAttributeAsync().ConfigureAwait(false);
		}

		/// <summary>Asynchronously writes the start of an attribute with the specified prefix, local name, and namespace URI.</summary>
		/// <param name="prefix">The namespace prefix of the attribute.</param>
		/// <param name="localName">The local name of the attribute.</param>
		/// <param name="ns">The namespace URI for the attribute.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteStartAttribute" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D94 RID: 3476 RVA: 0x0000349C File Offset: 0x0000169C
		protected internal virtual Task WriteStartAttributeAsync(string prefix, string localName, string ns)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously closes the previous <see cref="M:System.Xml.XmlWriter.WriteStartAttribute(System.String,System.String)" /> call.</summary>
		/// <returns>The task that represents the asynchronous <see langword="WriteEndAttribute" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D95 RID: 3477 RVA: 0x0000349C File Offset: 0x0000169C
		protected internal virtual Task WriteEndAttributeAsync()
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes out a &lt;![CDATA[...]]&gt; block containing the specified text.</summary>
		/// <param name="text">The text to place inside the CDATA block.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteCData" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D96 RID: 3478 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteCDataAsync(string text)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes out a comment &lt;!--...--&gt; containing the specified text.</summary>
		/// <param name="text">Text to place inside the comment.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteComment" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D97 RID: 3479 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteCommentAsync(string text)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes out a processing instruction with a space between the name and text as follows: &lt;?name text?&gt;.</summary>
		/// <param name="name">The name of the processing instruction.</param>
		/// <param name="text">The text to include in the processing instruction.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteProcessingInstruction" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D98 RID: 3480 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteProcessingInstructionAsync(string name, string text)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes out an entity reference as <see langword="&amp;name;" />.</summary>
		/// <param name="name">The name of the entity reference.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteEntityRef" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D99 RID: 3481 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteEntityRefAsync(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously forces the generation of a character entity for the specified Unicode character value.</summary>
		/// <param name="ch">The Unicode character for which to generate a character entity.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteCharEntity" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D9A RID: 3482 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteCharEntityAsync(char ch)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes out the given white space.</summary>
		/// <param name="ws">The string of white space characters.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteWhitespace" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D9B RID: 3483 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteWhitespaceAsync(string ws)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes the given text content.</summary>
		/// <param name="text">The text to write.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteString" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D9C RID: 3484 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteStringAsync(string text)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously generates and writes the surrogate character entity for the surrogate character pair.</summary>
		/// <param name="lowChar">The low surrogate. This must be a value between 0xDC00 and 0xDFFF.</param>
		/// <param name="highChar">The high surrogate. This must be a value between 0xD800 and 0xDBFF.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteSurrogateCharEntity" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D9D RID: 3485 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes text one buffer at a time.</summary>
		/// <param name="buffer">Character array containing the text to write.</param>
		/// <param name="index">The position in the buffer indicating the start of the text to write.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteChars" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D9E RID: 3486 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteCharsAsync(char[] buffer, int index, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes raw markup manually from a character buffer.</summary>
		/// <param name="buffer">Character array containing the text to write.</param>
		/// <param name="index">The position within the buffer indicating the start of the text to write.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteRaw" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000D9F RID: 3487 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteRawAsync(char[] buffer, int index, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes raw markup manually from a string.</summary>
		/// <param name="data">String containing the text to write.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteRaw" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000DA0 RID: 3488 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteRawAsync(string data)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously encodes the specified binary bytes as Base64 and writes out the resulting text.</summary>
		/// <param name="buffer">Byte array to encode.</param>
		/// <param name="index">The position in the buffer indicating the start of the bytes to write.</param>
		/// <param name="count">The number of bytes to write.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteBase64" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000DA1 RID: 3489 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task WriteBase64Async(byte[] buffer, int index, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously encodes the specified binary bytes as <see langword="BinHex" /> and writes out the resulting text.</summary>
		/// <param name="buffer">Byte array to encode.</param>
		/// <param name="index">The position in the buffer indicating the start of the bytes to write.</param>
		/// <param name="count">The number of bytes to write.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteBinHex" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000DA2 RID: 3490 RVA: 0x0005864F File Offset: 0x0005684F
		public virtual Task WriteBinHexAsync(byte[] buffer, int index, int count)
		{
			return BinHexEncoder.EncodeAsync(buffer, index, count, this);
		}

		/// <summary>Asynchronously flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.</summary>
		/// <returns>The task that represents the asynchronous <see langword="Flush" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000DA3 RID: 3491 RVA: 0x0000349C File Offset: 0x0000169C
		public virtual Task FlushAsync()
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously writes out the specified name, ensuring it is a valid NmToken according to the W3C XML 1.0 recommendation (http://www.w3.org/TR/1998/REC-xml-19980210#NT-Name).</summary>
		/// <param name="name">The name to write.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteNmToken" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000DA4 RID: 3492 RVA: 0x0005865A File Offset: 0x0005685A
		public virtual Task WriteNmTokenAsync(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
			}
			return this.WriteStringAsync(XmlConvert.VerifyNMTOKEN(name, ExceptionType.ArgumentException));
		}

		/// <summary>Asynchronously writes out the specified name, ensuring it is a valid name according to the W3C XML 1.0 recommendation (http://www.w3.org/TR/1998/REC-xml-19980210#NT-Name).</summary>
		/// <param name="name">The name to write.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteName" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000DA5 RID: 3493 RVA: 0x00058684 File Offset: 0x00056884
		public virtual Task WriteNameAsync(string name)
		{
			return this.WriteStringAsync(XmlConvert.VerifyQName(name, ExceptionType.ArgumentException));
		}

		/// <summary>Asynchronously writes out the namespace-qualified name. This method looks up the prefix that is in scope for the given namespace.</summary>
		/// <param name="localName">The local name to write.</param>
		/// <param name="ns">The namespace URI for the name.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteQualifiedName" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000DA6 RID: 3494 RVA: 0x00058694 File Offset: 0x00056894
		public virtual async Task WriteQualifiedNameAsync(string localName, string ns)
		{
			if (ns != null && ns.Length > 0)
			{
				string text = this.LookupPrefix(ns);
				if (text == null)
				{
					throw new ArgumentException(Res.GetString("The '{0}' namespace is not defined.", new object[] { ns }));
				}
				await this.WriteStringAsync(text).ConfigureAwait(false);
				await this.WriteStringAsync(":").ConfigureAwait(false);
			}
			await this.WriteStringAsync(localName).ConfigureAwait(false);
		}

		/// <summary>Asynchronously writes out all the attributes found at the current position in the <see cref="T:System.Xml.XmlReader" />.</summary>
		/// <param name="reader">The <see langword="XmlReader" /> from which to copy the attributes.</param>
		/// <param name="defattr">
		///       <see langword="true" /> to copy the default attributes from the <see langword="XmlReader" />; otherwise, <see langword="false" />.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteAttributes" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000DA7 RID: 3495 RVA: 0x000586E8 File Offset: 0x000568E8
		public virtual async Task WriteAttributesAsync(XmlReader reader, bool defattr)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader.NodeType == XmlNodeType.Element || reader.NodeType == XmlNodeType.XmlDeclaration)
			{
				if (reader.MoveToFirstAttribute())
				{
					await this.WriteAttributesAsync(reader, defattr).ConfigureAwait(false);
					reader.MoveToElement();
				}
			}
			else
			{
				if (reader.NodeType != XmlNodeType.Attribute)
				{
					throw new XmlException("The current position on the Reader is neither an element nor an attribute.", string.Empty);
				}
				do
				{
					if (defattr || !reader.IsDefaultInternal)
					{
						await this.WriteStartAttributeAsync(reader.Prefix, reader.LocalName, reader.NamespaceURI).ConfigureAwait(false);
						while (reader.ReadAttributeValue())
						{
							if (reader.NodeType == XmlNodeType.EntityReference)
							{
								await this.WriteEntityRefAsync(reader.Name).ConfigureAwait(false);
							}
							else
							{
								await this.WriteStringAsync(reader.Value).ConfigureAwait(false);
							}
						}
						await this.WriteEndAttributeAsync().ConfigureAwait(false);
					}
				}
				while (reader.MoveToNextAttribute());
			}
		}

		/// <summary>Asynchronously copies everything from the reader to the writer and moves the reader to the start of the next sibling.</summary>
		/// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> to read from.</param>
		/// <param name="defattr">
		///       <see langword="true" /> to copy the default attributes from the <see langword="XmlReader" />; otherwise, <see langword="false" />.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteNode" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000DA8 RID: 3496 RVA: 0x0005873B File Offset: 0x0005693B
		public virtual Task WriteNodeAsync(XmlReader reader, bool defattr)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader.Settings != null && reader.Settings.Async)
			{
				return this.WriteNodeAsync_CallAsyncReader(reader, defattr);
			}
			return this.WriteNodeAsync_CallSyncReader(reader, defattr);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00058774 File Offset: 0x00056974
		internal async Task WriteNodeAsync_CallSyncReader(XmlReader reader, bool defattr)
		{
			bool canReadChunk = reader.CanReadValueChunk;
			int d = ((reader.NodeType == XmlNodeType.None) ? (-1) : reader.Depth);
			do
			{
				switch (reader.NodeType)
				{
				case XmlNodeType.Element:
					await this.WriteStartElementAsync(reader.Prefix, reader.LocalName, reader.NamespaceURI).ConfigureAwait(false);
					await this.WriteAttributesAsync(reader, defattr).ConfigureAwait(false);
					if (reader.IsEmptyElement)
					{
						await this.WriteEndElementAsync().ConfigureAwait(false);
					}
					break;
				case XmlNodeType.Text:
					if (canReadChunk)
					{
						if (this.writeNodeBuffer == null)
						{
							this.writeNodeBuffer = new char[1024];
						}
						for (;;)
						{
							int num = reader.ReadValueChunk(this.writeNodeBuffer, 0, 1024);
							if (num <= 0)
							{
								break;
							}
							await this.WriteCharsAsync(this.writeNodeBuffer, 0, num).ConfigureAwait(false);
						}
					}
					else
					{
						await this.WriteStringAsync(reader.Value).ConfigureAwait(false);
					}
					break;
				case XmlNodeType.CDATA:
					await this.WriteCDataAsync(reader.Value).ConfigureAwait(false);
					break;
				case XmlNodeType.EntityReference:
					await this.WriteEntityRefAsync(reader.Name).ConfigureAwait(false);
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.XmlDeclaration:
					await this.WriteProcessingInstructionAsync(reader.Name, reader.Value).ConfigureAwait(false);
					break;
				case XmlNodeType.Comment:
					await this.WriteCommentAsync(reader.Value).ConfigureAwait(false);
					break;
				case XmlNodeType.DocumentType:
					await this.WriteDocTypeAsync(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value).ConfigureAwait(false);
					break;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					await this.WriteWhitespaceAsync(reader.Value).ConfigureAwait(false);
					break;
				case XmlNodeType.EndElement:
					await this.WriteFullEndElementAsync().ConfigureAwait(false);
					break;
				}
			}
			while (reader.Read() && (d < reader.Depth || (d == reader.Depth && reader.NodeType == XmlNodeType.EndElement)));
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000587C8 File Offset: 0x000569C8
		internal async Task WriteNodeAsync_CallAsyncReader(XmlReader reader, bool defattr)
		{
			bool canReadChunk = reader.CanReadValueChunk;
			int d = ((reader.NodeType == XmlNodeType.None) ? (-1) : reader.Depth);
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter;
			do
			{
				switch (reader.NodeType)
				{
				case XmlNodeType.Element:
					await this.WriteStartElementAsync(reader.Prefix, reader.LocalName, reader.NamespaceURI).ConfigureAwait(false);
					await this.WriteAttributesAsync(reader, defattr).ConfigureAwait(false);
					if (reader.IsEmptyElement)
					{
						await this.WriteEndElementAsync().ConfigureAwait(false);
					}
					break;
				case XmlNodeType.Text:
					if (canReadChunk)
					{
						if (this.writeNodeBuffer == null)
						{
							this.writeNodeBuffer = new char[1024];
						}
						for (;;)
						{
							object obj = await reader.ReadValueChunkAsync(this.writeNodeBuffer, 0, 1024).ConfigureAwait(false);
							if (obj <= 0)
							{
								break;
							}
							await this.WriteCharsAsync(this.writeNodeBuffer, 0, obj).ConfigureAwait(false);
						}
					}
					else
					{
						await this.WriteStringAsync(await reader.GetValueAsync().ConfigureAwait(false)).ConfigureAwait(false);
					}
					break;
				case XmlNodeType.CDATA:
					await this.WriteCDataAsync(reader.Value).ConfigureAwait(false);
					break;
				case XmlNodeType.EntityReference:
					await this.WriteEntityRefAsync(reader.Name).ConfigureAwait(false);
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.XmlDeclaration:
					await this.WriteProcessingInstructionAsync(reader.Name, reader.Value).ConfigureAwait(false);
					break;
				case XmlNodeType.Comment:
					await this.WriteCommentAsync(reader.Value).ConfigureAwait(false);
					break;
				case XmlNodeType.DocumentType:
					await this.WriteDocTypeAsync(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value).ConfigureAwait(false);
					break;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					await this.WriteWhitespaceAsync(await reader.GetValueAsync().ConfigureAwait(false)).ConfigureAwait(false);
					break;
				case XmlNodeType.EndElement:
					await this.WriteFullEndElementAsync().ConfigureAwait(false);
					break;
				}
				configuredTaskAwaiter = reader.ReadAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
			}
			while (configuredTaskAwaiter.GetResult() && (d < reader.Depth || (d == reader.Depth && reader.NodeType == XmlNodeType.EndElement)));
		}

		/// <summary>Asynchronously copies everything from the <see cref="T:System.Xml.XPath.XPathNavigator" /> object to the writer. The position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> remains unchanged.</summary>
		/// <param name="navigator">The <see cref="T:System.Xml.XPath.XPathNavigator" /> to copy from.</param>
		/// <param name="defattr">
		///       <see langword="true" /> to copy the default attributes; otherwise, <see langword="false" />.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteNode" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000DAB RID: 3499 RVA: 0x0005881C File Offset: 0x00056A1C
		public virtual async Task WriteNodeAsync(XPathNavigator navigator, bool defattr)
		{
			if (navigator == null)
			{
				throw new ArgumentNullException("navigator");
			}
			int iLevel = 0;
			navigator = navigator.Clone();
			for (;;)
			{
				IL_006F:
				bool mayHaveChildren = false;
				switch (navigator.NodeType)
				{
				case XPathNodeType.Root:
					mayHaveChildren = true;
					break;
				case XPathNodeType.Element:
					await this.WriteStartElementAsync(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI).ConfigureAwait(false);
					if (navigator.MoveToFirstAttribute())
					{
						do
						{
							IXmlSchemaInfo schemaInfo = navigator.SchemaInfo;
							if (defattr || schemaInfo == null || !schemaInfo.IsDefault)
							{
								await this.WriteStartAttributeAsync(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI).ConfigureAwait(false);
								await this.WriteStringAsync(navigator.Value).ConfigureAwait(false);
								await this.WriteEndAttributeAsync().ConfigureAwait(false);
							}
						}
						while (navigator.MoveToNextAttribute());
						navigator.MoveToParent();
					}
					if (navigator.MoveToFirstNamespace(XPathNamespaceScope.Local))
					{
						await this.WriteLocalNamespacesAsync(navigator).ConfigureAwait(false);
						navigator.MoveToParent();
					}
					mayHaveChildren = true;
					break;
				case XPathNodeType.Text:
					await this.WriteStringAsync(navigator.Value).ConfigureAwait(false);
					break;
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
					await this.WriteWhitespaceAsync(navigator.Value).ConfigureAwait(false);
					break;
				case XPathNodeType.ProcessingInstruction:
					await this.WriteProcessingInstructionAsync(navigator.LocalName, navigator.Value).ConfigureAwait(false);
					break;
				case XPathNodeType.Comment:
					await this.WriteCommentAsync(navigator.Value).ConfigureAwait(false);
					break;
				}
				if (mayHaveChildren)
				{
					if (navigator.MoveToFirstChild())
					{
						iLevel++;
						continue;
					}
					if (navigator.NodeType == XPathNodeType.Element)
					{
						if (navigator.IsEmptyElement)
						{
							await this.WriteEndElementAsync().ConfigureAwait(false);
						}
						else
						{
							await this.WriteFullEndElementAsync().ConfigureAwait(false);
						}
					}
				}
				while (iLevel != 0)
				{
					if (navigator.MoveToNext())
					{
						goto IL_006F;
					}
					iLevel--;
					navigator.MoveToParent();
					if (navigator.NodeType == XPathNodeType.Element)
					{
						await this.WriteFullEndElementAsync().ConfigureAwait(false);
					}
				}
				break;
			}
		}

		/// <summary>Asynchronously writes an element with the specified prefix, local name, namespace URI, and value.</summary>
		/// <param name="prefix">The prefix of the element.</param>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="ns">The namespace URI of the element.</param>
		/// <param name="value">The value of the element.</param>
		/// <returns>The task that represents the asynchronous <see langword="WriteElementString" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> method was called before a previous asynchronous operation finished. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “An asynchronous operation is already in progress.”</exception>
		/// <exception cref="T:System.InvalidOperationException">An <see cref="T:System.Xml.XmlWriter" /> asynchronous method was called without setting the <see cref="P:System.Xml.XmlWriterSettings.Async" /> flag to <see langword="true" />. In this case, <see cref="T:System.InvalidOperationException" /> is thrown with the message “Set XmlWriterSettings.Async to true if you want to use Async Methods.”</exception>
		// Token: 0x06000DAC RID: 3500 RVA: 0x00058870 File Offset: 0x00056A70
		public async Task WriteElementStringAsync(string prefix, string localName, string ns, string value)
		{
			await this.WriteStartElementAsync(prefix, localName, ns).ConfigureAwait(false);
			if (value != null && value.Length != 0)
			{
				await this.WriteStringAsync(value).ConfigureAwait(false);
			}
			await this.WriteEndElementAsync().ConfigureAwait(false);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x000588D4 File Offset: 0x00056AD4
		private async Task WriteLocalNamespacesAsync(XPathNavigator nsNav)
		{
			string prefix = nsNav.LocalName;
			string ns = nsNav.Value;
			if (nsNav.MoveToNextNamespace(XPathNamespaceScope.Local))
			{
				await this.WriteLocalNamespacesAsync(nsNav).ConfigureAwait(false);
			}
			if (prefix.Length == 0)
			{
				await this.WriteAttributeStringAsync(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/", ns).ConfigureAwait(false);
			}
			else
			{
				await this.WriteAttributeStringAsync("xmlns", prefix, "http://www.w3.org/2000/xmlns/", ns).ConfigureAwait(false);
			}
		}

		// Token: 0x04000EAC RID: 3756
		private char[] writeNodeBuffer;

		// Token: 0x04000EAD RID: 3757
		private const int WriteNodeBufferSize = 1024;
	}
}
