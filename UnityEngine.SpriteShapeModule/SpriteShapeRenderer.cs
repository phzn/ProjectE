using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.U2D
{
	// Token: 0x02000005 RID: 5
	[MovedFrom("UnityEngine.Experimental.U2D")]
	[NativeType(Header = "Modules/SpriteShape/Public/SpriteShapeRenderer.h")]
	public class SpriteShapeRenderer : Renderer
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020E0 File Offset: 0x000002E0
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000020F6 File Offset: 0x000002F6
		public Color color
		{
			get
			{
				Color color;
				this.get_color_Injected(out color);
				return color;
			}
			set
			{
				this.set_color_Injected(ref value);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11
		// (set) Token: 0x0600000C RID: 12
		public extern SpriteMaskInteraction maskInteraction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002100 File Offset: 0x00000300
		public void Prepare(JobHandle handle, SpriteShapeParameters shapeParams, Sprite[] sprites)
		{
			this.Prepare_Injected(ref handle, ref shapeParams, sprites);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002110 File Offset: 0x00000310
		private NativeArray<T> GetNativeDataArray<T>(SpriteShapeDataType dataType) where T : struct
		{
			SpriteChannelInfo dataInfo = this.GetDataInfo(dataType);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(dataInfo.buffer, dataInfo.count, Allocator.Invalid);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002140 File Offset: 0x00000340
		private unsafe NativeSlice<T> GetChannelDataArray<T>(SpriteShapeDataType dataType, VertexAttribute channel) where T : struct
		{
			SpriteChannelInfo channelInfo = this.GetChannelInfo(channel);
			byte* ptr = (byte*)channelInfo.buffer + channelInfo.offset;
			return NativeSliceUnsafeUtility.ConvertExistingDataToNativeSlice<T>((void*)ptr, channelInfo.stride, channelInfo.count);
		}

		// Token: 0x06000010 RID: 16
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetSegmentCount(int geomCount);

		// Token: 0x06000011 RID: 17
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetMeshDataCount(int vertexCount, int indexCount);

		// Token: 0x06000012 RID: 18
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetMeshChannelInfo(int vertexCount, int indexCount, int hotChannelMask);

		// Token: 0x06000013 RID: 19 RVA: 0x00002180 File Offset: 0x00000380
		private SpriteChannelInfo GetDataInfo(SpriteShapeDataType arrayType)
		{
			SpriteChannelInfo spriteChannelInfo;
			this.GetDataInfo_Injected(arrayType, out spriteChannelInfo);
			return spriteChannelInfo;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002198 File Offset: 0x00000398
		private SpriteChannelInfo GetChannelInfo(VertexAttribute channel)
		{
			SpriteChannelInfo spriteChannelInfo;
			this.GetChannelInfo_Injected(channel, out spriteChannelInfo);
			return spriteChannelInfo;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000021AF File Offset: 0x000003AF
		public void SetLocalAABB(Bounds bounds)
		{
			this.SetLocalAABB_Injected(ref bounds);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000021BC File Offset: 0x000003BC
		public NativeArray<Bounds> GetBounds()
		{
			return this.GetNativeDataArray<Bounds>(SpriteShapeDataType.BoundingBox);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000021D8 File Offset: 0x000003D8
		public NativeArray<SpriteShapeSegment> GetSegments(int dataSize)
		{
			this.SetSegmentCount(dataSize);
			return this.GetNativeDataArray<SpriteShapeSegment>(SpriteShapeDataType.Segment);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000021F9 File Offset: 0x000003F9
		public void GetChannels(int dataSize, out NativeArray<ushort> indices, out NativeSlice<Vector3> vertices, out NativeSlice<Vector2> texcoords)
		{
			this.SetMeshDataCount(dataSize, dataSize);
			indices = this.GetNativeDataArray<ushort>(SpriteShapeDataType.Index);
			vertices = this.GetChannelDataArray<Vector3>(SpriteShapeDataType.ChannelVertex, VertexAttribute.Position);
			texcoords = this.GetChannelDataArray<Vector2>(SpriteShapeDataType.ChannelTexCoord0, VertexAttribute.TexCoord0);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002230 File Offset: 0x00000430
		public void GetChannels(int dataSize, out NativeArray<ushort> indices, out NativeSlice<Vector3> vertices, out NativeSlice<Vector2> texcoords, out NativeSlice<Color32> colors)
		{
			this.SetMeshChannelInfo(dataSize, dataSize, 8);
			indices = this.GetNativeDataArray<ushort>(SpriteShapeDataType.Index);
			vertices = this.GetChannelDataArray<Vector3>(SpriteShapeDataType.ChannelVertex, VertexAttribute.Position);
			texcoords = this.GetChannelDataArray<Vector2>(SpriteShapeDataType.ChannelTexCoord0, VertexAttribute.TexCoord0);
			colors = this.GetChannelDataArray<Color32>(SpriteShapeDataType.ChannelColor, VertexAttribute.Color);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002284 File Offset: 0x00000484
		public void GetChannels(int dataSize, out NativeArray<ushort> indices, out NativeSlice<Vector3> vertices, out NativeSlice<Vector2> texcoords, out NativeSlice<Vector4> tangents)
		{
			this.SetMeshChannelInfo(dataSize, dataSize, 4);
			indices = this.GetNativeDataArray<ushort>(SpriteShapeDataType.Index);
			vertices = this.GetChannelDataArray<Vector3>(SpriteShapeDataType.ChannelVertex, VertexAttribute.Position);
			texcoords = this.GetChannelDataArray<Vector2>(SpriteShapeDataType.ChannelTexCoord0, VertexAttribute.TexCoord0);
			tangents = this.GetChannelDataArray<Vector4>(SpriteShapeDataType.ChannelTangent, VertexAttribute.Tangent);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000022D8 File Offset: 0x000004D8
		public void GetChannels(int dataSize, out NativeArray<ushort> indices, out NativeSlice<Vector3> vertices, out NativeSlice<Vector2> texcoords, out NativeSlice<Color32> colors, out NativeSlice<Vector4> tangents)
		{
			this.SetMeshChannelInfo(dataSize, dataSize, 12);
			indices = this.GetNativeDataArray<ushort>(SpriteShapeDataType.Index);
			vertices = this.GetChannelDataArray<Vector3>(SpriteShapeDataType.ChannelVertex, VertexAttribute.Position);
			texcoords = this.GetChannelDataArray<Vector2>(SpriteShapeDataType.ChannelTexCoord0, VertexAttribute.TexCoord0);
			colors = this.GetChannelDataArray<Color32>(SpriteShapeDataType.ChannelColor, VertexAttribute.Color);
			tangents = this.GetChannelDataArray<Vector4>(SpriteShapeDataType.ChannelTangent, VertexAttribute.Tangent);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000233C File Offset: 0x0000053C
		public void GetChannels(int dataSize, out NativeArray<ushort> indices, out NativeSlice<Vector3> vertices, out NativeSlice<Vector2> texcoords, out NativeSlice<Vector4> tangents, out NativeSlice<Vector3> normals)
		{
			this.SetMeshChannelInfo(dataSize, dataSize, 6);
			indices = this.GetNativeDataArray<ushort>(SpriteShapeDataType.Index);
			vertices = this.GetChannelDataArray<Vector3>(SpriteShapeDataType.ChannelVertex, VertexAttribute.Position);
			texcoords = this.GetChannelDataArray<Vector2>(SpriteShapeDataType.ChannelTexCoord0, VertexAttribute.TexCoord0);
			tangents = this.GetChannelDataArray<Vector4>(SpriteShapeDataType.ChannelTangent, VertexAttribute.Tangent);
			normals = this.GetChannelDataArray<Vector3>(SpriteShapeDataType.ChannelNormal, VertexAttribute.Normal);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000239C File Offset: 0x0000059C
		public void GetChannels(int dataSize, out NativeArray<ushort> indices, out NativeSlice<Vector3> vertices, out NativeSlice<Vector2> texcoords, out NativeSlice<Color32> colors, out NativeSlice<Vector4> tangents, out NativeSlice<Vector3> normals)
		{
			this.SetMeshChannelInfo(dataSize, dataSize, 14);
			indices = this.GetNativeDataArray<ushort>(SpriteShapeDataType.Index);
			vertices = this.GetChannelDataArray<Vector3>(SpriteShapeDataType.ChannelVertex, VertexAttribute.Position);
			texcoords = this.GetChannelDataArray<Vector2>(SpriteShapeDataType.ChannelTexCoord0, VertexAttribute.TexCoord0);
			colors = this.GetChannelDataArray<Color32>(SpriteShapeDataType.ChannelColor, VertexAttribute.Color);
			tangents = this.GetChannelDataArray<Vector4>(SpriteShapeDataType.ChannelTangent, VertexAttribute.Tangent);
			normals = this.GetChannelDataArray<Vector3>(SpriteShapeDataType.ChannelNormal, VertexAttribute.Normal);
		}

		// Token: 0x0600001F RID: 31
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_color_Injected(out Color ret);

		// Token: 0x06000020 RID: 32
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_color_Injected(ref Color value);

		// Token: 0x06000021 RID: 33
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Prepare_Injected(ref JobHandle handle, ref SpriteShapeParameters shapeParams, Sprite[] sprites);

		// Token: 0x06000022 RID: 34
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetDataInfo_Injected(SpriteShapeDataType arrayType, out SpriteChannelInfo ret);

		// Token: 0x06000023 RID: 35
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetChannelInfo_Injected(VertexAttribute channel, out SpriteChannelInfo ret);

		// Token: 0x06000024 RID: 36
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetLocalAABB_Injected(ref Bounds bounds);
	}
}
