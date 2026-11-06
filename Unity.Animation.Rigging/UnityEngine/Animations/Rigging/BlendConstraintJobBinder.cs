using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200000A RID: 10
	public class BlendConstraintJobBinder<T> : AnimationJobBinder<BlendConstraintJob, T> where T : struct, IAnimationJobData, IBlendConstraintData
	{
		// Token: 0x06000023 RID: 35 RVA: 0x000025B8 File Offset: 0x000007B8
		public override BlendConstraintJob Create(Animator animator, ref T data, Component component)
		{
			BlendConstraintJob blendConstraintJob = default(BlendConstraintJob);
			blendConstraintJob.driven = ReadWriteTransformHandle.Bind(animator, data.constrainedObject);
			blendConstraintJob.sourceA = ReadOnlyTransformHandle.Bind(animator, data.sourceObjectA);
			blendConstraintJob.sourceB = ReadOnlyTransformHandle.Bind(animator, data.sourceObjectB);
			blendConstraintJob.sourceAOffset = (blendConstraintJob.sourceBOffset = AffineTransform.identity);
			if (data.maintainPositionOffsets)
			{
				Vector3 position = data.constrainedObject.position;
				blendConstraintJob.sourceAOffset.translation = position - data.sourceObjectA.position;
				blendConstraintJob.sourceBOffset.translation = position - data.sourceObjectB.position;
			}
			if (data.maintainRotationOffsets)
			{
				Quaternion rotation = data.constrainedObject.rotation;
				blendConstraintJob.sourceAOffset.rotation = Quaternion.Inverse(data.sourceObjectA.rotation) * rotation;
				blendConstraintJob.sourceBOffset.rotation = Quaternion.Inverse(data.sourceObjectB.rotation) * rotation;
			}
			blendConstraintJob.blendPosition = BoolProperty.Bind(animator, component, data.blendPositionBoolProperty);
			blendConstraintJob.blendRotation = BoolProperty.Bind(animator, component, data.blendRotationBoolProperty);
			blendConstraintJob.positionWeight = FloatProperty.Bind(animator, component, data.positionWeightFloatProperty);
			blendConstraintJob.rotationWeight = FloatProperty.Bind(animator, component, data.rotationWeightFloatProperty);
			return blendConstraintJob;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000276D File Offset: 0x0000096D
		public override void Destroy(BlendConstraintJob job)
		{
		}
	}
}
