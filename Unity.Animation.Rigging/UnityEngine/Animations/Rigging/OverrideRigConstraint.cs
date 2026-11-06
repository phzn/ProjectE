using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200005A RID: 90
	public class OverrideRigConstraint<TConstraint, TJob, TData, TBinder> : IRigConstraint where TConstraint : MonoBehaviour, IRigConstraint where TJob : struct, IWeightedAnimationJob where TData : struct, IAnimationJobData where TBinder : AnimationJobBinder<TJob, TData>, new()
	{
		// Token: 0x06000242 RID: 578 RVA: 0x00007968 File Offset: 0x00005B68
		public OverrideRigConstraint(TConstraint baseConstraint)
		{
			this.m_Constraint = baseConstraint;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00007978 File Offset: 0x00005B78
		public IAnimationJob CreateJob(Animator animator)
		{
			TJob tjob = (TJob)((object)OverrideRigConstraint<TConstraint, TJob, TData, TBinder>.s_Binder.Create(animator, this.m_Constraint.data, this.m_Constraint));
			tjob.jobWeight = FloatProperty.BindCustom(animator, ConstraintsUtils.ConstructCustomPropertyName(this.m_Constraint, ConstraintProperties.s_Weight));
			return tjob;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000079E4 File Offset: 0x00005BE4
		public void DestroyJob(IAnimationJob job)
		{
			OverrideRigConstraint<TConstraint, TJob, TData, TBinder>.s_Binder.Destroy((TJob)((object)job));
		}

		// Token: 0x06000245 RID: 581 RVA: 0x000079FB File Offset: 0x00005BFB
		public void UpdateJob(IAnimationJob job)
		{
			OverrideRigConstraint<TConstraint, TJob, TData, TBinder>.s_Binder.Update(job, this.m_Constraint.data);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00007A1D File Offset: 0x00005C1D
		public bool IsValid()
		{
			return this.m_Constraint.IsValid();
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00007A2F File Offset: 0x00005C2F
		IAnimationJobBinder IRigConstraint.binder
		{
			get
			{
				return OverrideRigConstraint<TConstraint, TJob, TData, TBinder>.s_Binder;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00007A3B File Offset: 0x00005C3B
		IAnimationJobData IRigConstraint.data
		{
			get
			{
				return this.m_Constraint.data;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000249 RID: 585 RVA: 0x00007A4D File Offset: 0x00005C4D
		Component IRigConstraint.component
		{
			get
			{
				return this.m_Constraint.component;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00007A5F File Offset: 0x00005C5F
		// (set) Token: 0x0600024B RID: 587 RVA: 0x00007A71 File Offset: 0x00005C71
		public float weight
		{
			get
			{
				return this.m_Constraint.weight;
			}
			set
			{
				this.m_Constraint.weight = value;
			}
		}

		// Token: 0x040000F6 RID: 246
		[SerializeField]
		protected TConstraint m_Constraint;

		// Token: 0x040000F7 RID: 247
		private static readonly TBinder s_Binder = new TBinder();
	}
}
