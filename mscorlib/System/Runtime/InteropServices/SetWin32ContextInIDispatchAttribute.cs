using System;

namespace System.Runtime.InteropServices
{
	/// <summary>This attribute has been deprecated.</summary>
	// Token: 0x02000712 RID: 1810
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	[Obsolete("This attribute has been deprecated.  Application Domains no longer respect Activation Context boundaries in IDispatch calls.", false)]
	public sealed class SetWin32ContextInIDispatchAttribute : Attribute
	{
	}
}
