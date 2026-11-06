using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000010 RID: 16
	public class DampedTransformJobBinder<T> : AnimationJobBinder<DampedTransformJob, T> where T : struct, IAnimationJobData, IDampedTransformData
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002EB8 File Offset: 0x000010B8
		public override DampedTransformJob Create(Animator animator, ref T data, Component component)
		{
			DampedTransformJob dampedTransformJob = default(DampedTransformJob);
			dampedTransformJob.driven = ReadWriteTransformHandle.Bind(animator, data.constrainedObject);
			dampedTransformJob.source = ReadOnlyTransformHandle.Bind(animator, data.sourceObject);
			AffineTransform affineTransform = new AffineTransform(data.constrainedObject.position, data.constrainedObject.rotation);
			AffineTransform affineTransform2 = new AffineTransform(data.sourceObject.position, data.sourceObject.rotation);
			dampedTransformJob.localBindTx = affineTransform2.InverseMul(affineTransform);
			dampedTransformJob.prevDrivenTx = affineTransform;
			dampedTransformJob.dampPosition = FloatProperty.Bind(animator, component, data.dampPositionFloatProperty);
			dampedTransformJob.dampRotation = FloatProperty.Bind(animator, component, data.dampRotationFloatProperty);
			if (data.maintainAim && AnimationRuntimeUtils.SqrDistance(data.constrainedObject.position, data.sourceObject.position) > 0f)
			{
				dampedTransformJob.aimBindAxis = Quaternion.Inverse(data.constrainedObject.rotation) * (affineTransform2.translation - affineTransform.translation).normalized;
			}
			else
			{
				dampedTransformJob.aimBindAxis = Vector3.zero;
			}
			return dampedTransformJob;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003022 File Offset: 0x00001222
		public override void Destroy(DampedTransformJob job)
		{
		}
	}
}
