using System;
using System.Collections;
using Cinemachine;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002EB RID: 747
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Cinemachine/MMCinemachinePriorityBrainListener")]
	[RequireComponent(typeof(CinemachineBrain))]
	public class MMCinemachinePriorityBrainListener : MonoBehaviour
	{
		// Token: 0x060014FD RID: 5373 RVA: 0x00059B30 File Offset: 0x00057D30
		public virtual float GetTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00059B45 File Offset: 0x00057D45
		public virtual float GetDeltaTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00059B5A File Offset: 0x00057D5A
		protected virtual void Awake()
		{
			this._brain = base.gameObject.GetComponent<CinemachineBrain>();
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x00059B70 File Offset: 0x00057D70
		public virtual void OnMMCinemachinePriorityEvent(MMChannelData channelData, bool forceMaxPriority, int newPriority, bool forceTransition, CinemachineBlendDefinition blendDefinition, bool resetValuesAfterTransition, TimescaleModes timescaleMode, bool restore = false)
		{
			if (forceTransition)
			{
				if (this._coroutine != null)
				{
					base.StopCoroutine(this._coroutine);
				}
				else
				{
					this._initialDefinition = this._brain.m_DefaultBlend;
				}
				this._brain.m_DefaultBlend = blendDefinition;
				this.TimescaleMode = timescaleMode;
				this._coroutine = base.StartCoroutine(this.ResetBlendDefinition(blendDefinition.m_Time));
			}
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x00059BD6 File Offset: 0x00057DD6
		protected virtual IEnumerator ResetBlendDefinition(float delay)
		{
			for (float timer = 0f; timer < delay; timer += this.GetDeltaTime())
			{
				yield return null;
			}
			this._brain.m_DefaultBlend = this._initialDefinition;
			this._coroutine = null;
			yield break;
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00059BEC File Offset: 0x00057DEC
		protected virtual void OnEnable()
		{
			this._coroutine = null;
			MMCinemachinePriorityEvent.Register(new MMCinemachinePriorityEvent.Delegate(this.OnMMCinemachinePriorityEvent));
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x00059C07 File Offset: 0x00057E07
		protected virtual void OnDisable()
		{
			if (this._coroutine != null)
			{
				base.StopCoroutine(this._coroutine);
			}
			this._coroutine = null;
			MMCinemachinePriorityEvent.Unregister(new MMCinemachinePriorityEvent.Delegate(this.OnMMCinemachinePriorityEvent));
		}

		// Token: 0x04001809 RID: 6153
		[HideInInspector]
		public TimescaleModes TimescaleMode;

		// Token: 0x0400180A RID: 6154
		protected CinemachineBrain _brain;

		// Token: 0x0400180B RID: 6155
		protected CinemachineBlendDefinition _initialDefinition;

		// Token: 0x0400180C RID: 6156
		protected Coroutine _coroutine;
	}
}
