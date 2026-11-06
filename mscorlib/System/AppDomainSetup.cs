using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Policy;
using Mono.Security;
using Unity;

namespace System
{
	/// <summary>Represents assembly binding information that can be added to an instance of <see cref="T:System.AppDomain" />.</summary>
	// Token: 0x02000227 RID: 551
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class AppDomainSetup : IAppDomainSetup
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.AppDomainSetup" /> class.</summary>
		// Token: 0x0600188C RID: 6284 RVA: 0x0000259F File Offset: 0x0000079F
		public AppDomainSetup()
		{
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x0005D9B0 File Offset: 0x0005BBB0
		internal AppDomainSetup(AppDomainSetup setup)
		{
			this.application_base = setup.application_base;
			this.application_name = setup.application_name;
			this.cache_path = setup.cache_path;
			this.configuration_file = setup.configuration_file;
			this.dynamic_base = setup.dynamic_base;
			this.license_file = setup.license_file;
			this.private_bin_path = setup.private_bin_path;
			this.private_bin_path_probe = setup.private_bin_path_probe;
			this.shadow_copy_directories = setup.shadow_copy_directories;
			this.shadow_copy_files = setup.shadow_copy_files;
			this.publisher_policy = setup.publisher_policy;
			this.path_changed = setup.path_changed;
			this.loader_optimization = setup.loader_optimization;
			this.disallow_binding_redirects = setup.disallow_binding_redirects;
			this.disallow_code_downloads = setup.disallow_code_downloads;
			this._activationArguments = setup._activationArguments;
			this.domain_initializer = setup.domain_initializer;
			this.application_trust = setup.application_trust;
			this.domain_initializer_args = setup.domain_initializer_args;
			this.disallow_appbase_probe = setup.disallow_appbase_probe;
			this.configuration_bytes = setup.configuration_bytes;
			this.manager_assembly = setup.manager_assembly;
			this.manager_type = setup.manager_type;
			this.partial_visible_assemblies = setup.partial_visible_assemblies;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.AppDomainSetup" /> class with the specified activation arguments required for manifest-based activation of an application domain.</summary>
		/// <param name="activationArguments">An object that specifies information required for the manifest-based activation of a new application domain.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="activationArguments" /> is <see langword="null" />.</exception>
		// Token: 0x0600188E RID: 6286 RVA: 0x0005DAE3 File Offset: 0x0005BCE3
		public AppDomainSetup(ActivationArguments activationArguments)
		{
			this._activationArguments = activationArguments;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.AppDomainSetup" /> class with the specified activation context to use for manifest-based activation of an application domain.</summary>
		/// <param name="activationContext">The activation context to be used for an application domain.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="activationContext" /> is <see langword="null" />.</exception>
		// Token: 0x0600188F RID: 6287 RVA: 0x0005DAF2 File Offset: 0x0005BCF2
		public AppDomainSetup(ActivationContext activationContext)
		{
			this._activationArguments = new ActivationArguments(activationContext);
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x0005DB08 File Offset: 0x0005BD08
		private static string GetAppBase(string appBase)
		{
			if (appBase == null)
			{
				return null;
			}
			if (appBase == "")
			{
				appBase = Path.DirectorySeparatorChar.ToString();
			}
			if (appBase.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
			{
				appBase = new Uri(appBase).LocalPath;
				if (Path.DirectorySeparatorChar != '/')
				{
					appBase = appBase.Replace('/', Path.DirectorySeparatorChar);
				}
			}
			appBase = Path.GetFullPath(appBase);
			if (Path.DirectorySeparatorChar != '/')
			{
				bool flag = appBase.StartsWith("\\\\?\\", StringComparison.Ordinal);
				if (appBase.IndexOf(':', flag ? 6 : 2) != -1)
				{
					throw new NotSupportedException("The given path's format is not supported.");
				}
			}
			string directoryName = Path.GetDirectoryName(appBase);
			if (directoryName != null && directoryName.LastIndexOfAny(Path.GetInvalidPathChars()) >= 0)
			{
				throw new ArgumentException(string.Format(Locale.GetText("Invalid path characters in path: '{0}'"), appBase), "appBase");
			}
			string fileName = Path.GetFileName(appBase);
			if (fileName != null && fileName.LastIndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
			{
				throw new ArgumentException(string.Format(Locale.GetText("Invalid filename characters in path: '{0}'"), appBase), "appBase");
			}
			return appBase;
		}

		/// <summary>Gets or sets the name of the directory containing the application.</summary>
		/// <returns>The name of the application base directory.</returns>
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06001891 RID: 6289 RVA: 0x0005DC08 File Offset: 0x0005BE08
		// (set) Token: 0x06001892 RID: 6290 RVA: 0x0005DC15 File Offset: 0x0005BE15
		public string ApplicationBase
		{
			[SecuritySafeCritical]
			get
			{
				return AppDomainSetup.GetAppBase(this.application_base);
			}
			set
			{
				this.application_base = value;
			}
		}

		/// <summary>Gets or sets the name of the application.</summary>
		/// <returns>The name of the application.</returns>
		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06001893 RID: 6291 RVA: 0x0005DC1E File Offset: 0x0005BE1E
		// (set) Token: 0x06001894 RID: 6292 RVA: 0x0005DC26 File Offset: 0x0005BE26
		public string ApplicationName
		{
			get
			{
				return this.application_name;
			}
			set
			{
				this.application_name = value;
			}
		}

		/// <summary>Gets or sets the name of an area specific to the application where files are shadow copied.</summary>
		/// <returns>The fully qualified name of the directory path and file name where files are shadow copied.</returns>
		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06001895 RID: 6293 RVA: 0x0005DC2F File Offset: 0x0005BE2F
		// (set) Token: 0x06001896 RID: 6294 RVA: 0x0005DC37 File Offset: 0x0005BE37
		public string CachePath
		{
			[SecuritySafeCritical]
			get
			{
				return this.cache_path;
			}
			set
			{
				this.cache_path = value;
			}
		}

		/// <summary>Gets or sets the name of the configuration file for an application domain.</summary>
		/// <returns>The name of the configuration file.</returns>
		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06001897 RID: 6295 RVA: 0x0005DC40 File Offset: 0x0005BE40
		// (set) Token: 0x06001898 RID: 6296 RVA: 0x0005DC8F File Offset: 0x0005BE8F
		public string ConfigurationFile
		{
			[SecuritySafeCritical]
			get
			{
				if (this.configuration_file == null)
				{
					return null;
				}
				if (Path.IsPathRooted(this.configuration_file))
				{
					return this.configuration_file;
				}
				if (this.ApplicationBase == null)
				{
					throw new MemberAccessException("The ApplicationBase must be set before retrieving this property.");
				}
				return Path.Combine(this.ApplicationBase, this.configuration_file);
			}
			set
			{
				this.configuration_file = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the &lt;publisherPolicy&gt; section of the configuration file is applied to an application domain.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see langword="&lt;publisherPolicy&gt;" /> section of the configuration file for an application domain is ignored; <see langword="false" /> if the declared publisher policy is honored.</returns>
		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06001899 RID: 6297 RVA: 0x0005DC98 File Offset: 0x0005BE98
		// (set) Token: 0x0600189A RID: 6298 RVA: 0x0005DCA0 File Offset: 0x0005BEA0
		public bool DisallowPublisherPolicy
		{
			get
			{
				return this.publisher_policy;
			}
			set
			{
				this.publisher_policy = value;
			}
		}

		/// <summary>Gets or sets the base directory where the directory for dynamically generated files is located.</summary>
		/// <returns>The directory where the <see cref="P:System.AppDomain.DynamicDirectory" /> is located.  
		///
		///  The return value of this property is different from the value assigned.</returns>
		/// <exception cref="T:System.MemberAccessException">This property cannot be set because the application name on the application domain is <see langword="null" />.</exception>
		// Token: 0x1700028F RID: 655
		// (get) Token: 0x0600189B RID: 6299 RVA: 0x0005DCAC File Offset: 0x0005BEAC
		// (set) Token: 0x0600189C RID: 6300 RVA: 0x0005DCFC File Offset: 0x0005BEFC
		public string DynamicBase
		{
			[SecuritySafeCritical]
			get
			{
				if (this.dynamic_base == null)
				{
					return null;
				}
				if (Path.IsPathRooted(this.dynamic_base))
				{
					return this.dynamic_base;
				}
				if (this.ApplicationBase == null)
				{
					throw new MemberAccessException("The ApplicationBase must be set before retrieving this property.");
				}
				return Path.Combine(this.ApplicationBase, this.dynamic_base);
			}
			[SecuritySafeCritical]
			set
			{
				if (this.application_name == null)
				{
					throw new MemberAccessException("ApplicationName must be set before the DynamicBase can be set.");
				}
				this.dynamic_base = Path.Combine(value, ((uint)this.application_name.GetHashCode()).ToString("x"));
			}
		}

		/// <summary>Gets or sets the location of the license file associated with this domain.</summary>
		/// <returns>The location and name of the license file.</returns>
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600189D RID: 6301 RVA: 0x0005DD40 File Offset: 0x0005BF40
		// (set) Token: 0x0600189E RID: 6302 RVA: 0x0005DD48 File Offset: 0x0005BF48
		public string LicenseFile
		{
			[SecuritySafeCritical]
			get
			{
				return this.license_file;
			}
			set
			{
				this.license_file = value;
			}
		}

		/// <summary>Specifies the optimization policy used to load an executable.</summary>
		/// <returns>An enumerated constant that is used with the <see cref="T:System.LoaderOptimizationAttribute" />.</returns>
		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600189F RID: 6303 RVA: 0x0005DD51 File Offset: 0x0005BF51
		// (set) Token: 0x060018A0 RID: 6304 RVA: 0x0005DD59 File Offset: 0x0005BF59
		[MonoLimitation("In Mono this is controlled by the --share-code flag")]
		public LoaderOptimization LoaderOptimization
		{
			get
			{
				return this.loader_optimization;
			}
			set
			{
				this.loader_optimization = value;
			}
		}

		/// <summary>Gets or sets the display name of the assembly that provides the type of the application domain manager for application domains created using this <see cref="T:System.AppDomainSetup" /> object.</summary>
		/// <returns>The display name of the assembly that provides the <see cref="T:System.Type" /> of the application domain manager.</returns>
		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060018A1 RID: 6305 RVA: 0x0005DD62 File Offset: 0x0005BF62
		// (set) Token: 0x060018A2 RID: 6306 RVA: 0x0005DD6A File Offset: 0x0005BF6A
		public string AppDomainManagerAssembly
		{
			get
			{
				return this.manager_assembly;
			}
			set
			{
				this.manager_assembly = value;
			}
		}

		/// <summary>Gets or sets the full name of the type that provides the application domain manager for application domains created using this <see cref="T:System.AppDomainSetup" /> object.</summary>
		/// <returns>The full name of the type, including the namespace.</returns>
		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060018A3 RID: 6307 RVA: 0x0005DD73 File Offset: 0x0005BF73
		// (set) Token: 0x060018A4 RID: 6308 RVA: 0x0005DD7B File Offset: 0x0005BF7B
		public string AppDomainManagerType
		{
			get
			{
				return this.manager_type;
			}
			set
			{
				this.manager_type = value;
			}
		}

		/// <summary>Gets or sets a list of assemblies marked with the <see cref="F:System.Security.PartialTrustVisibilityLevel.NotVisibleByDefault" /> flag that are made visible to partial-trust code running in a sandboxed application domain.</summary>
		/// <returns>An array of partial assembly names, where each partial name consists of the simple assembly name and the public key.</returns>
		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060018A5 RID: 6309 RVA: 0x0005DD84 File Offset: 0x0005BF84
		// (set) Token: 0x060018A6 RID: 6310 RVA: 0x0005DD8C File Offset: 0x0005BF8C
		public string[] PartialTrustVisibleAssemblies
		{
			get
			{
				return this.partial_visible_assemblies;
			}
			set
			{
				if (value != null)
				{
					this.partial_visible_assemblies = (string[])value.Clone();
					Array.Sort<string>(this.partial_visible_assemblies, StringComparer.OrdinalIgnoreCase);
					return;
				}
				this.partial_visible_assemblies = null;
			}
		}

		/// <summary>Gets or sets the list of directories under the application base directory that are probed for private assemblies.</summary>
		/// <returns>A list of directory names separated by semicolons.</returns>
		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060018A7 RID: 6311 RVA: 0x0005DDBA File Offset: 0x0005BFBA
		// (set) Token: 0x060018A8 RID: 6312 RVA: 0x0005DDC2 File Offset: 0x0005BFC2
		public string PrivateBinPath
		{
			[SecuritySafeCritical]
			get
			{
				return this.private_bin_path;
			}
			set
			{
				this.private_bin_path = value;
				this.path_changed = true;
			}
		}

		/// <summary>Gets or sets a string value that includes or excludes <see cref="P:System.AppDomainSetup.ApplicationBase" /> from the search path for the application, and searches only <see cref="P:System.AppDomainSetup.PrivateBinPath" />.</summary>
		/// <returns>A null reference (<see langword="Nothing" /> in Visual Basic) to include the application base path when searching for assemblies; any non-null string value to exclude the path. The default value is <see langword="null" />.</returns>
		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060018A9 RID: 6313 RVA: 0x0005DDD2 File Offset: 0x0005BFD2
		// (set) Token: 0x060018AA RID: 6314 RVA: 0x0005DDDA File Offset: 0x0005BFDA
		public string PrivateBinPathProbe
		{
			get
			{
				return this.private_bin_path_probe;
			}
			set
			{
				this.private_bin_path_probe = value;
				this.path_changed = true;
			}
		}

		/// <summary>Gets or sets the names of the directories containing assemblies to be shadow copied.</summary>
		/// <returns>A list of directory names separated by semicolons.</returns>
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x0005DDEA File Offset: 0x0005BFEA
		// (set) Token: 0x060018AC RID: 6316 RVA: 0x0005DDF2 File Offset: 0x0005BFF2
		public string ShadowCopyDirectories
		{
			[SecuritySafeCritical]
			get
			{
				return this.shadow_copy_directories;
			}
			set
			{
				this.shadow_copy_directories = value;
			}
		}

		/// <summary>Gets or sets a string that indicates whether shadow copying is turned on or off.</summary>
		/// <returns>The string value "true" to indicate that shadow copying is turned on; or "false" to indicate that shadow copying is turned off.</returns>
		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x0005DDFB File Offset: 0x0005BFFB
		// (set) Token: 0x060018AE RID: 6318 RVA: 0x0005DE03 File Offset: 0x0005C003
		public string ShadowCopyFiles
		{
			get
			{
				return this.shadow_copy_files;
			}
			set
			{
				this.shadow_copy_files = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether an application domain allows assembly binding redirection.</summary>
		/// <returns>
		///   <see langword="true" /> if redirection of assemblies is not allowed; <see langword="false" /> if it is allowed.</returns>
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060018AF RID: 6319 RVA: 0x0005DE0C File Offset: 0x0005C00C
		// (set) Token: 0x060018B0 RID: 6320 RVA: 0x0005DE14 File Offset: 0x0005C014
		public bool DisallowBindingRedirects
		{
			get
			{
				return this.disallow_binding_redirects;
			}
			set
			{
				this.disallow_binding_redirects = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether HTTP download of assemblies is allowed for an application domain.</summary>
		/// <returns>
		///   <see langword="true" /> if HTTP download of assemblies is not allowed; <see langword="false" /> if it is allowed.</returns>
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x0005DE1D File Offset: 0x0005C01D
		// (set) Token: 0x060018B2 RID: 6322 RVA: 0x0005DE25 File Offset: 0x0005C025
		public bool DisallowCodeDownload
		{
			get
			{
				return this.disallow_code_downloads;
			}
			set
			{
				this.disallow_code_downloads = value;
			}
		}

		/// <summary>Gets or sets a string that specifies the target version and profile of the .NET Framework for the application domain, in a format that can be parsed by the <see cref="M:System.Runtime.Versioning.FrameworkName.#ctor(System.String)" /> constructor.</summary>
		/// <returns>The target version and profile of the .NET Framework.</returns>
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x0005DE2E File Offset: 0x0005C02E
		// (set) Token: 0x060018B4 RID: 6324 RVA: 0x0005DE36 File Offset: 0x0005C036
		public string TargetFrameworkName { get; set; }

		/// <summary>Gets or sets data about the activation of an application domain.</summary>
		/// <returns>An object that contains data about the activation of an application domain.</returns>
		/// <exception cref="T:System.InvalidOperationException">The property is set to an <see cref="T:System.Runtime.Hosting.ActivationArguments" /> object whose application identity does not match the application identity of the <see cref="T:System.Security.Policy.ApplicationTrust" /> object returned by the <see cref="P:System.AppDomainSetup.ApplicationTrust" /> property. No exception is thrown if the <see cref="P:System.AppDomainSetup.ApplicationTrust" /> property is <see langword="null" />.</exception>
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x0005DE3F File Offset: 0x0005C03F
		// (set) Token: 0x060018B6 RID: 6326 RVA: 0x0005DE5C File Offset: 0x0005C05C
		public ActivationArguments ActivationArguments
		{
			get
			{
				if (this._activationArguments != null)
				{
					return this._activationArguments;
				}
				this.DeserializeNonPrimitives();
				return this._activationArguments;
			}
			set
			{
				this._activationArguments = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.AppDomainInitializer" /> delegate, which represents a callback method that is invoked when the application domain is initialized.</summary>
		/// <returns>A delegate that represents a callback method that is invoked when the application domain is initialized.</returns>
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060018B7 RID: 6327 RVA: 0x0005DE65 File Offset: 0x0005C065
		// (set) Token: 0x060018B8 RID: 6328 RVA: 0x0005DE82 File Offset: 0x0005C082
		[MonoLimitation("it needs to be invoked within the created domain")]
		public AppDomainInitializer AppDomainInitializer
		{
			get
			{
				if (this.domain_initializer != null)
				{
					return this.domain_initializer;
				}
				this.DeserializeNonPrimitives();
				return this.domain_initializer;
			}
			set
			{
				this.domain_initializer = value;
			}
		}

		/// <summary>Gets or sets the arguments passed to the callback method represented by the <see cref="T:System.AppDomainInitializer" /> delegate. The callback method is invoked when the application domain is initialized.</summary>
		/// <returns>An array of strings that is passed to the callback method represented by the <see cref="T:System.AppDomainInitializer" /> delegate, when the callback method is invoked during <see cref="T:System.AppDomain" /> initialization.</returns>
		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x0005DE8B File Offset: 0x0005C08B
		// (set) Token: 0x060018BA RID: 6330 RVA: 0x0005DE93 File Offset: 0x0005C093
		[MonoLimitation("it needs to be used to invoke the initializer within the created domain")]
		public string[] AppDomainInitializerArguments
		{
			get
			{
				return this.domain_initializer_args;
			}
			set
			{
				this.domain_initializer_args = value;
			}
		}

		/// <summary>Gets or sets an object containing security and trust information.</summary>
		/// <returns>An object that contains security and trust information.</returns>
		/// <exception cref="T:System.InvalidOperationException">The property is set to an <see cref="T:System.Security.Policy.ApplicationTrust" /> object whose application identity does not match the application identity of the <see cref="T:System.Runtime.Hosting.ActivationArguments" /> object returned by the <see cref="P:System.AppDomainSetup.ActivationArguments" /> property. No exception is thrown if the <see cref="P:System.AppDomainSetup.ActivationArguments" /> property is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The property is set to <see langword="null" />.</exception>
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060018BB RID: 6331 RVA: 0x0005DE9C File Offset: 0x0005C09C
		// (set) Token: 0x060018BC RID: 6332 RVA: 0x0005DECC File Offset: 0x0005C0CC
		[MonoNotSupported("This property exists but not considered.")]
		public ApplicationTrust ApplicationTrust
		{
			get
			{
				if (this.application_trust != null)
				{
					return this.application_trust;
				}
				this.DeserializeNonPrimitives();
				if (this.application_trust == null)
				{
					this.application_trust = new ApplicationTrust();
				}
				return this.application_trust;
			}
			set
			{
				this.application_trust = value;
			}
		}

		/// <summary>Specifies whether the application base path and private binary path are probed when searching for assemblies to load.</summary>
		/// <returns>
		///   <see langword="true" /> if probing is not allowed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060018BD RID: 6333 RVA: 0x0005DED5 File Offset: 0x0005C0D5
		// (set) Token: 0x060018BE RID: 6334 RVA: 0x0005DEDD File Offset: 0x0005C0DD
		[MonoNotSupported("This property exists but not considered.")]
		public bool DisallowApplicationBaseProbing
		{
			get
			{
				return this.disallow_appbase_probe;
			}
			set
			{
				this.disallow_appbase_probe = value;
			}
		}

		/// <summary>Returns the XML configuration information set by the <see cref="M:System.AppDomainSetup.SetConfigurationBytes(System.Byte[])" /> method, which overrides the application's XML configuration information.</summary>
		/// <returns>An array that contains the XML configuration information that was set by the <see cref="M:System.AppDomainSetup.SetConfigurationBytes(System.Byte[])" /> method, or <see langword="null" /> if the <see cref="M:System.AppDomainSetup.SetConfigurationBytes(System.Byte[])" /> method has not been called.</returns>
		// Token: 0x060018BF RID: 6335 RVA: 0x0005DEE6 File Offset: 0x0005C0E6
		[MonoNotSupported("This method exists but not considered.")]
		public byte[] GetConfigurationBytes()
		{
			if (this.configuration_bytes == null)
			{
				return null;
			}
			return this.configuration_bytes.Clone() as byte[];
		}

		/// <summary>Provides XML configuration information for the application domain, replacing the application's XML configuration information.</summary>
		/// <param name="value">An array that contains the XML configuration information to be used for the application domain.</param>
		// Token: 0x060018C0 RID: 6336 RVA: 0x0005DF02 File Offset: 0x0005C102
		[MonoNotSupported("This method exists but not considered.")]
		public void SetConfigurationBytes(byte[] value)
		{
			this.configuration_bytes = value;
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x0005DF0C File Offset: 0x0005C10C
		private void DeserializeNonPrimitives()
		{
			lock (this)
			{
				if (this.serialized_non_primitives != null)
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					MemoryStream memoryStream = new MemoryStream(this.serialized_non_primitives);
					object[] array = (object[])binaryFormatter.Deserialize(memoryStream);
					this._activationArguments = (ActivationArguments)array[0];
					this.domain_initializer = (AppDomainInitializer)array[1];
					this.application_trust = (ApplicationTrust)array[2];
					this.serialized_non_primitives = null;
				}
			}
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x0005DF9C File Offset: 0x0005C19C
		internal void SerializeNonPrimitives()
		{
			object[] array = new object[] { this._activationArguments, this.domain_initializer, this.application_trust };
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, array);
			this.serialized_non_primitives = memoryStream.ToArray();
		}

		/// <summary>Sets the specified switches, making the application domain compatible with previous versions of the .NET Framework for the specified issues.</summary>
		/// <param name="switches">An enumerable set of string values that specify compatibility switches, or <see langword="null" /> to erase the existing compatibility switches.</param>
		// Token: 0x060018C3 RID: 6339 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[MonoTODO("not implemented, does not throw because it's used in testing moonlight")]
		public void SetCompatibilitySwitches(IEnumerable<string> switches)
		{
		}

		/// <summary>Gets or sets a value that indicates whether interface caching is disabled for interop calls in the application domain, so that a QueryInterface is performed on each call.</summary>
		/// <returns>
		///   <see langword="true" /> if interface caching is disabled for interop calls in application domains created with the current <see cref="T:System.AppDomainSetup" /> object; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060018C4 RID: 6340 RVA: 0x0005DFEC File Offset: 0x0005C1EC
		// (set) Token: 0x060018C5 RID: 6341 RVA: 0x000173AD File Offset: 0x000155AD
		public bool SandboxInterop
		{
			get
			{
				ThrowStub.ThrowNotSupportedException();
				return default(bool);
			}
			set
			{
				ThrowStub.ThrowNotSupportedException();
			}
		}

		/// <summary>Provides the common language runtime with an alternate implementation of a string comparison function.</summary>
		/// <param name="functionName">The name of the string comparison function to override.</param>
		/// <param name="functionVersion">The function version. For .NET Framework 4.5, its value must be 1 or greater.</param>
		/// <param name="functionPointer">A pointer to the function that overrides <paramref name="functionName" />.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="functionName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="functionVersion" /> is not 1 or greater.  
		/// -or-  
		/// <paramref name="functionPointer" /> is <see cref="F:System.IntPtr.Zero" />.</exception>
		// Token: 0x060018C6 RID: 6342 RVA: 0x000173AD File Offset: 0x000155AD
		[SecurityCritical]
		public void SetNativeFunction(string functionName, int functionVersion, IntPtr functionPointer)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x040016BB RID: 5819
		private string application_base;

		// Token: 0x040016BC RID: 5820
		private string application_name;

		// Token: 0x040016BD RID: 5821
		private string cache_path;

		// Token: 0x040016BE RID: 5822
		private string configuration_file;

		// Token: 0x040016BF RID: 5823
		private string dynamic_base;

		// Token: 0x040016C0 RID: 5824
		private string license_file;

		// Token: 0x040016C1 RID: 5825
		private string private_bin_path;

		// Token: 0x040016C2 RID: 5826
		private string private_bin_path_probe;

		// Token: 0x040016C3 RID: 5827
		private string shadow_copy_directories;

		// Token: 0x040016C4 RID: 5828
		private string shadow_copy_files;

		// Token: 0x040016C5 RID: 5829
		private bool publisher_policy;

		// Token: 0x040016C6 RID: 5830
		private bool path_changed;

		// Token: 0x040016C7 RID: 5831
		private LoaderOptimization loader_optimization;

		// Token: 0x040016C8 RID: 5832
		private bool disallow_binding_redirects;

		// Token: 0x040016C9 RID: 5833
		private bool disallow_code_downloads;

		// Token: 0x040016CA RID: 5834
		private ActivationArguments _activationArguments;

		// Token: 0x040016CB RID: 5835
		private AppDomainInitializer domain_initializer;

		// Token: 0x040016CC RID: 5836
		[NonSerialized]
		private ApplicationTrust application_trust;

		// Token: 0x040016CD RID: 5837
		private string[] domain_initializer_args;

		// Token: 0x040016CE RID: 5838
		private bool disallow_appbase_probe;

		// Token: 0x040016CF RID: 5839
		private byte[] configuration_bytes;

		// Token: 0x040016D0 RID: 5840
		private byte[] serialized_non_primitives;

		// Token: 0x040016D1 RID: 5841
		private string manager_assembly;

		// Token: 0x040016D2 RID: 5842
		private string manager_type;

		// Token: 0x040016D3 RID: 5843
		private string[] partial_visible_assemblies;
	}
}
