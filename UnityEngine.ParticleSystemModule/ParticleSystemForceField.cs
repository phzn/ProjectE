using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200005D RID: 93
	[NativeHeader("Modules/ParticleSystem/ParticleSystemForceField.h")]
	[RequireComponent(typeof(Transform))]
	[NativeHeader("ParticleSystemScriptingClasses.h")]
	[NativeHeader("Modules/ParticleSystem/ScriptBindings/ParticleSystemScriptBindings.h")]
	[NativeHeader("Modules/ParticleSystem/ParticleSystemForceFieldManager.h")]
	[NativeHeader("Modules/ParticleSystem/ParticleSystem.h")]
	public class ParticleSystemForceField : Behaviour
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000716 RID: 1814
		// (set) Token: 0x06000717 RID: 1815
		[NativeName("ForceShape")]
		public extern ParticleSystemForceFieldShape shape
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000718 RID: 1816
		// (set) Token: 0x06000719 RID: 1817
		public extern float startRange
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600071A RID: 1818
		// (set) Token: 0x0600071B RID: 1819
		public extern float endRange
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600071C RID: 1820
		// (set) Token: 0x0600071D RID: 1821
		public extern float length
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600071E RID: 1822
		// (set) Token: 0x0600071F RID: 1823
		public extern float gravityFocus
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0000662C File Offset: 0x0000482C
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x00006642 File Offset: 0x00004842
		public Vector2 rotationRandomness
		{
			get
			{
				Vector2 vector;
				this.get_rotationRandomness_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_rotationRandomness_Injected(ref value);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000722 RID: 1826
		// (set) Token: 0x06000723 RID: 1827
		public extern bool multiplyDragByParticleSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000724 RID: 1828
		// (set) Token: 0x06000725 RID: 1829
		public extern bool multiplyDragByParticleVelocity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000726 RID: 1830
		// (set) Token: 0x06000727 RID: 1831
		public extern Texture3D vectorField
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0000664C File Offset: 0x0000484C
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x00006662 File Offset: 0x00004862
		public ParticleSystem.MinMaxCurve directionX
		{
			get
			{
				ParticleSystem.MinMaxCurve minMaxCurve;
				this.get_directionX_Injected(out minMaxCurve);
				return minMaxCurve;
			}
			set
			{
				this.set_directionX_Injected(ref value);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0000666C File Offset: 0x0000486C
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x00006682 File Offset: 0x00004882
		public ParticleSystem.MinMaxCurve directionY
		{
			get
			{
				ParticleSystem.MinMaxCurve minMaxCurve;
				this.get_directionY_Injected(out minMaxCurve);
				return minMaxCurve;
			}
			set
			{
				this.set_directionY_Injected(ref value);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0000668C File Offset: 0x0000488C
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x000066A2 File Offset: 0x000048A2
		public ParticleSystem.MinMaxCurve directionZ
		{
			get
			{
				ParticleSystem.MinMaxCurve minMaxCurve;
				this.get_directionZ_Injected(out minMaxCurve);
				return minMaxCurve;
			}
			set
			{
				this.set_directionZ_Injected(ref value);
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x000066AC File Offset: 0x000048AC
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x000066C2 File Offset: 0x000048C2
		public ParticleSystem.MinMaxCurve gravity
		{
			get
			{
				ParticleSystem.MinMaxCurve minMaxCurve;
				this.get_gravity_Injected(out minMaxCurve);
				return minMaxCurve;
			}
			set
			{
				this.set_gravity_Injected(ref value);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x000066CC File Offset: 0x000048CC
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x000066E2 File Offset: 0x000048E2
		public ParticleSystem.MinMaxCurve rotationSpeed
		{
			get
			{
				ParticleSystem.MinMaxCurve minMaxCurve;
				this.get_rotationSpeed_Injected(out minMaxCurve);
				return minMaxCurve;
			}
			set
			{
				this.set_rotationSpeed_Injected(ref value);
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x000066EC File Offset: 0x000048EC
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x00006702 File Offset: 0x00004902
		public ParticleSystem.MinMaxCurve rotationAttraction
		{
			get
			{
				ParticleSystem.MinMaxCurve minMaxCurve;
				this.get_rotationAttraction_Injected(out minMaxCurve);
				return minMaxCurve;
			}
			set
			{
				this.set_rotationAttraction_Injected(ref value);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x0000670C File Offset: 0x0000490C
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x00006722 File Offset: 0x00004922
		public ParticleSystem.MinMaxCurve drag
		{
			get
			{
				ParticleSystem.MinMaxCurve minMaxCurve;
				this.get_drag_Injected(out minMaxCurve);
				return minMaxCurve;
			}
			set
			{
				this.set_drag_Injected(ref value);
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x0000672C File Offset: 0x0000492C
		// (set) Token: 0x06000737 RID: 1847 RVA: 0x00006742 File Offset: 0x00004942
		public ParticleSystem.MinMaxCurve vectorFieldSpeed
		{
			get
			{
				ParticleSystem.MinMaxCurve minMaxCurve;
				this.get_vectorFieldSpeed_Injected(out minMaxCurve);
				return minMaxCurve;
			}
			set
			{
				this.set_vectorFieldSpeed_Injected(ref value);
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x0000674C File Offset: 0x0000494C
		// (set) Token: 0x06000739 RID: 1849 RVA: 0x00006762 File Offset: 0x00004962
		public ParticleSystem.MinMaxCurve vectorFieldAttraction
		{
			get
			{
				ParticleSystem.MinMaxCurve minMaxCurve;
				this.get_vectorFieldAttraction_Injected(out minMaxCurve);
				return minMaxCurve;
			}
			set
			{
				this.set_vectorFieldAttraction_Injected(ref value);
			}
		}

		// Token: 0x0600073B RID: 1851
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_rotationRandomness_Injected(out Vector2 ret);

		// Token: 0x0600073C RID: 1852
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_rotationRandomness_Injected(ref Vector2 value);

		// Token: 0x0600073D RID: 1853
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_directionX_Injected(out ParticleSystem.MinMaxCurve ret);

		// Token: 0x0600073E RID: 1854
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_directionX_Injected(ref ParticleSystem.MinMaxCurve value);

		// Token: 0x0600073F RID: 1855
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_directionY_Injected(out ParticleSystem.MinMaxCurve ret);

		// Token: 0x06000740 RID: 1856
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_directionY_Injected(ref ParticleSystem.MinMaxCurve value);

		// Token: 0x06000741 RID: 1857
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_directionZ_Injected(out ParticleSystem.MinMaxCurve ret);

		// Token: 0x06000742 RID: 1858
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_directionZ_Injected(ref ParticleSystem.MinMaxCurve value);

		// Token: 0x06000743 RID: 1859
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_gravity_Injected(out ParticleSystem.MinMaxCurve ret);

		// Token: 0x06000744 RID: 1860
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_gravity_Injected(ref ParticleSystem.MinMaxCurve value);

		// Token: 0x06000745 RID: 1861
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_rotationSpeed_Injected(out ParticleSystem.MinMaxCurve ret);

		// Token: 0x06000746 RID: 1862
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_rotationSpeed_Injected(ref ParticleSystem.MinMaxCurve value);

		// Token: 0x06000747 RID: 1863
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_rotationAttraction_Injected(out ParticleSystem.MinMaxCurve ret);

		// Token: 0x06000748 RID: 1864
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_rotationAttraction_Injected(ref ParticleSystem.MinMaxCurve value);

		// Token: 0x06000749 RID: 1865
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_drag_Injected(out ParticleSystem.MinMaxCurve ret);

		// Token: 0x0600074A RID: 1866
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_drag_Injected(ref ParticleSystem.MinMaxCurve value);

		// Token: 0x0600074B RID: 1867
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_vectorFieldSpeed_Injected(out ParticleSystem.MinMaxCurve ret);

		// Token: 0x0600074C RID: 1868
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_vectorFieldSpeed_Injected(ref ParticleSystem.MinMaxCurve value);

		// Token: 0x0600074D RID: 1869
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_vectorFieldAttraction_Injected(out ParticleSystem.MinMaxCurve ret);

		// Token: 0x0600074E RID: 1870
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_vectorFieldAttraction_Injected(ref ParticleSystem.MinMaxCurve value);
	}
}
