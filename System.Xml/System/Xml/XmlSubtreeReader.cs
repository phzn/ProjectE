using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Xml
{
	// Token: 0x020000AB RID: 171
	internal sealed class XmlSubtreeReader : XmlWrappingReader, IXmlLineInfo, IXmlNamespaceResolver
	{
		// Token: 0x060006C7 RID: 1735 RVA: 0x00022460 File Offset: 0x00020660
		internal XmlSubtreeReader(XmlReader reader)
			: base(reader)
		{
			this.initialDepth = reader.Depth;
			this.state = XmlSubtreeReader.State.Initial;
			this.nsManager = new XmlNamespaceManager(reader.NameTable);
			this.xmlns = reader.NameTable.Add("xmlns");
			this.xmlnsUri = reader.NameTable.Add("http://www.w3.org/2000/xmlns/");
			this.tmpNode = new XmlSubtreeReader.NodeData();
			this.tmpNode.Set(XmlNodeType.None, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
			this.SetCurrentNode(this.tmpNode);
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x0002250E File Offset: 0x0002070E
		public override XmlNodeType NodeType
		{
			get
			{
				if (!this.useCurNode)
				{
					return this.reader.NodeType;
				}
				return this.curNode.type;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x0002252F File Offset: 0x0002072F
		public override string Name
		{
			get
			{
				if (!this.useCurNode)
				{
					return this.reader.Name;
				}
				return this.curNode.name;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x00022550 File Offset: 0x00020750
		public override string LocalName
		{
			get
			{
				if (!this.useCurNode)
				{
					return this.reader.LocalName;
				}
				return this.curNode.localName;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00022571 File Offset: 0x00020771
		public override string NamespaceURI
		{
			get
			{
				if (!this.useCurNode)
				{
					return this.reader.NamespaceURI;
				}
				return this.curNode.namespaceUri;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x00022592 File Offset: 0x00020792
		public override string Prefix
		{
			get
			{
				if (!this.useCurNode)
				{
					return this.reader.Prefix;
				}
				return this.curNode.prefix;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x000225B3 File Offset: 0x000207B3
		public override string Value
		{
			get
			{
				if (!this.useCurNode)
				{
					return this.reader.Value;
				}
				return this.curNode.value;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x000225D4 File Offset: 0x000207D4
		public override int Depth
		{
			get
			{
				int num = this.reader.Depth - this.initialDepth;
				if (this.curNsAttr != -1)
				{
					if (this.curNode.type == XmlNodeType.Text)
					{
						num += 2;
					}
					else
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x00022616 File Offset: 0x00020816
		public override string BaseURI
		{
			get
			{
				return this.reader.BaseURI;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00022623 File Offset: 0x00020823
		public override bool IsEmptyElement
		{
			get
			{
				return this.reader.IsEmptyElement;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00022630 File Offset: 0x00020830
		public override bool EOF
		{
			get
			{
				return this.state == XmlSubtreeReader.State.EndOfFile || this.state == XmlSubtreeReader.State.Closed;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00022646 File Offset: 0x00020846
		public override ReadState ReadState
		{
			get
			{
				if (this.reader.ReadState == ReadState.Error)
				{
					return ReadState.Error;
				}
				if (this.state <= XmlSubtreeReader.State.Closed)
				{
					return (ReadState)this.state;
				}
				return ReadState.Interactive;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00022669 File Offset: 0x00020869
		public override XmlNameTable NameTable
		{
			get
			{
				return this.reader.NameTable;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00022676 File Offset: 0x00020876
		public override int AttributeCount
		{
			get
			{
				if (!this.InAttributeActiveState)
				{
					return 0;
				}
				return this.reader.AttributeCount + this.nsAttrCount;
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00022694 File Offset: 0x00020894
		public override string GetAttribute(string name)
		{
			if (!this.InAttributeActiveState)
			{
				return null;
			}
			string attribute = this.reader.GetAttribute(name);
			if (attribute != null)
			{
				return attribute;
			}
			for (int i = 0; i < this.nsAttrCount; i++)
			{
				if (name == this.nsAttributes[i].name)
				{
					return this.nsAttributes[i].value;
				}
			}
			return null;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x000226F4 File Offset: 0x000208F4
		public override string GetAttribute(string name, string namespaceURI)
		{
			if (!this.InAttributeActiveState)
			{
				return null;
			}
			string attribute = this.reader.GetAttribute(name, namespaceURI);
			if (attribute != null)
			{
				return attribute;
			}
			for (int i = 0; i < this.nsAttrCount; i++)
			{
				if (name == this.nsAttributes[i].localName && namespaceURI == this.xmlnsUri)
				{
					return this.nsAttributes[i].value;
				}
			}
			return null;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00022764 File Offset: 0x00020964
		public override string GetAttribute(int i)
		{
			if (!this.InAttributeActiveState)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			int attributeCount = this.reader.AttributeCount;
			if (i < attributeCount)
			{
				return this.reader.GetAttribute(i);
			}
			if (i - attributeCount < this.nsAttrCount)
			{
				return this.nsAttributes[i - attributeCount].value;
			}
			throw new ArgumentOutOfRangeException("i");
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x000227C8 File Offset: 0x000209C8
		public override bool MoveToAttribute(string name)
		{
			if (!this.InAttributeActiveState)
			{
				return false;
			}
			if (this.reader.MoveToAttribute(name))
			{
				this.curNsAttr = -1;
				this.useCurNode = false;
				return true;
			}
			for (int i = 0; i < this.nsAttrCount; i++)
			{
				if (name == this.nsAttributes[i].name)
				{
					this.MoveToNsAttribute(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00022830 File Offset: 0x00020A30
		public override bool MoveToAttribute(string name, string ns)
		{
			if (!this.InAttributeActiveState)
			{
				return false;
			}
			if (this.reader.MoveToAttribute(name, ns))
			{
				this.curNsAttr = -1;
				this.useCurNode = false;
				return true;
			}
			for (int i = 0; i < this.nsAttrCount; i++)
			{
				if (name == this.nsAttributes[i].localName && ns == this.xmlnsUri)
				{
					this.MoveToNsAttribute(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x000228A4 File Offset: 0x00020AA4
		public override void MoveToAttribute(int i)
		{
			if (!this.InAttributeActiveState)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			int attributeCount = this.reader.AttributeCount;
			if (i < attributeCount)
			{
				this.reader.MoveToAttribute(i);
				this.curNsAttr = -1;
				this.useCurNode = false;
				return;
			}
			if (i - attributeCount < this.nsAttrCount)
			{
				this.MoveToNsAttribute(i - attributeCount);
				return;
			}
			throw new ArgumentOutOfRangeException("i");
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0002290E File Offset: 0x00020B0E
		public override bool MoveToFirstAttribute()
		{
			if (!this.InAttributeActiveState)
			{
				return false;
			}
			if (this.reader.MoveToFirstAttribute())
			{
				this.useCurNode = false;
				return true;
			}
			if (this.nsAttrCount > 0)
			{
				this.MoveToNsAttribute(0);
				return true;
			}
			return false;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00022944 File Offset: 0x00020B44
		public override bool MoveToNextAttribute()
		{
			if (!this.InAttributeActiveState)
			{
				return false;
			}
			if (this.curNsAttr == -1 && this.reader.MoveToNextAttribute())
			{
				return true;
			}
			if (this.curNsAttr + 1 < this.nsAttrCount)
			{
				this.MoveToNsAttribute(this.curNsAttr + 1);
				return true;
			}
			return false;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00022994 File Offset: 0x00020B94
		public override bool MoveToElement()
		{
			if (!this.InAttributeActiveState)
			{
				return false;
			}
			this.useCurNode = false;
			if (this.curNsAttr >= 0)
			{
				this.curNsAttr = -1;
				return true;
			}
			return this.reader.MoveToElement();
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x000229C4 File Offset: 0x00020BC4
		public override bool ReadAttributeValue()
		{
			if (!this.InAttributeActiveState)
			{
				return false;
			}
			if (this.curNsAttr == -1)
			{
				return this.reader.ReadAttributeValue();
			}
			if (this.curNode.type == XmlNodeType.Text)
			{
				return false;
			}
			this.tmpNode.type = XmlNodeType.Text;
			this.tmpNode.value = this.curNode.value;
			this.SetCurrentNode(this.tmpNode);
			return true;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00022A30 File Offset: 0x00020C30
		public override bool Read()
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
				this.useCurNode = false;
				this.state = XmlSubtreeReader.State.Interactive;
				this.ProcessNamespaces();
				return true;
			case XmlSubtreeReader.State.Interactive:
				break;
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return false;
			case XmlSubtreeReader.State.PopNamespaceScope:
				this.nsManager.PopScope();
				goto IL_00E5;
			case XmlSubtreeReader.State.ClearNsAttributes:
				goto IL_00E5;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
				return this.FinishReadElementContentAsBinary() && this.Read();
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				return this.FinishReadContentAsBinary() && this.Read();
			default:
				return false;
			}
			IL_0054:
			this.curNsAttr = -1;
			this.useCurNode = false;
			this.reader.MoveToElement();
			if (this.reader.Depth == this.initialDepth && (this.reader.NodeType == XmlNodeType.EndElement || (this.reader.NodeType == XmlNodeType.Element && this.reader.IsEmptyElement)))
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
				return false;
			}
			if (this.reader.Read())
			{
				this.ProcessNamespaces();
				return true;
			}
			this.SetEmptyNode();
			return false;
			IL_00E5:
			this.nsAttrCount = 0;
			this.state = XmlSubtreeReader.State.Interactive;
			goto IL_0054;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00022B58 File Offset: 0x00020D58
		public override void Close()
		{
			if (this.state == XmlSubtreeReader.State.Closed)
			{
				return;
			}
			try
			{
				if (this.state != XmlSubtreeReader.State.EndOfFile)
				{
					this.reader.MoveToElement();
					if (this.reader.Depth == this.initialDepth && this.reader.NodeType == XmlNodeType.Element && !this.reader.IsEmptyElement)
					{
						this.reader.Read();
					}
					while (this.reader.Depth > this.initialDepth && this.reader.Read())
					{
					}
				}
			}
			catch
			{
			}
			finally
			{
				this.curNsAttr = -1;
				this.useCurNode = false;
				this.state = XmlSubtreeReader.State.Closed;
				this.SetEmptyNode();
			}
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00022C1C File Offset: 0x00020E1C
		public override void Skip()
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
				this.Read();
				return;
			case XmlSubtreeReader.State.Interactive:
				break;
			case XmlSubtreeReader.State.Error:
				return;
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return;
			case XmlSubtreeReader.State.PopNamespaceScope:
				this.nsManager.PopScope();
				goto IL_011A;
			case XmlSubtreeReader.State.ClearNsAttributes:
				goto IL_011A;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
				if (this.FinishReadElementContentAsBinary())
				{
					this.Skip();
					return;
				}
				return;
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				if (this.FinishReadContentAsBinary())
				{
					this.Skip();
					return;
				}
				return;
			default:
				return;
			}
			IL_0042:
			this.curNsAttr = -1;
			this.useCurNode = false;
			this.reader.MoveToElement();
			if (this.reader.Depth == this.initialDepth)
			{
				if (this.reader.NodeType == XmlNodeType.Element && !this.reader.IsEmptyElement && this.reader.Read())
				{
					while (this.reader.NodeType != XmlNodeType.EndElement && this.reader.Depth > this.initialDepth)
					{
						this.reader.Skip();
					}
				}
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
				return;
			}
			if (this.reader.NodeType == XmlNodeType.Element && !this.reader.IsEmptyElement)
			{
				this.nsManager.PopScope();
			}
			this.reader.Skip();
			this.ProcessNamespaces();
			return;
			IL_011A:
			this.nsAttrCount = 0;
			this.state = XmlSubtreeReader.State.Interactive;
			goto IL_0042;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00022D78 File Offset: 0x00020F78
		public override object ReadContentAsObject()
		{
			object obj2;
			try
			{
				this.InitReadContentAsType("ReadContentAsObject");
				object obj = this.reader.ReadContentAsObject();
				this.FinishReadContentAsType();
				obj2 = obj;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return obj2;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00022DC0 File Offset: 0x00020FC0
		public override bool ReadContentAsBoolean()
		{
			bool flag2;
			try
			{
				this.InitReadContentAsType("ReadContentAsBoolean");
				bool flag = this.reader.ReadContentAsBoolean();
				this.FinishReadContentAsType();
				flag2 = flag;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return flag2;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00022E08 File Offset: 0x00021008
		public override DateTime ReadContentAsDateTime()
		{
			DateTime dateTime2;
			try
			{
				this.InitReadContentAsType("ReadContentAsDateTime");
				DateTime dateTime = this.reader.ReadContentAsDateTime();
				this.FinishReadContentAsType();
				dateTime2 = dateTime;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return dateTime2;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00022E50 File Offset: 0x00021050
		public override double ReadContentAsDouble()
		{
			double num2;
			try
			{
				this.InitReadContentAsType("ReadContentAsDouble");
				double num = this.reader.ReadContentAsDouble();
				this.FinishReadContentAsType();
				num2 = num;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return num2;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00022E98 File Offset: 0x00021098
		public override float ReadContentAsFloat()
		{
			float num2;
			try
			{
				this.InitReadContentAsType("ReadContentAsFloat");
				float num = this.reader.ReadContentAsFloat();
				this.FinishReadContentAsType();
				num2 = num;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return num2;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00022EE0 File Offset: 0x000210E0
		public override decimal ReadContentAsDecimal()
		{
			decimal num2;
			try
			{
				this.InitReadContentAsType("ReadContentAsDecimal");
				decimal num = this.reader.ReadContentAsDecimal();
				this.FinishReadContentAsType();
				num2 = num;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return num2;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00022F28 File Offset: 0x00021128
		public override int ReadContentAsInt()
		{
			int num2;
			try
			{
				this.InitReadContentAsType("ReadContentAsInt");
				int num = this.reader.ReadContentAsInt();
				this.FinishReadContentAsType();
				num2 = num;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return num2;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00022F70 File Offset: 0x00021170
		public override long ReadContentAsLong()
		{
			long num2;
			try
			{
				this.InitReadContentAsType("ReadContentAsLong");
				long num = this.reader.ReadContentAsLong();
				this.FinishReadContentAsType();
				num2 = num;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return num2;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00022FB8 File Offset: 0x000211B8
		public override string ReadContentAsString()
		{
			string text2;
			try
			{
				this.InitReadContentAsType("ReadContentAsString");
				string text = this.reader.ReadContentAsString();
				this.FinishReadContentAsType();
				text2 = text;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return text2;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00023000 File Offset: 0x00021200
		public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			object obj2;
			try
			{
				this.InitReadContentAsType("ReadContentAs");
				object obj = this.reader.ReadContentAs(returnType, namespaceResolver);
				this.FinishReadContentAsType();
				obj2 = obj;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return obj2;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0002304C File Offset: 0x0002124C
		public override bool CanReadBinaryContent
		{
			get
			{
				return this.reader.CanReadBinaryContent;
			}
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002305C File Offset: 0x0002125C
		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
				this.state = XmlSubtreeReader.State.ReadContentAsBase64;
				break;
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
			{
				XmlNodeType nodeType = this.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					throw base.CreateReadContentAsException("ReadContentAsBase64");
				case XmlNodeType.Attribute:
					if (this.curNsAttr != -1 && this.reader.CanReadBinaryContent)
					{
						this.CheckBuffer(buffer, index, count);
						if (count == 0)
						{
							return 0;
						}
						if (this.nsIncReadOffset == 0)
						{
							if (this.binDecoder != null && this.binDecoder is Base64Decoder)
							{
								this.binDecoder.Reset();
							}
							else
							{
								this.binDecoder = new Base64Decoder();
							}
						}
						if (this.nsIncReadOffset == this.curNode.value.Length)
						{
							return 0;
						}
						this.binDecoder.SetNextOutputBuffer(buffer, index, count);
						this.nsIncReadOffset += this.binDecoder.Decode(this.curNode.value, this.nsIncReadOffset, this.curNode.value.Length - this.nsIncReadOffset);
						return this.binDecoder.DecodedCount;
					}
					break;
				case XmlNodeType.Text:
					break;
				default:
					if (nodeType != XmlNodeType.EndElement)
					{
						return 0;
					}
					return 0;
				}
				return this.reader.ReadContentAsBase64(buffer, index, count);
			}
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			case XmlSubtreeReader.State.ReadContentAsBase64:
				break;
			default:
				return 0;
			}
			int num = this.reader.ReadContentAsBase64(buffer, index, count);
			if (num == 0)
			{
				this.state = XmlSubtreeReader.State.Interactive;
				this.ProcessNamespaces();
			}
			return num;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x000231F8 File Offset: 0x000213F8
		public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
				if (!this.InitReadElementContentAsBinary(XmlSubtreeReader.State.ReadElementContentAsBase64))
				{
					return 0;
				}
				break;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
				break;
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			default:
				return 0;
			}
			int num = this.reader.ReadContentAsBase64(buffer, index, count);
			if (num > 0 || count == 0)
			{
				return num;
			}
			if (this.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
			}
			this.state = XmlSubtreeReader.State.Interactive;
			this.ProcessNamespaces();
			if (this.reader.Depth == this.initialDepth)
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
			}
			else
			{
				this.Read();
			}
			return 0;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x000232EC File Offset: 0x000214EC
		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
				this.state = XmlSubtreeReader.State.ReadContentAsBinHex;
				break;
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
			{
				XmlNodeType nodeType = this.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					throw base.CreateReadContentAsException("ReadContentAsBinHex");
				case XmlNodeType.Attribute:
					if (this.curNsAttr != -1 && this.reader.CanReadBinaryContent)
					{
						this.CheckBuffer(buffer, index, count);
						if (count == 0)
						{
							return 0;
						}
						if (this.nsIncReadOffset == 0)
						{
							if (this.binDecoder != null && this.binDecoder is BinHexDecoder)
							{
								this.binDecoder.Reset();
							}
							else
							{
								this.binDecoder = new BinHexDecoder();
							}
						}
						if (this.nsIncReadOffset == this.curNode.value.Length)
						{
							return 0;
						}
						this.binDecoder.SetNextOutputBuffer(buffer, index, count);
						this.nsIncReadOffset += this.binDecoder.Decode(this.curNode.value, this.nsIncReadOffset, this.curNode.value.Length - this.nsIncReadOffset);
						return this.binDecoder.DecodedCount;
					}
					break;
				case XmlNodeType.Text:
					break;
				default:
					if (nodeType != XmlNodeType.EndElement)
					{
						return 0;
					}
					return 0;
				}
				return this.reader.ReadContentAsBinHex(buffer, index, count);
			}
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBase64:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				break;
			default:
				return 0;
			}
			int num = this.reader.ReadContentAsBinHex(buffer, index, count);
			if (num == 0)
			{
				this.state = XmlSubtreeReader.State.Interactive;
				this.ProcessNamespaces();
			}
			return num;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00023488 File Offset: 0x00021688
		public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
				if (!this.InitReadElementContentAsBinary(XmlSubtreeReader.State.ReadElementContentAsBinHex))
				{
					return 0;
				}
				break;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
				break;
			default:
				return 0;
			}
			int num = this.reader.ReadContentAsBinHex(buffer, index, count);
			if (num > 0 || count == 0)
			{
				return num;
			}
			if (this.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
			}
			this.state = XmlSubtreeReader.State.Interactive;
			this.ProcessNamespaces();
			if (this.reader.Depth == this.initialDepth)
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
			}
			else
			{
				this.Read();
			}
			return 0;
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0002357A File Offset: 0x0002177A
		public override bool CanReadValueChunk
		{
			get
			{
				return this.reader.CanReadValueChunk;
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00023588 File Offset: 0x00021788
		public override int ReadValueChunk(char[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
				break;
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
				if (this.curNsAttr != -1 && this.reader.CanReadValueChunk)
				{
					this.CheckBuffer(buffer, index, count);
					int num = this.curNode.value.Length - this.nsIncReadOffset;
					if (num > count)
					{
						num = count;
					}
					if (num > 0)
					{
						this.curNode.value.CopyTo(this.nsIncReadOffset, buffer, index, num);
					}
					this.nsIncReadOffset += num;
					return num;
				}
				break;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("ReadValueChunk calls cannot be mixed with ReadContentAsBase64 or ReadContentAsBinHex."));
			default:
				return 0;
			}
			return this.reader.ReadValueChunk(buffer, index, count);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0002365F File Offset: 0x0002185F
		public override string LookupNamespace(string prefix)
		{
			return ((IXmlNamespaceResolver)this).LookupNamespace(prefix);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00023668 File Offset: 0x00021868
		protected override void Dispose(bool disposing)
		{
			this.Close();
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00023670 File Offset: 0x00021870
		int IXmlLineInfo.LineNumber
		{
			get
			{
				if (!this.useCurNode)
				{
					IXmlLineInfo xmlLineInfo = this.reader as IXmlLineInfo;
					if (xmlLineInfo != null)
					{
						return xmlLineInfo.LineNumber;
					}
				}
				return 0;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0002369C File Offset: 0x0002189C
		int IXmlLineInfo.LinePosition
		{
			get
			{
				if (!this.useCurNode)
				{
					IXmlLineInfo xmlLineInfo = this.reader as IXmlLineInfo;
					if (xmlLineInfo != null)
					{
						return xmlLineInfo.LinePosition;
					}
				}
				return 0;
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000236C8 File Offset: 0x000218C8
		bool IXmlLineInfo.HasLineInfo()
		{
			return this.reader is IXmlLineInfo;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000236D8 File Offset: 0x000218D8
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			if (!this.InNamespaceActiveState)
			{
				return new Dictionary<string, string>();
			}
			return this.nsManager.GetNamespacesInScope(scope);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000236F4 File Offset: 0x000218F4
		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			if (!this.InNamespaceActiveState)
			{
				return null;
			}
			return this.nsManager.LookupNamespace(prefix);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002370C File Offset: 0x0002190C
		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			if (!this.InNamespaceActiveState)
			{
				return null;
			}
			return this.nsManager.LookupPrefix(namespaceName);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00023724 File Offset: 0x00021924
		private void ProcessNamespaces()
		{
			XmlNodeType nodeType = this.reader.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				if (nodeType != XmlNodeType.EndElement)
				{
					return;
				}
				this.state = XmlSubtreeReader.State.PopNamespaceScope;
			}
			else
			{
				this.nsManager.PushScope();
				string text = this.reader.Prefix;
				string text2 = this.reader.NamespaceURI;
				if (this.nsManager.LookupNamespace(text) != text2)
				{
					this.AddNamespace(text, text2);
				}
				if (this.reader.MoveToFirstAttribute())
				{
					do
					{
						text = this.reader.Prefix;
						text2 = this.reader.NamespaceURI;
						if (Ref.Equal(text2, this.xmlnsUri))
						{
							if (text.Length == 0)
							{
								this.nsManager.AddNamespace(string.Empty, this.reader.Value);
								this.RemoveNamespace(string.Empty, this.xmlns);
							}
							else
							{
								text = this.reader.LocalName;
								this.nsManager.AddNamespace(text, this.reader.Value);
								this.RemoveNamespace(this.xmlns, text);
							}
						}
						else if (text.Length != 0 && this.nsManager.LookupNamespace(text) != text2)
						{
							this.AddNamespace(text, text2);
						}
					}
					while (this.reader.MoveToNextAttribute());
					this.reader.MoveToElement();
				}
				if (this.reader.IsEmptyElement)
				{
					this.state = XmlSubtreeReader.State.PopNamespaceScope;
					return;
				}
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00023884 File Offset: 0x00021A84
		private void AddNamespace(string prefix, string ns)
		{
			this.nsManager.AddNamespace(prefix, ns);
			int num = this.nsAttrCount;
			this.nsAttrCount = num + 1;
			int num2 = num;
			if (this.nsAttributes == null)
			{
				this.nsAttributes = new XmlSubtreeReader.NodeData[this.InitialNamespaceAttributeCount];
			}
			if (num2 == this.nsAttributes.Length)
			{
				XmlSubtreeReader.NodeData[] array = new XmlSubtreeReader.NodeData[this.nsAttributes.Length * 2];
				Array.Copy(this.nsAttributes, 0, array, 0, num2);
				this.nsAttributes = array;
			}
			if (this.nsAttributes[num2] == null)
			{
				this.nsAttributes[num2] = new XmlSubtreeReader.NodeData();
			}
			if (prefix.Length == 0)
			{
				this.nsAttributes[num2].Set(XmlNodeType.Attribute, this.xmlns, string.Empty, this.xmlns, this.xmlnsUri, ns);
			}
			else
			{
				this.nsAttributes[num2].Set(XmlNodeType.Attribute, prefix, this.xmlns, this.reader.NameTable.Add(this.xmlns + ":" + prefix), this.xmlnsUri, ns);
			}
			this.state = XmlSubtreeReader.State.ClearNsAttributes;
			this.curNsAttr = -1;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0002398C File Offset: 0x00021B8C
		private void RemoveNamespace(string prefix, string localName)
		{
			for (int i = 0; i < this.nsAttrCount; i++)
			{
				if (Ref.Equal(prefix, this.nsAttributes[i].prefix) && Ref.Equal(localName, this.nsAttributes[i].localName))
				{
					if (i < this.nsAttrCount - 1)
					{
						XmlSubtreeReader.NodeData nodeData = this.nsAttributes[i];
						this.nsAttributes[i] = this.nsAttributes[this.nsAttrCount - 1];
						this.nsAttributes[this.nsAttrCount - 1] = nodeData;
					}
					this.nsAttrCount--;
					return;
				}
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00023A21 File Offset: 0x00021C21
		private void MoveToNsAttribute(int index)
		{
			this.reader.MoveToElement();
			this.curNsAttr = index;
			this.nsIncReadOffset = 0;
			this.SetCurrentNode(this.nsAttributes[index]);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00023A4C File Offset: 0x00021C4C
		private bool InitReadElementContentAsBinary(XmlSubtreeReader.State binaryState)
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw this.reader.CreateReadElementContentAsException("ReadElementContentAsBase64");
			}
			bool isEmptyElement = this.IsEmptyElement;
			if (!this.Read() || isEmptyElement)
			{
				return false;
			}
			XmlNodeType nodeType = this.NodeType;
			if (nodeType == XmlNodeType.Element)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
			}
			if (nodeType != XmlNodeType.EndElement)
			{
				this.state = binaryState;
				return true;
			}
			this.ProcessNamespaces();
			this.Read();
			return false;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00023AE4 File Offset: 0x00021CE4
		private bool FinishReadElementContentAsBinary()
		{
			byte[] array = new byte[256];
			if (this.state == XmlSubtreeReader.State.ReadElementContentAsBase64)
			{
				while (this.reader.ReadContentAsBase64(array, 0, 256) > 0)
				{
				}
			}
			else
			{
				while (this.reader.ReadContentAsBinHex(array, 0, 256) > 0)
				{
				}
			}
			if (this.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
			}
			this.state = XmlSubtreeReader.State.Interactive;
			this.ProcessNamespaces();
			if (this.reader.Depth == this.initialDepth)
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
				return false;
			}
			return this.Read();
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00023BA0 File Offset: 0x00021DA0
		private bool FinishReadContentAsBinary()
		{
			byte[] array = new byte[256];
			if (this.state == XmlSubtreeReader.State.ReadContentAsBase64)
			{
				while (this.reader.ReadContentAsBase64(array, 0, 256) > 0)
				{
				}
			}
			else
			{
				while (this.reader.ReadContentAsBinHex(array, 0, 256) > 0)
				{
				}
			}
			this.state = XmlSubtreeReader.State.Interactive;
			this.ProcessNamespaces();
			if (this.reader.Depth == this.initialDepth)
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
				return false;
			}
			return true;
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x00023C1E File Offset: 0x00021E1E
		private bool InAttributeActiveState
		{
			get
			{
				return (98 & (1 << (int)this.state)) != 0;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x00023C31 File Offset: 0x00021E31
		private bool InNamespaceActiveState
		{
			get
			{
				return (2018 & (1 << (int)this.state)) != 0;
			}
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00023C47 File Offset: 0x00021E47
		private void SetEmptyNode()
		{
			this.tmpNode.type = XmlNodeType.None;
			this.tmpNode.value = string.Empty;
			this.curNode = this.tmpNode;
			this.useCurNode = true;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00023C78 File Offset: 0x00021E78
		private void SetCurrentNode(XmlSubtreeReader.NodeData node)
		{
			this.curNode = node;
			this.useCurNode = true;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00023C88 File Offset: 0x00021E88
		private void InitReadContentAsType(string methodName)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				throw new InvalidOperationException(Res.GetString("The XmlReader is closed or in error state."));
			case XmlSubtreeReader.State.Interactive:
				return;
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
				return;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("ReadValueChunk calls cannot be mixed with ReadContentAsBase64 or ReadContentAsBinHex."));
			default:
				throw base.CreateReadContentAsException(methodName);
			}
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00023CFC File Offset: 0x00021EFC
		private void FinishReadContentAsType()
		{
			XmlNodeType nodeType = this.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				if (nodeType != XmlNodeType.Attribute)
				{
					if (nodeType != XmlNodeType.EndElement)
					{
						return;
					}
					this.state = XmlSubtreeReader.State.PopNamespaceScope;
				}
				return;
			}
			this.ProcessNamespaces();
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00023D2C File Offset: 0x00021F2C
		private void CheckBuffer(Array buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00023D7B File Offset: 0x00021F7B
		public override Task<string> GetValueAsync()
		{
			if (this.useCurNode)
			{
				return Task.FromResult<string>(this.curNode.value);
			}
			return this.reader.GetValueAsync();
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00023DA4 File Offset: 0x00021FA4
		public override async Task<bool> ReadAsync()
		{
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter;
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
				this.useCurNode = false;
				this.state = XmlSubtreeReader.State.Interactive;
				this.ProcessNamespaces();
				return true;
			case XmlSubtreeReader.State.Interactive:
				break;
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return false;
			case XmlSubtreeReader.State.PopNamespaceScope:
				this.nsManager.PopScope();
				goto IL_0188;
			case XmlSubtreeReader.State.ClearNsAttributes:
				goto IL_0188;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
				configuredTaskAwaiter = this.FinishReadElementContentAsBinaryAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					return false;
				}
				return await this.ReadAsync().ConfigureAwait(false);
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				configuredTaskAwaiter = this.FinishReadContentAsBinaryAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					return false;
				}
				return await this.ReadAsync().ConfigureAwait(false);
			default:
				return false;
			}
			IL_0081:
			this.curNsAttr = -1;
			this.useCurNode = false;
			this.reader.MoveToElement();
			if (this.reader.Depth == this.initialDepth && (this.reader.NodeType == XmlNodeType.EndElement || (this.reader.NodeType == XmlNodeType.Element && this.reader.IsEmptyElement)))
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
				return false;
			}
			configuredTaskAwaiter = this.reader.ReadAsync().ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
			}
			if (configuredTaskAwaiter.GetResult())
			{
				this.ProcessNamespaces();
				return true;
			}
			this.SetEmptyNode();
			return false;
			IL_0188:
			this.nsAttrCount = 0;
			this.state = XmlSubtreeReader.State.Interactive;
			goto IL_0081;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00023DE8 File Offset: 0x00021FE8
		public override async Task SkipAsync()
		{
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
				await this.ReadAsync().ConfigureAwait(false);
				return;
			case XmlSubtreeReader.State.Interactive:
				break;
			case XmlSubtreeReader.State.Error:
				return;
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				goto IL_02DD;
			case XmlSubtreeReader.State.PopNamespaceScope:
				this.nsManager.PopScope();
				goto IL_02EE;
			case XmlSubtreeReader.State.ClearNsAttributes:
				goto IL_02EE;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.FinishReadElementContentAsBinaryAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult())
				{
					await this.SkipAsync().ConfigureAwait(false);
					return;
				}
				return;
			}
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.FinishReadContentAsBinaryAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult())
				{
					await this.SkipAsync().ConfigureAwait(false);
					return;
				}
				return;
			}
			default:
				return;
			}
			IL_00DD:
			this.curNsAttr = -1;
			this.useCurNode = false;
			this.reader.MoveToElement();
			if (this.reader.Depth == this.initialDepth)
			{
				if (this.reader.NodeType == XmlNodeType.Element && !this.reader.IsEmptyElement)
				{
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.reader.ReadAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult())
					{
						while (this.reader.NodeType != XmlNodeType.EndElement && this.reader.Depth > this.initialDepth)
						{
							await this.reader.SkipAsync().ConfigureAwait(false);
						}
					}
				}
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
			}
			else
			{
				if (this.reader.NodeType == XmlNodeType.Element && !this.reader.IsEmptyElement)
				{
					this.nsManager.PopScope();
				}
				await this.reader.SkipAsync().ConfigureAwait(false);
				this.ProcessNamespaces();
			}
			IL_02DD:
			return;
			IL_02EE:
			this.nsAttrCount = 0;
			this.state = XmlSubtreeReader.State.Interactive;
			goto IL_00DD;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00023E2C File Offset: 0x0002202C
		public override async Task<object> ReadContentAsObjectAsync()
		{
			object obj2;
			try
			{
				this.InitReadContentAsType("ReadContentAsObject");
				object obj = await this.reader.ReadContentAsObjectAsync().ConfigureAwait(false);
				this.FinishReadContentAsType();
				obj2 = obj;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return obj2;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00023E70 File Offset: 0x00022070
		public override async Task<string> ReadContentAsStringAsync()
		{
			string text2;
			try
			{
				this.InitReadContentAsType("ReadContentAsString");
				string text = await this.reader.ReadContentAsStringAsync().ConfigureAwait(false);
				this.FinishReadContentAsType();
				text2 = text;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return text2;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00023EB4 File Offset: 0x000220B4
		public override async Task<object> ReadContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			object obj2;
			try
			{
				this.InitReadContentAsType("ReadContentAs");
				object obj = await this.reader.ReadContentAsAsync(returnType, namespaceResolver).ConfigureAwait(false);
				this.FinishReadContentAsType();
				obj2 = obj;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return obj2;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00023F08 File Offset: 0x00022108
		public override async Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
				this.state = XmlSubtreeReader.State.ReadContentAsBase64;
				break;
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
			{
				XmlNodeType nodeType = this.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					throw base.CreateReadContentAsException("ReadContentAsBase64");
				case XmlNodeType.Attribute:
					if (this.curNsAttr != -1 && this.reader.CanReadBinaryContent)
					{
						this.CheckBuffer(buffer, index, count);
						if (count == 0)
						{
							return 0;
						}
						if (this.nsIncReadOffset == 0)
						{
							if (this.binDecoder != null && this.binDecoder is Base64Decoder)
							{
								this.binDecoder.Reset();
							}
							else
							{
								this.binDecoder = new Base64Decoder();
							}
						}
						if (this.nsIncReadOffset == this.curNode.value.Length)
						{
							return 0;
						}
						this.binDecoder.SetNextOutputBuffer(buffer, index, count);
						this.nsIncReadOffset += this.binDecoder.Decode(this.curNode.value, this.nsIncReadOffset, this.curNode.value.Length - this.nsIncReadOffset);
						return this.binDecoder.DecodedCount;
					}
					break;
				case XmlNodeType.Text:
					break;
				default:
					if (nodeType != XmlNodeType.EndElement)
					{
						return 0;
					}
					return 0;
				}
				return await this.reader.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(false);
			}
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			case XmlSubtreeReader.State.ReadContentAsBase64:
				break;
			default:
				return 0;
			}
			object obj = await this.reader.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(false);
			if (obj == null)
			{
				this.state = XmlSubtreeReader.State.Interactive;
				this.ProcessNamespaces();
			}
			return obj;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00023F64 File Offset: 0x00022164
		public override async Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.InitReadElementContentAsBinaryAsync(XmlSubtreeReader.State.ReadElementContentAsBase64).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					return 0;
				}
				break;
			}
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
				break;
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			default:
				return 0;
			}
			int num = await this.reader.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(false);
			int num2;
			if (num > 0 || count == 0)
			{
				num2 = num;
			}
			else
			{
				if (this.NodeType != XmlNodeType.EndElement)
				{
					throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
				}
				this.state = XmlSubtreeReader.State.Interactive;
				this.ProcessNamespaces();
				if (this.reader.Depth == this.initialDepth)
				{
					this.state = XmlSubtreeReader.State.EndOfFile;
					this.SetEmptyNode();
				}
				else
				{
					await this.ReadAsync().ConfigureAwait(false);
				}
				num2 = 0;
			}
			return num2;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00023FC0 File Offset: 0x000221C0
		public override async Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
				this.state = XmlSubtreeReader.State.ReadContentAsBinHex;
				break;
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
			{
				XmlNodeType nodeType = this.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					throw base.CreateReadContentAsException("ReadContentAsBinHex");
				case XmlNodeType.Attribute:
					if (this.curNsAttr != -1 && this.reader.CanReadBinaryContent)
					{
						this.CheckBuffer(buffer, index, count);
						if (count == 0)
						{
							return 0;
						}
						if (this.nsIncReadOffset == 0)
						{
							if (this.binDecoder != null && this.binDecoder is BinHexDecoder)
							{
								this.binDecoder.Reset();
							}
							else
							{
								this.binDecoder = new BinHexDecoder();
							}
						}
						if (this.nsIncReadOffset == this.curNode.value.Length)
						{
							return 0;
						}
						this.binDecoder.SetNextOutputBuffer(buffer, index, count);
						this.nsIncReadOffset += this.binDecoder.Decode(this.curNode.value, this.nsIncReadOffset, this.curNode.value.Length - this.nsIncReadOffset);
						return this.binDecoder.DecodedCount;
					}
					break;
				case XmlNodeType.Text:
					break;
				default:
					if (nodeType != XmlNodeType.EndElement)
					{
						return 0;
					}
					return 0;
				}
				return await this.reader.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(false);
			}
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBase64:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				break;
			default:
				return 0;
			}
			object obj = await this.reader.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(false);
			if (obj == null)
			{
				this.state = XmlSubtreeReader.State.Interactive;
				this.ProcessNamespaces();
			}
			return obj;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0002401C File Offset: 0x0002221C
		public override async Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.InitReadElementContentAsBinaryAsync(XmlSubtreeReader.State.ReadElementContentAsBinHex).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					return 0;
				}
				break;
			}
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
				break;
			default:
				return 0;
			}
			int num = await this.reader.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(false);
			int num2;
			if (num > 0 || count == 0)
			{
				num2 = num;
			}
			else
			{
				if (this.NodeType != XmlNodeType.EndElement)
				{
					throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
				}
				this.state = XmlSubtreeReader.State.Interactive;
				this.ProcessNamespaces();
				if (this.reader.Depth == this.initialDepth)
				{
					this.state = XmlSubtreeReader.State.EndOfFile;
					this.SetEmptyNode();
				}
				else
				{
					await this.ReadAsync().ConfigureAwait(false);
				}
				num2 = 0;
			}
			return num2;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00024078 File Offset: 0x00022278
		public override Task<int> ReadValueChunkAsync(char[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return Task.FromResult<int>(0);
			case XmlSubtreeReader.State.Interactive:
				break;
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
				if (this.curNsAttr != -1 && this.reader.CanReadValueChunk)
				{
					this.CheckBuffer(buffer, index, count);
					int num = this.curNode.value.Length - this.nsIncReadOffset;
					if (num > count)
					{
						num = count;
					}
					if (num > 0)
					{
						this.curNode.value.CopyTo(this.nsIncReadOffset, buffer, index, num);
					}
					this.nsIncReadOffset += num;
					return Task.FromResult<int>(num);
				}
				break;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("ReadValueChunk calls cannot be mixed with ReadContentAsBase64 or ReadContentAsBinHex."));
			default:
				return Task.FromResult<int>(0);
			}
			return this.reader.ReadValueChunkAsync(buffer, index, count);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00024160 File Offset: 0x00022360
		private async Task<bool> InitReadElementContentAsBinaryAsync(XmlSubtreeReader.State binaryState)
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw this.reader.CreateReadElementContentAsException("ReadElementContentAsBase64");
			}
			bool isEmpty = this.IsEmptyElement;
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadAsync().ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
			}
			bool flag;
			if (!configuredTaskAwaiter.GetResult() || isEmpty)
			{
				flag = false;
			}
			else
			{
				XmlNodeType nodeType = this.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
				}
				if (nodeType != XmlNodeType.EndElement)
				{
					this.state = binaryState;
					flag = true;
				}
				else
				{
					this.ProcessNamespaces();
					await this.ReadAsync().ConfigureAwait(false);
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x000241AC File Offset: 0x000223AC
		private async Task<bool> FinishReadElementContentAsBinaryAsync()
		{
			byte[] bytes = new byte[256];
			if (this.state == XmlSubtreeReader.State.ReadElementContentAsBase64)
			{
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter;
				do
				{
					configuredTaskAwaiter = this.reader.ReadContentAsBase64Async(bytes, 0, 256).ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
					}
				}
				while (configuredTaskAwaiter.GetResult() > 0);
			}
			else
			{
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter;
				do
				{
					configuredTaskAwaiter = this.reader.ReadContentAsBinHexAsync(bytes, 0, 256).ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
					}
				}
				while (configuredTaskAwaiter.GetResult() > 0);
			}
			if (this.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
			}
			this.state = XmlSubtreeReader.State.Interactive;
			this.ProcessNamespaces();
			bool flag;
			if (this.reader.Depth == this.initialDepth)
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
				flag = false;
			}
			else
			{
				flag = await this.ReadAsync().ConfigureAwait(false);
			}
			return flag;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000241F0 File Offset: 0x000223F0
		private async Task<bool> FinishReadContentAsBinaryAsync()
		{
			byte[] bytes = new byte[256];
			if (this.state == XmlSubtreeReader.State.ReadContentAsBase64)
			{
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter;
				do
				{
					configuredTaskAwaiter = this.reader.ReadContentAsBase64Async(bytes, 0, 256).ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
					}
				}
				while (configuredTaskAwaiter.GetResult() > 0);
			}
			else
			{
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter;
				do
				{
					configuredTaskAwaiter = this.reader.ReadContentAsBinHexAsync(bytes, 0, 256).ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
					}
				}
				while (configuredTaskAwaiter.GetResult() > 0);
			}
			this.state = XmlSubtreeReader.State.Interactive;
			this.ProcessNamespaces();
			bool flag;
			if (this.reader.Depth == this.initialDepth)
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
				flag = false;
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x040008A9 RID: 2217
		private const int AttributeActiveStates = 98;

		// Token: 0x040008AA RID: 2218
		private const int NamespaceActiveStates = 2018;

		// Token: 0x040008AB RID: 2219
		private int initialDepth;

		// Token: 0x040008AC RID: 2220
		private XmlSubtreeReader.State state;

		// Token: 0x040008AD RID: 2221
		private XmlNamespaceManager nsManager;

		// Token: 0x040008AE RID: 2222
		private XmlSubtreeReader.NodeData[] nsAttributes;

		// Token: 0x040008AF RID: 2223
		private int nsAttrCount;

		// Token: 0x040008B0 RID: 2224
		private int curNsAttr = -1;

		// Token: 0x040008B1 RID: 2225
		private string xmlns;

		// Token: 0x040008B2 RID: 2226
		private string xmlnsUri;

		// Token: 0x040008B3 RID: 2227
		private int nsIncReadOffset;

		// Token: 0x040008B4 RID: 2228
		private IncrementalReadDecoder binDecoder;

		// Token: 0x040008B5 RID: 2229
		private bool useCurNode;

		// Token: 0x040008B6 RID: 2230
		private XmlSubtreeReader.NodeData curNode;

		// Token: 0x040008B7 RID: 2231
		private XmlSubtreeReader.NodeData tmpNode;

		// Token: 0x040008B8 RID: 2232
		internal int InitialNamespaceAttributeCount = 4;

		// Token: 0x020000AC RID: 172
		private class NodeData
		{
			// Token: 0x06000717 RID: 1815 RVA: 0x0000216B File Offset: 0x0000036B
			internal NodeData()
			{
			}

			// Token: 0x06000718 RID: 1816 RVA: 0x00024233 File Offset: 0x00022433
			internal void Set(XmlNodeType nodeType, string localName, string prefix, string name, string namespaceUri, string value)
			{
				this.type = nodeType;
				this.localName = localName;
				this.prefix = prefix;
				this.name = name;
				this.namespaceUri = namespaceUri;
				this.value = value;
			}

			// Token: 0x040008B9 RID: 2233
			internal XmlNodeType type;

			// Token: 0x040008BA RID: 2234
			internal string localName;

			// Token: 0x040008BB RID: 2235
			internal string prefix;

			// Token: 0x040008BC RID: 2236
			internal string name;

			// Token: 0x040008BD RID: 2237
			internal string namespaceUri;

			// Token: 0x040008BE RID: 2238
			internal string value;
		}

		// Token: 0x020000AD RID: 173
		private enum State
		{
			// Token: 0x040008C0 RID: 2240
			Initial,
			// Token: 0x040008C1 RID: 2241
			Interactive,
			// Token: 0x040008C2 RID: 2242
			Error,
			// Token: 0x040008C3 RID: 2243
			EndOfFile,
			// Token: 0x040008C4 RID: 2244
			Closed,
			// Token: 0x040008C5 RID: 2245
			PopNamespaceScope,
			// Token: 0x040008C6 RID: 2246
			ClearNsAttributes,
			// Token: 0x040008C7 RID: 2247
			ReadElementContentAsBase64,
			// Token: 0x040008C8 RID: 2248
			ReadElementContentAsBinHex,
			// Token: 0x040008C9 RID: 2249
			ReadContentAsBase64,
			// Token: 0x040008CA RID: 2250
			ReadContentAsBinHex
		}
	}
}
