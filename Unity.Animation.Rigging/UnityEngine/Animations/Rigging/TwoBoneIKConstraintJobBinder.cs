using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200003E RID: 62
	public class TwoBoneIKConstraintJobBinder<T> : AnimationJobBinder<TwoBoneIKConstraintJob, T> where T : struct, IAnimationJobData, ITwoBoneIKConstraintData
	{
		// Token: 0x06000126 RID: 294 RVA: 0x00006254 File Offset: 0x00004454
		public override TwoBoneIKConstraintJob Create(Animator animator, ref T data, Component component)
		{
			TwoBoneIKConstraintJob twoBoneIKConstraintJob = default(TwoBoneIKConstraintJob);
			twoBoneIKConstraintJob.root = ReadWriteTransformHandle.Bind(animator, data.root);
			twoBoneIKConstraintJob.mid = ReadWriteTransformHandle.Bind(animator, data.mid);
			twoBoneIKConstraintJob.tip = ReadWriteTransformHandle.Bind(animator, data.tip);
			twoBoneIKConstraintJob.target = ReadOnlyTransformHandle.Bind(animator, data.target);
			if (data.hint != null)
			{
				twoBoneIKConstraintJob.hint = ReadOnlyTransformHandle.Bind(animator, data.hint);
			}
			twoBoneIKConstraintJob.targetOffset = AffineTransform.identity;
			if (data.maintainTargetPositionOffset)
			{
				twoBoneIKConstraintJob.targetOffset.translation = data.tip.position - data.target.position;
			}
			if (data.maintainTargetRotationOffset)
			{
				twoBoneIKConstraintJob.targetOffset.rotation = Quaternion.Inverse(data.target.rotation) * data.tip.rotation;
			}
			twoBoneIKConstraintJob.targetPositionWeight = FloatProperty.Bind(animator, component, data.targetPositionWeightFloatProperty);
			twoBoneIKConstraintJob.targetRotationWeight = FloatProperty.Bind(animator, component, data.targetRotationWeightFloatProperty);
			twoBoneIKConstraintJob.hintWeight = FloatProperty.Bind(animator, component, data.hintWeightFloatProperty);
			return twoBoneIKConstraintJob;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000063DC File Offset: 0x000045DC
		public override void Destroy(TwoBoneIKConstraintJob job)
		{
		}
	}
}
