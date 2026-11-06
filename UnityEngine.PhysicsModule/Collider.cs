using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000029 RID: 41
	[NativeHeader("Modules/Physics/Collider.h")]
	[RequireComponent(typeof(Transform))]
	[RequiredByNativeCode]
	public class Collider : Component
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000312 RID: 786
		// (set) Token: 0x06000313 RID: 787
		public extern bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000314 RID: 788
		public extern Rigidbody attachedRigidbody
		{
			[NativeMethod("GetRigidbody")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000315 RID: 789
		public extern ArticulationBody attachedArticulationBody
		{
			[NativeMethod("GetArticulationBody")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000316 RID: 790
		// (set) Token: 0x06000317 RID: 791
		public extern bool isTrigger
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000318 RID: 792
		// (set) Token: 0x06000319 RID: 793
		public extern float contactOffset
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00005914 File Offset: 0x00003B14
		public Vector3 ClosestPoint(Vector3 position)
		{
			Vector3 vector;
			this.ClosestPoint_Injected(ref position, out vector);
			return vector;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000592C File Offset: 0x00003B2C
		public Bounds bounds
		{
			get
			{
				Bounds bounds;
				this.get_bounds_Injected(out bounds);
				return bounds;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600031C RID: 796
		// (set) Token: 0x0600031D RID: 797
		public extern bool hasModifiableContacts
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600031E RID: 798
		// (set) Token: 0x0600031F RID: 799
		public extern bool providesContacts
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000320 RID: 800
		// (set) Token: 0x06000321 RID: 801
		public extern int layerOverridePriority
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000322 RID: 802 RVA: 0x00005944 File Offset: 0x00003B44
		// (set) Token: 0x06000323 RID: 803 RVA: 0x0000595A File Offset: 0x00003B5A
		public LayerMask excludeLayers
		{
			get
			{
				LayerMask layerMask;
				this.get_excludeLayers_Injected(out layerMask);
				return layerMask;
			}
			set
			{
				this.set_excludeLayers_Injected(ref value);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000324 RID: 804 RVA: 0x00005964 File Offset: 0x00003B64
		// (set) Token: 0x06000325 RID: 805 RVA: 0x0000597A File Offset: 0x00003B7A
		public LayerMask includeLayers
		{
			get
			{
				LayerMask layerMask;
				this.get_includeLayers_Injected(out layerMask);
				return layerMask;
			}
			set
			{
				this.set_includeLayers_Injected(ref value);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000326 RID: 806
		// (set) Token: 0x06000327 RID: 807
		[NativeMethod("Material")]
		public extern PhysicMaterial sharedMaterial
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000328 RID: 808
		// (set) Token: 0x06000329 RID: 809
		public extern PhysicMaterial material
		{
			[NativeMethod("GetClonedMaterial")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeMethod("SetMaterial")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00005984 File Offset: 0x00003B84
		private RaycastHit Raycast(Ray ray, float maxDistance, ref bool hasHit)
		{
			RaycastHit raycastHit;
			this.Raycast_Injected(ref ray, maxDistance, ref hasHit, out raycastHit);
			return raycastHit;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x000059A0 File Offset: 0x00003BA0
		public bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
		{
			bool flag = false;
			hitInfo = this.Raycast(ray, maxDistance, ref flag);
			return flag;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x000059C5 File Offset: 0x00003BC5
		[NativeName("ClosestPointOnBounds")]
		private void Internal_ClosestPointOnBounds(Vector3 point, ref Vector3 outPos, ref float distance)
		{
			this.Internal_ClosestPointOnBounds_Injected(ref point, ref outPos, ref distance);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000059D4 File Offset: 0x00003BD4
		public Vector3 ClosestPointOnBounds(Vector3 position)
		{
			float num = 0f;
			Vector3 zero = Vector3.zero;
			this.Internal_ClosestPointOnBounds(position, ref zero, ref num);
			return zero;
		}

		// Token: 0x0600032F RID: 815
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ClosestPoint_Injected(ref Vector3 position, out Vector3 ret);

		// Token: 0x06000330 RID: 816
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_bounds_Injected(out Bounds ret);

		// Token: 0x06000331 RID: 817
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_excludeLayers_Injected(out LayerMask ret);

		// Token: 0x06000332 RID: 818
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_excludeLayers_Injected(ref LayerMask value);

		// Token: 0x06000333 RID: 819
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_includeLayers_Injected(out LayerMask ret);

		// Token: 0x06000334 RID: 820
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_includeLayers_Injected(ref LayerMask value);

		// Token: 0x06000335 RID: 821
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Raycast_Injected(ref Ray ray, float maxDistance, ref bool hasHit, out RaycastHit ret);

		// Token: 0x06000336 RID: 822
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_ClosestPointOnBounds_Injected(ref Vector3 point, ref Vector3 outPos, ref float distance);
	}
}
