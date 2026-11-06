using System;
using Invector.vCharacterController;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.BoobTouch
{
	// Token: 0x020014F6 RID: 5366
	[RequireComponent(typeof(Collider))]
	public class SoapPlayZone : MonoBehaviour
	{
		// Token: 0x0600A5C7 RID: 42439 RVA: 0x00453F80 File Offset: 0x00452180
		private void Update()
		{
			if (this._playerInRange == null || this._controller == null)
			{
				return;
			}
			bool flag = !this._controller.IsActive;
			if (flag != this._promptShown)
			{
				if (flag)
				{
					this.ShowPrompt();
				}
				else
				{
					this.HidePrompt();
				}
			}
			if (!this._controller.IsActive && this.interactInput != null && this.interactInput.GetButtonDown())
			{
				this.HidePrompt();
				this._controller.TryEnterMode();
			}
		}

		// Token: 0x0600A5C8 RID: 42440 RVA: 0x00454008 File Offset: 0x00452208
		private void OnTriggerEnter(Collider other)
		{
			GameObject gameObject;
			if (!this.IsPlayer(other, out gameObject))
			{
				return;
			}
			this._playerColliderCount++;
			if (this._playerInRange == null)
			{
				this._playerInRange = gameObject;
				this._controller = gameObject.GetComponentInChildren<BoobTouchModeController>(true);
				if (this._controller == null)
				{
					Debug.LogWarning("[SoapPlayZone] BoobTouchModeController non trovato sul player.", this);
				}
			}
		}

		// Token: 0x0600A5C9 RID: 42441 RVA: 0x0045406C File Offset: 0x0045226C
		private void OnTriggerExit(Collider other)
		{
			GameObject gameObject;
			if (!this.IsPlayer(other, out gameObject))
			{
				return;
			}
			if (this._playerInRange != null && gameObject != this._playerInRange)
			{
				return;
			}
			if (this._playerColliderCount > 0)
			{
				this._playerColliderCount--;
			}
			if (this._playerColliderCount > 0)
			{
				return;
			}
			this._playerColliderCount = 0;
			this.HidePrompt();
			this._playerInRange = null;
			this._controller = null;
		}

		// Token: 0x0600A5CA RID: 42442 RVA: 0x004540DE File Offset: 0x004522DE
		private void ShowPrompt()
		{
			if (this.config != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.Show(this.config);
			}
			this._promptShown = true;
		}

		// Token: 0x0600A5CB RID: 42443 RVA: 0x00454112 File Offset: 0x00452312
		private void HidePrompt()
		{
			if (this._promptShown && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.Hide();
			}
			this._promptShown = false;
		}

		// Token: 0x0600A5CC RID: 42444 RVA: 0x0045413A File Offset: 0x0045233A
		private void OnDisable()
		{
			this.HidePrompt();
		}

		// Token: 0x0600A5CD RID: 42445 RVA: 0x00454144 File Offset: 0x00452344
		private bool IsPlayer(Collider other, out GameObject playerRoot)
		{
			playerRoot = null;
			if (other == null)
			{
				return false;
			}
			Transform player = PlayerRegistry.Player;
			if (player != null)
			{
				if (other.transform == player || other.transform.IsChildOf(player))
				{
					playerRoot = player.gameObject;
					return true;
				}
				return false;
			}
			else
			{
				Transform root = other.transform.root;
				if (root.CompareTag(this.playerTag))
				{
					playerRoot = root.gameObject;
					return true;
				}
				return false;
			}
		}

		// Token: 0x0400AA4E RID: 43598
		[Tooltip("Tag del player (fallback se PlayerRegistry non c'e')")]
		[SerializeField]
		private string playerTag = "Player";

		// Token: 0x0400AA4F RID: 43599
		[Tooltip("Input per avviare (stesso tipo delle altre interazioni del gioco)")]
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "X", "E");

		// Token: 0x0400AA50 RID: 43600
		[Tooltip("Config del prompt HUD (creato dal setup tool)")]
		[SerializeField]
		private InteractionConfigSO config;

		// Token: 0x0400AA51 RID: 43601
		private GameObject _playerInRange;

		// Token: 0x0400AA52 RID: 43602
		private BoobTouchModeController _controller;

		// Token: 0x0400AA53 RID: 43603
		private int _playerColliderCount;

		// Token: 0x0400AA54 RID: 43604
		private bool _promptShown;
	}
}
