using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Security.Claims
{
	// Token: 0x020004F9 RID: 1273
	[ComVisible(false)]
	internal class RoleClaimProvider
	{
		// Token: 0x0600330B RID: 13067 RVA: 0x000BC3B7 File Offset: 0x000BA5B7
		public RoleClaimProvider(string issuer, string[] roles, ClaimsIdentity subject)
		{
			this.m_issuer = issuer;
			this.m_roles = roles;
			this.m_subject = subject;
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x0600330C RID: 13068 RVA: 0x000BC3D4 File Offset: 0x000BA5D4
		public IEnumerable<Claim> Claims
		{
			get
			{
				int num;
				for (int i = 0; i < this.m_roles.Length; i = num + 1)
				{
					if (this.m_roles[i] != null)
					{
						yield return new Claim(this.m_subject.RoleClaimType, this.m_roles[i], "http://www.w3.org/2001/XMLSchema#string", this.m_issuer, this.m_issuer, this.m_subject);
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x040023F0 RID: 9200
		private string m_issuer;

		// Token: 0x040023F1 RID: 9201
		private string[] m_roles;

		// Token: 0x040023F2 RID: 9202
		private ClaimsIdentity m_subject;
	}
}
