using System;
using System.Collections.Generic;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x0200001A RID: 26
	public class PlatformMappingService
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00006B49 File Offset: 0x00004D49
		internal static AddressablesPlatform GetAddressablesPlatformInternal(RuntimePlatform platform)
		{
			if (PlatformMappingService.s_RuntimeTargetMapping.ContainsKey(platform))
			{
				return PlatformMappingService.s_RuntimeTargetMapping[platform];
			}
			return AddressablesPlatform.Unknown;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006B68 File Offset: 0x00004D68
		internal static string GetAddressablesPlatformPathInternal(RuntimePlatform platform)
		{
			if (PlatformMappingService.s_RuntimeTargetMapping.ContainsKey(platform))
			{
				return PlatformMappingService.s_RuntimeTargetMapping[platform].ToString();
			}
			return platform.ToString();
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00006BA9 File Offset: 0x00004DA9
		[Obsolete("This API doesn't adapt to the addition of new platforms.  Use GetPlatformPathSubFolder instead.")]
		public static AddressablesPlatform GetPlatform()
		{
			return PlatformMappingService.GetAddressablesPlatformInternal(Application.platform);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006BB5 File Offset: 0x00004DB5
		public static string GetPlatformPathSubFolder()
		{
			return PlatformMappingService.GetAddressablesPlatformPathInternal(Application.platform);
		}

		// Token: 0x04000049 RID: 73
		internal static readonly Dictionary<RuntimePlatform, AddressablesPlatform> s_RuntimeTargetMapping = new Dictionary<RuntimePlatform, AddressablesPlatform>
		{
			{
				RuntimePlatform.XboxOne,
				AddressablesPlatform.XboxOne
			},
			{
				RuntimePlatform.Switch,
				AddressablesPlatform.Switch
			},
			{
				RuntimePlatform.PS4,
				AddressablesPlatform.PS4
			},
			{
				RuntimePlatform.IPhonePlayer,
				AddressablesPlatform.iOS
			},
			{
				RuntimePlatform.Android,
				AddressablesPlatform.Android
			},
			{
				RuntimePlatform.WebGLPlayer,
				AddressablesPlatform.WebGL
			},
			{
				RuntimePlatform.WindowsPlayer,
				AddressablesPlatform.Windows
			},
			{
				RuntimePlatform.OSXPlayer,
				AddressablesPlatform.OSX
			},
			{
				RuntimePlatform.LinuxPlayer,
				AddressablesPlatform.Linux
			},
			{
				RuntimePlatform.WindowsEditor,
				AddressablesPlatform.Windows
			},
			{
				RuntimePlatform.OSXEditor,
				AddressablesPlatform.OSX
			},
			{
				RuntimePlatform.LinuxEditor,
				AddressablesPlatform.Linux
			},
			{
				RuntimePlatform.MetroPlayerARM,
				AddressablesPlatform.WindowsUniversal
			},
			{
				RuntimePlatform.MetroPlayerX64,
				AddressablesPlatform.WindowsUniversal
			},
			{
				RuntimePlatform.MetroPlayerX86,
				AddressablesPlatform.WindowsUniversal
			}
		};
	}
}
