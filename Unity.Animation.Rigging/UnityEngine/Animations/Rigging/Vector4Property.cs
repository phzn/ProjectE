using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000019 RID: 25
	public struct Vector4Property : IAnimatableProperty<Vector4>
	{
		// Token: 0x06000061 RID: 97 RVA: 0x0000363C File Offset: 0x0000183C
		public static Vector4Property Bind(Animator animator, Component component, string name)
		{
			Type type = component.GetType();
			return new Vector4Property
			{
				x = animator.BindStreamProperty(component.transform, type, name + ".x"),
				y = animator.BindStreamProperty(component.transform, type, name + ".y"),
				z = animator.BindStreamProperty(component.transform, type, name + ".z"),
				w = animator.BindStreamProperty(component.transform, type, name + ".w")
			};
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000036D8 File Offset: 0x000018D8
		public static Vector4Property BindCustom(Animator animator, string name)
		{
			return new Vector4Property
			{
				x = animator.BindCustomStreamProperty(name + ".x", CustomStreamPropertyType.Float),
				y = animator.BindCustomStreamProperty(name + ".y", CustomStreamPropertyType.Float),
				z = animator.BindCustomStreamProperty(name + ".z", CustomStreamPropertyType.Float),
				w = animator.BindCustomStreamProperty(name + ".w", CustomStreamPropertyType.Float)
			};
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003752 File Offset: 0x00001952
		public Vector4 Get(AnimationStream stream)
		{
			return new Vector4(this.x.GetFloat(stream), this.y.GetFloat(stream), this.z.GetFloat(stream), this.w.GetFloat(stream));
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000378C File Offset: 0x0000198C
		public void Set(AnimationStream stream, Vector4 value)
		{
			this.x.SetFloat(stream, value.x);
			this.y.SetFloat(stream, value.y);
			this.z.SetFloat(stream, value.z);
			this.w.SetFloat(stream, value.w);
		}

		// Token: 0x04000034 RID: 52
		public PropertyStreamHandle x;

		// Token: 0x04000035 RID: 53
		public PropertyStreamHandle y;

		// Token: 0x04000036 RID: 54
		public PropertyStreamHandle z;

		// Token: 0x04000037 RID: 55
		public PropertyStreamHandle w;
	}
}
