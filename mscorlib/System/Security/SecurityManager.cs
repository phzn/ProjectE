using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;

namespace System.Security
{
	/// <summary>Provides the main access point for classes interacting with the security system. This class cannot be inherited.</summary>
	// Token: 0x020003EB RID: 1003
	[ComVisible(true)]
	public static class SecurityManager
	{
		/// <summary>Gets or sets a value indicating whether code must have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.Execution" /> in order to execute.</summary>
		/// <returns>
		///   <see langword="true" /> if code must have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.Execution" /> in order to execute; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />.</exception>
		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x0600295C RID: 10588 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		// (set) Token: 0x0600295D RID: 10589 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[Obsolete]
		public static bool CheckExecutionRights
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets a value indicating whether security is enabled.</summary>
		/// <returns>
		///   <see langword="true" /> if security is enabled; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />.</exception>
		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x0600295E RID: 10590
		// (set) Token: 0x0600295F RID: 10591
		[Obsolete("The security manager cannot be turned off on MS runtime")]
		public static extern bool SecurityEnabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x000040F7 File Offset: 0x000022F7
		internal static bool CheckElevatedPermissions()
		{
			return true;
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[Conditional("ENABLE_SANDBOX")]
		internal static void EnsureElevatedPermissions()
		{
		}

		/// <summary>Gets the granted zone identity and URL identity permission sets for the current assembly.</summary>
		/// <param name="zone">An output parameter that contains an <see cref="T:System.Collections.ArrayList" /> of granted <see cref="P:System.Security.Permissions.ZoneIdentityPermissionAttribute.Zone" /> objects.</param>
		/// <param name="origin">An output parameter that contains an <see cref="T:System.Collections.ArrayList" /> of granted <see cref="T:System.Security.Permissions.UrlIdentityPermission" /> objects.</param>
		/// <exception cref="T:System.Security.SecurityException">The request for <see cref="T:System.Security.Permissions.StrongNameIdentityPermission" /> failed.</exception>
		// Token: 0x06002962 RID: 10594 RVA: 0x00095FBB File Offset: 0x000941BB
		[MonoTODO("CAS support is experimental (and unsupported). This method only works in FullTrust.")]
		[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0x00000000000000000400000000000000")]
		public static void GetZoneAndOrigin(out ArrayList zone, out ArrayList origin)
		{
			zone = new ArrayList();
			origin = new ArrayList();
		}

		/// <summary>Determines whether a permission is granted to the caller.</summary>
		/// <param name="perm">The permission to test against the grant of the caller.</param>
		/// <returns>
		///   <see langword="true" /> if the permissions granted to the caller include the permission <paramref name="perm" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002963 RID: 10595 RVA: 0x00095FCB File Offset: 0x000941CB
		[Obsolete]
		public static bool IsGranted(IPermission perm)
		{
			return perm == null || !SecurityManager.SecurityEnabled || SecurityManager.IsGranted(Assembly.GetCallingAssembly(), perm);
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x00095FE8 File Offset: 0x000941E8
		internal static bool IsGranted(Assembly a, IPermission perm)
		{
			PermissionSet grantedPermissionSet = a.GrantedPermissionSet;
			if (grantedPermissionSet != null && !grantedPermissionSet.IsUnrestricted())
			{
				CodeAccessPermission codeAccessPermission = (CodeAccessPermission)grantedPermissionSet.GetPermission(perm.GetType());
				if (!perm.IsSubsetOf(codeAccessPermission))
				{
					return false;
				}
			}
			PermissionSet deniedPermissionSet = a.DeniedPermissionSet;
			if (deniedPermissionSet != null && !deniedPermissionSet.IsEmpty())
			{
				if (deniedPermissionSet.IsUnrestricted())
				{
					return false;
				}
				CodeAccessPermission codeAccessPermission2 = (CodeAccessPermission)a.DeniedPermissionSet.GetPermission(perm.GetType());
				if (codeAccessPermission2 != null && perm.IsSubsetOf(codeAccessPermission2))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Loads a <see cref="T:System.Security.Policy.PolicyLevel" /> from the specified file.</summary>
		/// <param name="path">The physical file path to a file containing the security policy information.</param>
		/// <param name="type">One of the enumeration values that specifies the type of the policy level to be loaded.</param>
		/// <returns>The loaded policy level.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The file indicated by the <paramref name="path" /> parameter does not exist.</exception>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />.  
		///  -or-  
		///  The code that calls this method does not have <see cref="F:System.Security.Permissions.FileIOPermissionAccess.Read" />.  
		///  -or-  
		///  The code that calls this method does not have <see cref="F:System.Security.Permissions.FileIOPermissionAccess.Write" />.  
		///  -or-  
		///  The code that calls this method does not have <see cref="F:System.Security.Permissions.FileIOPermissionAccess.PathDiscovery" />.</exception>
		/// <exception cref="T:System.NotSupportedException">This method uses code access security (CAS) policy, which is obsolete in the .NET Framework 4. To enable CAS policy for compatibility with earlier versions of the .NET Framework, use the &lt;legacyCasPolicy&gt; element.</exception>
		// Token: 0x06002965 RID: 10597 RVA: 0x00096068 File Offset: 0x00094268
		[Obsolete]
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public static PolicyLevel LoadPolicyLevelFromFile(string path, PolicyLevelType type)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			PolicyLevel policyLevel = null;
			try
			{
				policyLevel = new PolicyLevel(type.ToString(), type);
				policyLevel.LoadFromFile(path);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(Locale.GetText("Invalid policy XML"), ex);
			}
			return policyLevel;
		}

		/// <summary>Loads a <see cref="T:System.Security.Policy.PolicyLevel" /> from the specified string.</summary>
		/// <param name="str">The XML representation of a security policy level in the same form in which it appears in a configuration file.</param>
		/// <param name="type">One of the enumeration values that specifies the type of the policy level to be loaded.</param>
		/// <returns>The loaded policy level.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="str" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="str" /> parameter is not valid.</exception>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />.</exception>
		// Token: 0x06002966 RID: 10598 RVA: 0x000960C8 File Offset: 0x000942C8
		[Obsolete]
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public static PolicyLevel LoadPolicyLevelFromString(string str, PolicyLevelType type)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			PolicyLevel policyLevel = null;
			try
			{
				policyLevel = new PolicyLevel(type.ToString(), type);
				policyLevel.LoadFromString(str);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(Locale.GetText("Invalid policy XML"), ex);
			}
			return policyLevel;
		}

		/// <summary>Provides an enumerator to access the security policy hierarchy by levels, such as computer policy and user policy.</summary>
		/// <returns>An enumerator for <see cref="T:System.Security.Policy.PolicyLevel" /> objects that compose the security policy hierarchy.</returns>
		/// <exception cref="T:System.NotSupportedException">This method uses code access security (CAS) policy, which is obsolete in the .NET Framework 4. To enable CAS policy for compatibility with earlier versions of the .NET Framework, use the &lt;legacyCasPolicy&gt; element.</exception>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />.</exception>
		// Token: 0x06002967 RID: 10599 RVA: 0x00096128 File Offset: 0x00094328
		[Obsolete]
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public static IEnumerator PolicyHierarchy()
		{
			return SecurityManager.Hierarchy;
		}

		/// <summary>Determines what permissions to grant to code based on the specified evidence.</summary>
		/// <param name="evidence">The evidence set used to evaluate policy.</param>
		/// <returns>The set of permissions that can be granted by the security system.</returns>
		/// <exception cref="T:System.NotSupportedException">This method uses code access security (CAS) policy, which is obsolete in the .NET Framework 4. To enable CAS policy for compatibility with earlier versions of the .NET Framework, use the &lt;legacyCasPolicy&gt; element.</exception>
		// Token: 0x06002968 RID: 10600 RVA: 0x00096130 File Offset: 0x00094330
		[Obsolete]
		public static PermissionSet ResolvePolicy(Evidence evidence)
		{
			if (evidence == null)
			{
				return new PermissionSet(PermissionState.None);
			}
			PermissionSet permissionSet = null;
			IEnumerator hierarchy = SecurityManager.Hierarchy;
			while (hierarchy.MoveNext())
			{
				object obj = hierarchy.Current;
				PolicyLevel policyLevel = (PolicyLevel)obj;
				if (SecurityManager.ResolvePolicyLevel(ref permissionSet, policyLevel, evidence))
				{
					break;
				}
			}
			SecurityManager.ResolveIdentityPermissions(permissionSet, evidence);
			return permissionSet;
		}

		/// <summary>Determines what permissions to grant to code based on the specified evidence.</summary>
		/// <param name="evidences">An array of evidence objects used to evaluate policy.</param>
		/// <returns>The set of permissions that is appropriate for all of the provided evidence.</returns>
		/// <exception cref="T:System.NotSupportedException">This method uses code access security (CAS) policy, which is obsolete in the .NET Framework 4. To enable CAS policy for compatibility with earlier versions of the .NET Framework, use the &lt;legacyCasPolicy&gt; element.</exception>
		// Token: 0x06002969 RID: 10601 RVA: 0x00096178 File Offset: 0x00094378
		[MonoTODO("(2.0) more tests are needed")]
		[Obsolete]
		public static PermissionSet ResolvePolicy(Evidence[] evidences)
		{
			if (evidences == null || evidences.Length == 0 || (evidences.Length == 1 && evidences[0].Count == 0))
			{
				return new PermissionSet(PermissionState.None);
			}
			PermissionSet permissionSet = SecurityManager.ResolvePolicy(evidences[0]);
			for (int i = 1; i < evidences.Length; i++)
			{
				permissionSet = permissionSet.Intersect(SecurityManager.ResolvePolicy(evidences[i]));
			}
			return permissionSet;
		}

		/// <summary>Determines which permissions to grant to code based on the specified evidence, excluding the policy for the <see cref="T:System.AppDomain" /> level.</summary>
		/// <param name="evidence">The evidence set used to evaluate policy.</param>
		/// <returns>The set of permissions that can be granted by the security system.</returns>
		/// <exception cref="T:System.NotSupportedException">This method uses code access security (CAS) policy, which is obsolete in the .NET Framework 4. To enable CAS policy for compatibility with earlier versions of the .NET Framework, use the &lt;legacyCasPolicy&gt; element.</exception>
		// Token: 0x0600296A RID: 10602 RVA: 0x000961CC File Offset: 0x000943CC
		[Obsolete]
		public static PermissionSet ResolveSystemPolicy(Evidence evidence)
		{
			if (evidence == null)
			{
				return new PermissionSet(PermissionState.None);
			}
			PermissionSet permissionSet = null;
			IEnumerator hierarchy = SecurityManager.Hierarchy;
			while (hierarchy.MoveNext())
			{
				object obj = hierarchy.Current;
				PolicyLevel policyLevel = (PolicyLevel)obj;
				if (policyLevel.Type == PolicyLevelType.AppDomain || SecurityManager.ResolvePolicyLevel(ref permissionSet, policyLevel, evidence))
				{
					break;
				}
			}
			SecurityManager.ResolveIdentityPermissions(permissionSet, evidence);
			return permissionSet;
		}

		/// <summary>Determines what permissions to grant to code based on the specified evidence and requests.</summary>
		/// <param name="evidence">The evidence set used to evaluate policy.</param>
		/// <param name="reqdPset">The required permissions the code needs to run.</param>
		/// <param name="optPset">The optional permissions that will be used if granted, but aren't required for the code to run.</param>
		/// <param name="denyPset">The denied permissions that must never be granted to the code even if policy otherwise permits it.</param>
		/// <param name="denied">An output parameter that contains the set of permissions not granted.</param>
		/// <returns>The set of permissions that would be granted by the security system.</returns>
		/// <exception cref="T:System.NotSupportedException">This method uses code access security (CAS) policy, which is obsolete in the .NET Framework 4. To enable CAS policy for compatibility with earlier versions of the .NET Framework, use the &lt;legacyCasPolicy&gt; element.</exception>
		/// <exception cref="T:System.Security.Policy.PolicyException">Policy fails to grant the minimum required permissions specified by the <paramref name="reqdPset" /> parameter.</exception>
		// Token: 0x0600296B RID: 10603 RVA: 0x00096220 File Offset: 0x00094420
		[Obsolete]
		public static PermissionSet ResolvePolicy(Evidence evidence, PermissionSet reqdPset, PermissionSet optPset, PermissionSet denyPset, out PermissionSet denied)
		{
			PermissionSet permissionSet = SecurityManager.ResolvePolicy(evidence);
			if (reqdPset != null && !reqdPset.IsSubsetOf(permissionSet))
			{
				throw new PolicyException(Locale.GetText("Policy doesn't grant the minimal permissions required to execute the assembly."));
			}
			if (SecurityManager.CheckExecutionRights)
			{
				bool flag = false;
				if (permissionSet != null)
				{
					if (permissionSet.IsUnrestricted())
					{
						flag = true;
					}
					else
					{
						IPermission permission = permissionSet.GetPermission(typeof(SecurityPermission));
						flag = SecurityManager._execution.IsSubsetOf(permission);
					}
				}
				if (!flag)
				{
					throw new PolicyException(Locale.GetText("Policy doesn't grant the right to execute the assembly."));
				}
			}
			denied = denyPset;
			return permissionSet;
		}

		/// <summary>Gets a collection of code groups matching the specified evidence.</summary>
		/// <param name="evidence">The evidence set against which the policy is evaluated.</param>
		/// <returns>An enumeration of the set of code groups matching the evidence.</returns>
		/// <exception cref="T:System.NotSupportedException">This method uses code access security (CAS) policy, which is obsolete in the .NET Framework 4. To enable CAS policy for compatibility with earlier versions of the .NET Framework, use the &lt;legacyCasPolicy&gt; element.</exception>
		// Token: 0x0600296C RID: 10604 RVA: 0x000962A0 File Offset: 0x000944A0
		[Obsolete]
		public static IEnumerator ResolvePolicyGroups(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			ArrayList arrayList = new ArrayList();
			IEnumerator hierarchy = SecurityManager.Hierarchy;
			while (hierarchy.MoveNext())
			{
				object obj = hierarchy.Current;
				CodeGroup codeGroup = ((PolicyLevel)obj).ResolveMatchingCodeGroups(evidence);
				arrayList.Add(codeGroup);
			}
			return arrayList.GetEnumerator();
		}

		/// <summary>Saves the modified security policy state.</summary>
		/// <exception cref="T:System.NotSupportedException">This method uses code access security (CAS) policy, which is obsolete in the .NET Framework 4. To enable CAS policy for compatibility with earlier versions of the .NET Framework, use the &lt;legacyCasPolicy&gt; element.</exception>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />.</exception>
		// Token: 0x0600296D RID: 10605 RVA: 0x000962F4 File Offset: 0x000944F4
		[Obsolete]
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public static void SavePolicy()
		{
			IEnumerator hierarchy = SecurityManager.Hierarchy;
			while (hierarchy.MoveNext())
			{
				object obj = hierarchy.Current;
				(obj as PolicyLevel).Save();
			}
		}

		/// <summary>Saves a modified security policy level loaded with <see cref="M:System.Security.SecurityManager.LoadPolicyLevelFromFile(System.String,System.Security.PolicyLevelType)" />.</summary>
		/// <param name="level">The policy level object to be saved.</param>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />.</exception>
		/// <exception cref="T:System.NotSupportedException">This method uses code access security (CAS) policy, which is obsolete in the .NET Framework 4. To enable CAS policy for compatibility with earlier versions of the .NET Framework, use the &lt;legacyCasPolicy&gt; element.</exception>
		// Token: 0x0600296E RID: 10606 RVA: 0x00096321 File Offset: 0x00094521
		[Obsolete]
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public static void SavePolicyLevel(PolicyLevel level)
		{
			level.Save();
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x0600296F RID: 10607 RVA: 0x0009632C File Offset: 0x0009452C
		private static IEnumerator Hierarchy
		{
			get
			{
				object lockObject = SecurityManager._lockObject;
				lock (lockObject)
				{
					if (SecurityManager._hierarchy == null)
					{
						SecurityManager.InitializePolicyHierarchy();
					}
				}
				return SecurityManager._hierarchy.GetEnumerator();
			}
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x0009637C File Offset: 0x0009457C
		private static void InitializePolicyHierarchy()
		{
			string directoryName = Path.GetDirectoryName(Environment.GetMachineConfigPath());
			string text = Path.Combine(Environment.UnixGetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), "mono");
			PolicyLevel policyLevel = new PolicyLevel("Enterprise", PolicyLevelType.Enterprise);
			SecurityManager._level = policyLevel;
			policyLevel.LoadFromFile(Path.Combine(directoryName, "enterprisesec.config"));
			PolicyLevel policyLevel2 = new PolicyLevel("Machine", PolicyLevelType.Machine);
			SecurityManager._level = policyLevel2;
			policyLevel2.LoadFromFile(Path.Combine(directoryName, "security.config"));
			PolicyLevel policyLevel3 = new PolicyLevel("User", PolicyLevelType.User);
			SecurityManager._level = policyLevel3;
			policyLevel3.LoadFromFile(Path.Combine(text, "security.config"));
			SecurityManager._hierarchy = ArrayList.Synchronized(new ArrayList { policyLevel, policyLevel2, policyLevel3 });
			SecurityManager._level = null;
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x00096448 File Offset: 0x00094648
		internal static bool ResolvePolicyLevel(ref PermissionSet ps, PolicyLevel pl, Evidence evidence)
		{
			PolicyStatement policyStatement = pl.Resolve(evidence);
			if (policyStatement != null)
			{
				if (ps == null)
				{
					ps = policyStatement.PermissionSet;
				}
				else
				{
					ps = ps.Intersect(policyStatement.PermissionSet);
					if (ps == null)
					{
						ps = new PermissionSet(PermissionState.None);
					}
				}
				if ((policyStatement.Attributes & PolicyStatementAttribute.LevelFinal) == PolicyStatementAttribute.LevelFinal)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x00096498 File Offset: 0x00094698
		internal static void ResolveIdentityPermissions(PermissionSet ps, Evidence evidence)
		{
			if (ps.IsUnrestricted())
			{
				return;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				IIdentityPermissionFactory identityPermissionFactory = obj as IIdentityPermissionFactory;
				if (identityPermissionFactory != null)
				{
					IPermission permission = identityPermissionFactory.CreateIdentityPermission(evidence);
					ps.AddPermission(permission);
				}
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06002973 RID: 10611 RVA: 0x000964DE File Offset: 0x000946DE
		// (set) Token: 0x06002974 RID: 10612 RVA: 0x000964E5 File Offset: 0x000946E5
		internal static PolicyLevel ResolvingPolicyLevel
		{
			get
			{
				return SecurityManager._level;
			}
			set
			{
				SecurityManager._level = value;
			}
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000964F0 File Offset: 0x000946F0
		internal static PermissionSet Decode(IntPtr permissions, int length)
		{
			PermissionSet permissionSet = null;
			object lockObject = SecurityManager._lockObject;
			lock (lockObject)
			{
				if (SecurityManager._declsecCache == null)
				{
					SecurityManager._declsecCache = new Hashtable();
				}
				object obj = (int)permissions;
				permissionSet = (PermissionSet)SecurityManager._declsecCache[obj];
				if (permissionSet == null)
				{
					byte[] array = new byte[length];
					Marshal.Copy(permissions, array, 0, length);
					permissionSet = SecurityManager.Decode(array);
					permissionSet.DeclarativeSecurity = true;
					SecurityManager._declsecCache.Add(obj, permissionSet);
				}
			}
			return permissionSet;
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x0009658C File Offset: 0x0009478C
		internal static PermissionSet Decode(byte[] encodedPermissions)
		{
			if (encodedPermissions == null || encodedPermissions.Length < 1)
			{
				throw new SecurityException("Invalid metadata format.");
			}
			byte b = encodedPermissions[0];
			if (b == 46)
			{
				return PermissionSet.CreateFromBinaryFormat(encodedPermissions);
			}
			if (b == 60)
			{
				return new PermissionSet(Encoding.Unicode.GetString(encodedPermissions));
			}
			throw new SecurityException(Locale.GetText("Unknown metadata format."));
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06002977 RID: 10615 RVA: 0x000965E4 File Offset: 0x000947E4
		private static IPermission UnmanagedCode
		{
			get
			{
				object lockObject = SecurityManager._lockObject;
				lock (lockObject)
				{
					if (SecurityManager._unmanagedCode == null)
					{
						SecurityManager._unmanagedCode = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
					}
				}
				return SecurityManager._unmanagedCode;
			}
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x00096634 File Offset: 0x00094834
		private static void ThrowException(Exception ex)
		{
			throw ex;
		}

		/// <summary>Gets a permission set that is safe to grant to an application that has the provided evidence.</summary>
		/// <param name="evidence">The host evidence to match to a permission set.</param>
		/// <returns>A permission set that can be used as a grant set for the application that has the provided evidence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="evidence" /> is <see langword="null" />.</exception>
		// Token: 0x06002979 RID: 10617 RVA: 0x00096637 File Offset: 0x00094837
		public static PermissionSet GetStandardSandbox(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			throw new NotImplementedException();
		}

		/// <summary>Determines whether the current thread requires a security context capture if its security state has to be re-created at a later point in time.</summary>
		/// <returns>
		///   <see langword="false" /> if the stack contains no partially trusted application domains, no partially trusted assemblies, and no currently active <see cref="M:System.Security.CodeAccessPermission.PermitOnly" /> or <see cref="M:System.Security.CodeAccessPermission.Deny" /> modifiers; <see langword="true" /> if the common language runtime cannot guarantee that the stack contains none of these.</returns>
		// Token: 0x0600297A RID: 10618 RVA: 0x000479FC File Offset: 0x00045BFC
		public static bool CurrentThreadRequiresSecurityContextCapture()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04001EF5 RID: 7925
		private static object _lockObject = new object();

		// Token: 0x04001EF6 RID: 7926
		private static ArrayList _hierarchy;

		// Token: 0x04001EF7 RID: 7927
		private static IPermission _unmanagedCode;

		// Token: 0x04001EF8 RID: 7928
		private static Hashtable _declsecCache;

		// Token: 0x04001EF9 RID: 7929
		private static PolicyLevel _level;

		// Token: 0x04001EFA RID: 7930
		private static SecurityPermission _execution = new SecurityPermission(SecurityPermissionFlag.Execution);
	}
}
