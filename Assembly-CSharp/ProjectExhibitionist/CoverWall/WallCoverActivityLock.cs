using System;
using Invector.vCover;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectExhibitionist.CoverWall
{
	// Token: 0x02000A00 RID: 2560
	public class WallCoverActivityLock : MonoBehaviour
	{
		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x060048F9 RID: 18681 RVA: 0x001BDB2C File Offset: 0x001BBD2C
		public static vBasicCoverController ActiveController
		{
			get
			{
				Transform player = PlayerRegistry.Player;
				vBasicCoverController vBasicCoverController = ((player != null) ? player.GetComponentInChildren<vBasicCoverController>(true) : null);
				if (!(vBasicCoverController != null) || !vBasicCoverController.isActiveAndEnabled || (!vBasicCoverController.inCover && !vBasicCoverController.goingToCoverPoint))
				{
					return null;
				}
				return vBasicCoverController;
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x060048FA RID: 18682 RVA: 0x001BDB77 File Offset: 0x001BBD77
		public static bool IsPlayerInCover
		{
			get
			{
				return WallCoverActivityLock.ActiveController != null && WallCoverActivityLock.ActiveController.inCover && !WallCoverActivityLock.ActiveController.goingToCoverPoint;
			}
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x001BDBA1 File Offset: 0x001BBDA1
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (Object.FindObjectOfType<WallCoverActivityLock>() != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[WallCoverActivityLock]");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<WallCoverActivityLock>();
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x001BDBC8 File Offset: 0x001BBDC8
		private void Update()
		{
			Transform player = PlayerRegistry.Player;
			vBasicCoverController vBasicCoverController = this.controller;
			if (player != this.resolvedPlayer || (player != null && this.controller == null))
			{
				this.resolvedPlayer = player;
				vBasicCoverController = ((player != null) ? player.GetComponentInChildren<vBasicCoverController>(true) : null);
			}
			if (vBasicCoverController != this.subscribed)
			{
				this.Unsubscribe();
				this.controller = vBasicCoverController;
				if (this.controller != null)
				{
					this.controller.onEnterCover.AddListener(new UnityAction(this.HandleEnter));
					this.controller.onExitCover.AddListener(new UnityAction(this.HandleExit));
					this.subscribed = this.controller;
				}
			}
			if (this.controller != null && (this.controller.inCover || this.controller.goingToCoverPoint))
			{
				WallCoverActivityLock.EnsureLocked();
				return;
			}
			WallCoverActivityLock.EnsureUnlocked();
		}

		// Token: 0x060048FD RID: 18685 RVA: 0x001BDCBD File Offset: 0x001BBEBD
		private void OnDisable()
		{
			this.HandleExit();
			this.Unsubscribe();
		}

		// Token: 0x060048FE RID: 18686 RVA: 0x001BDCCC File Offset: 0x001BBECC
		private void Unsubscribe()
		{
			if (this.subscribed == null)
			{
				this.subscribed = null;
				return;
			}
			UnityEvent onEnterCover = this.subscribed.onEnterCover;
			if (onEnterCover != null)
			{
				onEnterCover.RemoveListener(new UnityAction(this.HandleEnter));
			}
			UnityEvent onExitCover = this.subscribed.onExitCover;
			if (onExitCover != null)
			{
				onExitCover.RemoveListener(new UnityAction(this.HandleExit));
			}
			this.subscribed = null;
		}

		// Token: 0x060048FF RID: 18687 RVA: 0x001BDD3A File Offset: 0x001BBF3A
		private void HandleEnter()
		{
			WallCoverActivityLock.EnsureLocked();
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x001BDD41 File Offset: 0x001BBF41
		private void HandleExit()
		{
			WallCoverActivityLock.EnsureUnlocked();
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x001BDD48 File Offset: 0x001BBF48
		private static void EnsureLocked()
		{
			if (!PlayerActivityGate.HasLock("WallCover.InCover"))
			{
				PlayerActivityGate.Push("WallCover.InCover", PlayerActivity.Emote | PlayerActivity.Flash | PlayerActivity.Interact | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.Crouch);
			}
		}

		// Token: 0x06004902 RID: 18690 RVA: 0x001BDD65 File Offset: 0x001BBF65
		private static void EnsureUnlocked()
		{
			if (PlayerActivityGate.HasLock("WallCover.InCover"))
			{
				PlayerActivityGate.Pop("WallCover.InCover");
			}
		}

		// Token: 0x040048EF RID: 18671
		public const string Reason = "WallCover.InCover";

		// Token: 0x040048F0 RID: 18672
		public const PlayerActivity BlockedWhileInCover = PlayerActivity.Emote | PlayerActivity.Flash | PlayerActivity.Interact | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.Crouch;

		// Token: 0x040048F1 RID: 18673
		private vBasicCoverController controller;

		// Token: 0x040048F2 RID: 18674
		private vBasicCoverController subscribed;

		// Token: 0x040048F3 RID: 18675
		private Transform resolvedPlayer;
	}
}
