using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200005F RID: 95
	public class RigConstraint<TJob, TData, TBinder> : MonoBehaviour, IRigConstraint where TJob : struct, IWeightedAnimationJob where TData : struct, IAnimationJobData where TBinder : AnimationJobBinder<TJob, TData>, new()
	{
		// Token: 0x06000277 RID: 631 RVA: 0x00008380 File Offset: 0x00006580
		public void Reset()
		{
			this.m_Weight = 1f;
			this.m_Data.SetDefaultValues();
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000839E File Offset: 0x0000659E
		public bool IsValid()
		{
			return this.m_Data.IsValid();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x000083B1 File Offset: 0x000065B1
		protected virtual void OnValidate()
		{
			this.m_Weight = Mathf.Clamp01(this.m_Weight);
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600027A RID: 634 RVA: 0x000083C4 File Offset: 0x000065C4
		public ref TData data
		{
			get
			{
				return ref this.m_Data;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600027B RID: 635 RVA: 0x000083CC File Offset: 0x000065CC
		// (set) Token: 0x0600027C RID: 636 RVA: 0x000083D4 File Offset: 0x000065D4
		public float weight
		{
			get
			{
				return this.m_Weight;
			}
			set
			{
				this.m_Weight = Mathf.Clamp01(value);
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000083E4 File Offset: 0x000065E4
		public IAnimationJob CreateJob(Animator animator)
		{
			TJob tjob = RigConstraint<TJob, TData, TBinder>.s_Binder.Create(animator, ref this.m_Data, this);
			tjob.jobWeight = FloatProperty.BindCustom(animator, ConstraintsUtils.ConstructCustomPropertyName(this, ConstraintProperties.s_Weight));
			return tjob;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000842D File Offset: 0x0000662D
		public void DestroyJob(IAnimationJob job)
		{
			RigConstraint<TJob, TData, TBinder>.s_Binder.Destroy((TJob)((object)job));
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00008444 File Offset: 0x00006644
		public void UpdateJob(IAnimationJob job)
		{
			RigConstraint<TJob, TData, TBinder>.s_Binder.Update((TJob)((object)job), ref this.m_Data);
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000280 RID: 640 RVA: 0x00008461 File Offset: 0x00006661
		IAnimationJobBinder IRigConstraint.binder
		{
			get
			{
				return RigConstraint<TJob, TData, TBinder>.s_Binder;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000846D File Offset: 0x0000666D
		IAnimationJobData IRigConstraint.data
		{
			get
			{
				return this.m_Data;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000847A File Offset: 0x0000667A
		Component IRigConstraint.component
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04000108 RID: 264
		[SerializeField]
		[Range(0f, 1f)]
		protected float m_Weight = 1f;

		// Token: 0x04000109 RID: 265
		[SerializeField]
		[ExpandChildren]
		protected TData m_Data;

		// Token: 0x0400010A RID: 266
		private static readonly TBinder s_Binder = new TBinder();
	}
}
