using System;

namespace System.CodeDom
{
	/// <summary>Represents an expression that raises an event.</summary>
	// Token: 0x02000307 RID: 775
	[Serializable]
	public class CodeDelegateInvokeExpression : CodeExpression
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.CodeDom.CodeDelegateInvokeExpression" /> class.</summary>
		// Token: 0x060018BD RID: 6333 RVA: 0x0005FA26 File Offset: 0x0005DC26
		public CodeDelegateInvokeExpression()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.CodeDom.CodeDelegateInvokeExpression" /> class using the specified target object.</summary>
		/// <param name="targetObject">A <see cref="T:System.CodeDom.CodeExpression" /> that indicates the target object.</param>
		// Token: 0x060018BE RID: 6334 RVA: 0x0005FA39 File Offset: 0x0005DC39
		public CodeDelegateInvokeExpression(CodeExpression targetObject)
		{
			this.TargetObject = targetObject;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.CodeDom.CodeDelegateInvokeExpression" /> class using the specified target object and parameters.</summary>
		/// <param name="targetObject">A <see cref="T:System.CodeDom.CodeExpression" /> that indicates the target object.</param>
		/// <param name="parameters">An array of <see cref="T:System.CodeDom.CodeExpression" /> objects that indicate the parameters.</param>
		// Token: 0x060018BF RID: 6335 RVA: 0x0005FA53 File Offset: 0x0005DC53
		public CodeDelegateInvokeExpression(CodeExpression targetObject, params CodeExpression[] parameters)
		{
			this.TargetObject = targetObject;
			this.Parameters.AddRange(parameters);
		}

		/// <summary>Gets or sets the event to invoke.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeExpression" /> that indicates the event to invoke.</returns>
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060018C0 RID: 6336 RVA: 0x0005FA79 File Offset: 0x0005DC79
		// (set) Token: 0x060018C1 RID: 6337 RVA: 0x0005FA81 File Offset: 0x0005DC81
		public CodeExpression TargetObject { get; set; }

		/// <summary>Gets or sets the parameters to pass to the event handling methods attached to the event.</summary>
		/// <returns>A <see cref="T:System.CodeDom.CodeExpression" /> that indicates the parameters to pass to the event handling methods attached to the event.</returns>
		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060018C2 RID: 6338 RVA: 0x0005FA8A File Offset: 0x0005DC8A
		public CodeExpressionCollection Parameters { get; } = new CodeExpressionCollection();
	}
}
