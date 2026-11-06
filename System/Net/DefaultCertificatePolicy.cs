using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x02000673 RID: 1651
	internal class DefaultCertificatePolicy : ICertificatePolicy
	{
		// Token: 0x060033F2 RID: 13298 RVA: 0x000B51E1 File Offset: 0x000B33E1
		public bool CheckValidationResult(ServicePoint point, X509Certificate certificate, WebRequest request, int certificateProblem)
		{
			return ServicePointManager.ServerCertificateValidationCallback != null || (certificateProblem == -2146762495 || certificateProblem == 0);
		}
	}
}
