using System;
using System.ComponentModel;
using Unity;

namespace System.Net.NetworkInformation
{
	/// <summary>Allows applications to receive notification when the Internet Protocol (IP) address of a network interface, also called a network card or adapter, changes.</summary>
	// Token: 0x02000710 RID: 1808
	public sealed class NetworkChange
	{
		/// <summary>Occurs when the IP address of a network interface changes.</summary>
		// Token: 0x1400006D RID: 109
		// (add) Token: 0x060039D0 RID: 14800 RVA: 0x000C8F48 File Offset: 0x000C7148
		// (remove) Token: 0x060039D1 RID: 14801 RVA: 0x000C8FA0 File Offset: 0x000C71A0
		public static event NetworkAddressChangedEventHandler NetworkAddressChanged
		{
			add
			{
				Type typeFromHandle = typeof(INetworkChange);
				lock (typeFromHandle)
				{
					NetworkChange.MaybeCreate();
					if (NetworkChange.networkChange != null)
					{
						NetworkChange.networkChange.NetworkAddressChanged += value;
					}
				}
			}
			remove
			{
				Type typeFromHandle = typeof(INetworkChange);
				lock (typeFromHandle)
				{
					if (NetworkChange.networkChange != null)
					{
						NetworkChange.networkChange.NetworkAddressChanged -= value;
						NetworkChange.MaybeDispose();
					}
				}
			}
		}

		/// <summary>Occurs when the availability of the network changes.</summary>
		// Token: 0x1400006E RID: 110
		// (add) Token: 0x060039D2 RID: 14802 RVA: 0x000C8FF8 File Offset: 0x000C71F8
		// (remove) Token: 0x060039D3 RID: 14803 RVA: 0x000C9050 File Offset: 0x000C7250
		public static event NetworkAvailabilityChangedEventHandler NetworkAvailabilityChanged
		{
			add
			{
				Type typeFromHandle = typeof(INetworkChange);
				lock (typeFromHandle)
				{
					NetworkChange.MaybeCreate();
					if (NetworkChange.networkChange != null)
					{
						NetworkChange.networkChange.NetworkAvailabilityChanged += value;
					}
				}
			}
			remove
			{
				Type typeFromHandle = typeof(INetworkChange);
				lock (typeFromHandle)
				{
					if (NetworkChange.networkChange != null)
					{
						NetworkChange.networkChange.NetworkAvailabilityChanged -= value;
						NetworkChange.MaybeDispose();
					}
				}
			}
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x000C90A8 File Offset: 0x000C72A8
		private static void MaybeCreate()
		{
			if (NetworkChange.networkChange != null)
			{
				return;
			}
			if (NetworkChange.IsWindows)
			{
				throw new PlatformNotSupportedException("NetworkInformation.NetworkChange is not supported on the current platform.");
			}
			try
			{
				NetworkChange.networkChange = new MacNetworkChange();
			}
			catch
			{
				NetworkChange.networkChange = new LinuxNetworkChange();
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x060039D5 RID: 14805 RVA: 0x000C90FC File Offset: 0x000C72FC
		private static bool IsWindows
		{
			get
			{
				PlatformID platform = Environment.OSVersion.Platform;
				return platform == PlatformID.Win32S || platform == PlatformID.Win32Windows || platform == PlatformID.Win32NT || platform == PlatformID.WinCE;
			}
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x000C9126 File Offset: 0x000C7326
		private static void MaybeDispose()
		{
			if (NetworkChange.networkChange != null && NetworkChange.networkChange.HasRegisteredEvents)
			{
				NetworkChange.networkChange.Dispose();
				NetworkChange.networkChange = null;
			}
		}

		/// <summary>Registers a network change instance to receive network change events.</summary>
		/// <param name="nc">The instance to register.</param>
		// Token: 0x060039D8 RID: 14808 RVA: 0x00013BCA File Offset: 0x00011DCA
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RegisterNetworkChange(NetworkChange nc)
		{
			global::Unity.ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04002200 RID: 8704
		private static INetworkChange networkChange;
	}
}
