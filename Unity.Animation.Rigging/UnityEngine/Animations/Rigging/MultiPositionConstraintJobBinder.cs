using System;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000026 RID: 38
	public class MultiPositionConstraintJobBinder<T> : AnimationJobBinder<MultiPositionConstraintJob, T> where T : struct, IAnimationJobData, IMultiPositionConstraintData
	{
		// Token: 0x060000AA RID: 170 RVA: 0x00004620 File Offset: 0x00002820
		public override MultiPositionConstraintJob Create(Animator animator, ref T data, Component component)
		{
			MultiPositionConstraintJob multiPositionConstraintJob = default(MultiPositionConstraintJob);
			multiPositionConstraintJob.driven = ReadWriteTransformHandle.Bind(animator, data.constrainedObject);
			multiPositionConstraintJob.drivenParent = ReadOnlyTransformHandle.Bind(animator, data.constrainedObject.parent);
			multiPositionConstraintJob.drivenOffset = Vector3Property.Bind(animator, component, data.offsetVector3Property);
			WeightedTransformArray sourceObjects = data.sourceObjects;
			WeightedTransformArrayBinder.BindReadOnlyTransforms(animator, component, sourceObjects, out multiPositionConstraintJob.sourceTransforms);
			WeightedTransformArrayBinder.BindWeights(animator, component, sourceObjects, data.sourceObjectsProperty, out multiPositionConstraintJob.sourceWeights);
			multiPositionConstraintJob.sourceOffsets = new NativeArray<Vector3>(sourceObjects.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			multiPositionConstraintJob.weightBuffer = new NativeArray<float>(sourceObjects.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			Vector3 position = data.constrainedObject.position;
			for (int i = 0; i < sourceObjects.Count; i++)
			{
				multiPositionConstraintJob.sourceOffsets[i] = (data.maintainOffset ? (position - sourceObjects[i].transform.position) : Vector3.zero);
			}
			multiPositionConstraintJob.axesMask = new Vector3(Convert.ToSingle(data.constrainedXAxis), Convert.ToSingle(data.constrainedYAxis), Convert.ToSingle(data.constrainedZAxis));
			return multiPositionConstraintJob;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004785 File Offset: 0x00002985
		public override void Destroy(MultiPositionConstraintJob job)
		{
			job.sourceTransforms.Dispose();
			job.sourceWeights.Dispose();
			job.sourceOffsets.Dispose();
			job.weightBuffer.Dispose();
		}
	}
}
