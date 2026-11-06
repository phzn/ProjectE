using System;

namespace System.Runtime.ConstrainedExecution
{
	/// <summary>Defines a contract for reliability between the author of some code, and the developers who have a dependency on that code.</summary>
	// Token: 0x020007D5 RID: 2005
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Interface, Inherited = false)]
	public sealed class ReliabilityContractAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.ConstrainedExecution.ReliabilityContractAttribute" /> class with the specified <see cref="T:System.Runtime.ConstrainedExecution.Consistency" /> guarantee and <see cref="T:System.Runtime.ConstrainedExecution.Cer" /> value.</summary>
		/// <param name="consistencyGuarantee">One of the <see cref="T:System.Runtime.ConstrainedExecution.Consistency" /> values.</param>
		/// <param name="cer">One of the <see cref="T:System.Runtime.ConstrainedExecution.Cer" /> values.</param>
		// Token: 0x060045B2 RID: 17842 RVA: 0x000E5107 File Offset: 0x000E3307
		public ReliabilityContractAttribute(Consistency consistencyGuarantee, Cer cer)
		{
			this.ConsistencyGuarantee = consistencyGuarantee;
			this.Cer = cer;
		}

		/// <summary>Gets the value of the <see cref="T:System.Runtime.ConstrainedExecution.Consistency" /> reliability contract.</summary>
		/// <returns>One of the <see cref="T:System.Runtime.ConstrainedExecution.Consistency" /> values.</returns>
		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x060045B3 RID: 17843 RVA: 0x000E511D File Offset: 0x000E331D
		public Consistency ConsistencyGuarantee { get; }

		/// <summary>Gets the value that determines the behavior of a method, type, or assembly when called under a Constrained Execution Region (CER).</summary>
		/// <returns>One of the <see cref="T:System.Runtime.ConstrainedExecution.Cer" /> values.</returns>
		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x060045B4 RID: 17844 RVA: 0x000E5125 File Offset: 0x000E3325
		public Cer Cer { get; }
	}
}
