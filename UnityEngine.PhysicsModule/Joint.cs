using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000030 RID: 48
	[RequireComponent(typeof(Rigidbody))]
	[NativeHeader("Modules/Physics/Joint.h")]
	[NativeClass("Unity::Joint")]
	public class Joint : Component
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000393 RID: 915
		// (set) Token: 0x06000394 RID: 916
		public extern Rigidbody connectedBody
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000395 RID: 917
		// (set) Token: 0x06000396 RID: 918
		public extern ArticulationBody connectedArticulationBody
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00005C00 File Offset: 0x00003E00
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00005C16 File Offset: 0x00003E16
		public Vector3 axis
		{
			get
			{
				Vector3 vector;
				this.get_axis_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_axis_Injected(ref value);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00005C20 File Offset: 0x00003E20
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00005C36 File Offset: 0x00003E36
		public Vector3 anchor
		{
			get
			{
				Vector3 vector;
				this.get_anchor_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_anchor_Injected(ref value);
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00005C40 File Offset: 0x00003E40
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00005C56 File Offset: 0x00003E56
		public Vector3 connectedAnchor
		{
			get
			{
				Vector3 vector;
				this.get_connectedAnchor_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_connectedAnchor_Injected(ref value);
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600039D RID: 925
		// (set) Token: 0x0600039E RID: 926
		public extern bool autoConfigureConnectedAnchor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600039F RID: 927
		// (set) Token: 0x060003A0 RID: 928
		public extern float breakForce
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060003A1 RID: 929
		// (set) Token: 0x060003A2 RID: 930
		public extern float breakTorque
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060003A3 RID: 931
		// (set) Token: 0x060003A4 RID: 932
		public extern bool enableCollision
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060003A5 RID: 933
		// (set) Token: 0x060003A6 RID: 934
		public extern bool enablePreprocessing
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060003A7 RID: 935
		// (set) Token: 0x060003A8 RID: 936
		public extern float massScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060003A9 RID: 937
		// (set) Token: 0x060003AA RID: 938
		public extern float connectedMassScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060003AB RID: 939
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetCurrentForces(ref Vector3 linearForce, ref Vector3 angularForce);

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00005C60 File Offset: 0x00003E60
		public Vector3 currentForce
		{
			get
			{
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				this.GetCurrentForces(ref zero, ref zero2);
				return zero;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00005C8C File Offset: 0x00003E8C
		public Vector3 currentTorque
		{
			get
			{
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				this.GetCurrentForces(ref zero, ref zero2);
				return zero2;
			}
		}

		// Token: 0x060003AF RID: 943
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_axis_Injected(out Vector3 ret);

		// Token: 0x060003B0 RID: 944
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_axis_Injected(ref Vector3 value);

		// Token: 0x060003B1 RID: 945
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_anchor_Injected(out Vector3 ret);

		// Token: 0x060003B2 RID: 946
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_anchor_Injected(ref Vector3 value);

		// Token: 0x060003B3 RID: 947
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_connectedAnchor_Injected(out Vector3 ret);

		// Token: 0x060003B4 RID: 948
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_connectedAnchor_Injected(ref Vector3 value);
	}
}
