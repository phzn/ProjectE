using System;

namespace System.CodeDom
{
	/// <summary>Represents a reference to an indexer property of an object.</summary>
	// Token: 0x02000312 RID: 786
	[Serializable]
	public class CodeIndexerExpression : CodeExpression
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.CodeDom.CodeIndexerExpression" /> class.</summary>
		// Token: 0x060018FA RID: 6394 RVA: 0x0005EE88 File Offset: 0x0005D088
		public CodeIndexerExpression()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.CodeDom.CodeIndexerExpression" /> class using the specified target object and index.</summary>
		/// <param name="targetObject">The target object.</param>
		/// <param name="indices">The index or indexes of the indexer expression.</param>
		// Token: 0x060018FB RID: 6395 RVA: 0x0005FCEC File Offset: 0x0005DEEC
		public CodeIndexerExpression(CodeExpression targetObject, params CodeExpression[] indices)
		{
			this.TargetObject = targetObject;
			this.Indices.AddRange(indices);
		}

		/// <summary>Gets or sets the target object that can be indexed.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeExpression" /> that indicates the indexer object.</returns>
		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060018FC RID: 6396 RVA: 0x0005FD07 File Offset: 0x0005DF07
		// (set) Token: 0x060018FD RID: 6397 RVA: 0x0005FD0F File Offset: 0x0005DF0F
		public CodeExpression TargetObject { get; set; }

		/// <summary>Gets the collection of indexes of the indexer expression.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeExpressionCollection" /> that indicates the index or indexes of the indexer expression.</returns>
		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060018FE RID: 6398 RVA: 0x0005FD18 File Offset: 0x0005DF18
		public CodeExpressionCollection Indices
		{
			get
			{
				CodeExpressionCollection codeExpressionCollection;
				if ((codeExpressionCollection = this._indices) == null)
				{
					codeExpressionCollection = (this._indices = new CodeExpressionCollection());
				}
				return codeExpressionCollection;
			}
		}

		// Token: 0x04000D8E RID: 3470
		private CodeExpressionCollection _indices;
	}
}
