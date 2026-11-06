using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000016 RID: 22
	public struct Vector3Property : IAnimatableProperty<Vector3>
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00003274 File Offset: 0x00001474
		public static Vector3Property Bind(Animator animator, Component component, string name)
		{
			Type type = component.GetType();
			return new Vector3Property
			{
				x = animator.BindStreamProperty(component.transform, type, name + ".x"),
				y = animator.BindStreamProperty(component.transform, type, name + ".y"),
				z = animator.BindStreamProperty(component.transform, type, name + ".z")
			};
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000032F0 File Offset: 0x000014F0
		public static Vector3Property BindCustom(Animator animator, string name)
		{
			return new Vector3Property
			{
				x = animator.BindCustomStreamProperty(name + ".x", CustomStreamPropertyType.Float),
				y = animator.BindCustomStreamProperty(name + ".y", CustomStreamPropertyType.Float),
				z = animator.BindCustomStreamProperty(name + ".z", CustomStreamPropertyType.Float)
			};
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003351 File Offset: 0x00001551
		public Vector3 Get(AnimationStream stream)
		{
			return new Vector3(this.x.GetFloat(stream), this.y.GetFloat(stream), this.z.GetFloat(stream));
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000337C File Offset: 0x0000157C
		public void Set(AnimationStream stream, Vector3 value)
		{
			this.x.SetFloat(stream, value.x);
			this.y.SetFloat(stream, value.y);
			this.z.SetFloat(stream, value.z);
		}

		// Token: 0x0400002B RID: 43
		public PropertyStreamHandle x;

		// Token: 0x0400002C RID: 44
		public PropertyStreamHandle y;

		// Token: 0x0400002D RID: 45
		public PropertyStreamHandle z;
	}
}
