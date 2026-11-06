using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000018 RID: 24
	public struct Vector3BoolProperty : IAnimatableProperty<Vector3Bool>
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000034FC File Offset: 0x000016FC
		public static Vector3BoolProperty Bind(Animator animator, Component component, string name)
		{
			Type type = component.GetType();
			return new Vector3BoolProperty
			{
				x = animator.BindStreamProperty(component.transform, type, name + ".x"),
				y = animator.BindStreamProperty(component.transform, type, name + ".y"),
				z = animator.BindStreamProperty(component.transform, type, name + ".z")
			};
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003578 File Offset: 0x00001778
		public static Vector3BoolProperty BindCustom(Animator animator, string name)
		{
			return new Vector3BoolProperty
			{
				x = animator.BindCustomStreamProperty(name + ".x", CustomStreamPropertyType.Bool),
				y = animator.BindCustomStreamProperty(name + ".y", CustomStreamPropertyType.Bool),
				z = animator.BindCustomStreamProperty(name + ".z", CustomStreamPropertyType.Bool)
			};
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000035D9 File Offset: 0x000017D9
		public Vector3Bool Get(AnimationStream stream)
		{
			return new Vector3Bool(this.x.GetBool(stream), this.y.GetBool(stream), this.z.GetBool(stream));
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003604 File Offset: 0x00001804
		public void Set(AnimationStream stream, Vector3Bool value)
		{
			this.x.SetBool(stream, value.x);
			this.y.SetBool(stream, value.y);
			this.z.SetBool(stream, value.z);
		}

		// Token: 0x04000031 RID: 49
		public PropertyStreamHandle x;

		// Token: 0x04000032 RID: 50
		public PropertyStreamHandle y;

		// Token: 0x04000033 RID: 51
		public PropertyStreamHandle z;
	}
}
