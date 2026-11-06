using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x020001EA RID: 490
	internal class DtdParser : IDtdParser
	{
		// Token: 0x06001338 RID: 4920 RVA: 0x00071150 File Offset: 0x0006F350
		private DtdParser()
		{
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x000711BD File Offset: 0x0006F3BD
		internal static IDtdParser Create()
		{
			return new DtdParser();
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x000711C4 File Offset: 0x0006F3C4
		private void Initialize(IDtdParserAdapter readerAdapter)
		{
			this.readerAdapter = readerAdapter;
			this.readerAdapterWithValidation = readerAdapter as IDtdParserAdapterWithValidation;
			this.nameTable = readerAdapter.NameTable;
			IDtdParserAdapterWithValidation dtdParserAdapterWithValidation = readerAdapter as IDtdParserAdapterWithValidation;
			if (dtdParserAdapterWithValidation != null)
			{
				this.validate = dtdParserAdapterWithValidation.DtdValidation;
			}
			IDtdParserAdapterV1 dtdParserAdapterV = readerAdapter as IDtdParserAdapterV1;
			if (dtdParserAdapterV != null)
			{
				this.v1Compat = dtdParserAdapterV.V1CompatibilityMode;
				this.normalize = dtdParserAdapterV.Normalization;
				this.supportNamespaces = dtdParserAdapterV.Namespaces;
			}
			this.schemaInfo = new SchemaInfo();
			this.schemaInfo.SchemaType = SchemaType.DTD;
			this.stringBuilder = new StringBuilder();
			Uri baseUri = readerAdapter.BaseUri;
			if (baseUri != null)
			{
				this.documentBaseUri = baseUri.ToString();
			}
			this.freeFloatingDtd = false;
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0007127C File Offset: 0x0006F47C
		private void InitializeFreeFloatingDtd(string baseUri, string docTypeName, string publicId, string systemId, string internalSubset, IDtdParserAdapter adapter)
		{
			this.Initialize(adapter);
			if (docTypeName == null || docTypeName.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(docTypeName, "docTypeName");
			}
			XmlConvert.VerifyName(docTypeName);
			int num = docTypeName.IndexOf(':');
			if (num == -1)
			{
				this.schemaInfo.DocTypeName = new XmlQualifiedName(this.nameTable.Add(docTypeName));
			}
			else
			{
				this.schemaInfo.DocTypeName = new XmlQualifiedName(this.nameTable.Add(docTypeName.Substring(0, num)), this.nameTable.Add(docTypeName.Substring(num + 1)));
			}
			if (systemId != null && systemId.Length > 0)
			{
				int num2;
				if ((num2 = this.xmlCharType.IsOnlyCharData(systemId)) >= 0)
				{
					this.ThrowInvalidChar(this.curPos, systemId, num2);
				}
				this.systemId = systemId;
			}
			if (publicId != null && publicId.Length > 0)
			{
				int num2;
				if ((num2 = this.xmlCharType.IsPublicId(publicId)) >= 0)
				{
					this.ThrowInvalidChar(this.curPos, publicId, num2);
				}
				this.publicId = publicId;
			}
			if (internalSubset != null && internalSubset.Length > 0)
			{
				this.readerAdapter.PushInternalDtd(baseUri, internalSubset);
				this.hasFreeFloatingInternalSubset = true;
			}
			Uri baseUri2 = this.readerAdapter.BaseUri;
			if (baseUri2 != null)
			{
				this.documentBaseUri = baseUri2.ToString();
			}
			this.freeFloatingDtd = true;
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x000713C5 File Offset: 0x0006F5C5
		IDtdInfo IDtdParser.ParseInternalDtd(IDtdParserAdapter adapter, bool saveInternalSubset)
		{
			this.Initialize(adapter);
			this.Parse(saveInternalSubset);
			return this.schemaInfo;
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x000713DB File Offset: 0x0006F5DB
		IDtdInfo IDtdParser.ParseFreeFloatingDtd(string baseUri, string docTypeName, string publicId, string systemId, string internalSubset, IDtdParserAdapter adapter)
		{
			this.InitializeFreeFloatingDtd(baseUri, docTypeName, publicId, systemId, internalSubset, adapter);
			this.Parse(false);
			return this.schemaInfo;
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x0600133E RID: 4926 RVA: 0x000713F9 File Offset: 0x0006F5F9
		private bool ParsingInternalSubset
		{
			get
			{
				return this.externalEntitiesDepth == 0;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x0600133F RID: 4927 RVA: 0x00071404 File Offset: 0x0006F604
		private bool IgnoreEntityReferences
		{
			get
			{
				return this.scanningFunction == DtdParser.ScanningFunction.CondSection3;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x00071410 File Offset: 0x0006F610
		private bool SaveInternalSubsetValue
		{
			get
			{
				return this.readerAdapter.EntityStackLength == 0 && this.internalSubsetValueSb != null;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001341 RID: 4929 RVA: 0x0007142A File Offset: 0x0006F62A
		private bool ParsingTopLevelMarkup
		{
			get
			{
				return this.scanningFunction == DtdParser.ScanningFunction.SubsetContent || (this.scanningFunction == DtdParser.ScanningFunction.ParamEntitySpace && this.savedScanningFunction == DtdParser.ScanningFunction.SubsetContent);
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001342 RID: 4930 RVA: 0x0007144B File Offset: 0x0006F64B
		private bool SupportNamespaces
		{
			get
			{
				return this.supportNamespaces;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001343 RID: 4931 RVA: 0x00071453 File Offset: 0x0006F653
		private bool Normalize
		{
			get
			{
				return this.normalize;
			}
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x0007145C File Offset: 0x0006F65C
		private void Parse(bool saveInternalSubset)
		{
			if (this.freeFloatingDtd)
			{
				this.ParseFreeFloatingDtd();
			}
			else
			{
				this.ParseInDocumentDtd(saveInternalSubset);
			}
			this.schemaInfo.Finish();
			if (this.validate && this.undeclaredNotations != null)
			{
				foreach (DtdParser.UndeclaredNotation undeclaredNotation in this.undeclaredNotations.Values)
				{
					for (DtdParser.UndeclaredNotation undeclaredNotation2 = undeclaredNotation; undeclaredNotation2 != null; undeclaredNotation2 = undeclaredNotation2.next)
					{
						this.SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("The '{0}' notation is not declared.", undeclaredNotation.name, this.BaseUriStr, undeclaredNotation.lineNo, undeclaredNotation.linePos));
					}
				}
			}
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00071518 File Offset: 0x0006F718
		private void ParseInDocumentDtd(bool saveInternalSubset)
		{
			this.LoadParsingBuffer();
			this.scanningFunction = DtdParser.ScanningFunction.QName;
			this.nextScaningFunction = DtdParser.ScanningFunction.Doctype1;
			if (this.GetToken(false) != DtdParser.Token.QName)
			{
				this.OnUnexpectedError();
			}
			this.schemaInfo.DocTypeName = this.GetNameQualified(true);
			DtdParser.Token token = this.GetToken(false);
			if (token == DtdParser.Token.SYSTEM || token == DtdParser.Token.PUBLIC)
			{
				this.ParseExternalId(token, DtdParser.Token.DOCTYPE, out this.publicId, out this.systemId);
				token = this.GetToken(false);
			}
			if (token != DtdParser.Token.GreaterThan)
			{
				if (token == DtdParser.Token.LeftBracket)
				{
					if (saveInternalSubset)
					{
						this.SaveParsingBuffer();
						this.internalSubsetValueSb = new StringBuilder();
					}
					this.ParseInternalSubset();
				}
				else
				{
					this.OnUnexpectedError();
				}
			}
			this.SaveParsingBuffer();
			if (this.systemId != null && this.systemId.Length > 0)
			{
				this.ParseExternalSubset();
			}
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x000715D9 File Offset: 0x0006F7D9
		private void ParseFreeFloatingDtd()
		{
			if (this.hasFreeFloatingInternalSubset)
			{
				this.LoadParsingBuffer();
				this.ParseInternalSubset();
				this.SaveParsingBuffer();
			}
			if (this.systemId != null && this.systemId.Length > 0)
			{
				this.ParseExternalSubset();
			}
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x00071611 File Offset: 0x0006F811
		private void ParseInternalSubset()
		{
			this.ParseSubset();
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0007161C File Offset: 0x0006F81C
		private void ParseExternalSubset()
		{
			if (!this.readerAdapter.PushExternalSubset(this.systemId, this.publicId))
			{
				return;
			}
			Uri baseUri = this.readerAdapter.BaseUri;
			if (baseUri != null)
			{
				this.externalDtdBaseUri = baseUri.ToString();
			}
			this.externalEntitiesDepth++;
			this.LoadParsingBuffer();
			this.ParseSubset();
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00071680 File Offset: 0x0006F880
		private void ParseSubset()
		{
			for (;;)
			{
				DtdParser.Token token = this.GetToken(false);
				int num = this.currentEntityId;
				switch (token)
				{
				case DtdParser.Token.AttlistDecl:
					this.ParseAttlistDecl();
					break;
				case DtdParser.Token.ElementDecl:
					this.ParseElementDecl();
					break;
				case DtdParser.Token.EntityDecl:
					this.ParseEntityDecl();
					break;
				case DtdParser.Token.NotationDecl:
					this.ParseNotationDecl();
					break;
				case DtdParser.Token.Comment:
					this.ParseComment();
					break;
				case DtdParser.Token.PI:
					this.ParsePI();
					break;
				case DtdParser.Token.CondSectionStart:
					if (this.ParsingInternalSubset)
					{
						this.Throw(this.curPos - 3, "A conditional section is not allowed in an internal subset.");
					}
					this.ParseCondSection();
					num = this.currentEntityId;
					break;
				case DtdParser.Token.CondSectionEnd:
					if (this.condSectionDepth > 0)
					{
						this.condSectionDepth--;
						if (this.validate && this.currentEntityId != this.condSectionEntityIds[this.condSectionDepth])
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
						}
					}
					else
					{
						this.Throw(this.curPos - 3, "']]>' is not expected.");
					}
					break;
				case DtdParser.Token.Eof:
					goto IL_01A9;
				default:
					if (token == DtdParser.Token.RightBracket)
					{
						goto IL_0126;
					}
					break;
				}
				if (this.currentEntityId != num)
				{
					if (this.validate)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
					else if (!this.v1Compat)
					{
						this.Throw(this.curPos, "The parameter entity replacement text must nest properly within markup declarations.");
					}
				}
			}
			IL_0126:
			if (this.ParsingInternalSubset)
			{
				if (this.condSectionDepth != 0)
				{
					this.Throw(this.curPos, "There is an unclosed conditional section.");
				}
				if (this.internalSubsetValueSb != null)
				{
					this.SaveParsingBuffer(this.curPos - 1);
					this.schemaInfo.InternalDtdSubset = this.internalSubsetValueSb.ToString();
					this.internalSubsetValueSb = null;
				}
				if (this.GetToken(false) != DtdParser.Token.GreaterThan)
				{
					this.ThrowUnexpectedToken(this.curPos, ">");
					return;
				}
			}
			else
			{
				this.Throw(this.curPos, "Expected DTD markup was not found.");
			}
			return;
			IL_01A9:
			if (this.ParsingInternalSubset && !this.freeFloatingDtd)
			{
				this.Throw(this.curPos, "Incomplete DTD content.");
			}
			if (this.condSectionDepth != 0)
			{
				this.Throw(this.curPos, "There is an unclosed conditional section.");
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x000718C4 File Offset: 0x0006FAC4
		private void ParseAttlistDecl()
		{
			if (this.GetToken(true) == DtdParser.Token.QName)
			{
				XmlQualifiedName nameQualified = this.GetNameQualified(true);
				SchemaElementDecl schemaElementDecl;
				if (!this.schemaInfo.ElementDecls.TryGetValue(nameQualified, out schemaElementDecl) && !this.schemaInfo.UndeclaredElementDecls.TryGetValue(nameQualified, out schemaElementDecl))
				{
					schemaElementDecl = new SchemaElementDecl(nameQualified, nameQualified.Namespace);
					this.schemaInfo.UndeclaredElementDecls.Add(nameQualified, schemaElementDecl);
				}
				SchemaAttDef schemaAttDef = null;
				DtdParser.Token token;
				for (;;)
				{
					token = this.GetToken(false);
					if (token != DtdParser.Token.QName)
					{
						break;
					}
					XmlQualifiedName nameQualified2 = this.GetNameQualified(true);
					schemaAttDef = new SchemaAttDef(nameQualified2, nameQualified2.Namespace);
					schemaAttDef.IsDeclaredInExternal = !this.ParsingInternalSubset;
					schemaAttDef.LineNumber = this.LineNo;
					schemaAttDef.LinePosition = this.LinePos - (this.curPos - this.tokenStartPos);
					bool flag = schemaElementDecl.GetAttDef(schemaAttDef.Name) != null;
					this.ParseAttlistType(schemaAttDef, schemaElementDecl, flag);
					this.ParseAttlistDefault(schemaAttDef, flag);
					if (schemaAttDef.Prefix.Length > 0 && schemaAttDef.Prefix.Equals("xml"))
					{
						if (schemaAttDef.Name.Name == "space")
						{
							if (this.v1Compat)
							{
								string text = schemaAttDef.DefaultValueExpanded.Trim();
								if (text.Equals("preserve") || text.Equals("default"))
								{
									schemaAttDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
								}
							}
							else
							{
								schemaAttDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
								if (schemaAttDef.TokenizedType != XmlTokenizedType.ENUMERATION)
								{
									this.Throw("Enumeration data type required.", string.Empty, schemaAttDef.LineNumber, schemaAttDef.LinePosition);
								}
								if (this.validate)
								{
									schemaAttDef.CheckXmlSpace(this.readerAdapterWithValidation.ValidationEventHandling);
								}
							}
						}
						else if (schemaAttDef.Name.Name == "lang")
						{
							schemaAttDef.Reserved = SchemaAttDef.Reserve.XmlLang;
						}
					}
					if (!flag)
					{
						schemaElementDecl.AddAttDef(schemaAttDef);
					}
				}
				if (token == DtdParser.Token.GreaterThan)
				{
					if (this.v1Compat && schemaAttDef != null && schemaAttDef.Prefix.Length > 0 && schemaAttDef.Prefix.Equals("xml") && schemaAttDef.Name.Name == "space")
					{
						schemaAttDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
						if (schemaAttDef.Datatype.TokenizedType != XmlTokenizedType.ENUMERATION)
						{
							this.Throw("Enumeration data type required.", string.Empty, schemaAttDef.LineNumber, schemaAttDef.LinePosition);
						}
						if (this.validate)
						{
							schemaAttDef.CheckXmlSpace(this.readerAdapterWithValidation.ValidationEventHandling);
						}
					}
					return;
				}
			}
			this.OnUnexpectedError();
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00071B48 File Offset: 0x0006FD48
		private void ParseAttlistType(SchemaAttDef attrDef, SchemaElementDecl elementDecl, bool ignoreErrors)
		{
			DtdParser.Token token = this.GetToken(true);
			if (token != DtdParser.Token.CDATA)
			{
				elementDecl.HasNonCDataAttribute = true;
			}
			if (this.IsAttributeValueType(token))
			{
				attrDef.TokenizedType = (XmlTokenizedType)token;
				attrDef.SchemaType = XmlSchemaType.GetBuiltInSimpleType(attrDef.Datatype.TypeCode);
				if (token == DtdParser.Token.ID)
				{
					if (this.validate && elementDecl.IsIdDeclared)
					{
						SchemaAttDef attDef = elementDecl.GetAttDef(attrDef.Name);
						if ((attDef == null || attDef.Datatype.TokenizedType != XmlTokenizedType.ID) && !ignoreErrors)
						{
							this.SendValidationEvent(XmlSeverityType.Error, "The attribute of type ID is already declared on the '{0}' element.", elementDecl.Name.ToString());
						}
					}
					elementDecl.IsIdDeclared = true;
					return;
				}
				if (token != DtdParser.Token.NOTATION)
				{
					return;
				}
				if (this.validate)
				{
					if (elementDecl.IsNotationDeclared && !ignoreErrors)
					{
						this.SendValidationEvent(this.curPos - 8, XmlSeverityType.Error, "No element type can have more than one NOTATION attribute specified.", elementDecl.Name.ToString());
					}
					else
					{
						if (elementDecl.ContentValidator != null && elementDecl.ContentValidator.ContentType == XmlSchemaContentType.Empty && !ignoreErrors)
						{
							this.SendValidationEvent(this.curPos - 8, XmlSeverityType.Error, "An attribute of type NOTATION must not be declared on an element declared EMPTY.", elementDecl.Name.ToString());
						}
						elementDecl.IsNotationDeclared = true;
					}
				}
				if (this.GetToken(true) == DtdParser.Token.LeftParen && this.GetToken(false) == DtdParser.Token.Name)
				{
					do
					{
						string nameString = this.GetNameString();
						if (!this.schemaInfo.Notations.ContainsKey(nameString))
						{
							this.AddUndeclaredNotation(nameString);
						}
						if (this.validate && !this.v1Compat && attrDef.Values != null && attrDef.Values.Contains(nameString) && !ignoreErrors)
						{
							this.SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("'{0}' is a duplicate notation value.", nameString, this.BaseUriStr, this.LineNo, this.LinePos));
						}
						attrDef.AddValue(nameString);
						DtdParser.Token token2 = this.GetToken(false);
						if (token2 == DtdParser.Token.RightParen)
						{
							return;
						}
						if (token2 != DtdParser.Token.Or)
						{
							break;
						}
					}
					while (this.GetToken(false) == DtdParser.Token.Name);
				}
			}
			else if (token == DtdParser.Token.LeftParen)
			{
				attrDef.TokenizedType = XmlTokenizedType.ENUMERATION;
				attrDef.SchemaType = XmlSchemaType.GetBuiltInSimpleType(attrDef.Datatype.TypeCode);
				if (this.GetToken(false) == DtdParser.Token.Nmtoken)
				{
					attrDef.AddValue(this.GetNameString());
					for (;;)
					{
						DtdParser.Token token2 = this.GetToken(false);
						if (token2 == DtdParser.Token.RightParen)
						{
							break;
						}
						if (token2 != DtdParser.Token.Or || this.GetToken(false) != DtdParser.Token.Nmtoken)
						{
							goto IL_0280;
						}
						string nmtokenString = this.GetNmtokenString();
						if (this.validate && !this.v1Compat && attrDef.Values != null && attrDef.Values.Contains(nmtokenString) && !ignoreErrors)
						{
							this.SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("'{0}' is a duplicate enumeration value.", nmtokenString, this.BaseUriStr, this.LineNo, this.LinePos));
						}
						attrDef.AddValue(nmtokenString);
					}
					return;
				}
			}
			IL_0280:
			this.OnUnexpectedError();
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00071DDC File Offset: 0x0006FFDC
		private void ParseAttlistDefault(SchemaAttDef attrDef, bool ignoreErrors)
		{
			DtdParser.Token token = this.GetToken(true);
			switch (token)
			{
			case DtdParser.Token.REQUIRED:
				attrDef.Presence = SchemaDeclBase.Use.Required;
				return;
			case DtdParser.Token.IMPLIED:
				attrDef.Presence = SchemaDeclBase.Use.Implied;
				return;
			case DtdParser.Token.FIXED:
				attrDef.Presence = SchemaDeclBase.Use.Fixed;
				if (this.GetToken(true) != DtdParser.Token.Literal)
				{
					goto IL_00CF;
				}
				break;
			default:
				if (token != DtdParser.Token.Literal)
				{
					goto IL_00CF;
				}
				break;
			}
			if (this.validate && attrDef.Datatype.TokenizedType == XmlTokenizedType.ID && !ignoreErrors)
			{
				this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "An attribute of type ID must have a declared default of either #IMPLIED or #REQUIRED.", string.Empty);
			}
			if (attrDef.TokenizedType != XmlTokenizedType.CDATA)
			{
				attrDef.DefaultValueExpanded = this.GetValueWithStrippedSpaces();
			}
			else
			{
				attrDef.DefaultValueExpanded = this.GetValue();
			}
			attrDef.ValueLineNumber = this.literalLineInfo.lineNo;
			attrDef.ValueLinePosition = this.literalLineInfo.linePos + 1;
			DtdValidator.SetDefaultTypedValue(attrDef, this.readerAdapter);
			return;
			IL_00CF:
			this.OnUnexpectedError();
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00071EC0 File Offset: 0x000700C0
		private void ParseElementDecl()
		{
			if (this.GetToken(true) == DtdParser.Token.QName)
			{
				SchemaElementDecl schemaElementDecl = null;
				XmlQualifiedName nameQualified = this.GetNameQualified(true);
				if (this.schemaInfo.ElementDecls.TryGetValue(nameQualified, out schemaElementDecl))
				{
					if (this.validate)
					{
						this.SendValidationEvent(this.curPos - nameQualified.Name.Length, XmlSeverityType.Error, "The '{0}' element has already been declared.", this.GetNameString());
					}
				}
				else
				{
					if (this.schemaInfo.UndeclaredElementDecls.TryGetValue(nameQualified, out schemaElementDecl))
					{
						this.schemaInfo.UndeclaredElementDecls.Remove(nameQualified);
					}
					else
					{
						schemaElementDecl = new SchemaElementDecl(nameQualified, nameQualified.Namespace);
					}
					this.schemaInfo.ElementDecls.Add(nameQualified, schemaElementDecl);
				}
				schemaElementDecl.IsDeclaredInExternal = !this.ParsingInternalSubset;
				DtdParser.Token token = this.GetToken(true);
				if (token != DtdParser.Token.LeftParen)
				{
					if (token != DtdParser.Token.ANY)
					{
						if (token != DtdParser.Token.EMPTY)
						{
							goto IL_0181;
						}
						schemaElementDecl.ContentValidator = ContentValidator.Empty;
					}
					else
					{
						schemaElementDecl.ContentValidator = ContentValidator.Any;
					}
				}
				else
				{
					int num = this.currentEntityId;
					DtdParser.Token token2 = this.GetToken(false);
					if (token2 != DtdParser.Token.None)
					{
						if (token2 != DtdParser.Token.PCDATA)
						{
							goto IL_0181;
						}
						ParticleContentValidator particleContentValidator = new ParticleContentValidator(XmlSchemaContentType.Mixed);
						particleContentValidator.Start();
						particleContentValidator.OpenGroup();
						this.ParseElementMixedContent(particleContentValidator, num);
						schemaElementDecl.ContentValidator = particleContentValidator.Finish(true);
					}
					else
					{
						ParticleContentValidator particleContentValidator2 = new ParticleContentValidator(XmlSchemaContentType.ElementOnly);
						particleContentValidator2.Start();
						particleContentValidator2.OpenGroup();
						this.ParseElementOnlyContent(particleContentValidator2, num);
						schemaElementDecl.ContentValidator = particleContentValidator2.Finish(true);
					}
				}
				if (this.GetToken(false) != DtdParser.Token.GreaterThan)
				{
					this.ThrowUnexpectedToken(this.curPos, ">");
				}
				return;
			}
			IL_0181:
			this.OnUnexpectedError();
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00072054 File Offset: 0x00070254
		private void ParseElementOnlyContent(ParticleContentValidator pcv, int startParenEntityId)
		{
			Stack<DtdParser.ParseElementOnlyContent_LocalFrame> stack = new Stack<DtdParser.ParseElementOnlyContent_LocalFrame>();
			DtdParser.ParseElementOnlyContent_LocalFrame parseElementOnlyContent_LocalFrame = new DtdParser.ParseElementOnlyContent_LocalFrame(startParenEntityId);
			stack.Push(parseElementOnlyContent_LocalFrame);
			for (;;)
			{
				DtdParser.Token token = this.GetToken(false);
				if (token != DtdParser.Token.QName)
				{
					if (token == DtdParser.Token.LeftParen)
					{
						pcv.OpenGroup();
						parseElementOnlyContent_LocalFrame = new DtdParser.ParseElementOnlyContent_LocalFrame(this.currentEntityId);
						stack.Push(parseElementOnlyContent_LocalFrame);
						continue;
					}
					if (token != DtdParser.Token.GreaterThan)
					{
						goto IL_0148;
					}
					this.Throw(this.curPos, "Invalid content model.");
					goto IL_014E;
				}
				else
				{
					pcv.AddName(this.GetNameQualified(true), null);
					this.ParseHowMany(pcv);
				}
				IL_0078:
				token = this.GetToken(false);
				switch (token)
				{
				case DtdParser.Token.RightParen:
					pcv.CloseGroup();
					if (this.validate && this.currentEntityId != parseElementOnlyContent_LocalFrame.startParenEntityId)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
					this.ParseHowMany(pcv);
					break;
				case DtdParser.Token.GreaterThan:
					this.Throw(this.curPos, "Invalid content model.");
					break;
				case DtdParser.Token.Or:
					if (parseElementOnlyContent_LocalFrame.parsingSchema == DtdParser.Token.Comma)
					{
						this.Throw(this.curPos, "Invalid content model.");
					}
					pcv.AddChoice();
					parseElementOnlyContent_LocalFrame.parsingSchema = DtdParser.Token.Or;
					continue;
				default:
					if (token == DtdParser.Token.Comma)
					{
						if (parseElementOnlyContent_LocalFrame.parsingSchema == DtdParser.Token.Or)
						{
							this.Throw(this.curPos, "Invalid content model.");
						}
						pcv.AddSequence();
						parseElementOnlyContent_LocalFrame.parsingSchema = DtdParser.Token.Comma;
						continue;
					}
					goto IL_0148;
				}
				IL_014E:
				stack.Pop();
				if (stack.Count > 0)
				{
					parseElementOnlyContent_LocalFrame = stack.Peek();
					goto IL_0078;
				}
				break;
				IL_0148:
				this.OnUnexpectedError();
				goto IL_014E;
			}
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x000721CC File Offset: 0x000703CC
		private void ParseHowMany(ParticleContentValidator pcv)
		{
			switch (this.GetToken(false))
			{
			case DtdParser.Token.Star:
				pcv.AddStar();
				return;
			case DtdParser.Token.QMark:
				pcv.AddQMark();
				return;
			case DtdParser.Token.Plus:
				pcv.AddPlus();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0007220C File Offset: 0x0007040C
		private void ParseElementMixedContent(ParticleContentValidator pcv, int startParenEntityId)
		{
			bool flag = false;
			int num = -1;
			int num2 = this.currentEntityId;
			for (;;)
			{
				DtdParser.Token token = this.GetToken(false);
				if (token == DtdParser.Token.RightParen)
				{
					break;
				}
				if (token == DtdParser.Token.Or)
				{
					if (!flag)
					{
						flag = true;
					}
					else
					{
						pcv.AddChoice();
					}
					if (this.validate)
					{
						num = this.currentEntityId;
						if (num2 < num)
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
						}
					}
					if (this.GetToken(false) == DtdParser.Token.QName)
					{
						XmlQualifiedName nameQualified = this.GetNameQualified(true);
						if (pcv.Exists(nameQualified) && this.validate)
						{
							this.SendValidationEvent(XmlSeverityType.Error, "The '{0}' element already exists in the content model.", nameQualified.ToString());
						}
						pcv.AddName(nameQualified, null);
						if (!this.validate)
						{
							continue;
						}
						num2 = this.currentEntityId;
						if (num2 < num)
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
							continue;
						}
						continue;
					}
				}
				this.OnUnexpectedError();
			}
			pcv.CloseGroup();
			if (this.validate && this.currentEntityId != startParenEntityId)
			{
				this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
			}
			if (this.GetToken(false) == DtdParser.Token.Star && flag)
			{
				pcv.AddStar();
				return;
			}
			if (flag)
			{
				this.ThrowUnexpectedToken(this.curPos, "*");
			}
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0007234C File Offset: 0x0007054C
		private void ParseEntityDecl()
		{
			bool flag = false;
			DtdParser.Token token = this.GetToken(true);
			if (token != DtdParser.Token.Name)
			{
				if (token != DtdParser.Token.Percent)
				{
					goto IL_01D6;
				}
				flag = true;
				if (this.GetToken(true) != DtdParser.Token.Name)
				{
					goto IL_01D6;
				}
			}
			XmlQualifiedName nameQualified = this.GetNameQualified(false);
			SchemaEntity schemaEntity = new SchemaEntity(nameQualified, flag);
			schemaEntity.BaseURI = this.BaseUriStr;
			schemaEntity.DeclaredURI = ((this.externalDtdBaseUri.Length == 0) ? this.documentBaseUri : this.externalDtdBaseUri);
			if (flag)
			{
				if (!this.schemaInfo.ParameterEntities.ContainsKey(nameQualified))
				{
					this.schemaInfo.ParameterEntities.Add(nameQualified, schemaEntity);
				}
			}
			else if (!this.schemaInfo.GeneralEntities.ContainsKey(nameQualified))
			{
				this.schemaInfo.GeneralEntities.Add(nameQualified, schemaEntity);
			}
			schemaEntity.DeclaredInExternal = !this.ParsingInternalSubset;
			schemaEntity.ParsingInProgress = true;
			DtdParser.Token token2 = this.GetToken(true);
			if (token2 - DtdParser.Token.PUBLIC > 1)
			{
				if (token2 != DtdParser.Token.Literal)
				{
					goto IL_01D6;
				}
				schemaEntity.Text = this.GetValue();
				schemaEntity.Line = this.literalLineInfo.lineNo;
				schemaEntity.Pos = this.literalLineInfo.linePos;
			}
			else
			{
				string text;
				string text2;
				this.ParseExternalId(token2, DtdParser.Token.EntityDecl, out text, out text2);
				schemaEntity.IsExternal = true;
				schemaEntity.Url = text2;
				schemaEntity.Pubid = text;
				if (this.GetToken(false) == DtdParser.Token.NData)
				{
					if (flag)
					{
						this.ThrowUnexpectedToken(this.curPos - 5, ">");
					}
					if (!this.whitespaceSeen)
					{
						this.Throw(this.curPos - 5, "'{0}' is an unexpected token. Expecting white space.", "NDATA");
					}
					if (this.GetToken(true) != DtdParser.Token.Name)
					{
						goto IL_01D6;
					}
					schemaEntity.NData = this.GetNameQualified(false);
					string name = schemaEntity.NData.Name;
					if (!this.schemaInfo.Notations.ContainsKey(name))
					{
						this.AddUndeclaredNotation(name);
					}
				}
			}
			if (this.GetToken(false) == DtdParser.Token.GreaterThan)
			{
				schemaEntity.ParsingInProgress = false;
				return;
			}
			IL_01D6:
			this.OnUnexpectedError();
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00072538 File Offset: 0x00070738
		private void ParseNotationDecl()
		{
			if (this.GetToken(true) != DtdParser.Token.Name)
			{
				this.OnUnexpectedError();
			}
			XmlQualifiedName nameQualified = this.GetNameQualified(false);
			SchemaNotation schemaNotation = null;
			if (!this.schemaInfo.Notations.ContainsKey(nameQualified.Name))
			{
				if (this.undeclaredNotations != null)
				{
					this.undeclaredNotations.Remove(nameQualified.Name);
				}
				schemaNotation = new SchemaNotation(nameQualified);
				this.schemaInfo.Notations.Add(schemaNotation.Name.Name, schemaNotation);
			}
			else if (this.validate)
			{
				this.SendValidationEvent(this.curPos - nameQualified.Name.Length, XmlSeverityType.Error, "The notation '{0}' has already been declared.", nameQualified.Name);
			}
			DtdParser.Token token = this.GetToken(true);
			if (token == DtdParser.Token.SYSTEM || token == DtdParser.Token.PUBLIC)
			{
				string text;
				string text2;
				this.ParseExternalId(token, DtdParser.Token.NOTATION, out text, out text2);
				if (schemaNotation != null)
				{
					schemaNotation.SystemLiteral = text2;
					schemaNotation.Pubid = text;
				}
			}
			else
			{
				this.OnUnexpectedError();
			}
			if (this.GetToken(false) != DtdParser.Token.GreaterThan)
			{
				this.OnUnexpectedError();
			}
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0007262C File Offset: 0x0007082C
		private void AddUndeclaredNotation(string notationName)
		{
			if (this.undeclaredNotations == null)
			{
				this.undeclaredNotations = new Dictionary<string, DtdParser.UndeclaredNotation>();
			}
			DtdParser.UndeclaredNotation undeclaredNotation = new DtdParser.UndeclaredNotation(notationName, this.LineNo, this.LinePos - notationName.Length);
			DtdParser.UndeclaredNotation undeclaredNotation2;
			if (this.undeclaredNotations.TryGetValue(notationName, out undeclaredNotation2))
			{
				undeclaredNotation.next = undeclaredNotation2.next;
				undeclaredNotation2.next = undeclaredNotation;
				return;
			}
			this.undeclaredNotations.Add(notationName, undeclaredNotation);
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00072698 File Offset: 0x00070898
		private void ParseComment()
		{
			this.SaveParsingBuffer();
			try
			{
				if (this.SaveInternalSubsetValue)
				{
					this.readerAdapter.ParseComment(this.internalSubsetValueSb);
					this.internalSubsetValueSb.Append("-->");
				}
				else
				{
					this.readerAdapter.ParseComment(null);
				}
			}
			catch (XmlException ex)
			{
				if (!(ex.ResString == "Unexpected end of file while parsing {0} has occurred.") || this.currentEntityId == 0)
				{
					throw;
				}
				this.SendValidationEvent(XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", null);
			}
			this.LoadParsingBuffer();
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00072728 File Offset: 0x00070928
		private void ParsePI()
		{
			this.SaveParsingBuffer();
			if (this.SaveInternalSubsetValue)
			{
				this.readerAdapter.ParsePI(this.internalSubsetValueSb);
				this.internalSubsetValueSb.Append("?>");
			}
			else
			{
				this.readerAdapter.ParsePI(null);
			}
			this.LoadParsingBuffer();
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0007277C File Offset: 0x0007097C
		private void ParseCondSection()
		{
			int num = this.currentEntityId;
			DtdParser.Token token = this.GetToken(false);
			if (token != DtdParser.Token.IGNORE)
			{
				if (token == DtdParser.Token.INCLUDE && this.GetToken(false) == DtdParser.Token.LeftBracket)
				{
					if (this.validate && num != this.currentEntityId)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
					if (this.validate)
					{
						if (this.condSectionEntityIds == null)
						{
							this.condSectionEntityIds = new int[2];
						}
						else if (this.condSectionEntityIds.Length == this.condSectionDepth)
						{
							int[] array = new int[this.condSectionEntityIds.Length * 2];
							Array.Copy(this.condSectionEntityIds, 0, array, 0, this.condSectionEntityIds.Length);
							this.condSectionEntityIds = array;
						}
						this.condSectionEntityIds[this.condSectionDepth] = num;
					}
					this.condSectionDepth++;
					return;
				}
			}
			else if (this.GetToken(false) == DtdParser.Token.LeftBracket)
			{
				if (this.validate && num != this.currentEntityId)
				{
					this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
				}
				if (this.GetToken(false) == DtdParser.Token.CondSectionEnd)
				{
					if (this.validate && num != this.currentEntityId)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
						return;
					}
					return;
				}
			}
			this.OnUnexpectedError();
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x000728C4 File Offset: 0x00070AC4
		private void ParseExternalId(DtdParser.Token idTokenType, DtdParser.Token declType, out string publicId, out string systemId)
		{
			LineInfo lineInfo = new LineInfo(this.LineNo, this.LinePos - 6);
			publicId = null;
			systemId = null;
			if (this.GetToken(true) != DtdParser.Token.Literal)
			{
				this.ThrowUnexpectedToken(this.curPos, "\"", "'");
			}
			if (idTokenType == DtdParser.Token.SYSTEM)
			{
				systemId = this.GetValue();
				if (systemId.IndexOf('#') >= 0)
				{
					this.Throw(this.curPos - systemId.Length - 1, "Fragment identifier '{0}' cannot be part of the system identifier '{1}'.", new string[]
					{
						systemId.Substring(systemId.IndexOf('#')),
						systemId
					});
				}
				if (declType == DtdParser.Token.DOCTYPE && !this.freeFloatingDtd)
				{
					this.literalLineInfo.linePos = this.literalLineInfo.linePos + 1;
					this.readerAdapter.OnSystemId(systemId, lineInfo, this.literalLineInfo);
					return;
				}
			}
			else
			{
				publicId = this.GetValue();
				int num;
				if ((num = this.xmlCharType.IsPublicId(publicId)) >= 0)
				{
					this.ThrowInvalidChar(this.curPos - 1 - publicId.Length + num, publicId, num);
				}
				if (declType == DtdParser.Token.DOCTYPE && !this.freeFloatingDtd)
				{
					this.literalLineInfo.linePos = this.literalLineInfo.linePos + 1;
					this.readerAdapter.OnPublicId(publicId, lineInfo, this.literalLineInfo);
					if (this.GetToken(false) == DtdParser.Token.Literal)
					{
						if (!this.whitespaceSeen)
						{
							this.Throw("'{0}' is an unexpected token. Expecting white space.", new string(this.literalQuoteChar, 1), this.literalLineInfo.lineNo, this.literalLineInfo.linePos);
						}
						systemId = this.GetValue();
						this.literalLineInfo.linePos = this.literalLineInfo.linePos + 1;
						this.readerAdapter.OnSystemId(systemId, lineInfo, this.literalLineInfo);
						return;
					}
					this.ThrowUnexpectedToken(this.curPos, "\"", "'");
					return;
				}
				else
				{
					if (this.GetToken(false) == DtdParser.Token.Literal)
					{
						if (!this.whitespaceSeen)
						{
							this.Throw("'{0}' is an unexpected token. Expecting white space.", new string(this.literalQuoteChar, 1), this.literalLineInfo.lineNo, this.literalLineInfo.linePos);
						}
						systemId = this.GetValue();
						return;
					}
					if (declType != DtdParser.Token.NOTATION)
					{
						this.ThrowUnexpectedToken(this.curPos, "\"", "'");
					}
				}
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x00072AF8 File Offset: 0x00070CF8
		private DtdParser.Token GetToken(bool needWhiteSpace)
		{
			this.whitespaceSeen = false;
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c <= '\r')
				{
					if (c != '\0')
					{
						switch (c)
						{
						case '\t':
							goto IL_014D;
						case '\n':
							this.whitespaceSeen = true;
							this.curPos++;
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						case '\r':
							this.whitespaceSeen = true;
							if (this.chars[this.curPos + 1] == '\n')
							{
								if (this.Normalize)
								{
									this.SaveParsingBuffer();
									IDtdParserAdapter dtdParserAdapter = this.readerAdapter;
									int currentPosition = dtdParserAdapter.CurrentPosition;
									dtdParserAdapter.CurrentPosition = currentPosition + 1;
								}
								this.curPos += 2;
							}
							else
							{
								if (this.curPos + 1 >= this.charsUsed && !this.readerAdapter.IsEof)
								{
									goto IL_0388;
								}
								this.chars[this.curPos] = '\n';
								this.curPos++;
							}
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						}
						break;
					}
					if (this.curPos != this.charsUsed)
					{
						this.ThrowInvalidChar(this.chars, this.charsUsed, this.curPos);
						goto IL_0388;
					}
					goto IL_0388;
				}
				else if (c != ' ')
				{
					if (c != '%')
					{
						break;
					}
					if (this.charsUsed - this.curPos < 2)
					{
						goto IL_0388;
					}
					if (this.xmlCharType.IsWhiteSpace(this.chars[this.curPos + 1]))
					{
						break;
					}
					if (this.IgnoreEntityReferences)
					{
						this.curPos++;
						continue;
					}
					this.HandleEntityReference(true, false, false);
					continue;
				}
				IL_014D:
				this.whitespaceSeen = true;
				this.curPos++;
				continue;
				IL_0388:
				if ((this.readerAdapter.IsEof || this.ReadData() == 0) && !this.HandleEntityEnd(false))
				{
					if (this.scanningFunction == DtdParser.ScanningFunction.SubsetContent)
					{
						return DtdParser.Token.Eof;
					}
					this.Throw(this.curPos, "Incomplete DTD content.");
				}
			}
			if (needWhiteSpace && !this.whitespaceSeen && this.scanningFunction != DtdParser.ScanningFunction.ParamEntitySpace)
			{
				this.Throw(this.curPos, "'{0}' is an unexpected token. Expecting white space.", this.ParseUnexpectedToken(this.curPos));
			}
			this.tokenStartPos = this.curPos;
			for (;;)
			{
				switch (this.scanningFunction)
				{
				case DtdParser.ScanningFunction.SubsetContent:
					goto IL_02A9;
				case DtdParser.ScanningFunction.Name:
					goto IL_0294;
				case DtdParser.ScanningFunction.QName:
					goto IL_029B;
				case DtdParser.ScanningFunction.Nmtoken:
					goto IL_02A2;
				case DtdParser.ScanningFunction.Doctype1:
					goto IL_02B0;
				case DtdParser.ScanningFunction.Doctype2:
					goto IL_02B7;
				case DtdParser.ScanningFunction.Element1:
					goto IL_02BE;
				case DtdParser.ScanningFunction.Element2:
					goto IL_02C5;
				case DtdParser.ScanningFunction.Element3:
					goto IL_02CC;
				case DtdParser.ScanningFunction.Element4:
					goto IL_02D3;
				case DtdParser.ScanningFunction.Element5:
					goto IL_02DA;
				case DtdParser.ScanningFunction.Element6:
					goto IL_02E1;
				case DtdParser.ScanningFunction.Element7:
					goto IL_02E8;
				case DtdParser.ScanningFunction.Attlist1:
					goto IL_02EF;
				case DtdParser.ScanningFunction.Attlist2:
					goto IL_02F6;
				case DtdParser.ScanningFunction.Attlist3:
					goto IL_02FD;
				case DtdParser.ScanningFunction.Attlist4:
					goto IL_0304;
				case DtdParser.ScanningFunction.Attlist5:
					goto IL_030B;
				case DtdParser.ScanningFunction.Attlist6:
					goto IL_0312;
				case DtdParser.ScanningFunction.Attlist7:
					goto IL_0319;
				case DtdParser.ScanningFunction.Entity1:
					goto IL_033C;
				case DtdParser.ScanningFunction.Entity2:
					goto IL_0343;
				case DtdParser.ScanningFunction.Entity3:
					goto IL_034A;
				case DtdParser.ScanningFunction.Notation1:
					goto IL_0320;
				case DtdParser.ScanningFunction.CondSection1:
					goto IL_0351;
				case DtdParser.ScanningFunction.CondSection2:
					goto IL_0358;
				case DtdParser.ScanningFunction.CondSection3:
					goto IL_035F;
				case DtdParser.ScanningFunction.SystemId:
					goto IL_0327;
				case DtdParser.ScanningFunction.PublicId1:
					goto IL_032E;
				case DtdParser.ScanningFunction.PublicId2:
					goto IL_0335;
				case DtdParser.ScanningFunction.ClosingTag:
					goto IL_0366;
				case DtdParser.ScanningFunction.ParamEntitySpace:
					this.whitespaceSeen = true;
					this.scanningFunction = this.savedScanningFunction;
					continue;
				}
				break;
			}
			return DtdParser.Token.None;
			IL_0294:
			return this.ScanNameExpected();
			IL_029B:
			return this.ScanQNameExpected();
			IL_02A2:
			return this.ScanNmtokenExpected();
			IL_02A9:
			return this.ScanSubsetContent();
			IL_02B0:
			return this.ScanDoctype1();
			IL_02B7:
			return this.ScanDoctype2();
			IL_02BE:
			return this.ScanElement1();
			IL_02C5:
			return this.ScanElement2();
			IL_02CC:
			return this.ScanElement3();
			IL_02D3:
			return this.ScanElement4();
			IL_02DA:
			return this.ScanElement5();
			IL_02E1:
			return this.ScanElement6();
			IL_02E8:
			return this.ScanElement7();
			IL_02EF:
			return this.ScanAttlist1();
			IL_02F6:
			return this.ScanAttlist2();
			IL_02FD:
			return this.ScanAttlist3();
			IL_0304:
			return this.ScanAttlist4();
			IL_030B:
			return this.ScanAttlist5();
			IL_0312:
			return this.ScanAttlist6();
			IL_0319:
			return this.ScanAttlist7();
			IL_0320:
			return this.ScanNotation1();
			IL_0327:
			return this.ScanSystemId();
			IL_032E:
			return this.ScanPublicId1();
			IL_0335:
			return this.ScanPublicId2();
			IL_033C:
			return this.ScanEntity1();
			IL_0343:
			return this.ScanEntity2();
			IL_034A:
			return this.ScanEntity3();
			IL_0351:
			return this.ScanCondSection1();
			IL_0358:
			return this.ScanCondSection2();
			IL_035F:
			return this.ScanCondSection3();
			IL_0366:
			return this.ScanClosingTag();
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00072ED4 File Offset: 0x000710D4
		private DtdParser.Token ScanSubsetContent()
		{
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c != '<')
				{
					if (c == ']')
					{
						if (this.charsUsed - this.curPos < 2 && !this.readerAdapter.IsEof)
						{
							goto IL_0513;
						}
						if (this.chars[this.curPos + 1] != ']')
						{
							goto Block_40;
						}
						if (this.charsUsed - this.curPos < 3 && !this.readerAdapter.IsEof)
						{
							goto IL_0513;
						}
						if (this.chars[this.curPos + 1] == ']' && this.chars[this.curPos + 2] == '>')
						{
							goto Block_43;
						}
					}
					if (this.charsUsed - this.curPos != 0)
					{
						this.Throw(this.curPos, "Expected DTD markup was not found.");
					}
				}
				else
				{
					char c2 = this.chars[this.curPos + 1];
					if (c2 != '!')
					{
						if (c2 == '?')
						{
							goto IL_041B;
						}
						if (this.charsUsed - this.curPos >= 2)
						{
							goto Block_38;
						}
					}
					else
					{
						char c3 = this.chars[this.curPos + 2];
						if (c3 <= 'A')
						{
							if (c3 != '-')
							{
								if (c3 == 'A')
								{
									if (this.charsUsed - this.curPos >= 9)
									{
										goto Block_22;
									}
									goto IL_0513;
								}
							}
							else
							{
								if (this.chars[this.curPos + 3] == '-')
								{
									goto Block_35;
								}
								if (this.charsUsed - this.curPos >= 4)
								{
									this.Throw(this.curPos, "Expected DTD markup was not found.");
									goto IL_0513;
								}
								goto IL_0513;
							}
						}
						else if (c3 != 'E')
						{
							if (c3 != 'N')
							{
								if (c3 == '[')
								{
									goto IL_038A;
								}
							}
							else
							{
								if (this.charsUsed - this.curPos >= 10)
								{
									goto Block_28;
								}
								goto IL_0513;
							}
						}
						else if (this.chars[this.curPos + 3] == 'L')
						{
							if (this.charsUsed - this.curPos >= 9)
							{
								break;
							}
							goto IL_0513;
						}
						else if (this.chars[this.curPos + 3] == 'N')
						{
							if (this.charsUsed - this.curPos >= 8)
							{
								goto Block_17;
							}
							goto IL_0513;
						}
						else
						{
							if (this.charsUsed - this.curPos >= 4)
							{
								goto Block_21;
							}
							goto IL_0513;
						}
						if (this.charsUsed - this.curPos >= 3)
						{
							this.Throw(this.curPos + 2, "Expected DTD markup was not found.");
						}
					}
				}
				IL_0513:
				if (this.ReadData() == 0)
				{
					this.Throw(this.charsUsed, "Incomplete DTD content.");
				}
			}
			if (this.chars[this.curPos + 4] != 'E' || this.chars[this.curPos + 5] != 'M' || this.chars[this.curPos + 6] != 'E' || this.chars[this.curPos + 7] != 'N' || this.chars[this.curPos + 8] != 'T')
			{
				this.Throw(this.curPos, "Expected DTD markup was not found.");
			}
			this.curPos += 9;
			this.scanningFunction = DtdParser.ScanningFunction.QName;
			this.nextScaningFunction = DtdParser.ScanningFunction.Element1;
			return DtdParser.Token.ElementDecl;
			Block_17:
			if (this.chars[this.curPos + 4] != 'T' || this.chars[this.curPos + 5] != 'I' || this.chars[this.curPos + 6] != 'T' || this.chars[this.curPos + 7] != 'Y')
			{
				this.Throw(this.curPos, "Expected DTD markup was not found.");
			}
			this.curPos += 8;
			this.scanningFunction = DtdParser.ScanningFunction.Entity1;
			return DtdParser.Token.EntityDecl;
			Block_21:
			this.Throw(this.curPos, "Expected DTD markup was not found.");
			return DtdParser.Token.None;
			Block_22:
			if (this.chars[this.curPos + 3] != 'T' || this.chars[this.curPos + 4] != 'T' || this.chars[this.curPos + 5] != 'L' || this.chars[this.curPos + 6] != 'I' || this.chars[this.curPos + 7] != 'S' || this.chars[this.curPos + 8] != 'T')
			{
				this.Throw(this.curPos, "Expected DTD markup was not found.");
			}
			this.curPos += 9;
			this.scanningFunction = DtdParser.ScanningFunction.QName;
			this.nextScaningFunction = DtdParser.ScanningFunction.Attlist1;
			return DtdParser.Token.AttlistDecl;
			Block_28:
			if (this.chars[this.curPos + 3] != 'O' || this.chars[this.curPos + 4] != 'T' || this.chars[this.curPos + 5] != 'A' || this.chars[this.curPos + 6] != 'T' || this.chars[this.curPos + 7] != 'I' || this.chars[this.curPos + 8] != 'O' || this.chars[this.curPos + 9] != 'N')
			{
				this.Throw(this.curPos, "Expected DTD markup was not found.");
			}
			this.curPos += 10;
			this.scanningFunction = DtdParser.ScanningFunction.Name;
			this.nextScaningFunction = DtdParser.ScanningFunction.Notation1;
			return DtdParser.Token.NotationDecl;
			IL_038A:
			this.curPos += 3;
			this.scanningFunction = DtdParser.ScanningFunction.CondSection1;
			return DtdParser.Token.CondSectionStart;
			Block_35:
			this.curPos += 4;
			return DtdParser.Token.Comment;
			IL_041B:
			this.curPos += 2;
			return DtdParser.Token.PI;
			Block_38:
			this.Throw(this.curPos, "Expected DTD markup was not found.");
			return DtdParser.Token.None;
			Block_40:
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			return DtdParser.Token.RightBracket;
			Block_43:
			this.curPos += 3;
			return DtdParser.Token.CondSectionEnd;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00073414 File Offset: 0x00071614
		private DtdParser.Token ScanNameExpected()
		{
			this.ScanName();
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Name;
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0007342A File Offset: 0x0007162A
		private DtdParser.Token ScanQNameExpected()
		{
			this.ScanQName();
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.QName;
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00073440 File Offset: 0x00071640
		private DtdParser.Token ScanNmtokenExpected()
		{
			this.ScanNmtoken();
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Nmtoken;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00073458 File Offset: 0x00071658
		private DtdParser.Token ScanDoctype1()
		{
			char c = this.chars[this.curPos];
			if (c <= 'P')
			{
				if (c == '>')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
					return DtdParser.Token.GreaterThan;
				}
				if (c == 'P')
				{
					if (!this.EatPublicKeyword())
					{
						this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.Doctype2;
					this.scanningFunction = DtdParser.ScanningFunction.PublicId1;
					return DtdParser.Token.PUBLIC;
				}
			}
			else
			{
				if (c == 'S')
				{
					if (!this.EatSystemKeyword())
					{
						this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.Doctype2;
					this.scanningFunction = DtdParser.ScanningFunction.SystemId;
					return DtdParser.Token.SYSTEM;
				}
				if (c == '[')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
					return DtdParser.Token.LeftBracket;
				}
			}
			this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
			return DtdParser.Token.None;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00073534 File Offset: 0x00071734
		private DtdParser.Token ScanDoctype2()
		{
			char c = this.chars[this.curPos];
			if (c == '>')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
				return DtdParser.Token.GreaterThan;
			}
			if (c == '[')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
				return DtdParser.Token.LeftBracket;
			}
			this.Throw(this.curPos, "Expecting an internal subset or the end of the DOCTYPE declaration.");
			return DtdParser.Token.None;
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0007359C File Offset: 0x0007179C
		private DtdParser.Token ScanClosingTag()
		{
			if (this.chars[this.curPos] != '>')
			{
				this.ThrowUnexpectedToken(this.curPos, ">");
			}
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
			return DtdParser.Token.GreaterThan;
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x000735D8 File Offset: 0x000717D8
		private DtdParser.Token ScanElement1()
		{
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c != '(')
				{
					if (c != 'A')
					{
						if (c != 'E')
						{
							goto IL_010A;
						}
						if (this.charsUsed - this.curPos >= 5)
						{
							if (this.chars[this.curPos + 1] == 'M' && this.chars[this.curPos + 2] == 'P' && this.chars[this.curPos + 3] == 'T' && this.chars[this.curPos + 4] == 'Y')
							{
								goto Block_7;
							}
							goto IL_010A;
						}
					}
					else if (this.charsUsed - this.curPos >= 3)
					{
						if (this.chars[this.curPos + 1] == 'N' && this.chars[this.curPos + 2] == 'Y')
						{
							goto Block_10;
						}
						goto IL_010A;
					}
					IL_011B:
					if (this.ReadData() == 0)
					{
						this.Throw(this.curPos, "Incomplete DTD content.");
						continue;
					}
					continue;
					IL_010A:
					this.Throw(this.curPos, "Invalid content model.");
					goto IL_011B;
				}
				break;
			}
			this.scanningFunction = DtdParser.ScanningFunction.Element2;
			this.curPos++;
			return DtdParser.Token.LeftParen;
			Block_7:
			this.curPos += 5;
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			return DtdParser.Token.EMPTY;
			Block_10:
			this.curPos += 3;
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			return DtdParser.Token.ANY;
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x00073720 File Offset: 0x00071920
		private DtdParser.Token ScanElement2()
		{
			if (this.chars[this.curPos] == '#')
			{
				while (this.charsUsed - this.curPos < 7)
				{
					if (this.ReadData() == 0)
					{
						this.Throw(this.curPos, "Incomplete DTD content.");
					}
				}
				if (this.chars[this.curPos + 1] == 'P' && this.chars[this.curPos + 2] == 'C' && this.chars[this.curPos + 3] == 'D' && this.chars[this.curPos + 4] == 'A' && this.chars[this.curPos + 5] == 'T' && this.chars[this.curPos + 6] == 'A')
				{
					this.curPos += 7;
					this.scanningFunction = DtdParser.ScanningFunction.Element6;
					return DtdParser.Token.PCDATA;
				}
				this.Throw(this.curPos + 1, "Expecting 'PCDATA'.");
			}
			this.scanningFunction = DtdParser.ScanningFunction.Element3;
			return DtdParser.Token.None;
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00073814 File Offset: 0x00071A14
		private DtdParser.Token ScanElement3()
		{
			char c = this.chars[this.curPos];
			if (c == '(')
			{
				this.curPos++;
				return DtdParser.Token.LeftParen;
			}
			if (c != '>')
			{
				this.ScanQName();
				this.scanningFunction = DtdParser.ScanningFunction.Element4;
				return DtdParser.Token.QName;
			}
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
			return DtdParser.Token.GreaterThan;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00073874 File Offset: 0x00071A74
		private DtdParser.Token ScanElement4()
		{
			this.scanningFunction = DtdParser.ScanningFunction.Element5;
			char c = this.chars[this.curPos];
			DtdParser.Token token;
			if (c != '*')
			{
				if (c != '+')
				{
					if (c != '?')
					{
						return DtdParser.Token.None;
					}
					token = DtdParser.Token.QMark;
				}
				else
				{
					token = DtdParser.Token.Plus;
				}
			}
			else
			{
				token = DtdParser.Token.Star;
			}
			if (this.whitespaceSeen)
			{
				this.Throw(this.curPos, "White space not allowed before '?', '*', or '+'.");
			}
			this.curPos++;
			return token;
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x000738E4 File Offset: 0x00071AE4
		private DtdParser.Token ScanElement5()
		{
			char c = this.chars[this.curPos];
			if (c <= ',')
			{
				if (c == ')')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.Element4;
					return DtdParser.Token.RightParen;
				}
				if (c == ',')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.Element3;
					return DtdParser.Token.Comma;
				}
			}
			else
			{
				if (c == '>')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
					return DtdParser.Token.GreaterThan;
				}
				if (c == '|')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.Element3;
					return DtdParser.Token.Or;
				}
			}
			this.Throw(this.curPos, "Expecting '?', '*', or '+'.");
			return DtdParser.Token.None;
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00073990 File Offset: 0x00071B90
		private DtdParser.Token ScanElement6()
		{
			char c = this.chars[this.curPos];
			if (c == ')')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.Element7;
				return DtdParser.Token.RightParen;
			}
			if (c != '|')
			{
				this.ThrowUnexpectedToken(this.curPos, ")", "|");
				return DtdParser.Token.None;
			}
			this.curPos++;
			this.nextScaningFunction = DtdParser.ScanningFunction.Element6;
			this.scanningFunction = DtdParser.ScanningFunction.QName;
			return DtdParser.Token.Or;
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00073A08 File Offset: 0x00071C08
		private DtdParser.Token ScanElement7()
		{
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			if (this.chars[this.curPos] == '*' && !this.whitespaceSeen)
			{
				this.curPos++;
				return DtdParser.Token.Star;
			}
			return DtdParser.Token.None;
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00073A40 File Offset: 0x00071C40
		private DtdParser.Token ScanAttlist1()
		{
			if (this.chars[this.curPos] == '>')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
				return DtdParser.Token.GreaterThan;
			}
			if (!this.whitespaceSeen)
			{
				this.Throw(this.curPos, "'{0}' is an unexpected token. Expecting white space.", this.ParseUnexpectedToken(this.curPos));
			}
			this.ScanQName();
			this.scanningFunction = DtdParser.ScanningFunction.Attlist2;
			return DtdParser.Token.QName;
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00073AAC File Offset: 0x00071CAC
		private DtdParser.Token ScanAttlist2()
		{
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c <= 'C')
				{
					if (c == '(')
					{
						break;
					}
					if (c != 'C')
					{
						goto IL_044E;
					}
					if (this.charsUsed - this.curPos >= 5)
					{
						goto Block_6;
					}
				}
				else if (c != 'E')
				{
					if (c != 'I')
					{
						if (c != 'N')
						{
							goto IL_044E;
						}
						if (this.charsUsed - this.curPos >= 8 || this.readerAdapter.IsEof)
						{
							char c2 = this.chars[this.curPos + 1];
							if (c2 == 'M')
							{
								goto IL_0390;
							}
							if (c2 == 'O')
							{
								goto Block_24;
							}
							this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
						}
					}
					else if (this.charsUsed - this.curPos >= 6)
					{
						goto Block_17;
					}
				}
				else if (this.charsUsed - this.curPos >= 9)
				{
					this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
					if (this.chars[this.curPos + 1] != 'N' || this.chars[this.curPos + 2] != 'T' || this.chars[this.curPos + 3] != 'I' || this.chars[this.curPos + 4] != 'T')
					{
						this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
					}
					char c2 = this.chars[this.curPos + 5];
					if (c2 == 'I')
					{
						goto IL_017C;
					}
					if (c2 == 'Y')
					{
						goto IL_01C3;
					}
					this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
				}
				IL_045F:
				if (this.ReadData() == 0)
				{
					this.Throw(this.curPos, "Incomplete DTD content.");
					continue;
				}
				continue;
				IL_044E:
				this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
				goto IL_045F;
			}
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.Nmtoken;
			this.nextScaningFunction = DtdParser.ScanningFunction.Attlist5;
			return DtdParser.Token.LeftParen;
			Block_6:
			if (this.chars[this.curPos + 1] != 'D' || this.chars[this.curPos + 2] != 'A' || this.chars[this.curPos + 3] != 'T' || this.chars[this.curPos + 4] != 'A')
			{
				this.Throw(this.curPos, "Invalid attribute type.");
			}
			this.curPos += 5;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
			return DtdParser.Token.CDATA;
			IL_017C:
			if (this.chars[this.curPos + 6] != 'E' || this.chars[this.curPos + 7] != 'S')
			{
				this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
			}
			this.curPos += 8;
			return DtdParser.Token.ENTITIES;
			IL_01C3:
			this.curPos += 6;
			return DtdParser.Token.ENTITY;
			Block_17:
			this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
			if (this.chars[this.curPos + 1] != 'D')
			{
				this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
			}
			if (this.chars[this.curPos + 2] != 'R')
			{
				this.curPos += 2;
				return DtdParser.Token.ID;
			}
			if (this.chars[this.curPos + 3] != 'E' || this.chars[this.curPos + 4] != 'F')
			{
				this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
			}
			if (this.chars[this.curPos + 5] != 'S')
			{
				this.curPos += 5;
				return DtdParser.Token.IDREF;
			}
			this.curPos += 6;
			return DtdParser.Token.IDREFS;
			Block_24:
			if (this.chars[this.curPos + 2] != 'T' || this.chars[this.curPos + 3] != 'A' || this.chars[this.curPos + 4] != 'T' || this.chars[this.curPos + 5] != 'I' || this.chars[this.curPos + 6] != 'O' || this.chars[this.curPos + 7] != 'N')
			{
				this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
			}
			this.curPos += 8;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist3;
			return DtdParser.Token.NOTATION;
			IL_0390:
			if (this.chars[this.curPos + 2] != 'T' || this.chars[this.curPos + 3] != 'O' || this.chars[this.curPos + 4] != 'K' || this.chars[this.curPos + 5] != 'E' || this.chars[this.curPos + 6] != 'N')
			{
				this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
			}
			this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
			if (this.chars[this.curPos + 7] == 'S')
			{
				this.curPos += 8;
				return DtdParser.Token.NMTOKENS;
			}
			this.curPos += 7;
			return DtdParser.Token.NMTOKEN;
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00073F38 File Offset: 0x00072138
		private DtdParser.Token ScanAttlist3()
		{
			if (this.chars[this.curPos] == '(')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.Name;
				this.nextScaningFunction = DtdParser.ScanningFunction.Attlist4;
				return DtdParser.Token.LeftParen;
			}
			this.ThrowUnexpectedToken(this.curPos, "(");
			return DtdParser.Token.None;
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x00073F8C File Offset: 0x0007218C
		private DtdParser.Token ScanAttlist4()
		{
			char c = this.chars[this.curPos];
			if (c == ')')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
				return DtdParser.Token.RightParen;
			}
			if (c != '|')
			{
				this.ThrowUnexpectedToken(this.curPos, ")", "|");
				return DtdParser.Token.None;
			}
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.Name;
			this.nextScaningFunction = DtdParser.ScanningFunction.Attlist4;
			return DtdParser.Token.Or;
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00074004 File Offset: 0x00072204
		private DtdParser.Token ScanAttlist5()
		{
			char c = this.chars[this.curPos];
			if (c == ')')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
				return DtdParser.Token.RightParen;
			}
			if (c != '|')
			{
				this.ThrowUnexpectedToken(this.curPos, ")", "|");
				return DtdParser.Token.None;
			}
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.Nmtoken;
			this.nextScaningFunction = DtdParser.ScanningFunction.Attlist5;
			return DtdParser.Token.Or;
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x0007407C File Offset: 0x0007227C
		private DtdParser.Token ScanAttlist6()
		{
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c == '"')
				{
					break;
				}
				if (c != '#')
				{
					if (c == '\'')
					{
						break;
					}
					this.Throw(this.curPos, "Expecting an attribute type.");
				}
				else if (this.charsUsed - this.curPos >= 6)
				{
					char c2 = this.chars[this.curPos + 1];
					if (c2 == 'F')
					{
						goto IL_01E1;
					}
					if (c2 != 'I')
					{
						if (c2 == 'R')
						{
							if (this.charsUsed - this.curPos >= 9)
							{
								goto Block_6;
							}
						}
						else
						{
							this.Throw(this.curPos, "Expecting an attribute type.");
						}
					}
					else if (this.charsUsed - this.curPos >= 8)
					{
						goto Block_13;
					}
				}
				if (this.ReadData() == 0)
				{
					this.Throw(this.curPos, "Incomplete DTD content.");
				}
			}
			this.ScanLiteral(DtdParser.LiteralType.AttributeValue);
			this.scanningFunction = DtdParser.ScanningFunction.Attlist1;
			return DtdParser.Token.Literal;
			Block_6:
			if (this.chars[this.curPos + 2] != 'E' || this.chars[this.curPos + 3] != 'Q' || this.chars[this.curPos + 4] != 'U' || this.chars[this.curPos + 5] != 'I' || this.chars[this.curPos + 6] != 'R' || this.chars[this.curPos + 7] != 'E' || this.chars[this.curPos + 8] != 'D')
			{
				this.Throw(this.curPos, "Expecting an attribute type.");
			}
			this.curPos += 9;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist1;
			return DtdParser.Token.REQUIRED;
			Block_13:
			if (this.chars[this.curPos + 2] != 'M' || this.chars[this.curPos + 3] != 'P' || this.chars[this.curPos + 4] != 'L' || this.chars[this.curPos + 5] != 'I' || this.chars[this.curPos + 6] != 'E' || this.chars[this.curPos + 7] != 'D')
			{
				this.Throw(this.curPos, "Expecting an attribute type.");
			}
			this.curPos += 8;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist1;
			return DtdParser.Token.IMPLIED;
			IL_01E1:
			if (this.chars[this.curPos + 2] != 'I' || this.chars[this.curPos + 3] != 'X' || this.chars[this.curPos + 4] != 'E' || this.chars[this.curPos + 5] != 'D')
			{
				this.Throw(this.curPos, "Expecting an attribute type.");
			}
			this.curPos += 6;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist7;
			return DtdParser.Token.FIXED;
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00074324 File Offset: 0x00072524
		private DtdParser.Token ScanAttlist7()
		{
			char c = this.chars[this.curPos];
			if (c == '"' || c == '\'')
			{
				this.ScanLiteral(DtdParser.LiteralType.AttributeValue);
				this.scanningFunction = DtdParser.ScanningFunction.Attlist1;
				return DtdParser.Token.Literal;
			}
			this.ThrowUnexpectedToken(this.curPos, "\"", "'");
			return DtdParser.Token.None;
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00074374 File Offset: 0x00072574
		private DtdParser.Token ScanLiteral(DtdParser.LiteralType literalType)
		{
			char c = this.chars[this.curPos];
			char c2 = ((literalType == DtdParser.LiteralType.AttributeValue) ? ' ' : '\n');
			int num = this.currentEntityId;
			this.literalLineInfo.Set(this.LineNo, this.LinePos);
			this.curPos++;
			this.tokenStartPos = this.curPos;
			this.stringBuilder.Length = 0;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[(int)this.chars[this.curPos]] & 128) == 0 || this.chars[this.curPos] == '%')
				{
					if (this.chars[this.curPos] == c && this.currentEntityId == num)
					{
						break;
					}
					int num2 = this.curPos - this.tokenStartPos;
					if (num2 > 0)
					{
						this.stringBuilder.Append(this.chars, this.tokenStartPos, num2);
						this.tokenStartPos = this.curPos;
					}
					char c3 = this.chars[this.curPos];
					if (c3 <= '\'')
					{
						switch (c3)
						{
						case '\t':
							if (literalType == DtdParser.LiteralType.AttributeValue && this.Normalize)
							{
								this.stringBuilder.Append(' ');
								this.tokenStartPos++;
							}
							this.curPos++;
							continue;
						case '\n':
							this.curPos++;
							if (this.Normalize)
							{
								this.stringBuilder.Append(c2);
								this.tokenStartPos = this.curPos;
							}
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						case '\v':
						case '\f':
							goto IL_054D;
						case '\r':
							if (this.chars[this.curPos + 1] == '\n')
							{
								if (this.Normalize)
								{
									if (literalType == DtdParser.LiteralType.AttributeValue)
									{
										this.stringBuilder.Append(this.readerAdapter.IsEntityEolNormalized ? "  " : " ");
									}
									else
									{
										this.stringBuilder.Append(this.readerAdapter.IsEntityEolNormalized ? "\r\n" : "\n");
									}
									this.tokenStartPos = this.curPos + 2;
									this.SaveParsingBuffer();
									IDtdParserAdapter dtdParserAdapter = this.readerAdapter;
									int currentPosition = dtdParserAdapter.CurrentPosition;
									dtdParserAdapter.CurrentPosition = currentPosition + 1;
								}
								this.curPos += 2;
							}
							else
							{
								if (this.curPos + 1 == this.charsUsed)
								{
									goto IL_05CF;
								}
								this.curPos++;
								if (this.Normalize)
								{
									this.stringBuilder.Append(c2);
									this.tokenStartPos = this.curPos;
								}
							}
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						default:
							switch (c3)
							{
							case '"':
							case '\'':
								break;
							case '#':
							case '$':
								goto IL_054D;
							case '%':
								if (literalType != DtdParser.LiteralType.EntityReplText)
								{
									this.curPos++;
									continue;
								}
								this.HandleEntityReference(true, true, literalType == DtdParser.LiteralType.AttributeValue);
								this.tokenStartPos = this.curPos;
								continue;
							case '&':
								if (literalType == DtdParser.LiteralType.SystemOrPublicID)
								{
									this.curPos++;
									continue;
								}
								if (this.curPos + 1 == this.charsUsed)
								{
									goto IL_05CF;
								}
								if (this.chars[this.curPos + 1] == '#')
								{
									this.SaveParsingBuffer();
									int num3 = this.readerAdapter.ParseNumericCharRef(this.SaveInternalSubsetValue ? this.internalSubsetValueSb : null);
									this.LoadParsingBuffer();
									this.stringBuilder.Append(this.chars, this.curPos, num3 - this.curPos);
									this.readerAdapter.CurrentPosition = num3;
									this.tokenStartPos = num3;
									this.curPos = num3;
									continue;
								}
								this.SaveParsingBuffer();
								if (literalType == DtdParser.LiteralType.AttributeValue)
								{
									int num4 = this.readerAdapter.ParseNamedCharRef(true, this.SaveInternalSubsetValue ? this.internalSubsetValueSb : null);
									this.LoadParsingBuffer();
									if (num4 >= 0)
									{
										this.stringBuilder.Append(this.chars, this.curPos, num4 - this.curPos);
										this.readerAdapter.CurrentPosition = num4;
										this.tokenStartPos = num4;
										this.curPos = num4;
										continue;
									}
									this.HandleEntityReference(false, true, true);
									this.tokenStartPos = this.curPos;
									continue;
								}
								else
								{
									int num5 = this.readerAdapter.ParseNamedCharRef(false, null);
									this.LoadParsingBuffer();
									if (num5 >= 0)
									{
										this.tokenStartPos = this.curPos;
										this.curPos = num5;
										continue;
									}
									this.stringBuilder.Append('&');
									this.curPos++;
									this.tokenStartPos = this.curPos;
									XmlQualifiedName xmlQualifiedName = this.ScanEntityName();
									this.VerifyEntityReference(xmlQualifiedName, false, false, false);
									continue;
								}
								break;
							default:
								goto IL_054D;
							}
							break;
						}
					}
					else
					{
						if (c3 == '<')
						{
							if (literalType == DtdParser.LiteralType.AttributeValue)
							{
								this.Throw(this.curPos, "'{0}', hexadecimal value {1}, is an invalid attribute character.", XmlException.BuildCharExceptionArgs('<', '\0'));
							}
							this.curPos++;
							continue;
						}
						if (c3 != '>')
						{
							goto IL_054D;
						}
					}
					this.curPos++;
					continue;
					IL_054D:
					if (this.curPos != this.charsUsed)
					{
						if (!XmlCharType.IsHighSurrogate((int)this.chars[this.curPos]))
						{
							goto IL_05B4;
						}
						if (this.curPos + 1 != this.charsUsed)
						{
							this.curPos++;
							if (XmlCharType.IsLowSurrogate((int)this.chars[this.curPos]))
							{
								this.curPos++;
								continue;
							}
							goto IL_05B4;
						}
					}
					IL_05CF:
					if ((this.readerAdapter.IsEof || this.ReadData() == 0) && (literalType == DtdParser.LiteralType.SystemOrPublicID || !this.HandleEntityEnd(true)))
					{
						this.Throw(this.curPos, "There is an unclosed literal string.");
					}
					this.tokenStartPos = this.curPos;
				}
				else
				{
					this.curPos++;
				}
			}
			if (this.stringBuilder.Length > 0)
			{
				this.stringBuilder.Append(this.chars, this.tokenStartPos, this.curPos - this.tokenStartPos);
			}
			this.curPos++;
			this.literalQuoteChar = c;
			return DtdParser.Token.Literal;
			IL_05B4:
			this.ThrowInvalidChar(this.chars, this.charsUsed, this.curPos);
			return DtdParser.Token.None;
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00074994 File Offset: 0x00072B94
		private XmlQualifiedName ScanEntityName()
		{
			try
			{
				this.ScanName();
			}
			catch (XmlException ex)
			{
				this.Throw("An error occurred while parsing EntityName.", string.Empty, ex.LineNumber, ex.LinePosition);
			}
			if (this.chars[this.curPos] != ';')
			{
				this.ThrowUnexpectedToken(this.curPos, ";");
			}
			XmlQualifiedName nameQualified = this.GetNameQualified(false);
			this.curPos++;
			return nameQualified;
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00074A10 File Offset: 0x00072C10
		private DtdParser.Token ScanNotation1()
		{
			char c = this.chars[this.curPos];
			if (c == 'P')
			{
				if (!this.EatPublicKeyword())
				{
					this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
				}
				this.nextScaningFunction = DtdParser.ScanningFunction.ClosingTag;
				this.scanningFunction = DtdParser.ScanningFunction.PublicId1;
				return DtdParser.Token.PUBLIC;
			}
			if (c != 'S')
			{
				this.Throw(this.curPos, "Expecting a system identifier or a public identifier.");
				return DtdParser.Token.None;
			}
			if (!this.EatSystemKeyword())
			{
				this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
			}
			this.nextScaningFunction = DtdParser.ScanningFunction.ClosingTag;
			this.scanningFunction = DtdParser.ScanningFunction.SystemId;
			return DtdParser.Token.SYSTEM;
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00074AA4 File Offset: 0x00072CA4
		private DtdParser.Token ScanSystemId()
		{
			if (this.chars[this.curPos] != '"' && this.chars[this.curPos] != '\'')
			{
				this.ThrowUnexpectedToken(this.curPos, "\"", "'");
			}
			this.ScanLiteral(DtdParser.LiteralType.SystemOrPublicID);
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Literal;
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x00074B00 File Offset: 0x00072D00
		private DtdParser.Token ScanEntity1()
		{
			if (this.chars[this.curPos] == '%')
			{
				this.curPos++;
				this.nextScaningFunction = DtdParser.ScanningFunction.Entity2;
				this.scanningFunction = DtdParser.ScanningFunction.Name;
				return DtdParser.Token.Percent;
			}
			this.ScanName();
			this.scanningFunction = DtdParser.ScanningFunction.Entity2;
			return DtdParser.Token.Name;
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00074B50 File Offset: 0x00072D50
		private DtdParser.Token ScanEntity2()
		{
			char c = this.chars[this.curPos];
			if (c <= '\'')
			{
				if (c == '"' || c == '\'')
				{
					this.ScanLiteral(DtdParser.LiteralType.EntityReplText);
					this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
					return DtdParser.Token.Literal;
				}
			}
			else
			{
				if (c == 'P')
				{
					if (!this.EatPublicKeyword())
					{
						this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.Entity3;
					this.scanningFunction = DtdParser.ScanningFunction.PublicId1;
					return DtdParser.Token.PUBLIC;
				}
				if (c == 'S')
				{
					if (!this.EatSystemKeyword())
					{
						this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.Entity3;
					this.scanningFunction = DtdParser.ScanningFunction.SystemId;
					return DtdParser.Token.SYSTEM;
				}
			}
			this.Throw(this.curPos, "Expecting an external identifier or an entity value.");
			return DtdParser.Token.None;
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00074C08 File Offset: 0x00072E08
		private DtdParser.Token ScanEntity3()
		{
			if (this.chars[this.curPos] == 'N')
			{
				while (this.charsUsed - this.curPos < 5)
				{
					if (this.ReadData() == 0)
					{
						goto IL_009A;
					}
				}
				if (this.chars[this.curPos + 1] == 'D' && this.chars[this.curPos + 2] == 'A' && this.chars[this.curPos + 3] == 'T' && this.chars[this.curPos + 4] == 'A')
				{
					this.curPos += 5;
					this.scanningFunction = DtdParser.ScanningFunction.Name;
					this.nextScaningFunction = DtdParser.ScanningFunction.ClosingTag;
					return DtdParser.Token.NData;
				}
			}
			IL_009A:
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			return DtdParser.Token.None;
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x00074CBC File Offset: 0x00072EBC
		private DtdParser.Token ScanPublicId1()
		{
			if (this.chars[this.curPos] != '"' && this.chars[this.curPos] != '\'')
			{
				this.ThrowUnexpectedToken(this.curPos, "\"", "'");
			}
			this.ScanLiteral(DtdParser.LiteralType.SystemOrPublicID);
			this.scanningFunction = DtdParser.ScanningFunction.PublicId2;
			return DtdParser.Token.Literal;
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x00074D14 File Offset: 0x00072F14
		private DtdParser.Token ScanPublicId2()
		{
			if (this.chars[this.curPos] != '"' && this.chars[this.curPos] != '\'')
			{
				this.scanningFunction = this.nextScaningFunction;
				return DtdParser.Token.None;
			}
			this.ScanLiteral(DtdParser.LiteralType.SystemOrPublicID);
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Literal;
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00074D68 File Offset: 0x00072F68
		private DtdParser.Token ScanCondSection1()
		{
			if (this.chars[this.curPos] != 'I')
			{
				this.Throw(this.curPos, "Conditional sections must specify the keyword 'IGNORE' or 'INCLUDE'.");
			}
			this.curPos++;
			for (;;)
			{
				if (this.charsUsed - this.curPos >= 5)
				{
					char c = this.chars[this.curPos];
					if (c == 'G')
					{
						goto IL_0121;
					}
					if (c != 'N')
					{
						goto IL_01AA;
					}
					if (this.charsUsed - this.curPos >= 6)
					{
						break;
					}
				}
				if (this.ReadData() == 0)
				{
					this.Throw(this.curPos, "Incomplete DTD content.");
				}
			}
			if (this.chars[this.curPos + 1] == 'C' && this.chars[this.curPos + 2] == 'L' && this.chars[this.curPos + 3] == 'U' && this.chars[this.curPos + 4] == 'D' && this.chars[this.curPos + 5] == 'E' && !this.xmlCharType.IsNameSingleChar(this.chars[this.curPos + 6]))
			{
				this.nextScaningFunction = DtdParser.ScanningFunction.SubsetContent;
				this.scanningFunction = DtdParser.ScanningFunction.CondSection2;
				this.curPos += 6;
				return DtdParser.Token.INCLUDE;
			}
			goto IL_01AA;
			IL_0121:
			if (this.chars[this.curPos + 1] == 'N' && this.chars[this.curPos + 2] == 'O' && this.chars[this.curPos + 3] == 'R' && this.chars[this.curPos + 4] == 'E' && !this.xmlCharType.IsNameSingleChar(this.chars[this.curPos + 5]))
			{
				this.nextScaningFunction = DtdParser.ScanningFunction.CondSection3;
				this.scanningFunction = DtdParser.ScanningFunction.CondSection2;
				this.curPos += 5;
				return DtdParser.Token.IGNORE;
			}
			IL_01AA:
			this.Throw(this.curPos - 1, "Conditional sections must specify the keyword 'IGNORE' or 'INCLUDE'.");
			return DtdParser.Token.None;
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00074F55 File Offset: 0x00073155
		private DtdParser.Token ScanCondSection2()
		{
			if (this.chars[this.curPos] != '[')
			{
				this.ThrowUnexpectedToken(this.curPos, "[");
			}
			this.curPos++;
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.LeftBracket;
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x00074F98 File Offset: 0x00073198
		private DtdParser.Token ScanCondSection3()
		{
			int num = 0;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[(int)this.chars[this.curPos]] & 64) == 0 || this.chars[this.curPos] == ']')
				{
					char c = this.chars[this.curPos];
					if (c <= '&')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
							this.curPos++;
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						case '\v':
						case '\f':
							goto IL_021A;
						case '\r':
							if (this.chars[this.curPos + 1] == '\n')
							{
								this.curPos += 2;
							}
							else
							{
								if (this.curPos + 1 >= this.charsUsed && !this.readerAdapter.IsEof)
								{
									goto IL_029C;
								}
								this.curPos++;
							}
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						default:
							if (c != '"' && c != '&')
							{
								goto IL_021A;
							}
							break;
						}
					}
					else if (c != '\'')
					{
						if (c != '<')
						{
							if (c != ']')
							{
								goto IL_021A;
							}
							if (this.charsUsed - this.curPos < 3)
							{
								goto IL_029C;
							}
							if (this.chars[this.curPos + 1] != ']' || this.chars[this.curPos + 2] != '>')
							{
								this.curPos++;
								continue;
							}
							if (num > 0)
							{
								num--;
								this.curPos += 3;
								continue;
							}
							break;
						}
						else
						{
							if (this.charsUsed - this.curPos < 3)
							{
								goto IL_029C;
							}
							if (this.chars[this.curPos + 1] != '!' || this.chars[this.curPos + 2] != '[')
							{
								this.curPos++;
								continue;
							}
							num++;
							this.curPos += 3;
							continue;
						}
					}
					this.curPos++;
					continue;
					IL_021A:
					if (this.curPos != this.charsUsed)
					{
						if (!XmlCharType.IsHighSurrogate((int)this.chars[this.curPos]))
						{
							goto IL_0281;
						}
						if (this.curPos + 1 != this.charsUsed)
						{
							this.curPos++;
							if (XmlCharType.IsLowSurrogate((int)this.chars[this.curPos]))
							{
								this.curPos++;
								continue;
							}
							goto IL_0281;
						}
					}
					IL_029C:
					if (this.readerAdapter.IsEof || this.ReadData() == 0)
					{
						if (this.HandleEntityEnd(false))
						{
							continue;
						}
						this.Throw(this.curPos, "There is an unclosed conditional section.");
					}
					this.tokenStartPos = this.curPos;
				}
				else
				{
					this.curPos++;
				}
			}
			this.curPos += 3;
			this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
			return DtdParser.Token.CondSectionEnd;
			IL_0281:
			this.ThrowInvalidChar(this.chars, this.charsUsed, this.curPos);
			return DtdParser.Token.None;
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00075283 File Offset: 0x00073483
		private void ScanName()
		{
			this.ScanQName(false);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0007528C File Offset: 0x0007348C
		private void ScanQName()
		{
			this.ScanQName(this.SupportNamespaces);
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0007529C File Offset: 0x0007349C
		private void ScanQName(bool isQName)
		{
			this.tokenStartPos = this.curPos;
			int num = -1;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[(int)this.chars[this.curPos]] & 4) != 0 || this.chars[this.curPos] == ':')
				{
					this.curPos++;
				}
				else if (this.curPos + 1 >= this.charsUsed)
				{
					if (this.ReadDataInName())
					{
						continue;
					}
					this.Throw(this.curPos, "Unexpected end of file while parsing {0} has occurred.", "Name");
				}
				else
				{
					this.Throw(this.curPos, "Name cannot begin with the '{0}' character, hexadecimal value {1}.", XmlException.BuildCharExceptionArgs(this.chars, this.charsUsed, this.curPos));
				}
				for (;;)
				{
					if ((this.xmlCharType.charProperties[(int)this.chars[this.curPos]] & 8) != 0)
					{
						this.curPos++;
					}
					else if (this.chars[this.curPos] == ':')
					{
						if (isQName)
						{
							break;
						}
						this.curPos++;
					}
					else
					{
						if (this.curPos != this.charsUsed)
						{
							goto IL_0173;
						}
						if (!this.ReadDataInName())
						{
							goto Block_9;
						}
					}
				}
				if (num != -1)
				{
					this.Throw(this.curPos, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(':', '\0'));
				}
				num = this.curPos - this.tokenStartPos;
				this.curPos++;
			}
			Block_9:
			if (this.tokenStartPos == this.curPos)
			{
				this.Throw(this.curPos, "Unexpected end of file while parsing {0} has occurred.", "Name");
			}
			IL_0173:
			this.colonPos = ((num == -1) ? (-1) : (this.tokenStartPos + num));
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00075434 File Offset: 0x00073634
		private bool ReadDataInName()
		{
			int num = this.curPos - this.tokenStartPos;
			this.curPos = this.tokenStartPos;
			bool flag = this.ReadData() != 0;
			this.tokenStartPos = this.curPos;
			this.curPos += num;
			return flag;
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x00075480 File Offset: 0x00073680
		private void ScanNmtoken()
		{
			this.tokenStartPos = this.curPos;
			int num;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[(int)this.chars[this.curPos]] & 8) != 0 || this.chars[this.curPos] == ':')
				{
					this.curPos++;
				}
				else
				{
					if (this.curPos < this.charsUsed)
					{
						break;
					}
					num = this.curPos - this.tokenStartPos;
					this.curPos = this.tokenStartPos;
					if (this.ReadData() == 0)
					{
						if (num > 0)
						{
							goto Block_5;
						}
						this.Throw(this.curPos, "Unexpected end of file while parsing {0} has occurred.", "NmToken");
					}
					this.tokenStartPos = this.curPos;
					this.curPos += num;
				}
			}
			if (this.curPos - this.tokenStartPos == 0)
			{
				this.Throw(this.curPos, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(this.chars, this.charsUsed, this.curPos));
			}
			return;
			Block_5:
			this.tokenStartPos = this.curPos;
			this.curPos += num;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x00075594 File Offset: 0x00073794
		private bool EatPublicKeyword()
		{
			while (this.charsUsed - this.curPos < 6)
			{
				if (this.ReadData() == 0)
				{
					return false;
				}
			}
			if (this.chars[this.curPos + 1] != 'U' || this.chars[this.curPos + 2] != 'B' || this.chars[this.curPos + 3] != 'L' || this.chars[this.curPos + 4] != 'I' || this.chars[this.curPos + 5] != 'C')
			{
				return false;
			}
			this.curPos += 6;
			return true;
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x00075630 File Offset: 0x00073830
		private bool EatSystemKeyword()
		{
			while (this.charsUsed - this.curPos < 6)
			{
				if (this.ReadData() == 0)
				{
					return false;
				}
			}
			if (this.chars[this.curPos + 1] != 'Y' || this.chars[this.curPos + 2] != 'S' || this.chars[this.curPos + 3] != 'T' || this.chars[this.curPos + 4] != 'E' || this.chars[this.curPos + 5] != 'M')
			{
				return false;
			}
			this.curPos += 6;
			return true;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x000756CC File Offset: 0x000738CC
		private XmlQualifiedName GetNameQualified(bool canHavePrefix)
		{
			if (this.colonPos == -1)
			{
				return new XmlQualifiedName(this.nameTable.Add(this.chars, this.tokenStartPos, this.curPos - this.tokenStartPos));
			}
			if (canHavePrefix)
			{
				return new XmlQualifiedName(this.nameTable.Add(this.chars, this.colonPos + 1, this.curPos - this.colonPos - 1), this.nameTable.Add(this.chars, this.tokenStartPos, this.colonPos - this.tokenStartPos));
			}
			this.Throw(this.tokenStartPos, "'{0}' is an unqualified name and cannot contain the character ':'.", this.GetNameString());
			return null;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x00075779 File Offset: 0x00073979
		private string GetNameString()
		{
			return new string(this.chars, this.tokenStartPos, this.curPos - this.tokenStartPos);
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00075799 File Offset: 0x00073999
		private string GetNmtokenString()
		{
			return this.GetNameString();
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x000757A1 File Offset: 0x000739A1
		private string GetValue()
		{
			if (this.stringBuilder.Length == 0)
			{
				return new string(this.chars, this.tokenStartPos, this.curPos - this.tokenStartPos - 1);
			}
			return this.stringBuilder.ToString();
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x000757DC File Offset: 0x000739DC
		private string GetValueWithStrippedSpaces()
		{
			return DtdParser.StripSpaces((this.stringBuilder.Length == 0) ? new string(this.chars, this.tokenStartPos, this.curPos - this.tokenStartPos - 1) : this.stringBuilder.ToString());
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00075828 File Offset: 0x00073A28
		private int ReadData()
		{
			this.SaveParsingBuffer();
			int num = this.readerAdapter.ReadData();
			this.LoadParsingBuffer();
			return num;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00075841 File Offset: 0x00073A41
		private void LoadParsingBuffer()
		{
			this.chars = this.readerAdapter.ParsingBuffer;
			this.charsUsed = this.readerAdapter.ParsingBufferLength;
			this.curPos = this.readerAdapter.CurrentPosition;
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00075876 File Offset: 0x00073A76
		private void SaveParsingBuffer()
		{
			this.SaveParsingBuffer(this.curPos);
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x00075884 File Offset: 0x00073A84
		private void SaveParsingBuffer(int internalSubsetValueEndPos)
		{
			if (this.SaveInternalSubsetValue)
			{
				int currentPosition = this.readerAdapter.CurrentPosition;
				if (internalSubsetValueEndPos - currentPosition > 0)
				{
					this.internalSubsetValueSb.Append(this.chars, currentPosition, internalSubsetValueEndPos - currentPosition);
				}
			}
			this.readerAdapter.CurrentPosition = this.curPos;
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x000758D2 File Offset: 0x00073AD2
		private bool HandleEntityReference(bool paramEntity, bool inLiteral, bool inAttribute)
		{
			this.curPos++;
			return this.HandleEntityReference(this.ScanEntityName(), paramEntity, inLiteral, inAttribute);
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x000758F4 File Offset: 0x00073AF4
		private bool HandleEntityReference(XmlQualifiedName entityName, bool paramEntity, bool inLiteral, bool inAttribute)
		{
			this.SaveParsingBuffer();
			if (paramEntity && this.ParsingInternalSubset && !this.ParsingTopLevelMarkup)
			{
				this.Throw(this.curPos - entityName.Name.Length - 1, "A parameter entity reference is not allowed in internal markup.");
			}
			SchemaEntity schemaEntity = this.VerifyEntityReference(entityName, paramEntity, true, inAttribute);
			if (schemaEntity == null)
			{
				return false;
			}
			if (schemaEntity.ParsingInProgress)
			{
				this.Throw(this.curPos - entityName.Name.Length - 1, paramEntity ? "Parameter entity '{0}' references itself." : "General entity '{0}' references itself.", entityName.Name);
			}
			int num;
			if (schemaEntity.IsExternal)
			{
				if (!this.readerAdapter.PushEntity(schemaEntity, out num))
				{
					return false;
				}
				this.externalEntitiesDepth++;
			}
			else
			{
				if (schemaEntity.Text.Length == 0)
				{
					return false;
				}
				if (!this.readerAdapter.PushEntity(schemaEntity, out num))
				{
					return false;
				}
			}
			this.currentEntityId = num;
			if (paramEntity && !inLiteral && this.scanningFunction != DtdParser.ScanningFunction.ParamEntitySpace)
			{
				this.savedScanningFunction = this.scanningFunction;
				this.scanningFunction = DtdParser.ScanningFunction.ParamEntitySpace;
			}
			this.LoadParsingBuffer();
			return true;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00075A00 File Offset: 0x00073C00
		private bool HandleEntityEnd(bool inLiteral)
		{
			this.SaveParsingBuffer();
			IDtdEntityInfo dtdEntityInfo;
			if (!this.readerAdapter.PopEntity(out dtdEntityInfo, out this.currentEntityId))
			{
				return false;
			}
			this.LoadParsingBuffer();
			if (dtdEntityInfo == null)
			{
				if (this.scanningFunction == DtdParser.ScanningFunction.ParamEntitySpace)
				{
					this.scanningFunction = this.savedScanningFunction;
				}
				return false;
			}
			if (dtdEntityInfo.IsExternal)
			{
				this.externalEntitiesDepth--;
			}
			if (!inLiteral && this.scanningFunction != DtdParser.ScanningFunction.ParamEntitySpace)
			{
				this.savedScanningFunction = this.scanningFunction;
				this.scanningFunction = DtdParser.ScanningFunction.ParamEntitySpace;
			}
			return true;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00075A84 File Offset: 0x00073C84
		private SchemaEntity VerifyEntityReference(XmlQualifiedName entityName, bool paramEntity, bool mustBeDeclared, bool inAttribute)
		{
			SchemaEntity schemaEntity;
			if (paramEntity)
			{
				this.schemaInfo.ParameterEntities.TryGetValue(entityName, out schemaEntity);
			}
			else
			{
				this.schemaInfo.GeneralEntities.TryGetValue(entityName, out schemaEntity);
			}
			if (schemaEntity == null)
			{
				if (paramEntity)
				{
					if (this.validate)
					{
						this.SendValidationEvent(this.curPos - entityName.Name.Length - 1, XmlSeverityType.Error, "Reference to undeclared parameter entity '{0}'.", entityName.Name);
					}
				}
				else if (mustBeDeclared)
				{
					if (!this.ParsingInternalSubset)
					{
						if (this.validate)
						{
							this.SendValidationEvent(this.curPos - entityName.Name.Length - 1, XmlSeverityType.Error, "Reference to undeclared entity '{0}'.", entityName.Name);
						}
					}
					else
					{
						this.Throw(this.curPos - entityName.Name.Length - 1, "Reference to undeclared entity '{0}'.", entityName.Name);
					}
				}
				return null;
			}
			if (!schemaEntity.NData.IsEmpty)
			{
				this.Throw(this.curPos - entityName.Name.Length - 1, "Reference to unparsed entity '{0}'.", entityName.Name);
			}
			if (inAttribute && schemaEntity.IsExternal)
			{
				this.Throw(this.curPos - entityName.Name.Length - 1, "External entity '{0}' reference cannot appear in the attribute value.", entityName.Name);
			}
			return schemaEntity;
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00075BC0 File Offset: 0x00073DC0
		private void SendValidationEvent(int pos, XmlSeverityType severity, string code, string arg)
		{
			this.SendValidationEvent(severity, new XmlSchemaException(code, arg, this.BaseUriStr, this.LineNo, this.LinePos + (pos - this.curPos)));
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00075BF7 File Offset: 0x00073DF7
		private void SendValidationEvent(XmlSeverityType severity, string code, string arg)
		{
			this.SendValidationEvent(severity, new XmlSchemaException(code, arg, this.BaseUriStr, this.LineNo, this.LinePos));
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00075C1C File Offset: 0x00073E1C
		private void SendValidationEvent(XmlSeverityType severity, XmlSchemaException e)
		{
			IValidationEventHandling validationEventHandling = this.readerAdapterWithValidation.ValidationEventHandling;
			if (validationEventHandling != null)
			{
				validationEventHandling.SendEvent(e, severity);
			}
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00075C40 File Offset: 0x00073E40
		private bool IsAttributeValueType(DtdParser.Token token)
		{
			return token >= DtdParser.Token.CDATA && token <= DtdParser.Token.NOTATION;
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x00075C4F File Offset: 0x00073E4F
		private int LineNo
		{
			get
			{
				return this.readerAdapter.LineNo;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x00075C5C File Offset: 0x00073E5C
		private int LinePos
		{
			get
			{
				return this.curPos - this.readerAdapter.LineStartPosition;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001394 RID: 5012 RVA: 0x00075C70 File Offset: 0x00073E70
		private string BaseUriStr
		{
			get
			{
				Uri baseUri = this.readerAdapter.BaseUri;
				if (!(baseUri != null))
				{
					return string.Empty;
				}
				return baseUri.ToString();
			}
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00075C9E File Offset: 0x00073E9E
		private void OnUnexpectedError()
		{
			this.Throw(this.curPos, "An internal error has occurred.");
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x00075CB1 File Offset: 0x00073EB1
		private void Throw(int curPos, string res)
		{
			this.Throw(curPos, res, string.Empty);
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00075CC0 File Offset: 0x00073EC0
		private void Throw(int curPos, string res, string arg)
		{
			this.curPos = curPos;
			Uri baseUri = this.readerAdapter.BaseUri;
			this.readerAdapter.Throw(new XmlException(res, arg, this.LineNo, this.LinePos, (baseUri == null) ? null : baseUri.ToString()));
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00075D10 File Offset: 0x00073F10
		private void Throw(int curPos, string res, string[] args)
		{
			this.curPos = curPos;
			Uri baseUri = this.readerAdapter.BaseUri;
			this.readerAdapter.Throw(new XmlException(res, args, this.LineNo, this.LinePos, (baseUri == null) ? null : baseUri.ToString()));
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00075D60 File Offset: 0x00073F60
		private void Throw(string res, string arg, int lineNo, int linePos)
		{
			Uri baseUri = this.readerAdapter.BaseUri;
			this.readerAdapter.Throw(new XmlException(res, arg, lineNo, linePos, (baseUri == null) ? null : baseUri.ToString()));
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00075DA0 File Offset: 0x00073FA0
		private void ThrowInvalidChar(int pos, string data, int invCharPos)
		{
			this.Throw(pos, "'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(data, invCharPos));
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00075DB5 File Offset: 0x00073FB5
		private void ThrowInvalidChar(char[] data, int length, int invCharPos)
		{
			this.Throw(invCharPos, "'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(data, length, invCharPos));
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00075DCB File Offset: 0x00073FCB
		private void ThrowUnexpectedToken(int pos, string expectedToken)
		{
			this.ThrowUnexpectedToken(pos, expectedToken, null);
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00075DD8 File Offset: 0x00073FD8
		private void ThrowUnexpectedToken(int pos, string expectedToken1, string expectedToken2)
		{
			string text = this.ParseUnexpectedToken(pos);
			if (expectedToken2 != null)
			{
				this.Throw(this.curPos, "'{0}' is an unexpected token. The expected token is '{1}' or '{2}'.", new string[] { text, expectedToken1, expectedToken2 });
				return;
			}
			this.Throw(this.curPos, "'{0}' is an unexpected token. The expected token is '{1}'.", new string[] { text, expectedToken1 });
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00075E34 File Offset: 0x00074034
		private string ParseUnexpectedToken(int startPos)
		{
			if (this.xmlCharType.IsNCNameSingleChar(this.chars[startPos]))
			{
				int num = startPos;
				while (this.xmlCharType.IsNCNameSingleChar(this.chars[num]))
				{
					num++;
				}
				int num2 = num - startPos;
				return new string(this.chars, startPos, (num2 > 0) ? num2 : 1);
			}
			return new string(this.chars, startPos, 1);
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00075E9C File Offset: 0x0007409C
		internal static string StripSpaces(string value)
		{
			int length = value.Length;
			if (length <= 0)
			{
				return string.Empty;
			}
			int num = 0;
			StringBuilder stringBuilder = null;
			while (value[num] == ' ')
			{
				num++;
				if (num == length)
				{
					return " ";
				}
			}
			int i;
			for (i = num; i < length; i++)
			{
				if (value[i] == ' ')
				{
					int num2 = i + 1;
					while (num2 < length && value[num2] == ' ')
					{
						num2++;
					}
					if (num2 == length)
					{
						if (stringBuilder == null)
						{
							return value.Substring(num, i - num);
						}
						stringBuilder.Append(value, num, i - num);
						return stringBuilder.ToString();
					}
					else if (num2 > i + 1)
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(length);
						}
						stringBuilder.Append(value, num, i - num + 1);
						num = num2;
						i = num2 - 1;
					}
				}
			}
			if (stringBuilder != null)
			{
				if (i > num)
				{
					stringBuilder.Append(value, num, i - num);
				}
				return stringBuilder.ToString();
			}
			if (num != 0)
			{
				return value.Substring(num, length - num);
			}
			return value;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00075F84 File Offset: 0x00074184
		async Task<IDtdInfo> IDtdParser.ParseInternalDtdAsync(IDtdParserAdapter adapter, bool saveInternalSubset)
		{
			this.Initialize(adapter);
			await this.ParseAsync(saveInternalSubset).ConfigureAwait(false);
			return this.schemaInfo;
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00075FD8 File Offset: 0x000741D8
		async Task<IDtdInfo> IDtdParser.ParseFreeFloatingDtdAsync(string baseUri, string docTypeName, string publicId, string systemId, string internalSubset, IDtdParserAdapter adapter)
		{
			this.InitializeFreeFloatingDtd(baseUri, docTypeName, publicId, systemId, internalSubset, adapter);
			await this.ParseAsync(false).ConfigureAwait(false);
			return this.schemaInfo;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00076050 File Offset: 0x00074250
		private async Task ParseAsync(bool saveInternalSubset)
		{
			if (this.freeFloatingDtd)
			{
				await this.ParseFreeFloatingDtdAsync().ConfigureAwait(false);
			}
			else
			{
				await this.ParseInDocumentDtdAsync(saveInternalSubset).ConfigureAwait(false);
			}
			this.schemaInfo.Finish();
			if (this.validate && this.undeclaredNotations != null)
			{
				foreach (DtdParser.UndeclaredNotation undeclaredNotation in this.undeclaredNotations.Values)
				{
					DtdParser.UndeclaredNotation undeclaredNotation3;
					DtdParser.UndeclaredNotation undeclaredNotation2 = (undeclaredNotation3 = undeclaredNotation);
					while (undeclaredNotation3 != null)
					{
						this.SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("The '{0}' notation is not declared.", undeclaredNotation2.name, this.BaseUriStr, undeclaredNotation2.lineNo, undeclaredNotation2.linePos));
						undeclaredNotation3 = undeclaredNotation3.next;
					}
				}
			}
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0007609C File Offset: 0x0007429C
		private async Task ParseInDocumentDtdAsync(bool saveInternalSubset)
		{
			this.LoadParsingBuffer();
			this.scanningFunction = DtdParser.ScanningFunction.QName;
			this.nextScaningFunction = DtdParser.ScanningFunction.Doctype1;
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
			}
			if (configuredTaskAwaiter.GetResult() != DtdParser.Token.QName)
			{
				this.OnUnexpectedError();
			}
			this.schemaInfo.DocTypeName = this.GetNameQualified(true);
			DtdParser.Token token = await this.GetTokenAsync(false).ConfigureAwait(false);
			if (token == DtdParser.Token.SYSTEM || token == DtdParser.Token.PUBLIC)
			{
				Tuple<string, string> tuple = await this.ParseExternalIdAsync(token, DtdParser.Token.DOCTYPE).ConfigureAwait(false);
				this.publicId = tuple.Item1;
				this.systemId = tuple.Item2;
				token = await this.GetTokenAsync(false).ConfigureAwait(false);
			}
			if (token != DtdParser.Token.GreaterThan)
			{
				if (token == DtdParser.Token.LeftBracket)
				{
					if (saveInternalSubset)
					{
						this.SaveParsingBuffer();
						this.internalSubsetValueSb = new StringBuilder();
					}
					await this.ParseInternalSubsetAsync().ConfigureAwait(false);
				}
				else
				{
					this.OnUnexpectedError();
				}
			}
			this.SaveParsingBuffer();
			if (this.systemId != null && this.systemId.Length > 0)
			{
				await this.ParseExternalSubsetAsync().ConfigureAwait(false);
			}
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x000760E8 File Offset: 0x000742E8
		private async Task ParseFreeFloatingDtdAsync()
		{
			if (this.hasFreeFloatingInternalSubset)
			{
				this.LoadParsingBuffer();
				await this.ParseInternalSubsetAsync().ConfigureAwait(false);
				this.SaveParsingBuffer();
			}
			if (this.systemId != null && this.systemId.Length > 0)
			{
				await this.ParseExternalSubsetAsync().ConfigureAwait(false);
			}
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x0007612B File Offset: 0x0007432B
		private Task ParseInternalSubsetAsync()
		{
			return this.ParseSubsetAsync();
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00076134 File Offset: 0x00074334
		private async Task ParseExternalSubsetAsync()
		{
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.readerAdapter.PushExternalSubsetAsync(this.systemId, this.publicId).ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
			}
			if (configuredTaskAwaiter.GetResult())
			{
				Uri baseUri = this.readerAdapter.BaseUri;
				if (baseUri != null)
				{
					this.externalDtdBaseUri = baseUri.ToString();
				}
				this.externalEntitiesDepth++;
				this.LoadParsingBuffer();
				await this.ParseSubsetAsync().ConfigureAwait(false);
			}
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00076178 File Offset: 0x00074378
		private async Task ParseSubsetAsync()
		{
			for (;;)
			{
				DtdParser.Token token = await this.GetTokenAsync(false).ConfigureAwait(false);
				int startTagEntityId = this.currentEntityId;
				switch (token)
				{
				case DtdParser.Token.AttlistDecl:
					await this.ParseAttlistDeclAsync().ConfigureAwait(false);
					break;
				case DtdParser.Token.ElementDecl:
					await this.ParseElementDeclAsync().ConfigureAwait(false);
					break;
				case DtdParser.Token.EntityDecl:
					await this.ParseEntityDeclAsync().ConfigureAwait(false);
					break;
				case DtdParser.Token.NotationDecl:
					await this.ParseNotationDeclAsync().ConfigureAwait(false);
					break;
				case DtdParser.Token.Comment:
					await this.ParseCommentAsync().ConfigureAwait(false);
					break;
				case DtdParser.Token.PI:
					await this.ParsePIAsync().ConfigureAwait(false);
					break;
				case DtdParser.Token.CondSectionStart:
					if (this.ParsingInternalSubset)
					{
						this.Throw(this.curPos - 3, "A conditional section is not allowed in an internal subset.");
					}
					await this.ParseCondSectionAsync().ConfigureAwait(false);
					startTagEntityId = this.currentEntityId;
					break;
				case DtdParser.Token.CondSectionEnd:
					if (this.condSectionDepth > 0)
					{
						this.condSectionDepth--;
						if (this.validate && this.currentEntityId != this.condSectionEntityIds[this.condSectionDepth])
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
						}
					}
					else
					{
						this.Throw(this.curPos - 3, "']]>' is not expected.");
					}
					break;
				case DtdParser.Token.Eof:
					goto IL_055F;
				default:
					if (token == DtdParser.Token.RightBracket)
					{
						goto IL_0475;
					}
					break;
				}
				if (this.currentEntityId != startTagEntityId)
				{
					if (this.validate)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
					else if (!this.v1Compat)
					{
						this.Throw(this.curPos, "The parameter entity replacement text must nest properly within markup declarations.");
					}
				}
			}
			IL_0475:
			if (this.ParsingInternalSubset)
			{
				if (this.condSectionDepth != 0)
				{
					this.Throw(this.curPos, "There is an unclosed conditional section.");
				}
				if (this.internalSubsetValueSb != null)
				{
					this.SaveParsingBuffer(this.curPos - 1);
					this.schemaInfo.InternalDtdSubset = this.internalSubsetValueSb.ToString();
					this.internalSubsetValueSb = null;
				}
				ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() != DtdParser.Token.GreaterThan)
				{
					this.ThrowUnexpectedToken(this.curPos, ">");
				}
			}
			else
			{
				this.Throw(this.curPos, "Expected DTD markup was not found.");
			}
			return;
			IL_055F:
			if (this.ParsingInternalSubset && !this.freeFloatingDtd)
			{
				this.Throw(this.curPos, "Incomplete DTD content.");
			}
			if (this.condSectionDepth != 0)
			{
				this.Throw(this.curPos, "There is an unclosed conditional section.");
			}
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x000761BC File Offset: 0x000743BC
		private async Task ParseAttlistDeclAsync()
		{
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.GetTokenAsync(true).ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
			}
			if (configuredTaskAwaiter.GetResult() == DtdParser.Token.QName)
			{
				XmlQualifiedName nameQualified = this.GetNameQualified(true);
				SchemaElementDecl elementDecl;
				if (!this.schemaInfo.ElementDecls.TryGetValue(nameQualified, out elementDecl) && !this.schemaInfo.UndeclaredElementDecls.TryGetValue(nameQualified, out elementDecl))
				{
					elementDecl = new SchemaElementDecl(nameQualified, nameQualified.Namespace);
					this.schemaInfo.UndeclaredElementDecls.Add(nameQualified, elementDecl);
				}
				SchemaAttDef attrDef = null;
				DtdParser.Token token;
				for (;;)
				{
					token = await this.GetTokenAsync(false).ConfigureAwait(false);
					if (token != DtdParser.Token.QName)
					{
						break;
					}
					XmlQualifiedName nameQualified2 = this.GetNameQualified(true);
					attrDef = new SchemaAttDef(nameQualified2, nameQualified2.Namespace);
					attrDef.IsDeclaredInExternal = !this.ParsingInternalSubset;
					attrDef.LineNumber = this.LineNo;
					attrDef.LinePosition = this.LinePos - (this.curPos - this.tokenStartPos);
					bool attrDefAlreadyExists = elementDecl.GetAttDef(attrDef.Name) != null;
					await this.ParseAttlistTypeAsync(attrDef, elementDecl, attrDefAlreadyExists).ConfigureAwait(false);
					await this.ParseAttlistDefaultAsync(attrDef, attrDefAlreadyExists).ConfigureAwait(false);
					if (attrDef.Prefix.Length > 0 && attrDef.Prefix.Equals("xml"))
					{
						if (attrDef.Name.Name == "space")
						{
							if (this.v1Compat)
							{
								string text = attrDef.DefaultValueExpanded.Trim();
								if (text.Equals("preserve") || text.Equals("default"))
								{
									attrDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
								}
							}
							else
							{
								attrDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
								if (attrDef.TokenizedType != XmlTokenizedType.ENUMERATION)
								{
									this.Throw("Enumeration data type required.", string.Empty, attrDef.LineNumber, attrDef.LinePosition);
								}
								if (this.validate)
								{
									attrDef.CheckXmlSpace(this.readerAdapterWithValidation.ValidationEventHandling);
								}
							}
						}
						else if (attrDef.Name.Name == "lang")
						{
							attrDef.Reserved = SchemaAttDef.Reserve.XmlLang;
						}
					}
					if (!attrDefAlreadyExists)
					{
						elementDecl.AddAttDef(attrDef);
					}
				}
				if (token == DtdParser.Token.GreaterThan)
				{
					if (this.v1Compat && attrDef != null && attrDef.Prefix.Length > 0 && attrDef.Prefix.Equals("xml") && attrDef.Name.Name == "space")
					{
						attrDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
						if (attrDef.Datatype.TokenizedType != XmlTokenizedType.ENUMERATION)
						{
							this.Throw("Enumeration data type required.", string.Empty, attrDef.LineNumber, attrDef.LinePosition);
						}
						if (this.validate)
						{
							attrDef.CheckXmlSpace(this.readerAdapterWithValidation.ValidationEventHandling);
						}
					}
					return;
				}
			}
			this.OnUnexpectedError();
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00076200 File Offset: 0x00074400
		private async Task ParseAttlistTypeAsync(SchemaAttDef attrDef, SchemaElementDecl elementDecl, bool ignoreErrors)
		{
			DtdParser.Token token = await this.GetTokenAsync(true).ConfigureAwait(false);
			if (token != DtdParser.Token.CDATA)
			{
				elementDecl.HasNonCDataAttribute = true;
			}
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
			if (this.IsAttributeValueType(token))
			{
				attrDef.TokenizedType = (XmlTokenizedType)token;
				attrDef.SchemaType = XmlSchemaType.GetBuiltInSimpleType(attrDef.Datatype.TypeCode);
				if (token != DtdParser.Token.ID)
				{
					if (token == DtdParser.Token.NOTATION)
					{
						if (this.validate)
						{
							if (elementDecl.IsNotationDeclared && !ignoreErrors)
							{
								this.SendValidationEvent(this.curPos - 8, XmlSeverityType.Error, "No element type can have more than one NOTATION attribute specified.", elementDecl.Name.ToString());
							}
							else
							{
								if (elementDecl.ContentValidator != null && elementDecl.ContentValidator.ContentType == XmlSchemaContentType.Empty && !ignoreErrors)
								{
									this.SendValidationEvent(this.curPos - 8, XmlSeverityType.Error, "An attribute of type NOTATION must not be declared on an element declared EMPTY.", elementDecl.Name.ToString());
								}
								elementDecl.IsNotationDeclared = true;
							}
						}
						ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.GetTokenAsync(true).ConfigureAwait(false).GetAwaiter();
						if (!configuredTaskAwaiter.IsCompleted)
						{
							await configuredTaskAwaiter;
							configuredTaskAwaiter = configuredTaskAwaiter2;
							configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
						}
						if (configuredTaskAwaiter.GetResult() != DtdParser.Token.LeftParen)
						{
							goto IL_0667;
						}
						configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
						if (!configuredTaskAwaiter.IsCompleted)
						{
							await configuredTaskAwaiter;
							configuredTaskAwaiter = configuredTaskAwaiter2;
							configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
						}
						if (configuredTaskAwaiter.GetResult() != DtdParser.Token.Name)
						{
							goto IL_0667;
						}
						do
						{
							string nameString = this.GetNameString();
							if (!this.schemaInfo.Notations.ContainsKey(nameString))
							{
								this.AddUndeclaredNotation(nameString);
							}
							if (this.validate && !this.v1Compat && attrDef.Values != null && attrDef.Values.Contains(nameString) && !ignoreErrors)
							{
								this.SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("'{0}' is a duplicate notation value.", nameString, this.BaseUriStr, this.LineNo, this.LinePos));
							}
							attrDef.AddValue(nameString);
							DtdParser.Token token2 = await this.GetTokenAsync(false).ConfigureAwait(false);
							if (token2 == DtdParser.Token.RightParen)
							{
								goto IL_0451;
							}
							if (token2 != DtdParser.Token.Or)
							{
								break;
							}
							configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
							if (!configuredTaskAwaiter.IsCompleted)
							{
								await configuredTaskAwaiter;
								configuredTaskAwaiter = configuredTaskAwaiter2;
								configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
							}
						}
						while (configuredTaskAwaiter.GetResult() == DtdParser.Token.Name);
						goto IL_0667;
						IL_0451:;
					}
				}
				else
				{
					if (this.validate && elementDecl.IsIdDeclared)
					{
						SchemaAttDef attDef = elementDecl.GetAttDef(attrDef.Name);
						if ((attDef == null || attDef.Datatype.TokenizedType != XmlTokenizedType.ID) && !ignoreErrors)
						{
							this.SendValidationEvent(XmlSeverityType.Error, "The attribute of type ID is already declared on the '{0}' element.", elementDecl.Name.ToString());
						}
					}
					elementDecl.IsIdDeclared = true;
				}
				return;
			}
			if (token == DtdParser.Token.LeftParen)
			{
				attrDef.TokenizedType = XmlTokenizedType.ENUMERATION;
				attrDef.SchemaType = XmlSchemaType.GetBuiltInSimpleType(attrDef.Datatype.TypeCode);
				ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() == DtdParser.Token.Nmtoken)
				{
					attrDef.AddValue(this.GetNameString());
					for (;;)
					{
						DtdParser.Token token2 = await this.GetTokenAsync(false).ConfigureAwait(false);
						if (token2 == DtdParser.Token.RightParen)
						{
							break;
						}
						if (token2 != DtdParser.Token.Or)
						{
							goto IL_0667;
						}
						configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
						if (!configuredTaskAwaiter.IsCompleted)
						{
							await configuredTaskAwaiter;
							configuredTaskAwaiter = configuredTaskAwaiter2;
							configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
						}
						if (configuredTaskAwaiter.GetResult() != DtdParser.Token.Nmtoken)
						{
							goto IL_0667;
						}
						string nmtokenString = this.GetNmtokenString();
						if (this.validate && !this.v1Compat && attrDef.Values != null && attrDef.Values.Contains(nmtokenString) && !ignoreErrors)
						{
							this.SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("'{0}' is a duplicate enumeration value.", nmtokenString, this.BaseUriStr, this.LineNo, this.LinePos));
						}
						attrDef.AddValue(nmtokenString);
					}
					return;
				}
			}
			IL_0667:
			this.OnUnexpectedError();
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x0007625C File Offset: 0x0007445C
		private async Task ParseAttlistDefaultAsync(SchemaAttDef attrDef, bool ignoreErrors)
		{
			DtdParser.Token token = await this.GetTokenAsync(true).ConfigureAwait(false);
			switch (token)
			{
			case DtdParser.Token.REQUIRED:
				attrDef.Presence = SchemaDeclBase.Use.Required;
				return;
			case DtdParser.Token.IMPLIED:
				attrDef.Presence = SchemaDeclBase.Use.Implied;
				return;
			case DtdParser.Token.FIXED:
			{
				attrDef.Presence = SchemaDeclBase.Use.Fixed;
				ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.GetTokenAsync(true).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() != DtdParser.Token.Literal)
				{
					goto IL_01E8;
				}
				break;
			}
			default:
				if (token != DtdParser.Token.Literal)
				{
					goto IL_01E8;
				}
				break;
			}
			if (this.validate && attrDef.Datatype.TokenizedType == XmlTokenizedType.ID && !ignoreErrors)
			{
				this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "An attribute of type ID must have a declared default of either #IMPLIED or #REQUIRED.", string.Empty);
			}
			if (attrDef.TokenizedType != XmlTokenizedType.CDATA)
			{
				attrDef.DefaultValueExpanded = this.GetValueWithStrippedSpaces();
			}
			else
			{
				attrDef.DefaultValueExpanded = this.GetValue();
			}
			attrDef.ValueLineNumber = this.literalLineInfo.lineNo;
			attrDef.ValueLinePosition = this.literalLineInfo.linePos + 1;
			DtdValidator.SetDefaultTypedValue(attrDef, this.readerAdapter);
			return;
			IL_01E8:
			this.OnUnexpectedError();
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x000762B0 File Offset: 0x000744B0
		private async Task ParseElementDeclAsync()
		{
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.GetTokenAsync(true).ConfigureAwait(false).GetAwaiter();
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
			}
			if (configuredTaskAwaiter.GetResult() == DtdParser.Token.QName)
			{
				SchemaElementDecl elementDecl = null;
				XmlQualifiedName nameQualified = this.GetNameQualified(true);
				if (this.schemaInfo.ElementDecls.TryGetValue(nameQualified, out elementDecl))
				{
					if (this.validate)
					{
						this.SendValidationEvent(this.curPos - nameQualified.Name.Length, XmlSeverityType.Error, "The '{0}' element has already been declared.", this.GetNameString());
					}
				}
				else
				{
					if (this.schemaInfo.UndeclaredElementDecls.TryGetValue(nameQualified, out elementDecl))
					{
						this.schemaInfo.UndeclaredElementDecls.Remove(nameQualified);
					}
					else
					{
						elementDecl = new SchemaElementDecl(nameQualified, nameQualified.Namespace);
					}
					this.schemaInfo.ElementDecls.Add(nameQualified, elementDecl);
				}
				elementDecl.IsDeclaredInExternal = !this.ParsingInternalSubset;
				DtdParser.Token token = await this.GetTokenAsync(true).ConfigureAwait(false);
				if (token != DtdParser.Token.LeftParen)
				{
					if (token != DtdParser.Token.ANY)
					{
						if (token != DtdParser.Token.EMPTY)
						{
							goto IL_0466;
						}
						elementDecl.ContentValidator = ContentValidator.Empty;
					}
					else
					{
						elementDecl.ContentValidator = ContentValidator.Any;
					}
				}
				else
				{
					int startParenEntityId = this.currentEntityId;
					DtdParser.Token token2 = await this.GetTokenAsync(false).ConfigureAwait(false);
					if (token2 != DtdParser.Token.None)
					{
						if (token2 != DtdParser.Token.PCDATA)
						{
							goto IL_0466;
						}
						ParticleContentValidator pcv = new ParticleContentValidator(XmlSchemaContentType.Mixed);
						pcv.Start();
						pcv.OpenGroup();
						await this.ParseElementMixedContentAsync(pcv, startParenEntityId).ConfigureAwait(false);
						elementDecl.ContentValidator = pcv.Finish(true);
					}
					else
					{
						ParticleContentValidator pcv = null;
						pcv = new ParticleContentValidator(XmlSchemaContentType.ElementOnly);
						pcv.Start();
						pcv.OpenGroup();
						await this.ParseElementOnlyContentAsync(pcv, startParenEntityId).ConfigureAwait(false);
						elementDecl.ContentValidator = pcv.Finish(true);
					}
				}
				configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() != DtdParser.Token.GreaterThan)
				{
					this.ThrowUnexpectedToken(this.curPos, ">");
				}
				return;
			}
			IL_0466:
			this.OnUnexpectedError();
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x000762F4 File Offset: 0x000744F4
		private async Task ParseElementOnlyContentAsync(ParticleContentValidator pcv, int startParenEntityId)
		{
			Stack<DtdParser.ParseElementOnlyContent_LocalFrame> localFrames = new Stack<DtdParser.ParseElementOnlyContent_LocalFrame>();
			DtdParser.ParseElementOnlyContent_LocalFrame currentFrame = new DtdParser.ParseElementOnlyContent_LocalFrame(startParenEntityId);
			localFrames.Push(currentFrame);
			for (;;)
			{
				DtdParser.Token token = await this.GetTokenAsync(false).ConfigureAwait(false);
				if (token != DtdParser.Token.QName)
				{
					if (token == DtdParser.Token.LeftParen)
					{
						pcv.OpenGroup();
						currentFrame = new DtdParser.ParseElementOnlyContent_LocalFrame(this.currentEntityId);
						localFrames.Push(currentFrame);
						continue;
					}
					if (token != DtdParser.Token.GreaterThan)
					{
						goto IL_035B;
					}
					this.Throw(this.curPos, "Invalid content model.");
					goto IL_0361;
				}
				else
				{
					pcv.AddName(this.GetNameQualified(true), null);
					await this.ParseHowManyAsync(pcv).ConfigureAwait(false);
				}
				IL_019D:
				token = await this.GetTokenAsync(false).ConfigureAwait(false);
				switch (token)
				{
				case DtdParser.Token.RightParen:
					pcv.CloseGroup();
					if (this.validate && this.currentEntityId != currentFrame.startParenEntityId)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
					await this.ParseHowManyAsync(pcv).ConfigureAwait(false);
					break;
				case DtdParser.Token.GreaterThan:
					this.Throw(this.curPos, "Invalid content model.");
					break;
				case DtdParser.Token.Or:
					if (currentFrame.parsingSchema == DtdParser.Token.Comma)
					{
						this.Throw(this.curPos, "Invalid content model.");
					}
					pcv.AddChoice();
					currentFrame.parsingSchema = DtdParser.Token.Or;
					continue;
				default:
					if (token == DtdParser.Token.Comma)
					{
						if (currentFrame.parsingSchema == DtdParser.Token.Or)
						{
							this.Throw(this.curPos, "Invalid content model.");
						}
						pcv.AddSequence();
						currentFrame.parsingSchema = DtdParser.Token.Comma;
						continue;
					}
					goto IL_035B;
				}
				IL_0361:
				localFrames.Pop();
				if (localFrames.Count > 0)
				{
					currentFrame = localFrames.Peek();
					goto IL_019D;
				}
				break;
				IL_035B:
				this.OnUnexpectedError();
				goto IL_0361;
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00076348 File Offset: 0x00074548
		private async Task ParseHowManyAsync(ParticleContentValidator pcv)
		{
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
			}
			switch (configuredTaskAwaiter.GetResult())
			{
			case DtdParser.Token.Star:
				pcv.AddStar();
				break;
			case DtdParser.Token.QMark:
				pcv.AddQMark();
				break;
			case DtdParser.Token.Plus:
				pcv.AddPlus();
				break;
			}
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00076394 File Offset: 0x00074594
		private async Task ParseElementMixedContentAsync(ParticleContentValidator pcv, int startParenEntityId)
		{
			bool hasNames = false;
			int connectorEntityId = -1;
			int contentEntityId = this.currentEntityId;
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter;
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
			for (;;)
			{
				DtdParser.Token token = await this.GetTokenAsync(false).ConfigureAwait(false);
				if (token == DtdParser.Token.RightParen)
				{
					break;
				}
				if (token == DtdParser.Token.Or)
				{
					if (!hasNames)
					{
						hasNames = true;
					}
					else
					{
						pcv.AddChoice();
					}
					if (this.validate)
					{
						connectorEntityId = this.currentEntityId;
						if (contentEntityId < connectorEntityId)
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
						}
					}
					configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult() == DtdParser.Token.QName)
					{
						XmlQualifiedName nameQualified = this.GetNameQualified(true);
						if (pcv.Exists(nameQualified) && this.validate)
						{
							this.SendValidationEvent(XmlSeverityType.Error, "The '{0}' element already exists in the content model.", nameQualified.ToString());
						}
						pcv.AddName(nameQualified, null);
						if (!this.validate)
						{
							continue;
						}
						contentEntityId = this.currentEntityId;
						if (contentEntityId < connectorEntityId)
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
							continue;
						}
						continue;
					}
				}
				this.OnUnexpectedError();
			}
			pcv.CloseGroup();
			if (this.validate && this.currentEntityId != startParenEntityId)
			{
				this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
			}
			configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
			}
			if (configuredTaskAwaiter.GetResult() == DtdParser.Token.Star && hasNames)
			{
				pcv.AddStar();
			}
			else if (hasNames)
			{
				this.ThrowUnexpectedToken(this.curPos, "*");
			}
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x000763E8 File Offset: 0x000745E8
		private async Task ParseEntityDeclAsync()
		{
			bool isParamEntity = false;
			SchemaEntity entity = null;
			DtdParser.Token token = await this.GetTokenAsync(true).ConfigureAwait(false);
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter;
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
			if (token != DtdParser.Token.Name)
			{
				if (token != DtdParser.Token.Percent)
				{
					goto IL_0531;
				}
				isParamEntity = true;
				configuredTaskAwaiter = this.GetTokenAsync(true).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() != DtdParser.Token.Name)
				{
					goto IL_0531;
				}
			}
			XmlQualifiedName nameQualified = this.GetNameQualified(false);
			entity = new SchemaEntity(nameQualified, isParamEntity);
			entity.BaseURI = this.BaseUriStr;
			entity.DeclaredURI = ((this.externalDtdBaseUri.Length == 0) ? this.documentBaseUri : this.externalDtdBaseUri);
			if (isParamEntity)
			{
				if (!this.schemaInfo.ParameterEntities.ContainsKey(nameQualified))
				{
					this.schemaInfo.ParameterEntities.Add(nameQualified, entity);
				}
			}
			else if (!this.schemaInfo.GeneralEntities.ContainsKey(nameQualified))
			{
				this.schemaInfo.GeneralEntities.Add(nameQualified, entity);
			}
			entity.DeclaredInExternal = !this.ParsingInternalSubset;
			entity.ParsingInProgress = true;
			DtdParser.Token token2 = await this.GetTokenAsync(true).ConfigureAwait(false);
			if (token2 - DtdParser.Token.PUBLIC > 1)
			{
				if (token2 != DtdParser.Token.Literal)
				{
					goto IL_0531;
				}
				entity.Text = this.GetValue();
				entity.Line = this.literalLineInfo.lineNo;
				entity.Pos = this.literalLineInfo.linePos;
			}
			else
			{
				object obj = await this.ParseExternalIdAsync(token2, DtdParser.Token.EntityDecl).ConfigureAwait(false);
				string item = obj.Item1;
				string item2 = obj.Item2;
				entity.IsExternal = true;
				entity.Url = item2;
				entity.Pubid = item;
				configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() == DtdParser.Token.NData)
				{
					if (isParamEntity)
					{
						this.ThrowUnexpectedToken(this.curPos - 5, ">");
					}
					if (!this.whitespaceSeen)
					{
						this.Throw(this.curPos - 5, "'{0}' is an unexpected token. Expecting white space.", "NDATA");
					}
					configuredTaskAwaiter = this.GetTokenAsync(true).ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult() != DtdParser.Token.Name)
					{
						goto IL_0531;
					}
					entity.NData = this.GetNameQualified(false);
					string name = entity.NData.Name;
					if (!this.schemaInfo.Notations.ContainsKey(name))
					{
						this.AddUndeclaredNotation(name);
					}
				}
			}
			configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
			}
			if (configuredTaskAwaiter.GetResult() == DtdParser.Token.GreaterThan)
			{
				entity.ParsingInProgress = false;
				return;
			}
			IL_0531:
			this.OnUnexpectedError();
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x0007642C File Offset: 0x0007462C
		private async Task ParseNotationDeclAsync()
		{
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.GetTokenAsync(true).ConfigureAwait(false).GetAwaiter();
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
			}
			if (configuredTaskAwaiter.GetResult() != DtdParser.Token.Name)
			{
				this.OnUnexpectedError();
			}
			XmlQualifiedName nameQualified = this.GetNameQualified(false);
			SchemaNotation notation = null;
			if (!this.schemaInfo.Notations.ContainsKey(nameQualified.Name))
			{
				if (this.undeclaredNotations != null)
				{
					this.undeclaredNotations.Remove(nameQualified.Name);
				}
				notation = new SchemaNotation(nameQualified);
				this.schemaInfo.Notations.Add(notation.Name.Name, notation);
			}
			else if (this.validate)
			{
				this.SendValidationEvent(this.curPos - nameQualified.Name.Length, XmlSeverityType.Error, "The notation '{0}' has already been declared.", nameQualified.Name);
			}
			DtdParser.Token token = await this.GetTokenAsync(true).ConfigureAwait(false);
			if (token == DtdParser.Token.SYSTEM || token == DtdParser.Token.PUBLIC)
			{
				object obj = await this.ParseExternalIdAsync(token, DtdParser.Token.NOTATION).ConfigureAwait(false);
				string item = obj.Item1;
				string item2 = obj.Item2;
				if (notation != null)
				{
					notation.SystemLiteral = item2;
					notation.Pubid = item;
				}
			}
			else
			{
				this.OnUnexpectedError();
			}
			configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
			}
			if (configuredTaskAwaiter.GetResult() != DtdParser.Token.GreaterThan)
			{
				this.OnUnexpectedError();
			}
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00076470 File Offset: 0x00074670
		private async Task ParseCommentAsync()
		{
			this.SaveParsingBuffer();
			try
			{
				if (this.SaveInternalSubsetValue)
				{
					await this.readerAdapter.ParseCommentAsync(this.internalSubsetValueSb).ConfigureAwait(false);
					this.internalSubsetValueSb.Append("-->");
				}
				else
				{
					await this.readerAdapter.ParseCommentAsync(null).ConfigureAwait(false);
				}
			}
			catch (XmlException ex)
			{
				if (!(ex.ResString == "Unexpected end of file while parsing {0} has occurred.") || this.currentEntityId == 0)
				{
					throw;
				}
				this.SendValidationEvent(XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", null);
			}
			this.LoadParsingBuffer();
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x000764B4 File Offset: 0x000746B4
		private async Task ParsePIAsync()
		{
			this.SaveParsingBuffer();
			if (this.SaveInternalSubsetValue)
			{
				await this.readerAdapter.ParsePIAsync(this.internalSubsetValueSb).ConfigureAwait(false);
				this.internalSubsetValueSb.Append("?>");
			}
			else
			{
				await this.readerAdapter.ParsePIAsync(null).ConfigureAwait(false);
			}
			this.LoadParsingBuffer();
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x000764F8 File Offset: 0x000746F8
		private async Task ParseCondSectionAsync()
		{
			int csEntityId = this.currentEntityId;
			DtdParser.Token token = await this.GetTokenAsync(false).ConfigureAwait(false);
			if (token != DtdParser.Token.IGNORE)
			{
				if (token == DtdParser.Token.INCLUDE)
				{
					ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult() == DtdParser.Token.LeftBracket)
					{
						if (this.validate && csEntityId != this.currentEntityId)
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
						}
						if (this.validate)
						{
							if (this.condSectionEntityIds == null)
							{
								this.condSectionEntityIds = new int[2];
							}
							else if (this.condSectionEntityIds.Length == this.condSectionDepth)
							{
								int[] array = new int[this.condSectionEntityIds.Length * 2];
								Array.Copy(this.condSectionEntityIds, 0, array, 0, this.condSectionEntityIds.Length);
								this.condSectionEntityIds = array;
							}
							this.condSectionEntityIds[this.condSectionDepth] = csEntityId;
						}
						this.condSectionDepth++;
						return;
					}
				}
			}
			else
			{
				ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
				ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() == DtdParser.Token.LeftBracket)
				{
					if (this.validate && csEntityId != this.currentEntityId)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
					configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult() == DtdParser.Token.CondSectionEnd)
					{
						if (this.validate && csEntityId != this.currentEntityId)
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
							return;
						}
						return;
					}
				}
			}
			this.OnUnexpectedError();
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0007653C File Offset: 0x0007473C
		private async Task<Tuple<string, string>> ParseExternalIdAsync(DtdParser.Token idTokenType, DtdParser.Token declType)
		{
			LineInfo keywordLineInfo = new LineInfo(this.LineNo, this.LinePos - 6);
			string publicId = null;
			string systemId = null;
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.GetTokenAsync(true).ConfigureAwait(false).GetAwaiter();
			ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
			}
			if (configuredTaskAwaiter.GetResult() != DtdParser.Token.Literal)
			{
				this.ThrowUnexpectedToken(this.curPos, "\"", "'");
			}
			if (idTokenType == DtdParser.Token.SYSTEM)
			{
				systemId = this.GetValue();
				if (systemId.IndexOf('#') >= 0)
				{
					this.Throw(this.curPos - systemId.Length - 1, "Fragment identifier '{0}' cannot be part of the system identifier '{1}'.", new string[]
					{
						systemId.Substring(systemId.IndexOf('#')),
						systemId
					});
				}
				if (declType == DtdParser.Token.DOCTYPE && !this.freeFloatingDtd)
				{
					this.literalLineInfo.linePos = this.literalLineInfo.linePos + 1;
					this.readerAdapter.OnSystemId(systemId, keywordLineInfo, this.literalLineInfo);
				}
			}
			else
			{
				publicId = this.GetValue();
				int num = this.xmlCharType.IsPublicId(publicId);
				if (num >= 0)
				{
					this.ThrowInvalidChar(this.curPos - 1 - publicId.Length + num, publicId, num);
				}
				if (declType == DtdParser.Token.DOCTYPE && !this.freeFloatingDtd)
				{
					this.literalLineInfo.linePos = this.literalLineInfo.linePos + 1;
					this.readerAdapter.OnPublicId(publicId, keywordLineInfo, this.literalLineInfo);
					configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult() == DtdParser.Token.Literal)
					{
						if (!this.whitespaceSeen)
						{
							this.Throw("'{0}' is an unexpected token. Expecting white space.", new string(this.literalQuoteChar, 1), this.literalLineInfo.lineNo, this.literalLineInfo.linePos);
						}
						systemId = this.GetValue();
						this.literalLineInfo.linePos = this.literalLineInfo.linePos + 1;
						this.readerAdapter.OnSystemId(systemId, keywordLineInfo, this.literalLineInfo);
					}
					else
					{
						this.ThrowUnexpectedToken(this.curPos, "\"", "'");
					}
				}
				else
				{
					configuredTaskAwaiter = this.GetTokenAsync(false).ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<DtdParser.Token>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult() == DtdParser.Token.Literal)
					{
						if (!this.whitespaceSeen)
						{
							this.Throw("'{0}' is an unexpected token. Expecting white space.", new string(this.literalQuoteChar, 1), this.literalLineInfo.lineNo, this.literalLineInfo.linePos);
						}
						systemId = this.GetValue();
					}
					else if (declType != DtdParser.Token.NOTATION)
					{
						this.ThrowUnexpectedToken(this.curPos, "\"", "'");
					}
				}
			}
			return new Tuple<string, string>(publicId, systemId);
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00076590 File Offset: 0x00074790
		private async Task<DtdParser.Token> GetTokenAsync(bool needWhiteSpace)
		{
			this.whitespaceSeen = false;
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c <= '\r')
				{
					if (c != '\0')
					{
						switch (c)
						{
						case '\t':
							goto IL_01BB;
						case '\n':
							this.whitespaceSeen = true;
							this.curPos++;
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						case '\r':
							this.whitespaceSeen = true;
							if (this.chars[this.curPos + 1] == '\n')
							{
								if (this.Normalize)
								{
									this.SaveParsingBuffer();
									IDtdParserAdapter dtdParserAdapter = this.readerAdapter;
									int currentPosition = dtdParserAdapter.CurrentPosition;
									dtdParserAdapter.CurrentPosition = currentPosition + 1;
								}
								this.curPos += 2;
							}
							else
							{
								if (this.curPos + 1 >= this.charsUsed && !this.readerAdapter.IsEof)
								{
									goto IL_0CB6;
								}
								this.chars[this.curPos] = '\n';
								this.curPos++;
							}
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						}
						break;
					}
					if (this.curPos != this.charsUsed)
					{
						this.ThrowInvalidChar(this.chars, this.charsUsed, this.curPos);
						goto IL_0CB6;
					}
					goto IL_0CB6;
				}
				else if (c != ' ')
				{
					if (c != '%')
					{
						break;
					}
					if (this.charsUsed - this.curPos < 2)
					{
						goto IL_0CB6;
					}
					if (this.xmlCharType.IsWhiteSpace(this.chars[this.curPos + 1]))
					{
						break;
					}
					if (this.IgnoreEntityReferences)
					{
						this.curPos++;
						continue;
					}
					await this.HandleEntityReferenceAsync(true, false, false).ConfigureAwait(false);
					continue;
				}
				IL_01BB:
				this.whitespaceSeen = true;
				this.curPos++;
				continue;
				IL_0CB6:
				bool flag = this.readerAdapter.IsEof;
				if (!flag)
				{
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
					}
					flag = configuredTaskAwaiter.GetResult() == 0;
				}
				if (flag && !this.HandleEntityEnd(false))
				{
					if (this.scanningFunction == DtdParser.ScanningFunction.SubsetContent)
					{
						goto Block_21;
					}
					this.Throw(this.curPos, "Incomplete DTD content.");
				}
			}
			if (needWhiteSpace && !this.whitespaceSeen && this.scanningFunction != DtdParser.ScanningFunction.ParamEntitySpace)
			{
				this.Throw(this.curPos, "'{0}' is an unexpected token. Expecting white space.", this.ParseUnexpectedToken(this.curPos));
			}
			this.tokenStartPos = this.curPos;
			for (;;)
			{
				switch (this.scanningFunction)
				{
				case DtdParser.ScanningFunction.SubsetContent:
					goto IL_04B8;
				case DtdParser.ScanningFunction.Name:
					goto IL_036E;
				case DtdParser.ScanningFunction.QName:
					goto IL_03DC;
				case DtdParser.ScanningFunction.Nmtoken:
					goto IL_044A;
				case DtdParser.ScanningFunction.Doctype1:
					goto IL_0526;
				case DtdParser.ScanningFunction.Doctype2:
					goto IL_0594;
				case DtdParser.ScanningFunction.Element1:
					goto IL_05A0;
				case DtdParser.ScanningFunction.Element2:
					goto IL_060E;
				case DtdParser.ScanningFunction.Element3:
					goto IL_067C;
				case DtdParser.ScanningFunction.Element4:
					goto IL_06EA;
				case DtdParser.ScanningFunction.Element5:
					goto IL_06F6;
				case DtdParser.ScanningFunction.Element6:
					goto IL_0702;
				case DtdParser.ScanningFunction.Element7:
					goto IL_070E;
				case DtdParser.ScanningFunction.Attlist1:
					goto IL_071A;
				case DtdParser.ScanningFunction.Attlist2:
					goto IL_0789;
				case DtdParser.ScanningFunction.Attlist3:
					goto IL_07F8;
				case DtdParser.ScanningFunction.Attlist4:
					goto IL_0804;
				case DtdParser.ScanningFunction.Attlist5:
					goto IL_0810;
				case DtdParser.ScanningFunction.Attlist6:
					goto IL_081C;
				case DtdParser.ScanningFunction.Attlist7:
					goto IL_088B;
				case DtdParser.ScanningFunction.Entity1:
					goto IL_0A53;
				case DtdParser.ScanningFunction.Entity2:
					goto IL_0AC2;
				case DtdParser.ScanningFunction.Entity3:
					goto IL_0B31;
				case DtdParser.ScanningFunction.Notation1:
					goto IL_0897;
				case DtdParser.ScanningFunction.CondSection1:
					goto IL_0BA0;
				case DtdParser.ScanningFunction.CondSection2:
					goto IL_0C0F;
				case DtdParser.ScanningFunction.CondSection3:
					goto IL_0C1B;
				case DtdParser.ScanningFunction.SystemId:
					goto IL_0906;
				case DtdParser.ScanningFunction.PublicId1:
					goto IL_0975;
				case DtdParser.ScanningFunction.PublicId2:
					goto IL_09E4;
				case DtdParser.ScanningFunction.ClosingTag:
					goto IL_0C8A;
				case DtdParser.ScanningFunction.ParamEntitySpace:
					this.whitespaceSeen = true;
					this.scanningFunction = this.savedScanningFunction;
					continue;
				}
				break;
			}
			goto IL_0CAE;
			IL_036E:
			return await this.ScanNameExpectedAsync().ConfigureAwait(false);
			IL_03DC:
			return await this.ScanQNameExpectedAsync().ConfigureAwait(false);
			IL_044A:
			return await this.ScanNmtokenExpectedAsync().ConfigureAwait(false);
			IL_04B8:
			return await this.ScanSubsetContentAsync().ConfigureAwait(false);
			IL_0526:
			return await this.ScanDoctype1Async().ConfigureAwait(false);
			IL_0594:
			return this.ScanDoctype2();
			IL_05A0:
			return await this.ScanElement1Async().ConfigureAwait(false);
			IL_060E:
			return await this.ScanElement2Async().ConfigureAwait(false);
			IL_067C:
			return await this.ScanElement3Async().ConfigureAwait(false);
			IL_06EA:
			return this.ScanElement4();
			IL_06F6:
			return this.ScanElement5();
			IL_0702:
			return this.ScanElement6();
			IL_070E:
			return this.ScanElement7();
			IL_071A:
			return await this.ScanAttlist1Async().ConfigureAwait(false);
			IL_0789:
			return await this.ScanAttlist2Async().ConfigureAwait(false);
			IL_07F8:
			return this.ScanAttlist3();
			IL_0804:
			return this.ScanAttlist4();
			IL_0810:
			return this.ScanAttlist5();
			IL_081C:
			return await this.ScanAttlist6Async().ConfigureAwait(false);
			IL_088B:
			return this.ScanAttlist7();
			IL_0897:
			return await this.ScanNotation1Async().ConfigureAwait(false);
			IL_0906:
			return await this.ScanSystemIdAsync().ConfigureAwait(false);
			IL_0975:
			return await this.ScanPublicId1Async().ConfigureAwait(false);
			IL_09E4:
			return await this.ScanPublicId2Async().ConfigureAwait(false);
			IL_0A53:
			return await this.ScanEntity1Async().ConfigureAwait(false);
			IL_0AC2:
			return await this.ScanEntity2Async().ConfigureAwait(false);
			IL_0B31:
			return await this.ScanEntity3Async().ConfigureAwait(false);
			IL_0BA0:
			return await this.ScanCondSection1Async().ConfigureAwait(false);
			IL_0C0F:
			return this.ScanCondSection2();
			IL_0C1B:
			return await this.ScanCondSection3Async().ConfigureAwait(false);
			IL_0C8A:
			return this.ScanClosingTag();
			IL_0CAE:
			return DtdParser.Token.None;
			Block_21:
			return DtdParser.Token.Eof;
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x000765DC File Offset: 0x000747DC
		private async Task<DtdParser.Token> ScanSubsetContentAsync()
		{
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c != '<')
				{
					if (c == ']')
					{
						if (this.charsUsed - this.curPos < 2 && !this.readerAdapter.IsEof)
						{
							goto IL_0568;
						}
						if (this.chars[this.curPos + 1] != ']')
						{
							goto Block_40;
						}
						if (this.charsUsed - this.curPos < 3 && !this.readerAdapter.IsEof)
						{
							goto IL_0568;
						}
						if (this.chars[this.curPos + 1] == ']' && this.chars[this.curPos + 2] == '>')
						{
							goto Block_43;
						}
					}
					if (this.charsUsed - this.curPos != 0)
					{
						this.Throw(this.curPos, "Expected DTD markup was not found.");
					}
				}
				else
				{
					char c2 = this.chars[this.curPos + 1];
					if (c2 != '!')
					{
						if (c2 == '?')
						{
							goto IL_045C;
						}
						if (this.charsUsed - this.curPos >= 2)
						{
							goto Block_38;
						}
					}
					else
					{
						char c3 = this.chars[this.curPos + 2];
						if (c3 <= 'A')
						{
							if (c3 != '-')
							{
								if (c3 == 'A')
								{
									if (this.charsUsed - this.curPos >= 9)
									{
										goto Block_22;
									}
									goto IL_0568;
								}
							}
							else
							{
								if (this.chars[this.curPos + 3] == '-')
								{
									goto Block_35;
								}
								if (this.charsUsed - this.curPos >= 4)
								{
									this.Throw(this.curPos, "Expected DTD markup was not found.");
									goto IL_0568;
								}
								goto IL_0568;
							}
						}
						else if (c3 != 'E')
						{
							if (c3 != 'N')
							{
								if (c3 == '[')
								{
									goto IL_03C1;
								}
							}
							else
							{
								if (this.charsUsed - this.curPos >= 10)
								{
									goto Block_28;
								}
								goto IL_0568;
							}
						}
						else if (this.chars[this.curPos + 3] == 'L')
						{
							if (this.charsUsed - this.curPos >= 9)
							{
								break;
							}
							goto IL_0568;
						}
						else if (this.chars[this.curPos + 3] == 'N')
						{
							if (this.charsUsed - this.curPos >= 8)
							{
								goto Block_17;
							}
							goto IL_0568;
						}
						else
						{
							if (this.charsUsed - this.curPos >= 4)
							{
								goto Block_21;
							}
							goto IL_0568;
						}
						if (this.charsUsed - this.curPos >= 3)
						{
							this.Throw(this.curPos + 2, "Expected DTD markup was not found.");
						}
					}
				}
				IL_0568:
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() == 0)
				{
					this.Throw(this.charsUsed, "Incomplete DTD content.");
				}
			}
			if (this.chars[this.curPos + 4] != 'E' || this.chars[this.curPos + 5] != 'M' || this.chars[this.curPos + 6] != 'E' || this.chars[this.curPos + 7] != 'N' || this.chars[this.curPos + 8] != 'T')
			{
				this.Throw(this.curPos, "Expected DTD markup was not found.");
			}
			this.curPos += 9;
			this.scanningFunction = DtdParser.ScanningFunction.QName;
			this.nextScaningFunction = DtdParser.ScanningFunction.Element1;
			return DtdParser.Token.ElementDecl;
			Block_17:
			if (this.chars[this.curPos + 4] != 'T' || this.chars[this.curPos + 5] != 'I' || this.chars[this.curPos + 6] != 'T' || this.chars[this.curPos + 7] != 'Y')
			{
				this.Throw(this.curPos, "Expected DTD markup was not found.");
			}
			this.curPos += 8;
			this.scanningFunction = DtdParser.ScanningFunction.Entity1;
			return DtdParser.Token.EntityDecl;
			Block_21:
			this.Throw(this.curPos, "Expected DTD markup was not found.");
			return DtdParser.Token.None;
			Block_22:
			if (this.chars[this.curPos + 3] != 'T' || this.chars[this.curPos + 4] != 'T' || this.chars[this.curPos + 5] != 'L' || this.chars[this.curPos + 6] != 'I' || this.chars[this.curPos + 7] != 'S' || this.chars[this.curPos + 8] != 'T')
			{
				this.Throw(this.curPos, "Expected DTD markup was not found.");
			}
			this.curPos += 9;
			this.scanningFunction = DtdParser.ScanningFunction.QName;
			this.nextScaningFunction = DtdParser.ScanningFunction.Attlist1;
			return DtdParser.Token.AttlistDecl;
			Block_28:
			if (this.chars[this.curPos + 3] != 'O' || this.chars[this.curPos + 4] != 'T' || this.chars[this.curPos + 5] != 'A' || this.chars[this.curPos + 6] != 'T' || this.chars[this.curPos + 7] != 'I' || this.chars[this.curPos + 8] != 'O' || this.chars[this.curPos + 9] != 'N')
			{
				this.Throw(this.curPos, "Expected DTD markup was not found.");
			}
			this.curPos += 10;
			this.scanningFunction = DtdParser.ScanningFunction.Name;
			this.nextScaningFunction = DtdParser.ScanningFunction.Notation1;
			return DtdParser.Token.NotationDecl;
			IL_03C1:
			this.curPos += 3;
			this.scanningFunction = DtdParser.ScanningFunction.CondSection1;
			return DtdParser.Token.CondSectionStart;
			Block_35:
			this.curPos += 4;
			return DtdParser.Token.Comment;
			IL_045C:
			this.curPos += 2;
			return DtdParser.Token.PI;
			Block_38:
			this.Throw(this.curPos, "Expected DTD markup was not found.");
			return DtdParser.Token.None;
			Block_40:
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			return DtdParser.Token.RightBracket;
			Block_43:
			this.curPos += 3;
			return DtdParser.Token.CondSectionEnd;
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00076620 File Offset: 0x00074820
		private async Task<DtdParser.Token> ScanNameExpectedAsync()
		{
			await this.ScanNameAsync().ConfigureAwait(false);
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Name;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00076664 File Offset: 0x00074864
		private async Task<DtdParser.Token> ScanQNameExpectedAsync()
		{
			await this.ScanQNameAsync().ConfigureAwait(false);
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.QName;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x000766A8 File Offset: 0x000748A8
		private async Task<DtdParser.Token> ScanNmtokenExpectedAsync()
		{
			await this.ScanNmtokenAsync().ConfigureAwait(false);
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Nmtoken;
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x000766EC File Offset: 0x000748EC
		private async Task<DtdParser.Token> ScanDoctype1Async()
		{
			char c = this.chars[this.curPos];
			if (c <= 'P')
			{
				if (c == '>')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
					return DtdParser.Token.GreaterThan;
				}
				if (c == 'P')
				{
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.EatPublicKeywordAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
					}
					if (!configuredTaskAwaiter.GetResult())
					{
						this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.Doctype2;
					this.scanningFunction = DtdParser.ScanningFunction.PublicId1;
					return DtdParser.Token.PUBLIC;
				}
			}
			else
			{
				if (c == 'S')
				{
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.EatSystemKeywordAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
					}
					if (!configuredTaskAwaiter.GetResult())
					{
						this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.Doctype2;
					this.scanningFunction = DtdParser.ScanningFunction.SystemId;
					return DtdParser.Token.SYSTEM;
				}
				if (c == '[')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
					return DtdParser.Token.LeftBracket;
				}
			}
			this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
			return DtdParser.Token.None;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00076730 File Offset: 0x00074930
		private async Task<DtdParser.Token> ScanElement1Async()
		{
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c != '(')
				{
					if (c != 'A')
					{
						if (c != 'E')
						{
							goto IL_0130;
						}
						if (this.charsUsed - this.curPos >= 5)
						{
							if (this.chars[this.curPos + 1] == 'M' && this.chars[this.curPos + 2] == 'P' && this.chars[this.curPos + 3] == 'T' && this.chars[this.curPos + 4] == 'Y')
							{
								goto Block_7;
							}
							goto IL_0130;
						}
					}
					else if (this.charsUsed - this.curPos >= 3)
					{
						if (this.chars[this.curPos + 1] == 'N' && this.chars[this.curPos + 2] == 'Y')
						{
							goto Block_10;
						}
						goto IL_0130;
					}
					IL_0141:
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult() == 0)
					{
						this.Throw(this.curPos, "Incomplete DTD content.");
						continue;
					}
					continue;
					IL_0130:
					this.Throw(this.curPos, "Invalid content model.");
					goto IL_0141;
				}
				break;
			}
			this.scanningFunction = DtdParser.ScanningFunction.Element2;
			this.curPos++;
			return DtdParser.Token.LeftParen;
			Block_7:
			this.curPos += 5;
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			return DtdParser.Token.EMPTY;
			Block_10:
			this.curPos += 3;
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			return DtdParser.Token.ANY;
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x00076774 File Offset: 0x00074974
		private async Task<DtdParser.Token> ScanElement2Async()
		{
			if (this.chars[this.curPos] == '#')
			{
				while (this.charsUsed - this.curPos < 7)
				{
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult() == 0)
					{
						this.Throw(this.curPos, "Incomplete DTD content.");
					}
				}
				if (this.chars[this.curPos + 1] == 'P' && this.chars[this.curPos + 2] == 'C' && this.chars[this.curPos + 3] == 'D' && this.chars[this.curPos + 4] == 'A' && this.chars[this.curPos + 5] == 'T' && this.chars[this.curPos + 6] == 'A')
				{
					this.curPos += 7;
					this.scanningFunction = DtdParser.ScanningFunction.Element6;
					return DtdParser.Token.PCDATA;
				}
				this.Throw(this.curPos + 1, "Expecting 'PCDATA'.");
			}
			this.scanningFunction = DtdParser.ScanningFunction.Element3;
			return DtdParser.Token.None;
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x000767B8 File Offset: 0x000749B8
		private async Task<DtdParser.Token> ScanElement3Async()
		{
			char c = this.chars[this.curPos];
			DtdParser.Token token;
			if (c != '(')
			{
				if (c != '>')
				{
					await this.ScanQNameAsync().ConfigureAwait(false);
					this.scanningFunction = DtdParser.ScanningFunction.Element4;
					token = DtdParser.Token.QName;
				}
				else
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
					token = DtdParser.Token.GreaterThan;
				}
			}
			else
			{
				this.curPos++;
				token = DtdParser.Token.LeftParen;
			}
			return token;
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x000767FC File Offset: 0x000749FC
		private async Task<DtdParser.Token> ScanAttlist1Async()
		{
			DtdParser.Token token;
			if (this.chars[this.curPos] == '>')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
				token = DtdParser.Token.GreaterThan;
			}
			else
			{
				if (!this.whitespaceSeen)
				{
					this.Throw(this.curPos, "'{0}' is an unexpected token. Expecting white space.", this.ParseUnexpectedToken(this.curPos));
				}
				await this.ScanQNameAsync().ConfigureAwait(false);
				this.scanningFunction = DtdParser.ScanningFunction.Attlist2;
				token = DtdParser.Token.QName;
			}
			return token;
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00076840 File Offset: 0x00074A40
		private async Task<DtdParser.Token> ScanAttlist2Async()
		{
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c <= 'C')
				{
					if (c == '(')
					{
						break;
					}
					if (c != 'C')
					{
						goto IL_049A;
					}
					if (this.charsUsed - this.curPos >= 5)
					{
						goto Block_6;
					}
				}
				else if (c != 'E')
				{
					if (c != 'I')
					{
						if (c != 'N')
						{
							goto IL_049A;
						}
						if (this.charsUsed - this.curPos >= 8 || this.readerAdapter.IsEof)
						{
							char c2 = this.chars[this.curPos + 1];
							if (c2 == 'M')
							{
								goto IL_03D2;
							}
							if (c2 == 'O')
							{
								goto Block_24;
							}
							this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
						}
					}
					else if (this.charsUsed - this.curPos >= 6)
					{
						goto Block_17;
					}
				}
				else if (this.charsUsed - this.curPos >= 9)
				{
					this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
					if (this.chars[this.curPos + 1] != 'N' || this.chars[this.curPos + 2] != 'T' || this.chars[this.curPos + 3] != 'I' || this.chars[this.curPos + 4] != 'T')
					{
						this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
					}
					char c2 = this.chars[this.curPos + 5];
					if (c2 == 'I')
					{
						goto IL_019D;
					}
					if (c2 == 'Y')
					{
						goto IL_01E9;
					}
					this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
				}
				IL_04AB:
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() == 0)
				{
					this.Throw(this.curPos, "Incomplete DTD content.");
					continue;
				}
				continue;
				IL_049A:
				this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
				goto IL_04AB;
			}
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.Nmtoken;
			this.nextScaningFunction = DtdParser.ScanningFunction.Attlist5;
			return DtdParser.Token.LeftParen;
			Block_6:
			if (this.chars[this.curPos + 1] != 'D' || this.chars[this.curPos + 2] != 'A' || this.chars[this.curPos + 3] != 'T' || this.chars[this.curPos + 4] != 'A')
			{
				this.Throw(this.curPos, "Invalid attribute type.");
			}
			this.curPos += 5;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
			return DtdParser.Token.CDATA;
			IL_019D:
			if (this.chars[this.curPos + 6] != 'E' || this.chars[this.curPos + 7] != 'S')
			{
				this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
			}
			this.curPos += 8;
			return DtdParser.Token.ENTITIES;
			IL_01E9:
			this.curPos += 6;
			return DtdParser.Token.ENTITY;
			Block_17:
			this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
			if (this.chars[this.curPos + 1] != 'D')
			{
				this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
			}
			if (this.chars[this.curPos + 2] != 'R')
			{
				this.curPos += 2;
				return DtdParser.Token.ID;
			}
			if (this.chars[this.curPos + 3] != 'E' || this.chars[this.curPos + 4] != 'F')
			{
				this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
			}
			if (this.chars[this.curPos + 5] != 'S')
			{
				this.curPos += 5;
				return DtdParser.Token.IDREF;
			}
			this.curPos += 6;
			return DtdParser.Token.IDREFS;
			Block_24:
			if (this.chars[this.curPos + 2] != 'T' || this.chars[this.curPos + 3] != 'A' || this.chars[this.curPos + 4] != 'T' || this.chars[this.curPos + 5] != 'I' || this.chars[this.curPos + 6] != 'O' || this.chars[this.curPos + 7] != 'N')
			{
				this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
			}
			this.curPos += 8;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist3;
			return DtdParser.Token.NOTATION;
			IL_03D2:
			if (this.chars[this.curPos + 2] != 'T' || this.chars[this.curPos + 3] != 'O' || this.chars[this.curPos + 4] != 'K' || this.chars[this.curPos + 5] != 'E' || this.chars[this.curPos + 6] != 'N')
			{
				this.Throw(this.curPos, "'{0}' is an invalid attribute type.");
			}
			this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
			DtdParser.Token token;
			if (this.chars[this.curPos + 7] == 'S')
			{
				this.curPos += 8;
				token = DtdParser.Token.NMTOKENS;
			}
			else
			{
				this.curPos += 7;
				token = DtdParser.Token.NMTOKEN;
			}
			return token;
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x00076884 File Offset: 0x00074A84
		private async Task<DtdParser.Token> ScanAttlist6Async()
		{
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c == '"')
				{
					break;
				}
				if (c != '#')
				{
					if (c == '\'')
					{
						break;
					}
					this.Throw(this.curPos, "Expecting an attribute type.");
				}
				else if (this.charsUsed - this.curPos >= 6)
				{
					char c2 = this.chars[this.curPos + 1];
					if (c2 == 'F')
					{
						goto IL_0271;
					}
					if (c2 != 'I')
					{
						if (c2 == 'R')
						{
							if (this.charsUsed - this.curPos >= 9)
							{
								goto Block_6;
							}
						}
						else
						{
							this.Throw(this.curPos, "Expecting an attribute type.");
						}
					}
					else if (this.charsUsed - this.curPos >= 8)
					{
						goto Block_13;
					}
				}
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() == 0)
				{
					this.Throw(this.curPos, "Incomplete DTD content.");
				}
			}
			await this.ScanLiteralAsync(DtdParser.LiteralType.AttributeValue).ConfigureAwait(false);
			this.scanningFunction = DtdParser.ScanningFunction.Attlist1;
			return DtdParser.Token.Literal;
			Block_6:
			if (this.chars[this.curPos + 2] != 'E' || this.chars[this.curPos + 3] != 'Q' || this.chars[this.curPos + 4] != 'U' || this.chars[this.curPos + 5] != 'I' || this.chars[this.curPos + 6] != 'R' || this.chars[this.curPos + 7] != 'E' || this.chars[this.curPos + 8] != 'D')
			{
				this.Throw(this.curPos, "Expecting an attribute type.");
			}
			this.curPos += 9;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist1;
			return DtdParser.Token.REQUIRED;
			Block_13:
			if (this.chars[this.curPos + 2] != 'M' || this.chars[this.curPos + 3] != 'P' || this.chars[this.curPos + 4] != 'L' || this.chars[this.curPos + 5] != 'I' || this.chars[this.curPos + 6] != 'E' || this.chars[this.curPos + 7] != 'D')
			{
				this.Throw(this.curPos, "Expecting an attribute type.");
			}
			this.curPos += 8;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist1;
			return DtdParser.Token.IMPLIED;
			IL_0271:
			if (this.chars[this.curPos + 2] != 'I' || this.chars[this.curPos + 3] != 'X' || this.chars[this.curPos + 4] != 'E' || this.chars[this.curPos + 5] != 'D')
			{
				this.Throw(this.curPos, "Expecting an attribute type.");
			}
			this.curPos += 6;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist7;
			return DtdParser.Token.FIXED;
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x000768C8 File Offset: 0x00074AC8
		private async Task<DtdParser.Token> ScanLiteralAsync(DtdParser.LiteralType literalType)
		{
			char quoteChar = this.chars[this.curPos];
			char replaceChar = ((literalType == DtdParser.LiteralType.AttributeValue) ? ' ' : '\n');
			int startQuoteEntityId = this.currentEntityId;
			this.literalLineInfo.Set(this.LineNo, this.LinePos);
			this.curPos++;
			this.tokenStartPos = this.curPos;
			this.stringBuilder.Length = 0;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[(int)this.chars[this.curPos]] & 128) == 0 || this.chars[this.curPos] == '%')
				{
					if (this.chars[this.curPos] == quoteChar && this.currentEntityId == startQuoteEntityId)
					{
						break;
					}
					int num = this.curPos - this.tokenStartPos;
					if (num > 0)
					{
						this.stringBuilder.Append(this.chars, this.tokenStartPos, num);
						this.tokenStartPos = this.curPos;
					}
					char c = this.chars[this.curPos];
					if (c <= '\'')
					{
						switch (c)
						{
						case '\t':
							if (literalType == DtdParser.LiteralType.AttributeValue && this.Normalize)
							{
								this.stringBuilder.Append(' ');
								this.tokenStartPos++;
							}
							this.curPos++;
							continue;
						case '\n':
							this.curPos++;
							if (this.Normalize)
							{
								this.stringBuilder.Append(replaceChar);
								this.tokenStartPos = this.curPos;
							}
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						case '\v':
						case '\f':
							goto IL_07BB;
						case '\r':
							if (this.chars[this.curPos + 1] == '\n')
							{
								if (this.Normalize)
								{
									if (literalType == DtdParser.LiteralType.AttributeValue)
									{
										this.stringBuilder.Append(this.readerAdapter.IsEntityEolNormalized ? "  " : " ");
									}
									else
									{
										this.stringBuilder.Append(this.readerAdapter.IsEntityEolNormalized ? "\r\n" : "\n");
									}
									this.tokenStartPos = this.curPos + 2;
									this.SaveParsingBuffer();
									IDtdParserAdapter dtdParserAdapter = this.readerAdapter;
									int currentPosition = dtdParserAdapter.CurrentPosition;
									dtdParserAdapter.CurrentPosition = currentPosition + 1;
								}
								this.curPos += 2;
							}
							else
							{
								if (this.curPos + 1 == this.charsUsed)
								{
									goto IL_0842;
								}
								this.curPos++;
								if (this.Normalize)
								{
									this.stringBuilder.Append(replaceChar);
									this.tokenStartPos = this.curPos;
								}
							}
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						default:
							switch (c)
							{
							case '"':
							case '\'':
								break;
							case '#':
							case '$':
								goto IL_07BB;
							case '%':
								if (literalType != DtdParser.LiteralType.EntityReplText)
								{
									this.curPos++;
									continue;
								}
								await this.HandleEntityReferenceAsync(true, true, literalType == DtdParser.LiteralType.AttributeValue).ConfigureAwait(false);
								this.tokenStartPos = this.curPos;
								continue;
							case '&':
								if (literalType == DtdParser.LiteralType.SystemOrPublicID)
								{
									this.curPos++;
									continue;
								}
								if (this.curPos + 1 == this.charsUsed)
								{
									goto IL_0842;
								}
								if (this.chars[this.curPos + 1] == '#')
								{
									this.SaveParsingBuffer();
									int num2 = await this.readerAdapter.ParseNumericCharRefAsync(this.SaveInternalSubsetValue ? this.internalSubsetValueSb : null).ConfigureAwait(false);
									this.LoadParsingBuffer();
									this.stringBuilder.Append(this.chars, this.curPos, num2 - this.curPos);
									this.readerAdapter.CurrentPosition = num2;
									this.tokenStartPos = num2;
									this.curPos = num2;
									continue;
								}
								this.SaveParsingBuffer();
								if (literalType == DtdParser.LiteralType.AttributeValue)
								{
									int num3 = await this.readerAdapter.ParseNamedCharRefAsync(true, this.SaveInternalSubsetValue ? this.internalSubsetValueSb : null).ConfigureAwait(false);
									this.LoadParsingBuffer();
									if (num3 >= 0)
									{
										this.stringBuilder.Append(this.chars, this.curPos, num3 - this.curPos);
										this.readerAdapter.CurrentPosition = num3;
										this.tokenStartPos = num3;
										this.curPos = num3;
										continue;
									}
									await this.HandleEntityReferenceAsync(false, true, true).ConfigureAwait(false);
									this.tokenStartPos = this.curPos;
									continue;
								}
								else
								{
									int num4 = await this.readerAdapter.ParseNamedCharRefAsync(false, null).ConfigureAwait(false);
									this.LoadParsingBuffer();
									if (num4 >= 0)
									{
										this.tokenStartPos = this.curPos;
										this.curPos = num4;
										continue;
									}
									this.stringBuilder.Append('&');
									this.curPos++;
									this.tokenStartPos = this.curPos;
									this.VerifyEntityReference(this.ScanEntityName(), false, false, false);
									continue;
								}
								break;
							default:
								goto IL_07BB;
							}
							break;
						}
					}
					else
					{
						if (c == '<')
						{
							if (literalType == DtdParser.LiteralType.AttributeValue)
							{
								this.Throw(this.curPos, "'{0}', hexadecimal value {1}, is an invalid attribute character.", XmlException.BuildCharExceptionArgs('<', '\0'));
							}
							this.curPos++;
							continue;
						}
						if (c != '>')
						{
							goto IL_07BB;
						}
					}
					this.curPos++;
					continue;
					IL_07BB:
					if (this.curPos != this.charsUsed)
					{
						if (!XmlCharType.IsHighSurrogate((int)this.chars[this.curPos]))
						{
							goto IL_0822;
						}
						if (this.curPos + 1 != this.charsUsed)
						{
							this.curPos++;
							if (XmlCharType.IsLowSurrogate((int)this.chars[this.curPos]))
							{
								this.curPos++;
								continue;
							}
							goto IL_0822;
						}
					}
					IL_0842:
					bool flag = this.readerAdapter.IsEof;
					if (!flag)
					{
						ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
						if (!configuredTaskAwaiter.IsCompleted)
						{
							await configuredTaskAwaiter;
							ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
							configuredTaskAwaiter = configuredTaskAwaiter2;
							configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
						}
						flag = configuredTaskAwaiter.GetResult() == 0;
					}
					if (flag && (literalType == DtdParser.LiteralType.SystemOrPublicID || !this.HandleEntityEnd(true)))
					{
						this.Throw(this.curPos, "There is an unclosed literal string.");
					}
					this.tokenStartPos = this.curPos;
				}
				else
				{
					this.curPos++;
				}
			}
			if (this.stringBuilder.Length > 0)
			{
				this.stringBuilder.Append(this.chars, this.tokenStartPos, this.curPos - this.tokenStartPos);
			}
			this.curPos++;
			this.literalQuoteChar = quoteChar;
			return DtdParser.Token.Literal;
			IL_0822:
			this.ThrowInvalidChar(this.chars, this.charsUsed, this.curPos);
			return DtdParser.Token.None;
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x00076914 File Offset: 0x00074B14
		private async Task<DtdParser.Token> ScanNotation1Async()
		{
			char c = this.chars[this.curPos];
			DtdParser.Token token;
			if (c != 'P')
			{
				if (c != 'S')
				{
					this.Throw(this.curPos, "Expecting a system identifier or a public identifier.");
					token = DtdParser.Token.None;
				}
				else
				{
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.EatSystemKeywordAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
					}
					if (!configuredTaskAwaiter.GetResult())
					{
						this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.ClosingTag;
					this.scanningFunction = DtdParser.ScanningFunction.SystemId;
					token = DtdParser.Token.SYSTEM;
				}
			}
			else
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.EatPublicKeywordAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
				}
				this.nextScaningFunction = DtdParser.ScanningFunction.ClosingTag;
				this.scanningFunction = DtdParser.ScanningFunction.PublicId1;
				token = DtdParser.Token.PUBLIC;
			}
			return token;
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00076958 File Offset: 0x00074B58
		private async Task<DtdParser.Token> ScanSystemIdAsync()
		{
			if (this.chars[this.curPos] != '"' && this.chars[this.curPos] != '\'')
			{
				this.ThrowUnexpectedToken(this.curPos, "\"", "'");
			}
			await this.ScanLiteralAsync(DtdParser.LiteralType.SystemOrPublicID).ConfigureAwait(false);
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Literal;
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0007699C File Offset: 0x00074B9C
		private async Task<DtdParser.Token> ScanEntity1Async()
		{
			DtdParser.Token token;
			if (this.chars[this.curPos] == '%')
			{
				this.curPos++;
				this.nextScaningFunction = DtdParser.ScanningFunction.Entity2;
				this.scanningFunction = DtdParser.ScanningFunction.Name;
				token = DtdParser.Token.Percent;
			}
			else
			{
				await this.ScanNameAsync().ConfigureAwait(false);
				this.scanningFunction = DtdParser.ScanningFunction.Entity2;
				token = DtdParser.Token.Name;
			}
			return token;
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x000769E0 File Offset: 0x00074BE0
		private async Task<DtdParser.Token> ScanEntity2Async()
		{
			char c = this.chars[this.curPos];
			if (c <= '\'')
			{
				if (c == '"' || c == '\'')
				{
					await this.ScanLiteralAsync(DtdParser.LiteralType.EntityReplText).ConfigureAwait(false);
					this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
					return DtdParser.Token.Literal;
				}
			}
			else
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				if (c == 'P')
				{
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.EatPublicKeywordAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
					}
					if (!configuredTaskAwaiter.GetResult())
					{
						this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.Entity3;
					this.scanningFunction = DtdParser.ScanningFunction.PublicId1;
					return DtdParser.Token.PUBLIC;
				}
				if (c == 'S')
				{
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.EatSystemKeywordAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
					}
					if (!configuredTaskAwaiter.GetResult())
					{
						this.Throw(this.curPos, "Expecting external ID, '[' or '>'.");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.Entity3;
					this.scanningFunction = DtdParser.ScanningFunction.SystemId;
					return DtdParser.Token.SYSTEM;
				}
			}
			this.Throw(this.curPos, "Expecting an external identifier or an entity value.");
			return DtdParser.Token.None;
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00076A24 File Offset: 0x00074C24
		private async Task<DtdParser.Token> ScanEntity3Async()
		{
			if (this.chars[this.curPos] == 'N')
			{
				while (this.charsUsed - this.curPos < 5)
				{
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult() == 0)
					{
						goto IL_010C;
					}
				}
				if (this.chars[this.curPos + 1] == 'D' && this.chars[this.curPos + 2] == 'A' && this.chars[this.curPos + 3] == 'T' && this.chars[this.curPos + 4] == 'A')
				{
					this.curPos += 5;
					this.scanningFunction = DtdParser.ScanningFunction.Name;
					this.nextScaningFunction = DtdParser.ScanningFunction.ClosingTag;
					return DtdParser.Token.NData;
				}
			}
			IL_010C:
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			return DtdParser.Token.None;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00076A68 File Offset: 0x00074C68
		private async Task<DtdParser.Token> ScanPublicId1Async()
		{
			if (this.chars[this.curPos] != '"' && this.chars[this.curPos] != '\'')
			{
				this.ThrowUnexpectedToken(this.curPos, "\"", "'");
			}
			await this.ScanLiteralAsync(DtdParser.LiteralType.SystemOrPublicID).ConfigureAwait(false);
			this.scanningFunction = DtdParser.ScanningFunction.PublicId2;
			return DtdParser.Token.Literal;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00076AAC File Offset: 0x00074CAC
		private async Task<DtdParser.Token> ScanPublicId2Async()
		{
			DtdParser.Token token;
			if (this.chars[this.curPos] != '"' && this.chars[this.curPos] != '\'')
			{
				this.scanningFunction = this.nextScaningFunction;
				token = DtdParser.Token.None;
			}
			else
			{
				await this.ScanLiteralAsync(DtdParser.LiteralType.SystemOrPublicID).ConfigureAwait(false);
				this.scanningFunction = this.nextScaningFunction;
				token = DtdParser.Token.Literal;
			}
			return token;
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00076AF0 File Offset: 0x00074CF0
		private async Task<DtdParser.Token> ScanCondSection1Async()
		{
			if (this.chars[this.curPos] != 'I')
			{
				this.Throw(this.curPos, "Conditional sections must specify the keyword 'IGNORE' or 'INCLUDE'.");
			}
			this.curPos++;
			for (;;)
			{
				if (this.charsUsed - this.curPos >= 5)
				{
					char c = this.chars[this.curPos];
					if (c == 'G')
					{
						goto IL_013A;
					}
					if (c != 'N')
					{
						goto IL_01C8;
					}
					if (this.charsUsed - this.curPos >= 6)
					{
						break;
					}
				}
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() == 0)
				{
					this.Throw(this.curPos, "Incomplete DTD content.");
				}
			}
			if (this.chars[this.curPos + 1] == 'C' && this.chars[this.curPos + 2] == 'L' && this.chars[this.curPos + 3] == 'U' && this.chars[this.curPos + 4] == 'D' && this.chars[this.curPos + 5] == 'E' && !this.xmlCharType.IsNameSingleChar(this.chars[this.curPos + 6]))
			{
				this.nextScaningFunction = DtdParser.ScanningFunction.SubsetContent;
				this.scanningFunction = DtdParser.ScanningFunction.CondSection2;
				this.curPos += 6;
				return DtdParser.Token.INCLUDE;
			}
			goto IL_01C8;
			IL_013A:
			if (this.chars[this.curPos + 1] == 'N' && this.chars[this.curPos + 2] == 'O' && this.chars[this.curPos + 3] == 'R' && this.chars[this.curPos + 4] == 'E' && !this.xmlCharType.IsNameSingleChar(this.chars[this.curPos + 5]))
			{
				this.nextScaningFunction = DtdParser.ScanningFunction.CondSection3;
				this.scanningFunction = DtdParser.ScanningFunction.CondSection2;
				this.curPos += 5;
				return DtdParser.Token.IGNORE;
			}
			IL_01C8:
			this.Throw(this.curPos - 1, "Conditional sections must specify the keyword 'IGNORE' or 'INCLUDE'.");
			return DtdParser.Token.None;
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00076B34 File Offset: 0x00074D34
		private async Task<DtdParser.Token> ScanCondSection3Async()
		{
			int ignoreSectionDepth = 0;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[(int)this.chars[this.curPos]] & 64) == 0 || this.chars[this.curPos] == ']')
				{
					char c = this.chars[this.curPos];
					if (c <= '&')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
							this.curPos++;
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						case '\v':
						case '\f':
							goto IL_0259;
						case '\r':
							if (this.chars[this.curPos + 1] == '\n')
							{
								this.curPos += 2;
							}
							else
							{
								if (this.curPos + 1 >= this.charsUsed && !this.readerAdapter.IsEof)
								{
									goto IL_02E0;
								}
								this.curPos++;
							}
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						default:
							if (c != '"' && c != '&')
							{
								goto IL_0259;
							}
							break;
						}
					}
					else if (c != '\'')
					{
						if (c != '<')
						{
							if (c != ']')
							{
								goto IL_0259;
							}
							if (this.charsUsed - this.curPos < 3)
							{
								goto IL_02E0;
							}
							if (this.chars[this.curPos + 1] != ']' || this.chars[this.curPos + 2] != '>')
							{
								this.curPos++;
								continue;
							}
							if (ignoreSectionDepth > 0)
							{
								int num = ignoreSectionDepth;
								ignoreSectionDepth = num - 1;
								this.curPos += 3;
								continue;
							}
							break;
						}
						else
						{
							if (this.charsUsed - this.curPos < 3)
							{
								goto IL_02E0;
							}
							if (this.chars[this.curPos + 1] != '!' || this.chars[this.curPos + 2] != '[')
							{
								this.curPos++;
								continue;
							}
							int num = ignoreSectionDepth;
							ignoreSectionDepth = num + 1;
							this.curPos += 3;
							continue;
						}
					}
					this.curPos++;
					continue;
					IL_0259:
					if (this.curPos != this.charsUsed)
					{
						if (!XmlCharType.IsHighSurrogate((int)this.chars[this.curPos]))
						{
							goto IL_02C0;
						}
						if (this.curPos + 1 != this.charsUsed)
						{
							this.curPos++;
							if (XmlCharType.IsLowSurrogate((int)this.chars[this.curPos]))
							{
								this.curPos++;
								continue;
							}
							goto IL_02C0;
						}
					}
					IL_02E0:
					bool flag = this.readerAdapter.IsEof;
					if (!flag)
					{
						ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
						if (!configuredTaskAwaiter.IsCompleted)
						{
							await configuredTaskAwaiter;
							ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
							configuredTaskAwaiter = configuredTaskAwaiter2;
							configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
						}
						flag = configuredTaskAwaiter.GetResult() == 0;
					}
					if (flag)
					{
						if (this.HandleEntityEnd(false))
						{
							continue;
						}
						this.Throw(this.curPos, "There is an unclosed conditional section.");
					}
					this.tokenStartPos = this.curPos;
				}
				else
				{
					this.curPos++;
				}
			}
			this.curPos += 3;
			this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
			return DtdParser.Token.CondSectionEnd;
			IL_02C0:
			this.ThrowInvalidChar(this.chars, this.charsUsed, this.curPos);
			return DtdParser.Token.None;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00076B77 File Offset: 0x00074D77
		private Task ScanNameAsync()
		{
			return this.ScanQNameAsync(false);
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00076B80 File Offset: 0x00074D80
		private Task ScanQNameAsync()
		{
			return this.ScanQNameAsync(this.SupportNamespaces);
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00076B90 File Offset: 0x00074D90
		private async Task ScanQNameAsync(bool isQName)
		{
			this.tokenStartPos = this.curPos;
			int colonOffset = -1;
			for (;;)
			{
				bool flag = false;
				if ((this.xmlCharType.charProperties[(int)this.chars[this.curPos]] & 4) != 0 || this.chars[this.curPos] == ':')
				{
					this.curPos++;
				}
				else if (this.curPos + 1 >= this.charsUsed)
				{
					flag = true;
				}
				else
				{
					this.Throw(this.curPos, "Name cannot begin with the '{0}' character, hexadecimal value {1}.", XmlException.BuildCharExceptionArgs(this.chars, this.charsUsed, this.curPos));
				}
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				if (flag)
				{
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataInNameAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult())
					{
						continue;
					}
					this.Throw(this.curPos, "Unexpected end of file while parsing {0} has occurred.", "Name");
				}
				for (;;)
				{
					if ((this.xmlCharType.charProperties[(int)this.chars[this.curPos]] & 8) != 0)
					{
						this.curPos++;
					}
					else if (this.chars[this.curPos] == ':')
					{
						if (isQName)
						{
							break;
						}
						this.curPos++;
					}
					else
					{
						if (this.curPos != this.charsUsed)
						{
							goto IL_0270;
						}
						ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataInNameAsync().ConfigureAwait(false).GetAwaiter();
						if (!configuredTaskAwaiter.IsCompleted)
						{
							await configuredTaskAwaiter;
							configuredTaskAwaiter = configuredTaskAwaiter2;
							configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
						}
						if (!configuredTaskAwaiter.GetResult())
						{
							goto Block_12;
						}
					}
				}
				if (colonOffset != -1)
				{
					this.Throw(this.curPos, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(':', '\0'));
				}
				colonOffset = this.curPos - this.tokenStartPos;
				this.curPos++;
			}
			Block_12:
			if (this.tokenStartPos == this.curPos)
			{
				this.Throw(this.curPos, "Unexpected end of file while parsing {0} has occurred.", "Name");
			}
			IL_0270:
			this.colonPos = ((colonOffset == -1) ? (-1) : (this.tokenStartPos + colonOffset));
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00076BDC File Offset: 0x00074DDC
		private async Task<bool> ReadDataInNameAsync()
		{
			int offset = this.curPos - this.tokenStartPos;
			this.curPos = this.tokenStartPos;
			ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
			}
			bool flag = configuredTaskAwaiter.GetResult() != 0;
			this.tokenStartPos = this.curPos;
			this.curPos += offset;
			return flag;
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00076C20 File Offset: 0x00074E20
		private async Task ScanNmtokenAsync()
		{
			this.tokenStartPos = this.curPos;
			int len;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[(int)this.chars[this.curPos]] & 8) != 0 || this.chars[this.curPos] == ':')
				{
					this.curPos++;
				}
				else
				{
					if (this.curPos < this.charsUsed)
					{
						break;
					}
					len = this.curPos - this.tokenStartPos;
					this.curPos = this.tokenStartPos;
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult() == 0)
					{
						if (len > 0)
						{
							goto Block_6;
						}
						this.Throw(this.curPos, "Unexpected end of file while parsing {0} has occurred.", "NmToken");
					}
					this.tokenStartPos = this.curPos;
					this.curPos += len;
				}
			}
			if (this.curPos - this.tokenStartPos == 0)
			{
				this.Throw(this.curPos, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(this.chars, this.charsUsed, this.curPos));
			}
			return;
			Block_6:
			this.tokenStartPos = this.curPos;
			this.curPos += len;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00076C64 File Offset: 0x00074E64
		private async Task<bool> EatPublicKeywordAsync()
		{
			while (this.charsUsed - this.curPos < 6)
			{
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() == 0)
				{
					return false;
				}
			}
			if (this.chars[this.curPos + 1] != 'U' || this.chars[this.curPos + 2] != 'B' || this.chars[this.curPos + 3] != 'L' || this.chars[this.curPos + 4] != 'I' || this.chars[this.curPos + 5] != 'C')
			{
				return false;
			}
			this.curPos += 6;
			return true;
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x00076CA8 File Offset: 0x00074EA8
		private async Task<bool> EatSystemKeywordAsync()
		{
			while (this.charsUsed - this.curPos < 6)
			{
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadDataAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
				}
				if (configuredTaskAwaiter.GetResult() == 0)
				{
					return false;
				}
			}
			if (this.chars[this.curPos + 1] != 'Y' || this.chars[this.curPos + 2] != 'S' || this.chars[this.curPos + 3] != 'T' || this.chars[this.curPos + 4] != 'E' || this.chars[this.curPos + 5] != 'M')
			{
				return false;
			}
			this.curPos += 6;
			return true;
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x00076CEC File Offset: 0x00074EEC
		private async Task<int> ReadDataAsync()
		{
			this.SaveParsingBuffer();
			int num = await this.readerAdapter.ReadDataAsync().ConfigureAwait(false);
			this.LoadParsingBuffer();
			return num;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00076D2F File Offset: 0x00074F2F
		private Task<bool> HandleEntityReferenceAsync(bool paramEntity, bool inLiteral, bool inAttribute)
		{
			this.curPos++;
			return this.HandleEntityReferenceAsync(this.ScanEntityName(), paramEntity, inLiteral, inAttribute);
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x00076D50 File Offset: 0x00074F50
		private async Task<bool> HandleEntityReferenceAsync(XmlQualifiedName entityName, bool paramEntity, bool inLiteral, bool inAttribute)
		{
			this.SaveParsingBuffer();
			if (paramEntity && this.ParsingInternalSubset && !this.ParsingTopLevelMarkup)
			{
				this.Throw(this.curPos - entityName.Name.Length - 1, "A parameter entity reference is not allowed in internal markup.");
			}
			SchemaEntity schemaEntity = this.VerifyEntityReference(entityName, paramEntity, true, inAttribute);
			bool flag;
			if (schemaEntity == null)
			{
				flag = false;
			}
			else
			{
				if (schemaEntity.ParsingInProgress)
				{
					this.Throw(this.curPos - entityName.Name.Length - 1, paramEntity ? "Parameter entity '{0}' references itself." : "General entity '{0}' references itself.", entityName.Name);
				}
				int num;
				if (schemaEntity.IsExternal)
				{
					object obj = await this.readerAdapter.PushEntityAsync(schemaEntity).ConfigureAwait(false);
					num = obj.Item1;
					if (!obj.Item2)
					{
						return false;
					}
					this.externalEntitiesDepth++;
				}
				else
				{
					if (schemaEntity.Text.Length == 0)
					{
						return false;
					}
					object obj2 = await this.readerAdapter.PushEntityAsync(schemaEntity).ConfigureAwait(false);
					num = obj2.Item1;
					if (!obj2.Item2)
					{
						return false;
					}
				}
				this.currentEntityId = num;
				if (paramEntity && !inLiteral && this.scanningFunction != DtdParser.ScanningFunction.ParamEntitySpace)
				{
					this.savedScanningFunction = this.scanningFunction;
					this.scanningFunction = DtdParser.ScanningFunction.ParamEntitySpace;
				}
				this.LoadParsingBuffer();
				flag = true;
			}
			return flag;
		}

		// Token: 0x040010F5 RID: 4341
		private IDtdParserAdapter readerAdapter;

		// Token: 0x040010F6 RID: 4342
		private IDtdParserAdapterWithValidation readerAdapterWithValidation;

		// Token: 0x040010F7 RID: 4343
		private XmlNameTable nameTable;

		// Token: 0x040010F8 RID: 4344
		private SchemaInfo schemaInfo;

		// Token: 0x040010F9 RID: 4345
		private XmlCharType xmlCharType = XmlCharType.Instance;

		// Token: 0x040010FA RID: 4346
		private string systemId = string.Empty;

		// Token: 0x040010FB RID: 4347
		private string publicId = string.Empty;

		// Token: 0x040010FC RID: 4348
		private bool normalize = true;

		// Token: 0x040010FD RID: 4349
		private bool validate;

		// Token: 0x040010FE RID: 4350
		private bool supportNamespaces = true;

		// Token: 0x040010FF RID: 4351
		private bool v1Compat;

		// Token: 0x04001100 RID: 4352
		private char[] chars;

		// Token: 0x04001101 RID: 4353
		private int charsUsed;

		// Token: 0x04001102 RID: 4354
		private int curPos;

		// Token: 0x04001103 RID: 4355
		private DtdParser.ScanningFunction scanningFunction;

		// Token: 0x04001104 RID: 4356
		private DtdParser.ScanningFunction nextScaningFunction;

		// Token: 0x04001105 RID: 4357
		private DtdParser.ScanningFunction savedScanningFunction;

		// Token: 0x04001106 RID: 4358
		private bool whitespaceSeen;

		// Token: 0x04001107 RID: 4359
		private int tokenStartPos;

		// Token: 0x04001108 RID: 4360
		private int colonPos;

		// Token: 0x04001109 RID: 4361
		private StringBuilder internalSubsetValueSb;

		// Token: 0x0400110A RID: 4362
		private int externalEntitiesDepth;

		// Token: 0x0400110B RID: 4363
		private int currentEntityId;

		// Token: 0x0400110C RID: 4364
		private bool freeFloatingDtd;

		// Token: 0x0400110D RID: 4365
		private bool hasFreeFloatingInternalSubset;

		// Token: 0x0400110E RID: 4366
		private StringBuilder stringBuilder;

		// Token: 0x0400110F RID: 4367
		private int condSectionDepth;

		// Token: 0x04001110 RID: 4368
		private LineInfo literalLineInfo = new LineInfo(0, 0);

		// Token: 0x04001111 RID: 4369
		private char literalQuoteChar = '"';

		// Token: 0x04001112 RID: 4370
		private string documentBaseUri = string.Empty;

		// Token: 0x04001113 RID: 4371
		private string externalDtdBaseUri = string.Empty;

		// Token: 0x04001114 RID: 4372
		private Dictionary<string, DtdParser.UndeclaredNotation> undeclaredNotations;

		// Token: 0x04001115 RID: 4373
		private int[] condSectionEntityIds;

		// Token: 0x04001116 RID: 4374
		private const int CondSectionEntityIdsInitialSize = 2;

		// Token: 0x020001EB RID: 491
		private enum Token
		{
			// Token: 0x04001118 RID: 4376
			CDATA,
			// Token: 0x04001119 RID: 4377
			ID,
			// Token: 0x0400111A RID: 4378
			IDREF,
			// Token: 0x0400111B RID: 4379
			IDREFS,
			// Token: 0x0400111C RID: 4380
			ENTITY,
			// Token: 0x0400111D RID: 4381
			ENTITIES,
			// Token: 0x0400111E RID: 4382
			NMTOKEN,
			// Token: 0x0400111F RID: 4383
			NMTOKENS,
			// Token: 0x04001120 RID: 4384
			NOTATION,
			// Token: 0x04001121 RID: 4385
			None,
			// Token: 0x04001122 RID: 4386
			PERef,
			// Token: 0x04001123 RID: 4387
			AttlistDecl,
			// Token: 0x04001124 RID: 4388
			ElementDecl,
			// Token: 0x04001125 RID: 4389
			EntityDecl,
			// Token: 0x04001126 RID: 4390
			NotationDecl,
			// Token: 0x04001127 RID: 4391
			Comment,
			// Token: 0x04001128 RID: 4392
			PI,
			// Token: 0x04001129 RID: 4393
			CondSectionStart,
			// Token: 0x0400112A RID: 4394
			CondSectionEnd,
			// Token: 0x0400112B RID: 4395
			Eof,
			// Token: 0x0400112C RID: 4396
			REQUIRED,
			// Token: 0x0400112D RID: 4397
			IMPLIED,
			// Token: 0x0400112E RID: 4398
			FIXED,
			// Token: 0x0400112F RID: 4399
			QName,
			// Token: 0x04001130 RID: 4400
			Name,
			// Token: 0x04001131 RID: 4401
			Nmtoken,
			// Token: 0x04001132 RID: 4402
			Quote,
			// Token: 0x04001133 RID: 4403
			LeftParen,
			// Token: 0x04001134 RID: 4404
			RightParen,
			// Token: 0x04001135 RID: 4405
			GreaterThan,
			// Token: 0x04001136 RID: 4406
			Or,
			// Token: 0x04001137 RID: 4407
			LeftBracket,
			// Token: 0x04001138 RID: 4408
			RightBracket,
			// Token: 0x04001139 RID: 4409
			PUBLIC,
			// Token: 0x0400113A RID: 4410
			SYSTEM,
			// Token: 0x0400113B RID: 4411
			Literal,
			// Token: 0x0400113C RID: 4412
			DOCTYPE,
			// Token: 0x0400113D RID: 4413
			NData,
			// Token: 0x0400113E RID: 4414
			Percent,
			// Token: 0x0400113F RID: 4415
			Star,
			// Token: 0x04001140 RID: 4416
			QMark,
			// Token: 0x04001141 RID: 4417
			Plus,
			// Token: 0x04001142 RID: 4418
			PCDATA,
			// Token: 0x04001143 RID: 4419
			Comma,
			// Token: 0x04001144 RID: 4420
			ANY,
			// Token: 0x04001145 RID: 4421
			EMPTY,
			// Token: 0x04001146 RID: 4422
			IGNORE,
			// Token: 0x04001147 RID: 4423
			INCLUDE
		}

		// Token: 0x020001EC RID: 492
		private enum ScanningFunction
		{
			// Token: 0x04001149 RID: 4425
			SubsetContent,
			// Token: 0x0400114A RID: 4426
			Name,
			// Token: 0x0400114B RID: 4427
			QName,
			// Token: 0x0400114C RID: 4428
			Nmtoken,
			// Token: 0x0400114D RID: 4429
			Doctype1,
			// Token: 0x0400114E RID: 4430
			Doctype2,
			// Token: 0x0400114F RID: 4431
			Element1,
			// Token: 0x04001150 RID: 4432
			Element2,
			// Token: 0x04001151 RID: 4433
			Element3,
			// Token: 0x04001152 RID: 4434
			Element4,
			// Token: 0x04001153 RID: 4435
			Element5,
			// Token: 0x04001154 RID: 4436
			Element6,
			// Token: 0x04001155 RID: 4437
			Element7,
			// Token: 0x04001156 RID: 4438
			Attlist1,
			// Token: 0x04001157 RID: 4439
			Attlist2,
			// Token: 0x04001158 RID: 4440
			Attlist3,
			// Token: 0x04001159 RID: 4441
			Attlist4,
			// Token: 0x0400115A RID: 4442
			Attlist5,
			// Token: 0x0400115B RID: 4443
			Attlist6,
			// Token: 0x0400115C RID: 4444
			Attlist7,
			// Token: 0x0400115D RID: 4445
			Entity1,
			// Token: 0x0400115E RID: 4446
			Entity2,
			// Token: 0x0400115F RID: 4447
			Entity3,
			// Token: 0x04001160 RID: 4448
			Notation1,
			// Token: 0x04001161 RID: 4449
			CondSection1,
			// Token: 0x04001162 RID: 4450
			CondSection2,
			// Token: 0x04001163 RID: 4451
			CondSection3,
			// Token: 0x04001164 RID: 4452
			Literal,
			// Token: 0x04001165 RID: 4453
			SystemId,
			// Token: 0x04001166 RID: 4454
			PublicId1,
			// Token: 0x04001167 RID: 4455
			PublicId2,
			// Token: 0x04001168 RID: 4456
			ClosingTag,
			// Token: 0x04001169 RID: 4457
			ParamEntitySpace,
			// Token: 0x0400116A RID: 4458
			None
		}

		// Token: 0x020001ED RID: 493
		private enum LiteralType
		{
			// Token: 0x0400116C RID: 4460
			AttributeValue,
			// Token: 0x0400116D RID: 4461
			EntityReplText,
			// Token: 0x0400116E RID: 4462
			SystemOrPublicID
		}

		// Token: 0x020001EE RID: 494
		private class UndeclaredNotation
		{
			// Token: 0x060013D5 RID: 5077 RVA: 0x00076DB4 File Offset: 0x00074FB4
			internal UndeclaredNotation(string name, int lineNo, int linePos)
			{
				this.name = name;
				this.lineNo = lineNo;
				this.linePos = linePos;
				this.next = null;
			}

			// Token: 0x0400116F RID: 4463
			internal string name;

			// Token: 0x04001170 RID: 4464
			internal int lineNo;

			// Token: 0x04001171 RID: 4465
			internal int linePos;

			// Token: 0x04001172 RID: 4466
			internal DtdParser.UndeclaredNotation next;
		}

		// Token: 0x020001EF RID: 495
		private class ParseElementOnlyContent_LocalFrame
		{
			// Token: 0x060013D6 RID: 5078 RVA: 0x00076DD8 File Offset: 0x00074FD8
			public ParseElementOnlyContent_LocalFrame(int startParentEntityIdParam)
			{
				this.startParenEntityId = startParentEntityIdParam;
				this.parsingSchema = DtdParser.Token.None;
			}

			// Token: 0x04001173 RID: 4467
			public int startParenEntityId;

			// Token: 0x04001174 RID: 4468
			public DtdParser.Token parsingSchema;
		}
	}
}
