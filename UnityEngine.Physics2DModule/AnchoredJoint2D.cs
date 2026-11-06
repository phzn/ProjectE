using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200002D RID: 45
	[NativeHeader("Modules/Physics2D/AnchoredJoint2D.h")]
	public class AnchoredJoint2D : Joint2D
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00008774 File Offset: 0x00006974
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x0000878A File Offset: 0x0000698A
		public Vector2 anchor
		{
			get
			{
				Vector2 vector;
				this.get_anchor_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_anchor_Injected(ref value);
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00008794 File Offset: 0x00006994
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x000087AA File Offset: 0x000069AA
		public Vector2 connectedAnchor
		{
			get
			{
				Vector2 vector;
				this.get_connectedAnchor_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_connectedAnchor_Injected(ref value);
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600040C RID: 1036
		// (set) Token: 0x0600040D RID: 1037
		public extern bool autoConfigureConnectedAnchor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600040F RID: 1039
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_anchor_Injected(out Vector2 ret);

		// Token: 0x06000410 RID: 1040
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_anchor_Injected(ref Vector2 value);

		// Token: 0x06000411 RID: 1041
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_connectedAnchor_Injected(out Vector2 ret);

		// Token: 0x06000412 RID: 1042
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_connectedAnchor_Injected(ref Vector2 value);
	}
}
