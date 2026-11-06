using System;
using UnityEngine;

namespace Invector.Utils
{
	// Token: 0x020005FD RID: 1533
	[vClassHeader("Set Animator Value", true, "icon_v2", false, "", useHelpBox = true, helpBoxText = "Use this component to set animator value using events")]
	public class vSetAnimatorValue : vMonoBehaviour
	{
		// Token: 0x0600236E RID: 9070 RVA: 0x000C98AA File Offset: 0x000C7AAA
		public void SetTargetAnimator(Animator animator)
		{
			this.targetAnimator = animator;
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x000C98B3 File Offset: 0x000C7AB3
		public void SetTrigger()
		{
			this.targetAnimator.SetTrigger(this.paramenterHash);
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x000C98C6 File Offset: 0x000C7AC6
		public void ResetTrigger()
		{
			this.targetAnimator.ResetTrigger(this.paramenterHash);
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x000C98D9 File Offset: 0x000C7AD9
		public void SetBoolean(bool value)
		{
			this.targetAnimator.SetBool(this.paramenterHash, value);
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x000C98ED File Offset: 0x000C7AED
		public void SetInteger(int value)
		{
			this.targetAnimator.SetInteger(this.paramenterHash, value);
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x000C9901 File Offset: 0x000C7B01
		public void SetFloat(float value)
		{
			this.targetAnimator.SetFloat(this.paramenterHash, value);
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x000C9915 File Offset: 0x000C7B15
		public void SetTargetParamenter(string targetParamenter)
		{
			this.targetParamenter = targetParamenter;
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x000C9920 File Offset: 0x000C7B20
		public void SetValueByExpression(string expression)
		{
			string[] array = expression.Split(',', StringSplitOptions.None);
			if (array.Length < 3)
			{
				Debug.LogWarning("Expression :<color=green>" + expression + "</color> does't match any valid expression");
				return;
			}
			string text = array[0];
			vSetAnimatorValue.ValueType valueType;
			if (Enum.TryParse<vSetAnimatorValue.ValueType>(array[1], out valueType))
			{
				string text2 = array[2];
				bool flag = false;
				switch (valueType)
				{
				case vSetAnimatorValue.ValueType.Float:
				{
					double num;
					if (double.TryParse(text2, out num))
					{
						this.targetAnimator.SetFloat(text, (float)num);
					}
					else
					{
						flag = true;
					}
					break;
				}
				case vSetAnimatorValue.ValueType.Int:
				{
					int num2;
					if (int.TryParse(text2, out num2))
					{
						this.targetAnimator.SetInteger(this.targetParamenter, num2);
					}
					else
					{
						flag = true;
					}
					break;
				}
				case vSetAnimatorValue.ValueType.Bool:
				{
					bool flag2;
					if (bool.TryParse(text2, out flag2))
					{
						this.targetAnimator.SetBool(text, flag2);
					}
					else
					{
						flag = true;
					}
					break;
				}
				case vSetAnimatorValue.ValueType.Trigger:
					if (text2 == "Set")
					{
						this.targetAnimator.SetTrigger(text);
					}
					else if (text2 == "Reset")
					{
						this.targetAnimator.ResetTrigger(text);
					}
					else
					{
						flag = true;
					}
					break;
				}
				if (flag)
				{
					Debug.LogWarning("Expression :<color=green>" + text2 + "</color> does't match any valid value");
					return;
				}
			}
			else
			{
				Debug.LogWarning("Expression :<color=green>" + array[0] + "</color> does't match any animator value");
			}
		}

		// Token: 0x04002382 RID: 9090
		[vSelectableString("Set Float exemple", "ParamenterName,Float,1.0")]
		[vSelectableString("Set Integer exemple", "ParamenterName,Int,1")]
		[vSelectableString("Set Bool exemple", "ParamenterName,Bool,true")]
		[vSelectableString("Set Trigger exemple", "ParamenterName,Trigger,Set")]
		[vSelectableString("Reset Trigger exemple", "ParamenterName,Trigger,Reset")]
		[vHelpBox("SetValueByExpression examples", vHelpBoxAttribute.MessageType.Info)]
		[Tooltip("Target Animator to set value")]
		public Animator targetAnimator;

		// Token: 0x04002383 RID: 9091
		[Tooltip("Target paramenter for normal methods\nDon't will be used when using SetValueByExpression method")]
		public string targetParamenter;

		// Token: 0x04002384 RID: 9092
		[SerializeField]
		[vReadOnly(true)]
		protected int paramenterHash;

		// Token: 0x020005FE RID: 1534
		private enum ValueType
		{
			// Token: 0x04002386 RID: 9094
			Float,
			// Token: 0x04002387 RID: 9095
			Int,
			// Token: 0x04002388 RID: 9096
			Bool,
			// Token: 0x04002389 RID: 9097
			Trigger
		}
	}
}
