using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	/// <summary>Provides a strongly typed collection of Cryptography Next Generation (CNG) properties.</summary>
	// Token: 0x02000045 RID: 69
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class CngPropertyCollection : Collection<CngProperty>
	{
	}
}
