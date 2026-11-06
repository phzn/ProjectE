using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000012 RID: 18
	public struct BoolProperty : IAnimatableProperty<bool>
	{
		// Token: 0x06000045 RID: 69 RVA: 0x0000302C File Offset: 0x0000122C
		public static BoolProperty Bind(Animator animator, Component component, string name)
		{
			return new BoolProperty
			{
				value = animator.BindStreamProperty(component.transform, component.GetType(), name)
			};
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000305C File Offset: 0x0000125C
		public static BoolProperty BindCustom(Animator animator, string property)
		{
			return new BoolProperty
			{
				value = animator.BindCustomStreamProperty(property, CustomStreamPropertyType.Bool)
			};
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003081 File Offset: 0x00001281
		public bool Get(AnimationStream stream)
		{
			return this.value.GetBool(stream);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000308F File Offset: 0x0000128F
		public void Set(AnimationStream stream, bool v)
		{
			this.value.SetBool(stream, v);
		}

		// Token: 0x04000026 RID: 38
		public PropertyStreamHandle value;
	}
}
