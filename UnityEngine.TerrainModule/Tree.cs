using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000007 RID: 7
	[NativeHeader("Modules/Terrain/Public/Tree.h")]
	[ExcludeFromPreset]
	public sealed class Tree : Component
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000080 RID: 128
		// (set) Token: 0x06000081 RID: 129
		[NativeProperty("TreeData")]
		public extern ScriptableObject data
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000082 RID: 130
		public extern bool hasSpeedTreeWind
		{
			[NativeMethod("HasSpeedTreeWind")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
