using System;

namespace System.Diagnostics
{
	/// <summary>Indicates to compilers that a method call or attribute should be ignored unless a specified conditional compilation symbol is defined.</summary>
	// Token: 0x020009AE RID: 2478
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	[Serializable]
	public sealed class ConditionalAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.ConditionalAttribute" /> class.</summary>
		/// <param name="conditionString">A string that specifies the case-sensitive conditional compilation symbol that is associated with the attribute.</param>
		// Token: 0x0600598A RID: 22922 RVA: 0x00132EAB File Offset: 0x001310AB
		public ConditionalAttribute(string conditionString)
		{
			this.ConditionString = conditionString;
		}

		/// <summary>Gets the conditional compilation symbol that is associated with the <see cref="T:System.Diagnostics.ConditionalAttribute" /> attribute.</summary>
		/// <returns>A string that specifies the case-sensitive conditional compilation symbol that is associated with the <see cref="T:System.Diagnostics.ConditionalAttribute" /> attribute.</returns>
		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x0600598B RID: 22923 RVA: 0x00132EBA File Offset: 0x001310BA
		public string ConditionString { get; }
	}
}
