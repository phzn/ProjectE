using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200002C RID: 44
	[NativeHeader("Modules/Physics2D/Joint2D.h")]
	[RequireComponent(typeof(Transform), typeof(Rigidbody2D))]
	public class Joint2D : Behaviour
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003F4 RID: 1012
		public extern Rigidbody2D attachedRigidbody
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003F5 RID: 1013
		// (set) Token: 0x060003F6 RID: 1014
		public extern Rigidbody2D connectedBody
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003F7 RID: 1015
		// (set) Token: 0x060003F8 RID: 1016
		public extern bool enableCollision
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003F9 RID: 1017
		// (set) Token: 0x060003FA RID: 1018
		public extern float breakForce
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003FB RID: 1019
		// (set) Token: 0x060003FC RID: 1020
		public extern float breakTorque
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060003FD RID: 1021
		// (set) Token: 0x060003FE RID: 1022
		public extern JointBreakAction2D breakAction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x00008720 File Offset: 0x00006920
		public Vector2 reactionForce
		{
			[NativeMethod("GetReactionForceFixedTime")]
			get
			{
				Vector2 vector;
				this.get_reactionForce_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000400 RID: 1024
		public extern float reactionTorque
		{
			[NativeMethod("GetReactionTorqueFixedTime")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00008738 File Offset: 0x00006938
		public Vector2 GetReactionForce(float timeStep)
		{
			Vector2 vector;
			this.GetReactionForce_Injected(timeStep, out vector);
			return vector;
		}

		// Token: 0x06000402 RID: 1026
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetReactionTorque(float timeStep);

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x00008750 File Offset: 0x00006950
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x00008768 File Offset: 0x00006968
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Joint2D.collideConnected has been deprecated. Use Joint2D.enableCollision instead (UnityUpgradable) -> enableCollision", true)]
		public bool collideConnected
		{
			get
			{
				return this.enableCollision;
			}
			set
			{
				this.enableCollision = value;
			}
		}

		// Token: 0x06000406 RID: 1030
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_reactionForce_Injected(out Vector2 ret);

		// Token: 0x06000407 RID: 1031
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetReactionForce_Injected(float timeStep, out Vector2 ret);
	}
}
