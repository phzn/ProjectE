using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.AI
{
	// Token: 0x02000012 RID: 18
	[NativeHeader("Modules/AI/NavMesh/NavMesh.bindings.h")]
	public sealed class NavMeshData : Object
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00002C13 File Offset: 0x00000E13
		public NavMeshData()
		{
			NavMeshData.Internal_Create(this, 0);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00002C25 File Offset: 0x00000E25
		public NavMeshData(int agentTypeID)
		{
			NavMeshData.Internal_Create(this, agentTypeID);
		}

		// Token: 0x060000FB RID: 251
		[StaticAccessor("NavMeshDataBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] NavMeshData mono, int agentTypeID);

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00002C38 File Offset: 0x00000E38
		public Bounds sourceBounds
		{
			get
			{
				Bounds bounds;
				this.get_sourceBounds_Injected(out bounds);
				return bounds;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00002C50 File Offset: 0x00000E50
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00002C66 File Offset: 0x00000E66
		public Vector3 position
		{
			get
			{
				Vector3 vector;
				this.get_position_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_position_Injected(ref value);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00002C70 File Offset: 0x00000E70
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00002C86 File Offset: 0x00000E86
		public Quaternion rotation
		{
			get
			{
				Quaternion quaternion;
				this.get_rotation_Injected(out quaternion);
				return quaternion;
			}
			set
			{
				this.set_rotation_Injected(ref value);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000101 RID: 257
		internal extern bool hasHeightMeshData
		{
			[NativeMethod("HasHeightMeshData")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00002C90 File Offset: 0x00000E90
		internal NavMeshBuildSettings buildSettings
		{
			get
			{
				NavMeshBuildSettings navMeshBuildSettings;
				this.get_buildSettings_Injected(out navMeshBuildSettings);
				return navMeshBuildSettings;
			}
		}

		// Token: 0x06000103 RID: 259
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_sourceBounds_Injected(out Bounds ret);

		// Token: 0x06000104 RID: 260
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_position_Injected(out Vector3 ret);

		// Token: 0x06000105 RID: 261
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_position_Injected(ref Vector3 value);

		// Token: 0x06000106 RID: 262
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_rotation_Injected(out Quaternion ret);

		// Token: 0x06000107 RID: 263
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_rotation_Injected(ref Quaternion value);

		// Token: 0x06000108 RID: 264
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_buildSettings_Injected(out NavMeshBuildSettings ret);
	}
}
