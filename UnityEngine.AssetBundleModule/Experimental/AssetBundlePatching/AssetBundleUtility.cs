using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.AssetBundlePatching
{
	// Token: 0x0200000A RID: 10
	[NativeHeader("Modules/AssetBundle/Public/AssetBundlePatching.h")]
	public static class AssetBundleUtility
	{
		// Token: 0x06000066 RID: 102
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void PatchAssetBundles(AssetBundle[] bundles, string[] filenames);
	}
}
