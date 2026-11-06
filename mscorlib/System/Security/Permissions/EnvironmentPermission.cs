using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Permissions
{
	/// <summary>Controls access to system and user environment variables. This class cannot be inherited.</summary>
	// Token: 0x02000433 RID: 1075
	[ComVisible(true)]
	[Serializable]
	public sealed class EnvironmentPermission : CodeAccessPermission, IUnrestrictedPermission, IBuiltInPermission
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.EnvironmentPermission" /> class with either restricted or unrestricted permission as specified.</summary>
		/// <param name="state">One of the <see cref="T:System.Security.Permissions.PermissionState" /> values.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="state" /> parameter is not a valid value of <see cref="T:System.Security.Permissions.PermissionState" />.</exception>
		// Token: 0x06002B9A RID: 11162 RVA: 0x0009D5C8 File Offset: 0x0009B7C8
		public EnvironmentPermission(PermissionState state)
		{
			this._state = CodeAccessPermission.CheckPermissionState(state, true);
			this.readList = new ArrayList();
			this.writeList = new ArrayList();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.EnvironmentPermission" /> class with the specified access to the specified environment variables.</summary>
		/// <param name="flag">One of the <see cref="T:System.Security.Permissions.EnvironmentPermissionAccess" /> values.</param>
		/// <param name="pathList">A list of environment variables (semicolon-separated) to which access is granted.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="pathList" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="flag" /> parameter is not a valid value of <see cref="T:System.Security.Permissions.EnvironmentPermissionAccess" />.</exception>
		// Token: 0x06002B9B RID: 11163 RVA: 0x0009D5F3 File Offset: 0x0009B7F3
		public EnvironmentPermission(EnvironmentPermissionAccess flag, string pathList)
		{
			this.readList = new ArrayList();
			this.writeList = new ArrayList();
			this.SetPathList(flag, pathList);
		}

		/// <summary>Adds access for the specified environment variables to the existing state of the permission.</summary>
		/// <param name="flag">One of the <see cref="T:System.Security.Permissions.EnvironmentPermissionAccess" /> values.</param>
		/// <param name="pathList">A list of environment variables (semicolon-separated).</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="pathList" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="flag" /> parameter is not a valid value of <see cref="T:System.Security.Permissions.EnvironmentPermissionAccess" />.</exception>
		// Token: 0x06002B9C RID: 11164 RVA: 0x0009D61C File Offset: 0x0009B81C
		public void AddPathList(EnvironmentPermissionAccess flag, string pathList)
		{
			if (pathList == null)
			{
				throw new ArgumentNullException("pathList");
			}
			switch (flag)
			{
			case EnvironmentPermissionAccess.NoAccess:
				break;
			case EnvironmentPermissionAccess.Read:
				foreach (string text in pathList.Split(';', StringSplitOptions.None))
				{
					if (!this.readList.Contains(text))
					{
						this.readList.Add(text);
					}
				}
				return;
			case EnvironmentPermissionAccess.Write:
				foreach (string text2 in pathList.Split(';', StringSplitOptions.None))
				{
					if (!this.writeList.Contains(text2))
					{
						this.writeList.Add(text2);
					}
				}
				return;
			case EnvironmentPermissionAccess.AllAccess:
				foreach (string text3 in pathList.Split(';', StringSplitOptions.None))
				{
					if (!this.readList.Contains(text3))
					{
						this.readList.Add(text3);
					}
					if (!this.writeList.Contains(text3))
					{
						this.writeList.Add(text3);
					}
				}
				return;
			default:
				this.ThrowInvalidFlag(flag, false);
				break;
			}
		}

		/// <summary>Creates and returns an identical copy of the current permission.</summary>
		/// <returns>A copy of the current permission.</returns>
		// Token: 0x06002B9D RID: 11165 RVA: 0x0009D720 File Offset: 0x0009B920
		public override IPermission Copy()
		{
			EnvironmentPermission environmentPermission = new EnvironmentPermission(this._state);
			string text = this.GetPathList(EnvironmentPermissionAccess.Read);
			if (text != null)
			{
				environmentPermission.SetPathList(EnvironmentPermissionAccess.Read, text);
			}
			text = this.GetPathList(EnvironmentPermissionAccess.Write);
			if (text != null)
			{
				environmentPermission.SetPathList(EnvironmentPermissionAccess.Write, text);
			}
			return environmentPermission;
		}

		/// <summary>Reconstructs a permission with a specified state from an XML encoding.</summary>
		/// <param name="esd">The XML encoding to use to reconstruct the permission.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="esd" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="esd" /> parameter is not a valid permission element.  
		///  -or-  
		///  The <paramref name="esd" /> parameter's version number is not valid.</exception>
		// Token: 0x06002B9E RID: 11166 RVA: 0x0009D760 File Offset: 0x0009B960
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
			if (CodeAccessPermission.IsUnrestricted(esd))
			{
				this._state = PermissionState.Unrestricted;
			}
			string text = esd.Attribute("Read");
			if (text != null && text.Length > 0)
			{
				this.SetPathList(EnvironmentPermissionAccess.Read, text);
			}
			string text2 = esd.Attribute("Write");
			if (text2 != null && text2.Length > 0)
			{
				this.SetPathList(EnvironmentPermissionAccess.Write, text2);
			}
		}

		/// <summary>Gets all environment variables with the specified <see cref="T:System.Security.Permissions.EnvironmentPermissionAccess" />.</summary>
		/// <param name="flag">One of the <see cref="T:System.Security.Permissions.EnvironmentPermissionAccess" /> values that represents a single type of environment variable access.</param>
		/// <returns>A list of environment variables (semicolon-separated) for the selected flag.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="flag" /> is not a valid value of <see cref="T:System.Security.Permissions.EnvironmentPermissionAccess" />.  
		/// -or-  
		/// <paramref name="flag" /> is <see cref="F:System.Security.Permissions.EnvironmentPermissionAccess.AllAccess" />, which represents more than one type of environment variable access, or <see cref="F:System.Security.Permissions.EnvironmentPermissionAccess.NoAccess" />, which does not represent any type of environment variable access.</exception>
		// Token: 0x06002B9F RID: 11167 RVA: 0x0009D7CC File Offset: 0x0009B9CC
		public string GetPathList(EnvironmentPermissionAccess flag)
		{
			switch (flag)
			{
			case EnvironmentPermissionAccess.NoAccess:
			case EnvironmentPermissionAccess.AllAccess:
				this.ThrowInvalidFlag(flag, true);
				break;
			case EnvironmentPermissionAccess.Read:
				return this.GetPathList(this.readList);
			case EnvironmentPermissionAccess.Write:
				return this.GetPathList(this.writeList);
			default:
				this.ThrowInvalidFlag(flag, false);
				break;
			}
			return null;
		}

		/// <summary>Creates and returns a permission that is the intersection of the current permission and the specified permission.</summary>
		/// <param name="target">A permission to intersect with the current permission. It must be of the same type as the current permission.</param>
		/// <returns>A new permission that represents the intersection of the current permission and the specified permission. This new permission is <see langword="null" /> if the intersection is empty.</returns>
		/// <exception cref="T:System.ArgumentException">The <paramref name="target" /> parameter is not <see langword="null" /> and is not of the same type as the current permission.</exception>
		// Token: 0x06002BA0 RID: 11168 RVA: 0x0009D820 File Offset: 0x0009BA20
		[SecuritySafeCritical]
		public override IPermission Intersect(IPermission target)
		{
			EnvironmentPermission environmentPermission = this.Cast(target);
			if (environmentPermission == null)
			{
				return null;
			}
			if (this.IsUnrestricted())
			{
				return environmentPermission.Copy();
			}
			if (environmentPermission.IsUnrestricted())
			{
				return this.Copy();
			}
			int num = 0;
			EnvironmentPermission environmentPermission2 = new EnvironmentPermission(PermissionState.None);
			string pathList = environmentPermission.GetPathList(EnvironmentPermissionAccess.Read);
			if (pathList != null)
			{
				foreach (string text in pathList.Split(';', StringSplitOptions.None))
				{
					if (this.readList.Contains(text))
					{
						environmentPermission2.AddPathList(EnvironmentPermissionAccess.Read, text);
						num++;
					}
				}
			}
			string pathList2 = environmentPermission.GetPathList(EnvironmentPermissionAccess.Write);
			if (pathList2 != null)
			{
				foreach (string text2 in pathList2.Split(';', StringSplitOptions.None))
				{
					if (this.writeList.Contains(text2))
					{
						environmentPermission2.AddPathList(EnvironmentPermissionAccess.Write, text2);
						num++;
					}
				}
			}
			if (num <= 0)
			{
				return null;
			}
			return environmentPermission2;
		}

		/// <summary>Determines whether the current permission is a subset of the specified permission.</summary>
		/// <param name="target">A permission that is to be tested for the subset relationship. This permission must be of the same type as the current permission.</param>
		/// <returns>
		///   <see langword="true" /> if the current permission is a subset of the specified permission; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <paramref name="target" /> parameter is not <see langword="null" /> and is not of the same type as the current permission.</exception>
		// Token: 0x06002BA1 RID: 11169 RVA: 0x0009D904 File Offset: 0x0009BB04
		[SecuritySafeCritical]
		public override bool IsSubsetOf(IPermission target)
		{
			EnvironmentPermission environmentPermission = this.Cast(target);
			if (environmentPermission == null)
			{
				return false;
			}
			if (this.IsUnrestricted())
			{
				return environmentPermission.IsUnrestricted();
			}
			if (environmentPermission.IsUnrestricted())
			{
				return true;
			}
			foreach (object obj in this.readList)
			{
				string text = (string)obj;
				if (!environmentPermission.readList.Contains(text))
				{
					return false;
				}
			}
			foreach (object obj2 in this.writeList)
			{
				string text2 = (string)obj2;
				if (!environmentPermission.writeList.Contains(text2))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Returns a value indicating whether the current permission is unrestricted.</summary>
		/// <returns>
		///   <see langword="true" /> if the current permission is unrestricted; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002BA2 RID: 11170 RVA: 0x0009D9EC File Offset: 0x0009BBEC
		public bool IsUnrestricted()
		{
			return this._state == PermissionState.Unrestricted;
		}

		/// <summary>Sets the specified access to the specified environment variables to the existing state of the permission.</summary>
		/// <param name="flag">One of the <see cref="T:System.Security.Permissions.EnvironmentPermissionAccess" /> values.</param>
		/// <param name="pathList">A list of environment variables (semicolon-separated).</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="pathList" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="flag" /> parameter is not a valid value of <see cref="T:System.Security.Permissions.EnvironmentPermissionAccess" />.</exception>
		// Token: 0x06002BA3 RID: 11171 RVA: 0x0009D9F8 File Offset: 0x0009BBF8
		public void SetPathList(EnvironmentPermissionAccess flag, string pathList)
		{
			if (pathList == null)
			{
				throw new ArgumentNullException("pathList");
			}
			switch (flag)
			{
			case EnvironmentPermissionAccess.NoAccess:
				break;
			case EnvironmentPermissionAccess.Read:
				this.readList.Clear();
				foreach (string text in pathList.Split(';', StringSplitOptions.None))
				{
					this.readList.Add(text);
				}
				return;
			case EnvironmentPermissionAccess.Write:
				this.writeList.Clear();
				foreach (string text2 in pathList.Split(';', StringSplitOptions.None))
				{
					this.writeList.Add(text2);
				}
				return;
			case EnvironmentPermissionAccess.AllAccess:
				this.readList.Clear();
				this.writeList.Clear();
				foreach (string text3 in pathList.Split(';', StringSplitOptions.None))
				{
					this.readList.Add(text3);
					this.writeList.Add(text3);
				}
				return;
			default:
				this.ThrowInvalidFlag(flag, false);
				break;
			}
		}

		/// <summary>Creates an XML encoding of the permission and its current state.</summary>
		/// <returns>An XML encoding of the permission, including any state information.</returns>
		// Token: 0x06002BA4 RID: 11172 RVA: 0x0009DAF0 File Offset: 0x0009BCF0
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			if (this._state == PermissionState.Unrestricted)
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			else
			{
				string text = this.GetPathList(EnvironmentPermissionAccess.Read);
				if (text != null)
				{
					securityElement.AddAttribute("Read", text);
				}
				text = this.GetPathList(EnvironmentPermissionAccess.Write);
				if (text != null)
				{
					securityElement.AddAttribute("Write", text);
				}
			}
			return securityElement;
		}

		/// <summary>Creates a permission that is the union of the current permission and the specified permission.</summary>
		/// <param name="other">A permission to combine with the current permission. It must be of the same type as the current permission.</param>
		/// <returns>A new permission that represents the union of the current permission and the specified permission.</returns>
		/// <exception cref="T:System.ArgumentException">The <paramref name="other" /> parameter is not <see langword="null" /> and is not of the same type as the current permission.</exception>
		// Token: 0x06002BA5 RID: 11173 RVA: 0x0009DB50 File Offset: 0x0009BD50
		[SecuritySafeCritical]
		public override IPermission Union(IPermission other)
		{
			EnvironmentPermission environmentPermission = this.Cast(other);
			if (environmentPermission == null)
			{
				return this.Copy();
			}
			if (this.IsUnrestricted() || environmentPermission.IsUnrestricted())
			{
				return new EnvironmentPermission(PermissionState.Unrestricted);
			}
			if (this.IsEmpty() && environmentPermission.IsEmpty())
			{
				return null;
			}
			EnvironmentPermission environmentPermission2 = (EnvironmentPermission)this.Copy();
			string text = environmentPermission.GetPathList(EnvironmentPermissionAccess.Read);
			if (text != null)
			{
				environmentPermission2.AddPathList(EnvironmentPermissionAccess.Read, text);
			}
			text = environmentPermission.GetPathList(EnvironmentPermissionAccess.Write);
			if (text != null)
			{
				environmentPermission2.AddPathList(EnvironmentPermissionAccess.Write, text);
			}
			return environmentPermission2;
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		int IBuiltInPermission.GetTokenIndex()
		{
			return 0;
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x0009DBCB File Offset: 0x0009BDCB
		private bool IsEmpty()
		{
			return this._state == PermissionState.None && this.readList.Count == 0 && this.writeList.Count == 0;
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x0009DBF2 File Offset: 0x0009BDF2
		private EnvironmentPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			EnvironmentPermission environmentPermission = target as EnvironmentPermission;
			if (environmentPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(EnvironmentPermission));
			}
			return environmentPermission;
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x0009DC14 File Offset: 0x0009BE14
		internal void ThrowInvalidFlag(EnvironmentPermissionAccess flag, bool context)
		{
			string text;
			if (context)
			{
				text = Locale.GetText("Unknown flag '{0}'.");
			}
			else
			{
				text = Locale.GetText("Invalid flag '{0}' in this context.");
			}
			throw new ArgumentException(string.Format(text, flag), "flag");
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x0009DC54 File Offset: 0x0009BE54
		private string GetPathList(ArrayList list)
		{
			if (this.IsUnrestricted())
			{
				return string.Empty;
			}
			if (list.Count == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in list)
			{
				string text = (string)obj;
				stringBuilder.Append(text);
				stringBuilder.Append(";");
			}
			string text2 = stringBuilder.ToString();
			int length = text2.Length;
			if (length > 0)
			{
				return text2.Substring(0, length - 1);
			}
			return string.Empty;
		}

		// Token: 0x04002004 RID: 8196
		private const int version = 1;

		// Token: 0x04002005 RID: 8197
		private PermissionState _state;

		// Token: 0x04002006 RID: 8198
		private ArrayList readList;

		// Token: 0x04002007 RID: 8199
		private ArrayList writeList;
	}
}
