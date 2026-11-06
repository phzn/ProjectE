using System;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000023 RID: 35
	public class MultiParentConstraintJobBinder<T> : AnimationJobBinder<MultiParentConstraintJob, T> where T : struct, IAnimationJobData, IMultiParentConstraintData
	{
		// Token: 0x0600009B RID: 155 RVA: 0x0000422C File Offset: 0x0000242C
		public override MultiParentConstraintJob Create(Animator animator, ref T data, Component component)
		{
			MultiParentConstraintJob multiParentConstraintJob = default(MultiParentConstraintJob);
			multiParentConstraintJob.driven = ReadWriteTransformHandle.Bind(animator, data.constrainedObject);
			multiParentConstraintJob.drivenParent = ReadOnlyTransformHandle.Bind(animator, data.constrainedObject.parent);
			WeightedTransformArray sourceObjects = data.sourceObjects;
			WeightedTransformArrayBinder.BindReadOnlyTransforms(animator, component, sourceObjects, out multiParentConstraintJob.sourceTransforms);
			WeightedTransformArrayBinder.BindWeights(animator, component, sourceObjects, data.sourceObjectsProperty, out multiParentConstraintJob.sourceWeights);
			multiParentConstraintJob.sourceOffsets = new NativeArray<AffineTransform>(sourceObjects.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			multiParentConstraintJob.weightBuffer = new NativeArray<float>(sourceObjects.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			AffineTransform affineTransform = new AffineTransform(data.constrainedObject.position, data.constrainedObject.rotation);
			for (int i = 0; i < sourceObjects.Count; i++)
			{
				Transform transform = sourceObjects[i].transform;
				AffineTransform affineTransform2 = new AffineTransform(transform.position, transform.rotation);
				AffineTransform identity = AffineTransform.identity;
				AffineTransform affineTransform3 = affineTransform2.InverseMul(affineTransform);
				if (data.maintainPositionOffset)
				{
					identity.translation = affineTransform3.translation;
				}
				if (data.maintainRotationOffset)
				{
					identity.rotation = affineTransform3.rotation;
				}
				multiParentConstraintJob.sourceOffsets[i] = identity;
			}
			multiParentConstraintJob.positionAxesMask = new Vector3(Convert.ToSingle(data.constrainedPositionXAxis), Convert.ToSingle(data.constrainedPositionYAxis), Convert.ToSingle(data.constrainedPositionZAxis));
			multiParentConstraintJob.rotationAxesMask = new Vector3(Convert.ToSingle(data.constrainedRotationXAxis), Convert.ToSingle(data.constrainedRotationYAxis), Convert.ToSingle(data.constrainedRotationZAxis));
			return multiParentConstraintJob;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004415 File Offset: 0x00002615
		public override void Destroy(MultiParentConstraintJob job)
		{
			job.sourceTransforms.Dispose();
			job.sourceWeights.Dispose();
			job.sourceOffsets.Dispose();
			job.weightBuffer.Dispose();
		}
	}
}
