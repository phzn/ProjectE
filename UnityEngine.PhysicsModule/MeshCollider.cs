using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200002B RID: 43
	[NativeHeader("Modules/Physics/MeshCollider.h")]
	[NativeHeader("Runtime/Graphics/Mesh/Mesh.h")]
	[RequiredByNativeCode]
	public class MeshCollider : Collider
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000354 RID: 852
		// (set) Token: 0x06000355 RID: 853
		public extern Mesh sharedMesh
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000356 RID: 854
		// (set) Token: 0x06000357 RID: 855
		public extern bool convex
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000358 RID: 856
		// (set) Token: 0x06000359 RID: 857
		public extern MeshColliderCookingOptions cookingOptions
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00005A58 File Offset: 0x00003C58
		// (set) Token: 0x0600035B RID: 859 RVA: 0x00002187 File Offset: 0x00000387
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Configuring smooth sphere collisions is no longer needed.", true)]
		public bool smoothSphereCollisions
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00005A6C File Offset: 0x00003C6C
		// (set) Token: 0x0600035D RID: 861 RVA: 0x00002187 File Offset: 0x00000387
		[Obsolete("MeshCollider.skinWidth is no longer used.")]
		public float skinWidth
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600035E RID: 862 RVA: 0x00005A84 File Offset: 0x00003C84
		// (set) Token: 0x0600035F RID: 863 RVA: 0x00002187 File Offset: 0x00000387
		[Obsolete("MeshCollider.inflateMesh is no longer supported. The new cooking algorithm doesn't need inflation to be used.")]
		public bool inflateMesh
		{
			get
			{
				return false;
			}
			set
			{
			}
		}
	}
}
