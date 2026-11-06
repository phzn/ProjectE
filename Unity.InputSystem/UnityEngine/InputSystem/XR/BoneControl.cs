using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x02000071 RID: 113
	public class BoneControl : InputControl<Bone>
	{
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x0003836C File Offset: 0x0003656C
		// (set) Token: 0x06000A46 RID: 2630 RVA: 0x00038374 File Offset: 0x00036574
		[InputControl(offset = 0U, displayName = "parentBoneIndex")]
		public IntegerControl parentBoneIndex { get; set; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0003837D File Offset: 0x0003657D
		// (set) Token: 0x06000A48 RID: 2632 RVA: 0x00038385 File Offset: 0x00036585
		[InputControl(offset = 4U, displayName = "Position")]
		public Vector3Control position { get; set; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0003838E File Offset: 0x0003658E
		// (set) Token: 0x06000A4A RID: 2634 RVA: 0x00038396 File Offset: 0x00036596
		[InputControl(offset = 16U, displayName = "Rotation")]
		public QuaternionControl rotation { get; set; }

		// Token: 0x06000A4B RID: 2635 RVA: 0x0003839F File Offset: 0x0003659F
		protected override void FinishSetup()
		{
			this.parentBoneIndex = base.GetChildControl<IntegerControl>("parentBoneIndex");
			this.position = base.GetChildControl<Vector3Control>("position");
			this.rotation = base.GetChildControl<QuaternionControl>("rotation");
			base.FinishSetup();
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x000383DC File Offset: 0x000365DC
		public unsafe override Bone ReadUnprocessedValueFromState(void* statePtr)
		{
			return new Bone
			{
				parentBoneIndex = (uint)this.parentBoneIndex.ReadUnprocessedValueFromStateWithCaching(statePtr),
				position = this.position.ReadUnprocessedValueFromStateWithCaching(statePtr),
				rotation = this.rotation.ReadUnprocessedValueFromStateWithCaching(statePtr)
			};
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0003842B File Offset: 0x0003662B
		public unsafe override void WriteValueIntoState(Bone value, void* statePtr)
		{
			this.parentBoneIndex.WriteValueIntoState((int)value.parentBoneIndex, statePtr);
			this.position.WriteValueIntoState(value.position, statePtr);
			this.rotation.WriteValueIntoState(value.rotation, statePtr);
		}
	}
}
