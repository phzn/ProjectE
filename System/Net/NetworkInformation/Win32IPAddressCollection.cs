using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000723 RID: 1827
	internal class Win32IPAddressCollection : IPAddressCollection
	{
		// Token: 0x06003A53 RID: 14931 RVA: 0x000CA718 File Offset: 0x000C8918
		private Win32IPAddressCollection()
		{
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x000CA720 File Offset: 0x000C8920
		public Win32IPAddressCollection(params IntPtr[] heads)
		{
			foreach (IntPtr intPtr in heads)
			{
				this.AddSubsequentlyString(intPtr);
			}
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x000CA750 File Offset: 0x000C8950
		public Win32IPAddressCollection(params Win32_IP_ADDR_STRING[] al)
		{
			foreach (Win32_IP_ADDR_STRING win32_IP_ADDR_STRING in al)
			{
				if (!string.IsNullOrEmpty(win32_IP_ADDR_STRING.IpAddress))
				{
					base.InternalAdd(IPAddress.Parse(win32_IP_ADDR_STRING.IpAddress));
					this.AddSubsequentlyString(win32_IP_ADDR_STRING.Next);
				}
			}
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x000CA7A8 File Offset: 0x000C89A8
		public static Win32IPAddressCollection FromAnycast(IntPtr ptr)
		{
			Win32IPAddressCollection win32IPAddressCollection = new Win32IPAddressCollection();
			IntPtr intPtr = ptr;
			while (intPtr != IntPtr.Zero)
			{
				Win32_IP_ADAPTER_ANYCAST_ADDRESS win32_IP_ADAPTER_ANYCAST_ADDRESS = (Win32_IP_ADAPTER_ANYCAST_ADDRESS)Marshal.PtrToStructure(intPtr, typeof(Win32_IP_ADAPTER_ANYCAST_ADDRESS));
				win32IPAddressCollection.InternalAdd(win32_IP_ADAPTER_ANYCAST_ADDRESS.Address.GetIPAddress());
				intPtr = win32_IP_ADAPTER_ANYCAST_ADDRESS.Next;
			}
			return win32IPAddressCollection;
		}

		// Token: 0x06003A57 RID: 14935 RVA: 0x000CA7FC File Offset: 0x000C89FC
		public static Win32IPAddressCollection FromDnsServer(IntPtr ptr)
		{
			Win32IPAddressCollection win32IPAddressCollection = new Win32IPAddressCollection();
			IntPtr intPtr = ptr;
			while (intPtr != IntPtr.Zero)
			{
				Win32_IP_ADAPTER_DNS_SERVER_ADDRESS win32_IP_ADAPTER_DNS_SERVER_ADDRESS = (Win32_IP_ADAPTER_DNS_SERVER_ADDRESS)Marshal.PtrToStructure(intPtr, typeof(Win32_IP_ADAPTER_DNS_SERVER_ADDRESS));
				win32IPAddressCollection.InternalAdd(win32_IP_ADAPTER_DNS_SERVER_ADDRESS.Address.GetIPAddress());
				intPtr = win32_IP_ADAPTER_DNS_SERVER_ADDRESS.Next;
			}
			return win32IPAddressCollection;
		}

		// Token: 0x06003A58 RID: 14936 RVA: 0x000CA850 File Offset: 0x000C8A50
		public static Win32IPAddressCollection FromSocketAddress(Win32_SOCKET_ADDRESS addr)
		{
			Win32IPAddressCollection win32IPAddressCollection = new Win32IPAddressCollection();
			if (addr.Sockaddr != IntPtr.Zero)
			{
				win32IPAddressCollection.InternalAdd(addr.GetIPAddress());
			}
			return win32IPAddressCollection;
		}

		// Token: 0x06003A59 RID: 14937 RVA: 0x000CA884 File Offset: 0x000C8A84
		public static Win32IPAddressCollection FromWinsServer(IntPtr ptr)
		{
			Win32IPAddressCollection win32IPAddressCollection = new Win32IPAddressCollection();
			IntPtr intPtr = ptr;
			while (intPtr != IntPtr.Zero)
			{
				Win32_IP_ADAPTER_WINS_SERVER_ADDRESS win32_IP_ADAPTER_WINS_SERVER_ADDRESS = (Win32_IP_ADAPTER_WINS_SERVER_ADDRESS)Marshal.PtrToStructure(intPtr, typeof(Win32_IP_ADAPTER_WINS_SERVER_ADDRESS));
				win32IPAddressCollection.InternalAdd(win32_IP_ADAPTER_WINS_SERVER_ADDRESS.Address.GetIPAddress());
				intPtr = win32_IP_ADAPTER_WINS_SERVER_ADDRESS.Next;
			}
			return win32IPAddressCollection;
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x000CA8D8 File Offset: 0x000C8AD8
		private void AddSubsequentlyString(IntPtr head)
		{
			IntPtr intPtr = head;
			while (intPtr != IntPtr.Zero)
			{
				Win32_IP_ADDR_STRING win32_IP_ADDR_STRING = (Win32_IP_ADDR_STRING)Marshal.PtrToStructure(intPtr, typeof(Win32_IP_ADDR_STRING));
				base.InternalAdd(IPAddress.Parse(win32_IP_ADDR_STRING.IpAddress));
				intPtr = win32_IP_ADDR_STRING.Next;
			}
		}

		// Token: 0x04002246 RID: 8774
		public static readonly Win32IPAddressCollection Empty = new Win32IPAddressCollection(new IntPtr[] { IntPtr.Zero });
	}
}
