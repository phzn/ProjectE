using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002EC RID: 748
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Cinemachine/MMCinemachinePriorityListener")]
	[RequireComponent(typeof(CinemachineVirtualCameraBase))]
	public class MMCinemachinePriorityListener : MonoBehaviour
	{
		// Token: 0x06001505 RID: 5381 RVA: 0x00059C3E File Offset: 0x00057E3E
		public virtual float GetTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x00059C53 File Offset: 0x00057E53
		public virtual float GetDeltaTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x00059C68 File Offset: 0x00057E68
		protected virtual void Awake()
		{
			this._camera = base.gameObject.GetComponent<CinemachineVirtualCameraBase>();
			this._initialPriority = this._camera.Priority;
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x00059C8C File Offset: 0x00057E8C
		public virtual void OnMMCinemachinePriorityEvent(MMChannelData channelData, bool forceMaxPriority, int newPriority, bool forceTransition, CinemachineBlendDefinition blendDefinition, bool resetValuesAfterTransition, TimescaleModes timescaleMode, bool restore = false)
		{
			this.TimescaleMode = timescaleMode;
			if (!MMChannel.Match(channelData, this.ChannelMode, this.Channel, this.MMChannelDefinition))
			{
				if (forceMaxPriority)
				{
					if (restore)
					{
						this.SetPriority(this._initialPriority);
						return;
					}
					this.SetPriority(0);
				}
				return;
			}
			if (restore)
			{
				this.SetPriority(this._initialPriority);
				return;
			}
			this.SetPriority(newPriority);
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x00059CEF File Offset: 0x00057EEF
		protected virtual void SetPriority(int newPriority)
		{
			this._camera.Priority = newPriority;
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00059CFD File Offset: 0x00057EFD
		protected virtual void OnEnable()
		{
			MMCinemachinePriorityEvent.Register(new MMCinemachinePriorityEvent.Delegate(this.OnMMCinemachinePriorityEvent));
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x00059D11 File Offset: 0x00057F11
		protected virtual void OnDisable()
		{
			MMCinemachinePriorityEvent.Unregister(new MMCinemachinePriorityEvent.Delegate(this.OnMMCinemachinePriorityEvent));
		}

		// Token: 0x0400180D RID: 6157
		[HideInInspector]
		public TimescaleModes TimescaleMode;

		// Token: 0x0400180E RID: 6158
		[Header("Priority Listener")]
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x0400180F RID: 6159
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMFEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x04001810 RID: 6160
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMFEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x04001811 RID: 6161
		protected CinemachineVirtualCameraBase _camera;

		// Token: 0x04001812 RID: 6162
		protected int _initialPriority;
	}
}
