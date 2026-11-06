using System;
using Features.DebugTools;
using Michsky.MUIP;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Interaction
{
	// Token: 0x02001172 RID: 4466
	[RequireComponent(typeof(Collider))]
	public class InteractionTrigger : MonoBehaviour
	{
		// Token: 0x06008833 RID: 34867 RVA: 0x0037BCE8 File Offset: 0x00379EE8
		private void Start()
		{
			if (this.uiObject != null)
			{
				this.uiObject.SetActive(false);
			}
			if (this.progressBar != null)
			{
				this.progressBar.currentPercent = 0f;
				this.progressBar.maxValue = this.holdDuration;
				this.progressBar.UpdateUI();
			}
		}

		// Token: 0x06008834 RID: 34868 RVA: 0x0037BD4C File Offset: 0x00379F4C
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (!this.canInteract)
			{
				return;
			}
			if (GameplayInput.GetKey(this.interactionKey))
			{
				if (!this.isInteracting)
				{
					this.isInteracting = true;
					UnityEvent unityEvent = this.onInteractionStart;
					if (unityEvent != null)
					{
						unityEvent.Invoke();
					}
				}
				this.ProcessInteraction();
				return;
			}
			if (this.isInteracting)
			{
				this.CancelInteraction();
			}
		}

		// Token: 0x06008835 RID: 34869 RVA: 0x0037BDAC File Offset: 0x00379FAC
		private void ProcessInteraction()
		{
			this.currentHoldTime += Time.deltaTime;
			if (this.progressBar != null)
			{
				this.progressBar.currentPercent = this.currentHoldTime;
				this.progressBar.UpdateUI();
			}
			if (this.currentHoldTime >= this.holdDuration)
			{
				this.CompleteInteraction();
			}
		}

		// Token: 0x06008836 RID: 34870 RVA: 0x0037BE0C File Offset: 0x0037A00C
		private void CompleteInteraction()
		{
			this.isInteracting = false;
			this.currentHoldTime = 0f;
			if (this.progressBar != null)
			{
				this.progressBar.currentPercent = 0f;
				this.progressBar.UpdateUI();
			}
			UnityEvent unityEvent = this.onInteractionComplete;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06008837 RID: 34871 RVA: 0x0037BE64 File Offset: 0x0037A064
		private void CancelInteraction()
		{
			this.isInteracting = false;
			this.currentHoldTime = 0f;
			if (this.progressBar != null)
			{
				this.progressBar.currentPercent = 0f;
				this.progressBar.UpdateUI();
			}
			UnityEvent unityEvent = this.onInteractionCancel;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06008838 RID: 34872 RVA: 0x0037BEBC File Offset: 0x0037A0BC
		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag(this.playerTag))
			{
				this.canInteract = true;
				if (this.uiObject != null)
				{
					this.uiObject.SetActive(true);
				}
			}
		}

		// Token: 0x06008839 RID: 34873 RVA: 0x0037BEED File Offset: 0x0037A0ED
		private void OnTriggerExit(Collider other)
		{
			if (other.CompareTag(this.playerTag))
			{
				this.canInteract = false;
				this.CancelInteraction();
				if (this.uiObject != null)
				{
					this.uiObject.SetActive(false);
				}
			}
		}

		// Token: 0x04008AE5 RID: 35557
		[Header("Interaction Settings")]
		[Tooltip("Key to hold for interaction")]
		public KeyCode interactionKey = KeyCode.E;

		// Token: 0x04008AE6 RID: 35558
		[Tooltip("How long (in seconds) the key must be held")]
		public float holdDuration = 1.5f;

		// Token: 0x04008AE7 RID: 35559
		[Tooltip("Tag of the player object to detect")]
		public string playerTag = "Player";

		// Token: 0x04008AE8 RID: 35560
		[Header("UI References")]
		[Tooltip("The World Space UI GameObject to show/hide")]
		public GameObject uiObject;

		// Token: 0x04008AE9 RID: 35561
		[Tooltip("The Progress Bar component from Modern UI Pack (optional)")]
		public ProgressBar progressBar;

		// Token: 0x04008AEA RID: 35562
		[Header("Events")]
		public UnityEvent onInteractionComplete;

		// Token: 0x04008AEB RID: 35563
		public UnityEvent onInteractionStart;

		// Token: 0x04008AEC RID: 35564
		public UnityEvent onInteractionCancel;

		// Token: 0x04008AED RID: 35565
		private bool canInteract;

		// Token: 0x04008AEE RID: 35566
		private bool isInteracting;

		// Token: 0x04008AEF RID: 35567
		private float currentHoldTime;
	}
}
