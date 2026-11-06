using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	/// <summary>Defines the interface for unwrapping marshal-by-value objects from indirection.</summary>
	// Token: 0x0200055E RID: 1374
	[Guid("C460E2B4-E199-412a-8456-84DC3E4838C3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	public interface IObjectHandle
	{
		/// <summary>Unwraps the object.</summary>
		/// <returns>The unwrapped object.</returns>
		// Token: 0x060035E4 RID: 13796
		object Unwrap();
	}
}
