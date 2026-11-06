using System;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace Microsoft.Win32
{
	/// <summary>Defines a credential policy to be used for resource requests that are made using <see cref="T:System.Net.WebRequest" /> and its derived classes.</summary>
	// Token: 0x0200011E RID: 286
	public class IntranetZoneCredentialPolicy : ICredentialPolicy
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.IntranetZoneCredentialPolicy" /> class.</summary>
		// Token: 0x060006C3 RID: 1731 RVA: 0x0000219B File Offset: 0x0000039B
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public IntranetZoneCredentialPolicy()
		{
		}

		/// <summary>Returns a <see cref="T:System.Boolean" /> that indicates whether the client's credentials are sent with a request for a resource that was made using <see cref="T:System.Net.WebRequest" />.</summary>
		/// <param name="challengeUri">The <see cref="T:System.Uri" /> that will receive the request.</param>
		/// <param name="request">The <see cref="T:System.Net.WebRequest" /> that represents the resource being requested.</param>
		/// <param name="credential">The <see cref="T:System.Net.NetworkCredential" /> that will be sent with the request if this method returns <see langword="true" />.</param>
		/// <param name="authModule">The <see cref="T:System.Net.IAuthenticationModule" /> that will conduct the authentication, if authentication is required.</param>
		/// <returns>
		///   <see langword="true" /> if the requested resource is in the same domain as the client making the request; otherwise, <see langword="false" />.</returns>
		// Token: 0x060006C4 RID: 1732 RVA: 0x0001367F File Offset: 0x0001187F
		public virtual bool ShouldSendCredential(Uri challengeUri, WebRequest request, NetworkCredential credential, IAuthenticationModule authModule)
		{
			return Zone.CreateFromUrl(challengeUri.AbsoluteUri).SecurityZone == SecurityZone.Intranet;
		}
	}
}
