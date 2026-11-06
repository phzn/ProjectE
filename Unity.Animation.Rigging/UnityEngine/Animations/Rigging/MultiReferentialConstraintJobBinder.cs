using System;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000029 RID: 41
	public class MultiReferentialConstraintJobBinder<T> : AnimationJobBinder<MultiReferentialConstraintJob, T> where T : struct, IAnimationJobData, IMultiReferentialConstraintData
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x00004990 File Offset: 0x00002B90
		public override MultiReferentialConstraintJob Create(Animator animator, ref T data, Component component)
		{
			MultiReferentialConstraintJob multiReferentialConstraintJob = default(MultiReferentialConstraintJob);
			Transform[] sourceObjects = data.sourceObjects;
			multiReferentialConstraintJob.driver = IntProperty.Bind(animator, component, data.driverIntProperty);
			multiReferentialConstraintJob.sources = new NativeArray<ReadWriteTransformHandle>(sourceObjects.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			multiReferentialConstraintJob.sourceBindTx = new NativeArray<AffineTransform>(sourceObjects.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			multiReferentialConstraintJob.offsetTx = new NativeArray<AffineTransform>(sourceObjects.Length - 1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < sourceObjects.Length; i++)
			{
				multiReferentialConstraintJob.sources[i] = ReadWriteTransformHandle.Bind(animator, sourceObjects[i].transform);
				multiReferentialConstraintJob.sourceBindTx[i] = new AffineTransform(sourceObjects[i].position, sourceObjects[i].rotation);
			}
			multiReferentialConstraintJob.UpdateOffsets(data.driverValue);
			return multiReferentialConstraintJob;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004A60 File Offset: 0x00002C60
		public override void Destroy(MultiReferentialConstraintJob job)
		{
			job.sources.Dispose();
			job.sourceBindTx.Dispose();
			job.offsetTx.Dispose();
		}
	}
}
