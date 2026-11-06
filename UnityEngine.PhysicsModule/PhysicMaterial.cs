using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000026 RID: 38
	[NativeHeader("Modules/Physics/PhysicMaterial.h")]
	public class PhysicMaterial : Object
	{
		// Token: 0x06000253 RID: 595 RVA: 0x0000502D File Offset: 0x0000322D
		public PhysicMaterial()
		{
			PhysicMaterial.Internal_CreateDynamicsMaterial(this, "DynamicMaterial");
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00005043 File Offset: 0x00003243
		public PhysicMaterial(string name)
		{
			PhysicMaterial.Internal_CreateDynamicsMaterial(this, name);
		}

		// Token: 0x06000255 RID: 597
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateDynamicsMaterial([Writable] PhysicMaterial mat, string name);

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000256 RID: 598
		// (set) Token: 0x06000257 RID: 599
		public extern float bounciness
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000258 RID: 600
		// (set) Token: 0x06000259 RID: 601
		public extern float dynamicFriction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600025A RID: 602
		// (set) Token: 0x0600025B RID: 603
		public extern float staticFriction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600025C RID: 604
		// (set) Token: 0x0600025D RID: 605
		public extern PhysicMaterialCombine frictionCombine
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600025E RID: 606
		// (set) Token: 0x0600025F RID: 607
		public extern PhysicMaterialCombine bounceCombine
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00005058 File Offset: 0x00003258
		// (set) Token: 0x06000261 RID: 609 RVA: 0x00005070 File Offset: 0x00003270
		[Obsolete("Use PhysicMaterial.bounciness instead (UnityUpgradable) -> bounciness")]
		public float bouncyness
		{
			get
			{
				return this.bounciness;
			}
			set
			{
				this.bounciness = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000507C File Offset: 0x0000327C
		// (set) Token: 0x06000263 RID: 611 RVA: 0x00002187 File Offset: 0x00000387
		[Obsolete("Anisotropic friction is no longer supported since Unity 5.0.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Vector3 frictionDirection2
		{
			get
			{
				return Vector3.zero;
			}
			set
			{
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00005094 File Offset: 0x00003294
		// (set) Token: 0x06000265 RID: 613 RVA: 0x00002187 File Offset: 0x00000387
		[Obsolete("Anisotropic friction is no longer supported since Unity 5.0.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float dynamicFriction2
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000266 RID: 614 RVA: 0x000050AC File Offset: 0x000032AC
		// (set) Token: 0x06000267 RID: 615 RVA: 0x00002187 File Offset: 0x00000387
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Anisotropic friction is no longer supported since Unity 5.0.", true)]
		public float staticFriction2
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000268 RID: 616 RVA: 0x000050C4 File Offset: 0x000032C4
		// (set) Token: 0x06000269 RID: 617 RVA: 0x00002187 File Offset: 0x00000387
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Anisotropic friction is no longer supported since Unity 5.0.", true)]
		public Vector3 frictionDirection
		{
			get
			{
				return Vector3.zero;
			}
			set
			{
			}
		}
	}
}
