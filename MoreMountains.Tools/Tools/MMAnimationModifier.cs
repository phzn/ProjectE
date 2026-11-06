using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200017E RID: 382
	[AddComponentMenu("More Mountains/Tools/Animation/MMAnimationModifier")]
	public class MMAnimationModifier : StateMachineBehaviour
	{
		// Token: 0x060008EE RID: 2286 RVA: 0x000299B8 File Offset: 0x00027BB8
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			this._initialSpeed = animator.speed;
			animator.speed = Random.Range(this.AnimationSpeed.x, this.AnimationSpeed.y);
			if (!this._enteredState)
			{
				animator.Play(stateInfo.fullPathHash, layerIndex, Random.Range(this.StartPosition.x, this.StartPosition.y));
			}
			this._enteredState = !this._enteredState;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00029A3B File Offset: 0x00027C3B
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateExit(animator, stateInfo, layerIndex);
			animator.speed = this._initialSpeed;
		}

		// Token: 0x04000A8A RID: 2698
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 StartPosition = new Vector2(0f, 0f);

		// Token: 0x04000A8B RID: 2699
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 AnimationSpeed = new Vector2(1f, 1f);

		// Token: 0x04000A8C RID: 2700
		protected bool _enteredState;

		// Token: 0x04000A8D RID: 2701
		protected float _initialSpeed;
	}
}
