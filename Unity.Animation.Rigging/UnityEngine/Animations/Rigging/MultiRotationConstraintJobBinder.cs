using System;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200002C RID: 44
	public class MultiRotationConstraintJobBinder<T> : AnimationJobBinder<MultiRotationConstraintJob, T> where T : struct, IAnimationJobData, IMultiRotationConstraintData
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00004C90 File Offset: 0x00002E90
		public override MultiRotationConstraintJob Create(Animator animator, ref T data, Component component)
		{
			MultiRotationConstraintJob multiRotationConstraintJob = default(MultiRotationConstraintJob);
			multiRotationConstraintJob.driven = ReadWriteTransformHandle.Bind(animator, data.constrainedObject);
			multiRotationConstraintJob.drivenParent = ReadOnlyTransformHandle.Bind(animator, data.constrainedObject.parent);
			multiRotationConstraintJob.drivenOffset = Vector3Property.Bind(animator, component, data.offsetVector3Property);
			WeightedTransformArray sourceObjects = data.sourceObjects;
			WeightedTransformArrayBinder.BindReadOnlyTransforms(animator, component, sourceObjects, out multiRotationConstraintJob.sourceTransforms);
			WeightedTransformArrayBinder.BindWeights(animator, component, sourceObjects, data.sourceObjectsProperty, out multiRotationConstraintJob.sourceWeights);
			multiRotationConstraintJob.sourceOffsets = new NativeArray<Quaternion>(sourceObjects.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			multiRotationConstraintJob.weightBuffer = new NativeArray<float>(sourceObjects.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			Quaternion rotation = data.constrainedObject.rotation;
			for (int i = 0; i < sourceObjects.Count; i++)
			{
				multiRotationConstraintJob.sourceOffsets[i] = (data.maintainOffset ? (Quaternion.Inverse(sourceObjects[i].transform.rotation) * rotation) : Quaternion.identity);
			}
			multiRotationConstraintJob.axesMask = new Vector3(Convert.ToSingle(data.constrainedXAxis), Convert.ToSingle(data.constrainedYAxis), Convert.ToSingle(data.constrainedZAxis));
			return multiRotationConstraintJob;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004DFA File Offset: 0x00002FFA
		public override void Destroy(MultiRotationConstraintJob job)
		{
			job.sourceTransforms.Dispose();
			job.sourceWeights.Dispose();
			job.sourceOffsets.Dispose();
			job.weightBuffer.Dispose();
		}
	}
}
