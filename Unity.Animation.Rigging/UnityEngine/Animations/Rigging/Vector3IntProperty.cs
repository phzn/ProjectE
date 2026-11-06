using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000017 RID: 23
	public struct Vector3IntProperty : IAnimatableProperty<Vector3Int>
	{
		// Token: 0x06000059 RID: 89 RVA: 0x000033B4 File Offset: 0x000015B4
		public static Vector3IntProperty Bind(Animator animator, Component component, string name)
		{
			Type type = component.GetType();
			return new Vector3IntProperty
			{
				x = animator.BindStreamProperty(component.transform, type, name + ".x"),
				y = animator.BindStreamProperty(component.transform, type, name + ".y"),
				z = animator.BindStreamProperty(component.transform, type, name + ".z")
			};
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003430 File Offset: 0x00001630
		public static Vector3IntProperty BindCustom(Animator animator, string name)
		{
			return new Vector3IntProperty
			{
				x = animator.BindCustomStreamProperty(name + ".x", CustomStreamPropertyType.Int),
				y = animator.BindCustomStreamProperty(name + ".y", CustomStreamPropertyType.Int),
				z = animator.BindCustomStreamProperty(name + ".z", CustomStreamPropertyType.Int)
			};
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003494 File Offset: 0x00001694
		public Vector3Int Get(AnimationStream stream)
		{
			return new Vector3Int(this.x.GetInt(stream), this.y.GetInt(stream), this.z.GetInt(stream));
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000034BF File Offset: 0x000016BF
		public void Set(AnimationStream stream, Vector3Int value)
		{
			this.x.SetInt(stream, value.x);
			this.y.SetInt(stream, value.y);
			this.z.SetInt(stream, value.z);
		}

		// Token: 0x0400002E RID: 46
		public PropertyStreamHandle x;

		// Token: 0x0400002F RID: 47
		public PropertyStreamHandle y;

		// Token: 0x04000030 RID: 48
		public PropertyStreamHandle z;
	}
}
