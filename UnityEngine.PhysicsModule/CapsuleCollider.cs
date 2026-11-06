using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200002C RID: 44
	[NativeHeader("Modules/Physics/CapsuleCollider.h")]
	[RequiredByNativeCode]
	public class CapsuleCollider : Collider
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00005A98 File Offset: 0x00003C98
		// (set) Token: 0x06000362 RID: 866 RVA: 0x00005AAE File Offset: 0x00003CAE
		public Vector3 center
		{
			get
			{
				Vector3 vector;
				this.get_center_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_center_Injected(ref value);
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000363 RID: 867
		// (set) Token: 0x06000364 RID: 868
		public extern float radius
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000365 RID: 869
		// (set) Token: 0x06000366 RID: 870
		public extern float height
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000367 RID: 871
		// (set) Token: 0x06000368 RID: 872
		public extern int direction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00005AB8 File Offset: 0x00003CB8
		internal Vector2 GetGlobalExtents()
		{
			Vector2 vector;
			this.GetGlobalExtents_Injected(out vector);
			return vector;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00005AD0 File Offset: 0x00003CD0
		internal Matrix4x4 CalculateTransform()
		{
			Matrix4x4 matrix4x;
			this.CalculateTransform_Injected(out matrix4x);
			return matrix4x;
		}

		// Token: 0x0600036C RID: 876
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_center_Injected(out Vector3 ret);

		// Token: 0x0600036D RID: 877
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_center_Injected(ref Vector3 value);

		// Token: 0x0600036E RID: 878
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetGlobalExtents_Injected(out Vector2 ret);

		// Token: 0x0600036F RID: 879
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CalculateTransform_Injected(out Matrix4x4 ret);
	}
}
