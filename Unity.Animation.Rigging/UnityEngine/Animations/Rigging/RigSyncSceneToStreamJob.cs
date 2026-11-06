using System;
using Unity.Burst;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000030 RID: 48
	[BurstCompile]
	internal struct RigSyncSceneToStreamJob : IAnimationJob
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x00005331 File Offset: 0x00003531
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005334 File Offset: 0x00003534
		public void ProcessAnimation(AnimationStream stream)
		{
			this.transformSyncer.Sync(ref stream);
			this.propertySyncer.Sync(ref stream);
			this.rigWeightSyncer.Sync(ref stream);
			this.constraintWeightSyncer.Sync(ref stream);
			NativeArray<float> nativeArray = this.rigWeightSyncer.StreamValues(ref stream);
			NativeArray<float> nativeArray2 = this.constraintWeightSyncer.StreamValues(ref stream);
			int num = 0;
			int i = 0;
			int length = nativeArray2.Length;
			while (i < length)
			{
				if (i >= this.rigConstraintEndIdx[num])
				{
					num++;
				}
				ref NativeArray<float> ptr = ref nativeArray2;
				int num2 = i;
				ptr[num2] *= nativeArray[num] * this.rigStates[num];
				i++;
			}
			AnimationStreamHandleUtility.WriteFloats(stream, this.modulatedConstraintWeights, nativeArray2, false);
		}

		// Token: 0x0400007B RID: 123
		public RigSyncSceneToStreamJob.TransformSyncer transformSyncer;

		// Token: 0x0400007C RID: 124
		public RigSyncSceneToStreamJob.PropertySyncer propertySyncer;

		// Token: 0x0400007D RID: 125
		public RigSyncSceneToStreamJob.PropertySyncer rigWeightSyncer;

		// Token: 0x0400007E RID: 126
		public RigSyncSceneToStreamJob.PropertySyncer constraintWeightSyncer;

		// Token: 0x0400007F RID: 127
		public NativeArray<float> rigStates;

		// Token: 0x04000080 RID: 128
		public NativeArray<int> rigConstraintEndIdx;

		// Token: 0x04000081 RID: 129
		public NativeArray<PropertyStreamHandle> modulatedConstraintWeights;

		// Token: 0x0200007D RID: 125
		public struct TransformSyncer : IDisposable
		{
			// Token: 0x0600030B RID: 779 RVA: 0x0000A3F8 File Offset: 0x000085F8
			public static RigSyncSceneToStreamJob.TransformSyncer Create(int size)
			{
				return new RigSyncSceneToStreamJob.TransformSyncer
				{
					sceneHandles = new NativeArray<TransformSceneHandle>(size, Allocator.Persistent, NativeArrayOptions.UninitializedMemory),
					streamHandles = new NativeArray<TransformStreamHandle>(size, Allocator.Persistent, NativeArrayOptions.UninitializedMemory)
				};
			}

			// Token: 0x0600030C RID: 780 RVA: 0x0000A42C File Offset: 0x0000862C
			public void Dispose()
			{
				if (this.sceneHandles.IsCreated)
				{
					this.sceneHandles.Dispose();
				}
				if (this.streamHandles.IsCreated)
				{
					this.streamHandles.Dispose();
				}
			}

			// Token: 0x0600030D RID: 781 RVA: 0x0000A45E File Offset: 0x0000865E
			public void BindAt(int index, Animator animator, Transform transform)
			{
				this.sceneHandles[index] = animator.BindSceneTransform(transform);
				this.streamHandles[index] = animator.BindStreamTransform(transform);
			}

			// Token: 0x0600030E RID: 782 RVA: 0x0000A488 File Offset: 0x00008688
			public void Sync(ref AnimationStream stream)
			{
				int i = 0;
				int length = this.sceneHandles.Length;
				while (i < length)
				{
					TransformSceneHandle transformSceneHandle = this.sceneHandles[i];
					if (transformSceneHandle.IsValid(stream))
					{
						TransformStreamHandle transformStreamHandle = this.streamHandles[i];
						Vector3 vector;
						Quaternion quaternion;
						Vector3 vector2;
						transformSceneHandle.GetLocalTRS(stream, out vector, out quaternion, out vector2);
						transformStreamHandle.SetLocalTRS(stream, vector, quaternion, vector2, true);
						this.streamHandles[i] = transformStreamHandle;
						this.sceneHandles[i] = transformSceneHandle;
					}
					i++;
				}
			}

			// Token: 0x04000155 RID: 341
			public NativeArray<TransformSceneHandle> sceneHandles;

			// Token: 0x04000156 RID: 342
			public NativeArray<TransformStreamHandle> streamHandles;
		}

		// Token: 0x0200007E RID: 126
		internal struct PropertySyncer : IDisposable
		{
			// Token: 0x0600030F RID: 783 RVA: 0x0000A518 File Offset: 0x00008718
			public static RigSyncSceneToStreamJob.PropertySyncer Create(int size)
			{
				return new RigSyncSceneToStreamJob.PropertySyncer
				{
					sceneHandles = new NativeArray<PropertySceneHandle>(size, Allocator.Persistent, NativeArrayOptions.UninitializedMemory),
					streamHandles = new NativeArray<PropertyStreamHandle>(size, Allocator.Persistent, NativeArrayOptions.UninitializedMemory),
					buffer = new NativeArray<float>(size, Allocator.Persistent, NativeArrayOptions.UninitializedMemory)
				};
			}

			// Token: 0x06000310 RID: 784 RVA: 0x0000A55C File Offset: 0x0000875C
			public void Dispose()
			{
				if (this.sceneHandles.IsCreated)
				{
					this.sceneHandles.Dispose();
				}
				if (this.streamHandles.IsCreated)
				{
					this.streamHandles.Dispose();
				}
				if (this.buffer.IsCreated)
				{
					this.buffer.Dispose();
				}
			}

			// Token: 0x06000311 RID: 785 RVA: 0x0000A5B4 File Offset: 0x000087B4
			public void BindAt(int index, Animator animator, Component component, string property)
			{
				this.sceneHandles[index] = animator.BindSceneProperty(component.transform, component.GetType(), property);
				this.streamHandles[index] = animator.BindStreamProperty(component.transform, component.GetType(), property);
			}

			// Token: 0x06000312 RID: 786 RVA: 0x0000A601 File Offset: 0x00008801
			public void Sync(ref AnimationStream stream)
			{
				AnimationSceneHandleUtility.ReadFloats(stream, this.sceneHandles, this.buffer);
				AnimationStreamHandleUtility.WriteFloats(stream, this.streamHandles, this.buffer, true);
			}

			// Token: 0x06000313 RID: 787 RVA: 0x0000A632 File Offset: 0x00008832
			public NativeArray<float> StreamValues(ref AnimationStream stream)
			{
				AnimationStreamHandleUtility.ReadFloats(stream, this.streamHandles, this.buffer);
				return this.buffer;
			}

			// Token: 0x04000157 RID: 343
			public NativeArray<PropertySceneHandle> sceneHandles;

			// Token: 0x04000158 RID: 344
			public NativeArray<PropertyStreamHandle> streamHandles;

			// Token: 0x04000159 RID: 345
			public NativeArray<float> buffer;
		}
	}
}
