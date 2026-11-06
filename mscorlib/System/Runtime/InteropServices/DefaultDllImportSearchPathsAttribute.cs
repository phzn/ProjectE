using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Specifies the paths that are used to search for DLLs that provide functions for platform invokes.</summary>
	// Token: 0x02000705 RID: 1797
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Method, AllowMultiple = false)]
	[ComVisible(false)]
	public sealed class DefaultDllImportSearchPathsAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.DefaultDllImportSearchPathsAttribute" /> class, specifying the paths to use when searching for the targets of platform invokes.</summary>
		/// <param name="paths">A bitwise combination of enumeration values that specify the paths that the LoadLibraryEx function searches during platform invokes.</param>
		// Token: 0x0600409C RID: 16540 RVA: 0x000E1248 File Offset: 0x000DF448
		public DefaultDllImportSearchPathsAttribute(DllImportSearchPath paths)
		{
			this._paths = paths;
		}

		/// <summary>Gets a bitwise combination of enumeration values that specify the paths that the LoadLibraryEx function searches during platform invokes.</summary>
		/// <returns>A bitwise combination of enumeration values that specify search paths for platform invokes.</returns>
		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x0600409D RID: 16541 RVA: 0x000E1257 File Offset: 0x000DF457
		public DllImportSearchPath Paths
		{
			get
			{
				return this._paths;
			}
		}

		// Token: 0x04002AD0 RID: 10960
		internal DllImportSearchPath _paths;
	}
}
