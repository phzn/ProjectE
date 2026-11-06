using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x02000023 RID: 35
	[NativeHeader("Runtime/Video/ScriptBindings/WebCamTexture.bindings.h")]
	[NativeHeader("Runtime/Video/BaseWebCamTexture.h")]
	[NativeHeader("AudioScriptingClasses.h")]
	public sealed class WebCamTexture : Texture
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600015A RID: 346
		public static extern WebCamDevice[] devices
		{
			[StaticAccessor("WebCamTextureBindings", StaticAccessorType.DoubleColon)]
			[NativeName("Internal_GetDevices")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00002E7C File Offset: 0x0000107C
		public WebCamTexture(string deviceName, int requestedWidth, int requestedHeight, int requestedFPS)
		{
			WebCamTexture.Internal_CreateWebCamTexture(this, deviceName, requestedWidth, requestedHeight, requestedFPS);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00002E92 File Offset: 0x00001092
		public WebCamTexture(string deviceName, int requestedWidth, int requestedHeight)
		{
			WebCamTexture.Internal_CreateWebCamTexture(this, deviceName, requestedWidth, requestedHeight, 0);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00002EA7 File Offset: 0x000010A7
		public WebCamTexture(string deviceName)
		{
			WebCamTexture.Internal_CreateWebCamTexture(this, deviceName, 0, 0, 0);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00002EBC File Offset: 0x000010BC
		public WebCamTexture(int requestedWidth, int requestedHeight, int requestedFPS)
		{
			WebCamTexture.Internal_CreateWebCamTexture(this, "", requestedWidth, requestedHeight, requestedFPS);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00002ED5 File Offset: 0x000010D5
		public WebCamTexture(int requestedWidth, int requestedHeight)
		{
			WebCamTexture.Internal_CreateWebCamTexture(this, "", requestedWidth, requestedHeight, 0);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00002EEE File Offset: 0x000010EE
		public WebCamTexture()
		{
			WebCamTexture.Internal_CreateWebCamTexture(this, "", 0, 0, 0);
		}

		// Token: 0x06000161 RID: 353
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Play();

		// Token: 0x06000162 RID: 354
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Pause();

		// Token: 0x06000163 RID: 355
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Stop();

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000164 RID: 356
		public extern bool isPlaying
		{
			[NativeName("IsPlaying")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000165 RID: 357
		// (set) Token: 0x06000166 RID: 358
		[NativeName("Device")]
		public extern string deviceName
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000167 RID: 359
		// (set) Token: 0x06000168 RID: 360
		public extern float requestedFPS
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000169 RID: 361
		// (set) Token: 0x0600016A RID: 362
		public extern int requestedWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600016B RID: 363
		// (set) Token: 0x0600016C RID: 364
		public extern int requestedHeight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600016D RID: 365
		public extern int videoRotationAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600016E RID: 366
		public extern bool videoVerticallyMirrored
		{
			[NativeName("IsVideoVerticallyMirrored")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600016F RID: 367
		public extern bool didUpdateThisFrame
		{
			[NativeName("DidUpdateThisFrame")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00002F08 File Offset: 0x00001108
		[FreeFunction("WebCamTextureBindings::Internal_GetPixel", HasExplicitThis = true)]
		public Color GetPixel(int x, int y)
		{
			Color color;
			this.GetPixel_Injected(x, y, out color);
			return color;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00002F20 File Offset: 0x00001120
		public Color[] GetPixels()
		{
			return this.GetPixels(0, 0, this.width, this.height);
		}

		// Token: 0x06000172 RID: 370
		[FreeFunction("WebCamTextureBindings::Internal_GetPixels", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color[] GetPixels(int x, int y, int blockWidth, int blockHeight);

		// Token: 0x06000173 RID: 371 RVA: 0x00002F48 File Offset: 0x00001148
		[ExcludeFromDocs]
		public Color32[] GetPixels32()
		{
			return this.GetPixels32(null);
		}

		// Token: 0x06000174 RID: 372
		[FreeFunction("WebCamTextureBindings::Internal_GetPixels32", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color32[] GetPixels32([Unmarshalled] [DefaultValue("null")] Color32[] colors);

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00002F64 File Offset: 0x00001164
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00002F9E File Offset: 0x0000119E
		public Vector2? autoFocusPoint
		{
			get
			{
				return (this.internalAutoFocusPoint.x < 0f) ? null : new Vector2?(this.internalAutoFocusPoint);
			}
			set
			{
				this.internalAutoFocusPoint = ((value == null) ? new Vector2(-1f, -1f) : value.Value);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00002FCC File Offset: 0x000011CC
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00002FE2 File Offset: 0x000011E2
		internal Vector2 internalAutoFocusPoint
		{
			get
			{
				Vector2 vector;
				this.get_internalAutoFocusPoint_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_internalAutoFocusPoint_Injected(ref value);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000179 RID: 377
		public extern bool isDepth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600017A RID: 378
		[StaticAccessor("WebCamTextureBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateWebCamTexture([Writable] WebCamTexture self, string scriptingDevice, int requestedWidth, int requestedHeight, int maxFramerate);

		// Token: 0x0600017B RID: 379
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPixel_Injected(int x, int y, out Color ret);

		// Token: 0x0600017C RID: 380
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_internalAutoFocusPoint_Injected(out Vector2 ret);

		// Token: 0x0600017D RID: 381
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_internalAutoFocusPoint_Injected(ref Vector2 value);
	}
}
