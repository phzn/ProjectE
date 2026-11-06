using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000014 RID: 20
	public struct FloatProperty : IAnimatableProperty<float>
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00003114 File Offset: 0x00001314
		public static FloatProperty Bind(Animator animator, Component component, string name)
		{
			return new FloatProperty
			{
				value = animator.BindStreamProperty(component.transform, component.GetType(), name)
			};
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003144 File Offset: 0x00001344
		public static FloatProperty BindCustom(Animator animator, string property)
		{
			return new FloatProperty
			{
				value = animator.BindCustomStreamProperty(property, CustomStreamPropertyType.Float)
			};
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003169 File Offset: 0x00001369
		public float Get(AnimationStream stream)
		{
			return this.value.GetFloat(stream);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003177 File Offset: 0x00001377
		public void Set(AnimationStream stream, float v)
		{
			this.value.SetFloat(stream, v);
		}

		// Token: 0x04000028 RID: 40
		public PropertyStreamHandle value;
	}
}
