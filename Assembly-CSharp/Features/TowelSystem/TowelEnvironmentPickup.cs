using System;
using Features.DebugTools;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.Missions;
using Features.RunModes;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.TowelSystem
{
	// Token: 0x02000C05 RID: 3077
	public class TowelEnvironmentPickup : MonoBehaviour, IInteractionPickupCandidate, IInteractionCandidate
	{
		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x060058E4 RID: 22756 RVA: 0x0022C673 File Offset: 0x0022A873
		// (set) Token: 0x060058E5 RID: 22757 RVA: 0x0022C67B File Offset: 0x0022A87B
		public bool CanPickUp { get; private set; }

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x060058E6 RID: 22758 RVA: 0x0022C684 File Offset: 0x0022A884
		public float CandidateSqrDistance
		{
			get
			{
				return this._sqrDistToPlayer;
			}
		}

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x060058E7 RID: 22759 RVA: 0x0022C68C File Offset: 0x0022A88C
		public bool IsCandidateEligible
		{
			get
			{
				return this.CanPickUp;
			}
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x060058E8 RID: 22760 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x060058E9 RID: 22761 RVA: 0x0022C694 File Offset: 0x0022A894
		public bool CanPrioritizePickup
		{
			get
			{
				return this._inventory != null && this.ResolveItem() != null && (this.FindClothSlot(this.ResolveItem()) >= 0 || this._inventory.CanAcceptWorldPickup(this.ResolveItem(), null, -1, true));
			}
		}

		// Token: 0x060058EA RID: 22762 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x060058EB RID: 22763 RVA: 0x0022C6EC File Offset: 0x0022A8EC
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this.SetPromptShown(false);
		}

		// Token: 0x060058EC RID: 22764 RVA: 0x0022C6FC File Offset: 0x0022A8FC
		private void Awake()
		{
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			if (this.promptUI != null && this.promptConfig != null)
			{
				this.promptUI.Show(this.promptConfig);
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x060058ED RID: 22765 RVA: 0x0022C764 File Offset: 0x0022A964
		private void Update()
		{
			this.CanPickUp = this.EvaluateCanPickUp();
			bool flag = this.CanPickUp && InteractionCandidateRegistry.IsWinner(this) && PlayerActivityGate.Allows(PlayerActivity.Interact);
			this.SetPromptShown(flag);
			if (flag && !FlyCamera.IsActive && this.pickupInput != null && this.pickupInput.GetButtonDown())
			{
				InteractionCandidateRegistry.MarkInteract();
				this.Pickup();
			}
		}

		// Token: 0x060058EE RID: 22766 RVA: 0x0022C7C9 File Offset: 0x0022A9C9
		private void SetPromptShown(bool shown)
		{
			if (this._promptShown == shown || this.promptUI == null)
			{
				return;
			}
			this._promptShown = shown;
			this.promptUI.gameObject.SetActive(shown);
		}

		// Token: 0x060058EF RID: 22767 RVA: 0x0022C7FC File Offset: 0x0022A9FC
		private bool EvaluateCanPickUp()
		{
			if (LongWayHomeLootPolicy.RestrictsWorldLoot)
			{
				return false;
			}
			this.ResolvePlayer();
			if (this._player == null || this._inventory == null || this._wearController == null)
			{
				return false;
			}
			if (this.ResolveItem() == null)
			{
				return false;
			}
			this._sqrDistToPlayer = (this._player.position - base.transform.position).sqrMagnitude;
			return this._sqrDistToPlayer <= this.pickupRange * this.pickupRange && !this._wearController.IsWorn(this.ResolveItem());
		}

		// Token: 0x060058F0 RID: 22768 RVA: 0x0022C8A8 File Offset: 0x0022AAA8
		private void Pickup()
		{
			if (LongWayHomeLootPolicy.RestrictsWorldLoot)
			{
				return;
			}
			WearableItemSO wearableItemSO = this.ResolveItem();
			if (this._inventory == null || this._wearController == null || wearableItemSO == null)
			{
				return;
			}
			int num = this.FindClothSlot(wearableItemSO);
			bool flag = false;
			if (num < 0)
			{
				num = this._inventory.AddClothingReturnIndex(wearableItemSO, false, wearableItemSO.rarity, -1, 1f, true);
				flag = num >= 0;
			}
			if (num < 0)
			{
				return;
			}
			this._wearController.WearClothFromSlot(wearableItemSO, num);
			if (flag)
			{
				MissionGameplaySignals.NotifyItemCollected(wearableItemSO, wearableItemSO.rarity);
			}
			if (this.pickupSound != null)
			{
				AudioSource.PlayClipAtPoint(this.pickupSound, base.transform.position, this.pickupSoundVolume);
			}
			UnityEvent onTowelPickedUp = this.OnTowelPickedUp;
			if (onTowelPickedUp != null)
			{
				onTowelPickedUp.Invoke();
			}
			if (this.consumeOnPickup)
			{
				this.CanPickUp = false;
				if (this.destroyOnPickup)
				{
					Object.Destroy(base.gameObject);
					return;
				}
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x060058F1 RID: 22769 RVA: 0x0022C9A8 File Offset: 0x0022ABA8
		private int FindClothSlot(WearableItemSO item)
		{
			int maxClothSlots = this._inventory.MaxClothSlots;
			for (int i = 0; i < maxClothSlots; i++)
			{
				if (this._inventory.GetClothAt(i) == item)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060058F2 RID: 22770 RVA: 0x0022C9E4 File Offset: 0x0022ABE4
		private WearableItemSO ResolveItem()
		{
			if (this.towelClothItem != null)
			{
				return this.towelClothItem;
			}
			foreach (WearableItemSO wearableItemSO in Resources.LoadAll<WearableItemSO>(this.wardrobeResourcesPath ?? string.Empty))
			{
				if (wearableItemSO != null && wearableItemSO.itemId == this.towelItemId)
				{
					this.towelClothItem = wearableItemSO;
					break;
				}
			}
			return this.towelClothItem;
		}

		// Token: 0x060058F3 RID: 22771 RVA: 0x0022CA58 File Offset: 0x0022AC58
		private void ResolvePlayer()
		{
			if (this._player != null && this._inventory != null && this._wearController != null)
			{
				return;
			}
			Transform player = PlayerRegistry.Player;
			if (player == null)
			{
				return;
			}
			PlayerInventoryManager playerInventoryManager = player.GetComponentInChildren<PlayerInventoryManager>(true) ?? PlayerInventoryManager.Instance;
			WearableUseController componentInChildren = player.GetComponentInChildren<WearableUseController>(true);
			if (playerInventoryManager == null || componentInChildren == null)
			{
				return;
			}
			this._player = player;
			this._inventory = playerInventoryManager;
			this._wearController = componentInChildren;
		}

		// Token: 0x060058F4 RID: 22772 RVA: 0x0022CAE1 File Offset: 0x0022ACE1
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(0.3f, 0.8f, 1f, 0.5f);
			Gizmos.DrawWireSphere(base.transform.position, this.pickupRange);
		}

		// Token: 0x04005972 RID: 22898
		[Header("Item")]
		[Tooltip("Il WearableItemSO Cloth_Towel da aggiungere all'inventario e indossare. Se vuoto, caricato da Resources per itemId 'cloth_towel'.")]
		[SerializeField]
		private WearableItemSO towelClothItem;

		// Token: 0x04005973 RID: 22899
		[Tooltip("Cartella Resources dei WearableItemSO cloth (come il ClothInventorySyncer).")]
		[SerializeField]
		private string wardrobeResourcesPath = "Wardrobe";

		// Token: 0x04005974 RID: 22900
		[SerializeField]
		private string towelItemId = "cloth_towel";

		// Token: 0x04005975 RID: 22901
		[Header("Input")]
		[SerializeField]
		private GenericInput pickupInput = new GenericInput("E", "A", "A");

		// Token: 0x04005976 RID: 22902
		[Tooltip("Max distance from the player to allow picking up this towel. Aligned with the clothes pickup radius (1.1) so every interactable engages at the same distance.")]
		[SerializeField]
		private float pickupRange = 1.1f;

		// Token: 0x04005977 RID: 22903
		[Header("UI Reference")]
		[Tooltip("World-space prompt shown while this towel is the active interactable (same UI as every other pickup). Auto-resolved from children when left empty.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04005978 RID: 22904
		[Tooltip("Config for the prompt (name/key/action). Optional; without it no prompt shows.")]
		[SerializeField]
		private InteractionConfigSO promptConfig;

		// Token: 0x04005979 RID: 22905
		[Header("Behaviour")]
		[Tooltip("Disable (or destroy) this scene towel after it has been picked up.")]
		[SerializeField]
		private bool consumeOnPickup = true;

		// Token: 0x0400597A RID: 22906
		[Tooltip("Destroy the GameObject instead of just deactivating it.")]
		[SerializeField]
		private bool destroyOnPickup;

		// Token: 0x0400597B RID: 22907
		[Header("Audio")]
		[Tooltip("Played at the towel position when it is picked up. Leave empty for no sound.")]
		[SerializeField]
		private AudioClip pickupSound;

		// Token: 0x0400597C RID: 22908
		[SerializeField]
		[Range(0f, 1f)]
		private float pickupSoundVolume = 1f;

		// Token: 0x0400597D RID: 22909
		[Header("Events")]
		public UnityEvent OnTowelPickedUp;

		// Token: 0x0400597E RID: 22910
		private PlayerInventoryManager _inventory;

		// Token: 0x0400597F RID: 22911
		private WearableUseController _wearController;

		// Token: 0x04005980 RID: 22912
		private Transform _player;

		// Token: 0x04005981 RID: 22913
		private float _sqrDistToPlayer = float.MaxValue;

		// Token: 0x04005982 RID: 22914
		private bool _promptShown;
	}
}
