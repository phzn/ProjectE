using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200002A RID: 42
	[NativeHeader("Modules/Physics/CharacterController.h")]
	public class CharacterController : Collider
	{
		// Token: 0x06000337 RID: 823 RVA: 0x000059FF File Offset: 0x00003BFF
		public bool SimpleMove(Vector3 speed)
		{
			return this.SimpleMove_Injected(ref speed);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00005A09 File Offset: 0x00003C09
		public CollisionFlags Move(Vector3 motion)
		{
			return this.Move_Injected(ref motion);
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00005A14 File Offset: 0x00003C14
		public Vector3 velocity
		{
			get
			{
				Vector3 vector;
				this.get_velocity_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600033A RID: 826
		public extern bool isGrounded
		{
			[NativeName("IsGrounded")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600033B RID: 827
		public extern CollisionFlags collisionFlags
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600033C RID: 828
		// (set) Token: 0x0600033D RID: 829
		public extern float radius
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600033E RID: 830
		// (set) Token: 0x0600033F RID: 831
		public extern float height
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00005A2C File Offset: 0x00003C2C
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00005A42 File Offset: 0x00003C42
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

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000342 RID: 834
		// (set) Token: 0x06000343 RID: 835
		public extern float slopeLimit
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000344 RID: 836
		// (set) Token: 0x06000345 RID: 837
		public extern float stepOffset
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000346 RID: 838
		// (set) Token: 0x06000347 RID: 839
		public extern float skinWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000348 RID: 840
		// (set) Token: 0x06000349 RID: 841
		public extern float minMoveDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600034A RID: 842
		// (set) Token: 0x0600034B RID: 843
		public extern bool detectCollisions
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600034C RID: 844
		// (set) Token: 0x0600034D RID: 845
		public extern bool enableOverlapRecovery
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600034F RID: 847
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool SimpleMove_Injected(ref Vector3 speed);

		// Token: 0x06000350 RID: 848
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern CollisionFlags Move_Injected(ref Vector3 motion);

		// Token: 0x06000351 RID: 849
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_velocity_Injected(out Vector3 ret);

		// Token: 0x06000352 RID: 850
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_center_Injected(out Vector3 ret);

		// Token: 0x06000353 RID: 851
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_center_Injected(ref Vector3 value);
	}
}
