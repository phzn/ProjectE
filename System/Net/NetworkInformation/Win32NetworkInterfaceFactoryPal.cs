using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200073B RID: 1851
	internal static class Win32NetworkInterfaceFactoryPal
	{
		// Token: 0x06003B1C RID: 15132 RVA: 0x000CBCEC File Offset: 0x000C9EEC
		public static NetworkInterfaceFactory Create()
		{
			Version version = new Version(5, 1);
			if (Environment.OSVersion.Version >= version)
			{
				return new Win32NetworkInterfaceAPI();
			}
			return null;
		}
	}
}
