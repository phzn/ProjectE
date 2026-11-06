using System;

namespace System.Runtime
{
	/// <summary>Indicates that the .NET Framework class library method to which this attribute is applied is unlikely to be affected by servicing releases, and therefore is eligible to be inlined across Native Image Generator (NGen) images.</summary>
	// Token: 0x0200054D RID: 1357
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class TargetedPatchingOptOutAttribute : Attribute
	{
		/// <summary>Gets the reason why the method to which this attribute is applied is considered to be eligible for inlining across Native Image Generator (NGen) images.</summary>
		/// <returns>The reason why the method is considered to be eligible for inlining across NGen images.</returns>
		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x060035A9 RID: 13737 RVA: 0x000C1F98 File Offset: 0x000C0198
		public string Reason { get; }

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.TargetedPatchingOptOutAttribute" /> class.</summary>
		/// <param name="reason">The reason why the method to which the <see cref="T:System.Runtime.TargetedPatchingOptOutAttribute" /> attribute is applied is considered to be eligible for inlining across Native Image Generator (NGen) images.</param>
		// Token: 0x060035AA RID: 13738 RVA: 0x000C1FA0 File Offset: 0x000C01A0
		public TargetedPatchingOptOutAttribute(string reason)
		{
			this.Reason = reason;
		}
	}
}
