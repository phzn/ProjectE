using System;
using System.Collections.Generic;
using Pathfinding.Drawing.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pathfinding.Drawing
{
	// Token: 0x0200004D RID: 77
	internal static class GeometryBuilder
	{
		// Token: 0x06000283 RID: 643 RVA: 0x0000BB0C File Offset: 0x00009D0C
		internal unsafe static JobHandle Build(DrawingData gizmos, DrawingData.ProcessedBuilderData.MeshBuffers* buffers, ref GeometryBuilder.CameraInfo cameraInfo, JobHandle dependency)
		{
			return new GeometryBuilderJob
			{
				buffers = buffers,
				currentMatrix = Matrix4x4.identity,
				currentLineWidthData = new CommandBuilder.LineWidthData
				{
					pixels = 1f,
					automaticJoins = false
				},
				lineWidthMultiplier = DrawingManager.lineWidthMultiplier,
				currentColor = Color.white,
				cameraPosition = cameraInfo.cameraPosition,
				cameraRotation = cameraInfo.cameraRotation,
				cameraDepthToPixelSize = cameraInfo.cameraDepthToPixelSize,
				cameraIsOrthographic = cameraInfo.cameraIsOrthographic,
				characterInfo = (SDFCharacter*)gizmos.fontData.characters.GetUnsafeReadOnlyPtr<SDFCharacter>(),
				characterInfoLength = gizmos.fontData.characters.Length,
				maxPixelError = 0.5f / math.max(0.1f, gizmos.settingsRef.curveResolution)
			}.Schedule(dependency);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000BC08 File Offset: 0x00009E08
		private static float2 CameraDepthToPixelSize(Camera camera)
		{
			if (camera.orthographic)
			{
				return new float2(0f, 2f * camera.orthographicSize / (float)camera.pixelHeight);
			}
			return new float2(Mathf.Tan(camera.fieldOfView * 0.017453292f * 0.5f) / (0.5f * (float)camera.pixelHeight), 0f);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000BC6B File Offset: 0x00009E6B
		private unsafe static NativeArray<T> ConvertExistingDataToNativeArray<T>(UnsafeAppendBuffer data) where T : struct
		{
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)data.Ptr, data.Length / UnsafeUtility.SizeOf<T>(), Allocator.Invalid);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000BC88 File Offset: 0x00009E88
		internal unsafe static void BuildMesh(DrawingData gizmos, List<DrawingData.MeshWithType> meshes, DrawingData.ProcessedBuilderData.MeshBuffers* inputBuffers)
		{
			if (inputBuffers->triangles.Length > 0)
			{
				Mesh mesh = GeometryBuilder.AssignMeshData<GeometryBuilderJob.Vertex>(gizmos, inputBuffers->bounds, inputBuffers->vertices, inputBuffers->triangles, MeshLayouts.MeshLayout);
				meshes.Add(new DrawingData.MeshWithType
				{
					mesh = mesh,
					type = DrawingData.MeshType.Lines
				});
			}
			if (inputBuffers->solidTriangles.Length > 0)
			{
				Mesh mesh2 = GeometryBuilder.AssignMeshData<GeometryBuilderJob.Vertex>(gizmos, inputBuffers->bounds, inputBuffers->solidVertices, inputBuffers->solidTriangles, MeshLayouts.MeshLayout);
				meshes.Add(new DrawingData.MeshWithType
				{
					mesh = mesh2,
					type = DrawingData.MeshType.Solid
				});
			}
			if (inputBuffers->textTriangles.Length > 0)
			{
				Mesh mesh3 = GeometryBuilder.AssignMeshData<GeometryBuilderJob.TextVertex>(gizmos, inputBuffers->bounds, inputBuffers->textVertices, inputBuffers->textTriangles, MeshLayouts.MeshLayoutText);
				meshes.Add(new DrawingData.MeshWithType
				{
					mesh = mesh3,
					type = DrawingData.MeshType.Text
				});
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000BD78 File Offset: 0x00009F78
		private static Mesh AssignMeshData<VertexType>(DrawingData gizmos, Bounds bounds, UnsafeAppendBuffer vertices, UnsafeAppendBuffer triangles, VertexAttributeDescriptor[] layout) where VertexType : struct
		{
			NativeArray<VertexType> nativeArray = GeometryBuilder.ConvertExistingDataToNativeArray<VertexType>(vertices);
			NativeArray<int> nativeArray2 = GeometryBuilder.ConvertExistingDataToNativeArray<int>(triangles);
			Mesh mesh = gizmos.GetMesh(nativeArray.Length);
			mesh.SetVertexBufferParams(math.ceilpow2(nativeArray.Length), layout);
			mesh.SetIndexBufferParams(math.ceilpow2(nativeArray2.Length), IndexFormat.UInt32);
			mesh.SetVertexBufferData<VertexType>(nativeArray, 0, 0, nativeArray.Length, 0, MeshUpdateFlags.Default);
			mesh.SetIndexBufferData<int>(nativeArray2, 0, 0, nativeArray2.Length, MeshUpdateFlags.DontValidateIndices);
			mesh.subMeshCount = 1;
			SubMeshDescriptor subMeshDescriptor = new SubMeshDescriptor(0, nativeArray2.Length, MeshTopology.Triangles)
			{
				vertexCount = nativeArray.Length,
				bounds = bounds
			};
			mesh.SetSubMesh(0, subMeshDescriptor, MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
			mesh.bounds = bounds;
			return mesh;
		}

		// Token: 0x0200004E RID: 78
		public struct CameraInfo
		{
			// Token: 0x06000288 RID: 648 RVA: 0x0000BE2C File Offset: 0x0000A02C
			public CameraInfo(Camera camera)
			{
				Transform transform = ((camera != null) ? camera.transform : null);
				this.cameraPosition = ((transform != null) ? transform.position : float3.zero);
				this.cameraRotation = ((transform != null) ? transform.rotation : quaternion.identity);
				this.cameraDepthToPixelSize = ((camera != null) ? GeometryBuilder.CameraDepthToPixelSize(camera) : 0);
				this.cameraIsOrthographic = camera != null && camera.orthographic;
			}

			// Token: 0x04000135 RID: 309
			public float3 cameraPosition;

			// Token: 0x04000136 RID: 310
			public quaternion cameraRotation;

			// Token: 0x04000137 RID: 311
			public float2 cameraDepthToPixelSize;

			// Token: 0x04000138 RID: 312
			public bool cameraIsOrthographic;
		}
	}
}
