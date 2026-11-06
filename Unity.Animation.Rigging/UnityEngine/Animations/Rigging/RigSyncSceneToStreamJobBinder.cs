using System;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000033 RID: 51
	internal class RigSyncSceneToStreamJobBinder<T> : AnimationJobBinder<RigSyncSceneToStreamJob, T> where T : struct, IAnimationJobData, IRigSyncSceneToStreamData
	{
		// Token: 0x060000DC RID: 220 RVA: 0x000053FC File Offset: 0x000035FC
		public override RigSyncSceneToStreamJob Create(Animator animator, ref T data, Component component)
		{
			RigSyncSceneToStreamJob rigSyncSceneToStreamJob = default(RigSyncSceneToStreamJob);
			Transform[] syncableTransforms = data.syncableTransforms;
			if (syncableTransforms != null)
			{
				rigSyncSceneToStreamJob.transformSyncer = RigSyncSceneToStreamJob.TransformSyncer.Create(syncableTransforms.Length);
				for (int i = 0; i < syncableTransforms.Length; i++)
				{
					rigSyncSceneToStreamJob.transformSyncer.BindAt(i, animator, syncableTransforms[i]);
				}
			}
			SyncableProperties[] syncableProperties = data.syncableProperties;
			if (syncableProperties != null)
			{
				int num = syncableProperties.Length;
				int num2 = 0;
				int num3 = 0;
				for (int j = 0; j < syncableProperties.Length; j++)
				{
					num2 += syncableProperties[j].constraints.Length;
					for (int k = 0; k < syncableProperties[j].constraints.Length; k++)
					{
						for (int l = 0; l < syncableProperties[j].constraints[k].properties.Length; l++)
						{
							num3 += syncableProperties[j].constraints[k].properties[l].descriptor.size;
						}
					}
				}
				rigSyncSceneToStreamJob.propertySyncer = RigSyncSceneToStreamJob.PropertySyncer.Create(num3);
				rigSyncSceneToStreamJob.rigWeightSyncer = RigSyncSceneToStreamJob.PropertySyncer.Create(num);
				rigSyncSceneToStreamJob.constraintWeightSyncer = RigSyncSceneToStreamJob.PropertySyncer.Create(num2);
				rigSyncSceneToStreamJob.rigStates = new NativeArray<float>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				rigSyncSceneToStreamJob.rigConstraintEndIdx = new NativeArray<int>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				rigSyncSceneToStreamJob.modulatedConstraintWeights = new NativeArray<PropertyStreamHandle>(num2, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				int num4 = 0;
				int num5 = 0;
				for (int m = 0; m < syncableProperties.Length; m++)
				{
					rigSyncSceneToStreamJob.rigWeightSyncer.BindAt(m, animator, syncableProperties[m].rig.component, RigProperties.s_Weight);
					rigSyncSceneToStreamJob.rigStates[m] = (data.rigStates[m] ? 1f : 0f);
					ConstraintProperties[] constraints = syncableProperties[m].constraints;
					for (int n = 0; n < constraints.Length; n++)
					{
						ref ConstraintProperties ptr = ref constraints[n];
						rigSyncSceneToStreamJob.constraintWeightSyncer.BindAt(num4, animator, ptr.component, ConstraintProperties.s_Weight);
						rigSyncSceneToStreamJob.modulatedConstraintWeights[num4++] = animator.BindCustomStreamProperty(ConstraintsUtils.ConstructCustomPropertyName(ptr.component, ConstraintProperties.s_Weight), CustomStreamPropertyType.Float);
						for (int num6 = 0; num6 < ptr.properties.Length; num6++)
						{
							ref Property ptr2 = ref ptr.properties[num6];
							if (ptr2.descriptor.size == 1)
							{
								rigSyncSceneToStreamJob.propertySyncer.BindAt(num5++, animator, ptr.component, ptr2.name);
							}
							else
							{
								for (int num7 = 0; num7 < ptr2.descriptor.size; num7++)
								{
									rigSyncSceneToStreamJob.propertySyncer.BindAt(num5++, animator, ptr.component, ptr2.name + RigSyncSceneToStreamJobBinder<T>.s_PropertyElementNames[num7]);
								}
							}
						}
					}
					rigSyncSceneToStreamJob.rigConstraintEndIdx[m] = num4;
				}
			}
			return rigSyncSceneToStreamJob;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005710 File Offset: 0x00003910
		public override void Destroy(RigSyncSceneToStreamJob job)
		{
			job.transformSyncer.Dispose();
			job.propertySyncer.Dispose();
			job.rigWeightSyncer.Dispose();
			job.constraintWeightSyncer.Dispose();
			if (job.rigStates.IsCreated)
			{
				job.rigStates.Dispose();
			}
			if (job.rigConstraintEndIdx.IsCreated)
			{
				job.rigConstraintEndIdx.Dispose();
			}
			if (job.modulatedConstraintWeights.IsCreated)
			{
				job.modulatedConstraintWeights.Dispose();
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000579C File Offset: 0x0000399C
		public override void Update(RigSyncSceneToStreamJob job, ref T data)
		{
			int num = Mathf.Min(job.rigStates.Length, data.rigStates.Length);
			for (int i = 0; i < num; i++)
			{
				job.rigStates[i] = (data.rigStates[i] ? 1f : 0f);
			}
		}

		// Token: 0x04000084 RID: 132
		internal static string[] s_PropertyElementNames = new string[] { ".x", ".y", ".z", ".w" };
	}
}
