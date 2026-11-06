using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000018 RID: 24
	[NativeHeader("TerrainScriptingClasses.h")]
	[NativeHeader("Modules/Terrain/Public/TerrainLayerScriptingInterface.h")]
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class TerrainLayer : Object
	{
		// Token: 0x06000180 RID: 384 RVA: 0x0000441D File Offset: 0x0000261D
		public TerrainLayer()
		{
			TerrainLayer.Internal_Create(this);
		}

		// Token: 0x06000181 RID: 385
		[FreeFunction("TerrainLayerScriptingInterface::Create")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] TerrainLayer layer);

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000182 RID: 386
		// (set) Token: 0x06000183 RID: 387
		public extern Texture2D diffuseTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000184 RID: 388
		// (set) Token: 0x06000185 RID: 389
		public extern Texture2D normalMapTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000186 RID: 390
		// (set) Token: 0x06000187 RID: 391
		public extern Texture2D maskMapTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00004430 File Offset: 0x00002630
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00004446 File Offset: 0x00002646
		public Vector2 tileSize
		{
			get
			{
				Vector2 vector;
				this.get_tileSize_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_tileSize_Injected(ref value);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00004450 File Offset: 0x00002650
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00004466 File Offset: 0x00002666
		public Vector2 tileOffset
		{
			get
			{
				Vector2 vector;
				this.get_tileOffset_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_tileOffset_Injected(ref value);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00004470 File Offset: 0x00002670
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00004486 File Offset: 0x00002686
		[NativeProperty("SpecularColor")]
		public Color specular
		{
			get
			{
				Color color;
				this.get_specular_Injected(out color);
				return color;
			}
			set
			{
				this.set_specular_Injected(ref value);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600018E RID: 398
		// (set) Token: 0x0600018F RID: 399
		public extern float metallic
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000190 RID: 400
		// (set) Token: 0x06000191 RID: 401
		public extern float smoothness
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000192 RID: 402
		// (set) Token: 0x06000193 RID: 403
		public extern float normalScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00004490 File Offset: 0x00002690
		// (set) Token: 0x06000195 RID: 405 RVA: 0x000044A6 File Offset: 0x000026A6
		public Vector4 diffuseRemapMin
		{
			get
			{
				Vector4 vector;
				this.get_diffuseRemapMin_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_diffuseRemapMin_Injected(ref value);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000044B0 File Offset: 0x000026B0
		// (set) Token: 0x06000197 RID: 407 RVA: 0x000044C6 File Offset: 0x000026C6
		public Vector4 diffuseRemapMax
		{
			get
			{
				Vector4 vector;
				this.get_diffuseRemapMax_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_diffuseRemapMax_Injected(ref value);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000044D0 File Offset: 0x000026D0
		// (set) Token: 0x06000199 RID: 409 RVA: 0x000044E6 File Offset: 0x000026E6
		public Vector4 maskMapRemapMin
		{
			get
			{
				Vector4 vector;
				this.get_maskMapRemapMin_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_maskMapRemapMin_Injected(ref value);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000044F0 File Offset: 0x000026F0
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00004506 File Offset: 0x00002706
		public Vector4 maskMapRemapMax
		{
			get
			{
				Vector4 vector;
				this.get_maskMapRemapMax_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_maskMapRemapMax_Injected(ref value);
			}
		}

		// Token: 0x0600019C RID: 412
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_tileSize_Injected(out Vector2 ret);

		// Token: 0x0600019D RID: 413
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_tileSize_Injected(ref Vector2 value);

		// Token: 0x0600019E RID: 414
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_tileOffset_Injected(out Vector2 ret);

		// Token: 0x0600019F RID: 415
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_tileOffset_Injected(ref Vector2 value);

		// Token: 0x060001A0 RID: 416
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_specular_Injected(out Color ret);

		// Token: 0x060001A1 RID: 417
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_specular_Injected(ref Color value);

		// Token: 0x060001A2 RID: 418
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_diffuseRemapMin_Injected(out Vector4 ret);

		// Token: 0x060001A3 RID: 419
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_diffuseRemapMin_Injected(ref Vector4 value);

		// Token: 0x060001A4 RID: 420
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_diffuseRemapMax_Injected(out Vector4 ret);

		// Token: 0x060001A5 RID: 421
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_diffuseRemapMax_Injected(ref Vector4 value);

		// Token: 0x060001A6 RID: 422
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_maskMapRemapMin_Injected(out Vector4 ret);

		// Token: 0x060001A7 RID: 423
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_maskMapRemapMin_Injected(ref Vector4 value);

		// Token: 0x060001A8 RID: 424
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_maskMapRemapMax_Injected(out Vector4 ret);

		// Token: 0x060001A9 RID: 425
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_maskMapRemapMax_Injected(ref Vector4 value);
	}
}
