using System;

namespace System.CodeDom
{
	/// <summary>Represents a declaration for a method of a type.</summary>
	// Token: 0x02000318 RID: 792
	[Serializable]
	public class CodeMemberMethod : CodeTypeMember
	{
		/// <summary>An event that will be raised the first time the <see cref="P:System.CodeDom.CodeMemberMethod.Parameters" /> collection is accessed.</summary>
		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06001923 RID: 6435 RVA: 0x0005FF58 File Offset: 0x0005E158
		// (remove) Token: 0x06001924 RID: 6436 RVA: 0x0005FF90 File Offset: 0x0005E190
		public event EventHandler PopulateParameters;

		/// <summary>An event that will be raised the first time the <see cref="P:System.CodeDom.CodeMemberMethod.Statements" /> collection is accessed.</summary>
		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06001925 RID: 6437 RVA: 0x0005FFC8 File Offset: 0x0005E1C8
		// (remove) Token: 0x06001926 RID: 6438 RVA: 0x00060000 File Offset: 0x0005E200
		public event EventHandler PopulateStatements;

		/// <summary>An event that will be raised the first time the <see cref="P:System.CodeDom.CodeMemberMethod.ImplementationTypes" /> collection is accessed.</summary>
		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06001927 RID: 6439 RVA: 0x00060038 File Offset: 0x0005E238
		// (remove) Token: 0x06001928 RID: 6440 RVA: 0x00060070 File Offset: 0x0005E270
		public event EventHandler PopulateImplementationTypes;

		/// <summary>Gets or sets the data type of the return value of the method.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeTypeReference" /> that indicates the data type of the value returned by the method.</returns>
		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x000600A8 File Offset: 0x0005E2A8
		// (set) Token: 0x0600192A RID: 6442 RVA: 0x000600DC File Offset: 0x0005E2DC
		public CodeTypeReference ReturnType
		{
			get
			{
				CodeTypeReference codeTypeReference;
				if ((codeTypeReference = this._returnType) == null)
				{
					codeTypeReference = (this._returnType = new CodeTypeReference(typeof(void).FullName));
				}
				return codeTypeReference;
			}
			set
			{
				this._returnType = value;
			}
		}

		/// <summary>Gets the statements within the method.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeStatementCollection" /> that indicates the statements within the method.</returns>
		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x0600192B RID: 6443 RVA: 0x000600E5 File Offset: 0x0005E2E5
		public CodeStatementCollection Statements
		{
			get
			{
				if ((this._populated & 2) == 0)
				{
					this._populated |= 2;
					EventHandler populateStatements = this.PopulateStatements;
					if (populateStatements != null)
					{
						populateStatements(this, EventArgs.Empty);
					}
				}
				return this._statements;
			}
		}

		/// <summary>Gets the parameter declarations for the method.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeParameterDeclarationExpressionCollection" /> that indicates the method parameters.</returns>
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x0600192C RID: 6444 RVA: 0x0006011C File Offset: 0x0005E31C
		public CodeParameterDeclarationExpressionCollection Parameters
		{
			get
			{
				if ((this._populated & 1) == 0)
				{
					this._populated |= 1;
					EventHandler populateParameters = this.PopulateParameters;
					if (populateParameters != null)
					{
						populateParameters(this, EventArgs.Empty);
					}
				}
				return this._parameters;
			}
		}

		/// <summary>Gets or sets the data type of the interface this method, if private, implements a method of, if any.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeTypeReference" /> that indicates the data type of the interface with the method that the private method whose declaration is represented by this <see cref="T:System.CodeDom.CodeMemberMethod" /> implements.</returns>
		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x0600192D RID: 6445 RVA: 0x00060153 File Offset: 0x0005E353
		// (set) Token: 0x0600192E RID: 6446 RVA: 0x0006015B File Offset: 0x0005E35B
		public CodeTypeReference PrivateImplementationType { get; set; }

		/// <summary>Gets the data types of the interfaces implemented by this method, unless it is a private method implementation, which is indicated by the <see cref="P:System.CodeDom.CodeMemberMethod.PrivateImplementationType" /> property.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeTypeReferenceCollection" /> that indicates the interfaces implemented by this method.</returns>
		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x0600192F RID: 6447 RVA: 0x00060164 File Offset: 0x0005E364
		public CodeTypeReferenceCollection ImplementationTypes
		{
			get
			{
				if (this._implementationTypes == null)
				{
					this._implementationTypes = new CodeTypeReferenceCollection();
				}
				if ((this._populated & 4) == 0)
				{
					this._populated |= 4;
					EventHandler populateImplementationTypes = this.PopulateImplementationTypes;
					if (populateImplementationTypes != null)
					{
						populateImplementationTypes(this, EventArgs.Empty);
					}
				}
				return this._implementationTypes;
			}
		}

		/// <summary>Gets the custom attributes of the return type of the method.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeAttributeDeclarationCollection" /> that indicates the custom attributes.</returns>
		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001930 RID: 6448 RVA: 0x000601BC File Offset: 0x0005E3BC
		public CodeAttributeDeclarationCollection ReturnTypeCustomAttributes
		{
			get
			{
				CodeAttributeDeclarationCollection codeAttributeDeclarationCollection;
				if ((codeAttributeDeclarationCollection = this._returnAttributes) == null)
				{
					codeAttributeDeclarationCollection = (this._returnAttributes = new CodeAttributeDeclarationCollection());
				}
				return codeAttributeDeclarationCollection;
			}
		}

		/// <summary>Gets the type parameters for the current generic method.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeTypeParameterCollection" /> that contains the type parameters for the generic method.</returns>
		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001931 RID: 6449 RVA: 0x000601E4 File Offset: 0x0005E3E4
		public CodeTypeParameterCollection TypeParameters
		{
			get
			{
				CodeTypeParameterCollection codeTypeParameterCollection;
				if ((codeTypeParameterCollection = this._typeParameters) == null)
				{
					codeTypeParameterCollection = (this._typeParameters = new CodeTypeParameterCollection());
				}
				return codeTypeParameterCollection;
			}
		}

		// Token: 0x04000D9D RID: 3485
		private readonly CodeParameterDeclarationExpressionCollection _parameters = new CodeParameterDeclarationExpressionCollection();

		// Token: 0x04000D9E RID: 3486
		private readonly CodeStatementCollection _statements = new CodeStatementCollection();

		// Token: 0x04000D9F RID: 3487
		private CodeTypeReference _returnType;

		// Token: 0x04000DA0 RID: 3488
		private CodeTypeReferenceCollection _implementationTypes;

		// Token: 0x04000DA1 RID: 3489
		private CodeAttributeDeclarationCollection _returnAttributes;

		// Token: 0x04000DA2 RID: 3490
		private CodeTypeParameterCollection _typeParameters;

		// Token: 0x04000DA3 RID: 3491
		private int _populated;

		// Token: 0x04000DA4 RID: 3492
		private const int ParametersCollection = 1;

		// Token: 0x04000DA5 RID: 3493
		private const int StatementsCollection = 2;

		// Token: 0x04000DA6 RID: 3494
		private const int ImplTypesCollection = 4;
	}
}
