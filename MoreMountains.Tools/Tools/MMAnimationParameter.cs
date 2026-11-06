using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200017F RID: 383
	[AddComponentMenu("More Mountains/Tools/Animation/MMAnimationParameter")]
	public class MMAnimationParameter : MonoBehaviour
	{
		// Token: 0x060008F1 RID: 2289 RVA: 0x00029A84 File Offset: 0x00027C84
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00029A8C File Offset: 0x00027C8C
		protected virtual void Initialization()
		{
			this._parameter = Animator.StringToHash(this.ParameterName);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00029A9F File Offset: 0x00027C9F
		public virtual void SetTrigger()
		{
			this.TargetAnimator.SetTrigger(this._parameter);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00029AB2 File Offset: 0x00027CB2
		public virtual void SetInt(int value)
		{
			this.TargetAnimator.SetInteger(this._parameter, value);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00029AC6 File Offset: 0x00027CC6
		public virtual void SetFloat(float value)
		{
			this.TargetAnimator.SetFloat(this._parameter, value);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00029ADA File Offset: 0x00027CDA
		public virtual void SetBool(bool value)
		{
			this.TargetAnimator.SetBool(this._parameter, value);
		}

		// Token: 0x04000A8E RID: 2702
		public string ParameterName;

		// Token: 0x04000A8F RID: 2703
		public Animator TargetAnimator;

		// Token: 0x04000A90 RID: 2704
		protected int _parameter;
	}
}
