using System;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x02000418 RID: 1048
	internal class MonoTrustManager : IApplicationTrustManager, ISecurityEncodable
	{
		// Token: 0x06002AC6 RID: 10950 RVA: 0x0009A7E4 File Offset: 0x000989E4
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public ApplicationTrust DetermineApplicationTrust(ActivationContext activationContext, TrustManagerContext context)
		{
			if (activationContext == null)
			{
				throw new ArgumentNullException("activationContext");
			}
			return null;
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x0009A7F5 File Offset: 0x000989F5
		public void FromXml(SecurityElement e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (e.Tag != "IApplicationTrustManager")
			{
				throw new ArgumentException("e", Locale.GetText("Invalid XML tag."));
			}
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x0009A82C File Offset: 0x00098A2C
		public SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IApplicationTrustManager");
			securityElement.AddAttribute("class", typeof(MonoTrustManager).AssemblyQualifiedName);
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x04001FA0 RID: 8096
		private const string tag = "IApplicationTrustManager";
	}
}
