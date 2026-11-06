using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net.Mail
{
	/// <summary>Controls access to Simple Mail Transport Protocol (SMTP) servers.</summary>
	// Token: 0x02000812 RID: 2066
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class SmtpPermissionAttribute : CodeAccessSecurityAttribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpPermissionAttribute" /> class.</summary>
		/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values that specifies the permission behavior.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="action" /> is not a valid <see cref="T:System.Security.Permissions.SecurityAction" />.</exception>
		// Token: 0x0600422B RID: 16939 RVA: 0x000A97B6 File Offset: 0x000A79B6
		public SmtpPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		/// <summary>Gets or sets the level of access to SMTP servers controlled by the attribute.</summary>
		/// <returns>A <see cref="T:System.String" /> value. Valid values are "Connect" and "None".</returns>
		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x0600422C RID: 16940 RVA: 0x000E4BF4 File Offset: 0x000E2DF4
		// (set) Token: 0x0600422D RID: 16941 RVA: 0x000E4BFC File Offset: 0x000E2DFC
		public string Access
		{
			get
			{
				return this.access;
			}
			set
			{
				this.access = value;
			}
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x000E4C08 File Offset: 0x000E2E08
		private SmtpAccess GetSmtpAccess()
		{
			if (this.access == null)
			{
				return SmtpAccess.None;
			}
			string text = this.access.ToLowerInvariant();
			if (text == "connecttounrestrictedport")
			{
				return SmtpAccess.ConnectToUnrestrictedPort;
			}
			if (text == "connect")
			{
				return SmtpAccess.Connect;
			}
			if (!(text == "none"))
			{
				string text2 = global::Locale.GetText("Invalid Access='{0}' value.", new object[] { this.access });
				throw new ArgumentException("Access", text2);
			}
			return SmtpAccess.None;
		}

		/// <summary>Creates a permission object that can be stored with the <see cref="T:System.Security.Permissions.SecurityAction" /> in an assembly's metadata.</summary>
		/// <returns>An <see cref="T:System.Net.Mail.SmtpPermission" /> instance.</returns>
		// Token: 0x0600422F RID: 16943 RVA: 0x000E4C7F File Offset: 0x000E2E7F
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new SmtpPermission(true);
			}
			return new SmtpPermission(this.GetSmtpAccess());
		}

		// Token: 0x04002784 RID: 10116
		private string access;
	}
}
