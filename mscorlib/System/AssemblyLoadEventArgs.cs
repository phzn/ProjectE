using System;
using System.Reflection;

namespace System
{
	/// <summary>Provides data for the <see cref="E:System.AppDomain.AssemblyLoad" /> event.</summary>
	// Token: 0x020000FA RID: 250
	public class AssemblyLoadEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.AssemblyLoadEventArgs" /> class using the specified <see cref="T:System.Reflection.Assembly" />.</summary>
		/// <param name="loadedAssembly">An instance that represents the currently loaded assembly.</param>
		// Token: 0x06000747 RID: 1863 RVA: 0x00021718 File Offset: 0x0001F918
		public AssemblyLoadEventArgs(Assembly loadedAssembly)
		{
			this.LoadedAssembly = loadedAssembly;
		}

		/// <summary>Gets an <see cref="T:System.Reflection.Assembly" /> that represents the currently loaded assembly.</summary>
		/// <returns>An instance of <see cref="T:System.Reflection.Assembly" /> that represents the currently loaded assembly.</returns>
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x00021727 File Offset: 0x0001F927
		public Assembly LoadedAssembly { get; }
	}
}
