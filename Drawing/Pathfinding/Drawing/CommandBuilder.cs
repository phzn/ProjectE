using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pathfinding.Drawing
{
	// Token: 0x0200000B RID: 11
	[BurstCompile]
	public struct CommandBuilder : IDisposable
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002409 File Offset: 0x00000609
		internal unsafe CommandBuilder(UnsafeAppendBuffer* buffer, GCHandle gizmos, int threadIndex, DrawingData.BuilderData.BitPackedMeta uniqueID)
		{
			this.buffer = buffer;
			this.gizmos = gizmos;
			this.threadIndex = threadIndex;
			this.uniqueID = uniqueID;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002428 File Offset: 0x00000628
		internal CommandBuilder(DrawingData gizmos, DrawingData.Hasher hasher, RedrawScope frameRedrawScope, RedrawScope customRedrawScope, bool isGizmos, bool isBuiltInCommandBuilder, int sceneModeVersion)
		{
			this.gizmos = GCHandle.Alloc(gizmos, GCHandleType.Normal);
			this.threadIndex = 0;
			this.uniqueID = gizmos.data.Reserve(isBuiltInCommandBuilder);
			gizmos.data.Get(this.uniqueID).Init(hasher, frameRedrawScope, customRedrawScope, isGizmos, gizmos.GetNextDrawOrderIndex(), sceneModeVersion);
			this.buffer = gizmos.data.Get(this.uniqueID).bufferPtr;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000012 RID: 18 RVA: 0x0000249C File Offset: 0x0000069C
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000024A9 File Offset: 0x000006A9
		internal unsafe int BufferSize
		{
			get
			{
				return this.buffer->Length;
			}
			set
			{
				this.buffer->Length = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000024B7 File Offset: 0x000006B7
		public CommandBuilder2D xy
		{
			get
			{
				return new CommandBuilder2D(this, true);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000024C5 File Offset: 0x000006C5
		public CommandBuilder2D xz
		{
			get
			{
				return new CommandBuilder2D(this, false);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000024D4 File Offset: 0x000006D4
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002548 File Offset: 0x00000748
		public Camera[] cameraTargets
		{
			get
			{
				if (this.gizmos.IsAllocated && this.gizmos.Target != null)
				{
					DrawingData drawingData = this.gizmos.Target as DrawingData;
					if (drawingData.data.StillExists(this.uniqueID))
					{
						return drawingData.data.Get(this.uniqueID).meta.cameraTargets;
					}
				}
				throw new Exception("Cannot get cameraTargets because the command builder has already been disposed or does not exist.");
			}
			set
			{
				if (this.uniqueID.isBuiltInCommandBuilder)
				{
					throw new Exception("You cannot set the camera targets for a built-in command builder. Create a custom command builder instead.");
				}
				if (!this.gizmos.IsAllocated || this.gizmos.Target == null)
				{
					throw new Exception("Cannot set cameraTargets because the command builder has already been disposed or does not exist.");
				}
				DrawingData drawingData = this.gizmos.Target as DrawingData;
				if (!drawingData.data.StillExists(this.uniqueID))
				{
					throw new Exception("Cannot set cameraTargets because the command builder has already been disposed or does not exist.");
				}
				drawingData.data.Get(this.uniqueID).meta.cameraTargets = value;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000025DB File Offset: 0x000007DB
		public void Dispose()
		{
			if (this.uniqueID.isBuiltInCommandBuilder)
			{
				throw new Exception("You cannot dispose a built-in command builder");
			}
			this.DisposeInternal();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000025FC File Offset: 0x000007FC
		public void DisposeAfter(JobHandle dependency, AllowedDelay allowedDelay = AllowedDelay.EndOfFrame)
		{
			if (!this.gizmos.IsAllocated)
			{
				throw new Exception("You cannot dispose an invalid command builder. Are you trying to dispose it twice?");
			}
			try
			{
				if (this.gizmos.IsAllocated && this.gizmos.Target != null)
				{
					DrawingData drawingData = this.gizmos.Target as DrawingData;
					if (!drawingData.data.StillExists(this.uniqueID))
					{
						throw new Exception("Cannot dispose the command builder because the drawing manager has been destroyed");
					}
					drawingData.data.Get(this.uniqueID).SubmitWithDependency(this.gizmos, dependency, allowedDelay);
				}
			}
			finally
			{
				this = default(CommandBuilder);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000026A4 File Offset: 0x000008A4
		internal void DisposeInternal()
		{
			if (!this.gizmos.IsAllocated)
			{
				throw new Exception("You cannot dispose an invalid command builder. Are you trying to dispose it twice?");
			}
			try
			{
				if (this.gizmos.IsAllocated && this.gizmos.Target != null)
				{
					DrawingData drawingData = this.gizmos.Target as DrawingData;
					if (!drawingData.data.StillExists(this.uniqueID))
					{
						throw new Exception("Cannot dispose the command builder because the drawing manager has been destroyed");
					}
					drawingData.data.Get(this.uniqueID).Submit(this.gizmos.Target as DrawingData);
				}
			}
			finally
			{
				this.gizmos.Free();
				this = default(CommandBuilder);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000275C File Offset: 0x0000095C
		public void DiscardAndDispose()
		{
			if (this.uniqueID.isBuiltInCommandBuilder)
			{
				throw new Exception("You cannot dispose a built-in command builder");
			}
			this.DiscardAndDisposeInternal();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000277C File Offset: 0x0000097C
		internal void DiscardAndDisposeInternal()
		{
			try
			{
				if (this.gizmos.IsAllocated && this.gizmos.Target != null)
				{
					DrawingData drawingData = this.gizmos.Target as DrawingData;
					if (!drawingData.data.StillExists(this.uniqueID))
					{
						throw new Exception("Cannot dispose the command builder because the drawing manager has been destroyed");
					}
					drawingData.data.Release(this.uniqueID);
				}
			}
			finally
			{
				if (this.gizmos.IsAllocated)
				{
					this.gizmos.Free();
				}
				this = default(CommandBuilder);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002814 File Offset: 0x00000A14
		public void Preallocate(int size)
		{
			this.Reserve(size);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002820 File Offset: 0x00000A20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe void Reserve(int additionalSpace)
		{
			if (Hint.Unlikely(this.threadIndex >= 0))
			{
				this.buffer += this.threadIndex;
				this.threadIndex = -1;
			}
			int num = this.buffer->Length + additionalSpace;
			if (num > this.buffer->Capacity)
			{
				this.buffer->SetCapacity(math.max(num, this.buffer->Length * 2));
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000289C File Offset: 0x00000A9C
		[BurstDiscard]
		private void AssertBufferExists()
		{
			if (!this.gizmos.IsAllocated || this.gizmos.Target == null || !(this.gizmos.Target as DrawingData).data.StillExists(this.uniqueID))
			{
				this = default(CommandBuilder);
				throw new Exception("This command builder no longer exists. Are you trying to draw to a command builder which has already been disposed?");
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000028F7 File Offset: 0x00000AF7
		[BurstDiscard]
		private static void AssertNotRendering()
		{
			if (!GizmoContext.drawingGizmos && !JobsUtility.IsExecutingJob && (Time.renderedFrameCount & 127) == 0 && StackTraceUtility.ExtractStackTrace().Contains("OnDrawGizmos"))
			{
				throw new Exception("You are trying to use Draw.* functions from within Unity's OnDrawGizmos function. Use this package's gizmo callbacks instead (see the documentation).");
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000292D File Offset: 0x00000B2D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Reserve<A>() where A : struct
		{
			this.Reserve(UnsafeUtility.SizeOf<CommandBuilder.Command>() + UnsafeUtility.SizeOf<A>());
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002940 File Offset: 0x00000B40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Reserve<A, B>() where A : struct where B : struct
		{
			this.Reserve(UnsafeUtility.SizeOf<CommandBuilder.Command>() * 2 + UnsafeUtility.SizeOf<A>() + UnsafeUtility.SizeOf<B>());
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000295B File Offset: 0x00000B5B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Reserve<A, B, C>() where A : struct where B : struct where C : struct
		{
			this.Reserve(UnsafeUtility.SizeOf<CommandBuilder.Command>() * 3 + UnsafeUtility.SizeOf<A>() + UnsafeUtility.SizeOf<B>() + UnsafeUtility.SizeOf<C>());
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000297C File Offset: 0x00000B7C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint ConvertColor(Color color)
		{
			if (X86.Sse2.IsSse2Supported)
			{
				int4 @int = (int4)(255f * new float4(color.r, color.g, color.b, color.a) + 0.5f);
				v128 v = new v128(@int.x, @int.y, @int.z, @int.w);
				v128 v2 = X86.Sse2.packs_epi32(v, v);
				return X86.Sse2.packus_epi16(v2, v2).UInt0;
			}
			uint num = (uint)Mathf.Clamp((int)(color.r * 255f + 0.5f), 0, 255);
			uint num2 = (uint)Mathf.Clamp((int)(color.g * 255f + 0.5f), 0, 255);
			uint num3 = (uint)Mathf.Clamp((int)(color.b * 255f + 0.5f), 0, 255);
			return (uint)((Mathf.Clamp((int)(color.a * 255f + 0.5f), 0, 255) << 24) | (int)((int)num3 << 16) | (int)((int)num2 << 8) | (int)num);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002A80 File Offset: 0x00000C80
		internal unsafe void Add<T>(T value) where T : struct
		{
			int num = UnsafeUtility.SizeOf<T>();
			UnsafeAppendBuffer* ptr = this.buffer;
			int length = ptr->Length;
			Hint.Assume(ptr->Ptr != null);
			Hint.Assume(ptr->Ptr + length != null);
			UnsafeUtility.CopyStructureToPtr<T>(ref value, (void*)(ptr->Ptr + length));
			ptr->Length = length + num;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002AE0 File Offset: 0x00000CE0
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix WithMatrix(Matrix4x4 matrix)
		{
			this.PushMatrix(matrix);
			return new CommandBuilder.ScopeMatrix
			{
				builder = this
			};
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002B0C File Offset: 0x00000D0C
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix WithMatrix(float3x3 matrix)
		{
			this.PushMatrix(new float4x4(matrix, float3.zero));
			return new CommandBuilder.ScopeMatrix
			{
				builder = this
			};
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002B40 File Offset: 0x00000D40
		[BurstDiscard]
		public CommandBuilder.ScopeColor WithColor(Color color)
		{
			this.PushColor(color);
			return new CommandBuilder.ScopeColor
			{
				builder = this
			};
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002B6C File Offset: 0x00000D6C
		[BurstDiscard]
		public CommandBuilder.ScopePersist WithDuration(float duration)
		{
			this.PushDuration(duration);
			return new CommandBuilder.ScopePersist
			{
				builder = this
			};
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002B98 File Offset: 0x00000D98
		[BurstDiscard]
		public CommandBuilder.ScopeLineWidth WithLineWidth(float pixels, bool automaticJoins = true)
		{
			this.PushLineWidth(pixels, automaticJoins);
			return new CommandBuilder.ScopeLineWidth
			{
				builder = this
			};
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002BC3 File Offset: 0x00000DC3
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix InLocalSpace(Transform transform)
		{
			return this.WithMatrix(transform.localToWorldMatrix);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002BD4 File Offset: 0x00000DD4
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix InScreenSpace(Camera camera)
		{
			return this.WithMatrix(camera.cameraToWorldMatrix * camera.nonJitteredProjectionMatrix.inverse * Matrix4x4.TRS(new Vector3(-1f, -1f, 0f), Quaternion.identity, new Vector3(2f / (float)camera.pixelWidth, 2f / (float)camera.pixelHeight, 1f)));
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002C47 File Offset: 0x00000E47
		public void PushMatrix(Matrix4x4 matrix)
		{
			this.Reserve<float4x4>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushMatrix);
			this.Add<Matrix4x4>(matrix);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002C5D File Offset: 0x00000E5D
		public void PushMatrix(float4x4 matrix)
		{
			this.Reserve<float4x4>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushMatrix);
			this.Add<float4x4>(matrix);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002C73 File Offset: 0x00000E73
		public void PushSetMatrix(Matrix4x4 matrix)
		{
			this.Reserve<float4x4>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushSetMatrix);
			this.Add<float4x4>(matrix);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002C8E File Offset: 0x00000E8E
		public void PushSetMatrix(float4x4 matrix)
		{
			this.Reserve<float4x4>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushSetMatrix);
			this.Add<float4x4>(matrix);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002CA4 File Offset: 0x00000EA4
		public void PopMatrix()
		{
			this.Reserve(4);
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PopMatrix);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002CB4 File Offset: 0x00000EB4
		public void PushColor(Color color)
		{
			this.Reserve<Color32>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColor);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002CCF File Offset: 0x00000ECF
		public void PopColor()
		{
			this.Reserve(4);
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PopColor);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002CE0 File Offset: 0x00000EE0
		public unsafe void PushDuration(float duration)
		{
			this.Reserve<CommandBuilder.PersistData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushPersist);
			this.Add<CommandBuilder.PersistData>(new CommandBuilder.PersistData
			{
				endTime = *SharedDrawingData.BurstTime.Data + duration
			});
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002D1E File Offset: 0x00000F1E
		public void PopDuration()
		{
			this.Reserve(4);
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PopPersist);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002D2F File Offset: 0x00000F2F
		[Obsolete("Renamed to PushDuration for consistency")]
		public void PushPersist(float duration)
		{
			this.PushDuration(duration);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002D38 File Offset: 0x00000F38
		[Obsolete("Renamed to PopDuration for consistency")]
		public void PopPersist()
		{
			this.PopDuration();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002D40 File Offset: 0x00000F40
		public void PushLineWidth(float pixels, bool automaticJoins = true)
		{
			if (pixels < 0f)
			{
				throw new ArgumentOutOfRangeException("pixels", "Line width must be positive");
			}
			this.Reserve<CommandBuilder.LineWidthData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushLineWidth);
			this.Add<CommandBuilder.LineWidthData>(new CommandBuilder.LineWidthData
			{
				pixels = pixels,
				automaticJoins = automaticJoins
			});
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002D92 File Offset: 0x00000F92
		public void PopLineWidth()
		{
			this.Reserve(4);
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PopLineWidth);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002DA4 File Offset: 0x00000FA4
		public void Line(float3 a, float3 b)
		{
			this.Reserve<CommandBuilder.LineData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Line);
			this.Add<CommandBuilder.LineData>(new CommandBuilder.LineData
			{
				a = a,
				b = b
			});
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002DE0 File Offset: 0x00000FE0
		public unsafe void Line(Vector3 a, Vector3 b)
		{
			this.Reserve<CommandBuilder.LineData>();
			int bufferSize = this.BufferSize;
			int num = bufferSize + 4 + 24;
			byte* ptr = this.buffer->Ptr + bufferSize;
			*(int*)ptr = 5;
			CommandBuilder.LineDataV3* ptr2 = (CommandBuilder.LineDataV3*)(ptr + 4);
			ptr2->a = a;
			ptr2->b = b;
			this.buffer->Length = num;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002E30 File Offset: 0x00001030
		public unsafe void Line(Vector3 a, Vector3 b, Color color)
		{
			this.Reserve<Color32, CommandBuilder.LineData>();
			int bufferSize = this.BufferSize;
			int num = bufferSize + 4 + 24 + 4;
			byte* ptr = this.buffer->Ptr + bufferSize;
			*(int*)ptr = 261;
			*(int*)(ptr + 4) = (int)CommandBuilder.ConvertColor(color);
			CommandBuilder.LineDataV3* ptr2 = (CommandBuilder.LineDataV3*)(ptr + 8);
			ptr2->a = a;
			ptr2->b = b;
			this.buffer->Length = num;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002E8E File Offset: 0x0000108E
		public void Ray(float3 origin, float3 direction)
		{
			this.Line(origin, origin + direction);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002E9E File Offset: 0x0000109E
		public void Ray(Ray ray, float length)
		{
			this.Line(ray.origin, ray.origin + ray.direction * length);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002EC8 File Offset: 0x000010C8
		public void Arc(float3 center, float3 start, float3 end)
		{
			float3 @float = start - center;
			float3 float2 = end - center;
			float3 float3 = math.cross(float2, @float);
			if (math.any(float3 != 0f) && math.all(math.isfinite(float3)))
			{
				Matrix4x4 matrix4x = Matrix4x4.TRS(center, Quaternion.LookRotation(@float, float3), Vector3.one);
				float num = Vector3.SignedAngle(@float, float2, float3) * 0.017453292f;
				this.PushMatrix(matrix4x);
				this.CircleXZInternal(float3.zero, math.length(@float), 1.5707964f, 1.5707964f - num);
				this.PopMatrix();
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002F78 File Offset: 0x00001178
		[Obsolete("Use Draw.xz.Circle instead")]
		public void CircleXZ(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.CircleXZInternal(center, radius, startAngle, endAngle);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002F88 File Offset: 0x00001188
		internal void CircleXZInternal(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.Reserve<CommandBuilder.CircleXZData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.CircleXZ);
			this.Add<CommandBuilder.CircleXZData>(new CommandBuilder.CircleXZData
			{
				center = center,
				radius = radius,
				startAngle = startAngle,
				endAngle = endAngle
			});
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002FD4 File Offset: 0x000011D4
		internal void CircleXZInternal(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.Reserve<Color32, CommandBuilder.CircleXZData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopColor | CommandBuilder.Command.PushMatrix | CommandBuilder.Command.PopMatrix);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.CircleXZData>(new CommandBuilder.CircleXZData
			{
				center = center,
				radius = radius,
				startAngle = startAngle,
				endAngle = endAngle
			});
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000302F File Offset: 0x0000122F
		[Obsolete("Use Draw.xy.Circle instead")]
		public void CircleXY(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
			this.CircleXZ(new float3(center.x, -center.z, center.y), radius, startAngle, endAngle);
			this.PopMatrix();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003064 File Offset: 0x00001264
		public void Circle(float3 center, float3 normal, float radius)
		{
			this.Reserve<CommandBuilder.CircleData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Circle);
			this.Add<CommandBuilder.CircleData>(new CommandBuilder.CircleData
			{
				center = center,
				normal = normal,
				radius = radius
			});
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000030A8 File Offset: 0x000012A8
		public void SolidArc(float3 center, float3 start, float3 end)
		{
			float3 @float = start - center;
			float3 float2 = end - center;
			float3 float3 = math.cross(float2, @float);
			if (math.any(float3))
			{
				Matrix4x4 matrix4x = Matrix4x4.TRS(center, Quaternion.LookRotation(@float, float3), Vector3.one);
				float num = Vector3.SignedAngle(@float, float2, float3) * 0.017453292f;
				this.PushMatrix(matrix4x);
				this.SolidCircleXZInternal(float3.zero, math.length(@float), 1.5707964f, 1.5707964f - num);
				this.PopMatrix();
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003141 File Offset: 0x00001341
		[Obsolete("Use Draw.xz.SolidCircle instead")]
		public void SolidCircleXZ(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.SolidCircleXZInternal(center, radius, startAngle, endAngle);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003150 File Offset: 0x00001350
		internal void SolidCircleXZInternal(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.Reserve<CommandBuilder.CircleXZData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.DiscXZ);
			this.Add<CommandBuilder.CircleXZData>(new CommandBuilder.CircleXZData
			{
				center = center,
				radius = radius,
				startAngle = startAngle,
				endAngle = endAngle
			});
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000319C File Offset: 0x0000139C
		internal void SolidCircleXZInternal(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.Reserve<Color32, CommandBuilder.CircleXZData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopColor | CommandBuilder.Command.Disc);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.CircleXZData>(new CommandBuilder.CircleXZData
			{
				center = center,
				radius = radius,
				startAngle = startAngle,
				endAngle = endAngle
			});
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000031F7 File Offset: 0x000013F7
		[Obsolete("Use Draw.xy.SolidCircle instead")]
		public void SolidCircleXY(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
			this.SolidCircleXZInternal(new float3(center.x, -center.z, center.y), radius, startAngle, endAngle);
			this.PopMatrix();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000322C File Offset: 0x0000142C
		public void SolidCircle(float3 center, float3 normal, float radius)
		{
			this.Reserve<CommandBuilder.CircleData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Disc);
			this.Add<CommandBuilder.CircleData>(new CommandBuilder.CircleData
			{
				center = center,
				normal = normal,
				radius = radius
			});
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003270 File Offset: 0x00001470
		public void SphereOutline(float3 center, float radius)
		{
			this.Reserve<CommandBuilder.SphereData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.SphereOutline);
			this.Add<CommandBuilder.SphereData>(new CommandBuilder.SphereData
			{
				center = center,
				radius = radius
			});
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000032AA File Offset: 0x000014AA
		public void WireCylinder(float3 bottom, float3 top, float radius)
		{
			this.WireCylinder(bottom, top - bottom, math.length(top - bottom), radius);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000032C8 File Offset: 0x000014C8
		public void WireCylinder(float3 position, float3 up, float height, float radius)
		{
			up = math.normalizesafe(up, default(float3));
			if (math.all(up == 0f) || math.any(math.isnan(up)) || math.isnan(height) || math.isnan(radius))
			{
				return;
			}
			float3 @float;
			float3 float2;
			CommandBuilder.OrthonormalBasis(up, out @float, out float2);
			this.PushMatrix(new float4x4(new float4(@float * radius, 0f), new float4(up * height, 0f), new float4(float2 * radius, 0f), new float4(position, 1f)));
			this.CircleXZInternal(float3.zero, 1f, 0f, 6.2831855f);
			if (height > 0f)
			{
				this.CircleXZInternal(new float3(0f, 1f, 0f), 1f, 0f, 6.2831855f);
				this.Line(new float3(1f, 0f, 0f), new float3(1f, 1f, 0f));
				this.Line(new float3(-1f, 0f, 0f), new float3(-1f, 1f, 0f));
				this.Line(new float3(0f, 0f, 1f), new float3(0f, 1f, 1f));
				this.Line(new float3(0f, 0f, -1f), new float3(0f, 1f, -1f));
			}
			this.PopMatrix();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003478 File Offset: 0x00001678
		private static void OrthonormalBasis(float3 normal, out float3 basis1, out float3 basis2)
		{
			basis1 = math.cross(normal, new float3(1f, 1f, 1f));
			if (math.all(basis1 == 0f))
			{
				basis1 = math.cross(normal, new float3(-1f, 1f, 1f));
			}
			basis1 = math.normalizesafe(basis1, default(float3));
			basis2 = math.cross(normal, basis1);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003508 File Offset: 0x00001708
		public void WireCapsule(float3 start, float3 end, float radius)
		{
			float3 @float = end - start;
			float num = math.length(@float);
			if ((double)num < 0.0001)
			{
				this.WireSphere(start, radius);
				return;
			}
			float3 float2 = @float / num;
			this.WireCapsule(start - float2 * radius, float2, num + 2f * radius, radius);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003560 File Offset: 0x00001760
		public void WireCapsule(float3 position, float3 direction, float length, float radius)
		{
			direction = math.normalizesafe(direction, default(float3));
			if (math.all(direction == 0f) || math.any(math.isnan(direction)) || math.isnan(length) || math.isnan(radius))
			{
				return;
			}
			if (radius <= 0f)
			{
				this.Line(position, position + direction * length);
				return;
			}
			length = math.max(length, radius * 2f);
			float3 @float;
			float3 float2;
			CommandBuilder.OrthonormalBasis(direction, out @float, out float2);
			this.PushMatrix(new float4x4(new float4(@float, 0f), new float4(direction, 0f), new float4(float2, 0f), new float4(position, 1f)));
			this.CircleXZInternal(new float3(0f, radius, 0f), radius, 0f, 6.2831855f);
			this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
			this.CircleXZInternal(new float3(0f, 0f, radius), radius, 3.1415927f, 6.2831855f);
			this.PopMatrix();
			this.PushMatrix(CommandBuilder.XZtoYZPlaneMatrix);
			this.CircleXZInternal(new float3(radius, 0f, 0f), radius, 1.5707964f, 4.712389f);
			this.PopMatrix();
			if (length > 0f)
			{
				float num = length - radius;
				this.CircleXZInternal(new float3(0f, num, 0f), radius, 0f, 6.2831855f);
				this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
				this.CircleXZInternal(new float3(0f, 0f, num), radius, 0f, 3.1415927f);
				this.PopMatrix();
				this.PushMatrix(CommandBuilder.XZtoYZPlaneMatrix);
				this.CircleXZInternal(new float3(num, 0f, 0f), radius, -1.5707964f, 1.5707964f);
				this.PopMatrix();
				this.Line(new float3(radius, radius, 0f), new float3(radius, num, 0f));
				this.Line(new float3(-radius, radius, 0f), new float3(-radius, num, 0f));
				this.Line(new float3(0f, radius, radius), new float3(0f, num, radius));
				this.Line(new float3(0f, radius, -radius), new float3(0f, num, -radius));
			}
			this.PopMatrix();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000037D4 File Offset: 0x000019D4
		public void WireSphere(float3 position, float radius)
		{
			this.SphereOutline(position, radius);
			this.Circle(position, new float3(1f, 0f, 0f), radius);
			this.Circle(position, new float3(0f, 1f, 0f), radius);
			this.Circle(position, new float3(0f, 0f, 1f), radius);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003840 File Offset: 0x00001A40
		[BurstDiscard]
		public void Polyline(List<Vector3> points, bool cycle = false)
		{
			for (int i = 0; i < points.Count - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Count > 1)
			{
				this.Line(points[points.Count - 1], points[0]);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000038A0 File Offset: 0x00001AA0
		public void Polyline<T>(T points, bool cycle = false) where T : IReadOnlyList<float3>
		{
			for (int i = 0; i < points.Count - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Count > 1)
			{
				this.Line(points[points.Count - 1], points[0]);
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003930 File Offset: 0x00001B30
		[BurstDiscard]
		public void Polyline(Vector3[] points, bool cycle = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003984 File Offset: 0x00001B84
		[BurstDiscard]
		public void Polyline(float3[] points, bool cycle = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000039D8 File Offset: 0x00001BD8
		public void Polyline(NativeArray<float3> points, bool cycle = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003A3C File Offset: 0x00001C3C
		public void DashedLine(float3 a, float3 b, float dash, float gap)
		{
			CommandBuilder.PolylineWithSymbol polylineWithSymbol = new CommandBuilder.PolylineWithSymbol(CommandBuilder.SymbolDecoration.None, gap, 0f, dash + gap, false, 0f);
			polylineWithSymbol.MoveTo(ref this, a);
			polylineWithSymbol.MoveTo(ref this, b);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003A74 File Offset: 0x00001C74
		public void DashedPolyline(List<Vector3> points, float dash, float gap)
		{
			CommandBuilder.PolylineWithSymbol polylineWithSymbol = new CommandBuilder.PolylineWithSymbol(CommandBuilder.SymbolDecoration.None, gap, 0f, dash + gap, false, 0f);
			for (int i = 0; i < points.Count; i++)
			{
				polylineWithSymbol.MoveTo(ref this, points[i]);
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003AC0 File Offset: 0x00001CC0
		public void WireBox(float3 center, float3 size)
		{
			this.Reserve<CommandBuilder.BoxData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.WireBox);
			this.Add<CommandBuilder.BoxData>(new CommandBuilder.BoxData
			{
				center = center,
				size = size
			});
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003AFA File Offset: 0x00001CFA
		public void WireBox(float3 center, quaternion rotation, float3 size)
		{
			this.PushMatrix(float4x4.TRS(center, rotation, size));
			this.WireBox(float3.zero, new float3(1f, 1f, 1f));
			this.PopMatrix();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003B2F File Offset: 0x00001D2F
		public void WireBox(Bounds bounds)
		{
			this.WireBox(bounds.center, bounds.size);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003B50 File Offset: 0x00001D50
		public void WireMesh(Mesh mesh)
		{
			if (mesh == null)
			{
				throw new ArgumentNullException();
			}
			Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(mesh);
			Mesh.MeshData meshData = meshDataArray[0];
			CommandBuilder.JobWireMesh.JobWireMeshFunctionPointer(ref meshData, ref this);
			meshDataArray.Dispose();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003B90 File Offset: 0x00001D90
		public unsafe void WireMesh(NativeArray<float3> vertices, NativeArray<int> triangles)
		{
			CommandBuilder.JobWireMesh.WireMesh((float3*)vertices.GetUnsafeReadOnlyPtr<float3>(), (int*)triangles.GetUnsafeReadOnlyPtr<int>(), vertices.Length, triangles.Length, ref this);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003BB2 File Offset: 0x00001DB2
		public void SolidMesh(Mesh mesh)
		{
			this.SolidMeshInternal(mesh, false);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003BBC File Offset: 0x00001DBC
		private void SolidMeshInternal(Mesh mesh, bool temporary, Color color)
		{
			this.PushColor(color);
			this.SolidMeshInternal(mesh, temporary);
			this.PopColor();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003BD4 File Offset: 0x00001DD4
		private void SolidMeshInternal(Mesh mesh, bool temporary)
		{
			(this.gizmos.Target as DrawingData).data.Get(this.uniqueID).meshes.Add(new DrawingData.SubmittedMesh
			{
				mesh = mesh,
				temporary = temporary
			});
			this.Reserve(4);
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.CaptureState);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003C34 File Offset: 0x00001E34
		[BurstDiscard]
		public void SolidMesh(List<Vector3> vertices, List<int> triangles, List<Color> colors)
		{
			if (vertices.Count != colors.Count)
			{
				throw new ArgumentException("Number of colors must be the same as the number of vertices");
			}
			Mesh mesh = (this.gizmos.Target as DrawingData).GetMesh(vertices.Count);
			mesh.Clear();
			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangles, 0);
			mesh.SetColors(colors);
			mesh.UploadMeshData(false);
			this.SolidMeshInternal(mesh, true);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003CA4 File Offset: 0x00001EA4
		[BurstDiscard]
		public void SolidMesh(Vector3[] vertices, int[] triangles, Color[] colors, int vertexCount, int indexCount)
		{
			if (vertices.Length != colors.Length)
			{
				throw new ArgumentException("Number of colors must be the same as the number of vertices");
			}
			Mesh mesh = (this.gizmos.Target as DrawingData).GetMesh(vertices.Length);
			mesh.Clear();
			mesh.SetVertices(vertices, 0, vertexCount);
			mesh.SetTriangles(triangles, 0, indexCount, 0, true, 0);
			mesh.SetColors(colors, 0, vertexCount);
			mesh.UploadMeshData(false);
			this.SolidMeshInternal(mesh, true);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003D14 File Offset: 0x00001F14
		public void Cross(float3 position, float size = 1f)
		{
			size *= 0.5f;
			this.Line(position - new float3(size, 0f, 0f), position + new float3(size, 0f, 0f));
			this.Line(position - new float3(0f, size, 0f), position + new float3(0f, size, 0f));
			this.Line(position - new float3(0f, 0f, size), position + new float3(0f, 0f, size));
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003DC0 File Offset: 0x00001FC0
		[Obsolete("Use Draw.xz.Cross instead")]
		public void CrossXZ(float3 position, float size = 1f)
		{
			size *= 0.5f;
			this.Line(position - new float3(size, 0f, 0f), position + new float3(size, 0f, 0f));
			this.Line(position - new float3(0f, 0f, size), position + new float3(0f, 0f, size));
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003E3C File Offset: 0x0000203C
		[Obsolete("Use Draw.xy.Cross instead")]
		public void CrossXY(float3 position, float size = 1f)
		{
			size *= 0.5f;
			this.Line(position - new float3(size, 0f, 0f), position + new float3(size, 0f, 0f));
			this.Line(position - new float3(0f, size, 0f), position + new float3(0f, size, 0f));
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003EB8 File Offset: 0x000020B8
		public static float3 EvaluateCubicBezier(float3 p0, float3 p1, float3 p2, float3 p3, float t)
		{
			t = math.clamp(t, 0f, 1f);
			float num = 1f - t;
			return num * num * num * p0 + 3f * num * num * t * p1 + 3f * num * t * t * p2 + t * t * t * p3;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003F30 File Offset: 0x00002130
		public void Bezier(float3 p0, float3 p1, float3 p2, float3 p3)
		{
			float3 @float = p0;
			for (int i = 1; i <= 20; i++)
			{
				float num = (float)i / 20f;
				float3 float2 = CommandBuilder.EvaluateCubicBezier(p0, p1, p2, p3, num);
				this.Line(@float, float2);
				@float = float2;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003F6C File Offset: 0x0000216C
		public void CatmullRom(List<Vector3> points)
		{
			if (points.Count < 2)
			{
				return;
			}
			if (points.Count == 2)
			{
				this.Line(points[0], points[1]);
				return;
			}
			int count = points.Count;
			this.CatmullRom(points[0], points[0], points[1], points[2]);
			int num = 0;
			while (num + 3 < count)
			{
				this.CatmullRom(points[num], points[num + 1], points[num + 2], points[num + 3]);
				num++;
			}
			this.CatmullRom(points[count - 3], points[count - 2], points[count - 1], points[count - 1]);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004068 File Offset: 0x00002268
		public void CatmullRom(float3 p0, float3 p1, float3 p2, float3 p3)
		{
			float3 @float = (-p0 + 6f * p1 + 1f * p2) * 0.16666667f;
			float3 float2 = (p1 + 6f * p2 - p3) * 0.16666667f;
			this.Bezier(p1, @float, float2, p2);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000040D7 File Offset: 0x000022D7
		public void Arrow(float3 from, float3 to)
		{
			this.ArrowRelativeSizeHead(from, to, CommandBuilder.DEFAULT_UP, 0.2f);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000040EC File Offset: 0x000022EC
		public void Arrow(float3 from, float3 to, float3 up, float headSize)
		{
			float num = math.lengthsq(to - from);
			if (num > 1E-06f)
			{
				this.ArrowRelativeSizeHead(from, to, up, headSize * math.rsqrt(num));
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004120 File Offset: 0x00002320
		public void ArrowRelativeSizeHead(float3 from, float3 to, float3 up, float headFraction)
		{
			this.Line(from, to);
			float3 @float = to - from;
			float3 float2 = math.cross(@float, up);
			if (math.all(float2 == 0f))
			{
				float2 = math.cross(new float3(1f, 0f, 0f), @float);
			}
			if (math.all(float2 == 0f))
			{
				float2 = math.cross(new float3(0f, 1f, 0f), @float);
			}
			float2 = math.normalizesafe(float2, default(float3)) * math.length(@float);
			this.Line(to, to - (@float + float2) * headFraction);
			this.Line(to, to - (@float - float2) * headFraction);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000041F0 File Offset: 0x000023F0
		public void Arrowhead(float3 center, float3 direction, float radius)
		{
			this.Arrowhead(center, direction, CommandBuilder.DEFAULT_UP, radius);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004200 File Offset: 0x00002400
		public void Arrowhead(float3 center, float3 direction, float3 up, float radius)
		{
			if (math.all(direction == 0f))
			{
				return;
			}
			direction = math.normalizesafe(direction, default(float3));
			float3 @float = math.cross(direction, up);
			float3 float2 = center - radius * 0.5f * 0.5f * direction;
			float3 float3 = float2 + radius * direction;
			float3 float4 = float2 - radius * 0.5f * direction + radius * 0.866025f * @float;
			float3 float5 = float2 - radius * 0.5f * direction - radius * 0.866025f * @float;
			this.Line(float3, float4);
			this.Line(float4, float2);
			this.Line(float2, float5);
			this.Line(float5, float3);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000042D8 File Offset: 0x000024D8
		public void ArrowheadArc(float3 origin, float3 direction, float offset, float width = 60f)
		{
			if (!math.any(direction))
			{
				return;
			}
			if (offset < 0f)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset == 0f)
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(direction, CommandBuilder.DEFAULT_UP);
			this.PushMatrix(Matrix4x4.TRS(origin, quaternion, Vector3.one));
			float num = 1.5707964f - width * 0.008726646f;
			float num2 = 1.5707964f + width * 0.008726646f;
			this.CircleXZInternal(float3.zero, offset, num, num2);
			float3 @float = new float3(math.cos(num), 0f, math.sin(num)) * offset;
			float3 float2 = new float3(math.cos(num2), 0f, math.sin(num2)) * offset;
			float3 float3 = new float3(0f, 0f, 1.4142f * offset);
			this.Line(@float, float3);
			this.Line(float3, float2);
			this.PopMatrix();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000043D0 File Offset: 0x000025D0
		public void WireGrid(float3 center, quaternion rotation, int2 cells, float2 totalSize)
		{
			cells = math.max(cells, new int2(1, 1));
			this.PushMatrix(float4x4.TRS(center, rotation, new Vector3(totalSize.x, 0f, totalSize.y)));
			int x = cells.x;
			int y = cells.y;
			for (int i = 0; i <= x; i++)
			{
				this.Line(new float3((float)i / (float)x - 0.5f, 0f, -0.5f), new float3((float)i / (float)x - 0.5f, 0f, 0.5f));
			}
			for (int j = 0; j <= y; j++)
			{
				this.Line(new float3(-0.5f, 0f, (float)j / (float)y - 0.5f), new float3(0.5f, 0f, (float)j / (float)y - 0.5f));
			}
			this.PopMatrix();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000044B6 File Offset: 0x000026B6
		public void WireTriangle(float3 a, float3 b, float3 c)
		{
			this.Line(a, b);
			this.Line(b, c);
			this.Line(c, a);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000044D0 File Offset: 0x000026D0
		[Obsolete("Use Draw.xz.WireRectangle instead")]
		public void WireRectangleXZ(float3 center, float2 size)
		{
			this.WireRectangle(center, quaternion.identity, size);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000044DF File Offset: 0x000026DF
		public void WireRectangle(float3 center, quaternion rotation, float2 size)
		{
			this.WirePlane(center, rotation, size);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000044EC File Offset: 0x000026EC
		[Obsolete("Use Draw.xy.WireRectangle instead")]
		public void WireRectangle(Rect rect)
		{
			this.xy.WireRectangle(rect);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004508 File Offset: 0x00002708
		public void WireTriangle(float3 center, quaternion rotation, float radius)
		{
			this.WirePolygon(center, 3, rotation, radius);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004514 File Offset: 0x00002714
		public void WirePentagon(float3 center, quaternion rotation, float radius)
		{
			this.WirePolygon(center, 5, rotation, radius);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004520 File Offset: 0x00002720
		public void WireHexagon(float3 center, quaternion rotation, float radius)
		{
			this.WirePolygon(center, 6, rotation, radius);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000452C File Offset: 0x0000272C
		public void WirePolygon(float3 center, int vertices, quaternion rotation, float radius)
		{
			this.PushMatrix(float4x4.TRS(center, rotation, new float3(radius, radius, radius)));
			float3 @float = new float3(0f, 0f, 1f);
			for (int i = 1; i <= vertices; i++)
			{
				float num = 6.2831855f * ((float)i / (float)vertices);
				float3 float2 = new float3(math.sin(num), 0f, math.cos(num));
				this.Line(@float, float2);
				@float = float2;
			}
			this.PopMatrix();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000045A8 File Offset: 0x000027A8
		[Obsolete("Use Draw.xy.SolidRectangle instead")]
		public void SolidRectangle(Rect rect)
		{
			this.xy.SolidRectangle(rect);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000045C4 File Offset: 0x000027C4
		public void SolidPlane(float3 center, float3 normal, float2 size)
		{
			if (math.any(normal))
			{
				this.SolidPlane(center, Quaternion.LookRotation(CommandBuilder.calculateTangent(normal), normal), size);
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000045F4 File Offset: 0x000027F4
		public void SolidPlane(float3 center, quaternion rotation, float2 size)
		{
			this.PushMatrix(float4x4.TRS(center, rotation, new float3(size.x, 0f, size.y)));
			this.Reserve<CommandBuilder.BoxData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Box);
			this.Add<CommandBuilder.BoxData>(new CommandBuilder.BoxData
			{
				center = 0,
				size = 1
			});
			this.PopMatrix();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004664 File Offset: 0x00002864
		private static float3 calculateTangent(float3 normal)
		{
			float3 @float = math.cross(new float3(0f, 1f, 0f), normal);
			if (math.all(@float == 0f))
			{
				@float = math.cross(new float3(1f, 0f, 0f), normal);
			}
			return @float;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000046BA File Offset: 0x000028BA
		public void WirePlane(float3 center, float3 normal, float2 size)
		{
			if (math.any(normal))
			{
				this.WirePlane(center, Quaternion.LookRotation(CommandBuilder.calculateTangent(normal), normal), size);
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000046E8 File Offset: 0x000028E8
		public void WirePlane(float3 center, quaternion rotation, float2 size)
		{
			this.Reserve<CommandBuilder.PlaneData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.WirePlane);
			this.Add<CommandBuilder.PlaneData>(new CommandBuilder.PlaneData
			{
				center = center,
				rotation = rotation,
				size = size
			});
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000472A File Offset: 0x0000292A
		public void PlaneWithNormal(float3 center, float3 normal, float2 size)
		{
			if (math.any(normal))
			{
				this.PlaneWithNormal(center, Quaternion.LookRotation(CommandBuilder.calculateTangent(normal), normal), size);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004758 File Offset: 0x00002958
		public void PlaneWithNormal(float3 center, quaternion rotation, float2 size)
		{
			this.SolidPlane(center, rotation, size);
			this.WirePlane(center, rotation, size);
			this.ArrowRelativeSizeHead(center, center + math.mul(rotation, new float3(0f, 1f, 0f)) * 0.5f, math.mul(rotation, new float3(0f, 0f, 1f)), 0.2f);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000047C8 File Offset: 0x000029C8
		public void SolidTriangle(float3 a, float3 b, float3 c)
		{
			this.Reserve<CommandBuilder.TriangleData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.SolidTriangle);
			this.Add<CommandBuilder.TriangleData>(new CommandBuilder.TriangleData
			{
				a = a,
				b = b,
				c = c
			});
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000480C File Offset: 0x00002A0C
		public void SolidBox(float3 center, float3 size)
		{
			this.Reserve<CommandBuilder.BoxData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Box);
			this.Add<CommandBuilder.BoxData>(new CommandBuilder.BoxData
			{
				center = center,
				size = size
			});
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004846 File Offset: 0x00002A46
		public void SolidBox(Bounds bounds)
		{
			this.SolidBox(bounds.center, bounds.size);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004866 File Offset: 0x00002A66
		public void SolidBox(float3 center, quaternion rotation, float3 size)
		{
			this.PushMatrix(float4x4.TRS(center, rotation, size));
			this.SolidBox(float3.zero, Vector3.one);
			this.PopMatrix();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004891 File Offset: 0x00002A91
		public void Label3D(float3 position, quaternion rotation, string text, float size)
		{
			this.Label3D(position, rotation, text, size, LabelAlignment.MiddleLeft);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000048A4 File Offset: 0x00002AA4
		public void Label3D(float3 position, quaternion rotation, string text, float size, LabelAlignment alignment)
		{
			this.AssertBufferExists();
			this.Reserve<CommandBuilder.TextData3D>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Text3D);
			this.Add<CommandBuilder.TextData3D>(new CommandBuilder.TextData3D
			{
				center = position,
				rotation = rotation,
				numCharacters = text.Length,
				size = size,
				alignment = alignment
			});
			this.AddText(text);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000490A File Offset: 0x00002B0A
		public void Label2D(float3 position, string text, float sizeInPixels = 14f)
		{
			this.Label2D(position, text, sizeInPixels, LabelAlignment.MiddleLeft);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000491C File Offset: 0x00002B1C
		public void Label2D(float3 position, string text, float sizeInPixels, LabelAlignment alignment)
		{
			this.AssertBufferExists();
			this.Reserve<CommandBuilder.TextData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Text);
			this.Add<CommandBuilder.TextData>(new CommandBuilder.TextData
			{
				center = position,
				numCharacters = text.Length,
				sizeInPixels = sizeInPixels,
				alignment = alignment
			});
			this.AddText(text);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000497C File Offset: 0x00002B7C
		private void AddText(string text)
		{
			DrawingData drawingData = this.gizmos.Target as DrawingData;
			this.Reserve(UnsafeUtility.SizeOf<ushort>() * text.Length);
			foreach (char c in text)
			{
				ushort num = (ushort)drawingData.fontData.GetIndex(c);
				this.Add<ushort>(num);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000049DA File Offset: 0x00002BDA
		public void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels = 14f)
		{
			this.Label2D(position, ref text, sizeInPixels, LabelAlignment.MiddleLeft);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000049EA File Offset: 0x00002BEA
		public void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels = 14f)
		{
			this.Label2D(position, ref text, sizeInPixels, LabelAlignment.MiddleLeft);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000049FA File Offset: 0x00002BFA
		public void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels = 14f)
		{
			this.Label2D(position, ref text, sizeInPixels, LabelAlignment.MiddleLeft);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004A0A File Offset: 0x00002C0A
		public void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels = 14f)
		{
			this.Label2D(position, ref text, sizeInPixels, LabelAlignment.MiddleLeft);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004A1A File Offset: 0x00002C1A
		public void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(position, text.GetUnsafePtr(), text.Length, sizeInPixels, alignment);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004A32 File Offset: 0x00002C32
		public void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(position, text.GetUnsafePtr(), text.Length, sizeInPixels, alignment);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004A4A File Offset: 0x00002C4A
		public void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(position, text.GetUnsafePtr(), text.Length, sizeInPixels, alignment);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004A62 File Offset: 0x00002C62
		public void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(position, text.GetUnsafePtr(), text.Length, sizeInPixels, alignment);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004A7C File Offset: 0x00002C7C
		internal unsafe void Label2D(float3 position, byte* text, int byteCount, float sizeInPixels, LabelAlignment alignment)
		{
			this.AssertBufferExists();
			this.Reserve<CommandBuilder.TextData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Text);
			this.Add<CommandBuilder.TextData>(new CommandBuilder.TextData
			{
				center = position,
				numCharacters = byteCount,
				sizeInPixels = sizeInPixels,
				alignment = alignment
			});
			this.Reserve(UnsafeUtility.SizeOf<ushort>() * byteCount);
			for (int i = 0; i < byteCount; i++)
			{
				ushort num = (ushort)text[i];
				if (num >= 128)
				{
					num = 63;
				}
				if (num == 10)
				{
					num = ushort.MaxValue;
				}
				if (num != 13)
				{
					this.Add<ushort>(num);
				}
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004B0E File Offset: 0x00002D0E
		public void Label3D(float3 position, quaternion rotation, ref FixedString32Bytes text, float size)
		{
			this.Label3D(position, rotation, ref text, size, LabelAlignment.MiddleLeft);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004B20 File Offset: 0x00002D20
		public void Label3D(float3 position, quaternion rotation, ref FixedString64Bytes text, float size)
		{
			this.Label3D(position, rotation, ref text, size, LabelAlignment.MiddleLeft);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004B32 File Offset: 0x00002D32
		public void Label3D(float3 position, quaternion rotation, ref FixedString128Bytes text, float size)
		{
			this.Label3D(position, rotation, ref text, size, LabelAlignment.MiddleLeft);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004B44 File Offset: 0x00002D44
		public void Label3D(float3 position, quaternion rotation, ref FixedString512Bytes text, float size)
		{
			this.Label3D(position, rotation, ref text, size, LabelAlignment.MiddleLeft);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004B56 File Offset: 0x00002D56
		public void Label3D(float3 position, quaternion rotation, ref FixedString32Bytes text, float size, LabelAlignment alignment)
		{
			this.Label3D(position, rotation, text.GetUnsafePtr(), text.Length, size, alignment);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004B70 File Offset: 0x00002D70
		public void Label3D(float3 position, quaternion rotation, ref FixedString64Bytes text, float size, LabelAlignment alignment)
		{
			this.Label3D(position, rotation, text.GetUnsafePtr(), text.Length, size, alignment);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004B8A File Offset: 0x00002D8A
		public void Label3D(float3 position, quaternion rotation, ref FixedString128Bytes text, float size, LabelAlignment alignment)
		{
			this.Label3D(position, rotation, text.GetUnsafePtr(), text.Length, size, alignment);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004BA4 File Offset: 0x00002DA4
		public void Label3D(float3 position, quaternion rotation, ref FixedString512Bytes text, float size, LabelAlignment alignment)
		{
			this.Label3D(position, rotation, text.GetUnsafePtr(), text.Length, size, alignment);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004BC0 File Offset: 0x00002DC0
		internal unsafe void Label3D(float3 position, quaternion rotation, byte* text, int byteCount, float size, LabelAlignment alignment)
		{
			this.AssertBufferExists();
			this.Reserve<CommandBuilder.TextData3D>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Text3D);
			this.Add<CommandBuilder.TextData3D>(new CommandBuilder.TextData3D
			{
				center = position,
				rotation = rotation,
				numCharacters = byteCount,
				size = size,
				alignment = alignment
			});
			this.Reserve(UnsafeUtility.SizeOf<ushort>() * byteCount);
			for (int i = 0; i < byteCount; i++)
			{
				ushort num = (ushort)text[i];
				if (num >= 128)
				{
					num = 63;
				}
				if (num == 10)
				{
					num = ushort.MaxValue;
				}
				this.Add<ushort>(num);
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004C58 File Offset: 0x00002E58
		public void Line(float3 a, float3 b, Color color)
		{
			this.Reserve<Color32, CommandBuilder.LineData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopColor | CommandBuilder.Command.PopMatrix);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.LineData>(new CommandBuilder.LineData
			{
				a = a,
				b = b
			});
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004CA1 File Offset: 0x00002EA1
		public void Ray(float3 origin, float3 direction, Color color)
		{
			this.Line(origin, origin + direction, color);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004CB2 File Offset: 0x00002EB2
		public void Ray(Ray ray, float length, Color color)
		{
			this.Line(ray.origin, ray.origin + ray.direction * length, color);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004CDC File Offset: 0x00002EDC
		public void Arc(float3 center, float3 start, float3 end, Color color)
		{
			this.PushColor(color);
			float3 @float = start - center;
			float3 float2 = end - center;
			float3 float3 = math.cross(float2, @float);
			if (math.any(float3 != 0f) && math.all(math.isfinite(float3)))
			{
				Matrix4x4 matrix4x = Matrix4x4.TRS(center, Quaternion.LookRotation(@float, float3), Vector3.one);
				float num = Vector3.SignedAngle(@float, float2, float3) * 0.017453292f;
				this.PushMatrix(matrix4x);
				this.CircleXZInternal(float3.zero, math.length(@float), 1.5707964f, 1.5707964f - num);
				this.PopMatrix();
			}
			this.PopColor();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004D9A File Offset: 0x00002F9A
		[Obsolete("Use Draw.xz.Circle instead")]
		public void CircleXZ(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.CircleXZInternal(center, radius, startAngle, endAngle, color);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004DA9 File Offset: 0x00002FA9
		[Obsolete("Use Draw.xz.Circle instead")]
		public void CircleXZ(float3 center, float radius, Color color)
		{
			this.CircleXZ(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004DC0 File Offset: 0x00002FC0
		public void Circle(float3 center, float3 normal, float radius, Color color)
		{
			this.Reserve<Color32, CommandBuilder.CircleData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PushMatrix | CommandBuilder.Command.PopMatrix);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.CircleData>(new CommandBuilder.CircleData
			{
				center = center,
				normal = normal,
				radius = radius
			});
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004E12 File Offset: 0x00003012
		public void WireCylinder(float3 bottom, float3 top, float radius, Color color)
		{
			this.WireCylinder(bottom, top - bottom, math.length(top - bottom), radius, color);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004E34 File Offset: 0x00003034
		public void WireCylinder(float3 position, float3 up, float height, float radius, Color color)
		{
			up = math.normalizesafe(up, default(float3));
			if (math.all(up == 0f) || math.any(math.isnan(up)) || math.isnan(height) || math.isnan(radius))
			{
				return;
			}
			this.PushColor(color);
			float3 @float;
			float3 float2;
			CommandBuilder.OrthonormalBasis(up, out @float, out float2);
			this.PushMatrix(new float4x4(new float4(@float * radius, 0f), new float4(up * height, 0f), new float4(float2 * radius, 0f), new float4(position, 1f)));
			this.CircleXZInternal(float3.zero, 1f, 0f, 6.2831855f);
			if (height > 0f)
			{
				this.CircleXZInternal(new float3(0f, 1f, 0f), 1f, 0f, 6.2831855f);
				this.Line(new float3(1f, 0f, 0f), new float3(1f, 1f, 0f));
				this.Line(new float3(-1f, 0f, 0f), new float3(-1f, 1f, 0f));
				this.Line(new float3(0f, 0f, 1f), new float3(0f, 1f, 1f));
				this.Line(new float3(0f, 0f, -1f), new float3(0f, 1f, -1f));
			}
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004FF4 File Offset: 0x000031F4
		public void WireCapsule(float3 start, float3 end, float radius, Color color)
		{
			this.PushColor(color);
			float3 @float = end - start;
			float num = math.length(@float);
			if ((double)num < 0.0001)
			{
				this.WireSphere(start, radius);
			}
			else
			{
				float3 float2 = @float / num;
				this.WireCapsule(start - float2 * radius, float2, num + 2f * radius, radius);
			}
			this.PopColor();
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000505C File Offset: 0x0000325C
		public void WireCapsule(float3 position, float3 direction, float length, float radius, Color color)
		{
			direction = math.normalizesafe(direction, default(float3));
			if (math.all(direction == 0f) || math.any(math.isnan(direction)) || math.isnan(length) || math.isnan(radius))
			{
				return;
			}
			this.PushColor(color);
			if (radius <= 0f)
			{
				this.Line(position, position + direction * length);
			}
			else
			{
				length = math.max(length, radius * 2f);
				float3 @float;
				float3 float2;
				CommandBuilder.OrthonormalBasis(direction, out @float, out float2);
				this.PushMatrix(new float4x4(new float4(@float, 0f), new float4(direction, 0f), new float4(float2, 0f), new float4(position, 1f)));
				this.CircleXZInternal(new float3(0f, radius, 0f), radius, 0f, 6.2831855f);
				this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
				this.CircleXZInternal(new float3(0f, 0f, radius), radius, 3.1415927f, 6.2831855f);
				this.PopMatrix();
				this.PushMatrix(CommandBuilder.XZtoYZPlaneMatrix);
				this.CircleXZInternal(new float3(radius, 0f, 0f), radius, 1.5707964f, 4.712389f);
				this.PopMatrix();
				if (length > 0f)
				{
					float num = length - radius;
					this.CircleXZInternal(new float3(0f, num, 0f), radius, 0f, 6.2831855f);
					this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
					this.CircleXZInternal(new float3(0f, 0f, num), radius, 0f, 3.1415927f);
					this.PopMatrix();
					this.PushMatrix(CommandBuilder.XZtoYZPlaneMatrix);
					this.CircleXZInternal(new float3(num, 0f, 0f), radius, -1.5707964f, 1.5707964f);
					this.PopMatrix();
					this.Line(new float3(radius, radius, 0f), new float3(radius, num, 0f));
					this.Line(new float3(-radius, radius, 0f), new float3(-radius, num, 0f));
					this.Line(new float3(0f, radius, radius), new float3(0f, num, radius));
					this.Line(new float3(0f, radius, -radius), new float3(0f, num, -radius));
				}
				this.PopMatrix();
			}
			this.PopColor();
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000052E4 File Offset: 0x000034E4
		public void WireSphere(float3 position, float radius, Color color)
		{
			this.PushColor(color);
			this.SphereOutline(position, radius);
			this.Circle(position, new float3(1f, 0f, 0f), radius);
			this.Circle(position, new float3(0f, 1f, 0f), radius);
			this.Circle(position, new float3(0f, 0f, 1f), radius);
			this.PopColor();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000535C File Offset: 0x0000355C
		[BurstDiscard]
		public void Polyline(List<Vector3> points, bool cycle, Color color)
		{
			this.PushColor(color);
			for (int i = 0; i < points.Count - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Count > 1)
			{
				this.Line(points[points.Count - 1], points[0]);
			}
			this.PopColor();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000053C6 File Offset: 0x000035C6
		[BurstDiscard]
		public void Polyline(List<Vector3> points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000053D4 File Offset: 0x000035D4
		[BurstDiscard]
		public void Polyline(Vector3[] points, bool cycle, Color color)
		{
			this.PushColor(color);
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
			this.PopColor();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005435 File Offset: 0x00003635
		[BurstDiscard]
		public void Polyline(Vector3[] points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005440 File Offset: 0x00003640
		[BurstDiscard]
		public void Polyline(float3[] points, bool cycle, Color color)
		{
			this.PushColor(color);
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
			this.PopColor();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000054A1 File Offset: 0x000036A1
		[BurstDiscard]
		public void Polyline(float3[] points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000054AC File Offset: 0x000036AC
		public void Polyline(NativeArray<float3> points, bool cycle, Color color)
		{
			this.PushColor(color);
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
			this.PopColor();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000551D File Offset: 0x0000371D
		public void Polyline(NativeArray<float3> points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005528 File Offset: 0x00003728
		public void WireBox(float3 center, float3 size, Color color)
		{
			this.Reserve<Color32, CommandBuilder.BoxData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopColor | CommandBuilder.Command.PopMatrix | CommandBuilder.Command.Disc);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.BoxData>(new CommandBuilder.BoxData
			{
				center = center,
				size = size
			});
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005574 File Offset: 0x00003774
		public void WireBox(float3 center, quaternion rotation, float3 size, Color color)
		{
			this.PushColor(color);
			this.PushMatrix(float4x4.TRS(center, rotation, size));
			this.WireBox(float3.zero, new float3(1f, 1f, 1f));
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000055C2 File Offset: 0x000037C2
		public void WireBox(Bounds bounds, Color color)
		{
			this.WireBox(bounds.center, bounds.size, color);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000055E4 File Offset: 0x000037E4
		public void WireMesh(Mesh mesh, Color color)
		{
			if (mesh == null)
			{
				throw new ArgumentNullException();
			}
			this.PushColor(color);
			Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(mesh);
			Mesh.MeshData meshData = meshDataArray[0];
			CommandBuilder.JobWireMesh.JobWireMeshFunctionPointer(ref meshData, ref this);
			meshDataArray.Dispose();
			this.PopColor();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005631 File Offset: 0x00003831
		public unsafe void WireMesh(NativeArray<float3> vertices, NativeArray<int> triangles, Color color)
		{
			this.PushColor(color);
			CommandBuilder.JobWireMesh.WireMesh((float3*)vertices.GetUnsafeReadOnlyPtr<float3>(), (int*)triangles.GetUnsafeReadOnlyPtr<int>(), vertices.Length, triangles.Length, ref this);
			this.PopColor();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005660 File Offset: 0x00003860
		public void Cross(float3 position, float size, Color color)
		{
			this.PushColor(color);
			size *= 0.5f;
			this.Line(position - new float3(size, 0f, 0f), position + new float3(size, 0f, 0f));
			this.Line(position - new float3(0f, size, 0f), position + new float3(0f, size, 0f));
			this.Line(position - new float3(0f, 0f, size), position + new float3(0f, 0f, size));
			this.PopColor();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005719 File Offset: 0x00003919
		public void Cross(float3 position, Color color)
		{
			this.Cross(position, 1f, color);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005728 File Offset: 0x00003928
		[Obsolete("Use Draw.xz.Cross instead")]
		public void CrossXZ(float3 position, float size, Color color)
		{
			this.PushColor(color);
			size *= 0.5f;
			this.Line(position - new float3(size, 0f, 0f), position + new float3(size, 0f, 0f));
			this.Line(position - new float3(0f, 0f, size), position + new float3(0f, 0f, size));
			this.PopColor();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000057AF File Offset: 0x000039AF
		[Obsolete("Use Draw.xz.Cross instead")]
		public void CrossXZ(float3 position, Color color)
		{
			this.CrossXZ(position, 1f, color);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000057C0 File Offset: 0x000039C0
		[Obsolete("Use Draw.xy.Cross instead")]
		public void CrossXY(float3 position, float size, Color color)
		{
			this.PushColor(color);
			size *= 0.5f;
			this.Line(position - new float3(size, 0f, 0f), position + new float3(size, 0f, 0f));
			this.Line(position - new float3(0f, size, 0f), position + new float3(0f, size, 0f));
			this.PopColor();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005847 File Offset: 0x00003A47
		[Obsolete("Use Draw.xy.Cross instead")]
		public void CrossXY(float3 position, Color color)
		{
			this.CrossXY(position, 1f, color);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005858 File Offset: 0x00003A58
		public void Bezier(float3 p0, float3 p1, float3 p2, float3 p3, Color color)
		{
			this.PushColor(color);
			float3 @float = p0;
			for (int i = 1; i <= 20; i++)
			{
				float num = (float)i / 20f;
				float3 float2 = CommandBuilder.EvaluateCubicBezier(p0, p1, p2, p3, num);
				this.Line(@float, float2);
				@float = float2;
			}
			this.PopColor();
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000058A1 File Offset: 0x00003AA1
		public void Arrow(float3 from, float3 to, Color color)
		{
			this.ArrowRelativeSizeHead(from, to, CommandBuilder.DEFAULT_UP, 0.2f, color);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000058B8 File Offset: 0x00003AB8
		public void Arrow(float3 from, float3 to, float3 up, float headSize, Color color)
		{
			this.PushColor(color);
			float num = math.lengthsq(to - from);
			if (num > 1E-06f)
			{
				this.ArrowRelativeSizeHead(from, to, up, headSize * math.rsqrt(num));
			}
			this.PopColor();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000058FC File Offset: 0x00003AFC
		public void ArrowRelativeSizeHead(float3 from, float3 to, float3 up, float headFraction, Color color)
		{
			this.PushColor(color);
			this.Line(from, to);
			float3 @float = to - from;
			float3 float2 = math.cross(@float, up);
			if (math.all(float2 == 0f))
			{
				float2 = math.cross(new float3(1f, 0f, 0f), @float);
			}
			if (math.all(float2 == 0f))
			{
				float2 = math.cross(new float3(0f, 1f, 0f), @float);
			}
			float2 = math.normalizesafe(float2, default(float3)) * math.length(@float);
			this.Line(to, to - (@float + float2) * headFraction);
			this.Line(to, to - (@float - float2) * headFraction);
			this.PopColor();
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000059DC File Offset: 0x00003BDC
		public void ArrowheadArc(float3 origin, float3 direction, float offset, float width, Color color)
		{
			if (!math.any(direction))
			{
				return;
			}
			if (offset < 0f)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset == 0f)
			{
				return;
			}
			this.PushColor(color);
			Quaternion quaternion = Quaternion.LookRotation(direction, CommandBuilder.DEFAULT_UP);
			this.PushMatrix(Matrix4x4.TRS(origin, quaternion, Vector3.one));
			float num = 1.5707964f - width * 0.008726646f;
			float num2 = 1.5707964f + width * 0.008726646f;
			this.CircleXZInternal(float3.zero, offset, num, num2);
			float3 @float = new float3(math.cos(num), 0f, math.sin(num)) * offset;
			float3 float2 = new float3(math.cos(num2), 0f, math.sin(num2)) * offset;
			float3 float3 = new float3(0f, 0f, 1.4142f * offset);
			this.Line(@float, float3);
			this.Line(float3, float2);
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005AE1 File Offset: 0x00003CE1
		public void ArrowheadArc(float3 origin, float3 direction, float offset, Color color)
		{
			this.ArrowheadArc(origin, direction, offset, 60f, color);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005AF4 File Offset: 0x00003CF4
		public void WireGrid(float3 center, quaternion rotation, int2 cells, float2 totalSize, Color color)
		{
			this.PushColor(color);
			cells = math.max(cells, new int2(1, 1));
			this.PushMatrix(float4x4.TRS(center, rotation, new Vector3(totalSize.x, 0f, totalSize.y)));
			int x = cells.x;
			int y = cells.y;
			for (int i = 0; i <= x; i++)
			{
				this.Line(new float3((float)i / (float)x - 0.5f, 0f, -0.5f), new float3((float)i / (float)x - 0.5f, 0f, 0.5f));
			}
			for (int j = 0; j <= y; j++)
			{
				this.Line(new float3(-0.5f, 0f, (float)j / (float)y - 0.5f), new float3(0.5f, 0f, (float)j / (float)y - 0.5f));
			}
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005BE8 File Offset: 0x00003DE8
		public void WireRectangle(float3 center, quaternion rotation, float2 size, Color color)
		{
			this.WirePlane(center, rotation, size, color);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005BF8 File Offset: 0x00003DF8
		[Obsolete("Use Draw.xy.WireRectangle instead")]
		public void WireRectangle(Rect rect, Color color)
		{
			this.xy.WireRectangle(rect, color);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005C15 File Offset: 0x00003E15
		public void WirePlane(float3 center, float3 normal, float2 size, Color color)
		{
			this.PushColor(color);
			if (math.any(normal))
			{
				this.WirePlane(center, Quaternion.LookRotation(CommandBuilder.calculateTangent(normal), normal), size);
			}
			this.PopColor();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005C50 File Offset: 0x00003E50
		public void WirePlane(float3 center, quaternion rotation, float2 size, Color color)
		{
			this.Reserve<Color32, CommandBuilder.PlaneData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopMatrix | CommandBuilder.Command.Disc);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.PlaneData>(new CommandBuilder.PlaneData
			{
				center = center,
				rotation = rotation,
				size = size
			});
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005CA4 File Offset: 0x00003EA4
		public void SolidBox(float3 center, float3 size, Color color)
		{
			this.Reserve<Color32, CommandBuilder.BoxData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopColor | CommandBuilder.Command.PushMatrix | CommandBuilder.Command.Disc);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.BoxData>(new CommandBuilder.BoxData
			{
				center = center,
				size = size
			});
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005CED File Offset: 0x00003EED
		public void SolidBox(Bounds bounds, Color color)
		{
			this.SolidBox(bounds.center, bounds.size, color);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005D0E File Offset: 0x00003F0E
		public void SolidBox(float3 center, quaternion rotation, float3 size, Color color)
		{
			this.PushColor(color);
			this.PushMatrix(float4x4.TRS(center, rotation, size));
			this.SolidBox(float3.zero, Vector3.one);
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005D80 File Offset: 0x00003F80
		public static void Initialize$JobWireMesh_WireMesh_000000D1$BurstDirectCall()
		{
			CommandBuilder.JobWireMesh.WireMesh_000000D1$BurstDirectCall.Initialize();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005D87 File Offset: 0x00003F87
		public static void Initialize$JobWireMesh_Execute_000000D2$BurstDirectCall()
		{
			CommandBuilder.JobWireMesh.Execute_000000D2$BurstDirectCall.Initialize();
		}

		// Token: 0x0400001D RID: 29
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeAppendBuffer* buffer;

		// Token: 0x0400001E RID: 30
		private GCHandle gizmos;

		// Token: 0x0400001F RID: 31
		[NativeSetThreadIndex]
		private int threadIndex;

		// Token: 0x04000020 RID: 32
		private DrawingData.BuilderData.BitPackedMeta uniqueID;

		// Token: 0x04000021 RID: 33
		private static readonly float3 DEFAULT_UP = new float3(0f, 1f, 0f);

		// Token: 0x04000022 RID: 34
		internal static readonly float4x4 XZtoXYPlaneMatrix = float4x4.RotateX(-1.5707964f);

		// Token: 0x04000023 RID: 35
		internal static readonly float4x4 XZtoYZPlaneMatrix = float4x4.RotateZ(1.5707964f);

		// Token: 0x0200000C RID: 12
		[Flags]
		internal enum Command
		{
			// Token: 0x04000025 RID: 37
			PushColorInline = 256,
			// Token: 0x04000026 RID: 38
			PushColor = 0,
			// Token: 0x04000027 RID: 39
			PopColor = 1,
			// Token: 0x04000028 RID: 40
			PushMatrix = 2,
			// Token: 0x04000029 RID: 41
			PushSetMatrix = 3,
			// Token: 0x0400002A RID: 42
			PopMatrix = 4,
			// Token: 0x0400002B RID: 43
			Line = 5,
			// Token: 0x0400002C RID: 44
			Circle = 6,
			// Token: 0x0400002D RID: 45
			CircleXZ = 7,
			// Token: 0x0400002E RID: 46
			Disc = 8,
			// Token: 0x0400002F RID: 47
			DiscXZ = 9,
			// Token: 0x04000030 RID: 48
			SphereOutline = 10,
			// Token: 0x04000031 RID: 49
			Box = 11,
			// Token: 0x04000032 RID: 50
			WirePlane = 12,
			// Token: 0x04000033 RID: 51
			WireBox = 13,
			// Token: 0x04000034 RID: 52
			SolidTriangle = 14,
			// Token: 0x04000035 RID: 53
			PushPersist = 15,
			// Token: 0x04000036 RID: 54
			PopPersist = 16,
			// Token: 0x04000037 RID: 55
			Text = 17,
			// Token: 0x04000038 RID: 56
			Text3D = 18,
			// Token: 0x04000039 RID: 57
			PushLineWidth = 19,
			// Token: 0x0400003A RID: 58
			PopLineWidth = 20,
			// Token: 0x0400003B RID: 59
			CaptureState = 21
		}

		// Token: 0x0200000D RID: 13
		internal struct TriangleData
		{
			// Token: 0x0400003C RID: 60
			public float3 a;

			// Token: 0x0400003D RID: 61
			public float3 b;

			// Token: 0x0400003E RID: 62
			public float3 c;
		}

		// Token: 0x0200000E RID: 14
		internal struct LineData
		{
			// Token: 0x0400003F RID: 63
			public float3 a;

			// Token: 0x04000040 RID: 64
			public float3 b;
		}

		// Token: 0x0200000F RID: 15
		internal struct LineDataV3
		{
			// Token: 0x04000041 RID: 65
			public Vector3 a;

			// Token: 0x04000042 RID: 66
			public Vector3 b;
		}

		// Token: 0x02000010 RID: 16
		internal struct CircleXZData
		{
			// Token: 0x04000043 RID: 67
			public float3 center;

			// Token: 0x04000044 RID: 68
			public float radius;

			// Token: 0x04000045 RID: 69
			public float startAngle;

			// Token: 0x04000046 RID: 70
			public float endAngle;
		}

		// Token: 0x02000011 RID: 17
		internal struct CircleData
		{
			// Token: 0x04000047 RID: 71
			public float3 center;

			// Token: 0x04000048 RID: 72
			public float3 normal;

			// Token: 0x04000049 RID: 73
			public float radius;
		}

		// Token: 0x02000012 RID: 18
		internal struct SphereData
		{
			// Token: 0x0400004A RID: 74
			public float3 center;

			// Token: 0x0400004B RID: 75
			public float radius;
		}

		// Token: 0x02000013 RID: 19
		internal struct BoxData
		{
			// Token: 0x0400004C RID: 76
			public float3 center;

			// Token: 0x0400004D RID: 77
			public float3 size;
		}

		// Token: 0x02000014 RID: 20
		internal struct PlaneData
		{
			// Token: 0x0400004E RID: 78
			public float3 center;

			// Token: 0x0400004F RID: 79
			public quaternion rotation;

			// Token: 0x04000050 RID: 80
			public float2 size;
		}

		// Token: 0x02000015 RID: 21
		internal struct PersistData
		{
			// Token: 0x04000051 RID: 81
			public float endTime;
		}

		// Token: 0x02000016 RID: 22
		internal struct LineWidthData
		{
			// Token: 0x04000052 RID: 82
			public float pixels;

			// Token: 0x04000053 RID: 83
			public bool automaticJoins;
		}

		// Token: 0x02000017 RID: 23
		internal struct TextData
		{
			// Token: 0x04000054 RID: 84
			public float3 center;

			// Token: 0x04000055 RID: 85
			public LabelAlignment alignment;

			// Token: 0x04000056 RID: 86
			public float sizeInPixels;

			// Token: 0x04000057 RID: 87
			public int numCharacters;
		}

		// Token: 0x02000018 RID: 24
		internal struct TextData3D
		{
			// Token: 0x04000058 RID: 88
			public float3 center;

			// Token: 0x04000059 RID: 89
			public quaternion rotation;

			// Token: 0x0400005A RID: 90
			public LabelAlignment alignment;

			// Token: 0x0400005B RID: 91
			public float size;

			// Token: 0x0400005C RID: 92
			public int numCharacters;
		}

		// Token: 0x02000019 RID: 25
		public struct ScopeMatrix : IDisposable
		{
			// Token: 0x060000CC RID: 204 RVA: 0x00005D8E File Offset: 0x00003F8E
			public void Dispose()
			{
				this.builder.PopMatrix();
				this.builder.buffer = null;
			}

			// Token: 0x0400005D RID: 93
			internal CommandBuilder builder;
		}

		// Token: 0x0200001A RID: 26
		public struct ScopeColor : IDisposable
		{
			// Token: 0x060000CD RID: 205 RVA: 0x00005DA8 File Offset: 0x00003FA8
			public void Dispose()
			{
				this.builder.PopColor();
				this.builder.buffer = null;
			}

			// Token: 0x0400005E RID: 94
			internal CommandBuilder builder;
		}

		// Token: 0x0200001B RID: 27
		public struct ScopePersist : IDisposable
		{
			// Token: 0x060000CE RID: 206 RVA: 0x00005DC2 File Offset: 0x00003FC2
			public void Dispose()
			{
				this.builder.PopDuration();
				this.builder.buffer = null;
			}

			// Token: 0x0400005F RID: 95
			internal CommandBuilder builder;
		}

		// Token: 0x0200001C RID: 28
		public struct ScopeEmpty : IDisposable
		{
			// Token: 0x060000CF RID: 207 RVA: 0x00002104 File Offset: 0x00000304
			public void Dispose()
			{
			}
		}

		// Token: 0x0200001D RID: 29
		public struct ScopeLineWidth : IDisposable
		{
			// Token: 0x060000D0 RID: 208 RVA: 0x00005DDC File Offset: 0x00003FDC
			public void Dispose()
			{
				this.builder.PopLineWidth();
				this.builder.buffer = null;
			}

			// Token: 0x04000060 RID: 96
			internal CommandBuilder builder;
		}

		// Token: 0x0200001E RID: 30
		public enum SymbolDecoration : byte
		{
			// Token: 0x04000062 RID: 98
			None,
			// Token: 0x04000063 RID: 99
			ArrowHead,
			// Token: 0x04000064 RID: 100
			Circle
		}

		// Token: 0x0200001F RID: 31
		public struct PolylineWithSymbol
		{
			// Token: 0x060000D1 RID: 209 RVA: 0x00005DF8 File Offset: 0x00003FF8
			public PolylineWithSymbol(CommandBuilder.SymbolDecoration symbol, float symbolSize, float symbolPadding, float symbolSpacing, bool reverseSymbols = false, float offset = 0f)
			{
				if (symbolSpacing <= 1.1754944E-38f)
				{
					throw new ArgumentOutOfRangeException("symbolSpacing", "Symbol spacing must be greater than zero");
				}
				if (symbolSize <= 1.1754944E-38f)
				{
					throw new ArgumentOutOfRangeException("symbolSize", "Symbol size must be greater than zero");
				}
				if (symbolPadding < 0f)
				{
					throw new ArgumentOutOfRangeException("symbolPadding", "Symbol padding must non-negative");
				}
				this.prev = float3.zero;
				this.symbol = symbol;
				this.symbolSize = symbolSize;
				this.symbolPadding = symbolPadding;
				this.connectingSegmentLength = math.max(0f, symbolSpacing - symbolPadding * 2f - symbolSize);
				symbolSpacing = symbolPadding * 2f + symbolSize + this.connectingSegmentLength;
				this.reverseSymbols = reverseSymbols;
				this.up = new float3(0f, 1f, 0f);
				this.symbolOffset = ((symbol == CommandBuilder.SymbolDecoration.ArrowHead) ? (-0.25f * symbolSize) : 0f);
				if (reverseSymbols)
				{
					this.symbolOffset = -this.symbolOffset;
				}
				this.symbolOffset += 0.5f * symbolSize;
				this.offset = (this.connectingSegmentLength * 0.5f + offset) % symbolSpacing;
				if (this.offset > 0f)
				{
					this.offset -= symbolSpacing;
				}
				this.state = CommandBuilder.PolylineWithSymbol.State.NotStarted;
			}

			// Token: 0x060000D2 RID: 210 RVA: 0x00005F38 File Offset: 0x00004138
			public void MoveTo(ref CommandBuilder draw, float3 next)
			{
				if (this.state == CommandBuilder.PolylineWithSymbol.State.NotStarted)
				{
					this.prev = next;
					this.state = CommandBuilder.PolylineWithSymbol.State.ConnectingSegment;
					return;
				}
				float num = math.length(next - this.prev);
				float num2 = math.rcp(num);
				float3 @float = next - this.prev;
				float3 float2 = default(float3);
				if (this.symbol != CommandBuilder.SymbolDecoration.None)
				{
					float2 = math.normalizesafe(math.cross(@float, math.cross(@float, this.up)), default(float3));
					if (math.all(float2 == 0f))
					{
						float2 = new float3(0f, 0f, 1f);
					}
					if (this.reverseSymbols)
					{
						@float = -@float;
					}
				}
				float num3 = 0f;
				for (;;)
				{
					if (this.state == CommandBuilder.PolylineWithSymbol.State.ConnectingSegment)
					{
						if (this.offset >= 0f && this.offset != num3)
						{
							num3 = math.max(0f, num3);
							float3 float3 = math.lerp(this.prev, next, num3 * num2);
							float3 float4 = math.lerp(this.prev, next, math.min(this.offset * num2, 1f));
							draw.Line(float3, float4);
						}
						if (this.offset >= num)
						{
							goto IL_0282;
						}
						this.state = CommandBuilder.PolylineWithSymbol.State.PreSymbolPadding;
						num3 = this.offset;
						this.offset += this.symbolPadding;
					}
					else if (this.state == CommandBuilder.PolylineWithSymbol.State.PreSymbolPadding)
					{
						if (this.offset >= num)
						{
							goto IL_0282;
						}
						this.state = CommandBuilder.PolylineWithSymbol.State.Symbol;
						num3 = this.offset;
						this.offset += this.symbolOffset;
					}
					else if (this.state == CommandBuilder.PolylineWithSymbol.State.Symbol)
					{
						if (this.offset >= num)
						{
							goto IL_0282;
						}
						if (this.offset >= 0f)
						{
							float3 float5 = math.lerp(this.prev, next, this.offset * num2);
							switch (this.symbol)
							{
							case CommandBuilder.SymbolDecoration.None:
								goto IL_0208;
							case CommandBuilder.SymbolDecoration.ArrowHead:
								draw.Arrowhead(float5, @float, float2, this.symbolSize);
								goto IL_0208;
							}
							draw.Circle(float5, float2, this.symbolSize * 0.5f);
						}
						IL_0208:
						this.state = CommandBuilder.PolylineWithSymbol.State.PostSymbolPadding;
						num3 = this.offset;
						this.offset += -this.symbolOffset + this.symbolSize + this.symbolPadding;
					}
					else
					{
						if (this.state != CommandBuilder.PolylineWithSymbol.State.PostSymbolPadding)
						{
							break;
						}
						if (this.offset >= num)
						{
							goto IL_0282;
						}
						this.state = CommandBuilder.PolylineWithSymbol.State.ConnectingSegment;
						num3 = this.offset;
						this.offset += this.connectingSegmentLength;
					}
				}
				throw new Exception("Invalid state");
				IL_0282:
				this.offset -= num;
				this.prev = next;
			}

			// Token: 0x04000065 RID: 101
			private float3 prev;

			// Token: 0x04000066 RID: 102
			private float offset;

			// Token: 0x04000067 RID: 103
			private readonly float symbolSize;

			// Token: 0x04000068 RID: 104
			private readonly float connectingSegmentLength;

			// Token: 0x04000069 RID: 105
			private readonly float symbolPadding;

			// Token: 0x0400006A RID: 106
			private readonly float symbolOffset;

			// Token: 0x0400006B RID: 107
			public float3 up;

			// Token: 0x0400006C RID: 108
			private readonly CommandBuilder.SymbolDecoration symbol;

			// Token: 0x0400006D RID: 109
			private CommandBuilder.PolylineWithSymbol.State state;

			// Token: 0x0400006E RID: 110
			private readonly bool reverseSymbols;

			// Token: 0x02000020 RID: 32
			private enum State : byte
			{
				// Token: 0x04000070 RID: 112
				NotStarted,
				// Token: 0x04000071 RID: 113
				ConnectingSegment,
				// Token: 0x04000072 RID: 114
				PreSymbolPadding,
				// Token: 0x04000073 RID: 115
				Symbol,
				// Token: 0x04000074 RID: 116
				PostSymbolPadding
			}
		}

		// Token: 0x02000021 RID: 33
		[BurstCompile]
		private class JobWireMesh
		{
			// Token: 0x060000D3 RID: 211 RVA: 0x000061DC File Offset: 0x000043DC
			[BurstCompile]
			public unsafe static void WireMesh(float3* verts, int* indices, int vertexCount, int indexCount, ref CommandBuilder draw)
			{
				CommandBuilder.JobWireMesh.WireMesh_000000D1$BurstDirectCall.Invoke(verts, indices, vertexCount, indexCount, ref draw);
			}

			// Token: 0x060000D4 RID: 212 RVA: 0x000061E9 File Offset: 0x000043E9
			[BurstCompile]
			[MonoPInvokeCallback(typeof(CommandBuilder.JobWireMesh.JobWireMeshDelegate))]
			private static void Execute(ref Mesh.MeshData rawMeshData, ref CommandBuilder draw)
			{
				CommandBuilder.JobWireMesh.Execute_000000D2$BurstDirectCall.Invoke(ref rawMeshData, ref draw);
			}

			// Token: 0x060000D7 RID: 215 RVA: 0x00006220 File Offset: 0x00004420
			[BurstCompile]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static void WireMesh$BurstManaged(float3* verts, int* indices, int vertexCount, int indexCount, ref CommandBuilder draw)
			{
				if (indexCount % 3 != 0)
				{
					throw new ArgumentException("Invalid index count. Must be a multiple of 3");
				}
				NativeHashMap<int2, bool> nativeHashMap = new NativeHashMap<int2, bool>(indexCount, Allocator.Temp);
				for (int i = 0; i < indexCount; i += 3)
				{
					int num = indices[i];
					int num2 = indices[i + 1];
					int num3 = indices[i + 2];
					if (num < 0 || num2 < 0 || num3 < 0 || num >= vertexCount || num2 >= vertexCount || num3 >= vertexCount)
					{
						throw new Exception("Invalid vertex index. Index out of bounds");
					}
					int num4 = math.min(num, num2);
					int num5 = math.max(num, num2);
					if (!nativeHashMap.ContainsKey(new int2(num4, num5)))
					{
						nativeHashMap.Add(new int2(num4, num5), true);
						draw.Line(verts[num4], verts[num5]);
					}
					num4 = math.min(num2, num3);
					num5 = math.max(num2, num3);
					if (!nativeHashMap.ContainsKey(new int2(num4, num5)))
					{
						nativeHashMap.Add(new int2(num4, num5), true);
						draw.Line(verts[num4], verts[num5]);
					}
					num4 = math.min(num3, num);
					num5 = math.max(num3, num);
					if (!nativeHashMap.ContainsKey(new int2(num4, num5)))
					{
						nativeHashMap.Add(new int2(num4, num5), true);
						draw.Line(verts[num4], verts[num5]);
					}
				}
			}

			// Token: 0x060000D8 RID: 216 RVA: 0x000063C0 File Offset: 0x000045C0
			[BurstCompile]
			[MonoPInvokeCallback(typeof(CommandBuilder.JobWireMesh.JobWireMeshDelegate))]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static void Execute$BurstManaged(ref Mesh.MeshData rawMeshData, ref CommandBuilder draw)
			{
				int num = 0;
				for (int i = 0; i < rawMeshData.subMeshCount; i++)
				{
					num = math.max(num, rawMeshData.GetSubMesh(i).indexCount);
				}
				NativeArray<int> nativeArray = new NativeArray<int>(num, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				NativeArray<Vector3> nativeArray2 = new NativeArray<Vector3>(rawMeshData.vertexCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				rawMeshData.GetVertices(nativeArray2);
				for (int j = 0; j < rawMeshData.subMeshCount; j++)
				{
					SubMeshDescriptor subMesh = rawMeshData.GetSubMesh(j);
					rawMeshData.GetIndices(nativeArray, j, true);
					CommandBuilder.JobWireMesh.WireMesh((float3*)nativeArray2.GetUnsafeReadOnlyPtr<Vector3>(), (int*)nativeArray.GetUnsafeReadOnlyPtr<int>(), nativeArray2.Length, subMesh.indexCount, ref draw);
				}
			}

			// Token: 0x04000075 RID: 117
			public static readonly CommandBuilder.JobWireMesh.JobWireMeshDelegate JobWireMeshFunctionPointer = BurstCompiler.CompileFunctionPointer<CommandBuilder.JobWireMesh.JobWireMeshDelegate>(new CommandBuilder.JobWireMesh.JobWireMeshDelegate(CommandBuilder.JobWireMesh.Execute)).Invoke;

			// Token: 0x02000022 RID: 34
			// (Invoke) Token: 0x060000DA RID: 218
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void JobWireMeshDelegate(ref Mesh.MeshData rawMeshData, ref CommandBuilder draw);

			// Token: 0x02000023 RID: 35
			// (Invoke) Token: 0x060000DE RID: 222
			internal unsafe delegate void WireMesh_000000D1$PostfixBurstDelegate(float3* verts, int* indices, int vertexCount, int indexCount, ref CommandBuilder draw);

			// Token: 0x02000024 RID: 36
			internal static class WireMesh_000000D1$BurstDirectCall
			{
				// Token: 0x060000E1 RID: 225 RVA: 0x00006460 File Offset: 0x00004660
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (CommandBuilder.JobWireMesh.WireMesh_000000D1$BurstDirectCall.Pointer == 0)
					{
						CommandBuilder.JobWireMesh.WireMesh_000000D1$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(CommandBuilder.JobWireMesh.WireMesh_000000D1$BurstDirectCall.DeferredCompilation, methodof(CommandBuilder.JobWireMesh.WireMesh$BurstManaged(float3*, int*, int, int, ref CommandBuilder)).MethodHandle, typeof(CommandBuilder.JobWireMesh.WireMesh_000000D1$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = CommandBuilder.JobWireMesh.WireMesh_000000D1$BurstDirectCall.Pointer;
				}

				// Token: 0x060000E2 RID: 226 RVA: 0x0000648C File Offset: 0x0000468C
				private static IntPtr GetFunctionPointer()
				{
					IntPtr intPtr = (IntPtr)0;
					CommandBuilder.JobWireMesh.WireMesh_000000D1$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
					return intPtr;
				}

				// Token: 0x060000E3 RID: 227 RVA: 0x000064A4 File Offset: 0x000046A4
				public unsafe static void Constructor()
				{
					CommandBuilder.JobWireMesh.WireMesh_000000D1$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(CommandBuilder.JobWireMesh.WireMesh(float3*, int*, int, int, ref CommandBuilder)).MethodHandle);
				}

				// Token: 0x060000E4 RID: 228 RVA: 0x00002104 File Offset: 0x00000304
				public static void Initialize()
				{
				}

				// Token: 0x060000E5 RID: 229 RVA: 0x000064B5 File Offset: 0x000046B5
				// Note: this type is marked as 'beforefieldinit'.
				static WireMesh_000000D1$BurstDirectCall()
				{
					CommandBuilder.JobWireMesh.WireMesh_000000D1$BurstDirectCall.Constructor();
				}

				// Token: 0x060000E6 RID: 230 RVA: 0x000064BC File Offset: 0x000046BC
				public unsafe static void Invoke(float3* verts, int* indices, int vertexCount, int indexCount, ref CommandBuilder draw)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = CommandBuilder.JobWireMesh.WireMesh_000000D1$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(Unity.Mathematics.float3*,System.Int32*,System.Int32,System.Int32,Pathfinding.Drawing.CommandBuilder&), verts, indices, vertexCount, indexCount, ref draw, functionPointer);
							return;
						}
					}
					CommandBuilder.JobWireMesh.WireMesh$BurstManaged(verts, indices, vertexCount, indexCount, ref draw);
				}

				// Token: 0x04000076 RID: 118
				private static IntPtr Pointer;

				// Token: 0x04000077 RID: 119
				private static IntPtr DeferredCompilation;
			}

			// Token: 0x02000025 RID: 37
			// (Invoke) Token: 0x060000E8 RID: 232
			internal delegate void Execute_000000D2$PostfixBurstDelegate(ref Mesh.MeshData rawMeshData, ref CommandBuilder draw);

			// Token: 0x02000026 RID: 38
			internal static class Execute_000000D2$BurstDirectCall
			{
				// Token: 0x060000EB RID: 235 RVA: 0x000064F7 File Offset: 0x000046F7
				[BurstDiscard]
				private static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (CommandBuilder.JobWireMesh.Execute_000000D2$BurstDirectCall.Pointer == 0)
					{
						CommandBuilder.JobWireMesh.Execute_000000D2$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(CommandBuilder.JobWireMesh.Execute_000000D2$BurstDirectCall.DeferredCompilation, methodof(CommandBuilder.JobWireMesh.Execute$BurstManaged(ref Mesh.MeshData, ref CommandBuilder)).MethodHandle, typeof(CommandBuilder.JobWireMesh.Execute_000000D2$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = CommandBuilder.JobWireMesh.Execute_000000D2$BurstDirectCall.Pointer;
				}

				// Token: 0x060000EC RID: 236 RVA: 0x00006524 File Offset: 0x00004724
				private static IntPtr GetFunctionPointer()
				{
					IntPtr intPtr = (IntPtr)0;
					CommandBuilder.JobWireMesh.Execute_000000D2$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
					return intPtr;
				}

				// Token: 0x060000ED RID: 237 RVA: 0x0000653C File Offset: 0x0000473C
				public static void Constructor()
				{
					CommandBuilder.JobWireMesh.Execute_000000D2$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(CommandBuilder.JobWireMesh.Execute(ref Mesh.MeshData, ref CommandBuilder)).MethodHandle);
				}

				// Token: 0x060000EE RID: 238 RVA: 0x00002104 File Offset: 0x00000304
				public static void Initialize()
				{
				}

				// Token: 0x060000EF RID: 239 RVA: 0x0000654D File Offset: 0x0000474D
				// Note: this type is marked as 'beforefieldinit'.
				static Execute_000000D2$BurstDirectCall()
				{
					CommandBuilder.JobWireMesh.Execute_000000D2$BurstDirectCall.Constructor();
				}

				// Token: 0x060000F0 RID: 240 RVA: 0x00006554 File Offset: 0x00004754
				public static void Invoke(ref Mesh.MeshData rawMeshData, ref CommandBuilder draw)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = CommandBuilder.JobWireMesh.Execute_000000D2$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(UnityEngine.Mesh/MeshData&,Pathfinding.Drawing.CommandBuilder&), ref rawMeshData, ref draw, functionPointer);
							return;
						}
					}
					CommandBuilder.JobWireMesh.Execute$BurstManaged(ref rawMeshData, ref draw);
				}

				// Token: 0x04000078 RID: 120
				private static IntPtr Pointer;

				// Token: 0x04000079 RID: 121
				private static IntPtr DeferredCompilation;
			}
		}
	}
}
