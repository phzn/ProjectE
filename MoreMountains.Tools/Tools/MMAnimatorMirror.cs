using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000180 RID: 384
	public class MMAnimatorMirror : MonoBehaviour
	{
		// Token: 0x060008F8 RID: 2296 RVA: 0x00029AF6 File Offset: 0x00027CF6
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00029B00 File Offset: 0x00027D00
		public virtual void Initialization()
		{
			if (this.TargetAnimator == null)
			{
				this.TargetAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (this.TargetAnimator == null || this.SourceAnimator == null)
			{
				return;
			}
			int num = this.SourceAnimator.parameterCount;
			this._sourceParameters = new AnimatorControllerParameter[num];
			for (int i = 0; i < num; i++)
			{
				this._sourceParameters[i] = this.SourceAnimator.GetParameter(i);
			}
			num = this.TargetAnimator.parameterCount;
			this._targetParameters = new AnimatorControllerParameter[num];
			for (int j = 0; j < num; j++)
			{
				this._targetParameters[j] = this.TargetAnimator.GetParameter(j);
			}
			this._updateParameters = new List<MMAnimatorMirror.MMAnimatorMirrorBind>();
			foreach (AnimatorControllerParameter animatorControllerParameter in this._sourceParameters)
			{
				foreach (AnimatorControllerParameter animatorControllerParameter2 in this._targetParameters)
				{
					if (animatorControllerParameter.name == animatorControllerParameter2.name)
					{
						MMAnimatorMirror.MMAnimatorMirrorBind mmanimatorMirrorBind = default(MMAnimatorMirror.MMAnimatorMirrorBind);
						mmanimatorMirrorBind.ParameterHash = animatorControllerParameter.nameHash;
						mmanimatorMirrorBind.ParameterType = animatorControllerParameter.type;
						this._updateParameters.Add(mmanimatorMirrorBind);
					}
				}
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00029C4B File Offset: 0x00027E4B
		protected virtual void Update()
		{
			this.Mirror();
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00029C54 File Offset: 0x00027E54
		protected virtual void Mirror()
		{
			if (this.TargetAnimator == null || this.SourceAnimator == null)
			{
				return;
			}
			foreach (MMAnimatorMirror.MMAnimatorMirrorBind mmanimatorMirrorBind in this._updateParameters)
			{
				AnimatorControllerParameterType parameterType = mmanimatorMirrorBind.ParameterType;
				switch (parameterType)
				{
				case AnimatorControllerParameterType.Float:
					this.TargetAnimator.SetFloat(mmanimatorMirrorBind.ParameterHash, this.SourceAnimator.GetFloat(mmanimatorMirrorBind.ParameterHash));
					break;
				case (AnimatorControllerParameterType)2:
					break;
				case AnimatorControllerParameterType.Int:
					this.TargetAnimator.SetInteger(mmanimatorMirrorBind.ParameterHash, this.SourceAnimator.GetInteger(mmanimatorMirrorBind.ParameterHash));
					break;
				case AnimatorControllerParameterType.Bool:
					this.TargetAnimator.SetBool(mmanimatorMirrorBind.ParameterHash, this.SourceAnimator.GetBool(mmanimatorMirrorBind.ParameterHash));
					break;
				default:
					if (parameterType == AnimatorControllerParameterType.Trigger)
					{
						if (this.SourceAnimator.GetBool(mmanimatorMirrorBind.ParameterHash))
						{
							this.TargetAnimator.SetTrigger(mmanimatorMirrorBind.ParameterHash);
						}
						else
						{
							this.TargetAnimator.ResetTrigger(mmanimatorMirrorBind.ParameterHash);
						}
					}
					break;
				}
			}
		}

		// Token: 0x04000A91 RID: 2705
		[Header("Bindings")]
		public Animator SourceAnimator;

		// Token: 0x04000A92 RID: 2706
		public Animator TargetAnimator;

		// Token: 0x04000A93 RID: 2707
		protected AnimatorControllerParameter[] _sourceParameters;

		// Token: 0x04000A94 RID: 2708
		protected AnimatorControllerParameter[] _targetParameters;

		// Token: 0x04000A95 RID: 2709
		protected List<MMAnimatorMirror.MMAnimatorMirrorBind> _updateParameters;

		// Token: 0x02000472 RID: 1138
		public struct MMAnimatorMirrorBind
		{
			// Token: 0x040020CA RID: 8394
			public int ParameterHash;

			// Token: 0x040020CB RID: 8395
			public AnimatorControllerParameterType ParameterType;
		}
	}
}
