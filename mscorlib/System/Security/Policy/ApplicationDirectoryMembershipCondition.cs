using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using Mono.Security;

namespace System.Security.Policy
{
	/// <summary>Determines whether an assembly belongs to a code group by testing its application directory. This class cannot be inherited.</summary>
	// Token: 0x02000400 RID: 1024
	[ComVisible(true)]
	[Serializable]
	public sealed class ApplicationDirectoryMembershipCondition : IConstantMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		/// <summary>Determines whether the membership condition is satisfied by the specified evidence.</summary>
		/// <param name="evidence">The evidence set against which to make the test.</param>
		/// <returns>
		///   <see langword="true" /> if the specified evidence satisfies the membership condition; otherwise, <see langword="false" />.</returns>
		// Token: 0x060029DB RID: 10715 RVA: 0x00098004 File Offset: 0x00096204
		public bool Check(Evidence evidence)
		{
			if (evidence == null)
			{
				return false;
			}
			string codeBase = Assembly.GetCallingAssembly().CodeBase;
			Uri uri = new Uri(codeBase);
			Url url = new Url(codeBase);
			bool flag = false;
			bool flag2 = false;
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				if (!flag && obj is ApplicationDirectory)
				{
					string directory = (obj as ApplicationDirectory).Directory;
					flag = string.Compare(directory, 0, uri.ToString(), 0, directory.Length, true, CultureInfo.InvariantCulture) == 0;
				}
				else if (!flag2 && obj is Url)
				{
					flag2 = url.Equals(obj);
				}
				if (flag && flag2)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Creates an equivalent copy of the membership condition.</summary>
		/// <returns>A new, identical copy of the current membership condition.</returns>
		// Token: 0x060029DC RID: 10716 RVA: 0x000980A6 File Offset: 0x000962A6
		public IMembershipCondition Copy()
		{
			return new ApplicationDirectoryMembershipCondition();
		}

		/// <summary>Determines whether the specified membership condition is an <see cref="T:System.Security.Policy.ApplicationDirectoryMembershipCondition" />.</summary>
		/// <param name="o">The object to compare to <see cref="T:System.Security.Policy.ApplicationDirectoryMembershipCondition" />.</param>
		/// <returns>
		///   <see langword="true" /> if the specified membership condition is an <see cref="T:System.Security.Policy.ApplicationDirectoryMembershipCondition" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060029DD RID: 10717 RVA: 0x000980AD File Offset: 0x000962AD
		public override bool Equals(object o)
		{
			return o is ApplicationDirectoryMembershipCondition;
		}

		/// <summary>Reconstructs a security object with a specified state from an XML encoding.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="e" /> parameter is not a valid application directory membership condition element.</exception>
		// Token: 0x060029DE RID: 10718 RVA: 0x000980B8 File Offset: 0x000962B8
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		/// <summary>Reconstructs a security object with a specified state from an XML encoding.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object.</param>
		/// <param name="level">The policy level context, used to resolve named permission set references.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="e" /> parameter is not a valid application directory membership condition element.</exception>
		// Token: 0x060029DF RID: 10719 RVA: 0x000980C2 File Offset: 0x000962C2
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			MembershipConditionHelper.CheckSecurityElement(e, "e", this.version, this.version);
		}

		/// <summary>Gets the hash code for the current membership condition.</summary>
		/// <returns>The hash code for the current membership condition.</returns>
		// Token: 0x060029E0 RID: 10720 RVA: 0x000980DC File Offset: 0x000962DC
		public override int GetHashCode()
		{
			return typeof(ApplicationDirectoryMembershipCondition).GetHashCode();
		}

		/// <summary>Creates and returns a string representation of the membership condition.</summary>
		/// <returns>A string representation of the state of the membership condition.</returns>
		// Token: 0x060029E1 RID: 10721 RVA: 0x000980ED File Offset: 0x000962ED
		public override string ToString()
		{
			return "ApplicationDirectory";
		}

		/// <summary>Creates an XML encoding of the security object and its current state.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		// Token: 0x060029E2 RID: 10722 RVA: 0x000980F4 File Offset: 0x000962F4
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		/// <summary>Creates an XML encoding of the security object and its current state with the specified <see cref="T:System.Security.Policy.PolicyLevel" />.</summary>
		/// <param name="level">The policy level context for resolving named permission set references.</param>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		// Token: 0x060029E3 RID: 10723 RVA: 0x000980FD File Offset: 0x000962FD
		public SecurityElement ToXml(PolicyLevel level)
		{
			return MembershipConditionHelper.Element(typeof(ApplicationDirectoryMembershipCondition), this.version);
		}

		// Token: 0x04001F54 RID: 8020
		private readonly int version = 1;
	}
}
