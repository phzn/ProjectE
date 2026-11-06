using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200002F RID: 47
	public class OverrideTransformJobBinder<T> : AnimationJobBinder<OverrideTransformJob, T> where T : struct, IAnimationJobData, IOverrideTransformData
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x000050B0 File Offset: 0x000032B0
		public override OverrideTransformJob Create(Animator animator, ref T data, Component component)
		{
			OverrideTransformJob overrideTransformJob = default(OverrideTransformJob);
			AnimationJobCacheBuilder animationJobCacheBuilder = new AnimationJobCacheBuilder();
			overrideTransformJob.driven = ReadWriteTransformHandle.Bind(animator, data.constrainedObject);
			if (data.sourceObject != null)
			{
				overrideTransformJob.source = ReadOnlyTransformHandle.Bind(animator, data.sourceObject);
				AffineTransform affineTransform = new AffineTransform(data.sourceObject.localPosition, data.sourceObject.localRotation);
				overrideTransformJob.sourceInvLocalBindTx = affineTransform.Inverse();
				AffineTransform affineTransform2 = new AffineTransform(data.sourceObject.position, data.sourceObject.rotation);
				AffineTransform affineTransform3 = new AffineTransform(data.constrainedObject.position, data.constrainedObject.rotation);
				overrideTransformJob.sourceToWorldRot = affineTransform2.Inverse().rotation;
				overrideTransformJob.sourceToPivotRot = affineTransform2.InverseMul(affineTransform3).rotation;
				Transform parent = data.constrainedObject.parent;
				if (parent != null)
				{
					AffineTransform affineTransform4 = new AffineTransform(parent.position, parent.rotation);
					overrideTransformJob.sourceToLocalRot = affineTransform2.InverseMul(affineTransform4).rotation;
				}
				else
				{
					overrideTransformJob.sourceToLocalRot = overrideTransformJob.sourceToPivotRot;
				}
			}
			overrideTransformJob.spaceIdx = animationJobCacheBuilder.Add((float)data.space);
			if (data.space == 2)
			{
				overrideTransformJob.sourceToCurrSpaceRotIdx = animationJobCacheBuilder.Add(overrideTransformJob.sourceToPivotRot);
			}
			else if (data.space == 1)
			{
				overrideTransformJob.sourceToCurrSpaceRotIdx = animationJobCacheBuilder.Add(overrideTransformJob.sourceToLocalRot);
			}
			else
			{
				overrideTransformJob.sourceToCurrSpaceRotIdx = animationJobCacheBuilder.Add(overrideTransformJob.sourceToWorldRot);
			}
			overrideTransformJob.position = Vector3Property.Bind(animator, component, data.positionVector3Property);
			overrideTransformJob.rotation = Vector3Property.Bind(animator, component, data.rotationVector3Property);
			overrideTransformJob.positionWeight = FloatProperty.Bind(animator, component, data.positionWeightFloatProperty);
			overrideTransformJob.rotationWeight = FloatProperty.Bind(animator, component, data.rotationWeightFloatProperty);
			overrideTransformJob.cache = animationJobCacheBuilder.Build();
			return overrideTransformJob;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005306 File Offset: 0x00003506
		public override void Destroy(OverrideTransformJob job)
		{
			job.cache.Dispose();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005314 File Offset: 0x00003514
		public override void Update(OverrideTransformJob job, ref T data)
		{
			job.UpdateSpace(data.space);
		}
	}
}
