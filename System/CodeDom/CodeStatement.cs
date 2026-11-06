using System;

namespace System.CodeDom
{
	/// <summary>Represents the <see langword="abstract" /> base class from which all code statements derive.</summary>
	// Token: 0x0200032D RID: 813
	[Serializable]
	public class CodeStatement : CodeObject
	{
		/// <summary>Gets or sets the line on which the code statement occurs.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeLinePragma" /> object that indicates the context of the code statement.</returns>
		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x00060D68 File Offset: 0x0005EF68
		// (set) Token: 0x060019CE RID: 6606 RVA: 0x00060D70 File Offset: 0x0005EF70
		public CodeLinePragma LinePragma { get; set; }

		/// <summary>Gets a <see cref="T:System.CodeDom.CodeDirectiveCollection" /> object that contains start directives.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeDirectiveCollection" /> object containing start directives.</returns>
		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x00060D7C File Offset: 0x0005EF7C
		public CodeDirectiveCollection StartDirectives
		{
			get
			{
				CodeDirectiveCollection codeDirectiveCollection;
				if ((codeDirectiveCollection = this._startDirectives) == null)
				{
					codeDirectiveCollection = (this._startDirectives = new CodeDirectiveCollection());
				}
				return codeDirectiveCollection;
			}
		}

		/// <summary>Gets a <see cref="T:System.CodeDom.CodeDirectiveCollection" /> object that contains end directives.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeDirectiveCollection" /> object containing end directives.</returns>
		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x00060DA4 File Offset: 0x0005EFA4
		public CodeDirectiveCollection EndDirectives
		{
			get
			{
				CodeDirectiveCollection codeDirectiveCollection;
				if ((codeDirectiveCollection = this._endDirectives) == null)
				{
					codeDirectiveCollection = (this._endDirectives = new CodeDirectiveCollection());
				}
				return codeDirectiveCollection;
			}
		}

		// Token: 0x04000DDB RID: 3547
		private CodeDirectiveCollection _startDirectives;

		// Token: 0x04000DDC RID: 3548
		private CodeDirectiveCollection _endDirectives;
	}
}
