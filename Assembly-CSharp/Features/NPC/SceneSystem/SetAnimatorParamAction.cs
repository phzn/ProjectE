using System;
using System.Collections;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F27 RID: 3879
	[CreateAssetMenu(fileName = "SetAnimatorParamAction", menuName = "NPC/Scene System/Actions/Set Animator Param")]
	public class SetAnimatorParamAction : SceneActionSO
	{
		// Token: 0x06007488 RID: 29832 RVA: 0x002F4CD9 File Offset: 0x002F2ED9
		public override IEnumerator Execute(SceneContext context, string targetSlotId)
		{
			Animator animator = context.GetAnimator(targetSlotId);
			if (animator == null)
			{
				Debug.LogWarning("[SceneAction] SetAnimatorParam: No animator for slot '" + targetSlotId + "'");
				yield break;
			}
			switch (this.parameterType)
			{
			case SetAnimatorParamAction.ParamType.Bool:
				animator.SetBool(this.parameterName, this.boolValue);
				break;
			case SetAnimatorParamAction.ParamType.Int:
				animator.SetInteger(this.parameterName, this.intValue);
				break;
			case SetAnimatorParamAction.ParamType.Float:
				animator.SetFloat(this.parameterName, this.floatValue);
				break;
			case SetAnimatorParamAction.ParamType.Trigger:
				animator.SetTrigger(this.parameterName);
				break;
			}
			yield return null;
			yield break;
		}

		// Token: 0x04007751 RID: 30545
		[Header("Animator Parameter")]
		[Tooltip("Name of the animator parameter")]
		public string parameterName;

		// Token: 0x04007752 RID: 30546
		[Tooltip("Type of the parameter")]
		public SetAnimatorParamAction.ParamType parameterType = SetAnimatorParamAction.ParamType.Trigger;

		// Token: 0x04007753 RID: 30547
		[Tooltip("Value for Bool parameters")]
		public bool boolValue = true;

		// Token: 0x04007754 RID: 30548
		[Tooltip("Value for Int parameters")]
		public int intValue;

		// Token: 0x04007755 RID: 30549
		[Tooltip("Value for Float parameters")]
		public float floatValue;

		// Token: 0x02000F28 RID: 3880
		public enum ParamType
		{
			// Token: 0x04007757 RID: 30551
			Bool,
			// Token: 0x04007758 RID: 30552
			Int,
			// Token: 0x04007759 RID: 30553
			Float,
			// Token: 0x0400775A RID: 30554
			Trigger
		}
	}
}
