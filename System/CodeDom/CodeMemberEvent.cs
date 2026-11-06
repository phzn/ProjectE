using System;

namespace System.CodeDom
{
	/// <summary>Represents a declaration for an event of a type.</summary>
	// Token: 0x02000316 RID: 790
	[Serializable]
	public class CodeMemberEvent : CodeTypeMember
	{
		/// <summary>Gets or sets the data type of the delegate type that handles the event.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeTypeReference" /> that indicates the delegate type that handles the event.</returns>
		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001916 RID: 6422 RVA: 0x0005FE5C File Offset: 0x0005E05C
		// (set) Token: 0x06001917 RID: 6423 RVA: 0x0005FE86 File Offset: 0x0005E086
		public CodeTypeReference Type
		{
			get
			{
				CodeTypeReference codeTypeReference;
				if ((codeTypeReference = this._type) == null)
				{
					codeTypeReference = (this._type = new CodeTypeReference(""));
				}
				return codeTypeReference;
			}
			set
			{
				this._type = value;
			}
		}

		/// <summary>Gets or sets the privately implemented data type, if any.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeTypeReference" /> that indicates the data type that the event privately implements.</returns>
		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001918 RID: 6424 RVA: 0x0005FE8F File Offset: 0x0005E08F
		// (set) Token: 0x06001919 RID: 6425 RVA: 0x0005FE97 File Offset: 0x0005E097
		public CodeTypeReference PrivateImplementationType { get; set; }

		/// <summary>Gets or sets the data type that the member event implements.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeTypeReferenceCollection" /> that indicates the data type or types that the member event implements.</returns>
		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x0600191A RID: 6426 RVA: 0x0005FEA0 File Offset: 0x0005E0A0
		public CodeTypeReferenceCollection ImplementationTypes
		{
			get
			{
				CodeTypeReferenceCollection codeTypeReferenceCollection;
				if ((codeTypeReferenceCollection = this._implementationTypes) == null)
				{
					codeTypeReferenceCollection = (this._implementationTypes = new CodeTypeReferenceCollection());
				}
				return codeTypeReferenceCollection;
			}
		}

		// Token: 0x04000D98 RID: 3480
		private CodeTypeReference _type;

		// Token: 0x04000D99 RID: 3481
		private CodeTypeReferenceCollection _implementationTypes;
	}
}
