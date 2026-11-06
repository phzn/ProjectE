using System;
using System.Collections.ObjectModel;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	/// <summary>Provides data for the <see cref="E:System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMetadata.DesignerNamespaceResolve" /> event.</summary>
	// Token: 0x02000797 RID: 1943
	[ComVisible(false)]
	public class DesignerNamespaceResolveEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.WindowsRuntime.DesignerNamespaceResolveEventArgs" /> class.</summary>
		/// <param name="namespaceName">The name of the namespace to resolve.</param>
		// Token: 0x060044D8 RID: 17624 RVA: 0x000E4EF8 File Offset: 0x000E30F8
		public DesignerNamespaceResolveEventArgs(string namespaceName)
		{
			this.NamespaceName = namespaceName;
			this.ResolvedAssemblyFiles = new Collection<string>();
		}

		/// <summary>Gets the name of the namespace to resolve.</summary>
		/// <returns>The name of the namespace to resolve.</returns>
		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x060044D9 RID: 17625 RVA: 0x000E4F12 File Offset: 0x000E3112
		// (set) Token: 0x060044DA RID: 17626 RVA: 0x000E4F1A File Offset: 0x000E311A
		public string NamespaceName { get; private set; }

		/// <summary>Gets a collection of assembly file paths; when the event handler for the <see cref="E:System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMetadata.DesignerNamespaceResolve" /> event is invoked, the collection is empty, and the event handler is responsible for adding the necessary assembly files.</summary>
		/// <returns>A collection of assembly files that define the requested namespace.</returns>
		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x060044DB RID: 17627 RVA: 0x000E4F23 File Offset: 0x000E3123
		// (set) Token: 0x060044DC RID: 17628 RVA: 0x000E4F2B File Offset: 0x000E312B
		public Collection<string> ResolvedAssemblyFiles { get; private set; }
	}
}
