using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200071A RID: 1818
	internal static class NetworkInterfaceFactoryPal
	{
		// Token: 0x06003A17 RID: 14871 RVA: 0x000C99E8 File Offset: 0x000C7BE8
		public static NetworkInterfaceFactory Create()
		{
			NetworkInterfaceFactory networkInterfaceFactory = UnixNetworkInterfaceFactoryPal.Create();
			if (networkInterfaceFactory == null)
			{
				networkInterfaceFactory = Win32NetworkInterfaceFactoryPal.Create();
			}
			if (networkInterfaceFactory == null)
			{
				throw new NotImplementedException();
			}
			return networkInterfaceFactory;
		}
	}
}
