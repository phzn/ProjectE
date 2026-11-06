using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x0200038F RID: 911
	[AddComponentMenu("")]
	public class Falcon : MonoBehaviour
	{
		// Token: 0x060018D7 RID: 6359 RVA: 0x000698B1 File Offset: 0x00067AB1
		protected virtual void Start()
		{
			this.SetCar(false);
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x000698BC File Offset: 0x00067ABC
		protected virtual void SetCar(bool status)
		{
			foreach (MMWiggle mmwiggle in this.Wigglers)
			{
				mmwiggle.PositionActive = status;
			}
			foreach (ParticleSystem particleSystem in this.ParticleEmitters)
			{
				if (status)
				{
					particleSystem.Play();
				}
				else
				{
					particleSystem.Stop();
				}
			}
			foreach (ParticleSystem particleSystem2 in this.ParticleLoops)
			{
				if (status)
				{
					particleSystem2.Play();
				}
				else
				{
					particleSystem2.Pause();
				}
			}
			foreach (MMAutoRotate mmautoRotate in this.AutoRotaters)
			{
				mmautoRotate.Rotating = status;
			}
			this.Offsetter.TextureShouldPan = status;
			this.CameraWiggler.PositionActive = status;
			this.CameraWiggler.RotationActive = status;
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x00069A0C File Offset: 0x00067C0C
		protected virtual void Update()
		{
			this.HandleInput();
			this.HandleCar();
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x00069A1A File Offset: 0x00067C1A
		protected virtual void HandleInput()
		{
			if (FeelDemosInputHelper.CheckMainActionInputPressed())
			{
				this.Drive();
			}
			if (FeelDemosInputHelper.CheckMainActionInputUpThisFrame())
			{
				this.TurnStop();
			}
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x00069A36 File Offset: 0x00067C36
		protected virtual void HandleCar()
		{
			bool turning = this._turning;
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x00069A3F File Offset: 0x00067C3F
		protected virtual void Drive()
		{
			if (!this._turning)
			{
				MMFeedbacks driveFeedback = this.DriveFeedback;
				if (driveFeedback != null)
				{
					driveFeedback.PlayFeedbacks();
				}
				this.SetCar(true);
			}
			this._turning = true;
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x00069A68 File Offset: 0x00067C68
		protected virtual void TurnStop()
		{
			MMFeedbacks driveFeedback = this.DriveFeedback;
			if (driveFeedback != null)
			{
				driveFeedback.StopFeedbacks();
			}
			MMFeedbacks stopFeedback = this.StopFeedback;
			if (stopFeedback != null)
			{
				stopFeedback.PlayFeedbacks();
			}
			this.SetCar(false);
			this._turning = false;
		}

		// Token: 0x04001CFB RID: 7419
		[Header("Input")]
		[Tooltip("a key to use to jump")]
		public KeyCode ActionKey = KeyCode.Space;

		// Token: 0x04001CFC RID: 7420
		[Tooltip("a secondary key to use to jump")]
		public KeyCode ActionKeyAlt = KeyCode.Joystick1Button0;

		// Token: 0x04001CFD RID: 7421
		[Header("Bindings")]
		[Tooltip("the various wigglers that make the car move")]
		public List<MMWiggle> Wigglers;

		// Token: 0x04001CFE RID: 7422
		[Tooltip("the wiggler associated to the camera")]
		public MMWiggle CameraWiggler;

		// Token: 0x04001CFF RID: 7423
		[Tooltip("the ground's panning texture")]
		public MMPanningTexture Offsetter;

		// Token: 0x04001D00 RID: 7424
		[Tooltip("the particles that are supposed to loop (rocks etc)")]
		public List<ParticleSystem> ParticleLoops;

		// Token: 0x04001D01 RID: 7425
		[Tooltip("the on/off emitters (wind, smoke)")]
		public List<ParticleSystem> ParticleEmitters;

		// Token: 0x04001D02 RID: 7426
		[Tooltip("the wheels' auto rotators")]
		public List<MMAutoRotate> AutoRotaters;

		// Token: 0x04001D03 RID: 7427
		[Header("Settings")]
		[Tooltip("the speed at which the wheel should rotate")]
		public float RotationSpeed = 20f;

		// Token: 0x04001D04 RID: 7428
		[Header("Feedbacks")]
		[Tooltip("a feedback to call when the car starts driving")]
		public MMFeedbacks DriveFeedback;

		// Token: 0x04001D05 RID: 7429
		[Tooltip("a feedback to call when the car stops")]
		public MMFeedbacks StopFeedback;

		// Token: 0x04001D06 RID: 7430
		protected bool _turning;
	}
}
