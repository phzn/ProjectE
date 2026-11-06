using System;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000020 RID: 32
	public class MultiAimConstraintJobBinder<T> : AnimationJobBinder<MultiAimConstraintJob, T> where T : struct, IAnimationJobData, IMultiAimConstraintData
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00003CB4 File Offset: 0x00001EB4
		public override MultiAimConstraintJob Create(Animator animator, ref T data, Component component)
		{
			MultiAimConstraintJob multiAimConstraintJob = default(MultiAimConstraintJob);
			multiAimConstraintJob.driven = ReadWriteTransformHandle.Bind(animator, data.constrainedObject);
			multiAimConstraintJob.drivenParent = ReadOnlyTransformHandle.Bind(animator, data.constrainedObject.parent);
			multiAimConstraintJob.aimAxis = data.aimAxis;
			multiAimConstraintJob.upAxis = data.upAxis;
			multiAimConstraintJob.worldUpType = (MultiAimConstraintJob.WorldUpType)data.worldUpType;
			multiAimConstraintJob.worldUpAxis = data.worldUpAxis;
			if (data.worldUpObject != null)
			{
				multiAimConstraintJob.worldUpObject = ReadOnlyTransformHandle.Bind(animator, data.worldUpObject);
			}
			WeightedTransformArray sourceObjects = data.sourceObjects;
			WeightedTransformArrayBinder.BindReadOnlyTransforms(animator, component, sourceObjects, out multiAimConstraintJob.sourceTransforms);
			WeightedTransformArrayBinder.BindWeights(animator, component, sourceObjects, data.sourceObjectsProperty, out multiAimConstraintJob.sourceWeights);
			multiAimConstraintJob.sourceOffsets = new NativeArray<Quaternion>(sourceObjects.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			multiAimConstraintJob.weightBuffer = new NativeArray<float>(sourceObjects.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < sourceObjects.Count; i++)
			{
				if (data.maintainOffset)
				{
					Vector3 vector = data.constrainedObject.rotation * data.aimAxis;
					multiAimConstraintJob.sourceOffsets[i] = QuaternionExt.FromToRotation(sourceObjects[i].transform.position - data.constrainedObject.position, vector);
				}
				else
				{
					multiAimConstraintJob.sourceOffsets[i] = Quaternion.identity;
				}
			}
			multiAimConstraintJob.minLimit = FloatProperty.Bind(animator, component, data.minLimitFloatProperty);
			multiAimConstraintJob.maxLimit = FloatProperty.Bind(animator, component, data.maxLimitFloatProperty);
			multiAimConstraintJob.drivenOffset = Vector3Property.Bind(animator, component, data.offsetVector3Property);
			multiAimConstraintJob.axesMask = new Vector3(Convert.ToSingle(data.constrainedXAxis), Convert.ToSingle(data.constrainedYAxis), Convert.ToSingle(data.constrainedZAxis));
			return multiAimConstraintJob;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003F00 File Offset: 0x00002100
		public override void Destroy(MultiAimConstraintJob job)
		{
			job.sourceTransforms.Dispose();
			job.sourceWeights.Dispose();
			job.sourceOffsets.Dispose();
			job.weightBuffer.Dispose();
		}
	}
}
