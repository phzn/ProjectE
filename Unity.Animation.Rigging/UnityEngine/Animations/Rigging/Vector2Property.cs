using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000015 RID: 21
	public struct Vector2Property : IAnimatableProperty<Vector2>
	{
		// Token: 0x06000051 RID: 81 RVA: 0x00003188 File Offset: 0x00001388
		public static Vector2Property Bind(Animator animator, Component component, string name)
		{
			Type type = component.GetType();
			return new Vector2Property
			{
				x = animator.BindStreamProperty(component.transform, type, name + ".x"),
				y = animator.BindStreamProperty(component.transform, type, name + ".y")
			};
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000031E4 File Offset: 0x000013E4
		public static Vector2Property BindCustom(Animator animator, string name)
		{
			return new Vector2Property
			{
				x = animator.BindCustomStreamProperty(name + ".x", CustomStreamPropertyType.Float),
				y = animator.BindCustomStreamProperty(name + ".y", CustomStreamPropertyType.Float)
			};
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000322C File Offset: 0x0000142C
		public Vector2 Get(AnimationStream stream)
		{
			return new Vector2(this.x.GetFloat(stream), this.y.GetFloat(stream));
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000324B File Offset: 0x0000144B
		public void Set(AnimationStream stream, Vector2 value)
		{
			this.x.SetFloat(stream, value.x);
			this.y.SetFloat(stream, value.y);
		}

		// Token: 0x04000029 RID: 41
		public PropertyStreamHandle x;

		// Token: 0x0400002A RID: 42
		public PropertyStreamHandle y;
	}
}
