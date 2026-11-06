using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200019E RID: 414
	[Serializable]
	public class MMCooldown
	{
		// Token: 0x060009B8 RID: 2488 RVA: 0x0002CC6D File Offset: 0x0002AE6D
		public virtual void Initialization()
		{
			this.CurrentDurationLeft = this.ConsumptionDuration;
			this.ChangeState(MMCooldown.CooldownStates.Idle);
			this._emptyReachedTimestamp = 0f;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0002CC8D File Offset: 0x0002AE8D
		public virtual void Start()
		{
			if (this.Ready())
			{
				this.ChangeState(MMCooldown.CooldownStates.Consuming);
			}
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0002CC9E File Offset: 0x0002AE9E
		public virtual bool Ready()
		{
			return this.Unlimited || this.CooldownState == MMCooldown.CooldownStates.Idle || (this.CooldownState == MMCooldown.CooldownStates.Refilling && this.CanInterruptRefill);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0002CCC8 File Offset: 0x0002AEC8
		public virtual void Stop()
		{
			if (this.CooldownState == MMCooldown.CooldownStates.Consuming)
			{
				this.ChangeState(MMCooldown.CooldownStates.Stopped);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0002CCDC File Offset: 0x0002AEDC
		public float Progress
		{
			get
			{
				if (this.Unlimited)
				{
					return 1f;
				}
				if (this.CooldownState == MMCooldown.CooldownStates.Consuming || this.CooldownState == MMCooldown.CooldownStates.Stopped)
				{
					return 0f;
				}
				if (this.CooldownState == MMCooldown.CooldownStates.Refilling)
				{
					return this.CurrentDurationLeft / this.RefillDuration;
				}
				return 1f;
			}
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0002CD2C File Offset: 0x0002AF2C
		public virtual void Update()
		{
			if (this.Unlimited)
			{
				return;
			}
			switch (this.CooldownState)
			{
			case MMCooldown.CooldownStates.Idle:
				break;
			case MMCooldown.CooldownStates.Consuming:
				this.CurrentDurationLeft -= Time.deltaTime;
				if (this.CurrentDurationLeft <= 0f)
				{
					this.CurrentDurationLeft = 0f;
					this._emptyReachedTimestamp = Time.time;
					this.ChangeState(MMCooldown.CooldownStates.Stopped);
					return;
				}
				break;
			case MMCooldown.CooldownStates.Stopped:
				if (Time.time - this._emptyReachedTimestamp >= this.PauseOnEmptyDuration)
				{
					this.ChangeState(MMCooldown.CooldownStates.Refilling);
					return;
				}
				break;
			case MMCooldown.CooldownStates.Refilling:
				this.CurrentDurationLeft += this.RefillDuration * Time.deltaTime / this.RefillDuration;
				if (this.CurrentDurationLeft >= this.RefillDuration)
				{
					this.CurrentDurationLeft = this.ConsumptionDuration;
					this.ChangeState(MMCooldown.CooldownStates.Idle);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0002CDFA File Offset: 0x0002AFFA
		protected virtual void ChangeState(MMCooldown.CooldownStates newState)
		{
			this.CooldownState = newState;
			MMCooldown.OnStateChangeDelegate onStateChange = this.OnStateChange;
			if (onStateChange == null)
			{
				return;
			}
			onStateChange(newState);
		}

		// Token: 0x04000B7D RID: 2941
		public bool Unlimited;

		// Token: 0x04000B7E RID: 2942
		public float ConsumptionDuration = 2f;

		// Token: 0x04000B7F RID: 2943
		public float PauseOnEmptyDuration = 1f;

		// Token: 0x04000B80 RID: 2944
		public float RefillDuration = 1f;

		// Token: 0x04000B81 RID: 2945
		public bool CanInterruptRefill = true;

		// Token: 0x04000B82 RID: 2946
		[MMReadOnly]
		public MMCooldown.CooldownStates CooldownState;

		// Token: 0x04000B83 RID: 2947
		[MMReadOnly]
		public float CurrentDurationLeft;

		// Token: 0x04000B84 RID: 2948
		public MMCooldown.OnStateChangeDelegate OnStateChange;

		// Token: 0x04000B85 RID: 2949
		protected float _emptyReachedTimestamp;

		// Token: 0x02000479 RID: 1145
		public enum CooldownStates
		{
			// Token: 0x040020E6 RID: 8422
			Idle,
			// Token: 0x040020E7 RID: 8423
			Consuming,
			// Token: 0x040020E8 RID: 8424
			Stopped,
			// Token: 0x040020E9 RID: 8425
			Refilling
		}

		// Token: 0x0200047A RID: 1146
		// (Invoke) Token: 0x06001AC1 RID: 6849
		public delegate void OnStateChangeDelegate(MMCooldown.CooldownStates newState);
	}
}
