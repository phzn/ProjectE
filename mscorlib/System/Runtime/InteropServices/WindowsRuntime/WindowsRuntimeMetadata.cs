using System;
using System.Collections.Generic;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	/// <summary>Provides an event for resolving reflection-only type requests for types that are provided by Windows Metadata files, and methods for performing the resolution.</summary>
	// Token: 0x0200079A RID: 1946
	[MonoTODO]
	public static class WindowsRuntimeMetadata
	{
		/// <summary>Locates the Windows Metadata files for the specified namespace, given the specified locations to search.</summary>
		/// <param name="namespaceName">The namespace to resolve.</param>
		/// <param name="packageGraphFilePaths">The application paths to search for Windows Metadata files, or <see langword="null" /> to search only for Windows Metadata files from the operating system installation.</param>
		/// <returns>An enumerable list of strings that represent the Windows Metadata files that define <paramref name="namespaceName" />.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The operating system version does not support the Windows Runtime.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="namespaceName" /> is <see langword="null" />.</exception>
		// Token: 0x060044EA RID: 17642 RVA: 0x000479FC File Offset: 0x00045BFC
		public static IEnumerable<string> ResolveNamespace(string namespaceName, IEnumerable<string> packageGraphFilePaths)
		{
			throw new NotImplementedException();
		}

		/// <summary>Locates the Windows Metadata files for the specified namespace, given the specified locations to search.</summary>
		/// <param name="namespaceName">The namespace to resolve.</param>
		/// <param name="windowsSdkFilePath">The path to search for Windows Metadata files provided by the SDK, or <see langword="null" /> to search for Windows Metadata files from the operating system installation.</param>
		/// <param name="packageGraphFilePaths">The application paths to search for Windows Metadata files.</param>
		/// <returns>An enumerable list of strings that represent the Windows Metadata files that define <paramref name="namespaceName" />.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The operating system version does not support the Windows Runtime.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="namespaceName" /> is <see langword="null" />.</exception>
		// Token: 0x060044EB RID: 17643 RVA: 0x000479FC File Offset: 0x00045BFC
		public static IEnumerable<string> ResolveNamespace(string namespaceName, string windowsSdkFilePath, IEnumerable<string> packageGraphFilePaths)
		{
			throw new NotImplementedException();
		}

		/// <summary>Occurs when the resolution of a Windows Metadata file fails in the design environment.</summary>
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060044EC RID: 17644 RVA: 0x000E4F88 File Offset: 0x000E3188
		// (remove) Token: 0x060044ED RID: 17645 RVA: 0x000E4FBC File Offset: 0x000E31BC
		public static event EventHandler<DesignerNamespaceResolveEventArgs> DesignerNamespaceResolve;

		/// <summary>Occurs when the resolution of a Windows Metadata file fails in the reflection-only context.</summary>
		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060044EE RID: 17646 RVA: 0x000E4FF0 File Offset: 0x000E31F0
		// (remove) Token: 0x060044EF RID: 17647 RVA: 0x000E5024 File Offset: 0x000E3224
		public static event EventHandler<NamespaceResolveEventArgs> ReflectionOnlyNamespaceResolve;
	}
}
