using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000009 RID: 9
	[RequiredByNativeCode]
	[NativeHeader("Modules/AssetBundle/Public/AssetBundleUnloadOperation.h")]
	[StructLayout(LayoutKind.Sequential)]
	public class AssetBundleUnloadOperation : AsyncOperation
	{
		// Token: 0x06000064 RID: 100
		[NativeMethod("WaitForCompletion")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WaitForCompletion();
	}
}
