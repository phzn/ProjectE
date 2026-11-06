using System;
using System.Collections.Generic;
using Features.Inventory;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.InsertableToys
{
	// Token: 0x02001177 RID: 4471
	[CreateAssetMenu(fileName = "InsertableToy_New", menuName = "Features/Inventory/Insertable Toy Definition")]
	public sealed class InsertableToyItemSO : InventoryItemSO
	{
		// Token: 0x170015B0 RID: 5552
		// (get) Token: 0x06008887 RID: 34951 RVA: 0x0037E456 File Offset: 0x0037C656
		public GameObject InsertedPrefab
		{
			get
			{
				if (!(this.insertedPrefab != null))
				{
					return this.handPrefab;
				}
				return this.insertedPrefab;
			}
		}

		// Token: 0x170015B1 RID: 5553
		// (get) Token: 0x06008888 RID: 34952 RVA: 0x0037E473 File Offset: 0x0037C673
		public IReadOnlyList<string> WorldSourceNamePrefixes
		{
			get
			{
				return this.worldSourceNamePrefixes;
			}
		}

		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x06008889 RID: 34953 RVA: 0x0037E47B File Offset: 0x0037C67B
		public float SourceDetectionRadius
		{
			get
			{
				return this.sourceDetectionRadius;
			}
		}

		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x0600888A RID: 34954 RVA: 0x0037E483 File Offset: 0x0037C683
		public float SourcePromptHeight
		{
			get
			{
				return this.sourcePromptHeight;
			}
		}

		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x0600888B RID: 34955 RVA: 0x0037E48B File Offset: 0x0037C68B
		public bool ConsumeOneSourceChild
		{
			get
			{
				return this.consumeOneSourceChild;
			}
		}

		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x0600888C RID: 34956 RVA: 0x0037E493 File Offset: 0x0037C693
		public InteractablePromptUI PickupPromptPrefab
		{
			get
			{
				return this.pickupPromptPrefab;
			}
		}

		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x0600888D RID: 34957 RVA: 0x0037E49B File Offset: 0x0037C69B
		public string InsertedHudName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.insertedHudName))
				{
					return this.insertedHudName;
				}
				return this.itemName;
			}
		}

		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x0600888E RID: 34958 RVA: 0x0037E4B7 File Offset: 0x0037C6B7
		public string RemoveActionName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.removeActionName))
				{
					return this.removeActionName;
				}
				return "Remove toy";
			}
		}

		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x0600888F RID: 34959 RVA: 0x0037E4D2 File Offset: 0x0037C6D2
		public KeyCode RemoveInputKey
		{
			get
			{
				return this.removeInputKey;
			}
		}

		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x06008890 RID: 34960 RVA: 0x0037E4DA File Offset: 0x0037C6DA
		public string RemoveKeyDisplayLabel
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.removeKeyDisplayLabel))
				{
					return this.removeKeyDisplayLabel;
				}
				return this.removeInputKey.ToString();
			}
		}

		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x06008891 RID: 34961 RVA: 0x0037E501 File Offset: 0x0037C701
		public float InsertedRunningSpeed
		{
			get
			{
				return Mathf.Max(0f, this.insertedRunningSpeed);
			}
		}

		// Token: 0x170015BB RID: 5563
		// (get) Token: 0x06008892 RID: 34962 RVA: 0x0037E513 File Offset: 0x0037C713
		public AudioClip InsertionSfx
		{
			get
			{
				return this.insertionSfx;
			}
		}

		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x06008893 RID: 34963 RVA: 0x0037E51B File Offset: 0x0037C71B
		public float InsertionSfxVolume
		{
			get
			{
				return Mathf.Clamp01(this.insertionSfxVolume);
			}
		}

		// Token: 0x170015BD RID: 5565
		// (get) Token: 0x06008894 RID: 34964 RVA: 0x0037E528 File Offset: 0x0037C728
		public float InsertionSfxPitchMin
		{
			get
			{
				return Mathf.Clamp(Mathf.Min(this.insertionSfxPitchMin, this.insertionSfxPitchMax), 0.1f, 3f);
			}
		}

		// Token: 0x170015BE RID: 5566
		// (get) Token: 0x06008895 RID: 34965 RVA: 0x0037E54A File Offset: 0x0037C74A
		public float InsertionSfxPitchMax
		{
			get
			{
				return Mathf.Clamp(Mathf.Max(this.insertionSfxPitchMin, this.insertionSfxPitchMax), 0.1f, 3f);
			}
		}

		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x06008896 RID: 34966 RVA: 0x0037E56C File Offset: 0x0037C76C
		public AudioClip RemovalSfx
		{
			get
			{
				return this.removalSfx;
			}
		}

		// Token: 0x170015C0 RID: 5568
		// (get) Token: 0x06008897 RID: 34967 RVA: 0x0037E574 File Offset: 0x0037C774
		public float RemovalSfxVolume
		{
			get
			{
				return Mathf.Clamp01(this.removalSfxVolume);
			}
		}

		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x06008898 RID: 34968 RVA: 0x0037E581 File Offset: 0x0037C781
		public float RemovalSfxPitchMin
		{
			get
			{
				return Mathf.Clamp(Mathf.Min(this.removalSfxPitchMin, this.removalSfxPitchMax), 0.1f, 3f);
			}
		}

		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x06008899 RID: 34969 RVA: 0x0037E5A3 File Offset: 0x0037C7A3
		public float RemovalSfxPitchMax
		{
			get
			{
				return Mathf.Clamp(Mathf.Max(this.removalSfxPitchMin, this.removalSfxPitchMax), 0.1f, 3f);
			}
		}

		// Token: 0x0600889A RID: 34970 RVA: 0x0037E5C5 File Offset: 0x0037C7C5
		public InsertableToyPlacement GetPlacement(InsertionSite site)
		{
			if (site != InsertionSite.Anal)
			{
				return this.vaginalPlacement;
			}
			return this.analPlacement;
		}

		// Token: 0x170015C3 RID: 5571
		// (get) Token: 0x0600889B RID: 34971 RVA: 0x0037E5D7 File Offset: 0x0037C7D7
		public InsertableToyPlacement AnalPlacement
		{
			get
			{
				return this.analPlacement;
			}
		}

		// Token: 0x170015C4 RID: 5572
		// (get) Token: 0x0600889C RID: 34972 RVA: 0x0037E5DF File Offset: 0x0037C7DF
		public InsertableToyPlacement VaginalPlacement
		{
			get
			{
				return this.vaginalPlacement;
			}
		}

		// Token: 0x0600889D RID: 34973 RVA: 0x0037E5E8 File Offset: 0x0037C7E8
		public bool MatchesWorldSource(string objectName)
		{
			if (string.IsNullOrWhiteSpace(objectName) || this.worldSourceNamePrefixes == null)
			{
				return false;
			}
			for (int i = 0; i < this.worldSourceNamePrefixes.Count; i++)
			{
				string text = this.worldSourceNamePrefixes[i];
				if (!string.IsNullOrWhiteSpace(text) && objectName.StartsWith(text, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008B3C RID: 35644
		[Header("Inserted visual")]
		[Tooltip("Optional visual used on the body. Falls back to handPrefab when empty.")]
		[SerializeField]
		private GameObject insertedPrefab;

		// Token: 0x04008B3D RID: 35645
		[Tooltip("Anal placement/action. Disable it for toys that cannot use this route.")]
		[SerializeField]
		private InsertableToyPlacement analPlacement = new InsertableToyPlacement(InsertionSite.Anal, "Insert into butt", KeyCode.F, "Kupa_A", new Vector3(0f, -0.1195f, -0.0278f), new Vector3(9.484f, 0f, 0f), Vector3.one * 0.4f);

		// Token: 0x04008B3E RID: 35646
		[Tooltip("Vaginal placement/action. Disable it for toys that cannot use this route.")]
		[SerializeField]
		private InsertableToyPlacement vaginalPlacement = new InsertableToyPlacement(InsertionSite.Vaginal, "Insert into vagina", KeyCode.G, "Kupa_V_LR", new Vector3(0.0049f, -0.1457f, 0.0182f), new Vector3(-16.775f, 24.043f, -5f), Vector3.one * 0.4f);

		// Token: 0x04008B3F RID: 35647
		[Header("Authored world sources")]
		[Tooltip("Scene-object name prefixes treated as a stock/source for this item. For example, Bowling_Pins_Sets matches all numbered copies. Add future mappings here.")]
		[SerializeField]
		private List<string> worldSourceNamePrefixes = new List<string>();

		// Token: 0x04008B40 RID: 35648
		[SerializeField]
		[Min(0.25f)]
		private float sourceDetectionRadius = 1.35f;

		// Token: 0x04008B41 RID: 35649
		[SerializeField]
		[Min(0f)]
		private float sourcePromptHeight = 0.75f;

		// Token: 0x04008B42 RID: 35650
		[Tooltip("When enabled, every successful pickup hides one direct rendered child of the source.")]
		[SerializeField]
		private bool consumeOneSourceChild = true;

		// Token: 0x04008B43 RID: 35651
		[Tooltip("World-space prompt cloned by authored multi-item sources. This keeps the source UI visually identical to every other world interaction.")]
		[SerializeField]
		private InteractablePromptUI pickupPromptPrefab;

		// Token: 0x04008B44 RID: 35652
		[Header("Presentation")]
		[SerializeField]
		private string insertedHudName = "Inserted toy";

		// Token: 0x04008B45 RID: 35653
		[SerializeField]
		private string removeActionName = "Remove toy";

		// Token: 0x04008B46 RID: 35654
		[SerializeField]
		private KeyCode removeInputKey = KeyCode.X;

		// Token: 0x04008B47 RID: 35655
		[SerializeField]
		private string removeKeyDisplayLabel = "X";

		// Token: 0x04008B48 RID: 35656
		[Header("Inserted locomotion")]
		[Tooltip("Temporary free-locomotion run speed while this toy is inserted. Set to 0 to keep the character controller's current running speed.")]
		[SerializeField]
		[Min(0f)]
		private float insertedRunningSpeed = 1.3f;

		// Token: 0x04008B49 RID: 35657
		[Header("Insertion audio")]
		[Tooltip("Optional 2D SFX played once after this item has been inserted successfully.")]
		[SerializeField]
		private AudioClip insertionSfx;

		// Token: 0x04008B4A RID: 35658
		[SerializeField]
		[Range(0f, 1f)]
		private float insertionSfxVolume = 0.85f;

		// Token: 0x04008B4B RID: 35659
		[SerializeField]
		[Range(0.1f, 3f)]
		private float insertionSfxPitchMin = 0.95f;

		// Token: 0x04008B4C RID: 35660
		[SerializeField]
		[Range(0.1f, 3f)]
		private float insertionSfxPitchMax = 1.05f;

		// Token: 0x04008B4D RID: 35661
		[Tooltip("Optional 2D SFX played once after a voluntary removal has completed.")]
		[SerializeField]
		private AudioClip removalSfx;

		// Token: 0x04008B4E RID: 35662
		[SerializeField]
		[Range(0f, 1f)]
		private float removalSfxVolume = 0.85f;

		// Token: 0x04008B4F RID: 35663
		[SerializeField]
		[Range(0.1f, 3f)]
		private float removalSfxPitchMin = 0.95f;

		// Token: 0x04008B50 RID: 35664
		[SerializeField]
		[Range(0.1f, 3f)]
		private float removalSfxPitchMax = 1.05f;
	}
}
