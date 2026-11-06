using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.U2D;

namespace UnityEngine.Tilemaps
{
	// Token: 0x02000011 RID: 17
	[NativeHeader("Modules/Tilemap/TilemapRendererJobs.h")]
	[NativeHeader("Modules/Grid/Public/GridMarshalling.h")]
	[RequireComponent(typeof(Tilemap))]
	[NativeType(Header = "Modules/Tilemap/Public/TilemapRenderer.h")]
	[NativeHeader("Modules/Tilemap/Public/TilemapMarshalling.h")]
	public sealed class TilemapRenderer : Renderer
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000DA RID: 218 RVA: 0x0000309C File Offset: 0x0000129C
		// (set) Token: 0x060000DB RID: 219 RVA: 0x000030B2 File Offset: 0x000012B2
		public Vector3Int chunkSize
		{
			get
			{
				Vector3Int vector3Int;
				this.get_chunkSize_Injected(out vector3Int);
				return vector3Int;
			}
			set
			{
				this.set_chunkSize_Injected(ref value);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000030BC File Offset: 0x000012BC
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000030D2 File Offset: 0x000012D2
		public Vector3 chunkCullingBounds
		{
			[FreeFunction("TilemapRendererBindings::GetChunkCullingBounds", HasExplicitThis = true)]
			get
			{
				Vector3 vector;
				this.get_chunkCullingBounds_Injected(out vector);
				return vector;
			}
			[FreeFunction("TilemapRendererBindings::SetChunkCullingBounds", HasExplicitThis = true)]
			set
			{
				this.set_chunkCullingBounds_Injected(ref value);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000DE RID: 222
		// (set) Token: 0x060000DF RID: 223
		public extern int maxChunkCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000E0 RID: 224
		// (set) Token: 0x060000E1 RID: 225
		public extern int maxFrameAge
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000E2 RID: 226
		// (set) Token: 0x060000E3 RID: 227
		public extern TilemapRenderer.SortOrder sortOrder
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000E4 RID: 228
		// (set) Token: 0x060000E5 RID: 229
		[NativeProperty("RenderMode")]
		public extern TilemapRenderer.Mode mode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000E6 RID: 230
		// (set) Token: 0x060000E7 RID: 231
		public extern TilemapRenderer.DetectChunkCullingBounds detectChunkCullingBounds
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000E8 RID: 232
		// (set) Token: 0x060000E9 RID: 233
		public extern SpriteMaskInteraction maskInteraction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000030DC File Offset: 0x000012DC
		[RequiredByNativeCode]
		internal void RegisterSpriteAtlasRegistered()
		{
			SpriteAtlasManager.atlasRegistered += this.OnSpriteAtlasRegistered;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000030F1 File Offset: 0x000012F1
		[RequiredByNativeCode]
		internal void UnregisterSpriteAtlasRegistered()
		{
			SpriteAtlasManager.atlasRegistered -= this.OnSpriteAtlasRegistered;
		}

		// Token: 0x060000EC RID: 236
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void OnSpriteAtlasRegistered(SpriteAtlas atlas);

		// Token: 0x060000EE RID: 238
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_chunkSize_Injected(out Vector3Int ret);

		// Token: 0x060000EF RID: 239
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_chunkSize_Injected(ref Vector3Int value);

		// Token: 0x060000F0 RID: 240
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_chunkCullingBounds_Injected(out Vector3 ret);

		// Token: 0x060000F1 RID: 241
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_chunkCullingBounds_Injected(ref Vector3 value);

		// Token: 0x02000012 RID: 18
		public enum SortOrder
		{
			// Token: 0x04000041 RID: 65
			BottomLeft,
			// Token: 0x04000042 RID: 66
			BottomRight,
			// Token: 0x04000043 RID: 67
			TopLeft,
			// Token: 0x04000044 RID: 68
			TopRight
		}

		// Token: 0x02000013 RID: 19
		public enum Mode
		{
			// Token: 0x04000046 RID: 70
			Chunk,
			// Token: 0x04000047 RID: 71
			Individual
		}

		// Token: 0x02000014 RID: 20
		public enum DetectChunkCullingBounds
		{
			// Token: 0x04000049 RID: 73
			Auto,
			// Token: 0x0400004A RID: 74
			Manual
		}
	}
}
