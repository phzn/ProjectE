using System;
using Features.DebugTools;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C41 RID: 3137
	public class StadiumHypeTrigger : MonoBehaviour
	{
		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06005A7C RID: 23164 RVA: 0x00236B84 File Offset: 0x00234D84
		internal static StadiumParachuteController ActiveFlight
		{
			get
			{
				StadiumParachuteController resourceOwner = PlayerActivityGate.GetResourceOwner<StadiumParachuteController>(PlayerContextResource.WorldInteraction);
				if (!(resourceOwner != null) || (!resourceOwner.IsFlying && !resourceOwner.IsLanding))
				{
					return null;
				}
				return resourceOwner;
			}
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06005A7D RID: 23165 RVA: 0x00236BB4 File Offset: 0x00234DB4
		internal static bool CanReadHypeInput
		{
			get
			{
				return !GameplayInputGate.BlocksGameplayInput && !TextInputFocus.BlocksGameplayInput && Time.timeScale > 0f && !FlyCamera.IsActive && (!PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.ModalUI) || (StadiumHypeTrigger.ActiveFlight != null && PlayerActivityGate.CanAcquireAlongside(PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.ModalUI, StadiumHypeTrigger.ActiveFlight, null)));
			}
		}

		// Token: 0x06005A7E RID: 23166 RVA: 0x00236C10 File Offset: 0x00234E10
		private void Update()
		{
			if (this.audioController == null)
			{
				return;
			}
			if (!StadiumHypeTrigger.CanReadHypeInput)
			{
				this.audioController.ReleaseHype();
				return;
			}
			if (this.triggerInput.GetButtonDown())
			{
				this.audioController.StartHype();
				if (this.debugLogs)
				{
					Debug.Log("[StadiumHypeTrigger] Hype start");
				}
			}
			else if (this.triggerInput.GetButton())
			{
				this.audioController.HoldHype();
			}
			if (this.triggerInput.GetButtonUp())
			{
				this.audioController.ReleaseHype();
				if (this.debugLogs)
				{
					Debug.Log("[StadiumHypeTrigger] Hype released");
				}
			}
		}

		// Token: 0x06005A7F RID: 23167 RVA: 0x00236CAD File Offset: 0x00234EAD
		private void OnDisable()
		{
			if (this.audioController != null)
			{
				this.audioController.ReleaseHype();
			}
		}

		// Token: 0x04005B6E RID: 23406
		[Header("References")]
		[SerializeField]
		private StadiumAudioController audioController;

		// Token: 0x04005B6F RID: 23407
		[Header("Input")]
		[SerializeField]
		private GenericInput triggerInput = new GenericInput("G", "X", "X");

		// Token: 0x04005B70 RID: 23408
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;
	}
}
