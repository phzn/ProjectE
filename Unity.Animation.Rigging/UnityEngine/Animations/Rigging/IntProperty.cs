using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000013 RID: 19
	public struct IntProperty : IAnimatableProperty<int>
	{
		// Token: 0x06000049 RID: 73 RVA: 0x000030A0 File Offset: 0x000012A0
		public static IntProperty Bind(Animator animator, Component component, string name)
		{
			return new IntProperty
			{
				value = animator.BindStreamProperty(component.transform, component.GetType(), name)
			};
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000030D0 File Offset: 0x000012D0
		public static IntProperty BindCustom(Animator animator, string property)
		{
			return new IntProperty
			{
				value = animator.BindCustomStreamProperty(property, CustomStreamPropertyType.Int)
			};
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000030F6 File Offset: 0x000012F6
		public int Get(AnimationStream stream)
		{
			return this.value.GetInt(stream);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003104 File Offset: 0x00001304
		public void Set(AnimationStream stream, int v)
		{
			this.value.SetInt(stream, v);
		}

		// Token: 0x04000027 RID: 39
		public PropertyStreamHandle value;
	}
}
