using System;
using System.Security.Permissions;

namespace System.Security.Policy
{
	/// <summary>Provides a base class from which all objects to be used as evidence must derive.</summary>
	// Token: 0x0200040D RID: 1037
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	[Serializable]
	public abstract class EvidenceBase
	{
		/// <summary>Creates a new object that is a complete copy of the current instance.</summary>
		/// <returns>A duplicate copy of this evidence object.</returns>
		// Token: 0x06002A74 RID: 10868 RVA: 0x000479FC File Offset: 0x00045BFC
		[SecurityPermission(SecurityAction.Assert, SerializationFormatter = true)]
		public virtual EvidenceBase Clone()
		{
			throw new NotImplementedException();
		}
	}
}
