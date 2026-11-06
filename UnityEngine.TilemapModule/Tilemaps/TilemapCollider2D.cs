using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Tilemaps
{
	// Token: 0x02000019 RID: 25
	[NativeType(Header = "Modules/Tilemap/Public/TilemapCollider2D.h")]
	[RequireComponent(typeof(Tilemap))]
	public sealed class TilemapCollider2D : Collider2D
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600011E RID: 286
		// (set) Token: 0x0600011F RID: 287
		public extern bool useDelaunayMesh
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000120 RID: 288
		// (set) Token: 0x06000121 RID: 289
		public extern uint maximumTileChangeCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000122 RID: 290
		// (set) Token: 0x06000123 RID: 291
		public extern float extrusionFactor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000124 RID: 292
		public extern bool hasTilemapChanges
		{
			[NativeMethod("HasTilemapChanges")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000125 RID: 293
		[NativeMethod(Name = "ProcessTileChangeQueue")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ProcessTilemapChanges();
	}
}
