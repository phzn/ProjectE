using System;
using UnityEngine;

namespace Features.GachaMachine
{
	// Token: 0x020011E4 RID: 4580
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(60)]
	public sealed class GachaMachineAudio : MonoBehaviour
	{
		// Token: 0x06008D36 RID: 36150 RVA: 0x0039A684 File Offset: 0x00398884
		private void Awake()
		{
			if (this.controller == null)
			{
				this.controller = base.GetComponentInParent<GachaMachineController>();
			}
			if (this.motorSource != null)
			{
				this.motorSource.loop = true;
				this.motorSource.volume = 0f;
			}
			this._previousState = GachaMachineController.SessionState.Outside;
		}

		// Token: 0x06008D37 RID: 36151 RVA: 0x0039A6DC File Offset: 0x003988DC
		private void Update()
		{
			if (this.controller == null)
			{
				return;
			}
			GachaMachineController.SessionState state = this.controller.State;
			if (state != this._previousState)
			{
				if (state == GachaMachineController.SessionState.Entering)
				{
					this.PlayJingle(this.enterJingle);
				}
				else if (state == GachaMachineController.SessionState.Exiting)
				{
					this.PlayJingle(this.exitJingle);
				}
				this._previousState = state;
			}
			this.UpdateMotorHum(state);
		}

		// Token: 0x06008D38 RID: 36152 RVA: 0x0039A740 File Offset: 0x00398940
		private void UpdateMotorHum(GachaMachineController.SessionState state)
		{
			if (this.motorSource == null)
			{
				return;
			}
			GachaClawRig activeClaw = this.controller.ActiveClaw;
			if (activeClaw != this._trackedClaw)
			{
				this._trackedClaw = activeClaw;
				if (activeClaw != null && activeClaw.ClawMotor != null)
				{
					this._previousMotorPosition = activeClaw.ClawMotor.position;
					this._previousClosure = activeClaw.Closure01;
				}
			}
			float num = 0f;
			if (state == GachaMachineController.SessionState.Inside && activeClaw != null && activeClaw.ClawMotor != null)
			{
				Vector3 position = activeClaw.ClawMotor.position;
				float magnitude = (position - this._previousMotorPosition).magnitude;
				float num2 = Mathf.Abs(activeClaw.Closure01 - this._previousClosure);
				this._previousMotorPosition = position;
				this._previousClosure = activeClaw.Closure01;
				if (magnitude > this.movementThreshold || num2 > this.closureThreshold)
				{
					num = this.motorVolume;
				}
			}
			this.motorSource.volume = Mathf.MoveTowards(this.motorSource.volume, num, this.motorFadeSpeed * Time.deltaTime);
			if (this.motorSource.volume > 0.001f)
			{
				if (!this.motorSource.isPlaying && this.clawMotorLoop != null)
				{
					this.motorSource.clip = this.clawMotorLoop;
					this.motorSource.Play();
					return;
				}
			}
			else if (this.motorSource.isPlaying)
			{
				this.motorSource.Pause();
			}
		}

		// Token: 0x06008D39 RID: 36153 RVA: 0x0039A8C0 File Offset: 0x00398AC0
		private void PlayJingle(AudioClip clip)
		{
			if (this.jingleSource == null || clip == null)
			{
				return;
			}
			this.jingleSource.PlayOneShot(clip, this.jingleVolume);
		}

		// Token: 0x06008D3A RID: 36154 RVA: 0x0039A8EC File Offset: 0x00398AEC
		private void OnDisable()
		{
			if (this.motorSource != null)
			{
				this.motorSource.volume = 0f;
				if (this.motorSource.isPlaying)
				{
					this.motorSource.Stop();
				}
			}
		}

		// Token: 0x0400902E RID: 36910
		[Header("Wiring")]
		[SerializeField]
		private GachaMachineController controller;

		// Token: 0x0400902F RID: 36911
		[SerializeField]
		private AudioSource jingleSource;

		// Token: 0x04009030 RID: 36912
		[SerializeField]
		private AudioSource motorSource;

		// Token: 0x04009031 RID: 36913
		[Header("Clips")]
		[SerializeField]
		private AudioClip enterJingle;

		// Token: 0x04009032 RID: 36914
		[SerializeField]
		private AudioClip exitJingle;

		// Token: 0x04009033 RID: 36915
		[SerializeField]
		private AudioClip clawMotorLoop;

		// Token: 0x04009034 RID: 36916
		[Header("Tuning")]
		[SerializeField]
		[Range(0f, 1f)]
		private float jingleVolume = 0.3f;

		// Token: 0x04009035 RID: 36917
		[SerializeField]
		[Range(0f, 1f)]
		private float motorVolume = 0.16f;

		// Token: 0x04009036 RID: 36918
		[Tooltip("How fast the servo hum swells in and dies out around actual claw motion.")]
		[SerializeField]
		[Min(0.5f)]
		private float motorFadeSpeed = 7f;

		// Token: 0x04009037 RID: 36919
		[Tooltip("World metres the claw motor must travel in one frame to count as moving.")]
		[SerializeField]
		[Min(0.0001f)]
		private float movementThreshold = 0.0012f;

		// Token: 0x04009038 RID: 36920
		[SerializeField]
		[Min(0.0005f)]
		private float closureThreshold = 0.004f;

		// Token: 0x04009039 RID: 36921
		private GachaMachineController.SessionState _previousState;

		// Token: 0x0400903A RID: 36922
		private GachaClawRig _trackedClaw;

		// Token: 0x0400903B RID: 36923
		private Vector3 _previousMotorPosition;

		// Token: 0x0400903C RID: 36924
		private float _previousClosure;
	}
}
