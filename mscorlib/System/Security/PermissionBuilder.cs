using System;
using System.Security.Permissions;

namespace System.Security
{
	// Token: 0x020003E2 RID: 994
	internal static class PermissionBuilder
	{
		// Token: 0x060028C5 RID: 10437 RVA: 0x000935A4 File Offset: 0x000917A4
		public static IPermission Create(string fullname, PermissionState state)
		{
			if (fullname == null)
			{
				throw new ArgumentNullException("fullname");
			}
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", fullname);
			securityElement.AddAttribute("version", "1");
			if (state == PermissionState.Unrestricted)
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return PermissionBuilder.CreatePermission(fullname, securityElement);
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x00093604 File Offset: 0x00091804
		public static IPermission Create(SecurityElement se)
		{
			if (se == null)
			{
				throw new ArgumentNullException("se");
			}
			string text = se.Attribute("class");
			if (text == null || text.Length == 0)
			{
				throw new ArgumentException("class");
			}
			return PermissionBuilder.CreatePermission(text, se);
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x00093648 File Offset: 0x00091848
		public static IPermission Create(string fullname, SecurityElement se)
		{
			if (fullname == null)
			{
				throw new ArgumentNullException("fullname");
			}
			if (se == null)
			{
				throw new ArgumentNullException("se");
			}
			return PermissionBuilder.CreatePermission(fullname, se);
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x0009366D File Offset: 0x0009186D
		public static IPermission Create(Type type)
		{
			return (IPermission)Activator.CreateInstance(type, PermissionBuilder.psNone);
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x0009367F File Offset: 0x0009187F
		internal static IPermission CreatePermission(string fullname, SecurityElement se)
		{
			Type type = Type.GetType(fullname);
			if (type == null)
			{
				throw new TypeLoadException(string.Format(Locale.GetText("Can't create an instance of permission class {0}."), fullname));
			}
			IPermission permission = PermissionBuilder.Create(type);
			permission.FromXml(se);
			return permission;
		}

		// Token: 0x04001EC1 RID: 7873
		private static object[] psNone = new object[] { PermissionState.None };
	}
}
