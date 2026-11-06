using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x0200018C RID: 396
	internal class XsdValidatingReader : XmlReader, IXmlSchemaInfo, IXmlLineInfo, IXmlNamespaceResolver
	{
		// Token: 0x06000E42 RID: 3650 RVA: 0x0005C8E8 File Offset: 0x0005AAE8
		internal XsdValidatingReader(XmlReader reader, XmlResolver xmlResolver, XmlReaderSettings readerSettings, XmlSchemaObject partialValidationType)
		{
			this.coreReader = reader;
			this.coreReaderNSResolver = reader as IXmlNamespaceResolver;
			this.lineInfo = reader as IXmlLineInfo;
			this.coreReaderNameTable = this.coreReader.NameTable;
			if (this.coreReaderNSResolver == null)
			{
				this.nsManager = new XmlNamespaceManager(this.coreReaderNameTable);
				this.manageNamespaces = true;
			}
			this.thisNSResolver = this;
			this.xmlResolver = xmlResolver;
			this.processInlineSchema = (readerSettings.ValidationFlags & XmlSchemaValidationFlags.ProcessInlineSchema) > XmlSchemaValidationFlags.None;
			this.Init();
			this.SetupValidator(readerSettings, reader, partialValidationType);
			this.validationEvent = readerSettings.GetEventHandler();
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0005C991 File Offset: 0x0005AB91
		internal XsdValidatingReader(XmlReader reader, XmlResolver xmlResolver, XmlReaderSettings readerSettings)
			: this(reader, xmlResolver, readerSettings, null)
		{
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0005C9A0 File Offset: 0x0005ABA0
		private void Init()
		{
			this.validationState = XsdValidatingReader.ValidatingReaderState.Init;
			this.defaultAttributes = new ArrayList();
			this.currentAttrIndex = -1;
			this.attributePSVINodes = new AttributePSVIInfo[8];
			this.valueGetter = new XmlValueGetter(this.GetStringValue);
			XsdValidatingReader.TypeOfString = typeof(string);
			this.xmlSchemaInfo = new XmlSchemaInfo();
			this.NsXmlNs = this.coreReaderNameTable.Add("http://www.w3.org/2000/xmlns/");
			this.NsXs = this.coreReaderNameTable.Add("http://www.w3.org/2001/XMLSchema");
			this.NsXsi = this.coreReaderNameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
			this.XsiType = this.coreReaderNameTable.Add("type");
			this.XsiNil = this.coreReaderNameTable.Add("nil");
			this.XsiSchemaLocation = this.coreReaderNameTable.Add("schemaLocation");
			this.XsiNoNamespaceSchemaLocation = this.coreReaderNameTable.Add("noNamespaceSchemaLocation");
			this.XsdSchema = this.coreReaderNameTable.Add("schema");
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0005CAB0 File Offset: 0x0005ACB0
		private void SetupValidator(XmlReaderSettings readerSettings, XmlReader reader, XmlSchemaObject partialValidationType)
		{
			this.validator = new XmlSchemaValidator(this.coreReaderNameTable, readerSettings.Schemas, this.thisNSResolver, readerSettings.ValidationFlags);
			this.validator.XmlResolver = this.xmlResolver;
			this.validator.SourceUri = XmlConvert.ToUri(reader.BaseURI);
			this.validator.ValidationEventSender = this;
			this.validator.ValidationEventHandler += readerSettings.GetEventHandler();
			this.validator.LineInfoProvider = this.lineInfo;
			if (this.validator.ProcessSchemaHints)
			{
				this.validator.SchemaSet.ReaderSettings.DtdProcessing = readerSettings.DtdProcessing;
			}
			this.validator.SetDtdSchemaInfo(reader.DtdInfo);
			if (partialValidationType != null)
			{
				this.validator.Initialize(partialValidationType);
				return;
			}
			this.validator.Initialize();
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0005CB8C File Offset: 0x0005AD8C
		public override XmlReaderSettings Settings
		{
			get
			{
				XmlReaderSettings xmlReaderSettings = this.coreReader.Settings;
				if (xmlReaderSettings != null)
				{
					xmlReaderSettings = xmlReaderSettings.Clone();
				}
				if (xmlReaderSettings == null)
				{
					xmlReaderSettings = new XmlReaderSettings();
				}
				xmlReaderSettings.Schemas = this.validator.SchemaSet;
				xmlReaderSettings.ValidationType = ValidationType.Schema;
				xmlReaderSettings.ValidationFlags = this.validator.ValidationFlags;
				xmlReaderSettings.ReadOnly = true;
				return xmlReaderSettings;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x0005CBEC File Offset: 0x0005ADEC
		public override XmlNodeType NodeType
		{
			get
			{
				if (this.validationState < XsdValidatingReader.ValidatingReaderState.None)
				{
					return this.cachedNode.NodeType;
				}
				XmlNodeType nodeType = this.coreReader.NodeType;
				if (nodeType == XmlNodeType.Whitespace && (this.validator.CurrentContentType == XmlSchemaContentType.TextOnly || this.validator.CurrentContentType == XmlSchemaContentType.Mixed))
				{
					return XmlNodeType.SignificantWhitespace;
				}
				return nodeType;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0005CC40 File Offset: 0x0005AE40
		public override string Name
		{
			get
			{
				if (this.validationState != XsdValidatingReader.ValidatingReaderState.OnDefaultAttribute)
				{
					return this.coreReader.Name;
				}
				string defaultAttributePrefix = this.validator.GetDefaultAttributePrefix(this.cachedNode.Namespace);
				if (defaultAttributePrefix != null && defaultAttributePrefix.Length != 0)
				{
					return string.Concat(new string[] { defaultAttributePrefix + ":" + this.cachedNode.LocalName });
				}
				return this.cachedNode.LocalName;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x0005CCB4 File Offset: 0x0005AEB4
		public override string LocalName
		{
			get
			{
				if (this.validationState < XsdValidatingReader.ValidatingReaderState.None)
				{
					return this.cachedNode.LocalName;
				}
				return this.coreReader.LocalName;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x0005CCD6 File Offset: 0x0005AED6
		public override string NamespaceURI
		{
			get
			{
				if (this.validationState < XsdValidatingReader.ValidatingReaderState.None)
				{
					return this.cachedNode.Namespace;
				}
				return this.coreReader.NamespaceURI;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x0005CCF8 File Offset: 0x0005AEF8
		public override string Prefix
		{
			get
			{
				if (this.validationState < XsdValidatingReader.ValidatingReaderState.None)
				{
					return this.cachedNode.Prefix;
				}
				return this.coreReader.Prefix;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x0005CD1A File Offset: 0x0005AF1A
		public override bool HasValue
		{
			get
			{
				return this.validationState < XsdValidatingReader.ValidatingReaderState.None || this.coreReader.HasValue;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x0005CD32 File Offset: 0x0005AF32
		public override string Value
		{
			get
			{
				if (this.validationState < XsdValidatingReader.ValidatingReaderState.None)
				{
					return this.cachedNode.RawValue;
				}
				return this.coreReader.Value;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x0005CD54 File Offset: 0x0005AF54
		public override int Depth
		{
			get
			{
				if (this.validationState < XsdValidatingReader.ValidatingReaderState.None)
				{
					return this.cachedNode.Depth;
				}
				return this.coreReader.Depth;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x0005CD76 File Offset: 0x0005AF76
		public override string BaseURI
		{
			get
			{
				return this.coreReader.BaseURI;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x0005CD83 File Offset: 0x0005AF83
		public override bool IsEmptyElement
		{
			get
			{
				return this.coreReader.IsEmptyElement;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x0005CD90 File Offset: 0x0005AF90
		public override bool IsDefault
		{
			get
			{
				return this.validationState == XsdValidatingReader.ValidatingReaderState.OnDefaultAttribute || this.coreReader.IsDefault;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x0005CDA8 File Offset: 0x0005AFA8
		public override char QuoteChar
		{
			get
			{
				return this.coreReader.QuoteChar;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x0005CDB5 File Offset: 0x0005AFB5
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.coreReader.XmlSpace;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x0005CDC2 File Offset: 0x0005AFC2
		public override string XmlLang
		{
			get
			{
				return this.coreReader.XmlLang;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x00002068 File Offset: 0x00000268
		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x0005CDD0 File Offset: 0x0005AFD0
		public override Type ValueType
		{
			get
			{
				XmlNodeType nodeType = this.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType != XmlNodeType.Attribute)
					{
						if (nodeType != XmlNodeType.EndElement)
						{
							goto IL_0062;
						}
					}
					else
					{
						if (this.attributePSVI != null && this.AttributeSchemaInfo.ContentType == XmlSchemaContentType.TextOnly)
						{
							return this.AttributeSchemaInfo.SchemaType.Datatype.ValueType;
						}
						goto IL_0062;
					}
				}
				if (this.xmlSchemaInfo.ContentType == XmlSchemaContentType.TextOnly)
				{
					return this.xmlSchemaInfo.SchemaType.Datatype.ValueType;
				}
				IL_0062:
				return XsdValidatingReader.TypeOfString;
			}
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0005CE46 File Offset: 0x0005B046
		public override object ReadContentAsObject()
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAsObject");
			}
			return this.InternalReadContentAsObject(true);
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0005CE68 File Offset: 0x0005B068
		public override bool ReadContentAsBoolean()
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAsBoolean");
			}
			object obj = this.InternalReadContentAsObject();
			XmlSchemaType xmlSchemaType = ((this.NodeType == XmlNodeType.Attribute) ? this.AttributeXmlType : this.ElementXmlType);
			bool flag;
			try
			{
				if (xmlSchemaType != null)
				{
					flag = xmlSchemaType.ValueConverter.ToBoolean(obj);
				}
				else
				{
					flag = XmlUntypedConverter.Untyped.ToBoolean(obj);
				}
			}
			catch (InvalidCastException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", ex, this);
			}
			catch (FormatException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", ex3, this);
			}
			return flag;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0005CF34 File Offset: 0x0005B134
		public override DateTime ReadContentAsDateTime()
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAsDateTime");
			}
			object obj = this.InternalReadContentAsObject();
			XmlSchemaType xmlSchemaType = ((this.NodeType == XmlNodeType.Attribute) ? this.AttributeXmlType : this.ElementXmlType);
			DateTime dateTime;
			try
			{
				if (xmlSchemaType != null)
				{
					dateTime = xmlSchemaType.ValueConverter.ToDateTime(obj);
				}
				else
				{
					dateTime = XmlUntypedConverter.Untyped.ToDateTime(obj);
				}
			}
			catch (InvalidCastException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", ex, this);
			}
			catch (FormatException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", ex3, this);
			}
			return dateTime;
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0005D000 File Offset: 0x0005B200
		public override double ReadContentAsDouble()
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAsDouble");
			}
			object obj = this.InternalReadContentAsObject();
			XmlSchemaType xmlSchemaType = ((this.NodeType == XmlNodeType.Attribute) ? this.AttributeXmlType : this.ElementXmlType);
			double num;
			try
			{
				if (xmlSchemaType != null)
				{
					num = xmlSchemaType.ValueConverter.ToDouble(obj);
				}
				else
				{
					num = XmlUntypedConverter.Untyped.ToDouble(obj);
				}
			}
			catch (InvalidCastException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Double", ex, this);
			}
			catch (FormatException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Double", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Double", ex3, this);
			}
			return num;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0005D0CC File Offset: 0x0005B2CC
		public override float ReadContentAsFloat()
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAsFloat");
			}
			object obj = this.InternalReadContentAsObject();
			XmlSchemaType xmlSchemaType = ((this.NodeType == XmlNodeType.Attribute) ? this.AttributeXmlType : this.ElementXmlType);
			float num;
			try
			{
				if (xmlSchemaType != null)
				{
					num = xmlSchemaType.ValueConverter.ToSingle(obj);
				}
				else
				{
					num = XmlUntypedConverter.Untyped.ToSingle(obj);
				}
			}
			catch (InvalidCastException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Float", ex, this);
			}
			catch (FormatException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Float", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Float", ex3, this);
			}
			return num;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0005D198 File Offset: 0x0005B398
		public override decimal ReadContentAsDecimal()
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAsDecimal");
			}
			object obj = this.InternalReadContentAsObject();
			XmlSchemaType xmlSchemaType = ((this.NodeType == XmlNodeType.Attribute) ? this.AttributeXmlType : this.ElementXmlType);
			decimal num;
			try
			{
				if (xmlSchemaType != null)
				{
					num = xmlSchemaType.ValueConverter.ToDecimal(obj);
				}
				else
				{
					num = XmlUntypedConverter.Untyped.ToDecimal(obj);
				}
			}
			catch (InvalidCastException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", ex, this);
			}
			catch (FormatException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", ex3, this);
			}
			return num;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0005D264 File Offset: 0x0005B464
		public override int ReadContentAsInt()
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAsInt");
			}
			object obj = this.InternalReadContentAsObject();
			XmlSchemaType xmlSchemaType = ((this.NodeType == XmlNodeType.Attribute) ? this.AttributeXmlType : this.ElementXmlType);
			int num;
			try
			{
				if (xmlSchemaType != null)
				{
					num = xmlSchemaType.ValueConverter.ToInt32(obj);
				}
				else
				{
					num = XmlUntypedConverter.Untyped.ToInt32(obj);
				}
			}
			catch (InvalidCastException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Int", ex, this);
			}
			catch (FormatException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Int", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Int", ex3, this);
			}
			return num;
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0005D330 File Offset: 0x0005B530
		public override long ReadContentAsLong()
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAsLong");
			}
			object obj = this.InternalReadContentAsObject();
			XmlSchemaType xmlSchemaType = ((this.NodeType == XmlNodeType.Attribute) ? this.AttributeXmlType : this.ElementXmlType);
			long num;
			try
			{
				if (xmlSchemaType != null)
				{
					num = xmlSchemaType.ValueConverter.ToInt64(obj);
				}
				else
				{
					num = XmlUntypedConverter.Untyped.ToInt64(obj);
				}
			}
			catch (InvalidCastException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Long", ex, this);
			}
			catch (FormatException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Long", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Long", ex3, this);
			}
			return num;
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0005D3FC File Offset: 0x0005B5FC
		public override string ReadContentAsString()
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAsString");
			}
			object obj = this.InternalReadContentAsObject();
			XmlSchemaType xmlSchemaType = ((this.NodeType == XmlNodeType.Attribute) ? this.AttributeXmlType : this.ElementXmlType);
			string text;
			try
			{
				if (xmlSchemaType != null)
				{
					text = xmlSchemaType.ValueConverter.ToString(obj);
				}
				else
				{
					text = obj as string;
				}
			}
			catch (InvalidCastException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "String", ex, this);
			}
			catch (FormatException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "String", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "String", ex3, this);
			}
			return text;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0005D4C4 File Offset: 0x0005B6C4
		public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAs");
			}
			string text;
			object obj = this.InternalReadContentAsObject(false, out text);
			XmlSchemaType xmlSchemaType = ((this.NodeType == XmlNodeType.Attribute) ? this.AttributeXmlType : this.ElementXmlType);
			object obj2;
			try
			{
				if (xmlSchemaType != null)
				{
					if (returnType == typeof(DateTimeOffset) && xmlSchemaType.Datatype is Datatype_dateTimeBase)
					{
						obj = text;
					}
					obj2 = xmlSchemaType.ValueConverter.ChangeType(obj, returnType);
				}
				else
				{
					obj2 = XmlUntypedConverter.Untyped.ChangeType(obj, returnType, namespaceResolver);
				}
			}
			catch (FormatException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), ex, this);
			}
			catch (InvalidCastException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), ex3, this);
			}
			return obj2;
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0005D5BC File Offset: 0x0005B7BC
		public override object ReadElementContentAsObject()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAsObject");
			}
			XmlSchemaType xmlSchemaType;
			return this.InternalReadElementContentAsObject(out xmlSchemaType, true);
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0005D5E8 File Offset: 0x0005B7E8
		public override bool ReadElementContentAsBoolean()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAsBoolean");
			}
			XmlSchemaType xmlSchemaType;
			object obj = this.InternalReadElementContentAsObject(out xmlSchemaType);
			bool flag;
			try
			{
				if (xmlSchemaType != null)
				{
					flag = xmlSchemaType.ValueConverter.ToBoolean(obj);
				}
				else
				{
					flag = XmlUntypedConverter.Untyped.ToBoolean(obj);
				}
			}
			catch (FormatException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", ex, this);
			}
			catch (InvalidCastException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", ex3, this);
			}
			return flag;
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x0005D69C File Offset: 0x0005B89C
		public override DateTime ReadElementContentAsDateTime()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAsDateTime");
			}
			XmlSchemaType xmlSchemaType;
			object obj = this.InternalReadElementContentAsObject(out xmlSchemaType);
			DateTime dateTime;
			try
			{
				if (xmlSchemaType != null)
				{
					dateTime = xmlSchemaType.ValueConverter.ToDateTime(obj);
				}
				else
				{
					dateTime = XmlUntypedConverter.Untyped.ToDateTime(obj);
				}
			}
			catch (FormatException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", ex, this);
			}
			catch (InvalidCastException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", ex3, this);
			}
			return dateTime;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0005D750 File Offset: 0x0005B950
		public override double ReadElementContentAsDouble()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAsDouble");
			}
			XmlSchemaType xmlSchemaType;
			object obj = this.InternalReadElementContentAsObject(out xmlSchemaType);
			double num;
			try
			{
				if (xmlSchemaType != null)
				{
					num = xmlSchemaType.ValueConverter.ToDouble(obj);
				}
				else
				{
					num = XmlUntypedConverter.Untyped.ToDouble(obj);
				}
			}
			catch (FormatException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Double", ex, this);
			}
			catch (InvalidCastException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Double", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Double", ex3, this);
			}
			return num;
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0005D804 File Offset: 0x0005BA04
		public override float ReadElementContentAsFloat()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAsFloat");
			}
			XmlSchemaType xmlSchemaType;
			object obj = this.InternalReadElementContentAsObject(out xmlSchemaType);
			float num;
			try
			{
				if (xmlSchemaType != null)
				{
					num = xmlSchemaType.ValueConverter.ToSingle(obj);
				}
				else
				{
					num = XmlUntypedConverter.Untyped.ToSingle(obj);
				}
			}
			catch (FormatException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Float", ex, this);
			}
			catch (InvalidCastException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Float", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Float", ex3, this);
			}
			return num;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0005D8B8 File Offset: 0x0005BAB8
		public override decimal ReadElementContentAsDecimal()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAsDecimal");
			}
			XmlSchemaType xmlSchemaType;
			object obj = this.InternalReadElementContentAsObject(out xmlSchemaType);
			decimal num;
			try
			{
				if (xmlSchemaType != null)
				{
					num = xmlSchemaType.ValueConverter.ToDecimal(obj);
				}
				else
				{
					num = XmlUntypedConverter.Untyped.ToDecimal(obj);
				}
			}
			catch (FormatException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", ex, this);
			}
			catch (InvalidCastException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", ex3, this);
			}
			return num;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0005D96C File Offset: 0x0005BB6C
		public override int ReadElementContentAsInt()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAsInt");
			}
			XmlSchemaType xmlSchemaType;
			object obj = this.InternalReadElementContentAsObject(out xmlSchemaType);
			int num;
			try
			{
				if (xmlSchemaType != null)
				{
					num = xmlSchemaType.ValueConverter.ToInt32(obj);
				}
				else
				{
					num = XmlUntypedConverter.Untyped.ToInt32(obj);
				}
			}
			catch (FormatException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Int", ex, this);
			}
			catch (InvalidCastException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Int", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Int", ex3, this);
			}
			return num;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0005DA20 File Offset: 0x0005BC20
		public override long ReadElementContentAsLong()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAsLong");
			}
			XmlSchemaType xmlSchemaType;
			object obj = this.InternalReadElementContentAsObject(out xmlSchemaType);
			long num;
			try
			{
				if (xmlSchemaType != null)
				{
					num = xmlSchemaType.ValueConverter.ToInt64(obj);
				}
				else
				{
					num = XmlUntypedConverter.Untyped.ToInt64(obj);
				}
			}
			catch (FormatException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Long", ex, this);
			}
			catch (InvalidCastException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Long", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "Long", ex3, this);
			}
			return num;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0005DAD4 File Offset: 0x0005BCD4
		public override string ReadElementContentAsString()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAsString");
			}
			XmlSchemaType xmlSchemaType;
			object obj = this.InternalReadElementContentAsObject(out xmlSchemaType);
			string text;
			try
			{
				if (xmlSchemaType != null)
				{
					text = xmlSchemaType.ValueConverter.ToString(obj);
				}
				else
				{
					text = obj as string;
				}
			}
			catch (InvalidCastException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "String", ex, this);
			}
			catch (FormatException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "String", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "String", ex3, this);
			}
			return text;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0005DB80 File Offset: 0x0005BD80
		public override object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAs");
			}
			XmlSchemaType xmlSchemaType;
			string text;
			object obj = this.InternalReadElementContentAsObject(out xmlSchemaType, false, out text);
			object obj2;
			try
			{
				if (xmlSchemaType != null)
				{
					if (returnType == typeof(DateTimeOffset) && xmlSchemaType.Datatype is Datatype_dateTimeBase)
					{
						obj = text;
					}
					obj2 = xmlSchemaType.ValueConverter.ChangeType(obj, returnType, namespaceResolver);
				}
				else
				{
					obj2 = XmlUntypedConverter.Untyped.ChangeType(obj, returnType, namespaceResolver);
				}
			}
			catch (FormatException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), ex, this);
			}
			catch (InvalidCastException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), ex3, this);
			}
			return obj2;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000E6B RID: 3691 RVA: 0x0005DC60 File Offset: 0x0005BE60
		public override int AttributeCount
		{
			get
			{
				return this.attributeCount;
			}
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0005DC68 File Offset: 0x0005BE68
		public override string GetAttribute(string name)
		{
			string text = this.coreReader.GetAttribute(name);
			if (text == null && this.attributeCount > 0)
			{
				ValidatingReaderNodeData defaultAttribute = this.GetDefaultAttribute(name, false);
				if (defaultAttribute != null)
				{
					text = defaultAttribute.RawValue;
				}
			}
			return text;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0005DCA4 File Offset: 0x0005BEA4
		public override string GetAttribute(string name, string namespaceURI)
		{
			string attribute = this.coreReader.GetAttribute(name, namespaceURI);
			if (attribute == null && this.attributeCount > 0)
			{
				namespaceURI = ((namespaceURI == null) ? string.Empty : this.coreReaderNameTable.Get(namespaceURI));
				name = this.coreReaderNameTable.Get(name);
				if (name == null || namespaceURI == null)
				{
					return null;
				}
				ValidatingReaderNodeData defaultAttribute = this.GetDefaultAttribute(name, namespaceURI, false);
				if (defaultAttribute != null)
				{
					return defaultAttribute.RawValue;
				}
			}
			return attribute;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0005DD10 File Offset: 0x0005BF10
		public override string GetAttribute(int i)
		{
			if (this.attributeCount == 0)
			{
				return null;
			}
			if (i < this.coreReaderAttributeCount)
			{
				return this.coreReader.GetAttribute(i);
			}
			int num = i - this.coreReaderAttributeCount;
			return ((ValidatingReaderNodeData)this.defaultAttributes[num]).RawValue;
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0005DD5C File Offset: 0x0005BF5C
		public override bool MoveToAttribute(string name)
		{
			if (!this.coreReader.MoveToAttribute(name))
			{
				if (this.attributeCount > 0)
				{
					ValidatingReaderNodeData defaultAttribute = this.GetDefaultAttribute(name, true);
					if (defaultAttribute != null)
					{
						this.validationState = XsdValidatingReader.ValidatingReaderState.OnDefaultAttribute;
						this.attributePSVI = defaultAttribute.AttInfo;
						this.cachedNode = defaultAttribute;
						goto IL_0057;
					}
				}
				return false;
			}
			this.validationState = XsdValidatingReader.ValidatingReaderState.OnAttribute;
			this.attributePSVI = this.GetAttributePSVI(name);
			IL_0057:
			if (this.validationState == XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
			{
				this.readBinaryHelper.Finish();
				this.validationState = this.savedState;
			}
			return true;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0005DDE4 File Offset: 0x0005BFE4
		public override bool MoveToAttribute(string name, string ns)
		{
			name = this.coreReaderNameTable.Get(name);
			ns = ((ns != null) ? this.coreReaderNameTable.Get(ns) : string.Empty);
			if (name == null || ns == null)
			{
				return false;
			}
			if (this.coreReader.MoveToAttribute(name, ns))
			{
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnAttribute;
				if (this.inlineSchemaParser == null)
				{
					this.attributePSVI = this.GetAttributePSVI(name, ns);
				}
				else
				{
					this.attributePSVI = null;
				}
			}
			else
			{
				ValidatingReaderNodeData defaultAttribute = this.GetDefaultAttribute(name, ns, true);
				if (defaultAttribute == null)
				{
					return false;
				}
				this.attributePSVI = defaultAttribute.AttInfo;
				this.cachedNode = defaultAttribute;
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnDefaultAttribute;
			}
			if (this.validationState == XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
			{
				this.readBinaryHelper.Finish();
				this.validationState = this.savedState;
			}
			return true;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0005DEA4 File Offset: 0x0005C0A4
		public override void MoveToAttribute(int i)
		{
			if (i < 0 || i >= this.attributeCount)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			this.currentAttrIndex = i;
			if (i < this.coreReaderAttributeCount)
			{
				this.coreReader.MoveToAttribute(i);
				if (this.inlineSchemaParser == null)
				{
					this.attributePSVI = this.attributePSVINodes[i];
				}
				else
				{
					this.attributePSVI = null;
				}
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnAttribute;
			}
			else
			{
				int num = i - this.coreReaderAttributeCount;
				this.cachedNode = (ValidatingReaderNodeData)this.defaultAttributes[num];
				this.attributePSVI = this.cachedNode.AttInfo;
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnDefaultAttribute;
			}
			if (this.validationState == XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
			{
				this.readBinaryHelper.Finish();
				this.validationState = this.savedState;
			}
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0005DF68 File Offset: 0x0005C168
		public override bool MoveToFirstAttribute()
		{
			if (this.coreReader.MoveToFirstAttribute())
			{
				this.currentAttrIndex = 0;
				if (this.inlineSchemaParser == null)
				{
					this.attributePSVI = this.attributePSVINodes[0];
				}
				else
				{
					this.attributePSVI = null;
				}
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnAttribute;
			}
			else
			{
				if (this.defaultAttributes.Count <= 0)
				{
					return false;
				}
				this.cachedNode = (ValidatingReaderNodeData)this.defaultAttributes[0];
				this.attributePSVI = this.cachedNode.AttInfo;
				this.currentAttrIndex = 0;
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnDefaultAttribute;
			}
			if (this.validationState == XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
			{
				this.readBinaryHelper.Finish();
				this.validationState = this.savedState;
			}
			return true;
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0005E01C File Offset: 0x0005C21C
		public override bool MoveToNextAttribute()
		{
			if (this.currentAttrIndex + 1 < this.coreReaderAttributeCount)
			{
				this.coreReader.MoveToNextAttribute();
				this.currentAttrIndex++;
				if (this.inlineSchemaParser == null)
				{
					this.attributePSVI = this.attributePSVINodes[this.currentAttrIndex];
				}
				else
				{
					this.attributePSVI = null;
				}
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnAttribute;
			}
			else
			{
				if (this.currentAttrIndex + 1 >= this.attributeCount)
				{
					return false;
				}
				int num = this.currentAttrIndex + 1;
				this.currentAttrIndex = num;
				int num2 = num - this.coreReaderAttributeCount;
				this.cachedNode = (ValidatingReaderNodeData)this.defaultAttributes[num2];
				this.attributePSVI = this.cachedNode.AttInfo;
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnDefaultAttribute;
			}
			if (this.validationState == XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
			{
				this.readBinaryHelper.Finish();
				this.validationState = this.savedState;
			}
			return true;
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x0005E0FD File Offset: 0x0005C2FD
		public override bool MoveToElement()
		{
			if (this.coreReader.MoveToElement() || this.validationState < XsdValidatingReader.ValidatingReaderState.None)
			{
				this.currentAttrIndex = -1;
				this.validationState = XsdValidatingReader.ValidatingReaderState.ClearAttributes;
				return true;
			}
			return false;
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0005E128 File Offset: 0x0005C328
		public override bool Read()
		{
			switch (this.validationState)
			{
			case XsdValidatingReader.ValidatingReaderState.OnReadAttributeValue:
			case XsdValidatingReader.ValidatingReaderState.OnDefaultAttribute:
			case XsdValidatingReader.ValidatingReaderState.OnAttribute:
			case XsdValidatingReader.ValidatingReaderState.ClearAttributes:
				this.ClearAttributesInfo();
				if (this.inlineSchemaParser != null)
				{
					this.validationState = XsdValidatingReader.ValidatingReaderState.ParseInlineSchema;
					goto IL_007C;
				}
				this.validationState = XsdValidatingReader.ValidatingReaderState.Read;
				break;
			case XsdValidatingReader.ValidatingReaderState.None:
				return false;
			case XsdValidatingReader.ValidatingReaderState.Init:
				this.validationState = XsdValidatingReader.ValidatingReaderState.Read;
				if (this.coreReader.ReadState == ReadState.Interactive)
				{
					this.ProcessReaderEvent();
					return true;
				}
				break;
			case XsdValidatingReader.ValidatingReaderState.Read:
				break;
			case XsdValidatingReader.ValidatingReaderState.ParseInlineSchema:
				goto IL_007C;
			case XsdValidatingReader.ValidatingReaderState.ReadAhead:
				this.ClearAttributesInfo();
				this.ProcessReaderEvent();
				this.validationState = XsdValidatingReader.ValidatingReaderState.Read;
				return true;
			case XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent:
				this.validationState = this.savedState;
				this.readBinaryHelper.Finish();
				return this.Read();
			case XsdValidatingReader.ValidatingReaderState.ReaderClosed:
			case XsdValidatingReader.ValidatingReaderState.EOF:
				return false;
			default:
				return false;
			}
			if (this.coreReader.Read())
			{
				this.ProcessReaderEvent();
				return true;
			}
			this.validator.EndValidation();
			if (this.coreReader.EOF)
			{
				this.validationState = XsdValidatingReader.ValidatingReaderState.EOF;
			}
			return false;
			IL_007C:
			this.ProcessInlineSchema();
			return true;
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0005E22F File Offset: 0x0005C42F
		public override bool EOF
		{
			get
			{
				return this.coreReader.EOF;
			}
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x0005E23C File Offset: 0x0005C43C
		public override void Close()
		{
			this.coreReader.Close();
			this.validationState = XsdValidatingReader.ValidatingReaderState.ReaderClosed;
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x0005E250 File Offset: 0x0005C450
		public override ReadState ReadState
		{
			get
			{
				if (this.validationState != XsdValidatingReader.ValidatingReaderState.Init)
				{
					return this.coreReader.ReadState;
				}
				return ReadState.Initial;
			}
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0005E268 File Offset: 0x0005C468
		public override void Skip()
		{
			int depth = this.Depth;
			XmlNodeType nodeType = this.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				if (nodeType != XmlNodeType.Attribute)
				{
					goto IL_0081;
				}
				this.MoveToElement();
			}
			if (!this.coreReader.IsEmptyElement)
			{
				bool flag = true;
				if ((this.xmlSchemaInfo.IsUnionType || this.xmlSchemaInfo.IsDefault) && this.coreReader is XsdCachingReader)
				{
					flag = false;
				}
				this.coreReader.Skip();
				this.validationState = XsdValidatingReader.ValidatingReaderState.ReadAhead;
				if (flag)
				{
					this.validator.SkipToEndElement(this.xmlSchemaInfo);
				}
			}
			IL_0081:
			this.Read();
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x0005E2FD File Offset: 0x0005C4FD
		public override XmlNameTable NameTable
		{
			get
			{
				return this.coreReaderNameTable;
			}
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0005E305 File Offset: 0x0005C505
		public override string LookupNamespace(string prefix)
		{
			return this.thisNSResolver.LookupNamespace(prefix);
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x0005BE99 File Offset: 0x0005A099
		public override void ResolveEntity()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0005E314 File Offset: 0x0005C514
		public override bool ReadAttributeValue()
		{
			if (this.validationState == XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
			{
				this.readBinaryHelper.Finish();
				this.validationState = this.savedState;
			}
			if (this.NodeType != XmlNodeType.Attribute)
			{
				return false;
			}
			if (this.validationState == XsdValidatingReader.ValidatingReaderState.OnDefaultAttribute)
			{
				this.cachedNode = this.CreateDummyTextNode(this.cachedNode.RawValue, this.cachedNode.Depth + 1);
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnReadAttributeValue;
				return true;
			}
			return this.coreReader.ReadAttributeValue();
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x0001222F File Offset: 0x0001042F
		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0005E390 File Offset: 0x0005C590
		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			if (this.validationState != XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
			{
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
				this.savedState = this.validationState;
			}
			this.validationState = this.savedState;
			int num = this.readBinaryHelper.ReadContentAsBase64(buffer, index, count);
			this.savedState = this.validationState;
			this.validationState = XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent;
			return num;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0005E3FC File Offset: 0x0005C5FC
		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			if (this.validationState != XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
			{
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
				this.savedState = this.validationState;
			}
			this.validationState = this.savedState;
			int num = this.readBinaryHelper.ReadContentAsBinHex(buffer, index, count);
			this.savedState = this.validationState;
			this.validationState = XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent;
			return num;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0005E468 File Offset: 0x0005C668
		public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			if (this.validationState != XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
			{
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
				this.savedState = this.validationState;
			}
			this.validationState = this.savedState;
			int num = this.readBinaryHelper.ReadElementContentAsBase64(buffer, index, count);
			this.savedState = this.validationState;
			this.validationState = XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent;
			return num;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x0005E4D4 File Offset: 0x0005C6D4
		public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			if (this.validationState != XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
			{
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
				this.savedState = this.validationState;
			}
			this.validationState = this.savedState;
			int num = this.readBinaryHelper.ReadElementContentAsBinHex(buffer, index, count);
			this.savedState = this.validationState;
			this.validationState = XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent;
			return num;
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x0005E540 File Offset: 0x0005C740
		bool IXmlSchemaInfo.IsDefault
		{
			get
			{
				XmlNodeType nodeType = this.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType != XmlNodeType.Attribute)
					{
						if (nodeType == XmlNodeType.EndElement)
						{
							return this.xmlSchemaInfo.IsDefault;
						}
					}
					else if (this.attributePSVI != null)
					{
						return this.AttributeSchemaInfo.IsDefault;
					}
					return false;
				}
				if (!this.coreReader.IsEmptyElement)
				{
					this.GetIsDefault();
				}
				return this.xmlSchemaInfo.IsDefault;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0005E5A4 File Offset: 0x0005C7A4
		bool IXmlSchemaInfo.IsNil
		{
			get
			{
				XmlNodeType nodeType = this.NodeType;
				return (nodeType == XmlNodeType.Element || nodeType == XmlNodeType.EndElement) && this.xmlSchemaInfo.IsNil;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0005E5D0 File Offset: 0x0005C7D0
		XmlSchemaValidity IXmlSchemaInfo.Validity
		{
			get
			{
				XmlNodeType nodeType = this.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType != XmlNodeType.Attribute)
					{
						if (nodeType == XmlNodeType.EndElement)
						{
							return this.xmlSchemaInfo.Validity;
						}
					}
					else if (this.attributePSVI != null)
					{
						return this.AttributeSchemaInfo.Validity;
					}
					return XmlSchemaValidity.NotKnown;
				}
				if (this.coreReader.IsEmptyElement)
				{
					return this.xmlSchemaInfo.Validity;
				}
				if (this.xmlSchemaInfo.Validity == XmlSchemaValidity.Valid)
				{
					return XmlSchemaValidity.NotKnown;
				}
				return this.xmlSchemaInfo.Validity;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0005E64C File Offset: 0x0005C84C
		XmlSchemaSimpleType IXmlSchemaInfo.MemberType
		{
			get
			{
				XmlNodeType nodeType = this.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					if (!this.coreReader.IsEmptyElement)
					{
						this.GetMemberType();
					}
					return this.xmlSchemaInfo.MemberType;
				}
				if (nodeType != XmlNodeType.Attribute)
				{
					if (nodeType != XmlNodeType.EndElement)
					{
						return null;
					}
					return this.xmlSchemaInfo.MemberType;
				}
				else
				{
					if (this.attributePSVI != null)
					{
						return this.AttributeSchemaInfo.MemberType;
					}
					return null;
				}
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0005E6B4 File Offset: 0x0005C8B4
		XmlSchemaType IXmlSchemaInfo.SchemaType
		{
			get
			{
				XmlNodeType nodeType = this.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType != XmlNodeType.Attribute)
					{
						if (nodeType != XmlNodeType.EndElement)
						{
							return null;
						}
					}
					else
					{
						if (this.attributePSVI != null)
						{
							return this.AttributeSchemaInfo.SchemaType;
						}
						return null;
					}
				}
				return this.xmlSchemaInfo.SchemaType;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0005E6F8 File Offset: 0x0005C8F8
		XmlSchemaElement IXmlSchemaInfo.SchemaElement
		{
			get
			{
				if (this.NodeType == XmlNodeType.Element || this.NodeType == XmlNodeType.EndElement)
				{
					return this.xmlSchemaInfo.SchemaElement;
				}
				return null;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0005E71A File Offset: 0x0005C91A
		XmlSchemaAttribute IXmlSchemaInfo.SchemaAttribute
		{
			get
			{
				if (this.NodeType == XmlNodeType.Attribute && this.attributePSVI != null)
				{
					return this.AttributeSchemaInfo.SchemaAttribute;
				}
				return null;
			}
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x0001222F File Offset: 0x0001042F
		public bool HasLineInfo()
		{
			return true;
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0005E73A File Offset: 0x0005C93A
		public int LineNumber
		{
			get
			{
				if (this.lineInfo != null)
				{
					return this.lineInfo.LineNumber;
				}
				return 0;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x0005E751 File Offset: 0x0005C951
		public int LinePosition
		{
			get
			{
				if (this.lineInfo != null)
				{
					return this.lineInfo.LinePosition;
				}
				return 0;
			}
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0005E768 File Offset: 0x0005C968
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			if (this.coreReaderNSResolver != null)
			{
				return this.coreReaderNSResolver.GetNamespacesInScope(scope);
			}
			return this.nsManager.GetNamespacesInScope(scope);
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0005E78B File Offset: 0x0005C98B
		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			if (this.coreReaderNSResolver != null)
			{
				return this.coreReaderNSResolver.LookupNamespace(prefix);
			}
			return this.nsManager.LookupNamespace(prefix);
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0005E7AE File Offset: 0x0005C9AE
		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			if (this.coreReaderNSResolver != null)
			{
				return this.coreReaderNSResolver.LookupPrefix(namespaceName);
			}
			return this.nsManager.LookupPrefix(namespaceName);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0005E7D1 File Offset: 0x0005C9D1
		private object GetStringValue()
		{
			return this.coreReader.Value;
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x0005E7DE File Offset: 0x0005C9DE
		private XmlSchemaType ElementXmlType
		{
			get
			{
				return this.xmlSchemaInfo.XmlType;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0005E7EB File Offset: 0x0005C9EB
		private XmlSchemaType AttributeXmlType
		{
			get
			{
				if (this.attributePSVI != null)
				{
					return this.AttributeSchemaInfo.XmlType;
				}
				return null;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x0005E802 File Offset: 0x0005CA02
		private XmlSchemaInfo AttributeSchemaInfo
		{
			get
			{
				return this.attributePSVI.attributeSchemaInfo;
			}
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0005E810 File Offset: 0x0005CA10
		private void ProcessReaderEvent()
		{
			if (this.replayCache)
			{
				return;
			}
			switch (this.coreReader.NodeType)
			{
			case XmlNodeType.Element:
				this.ProcessElementEvent();
				return;
			case XmlNodeType.Attribute:
			case XmlNodeType.Entity:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.Document:
			case XmlNodeType.DocumentFragment:
			case XmlNodeType.Notation:
				break;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				this.validator.ValidateText(new XmlValueGetter(this.GetStringValue));
				return;
			case XmlNodeType.EntityReference:
				throw new InvalidOperationException();
			case XmlNodeType.DocumentType:
				this.validator.SetDtdSchemaInfo(this.coreReader.DtdInfo);
				break;
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				this.validator.ValidateWhitespace(new XmlValueGetter(this.GetStringValue));
				return;
			case XmlNodeType.EndElement:
				this.ProcessEndElementEvent();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0005E8D4 File Offset: 0x0005CAD4
		private void ProcessElementEvent()
		{
			if (!this.processInlineSchema || !this.IsXSDRoot(this.coreReader.LocalName, this.coreReader.NamespaceURI) || this.coreReader.Depth <= 0)
			{
				this.atomicValue = null;
				this.originalAtomicValueString = null;
				this.xmlSchemaInfo.Clear();
				if (this.manageNamespaces)
				{
					this.nsManager.PushScope();
				}
				string text = null;
				string text2 = null;
				string text3 = null;
				string text4 = null;
				if (this.coreReader.MoveToFirstAttribute())
				{
					do
					{
						string namespaceURI = this.coreReader.NamespaceURI;
						string localName = this.coreReader.LocalName;
						if (Ref.Equal(namespaceURI, this.NsXsi))
						{
							if (Ref.Equal(localName, this.XsiSchemaLocation))
							{
								text = this.coreReader.Value;
							}
							else if (Ref.Equal(localName, this.XsiNoNamespaceSchemaLocation))
							{
								text2 = this.coreReader.Value;
							}
							else if (Ref.Equal(localName, this.XsiType))
							{
								text4 = this.coreReader.Value;
							}
							else if (Ref.Equal(localName, this.XsiNil))
							{
								text3 = this.coreReader.Value;
							}
						}
						if (this.manageNamespaces && Ref.Equal(this.coreReader.NamespaceURI, this.NsXmlNs))
						{
							this.nsManager.AddNamespace((this.coreReader.Prefix.Length == 0) ? string.Empty : this.coreReader.LocalName, this.coreReader.Value);
						}
					}
					while (this.coreReader.MoveToNextAttribute());
					this.coreReader.MoveToElement();
				}
				this.validator.ValidateElement(this.coreReader.LocalName, this.coreReader.NamespaceURI, this.xmlSchemaInfo, text4, text3, text, text2);
				this.ValidateAttributes();
				this.validator.ValidateEndOfAttributes(this.xmlSchemaInfo);
				if (this.coreReader.IsEmptyElement)
				{
					this.ProcessEndElementEvent();
				}
				this.validationState = XsdValidatingReader.ValidatingReaderState.ClearAttributes;
				return;
			}
			this.xmlSchemaInfo.Clear();
			this.attributeCount = (this.coreReaderAttributeCount = this.coreReader.AttributeCount);
			if (!this.coreReader.IsEmptyElement)
			{
				this.inlineSchemaParser = new Parser(SchemaType.XSD, this.coreReaderNameTable, this.validator.SchemaSet.GetSchemaNames(this.coreReaderNameTable), this.validationEvent);
				this.inlineSchemaParser.StartParsing(this.coreReader, null);
				this.inlineSchemaParser.ParseReaderNode();
				this.validationState = XsdValidatingReader.ValidatingReaderState.ParseInlineSchema;
				return;
			}
			this.validationState = XsdValidatingReader.ValidatingReaderState.ClearAttributes;
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0005EB5C File Offset: 0x0005CD5C
		private void ProcessEndElementEvent()
		{
			this.atomicValue = this.validator.ValidateEndElement(this.xmlSchemaInfo);
			this.originalAtomicValueString = this.GetOriginalAtomicValueStringOfElement();
			if (this.xmlSchemaInfo.IsDefault)
			{
				int depth = this.coreReader.Depth;
				this.coreReader = this.GetCachingReader();
				this.cachingReader.RecordTextNode(this.xmlSchemaInfo.XmlType.ValueConverter.ToString(this.atomicValue), this.originalAtomicValueString, depth + 1, 0, 0);
				this.cachingReader.RecordEndElementNode();
				this.cachingReader.SetToReplayMode();
				this.replayCache = true;
				return;
			}
			if (this.manageNamespaces)
			{
				this.nsManager.PopScope();
			}
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x0005EC18 File Offset: 0x0005CE18
		private void ValidateAttributes()
		{
			this.attributeCount = (this.coreReaderAttributeCount = this.coreReader.AttributeCount);
			int num = 0;
			bool flag = false;
			if (this.coreReader.MoveToFirstAttribute())
			{
				do
				{
					string localName = this.coreReader.LocalName;
					string namespaceURI = this.coreReader.NamespaceURI;
					AttributePSVIInfo attributePSVIInfo = this.AddAttributePSVI(num);
					attributePSVIInfo.localName = localName;
					attributePSVIInfo.namespaceUri = namespaceURI;
					if (namespaceURI == this.NsXmlNs)
					{
						num++;
					}
					else
					{
						attributePSVIInfo.typedAttributeValue = this.validator.ValidateAttribute(localName, namespaceURI, this.valueGetter, attributePSVIInfo.attributeSchemaInfo);
						if (!flag)
						{
							flag = attributePSVIInfo.attributeSchemaInfo.Validity == XmlSchemaValidity.Invalid;
						}
						num++;
					}
				}
				while (this.coreReader.MoveToNextAttribute());
			}
			this.coreReader.MoveToElement();
			if (flag)
			{
				this.xmlSchemaInfo.Validity = XmlSchemaValidity.Invalid;
			}
			this.validator.GetUnspecifiedDefaultAttributes(this.defaultAttributes, true);
			this.attributeCount += this.defaultAttributes.Count;
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0005ED21 File Offset: 0x0005CF21
		private void ClearAttributesInfo()
		{
			this.attributeCount = 0;
			this.coreReaderAttributeCount = 0;
			this.currentAttrIndex = -1;
			this.defaultAttributes.Clear();
			this.attributePSVI = null;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0005ED4C File Offset: 0x0005CF4C
		private AttributePSVIInfo GetAttributePSVI(string name)
		{
			if (this.inlineSchemaParser != null)
			{
				return null;
			}
			string text;
			string text2;
			ValidateNames.SplitQName(name, out text, out text2);
			text = this.coreReaderNameTable.Add(text);
			text2 = this.coreReaderNameTable.Add(text2);
			string text3;
			if (text.Length == 0)
			{
				text3 = string.Empty;
			}
			else
			{
				text3 = this.thisNSResolver.LookupNamespace(text);
			}
			return this.GetAttributePSVI(text2, text3);
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0005EDAC File Offset: 0x0005CFAC
		private AttributePSVIInfo GetAttributePSVI(string localName, string ns)
		{
			for (int i = 0; i < this.coreReaderAttributeCount; i++)
			{
				AttributePSVIInfo attributePSVIInfo = this.attributePSVINodes[i];
				if (attributePSVIInfo != null && Ref.Equal(localName, attributePSVIInfo.localName) && Ref.Equal(ns, attributePSVIInfo.namespaceUri))
				{
					this.currentAttrIndex = i;
					return attributePSVIInfo;
				}
			}
			return null;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0005EE00 File Offset: 0x0005D000
		private ValidatingReaderNodeData GetDefaultAttribute(string name, bool updatePosition)
		{
			string text;
			string text2;
			ValidateNames.SplitQName(name, out text, out text2);
			text = this.coreReaderNameTable.Add(text);
			text2 = this.coreReaderNameTable.Add(text2);
			string text3;
			if (text.Length == 0)
			{
				text3 = string.Empty;
			}
			else
			{
				text3 = this.thisNSResolver.LookupNamespace(text);
			}
			return this.GetDefaultAttribute(text2, text3, updatePosition);
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0005EE58 File Offset: 0x0005D058
		private ValidatingReaderNodeData GetDefaultAttribute(string attrLocalName, string ns, bool updatePosition)
		{
			for (int i = 0; i < this.defaultAttributes.Count; i++)
			{
				ValidatingReaderNodeData validatingReaderNodeData = (ValidatingReaderNodeData)this.defaultAttributes[i];
				if (Ref.Equal(validatingReaderNodeData.LocalName, attrLocalName) && Ref.Equal(validatingReaderNodeData.Namespace, ns))
				{
					if (updatePosition)
					{
						this.currentAttrIndex = this.coreReader.AttributeCount + i;
					}
					return validatingReaderNodeData;
				}
			}
			return null;
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0005EEC4 File Offset: 0x0005D0C4
		private AttributePSVIInfo AddAttributePSVI(int attIndex)
		{
			AttributePSVIInfo attributePSVIInfo = this.attributePSVINodes[attIndex];
			if (attributePSVIInfo != null)
			{
				attributePSVIInfo.Reset();
				return attributePSVIInfo;
			}
			if (attIndex >= this.attributePSVINodes.Length - 1)
			{
				AttributePSVIInfo[] array = new AttributePSVIInfo[this.attributePSVINodes.Length * 2];
				Array.Copy(this.attributePSVINodes, 0, array, 0, this.attributePSVINodes.Length);
				this.attributePSVINodes = array;
			}
			attributePSVIInfo = this.attributePSVINodes[attIndex];
			if (attributePSVIInfo == null)
			{
				attributePSVIInfo = new AttributePSVIInfo();
				this.attributePSVINodes[attIndex] = attributePSVIInfo;
			}
			return attributePSVIInfo;
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0005EF3B File Offset: 0x0005D13B
		private bool IsXSDRoot(string localName, string ns)
		{
			return Ref.Equal(ns, this.NsXs) && Ref.Equal(localName, this.XsdSchema);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0005EF5C File Offset: 0x0005D15C
		private void ProcessInlineSchema()
		{
			if (this.coreReader.Read())
			{
				if (this.coreReader.NodeType == XmlNodeType.Element)
				{
					this.attributeCount = (this.coreReaderAttributeCount = this.coreReader.AttributeCount);
				}
				else
				{
					this.ClearAttributesInfo();
				}
				if (!this.inlineSchemaParser.ParseReaderNode())
				{
					this.inlineSchemaParser.FinishParsing();
					XmlSchema xmlSchema = this.inlineSchemaParser.XmlSchema;
					this.validator.AddSchema(xmlSchema);
					this.inlineSchemaParser = null;
					this.validationState = XsdValidatingReader.ValidatingReaderState.Read;
				}
			}
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0005EFE5 File Offset: 0x0005D1E5
		private object InternalReadContentAsObject()
		{
			return this.InternalReadContentAsObject(false);
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0005EFF0 File Offset: 0x0005D1F0
		private object InternalReadContentAsObject(bool unwrapTypedValue)
		{
			string text;
			return this.InternalReadContentAsObject(unwrapTypedValue, out text);
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0005F008 File Offset: 0x0005D208
		private object InternalReadContentAsObject(bool unwrapTypedValue, out string originalStringValue)
		{
			XmlNodeType nodeType = this.NodeType;
			if (nodeType == XmlNodeType.Attribute)
			{
				originalStringValue = this.Value;
				if (this.attributePSVI != null && this.attributePSVI.typedAttributeValue != null)
				{
					if (this.validationState == XsdValidatingReader.ValidatingReaderState.OnDefaultAttribute)
					{
						XmlSchemaAttribute schemaAttribute = this.attributePSVI.attributeSchemaInfo.SchemaAttribute;
						originalStringValue = ((schemaAttribute.DefaultValue != null) ? schemaAttribute.DefaultValue : schemaAttribute.FixedValue);
					}
					return this.ReturnBoxedValue(this.attributePSVI.typedAttributeValue, this.AttributeSchemaInfo.XmlType, unwrapTypedValue);
				}
				return this.Value;
			}
			else if (nodeType == XmlNodeType.EndElement)
			{
				if (this.atomicValue != null)
				{
					originalStringValue = this.originalAtomicValueString;
					return this.atomicValue;
				}
				originalStringValue = string.Empty;
				return string.Empty;
			}
			else
			{
				if (this.validator.CurrentContentType == XmlSchemaContentType.TextOnly)
				{
					object obj = this.ReturnBoxedValue(this.ReadTillEndElement(), this.xmlSchemaInfo.XmlType, unwrapTypedValue);
					originalStringValue = this.originalAtomicValueString;
					return obj;
				}
				XsdCachingReader xsdCachingReader = this.coreReader as XsdCachingReader;
				if (xsdCachingReader != null)
				{
					originalStringValue = xsdCachingReader.ReadOriginalContentAsString();
				}
				else
				{
					originalStringValue = base.InternalReadContentAsString();
				}
				return originalStringValue;
			}
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0005F10E File Offset: 0x0005D30E
		private object InternalReadElementContentAsObject(out XmlSchemaType xmlType)
		{
			return this.InternalReadElementContentAsObject(out xmlType, false);
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0005F118 File Offset: 0x0005D318
		private object InternalReadElementContentAsObject(out XmlSchemaType xmlType, bool unwrapTypedValue)
		{
			string text;
			return this.InternalReadElementContentAsObject(out xmlType, unwrapTypedValue, out text);
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0005F130 File Offset: 0x0005D330
		private object InternalReadElementContentAsObject(out XmlSchemaType xmlType, bool unwrapTypedValue, out string originalString)
		{
			xmlType = null;
			object obj;
			if (this.IsEmptyElement)
			{
				if (this.xmlSchemaInfo.ContentType == XmlSchemaContentType.TextOnly)
				{
					obj = this.ReturnBoxedValue(this.atomicValue, this.xmlSchemaInfo.XmlType, unwrapTypedValue);
				}
				else
				{
					obj = this.atomicValue;
				}
				originalString = this.originalAtomicValueString;
				xmlType = this.ElementXmlType;
				this.Read();
				return obj;
			}
			this.Read();
			if (this.NodeType == XmlNodeType.EndElement)
			{
				if (this.xmlSchemaInfo.IsDefault)
				{
					if (this.xmlSchemaInfo.ContentType == XmlSchemaContentType.TextOnly)
					{
						obj = this.ReturnBoxedValue(this.atomicValue, this.xmlSchemaInfo.XmlType, unwrapTypedValue);
					}
					else
					{
						obj = this.atomicValue;
					}
					originalString = this.originalAtomicValueString;
				}
				else
				{
					obj = string.Empty;
					originalString = string.Empty;
				}
			}
			else
			{
				if (this.NodeType == XmlNodeType.Element)
				{
					throw new XmlException("ReadElementContentAs() methods cannot be called on an element that has child elements.", string.Empty, this);
				}
				obj = this.InternalReadContentAsObject(unwrapTypedValue, out originalString);
				if (this.NodeType != XmlNodeType.EndElement)
				{
					throw new XmlException("ReadElementContentAs() methods cannot be called on an element that has child elements.", string.Empty, this);
				}
			}
			xmlType = this.ElementXmlType;
			this.Read();
			return obj;
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0005F248 File Offset: 0x0005D448
		private object ReadTillEndElement()
		{
			if (this.atomicValue == null)
			{
				while (this.coreReader.Read())
				{
					if (!this.replayCache)
					{
						switch (this.coreReader.NodeType)
						{
						case XmlNodeType.Element:
							this.ProcessReaderEvent();
							goto IL_010B;
						case XmlNodeType.Text:
						case XmlNodeType.CDATA:
							this.validator.ValidateText(new XmlValueGetter(this.GetStringValue));
							break;
						case XmlNodeType.Whitespace:
						case XmlNodeType.SignificantWhitespace:
							this.validator.ValidateWhitespace(new XmlValueGetter(this.GetStringValue));
							break;
						case XmlNodeType.EndElement:
							this.atomicValue = this.validator.ValidateEndElement(this.xmlSchemaInfo);
							this.originalAtomicValueString = this.GetOriginalAtomicValueStringOfElement();
							if (this.manageNamespaces)
							{
								this.nsManager.PopScope();
								goto IL_010B;
							}
							goto IL_010B;
						}
					}
				}
			}
			else
			{
				if (this.atomicValue == this)
				{
					this.atomicValue = null;
				}
				this.SwitchReader();
			}
			IL_010B:
			return this.atomicValue;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0005F368 File Offset: 0x0005D568
		private void SwitchReader()
		{
			XsdCachingReader xsdCachingReader = this.coreReader as XsdCachingReader;
			if (xsdCachingReader != null)
			{
				this.coreReader = xsdCachingReader.GetCoreReader();
			}
			this.replayCache = false;
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0005F398 File Offset: 0x0005D598
		private void ReadAheadForMemberType()
		{
			while (this.coreReader.Read())
			{
				switch (this.coreReader.NodeType)
				{
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					this.validator.ValidateText(new XmlValueGetter(this.GetStringValue));
					break;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					this.validator.ValidateWhitespace(new XmlValueGetter(this.GetStringValue));
					break;
				case XmlNodeType.EndElement:
					this.atomicValue = this.validator.ValidateEndElement(this.xmlSchemaInfo);
					this.originalAtomicValueString = this.GetOriginalAtomicValueStringOfElement();
					if (this.atomicValue == null)
					{
						this.atomicValue = this;
						return;
					}
					if (this.xmlSchemaInfo.IsDefault)
					{
						this.cachingReader.SwitchTextNodeAndEndElement(this.xmlSchemaInfo.XmlType.ValueConverter.ToString(this.atomicValue), this.originalAtomicValueString);
						return;
					}
					return;
				}
			}
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0005F4B4 File Offset: 0x0005D6B4
		private void GetIsDefault()
		{
			if (!(this.coreReader is XsdCachingReader) && this.xmlSchemaInfo.HasDefaultValue)
			{
				this.coreReader = this.GetCachingReader();
				if (this.xmlSchemaInfo.IsUnionType && !this.xmlSchemaInfo.IsNil)
				{
					this.ReadAheadForMemberType();
				}
				else if (this.coreReader.Read())
				{
					switch (this.coreReader.NodeType)
					{
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
						this.validator.ValidateText(new XmlValueGetter(this.GetStringValue));
						break;
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						this.validator.ValidateWhitespace(new XmlValueGetter(this.GetStringValue));
						break;
					case XmlNodeType.EndElement:
						this.atomicValue = this.validator.ValidateEndElement(this.xmlSchemaInfo);
						this.originalAtomicValueString = this.GetOriginalAtomicValueStringOfElement();
						if (this.xmlSchemaInfo.IsDefault)
						{
							this.cachingReader.SwitchTextNodeAndEndElement(this.xmlSchemaInfo.XmlType.ValueConverter.ToString(this.atomicValue), this.originalAtomicValueString);
						}
						break;
					}
				}
				this.cachingReader.SetToReplayMode();
				this.replayCache = true;
			}
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0005F618 File Offset: 0x0005D818
		private void GetMemberType()
		{
			if (this.xmlSchemaInfo.MemberType != null || this.atomicValue == this)
			{
				return;
			}
			if (!(this.coreReader is XsdCachingReader) && this.xmlSchemaInfo.IsUnionType && !this.xmlSchemaInfo.IsNil)
			{
				this.coreReader = this.GetCachingReader();
				this.ReadAheadForMemberType();
				this.cachingReader.SetToReplayMode();
				this.replayCache = true;
			}
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0005F688 File Offset: 0x0005D888
		private object ReturnBoxedValue(object typedValue, XmlSchemaType xmlType, bool unWrap)
		{
			if (typedValue != null)
			{
				if (unWrap && xmlType.Datatype.Variety == XmlSchemaDatatypeVariety.List && (xmlType.Datatype as Datatype_List).ItemType.Variety == XmlSchemaDatatypeVariety.Union)
				{
					typedValue = xmlType.ValueConverter.ChangeType(typedValue, xmlType.Datatype.ValueType, this.thisNSResolver);
				}
				return typedValue;
			}
			typedValue = this.validator.GetConcatenatedValue();
			return typedValue;
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0005F6F0 File Offset: 0x0005D8F0
		private XsdCachingReader GetCachingReader()
		{
			if (this.cachingReader == null)
			{
				this.cachingReader = new XsdCachingReader(this.coreReader, this.lineInfo, new CachingEventHandler(this.CachingCallBack));
			}
			else
			{
				this.cachingReader.Reset(this.coreReader);
			}
			this.lineInfo = this.cachingReader;
			return this.cachingReader;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0005F74D File Offset: 0x0005D94D
		internal ValidatingReaderNodeData CreateDummyTextNode(string attributeValue, int depth)
		{
			if (this.textNode == null)
			{
				this.textNode = new ValidatingReaderNodeData(XmlNodeType.Text);
			}
			this.textNode.Depth = depth;
			this.textNode.RawValue = attributeValue;
			return this.textNode;
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0005F781 File Offset: 0x0005D981
		internal void CachingCallBack(XsdCachingReader cachingReader)
		{
			this.coreReader = cachingReader.GetCoreReader();
			this.lineInfo = cachingReader.GetLineInfo();
			this.replayCache = false;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0005F7A4 File Offset: 0x0005D9A4
		private string GetOriginalAtomicValueStringOfElement()
		{
			if (!this.xmlSchemaInfo.IsDefault)
			{
				return this.validator.GetConcatenatedValue();
			}
			XmlSchemaElement schemaElement = this.xmlSchemaInfo.SchemaElement;
			if (schemaElement == null)
			{
				return string.Empty;
			}
			if (schemaElement.DefaultValue == null)
			{
				return schemaElement.FixedValue;
			}
			return schemaElement.DefaultValue;
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0005F7F4 File Offset: 0x0005D9F4
		public override Task<string> GetValueAsync()
		{
			if (this.validationState < XsdValidatingReader.ValidatingReaderState.None)
			{
				return Task.FromResult<string>(this.cachedNode.RawValue);
			}
			return this.coreReader.GetValueAsync();
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0005F81B File Offset: 0x0005DA1B
		public override Task<object> ReadContentAsObjectAsync()
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAsObject");
			}
			return this.InternalReadContentAsObjectAsync(true);
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0005F840 File Offset: 0x0005DA40
		public override async Task<string> ReadContentAsStringAsync()
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAsString");
			}
			object obj = await this.InternalReadContentAsObjectAsync().ConfigureAwait(false);
			XmlSchemaType xmlSchemaType = ((this.NodeType == XmlNodeType.Attribute) ? this.AttributeXmlType : this.ElementXmlType);
			string text;
			try
			{
				if (xmlSchemaType != null)
				{
					text = xmlSchemaType.ValueConverter.ToString(obj);
				}
				else
				{
					text = obj as string;
				}
			}
			catch (InvalidCastException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "String", ex, this);
			}
			catch (FormatException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "String", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "String", ex3, this);
			}
			return text;
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0005F884 File Offset: 0x0005DA84
		public override async Task<object> ReadContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException("ReadContentAs");
			}
			object obj = await this.InternalReadContentAsObjectTupleAsync(false).ConfigureAwait(false);
			string item = obj.Item1;
			object obj2 = obj.Item2;
			XmlSchemaType xmlSchemaType = ((this.NodeType == XmlNodeType.Attribute) ? this.AttributeXmlType : this.ElementXmlType);
			object obj3;
			try
			{
				if (xmlSchemaType != null)
				{
					if (returnType == typeof(DateTimeOffset) && xmlSchemaType.Datatype is Datatype_dateTimeBase)
					{
						obj2 = item;
					}
					obj3 = xmlSchemaType.ValueConverter.ChangeType(obj2, returnType);
				}
				else
				{
					obj3 = XmlUntypedConverter.Untyped.ChangeType(obj2, returnType, namespaceResolver);
				}
			}
			catch (FormatException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), ex, this);
			}
			catch (InvalidCastException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), ex3, this);
			}
			return obj3;
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0005F8D8 File Offset: 0x0005DAD8
		public override async Task<object> ReadElementContentAsObjectAsync()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAsObject");
			}
			return (await this.InternalReadElementContentAsObjectAsync(true).ConfigureAwait(false)).Item2;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0005F91C File Offset: 0x0005DB1C
		public override async Task<string> ReadElementContentAsStringAsync()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAsString");
			}
			object obj = await this.InternalReadElementContentAsObjectAsync().ConfigureAwait(false);
			XmlSchemaType item = obj.Item1;
			object item2 = obj.Item2;
			string text;
			try
			{
				if (item != null)
				{
					text = item.ValueConverter.ToString(item2);
				}
				else
				{
					text = item2 as string;
				}
			}
			catch (InvalidCastException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "String", ex, this);
			}
			catch (FormatException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "String", ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", "String", ex3, this);
			}
			return text;
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0005F960 File Offset: 0x0005DB60
		public override async Task<object> ReadElementContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw base.CreateReadElementContentAsException("ReadElementContentAs");
			}
			object obj = await this.InternalReadElementContentAsObjectTupleAsync(false).ConfigureAwait(false);
			XmlSchemaType item = obj.Item1;
			string item2 = obj.Item2;
			object obj2 = obj.Item3;
			object obj3;
			try
			{
				if (item != null)
				{
					if (returnType == typeof(DateTimeOffset) && item.Datatype is Datatype_dateTimeBase)
					{
						obj2 = item2;
					}
					obj3 = item.ValueConverter.ChangeType(obj2, returnType, namespaceResolver);
				}
				else
				{
					obj3 = XmlUntypedConverter.Untyped.ChangeType(obj2, returnType, namespaceResolver);
				}
			}
			catch (FormatException ex)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), ex, this);
			}
			catch (InvalidCastException ex2)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), ex2, this);
			}
			catch (OverflowException ex3)
			{
				throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), ex3, this);
			}
			return obj3;
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0005F9B4 File Offset: 0x0005DBB4
		private Task<bool> ReadAsync_Read(Task<bool> task)
		{
			if (!task.IsSuccess())
			{
				return this._ReadAsync_Read(task);
			}
			if (task.Result)
			{
				return this.ProcessReaderEventAsync().ReturnTaskBoolWhenFinish(true);
			}
			this.validator.EndValidation();
			if (this.coreReader.EOF)
			{
				this.validationState = XsdValidatingReader.ValidatingReaderState.EOF;
			}
			return AsyncHelper.DoneTaskFalse;
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0005FA0C File Offset: 0x0005DC0C
		private async Task<bool> _ReadAsync_Read(Task<bool> task)
		{
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = task.ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
			}
			bool flag;
			if (configuredTaskAwaiter.GetResult())
			{
				await this.ProcessReaderEventAsync().ConfigureAwait(false);
				flag = true;
			}
			else
			{
				this.validator.EndValidation();
				if (this.coreReader.EOF)
				{
					this.validationState = XsdValidatingReader.ValidatingReaderState.EOF;
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0005FA57 File Offset: 0x0005DC57
		private Task<bool> ReadAsync_ReadAhead(Task task)
		{
			if (task.IsSuccess())
			{
				this.validationState = XsdValidatingReader.ValidatingReaderState.Read;
				return AsyncHelper.DoneTaskTrue;
			}
			return this._ReadAsync_ReadAhead(task);
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0005FA78 File Offset: 0x0005DC78
		private async Task<bool> _ReadAsync_ReadAhead(Task task)
		{
			await task.ConfigureAwait(false);
			this.validationState = XsdValidatingReader.ValidatingReaderState.Read;
			return true;
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0005FAC4 File Offset: 0x0005DCC4
		public override Task<bool> ReadAsync()
		{
			switch (this.validationState)
			{
			case XsdValidatingReader.ValidatingReaderState.OnReadAttributeValue:
			case XsdValidatingReader.ValidatingReaderState.OnDefaultAttribute:
			case XsdValidatingReader.ValidatingReaderState.OnAttribute:
			case XsdValidatingReader.ValidatingReaderState.ClearAttributes:
				this.ClearAttributesInfo();
				if (this.inlineSchemaParser != null)
				{
					this.validationState = XsdValidatingReader.ValidatingReaderState.ParseInlineSchema;
					goto IL_0059;
				}
				this.validationState = XsdValidatingReader.ValidatingReaderState.Read;
				break;
			case XsdValidatingReader.ValidatingReaderState.None:
				goto IL_00F0;
			case XsdValidatingReader.ValidatingReaderState.Init:
				this.validationState = XsdValidatingReader.ValidatingReaderState.Read;
				if (this.coreReader.ReadState == ReadState.Interactive)
				{
					return this.ProcessReaderEventAsync().ReturnTaskBoolWhenFinish(true);
				}
				break;
			case XsdValidatingReader.ValidatingReaderState.Read:
				break;
			case XsdValidatingReader.ValidatingReaderState.ParseInlineSchema:
				goto IL_0059;
			case XsdValidatingReader.ValidatingReaderState.ReadAhead:
			{
				this.ClearAttributesInfo();
				Task task = this.ProcessReaderEventAsync();
				return this.ReadAsync_ReadAhead(task);
			}
			case XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent:
				this.validationState = this.savedState;
				return this.readBinaryHelper.FinishAsync().CallBoolTaskFuncWhenFinish(new Func<Task<bool>>(this.ReadAsync));
			case XsdValidatingReader.ValidatingReaderState.ReaderClosed:
			case XsdValidatingReader.ValidatingReaderState.EOF:
				return AsyncHelper.DoneTaskFalse;
			default:
				goto IL_00F0;
			}
			Task<bool> task2 = this.coreReader.ReadAsync();
			return this.ReadAsync_Read(task2);
			IL_0059:
			return this.ProcessInlineSchemaAsync().ReturnTaskBoolWhenFinish(true);
			IL_00F0:
			return AsyncHelper.DoneTaskFalse;
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0005FBC8 File Offset: 0x0005DDC8
		public override async Task SkipAsync()
		{
			int depth = this.Depth;
			XmlNodeType nodeType = this.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				if (nodeType != XmlNodeType.Attribute)
				{
					goto IL_0116;
				}
				this.MoveToElement();
			}
			if (!this.coreReader.IsEmptyElement)
			{
				bool callSkipToEndElem = true;
				if ((this.xmlSchemaInfo.IsUnionType || this.xmlSchemaInfo.IsDefault) && this.coreReader is XsdCachingReader)
				{
					callSkipToEndElem = false;
				}
				await this.coreReader.SkipAsync().ConfigureAwait(false);
				this.validationState = XsdValidatingReader.ValidatingReaderState.ReadAhead;
				if (callSkipToEndElem)
				{
					this.validator.SkipToEndElement(this.xmlSchemaInfo);
				}
			}
			IL_0116:
			await this.ReadAsync().ConfigureAwait(false);
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0005FC0C File Offset: 0x0005DE0C
		public override async Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
		{
			int num;
			if (this.ReadState != ReadState.Interactive)
			{
				num = 0;
			}
			else
			{
				if (this.validationState != XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
				{
					this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
					this.savedState = this.validationState;
				}
				this.validationState = this.savedState;
				int num2 = await this.readBinaryHelper.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(false);
				this.savedState = this.validationState;
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent;
				num = num2;
			}
			return num;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0005FC68 File Offset: 0x0005DE68
		public override async Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count)
		{
			int num;
			if (this.ReadState != ReadState.Interactive)
			{
				num = 0;
			}
			else
			{
				if (this.validationState != XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
				{
					this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
					this.savedState = this.validationState;
				}
				this.validationState = this.savedState;
				int num2 = await this.readBinaryHelper.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(false);
				this.savedState = this.validationState;
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent;
				num = num2;
			}
			return num;
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x0005FCC4 File Offset: 0x0005DEC4
		public override async Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count)
		{
			int num;
			if (this.ReadState != ReadState.Interactive)
			{
				num = 0;
			}
			else
			{
				if (this.validationState != XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
				{
					this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
					this.savedState = this.validationState;
				}
				this.validationState = this.savedState;
				int num2 = await this.readBinaryHelper.ReadElementContentAsBase64Async(buffer, index, count).ConfigureAwait(false);
				this.savedState = this.validationState;
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent;
				num = num2;
			}
			return num;
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0005FD20 File Offset: 0x0005DF20
		public override async Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count)
		{
			int num;
			if (this.ReadState != ReadState.Interactive)
			{
				num = 0;
			}
			else
			{
				if (this.validationState != XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent)
				{
					this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
					this.savedState = this.validationState;
				}
				this.validationState = this.savedState;
				int num2 = await this.readBinaryHelper.ReadElementContentAsBinHexAsync(buffer, index, count).ConfigureAwait(false);
				this.savedState = this.validationState;
				this.validationState = XsdValidatingReader.ValidatingReaderState.OnReadBinaryContent;
				num = num2;
			}
			return num;
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0005FD7C File Offset: 0x0005DF7C
		private Task ProcessReaderEventAsync()
		{
			if (this.replayCache)
			{
				return AsyncHelper.DoneTask;
			}
			switch (this.coreReader.NodeType)
			{
			case XmlNodeType.Element:
				return this.ProcessElementEventAsync();
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				this.validator.ValidateText(new XmlValueGetter(this.GetStringValue));
				break;
			case XmlNodeType.EntityReference:
				throw new InvalidOperationException();
			case XmlNodeType.DocumentType:
				this.validator.SetDtdSchemaInfo(this.coreReader.DtdInfo);
				break;
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				this.validator.ValidateWhitespace(new XmlValueGetter(this.GetStringValue));
				break;
			case XmlNodeType.EndElement:
				return this.ProcessEndElementEventAsync();
			}
			return AsyncHelper.DoneTask;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0005FE4C File Offset: 0x0005E04C
		private async Task ProcessElementEventAsync()
		{
			if (this.processInlineSchema && this.IsXSDRoot(this.coreReader.LocalName, this.coreReader.NamespaceURI) && this.coreReader.Depth > 0)
			{
				this.xmlSchemaInfo.Clear();
				this.attributeCount = (this.coreReaderAttributeCount = this.coreReader.AttributeCount);
				if (!this.coreReader.IsEmptyElement)
				{
					this.inlineSchemaParser = new Parser(SchemaType.XSD, this.coreReaderNameTable, this.validator.SchemaSet.GetSchemaNames(this.coreReaderNameTable), this.validationEvent);
					await this.inlineSchemaParser.StartParsingAsync(this.coreReader, null).ConfigureAwait(false);
					this.inlineSchemaParser.ParseReaderNode();
					this.validationState = XsdValidatingReader.ValidatingReaderState.ParseInlineSchema;
				}
				else
				{
					this.validationState = XsdValidatingReader.ValidatingReaderState.ClearAttributes;
				}
			}
			else
			{
				this.atomicValue = null;
				this.originalAtomicValueString = null;
				this.xmlSchemaInfo.Clear();
				if (this.manageNamespaces)
				{
					this.nsManager.PushScope();
				}
				string text = null;
				string text2 = null;
				string text3 = null;
				string text4 = null;
				if (this.coreReader.MoveToFirstAttribute())
				{
					do
					{
						string namespaceURI = this.coreReader.NamespaceURI;
						string localName = this.coreReader.LocalName;
						if (Ref.Equal(namespaceURI, this.NsXsi))
						{
							if (Ref.Equal(localName, this.XsiSchemaLocation))
							{
								text = this.coreReader.Value;
							}
							else if (Ref.Equal(localName, this.XsiNoNamespaceSchemaLocation))
							{
								text2 = this.coreReader.Value;
							}
							else if (Ref.Equal(localName, this.XsiType))
							{
								text4 = this.coreReader.Value;
							}
							else if (Ref.Equal(localName, this.XsiNil))
							{
								text3 = this.coreReader.Value;
							}
						}
						if (this.manageNamespaces && Ref.Equal(this.coreReader.NamespaceURI, this.NsXmlNs))
						{
							this.nsManager.AddNamespace((this.coreReader.Prefix.Length == 0) ? string.Empty : this.coreReader.LocalName, this.coreReader.Value);
						}
					}
					while (this.coreReader.MoveToNextAttribute());
					this.coreReader.MoveToElement();
				}
				this.validator.ValidateElement(this.coreReader.LocalName, this.coreReader.NamespaceURI, this.xmlSchemaInfo, text4, text3, text, text2);
				this.ValidateAttributes();
				this.validator.ValidateEndOfAttributes(this.xmlSchemaInfo);
				if (this.coreReader.IsEmptyElement)
				{
					await this.ProcessEndElementEventAsync().ConfigureAwait(false);
				}
				this.validationState = XsdValidatingReader.ValidatingReaderState.ClearAttributes;
			}
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0005FE90 File Offset: 0x0005E090
		private async Task ProcessEndElementEventAsync()
		{
			this.atomicValue = this.validator.ValidateEndElement(this.xmlSchemaInfo);
			this.originalAtomicValueString = this.GetOriginalAtomicValueStringOfElement();
			if (this.xmlSchemaInfo.IsDefault)
			{
				int depth = this.coreReader.Depth;
				this.coreReader = this.GetCachingReader();
				this.cachingReader.RecordTextNode(this.xmlSchemaInfo.XmlType.ValueConverter.ToString(this.atomicValue), this.originalAtomicValueString, depth + 1, 0, 0);
				this.cachingReader.RecordEndElementNode();
				await this.cachingReader.SetToReplayModeAsync().ConfigureAwait(false);
				this.replayCache = true;
			}
			else if (this.manageNamespaces)
			{
				this.nsManager.PopScope();
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0005FED4 File Offset: 0x0005E0D4
		private async Task ProcessInlineSchemaAsync()
		{
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.coreReader.ReadAsync().ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
			}
			if (configuredTaskAwaiter.GetResult())
			{
				if (this.coreReader.NodeType == XmlNodeType.Element)
				{
					int num = this.coreReader.AttributeCount;
					this.coreReaderAttributeCount = num;
					this.attributeCount = num;
				}
				else
				{
					this.ClearAttributesInfo();
				}
				if (!this.inlineSchemaParser.ParseReaderNode())
				{
					this.inlineSchemaParser.FinishParsing();
					XmlSchema xmlSchema = this.inlineSchemaParser.XmlSchema;
					this.validator.AddSchema(xmlSchema);
					this.inlineSchemaParser = null;
					this.validationState = XsdValidatingReader.ValidatingReaderState.Read;
				}
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0005FF17 File Offset: 0x0005E117
		private Task<object> InternalReadContentAsObjectAsync()
		{
			return this.InternalReadContentAsObjectAsync(false);
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0005FF20 File Offset: 0x0005E120
		private async Task<object> InternalReadContentAsObjectAsync(bool unwrapTypedValue)
		{
			return (await this.InternalReadContentAsObjectTupleAsync(unwrapTypedValue).ConfigureAwait(false)).Item2;
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0005FF6C File Offset: 0x0005E16C
		private async Task<Tuple<string, object>> InternalReadContentAsObjectTupleAsync(bool unwrapTypedValue)
		{
			XmlNodeType nodeType = this.NodeType;
			Tuple<string, object> tuple;
			if (nodeType == XmlNodeType.Attribute)
			{
				string text = this.Value;
				if (this.attributePSVI != null && this.attributePSVI.typedAttributeValue != null)
				{
					if (this.validationState == XsdValidatingReader.ValidatingReaderState.OnDefaultAttribute)
					{
						XmlSchemaAttribute schemaAttribute = this.attributePSVI.attributeSchemaInfo.SchemaAttribute;
						text = ((schemaAttribute.DefaultValue != null) ? schemaAttribute.DefaultValue : schemaAttribute.FixedValue);
					}
					tuple = new Tuple<string, object>(text, this.ReturnBoxedValue(this.attributePSVI.typedAttributeValue, this.AttributeSchemaInfo.XmlType, unwrapTypedValue));
				}
				else
				{
					tuple = new Tuple<string, object>(text, this.Value);
				}
			}
			else if (nodeType == XmlNodeType.EndElement)
			{
				if (this.atomicValue != null)
				{
					string text = this.originalAtomicValueString;
					tuple = new Tuple<string, object>(text, this.atomicValue);
				}
				else
				{
					string text = string.Empty;
					tuple = new Tuple<string, object>(text, string.Empty);
				}
			}
			else if (this.validator.CurrentContentType == XmlSchemaContentType.TextOnly)
			{
				object obj = await this.ReadTillEndElementAsync().ConfigureAwait(false);
				object obj2 = this.ReturnBoxedValue(obj, this.xmlSchemaInfo.XmlType, unwrapTypedValue);
				string text = this.originalAtomicValueString;
				tuple = new Tuple<string, object>(text, obj2);
			}
			else
			{
				XsdCachingReader xsdCachingReader = this.coreReader as XsdCachingReader;
				string text;
				if (xsdCachingReader != null)
				{
					text = xsdCachingReader.ReadOriginalContentAsString();
				}
				else
				{
					text = await base.InternalReadContentAsStringAsync().ConfigureAwait(false);
				}
				tuple = new Tuple<string, object>(text, text);
			}
			return tuple;
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0005FFB7 File Offset: 0x0005E1B7
		private Task<Tuple<XmlSchemaType, object>> InternalReadElementContentAsObjectAsync()
		{
			return this.InternalReadElementContentAsObjectAsync(false);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0005FFC0 File Offset: 0x0005E1C0
		private async Task<Tuple<XmlSchemaType, object>> InternalReadElementContentAsObjectAsync(bool unwrapTypedValue)
		{
			Tuple<XmlSchemaType, string, object> tuple = await this.InternalReadElementContentAsObjectTupleAsync(unwrapTypedValue).ConfigureAwait(false);
			return new Tuple<XmlSchemaType, object>(tuple.Item1, tuple.Item3);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0006000C File Offset: 0x0005E20C
		private async Task<Tuple<XmlSchemaType, string, object>> InternalReadElementContentAsObjectTupleAsync(bool unwrapTypedValue)
		{
			object typedValue = null;
			XmlSchemaType xmlType = null;
			Tuple<XmlSchemaType, string, object> tuple;
			if (this.IsEmptyElement)
			{
				if (this.xmlSchemaInfo.ContentType == XmlSchemaContentType.TextOnly)
				{
					typedValue = this.ReturnBoxedValue(this.atomicValue, this.xmlSchemaInfo.XmlType, unwrapTypedValue);
				}
				else
				{
					typedValue = this.atomicValue;
				}
				string originalString = this.originalAtomicValueString;
				xmlType = this.ElementXmlType;
				await this.ReadAsync().ConfigureAwait(false);
				tuple = new Tuple<XmlSchemaType, string, object>(xmlType, originalString, typedValue);
			}
			else
			{
				await this.ReadAsync().ConfigureAwait(false);
				string originalString;
				if (this.NodeType == XmlNodeType.EndElement)
				{
					if (this.xmlSchemaInfo.IsDefault)
					{
						if (this.xmlSchemaInfo.ContentType == XmlSchemaContentType.TextOnly)
						{
							typedValue = this.ReturnBoxedValue(this.atomicValue, this.xmlSchemaInfo.XmlType, unwrapTypedValue);
						}
						else
						{
							typedValue = this.atomicValue;
						}
						originalString = this.originalAtomicValueString;
					}
					else
					{
						typedValue = string.Empty;
						originalString = string.Empty;
					}
				}
				else
				{
					if (this.NodeType == XmlNodeType.Element)
					{
						throw new XmlException("ReadElementContentAs() methods cannot be called on an element that has child elements.", string.Empty, this);
					}
					Tuple<string, object> tuple2 = await this.InternalReadContentAsObjectTupleAsync(unwrapTypedValue).ConfigureAwait(false);
					originalString = tuple2.Item1;
					typedValue = tuple2.Item2;
					if (this.NodeType != XmlNodeType.EndElement)
					{
						throw new XmlException("ReadElementContentAs() methods cannot be called on an element that has child elements.", string.Empty, this);
					}
				}
				xmlType = this.ElementXmlType;
				await this.ReadAsync().ConfigureAwait(false);
				tuple = new Tuple<XmlSchemaType, string, object>(xmlType, originalString, typedValue);
			}
			return tuple;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00060058 File Offset: 0x0005E258
		private async Task<object> ReadTillEndElementAsync()
		{
			if (this.atomicValue == null)
			{
				for (;;)
				{
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.coreReader.ReadAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
					}
					if (!configuredTaskAwaiter.GetResult())
					{
						goto Block_6;
					}
					if (!this.replayCache)
					{
						switch (this.coreReader.NodeType)
						{
						case XmlNodeType.Element:
							goto IL_008B;
						case XmlNodeType.Text:
						case XmlNodeType.CDATA:
							this.validator.ValidateText(new XmlValueGetter(this.GetStringValue));
							break;
						case XmlNodeType.Whitespace:
						case XmlNodeType.SignificantWhitespace:
							this.validator.ValidateWhitespace(new XmlValueGetter(this.GetStringValue));
							break;
						case XmlNodeType.EndElement:
							goto IL_012A;
						}
					}
				}
				IL_008B:
				await this.ProcessReaderEventAsync().ConfigureAwait(false);
				goto IL_01F0;
				IL_012A:
				this.atomicValue = this.validator.ValidateEndElement(this.xmlSchemaInfo);
				this.originalAtomicValueString = this.GetOriginalAtomicValueStringOfElement();
				if (this.manageNamespaces)
				{
					this.nsManager.PopScope();
				}
				Block_6:;
			}
			else
			{
				if (this.atomicValue == this)
				{
					this.atomicValue = null;
				}
				this.SwitchReader();
			}
			IL_01F0:
			return this.atomicValue;
		}

		// Token: 0x04000F36 RID: 3894
		private XmlReader coreReader;

		// Token: 0x04000F37 RID: 3895
		private IXmlNamespaceResolver coreReaderNSResolver;

		// Token: 0x04000F38 RID: 3896
		private IXmlNamespaceResolver thisNSResolver;

		// Token: 0x04000F39 RID: 3897
		private XmlSchemaValidator validator;

		// Token: 0x04000F3A RID: 3898
		private XmlResolver xmlResolver;

		// Token: 0x04000F3B RID: 3899
		private ValidationEventHandler validationEvent;

		// Token: 0x04000F3C RID: 3900
		private XsdValidatingReader.ValidatingReaderState validationState;

		// Token: 0x04000F3D RID: 3901
		private XmlValueGetter valueGetter;

		// Token: 0x04000F3E RID: 3902
		private XmlNamespaceManager nsManager;

		// Token: 0x04000F3F RID: 3903
		private bool manageNamespaces;

		// Token: 0x04000F40 RID: 3904
		private bool processInlineSchema;

		// Token: 0x04000F41 RID: 3905
		private bool replayCache;

		// Token: 0x04000F42 RID: 3906
		private ValidatingReaderNodeData cachedNode;

		// Token: 0x04000F43 RID: 3907
		private AttributePSVIInfo attributePSVI;

		// Token: 0x04000F44 RID: 3908
		private int attributeCount;

		// Token: 0x04000F45 RID: 3909
		private int coreReaderAttributeCount;

		// Token: 0x04000F46 RID: 3910
		private int currentAttrIndex;

		// Token: 0x04000F47 RID: 3911
		private AttributePSVIInfo[] attributePSVINodes;

		// Token: 0x04000F48 RID: 3912
		private ArrayList defaultAttributes;

		// Token: 0x04000F49 RID: 3913
		private Parser inlineSchemaParser;

		// Token: 0x04000F4A RID: 3914
		private object atomicValue;

		// Token: 0x04000F4B RID: 3915
		private XmlSchemaInfo xmlSchemaInfo;

		// Token: 0x04000F4C RID: 3916
		private string originalAtomicValueString;

		// Token: 0x04000F4D RID: 3917
		private XmlNameTable coreReaderNameTable;

		// Token: 0x04000F4E RID: 3918
		private XsdCachingReader cachingReader;

		// Token: 0x04000F4F RID: 3919
		private ValidatingReaderNodeData textNode;

		// Token: 0x04000F50 RID: 3920
		private string NsXmlNs;

		// Token: 0x04000F51 RID: 3921
		private string NsXs;

		// Token: 0x04000F52 RID: 3922
		private string NsXsi;

		// Token: 0x04000F53 RID: 3923
		private string XsiType;

		// Token: 0x04000F54 RID: 3924
		private string XsiNil;

		// Token: 0x04000F55 RID: 3925
		private string XsdSchema;

		// Token: 0x04000F56 RID: 3926
		private string XsiSchemaLocation;

		// Token: 0x04000F57 RID: 3927
		private string XsiNoNamespaceSchemaLocation;

		// Token: 0x04000F58 RID: 3928
		private XmlCharType xmlCharType = XmlCharType.Instance;

		// Token: 0x04000F59 RID: 3929
		private IXmlLineInfo lineInfo;

		// Token: 0x04000F5A RID: 3930
		private ReadContentAsBinaryHelper readBinaryHelper;

		// Token: 0x04000F5B RID: 3931
		private XsdValidatingReader.ValidatingReaderState savedState;

		// Token: 0x04000F5C RID: 3932
		private const int InitialAttributeCount = 8;

		// Token: 0x04000F5D RID: 3933
		private static volatile Type TypeOfString;

		// Token: 0x0200018D RID: 397
		private enum ValidatingReaderState
		{
			// Token: 0x04000F5F RID: 3935
			None,
			// Token: 0x04000F60 RID: 3936
			Init,
			// Token: 0x04000F61 RID: 3937
			Read,
			// Token: 0x04000F62 RID: 3938
			OnDefaultAttribute = -1,
			// Token: 0x04000F63 RID: 3939
			OnReadAttributeValue = -2,
			// Token: 0x04000F64 RID: 3940
			OnAttribute = 3,
			// Token: 0x04000F65 RID: 3941
			ClearAttributes,
			// Token: 0x04000F66 RID: 3942
			ParseInlineSchema,
			// Token: 0x04000F67 RID: 3943
			ReadAhead,
			// Token: 0x04000F68 RID: 3944
			OnReadBinaryContent,
			// Token: 0x04000F69 RID: 3945
			ReaderClosed,
			// Token: 0x04000F6A RID: 3946
			EOF,
			// Token: 0x04000F6B RID: 3947
			Error
		}
	}
}
