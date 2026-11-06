using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UltimateRadialMenuExample.CharacterInventory2D
{
	// Token: 0x0200017C RID: 380
	public class CharacterInventoryGameManager : MonoBehaviour
	{
		// Token: 0x06000BE4 RID: 3044 RVA: 0x00052908 File Offset: 0x00050B08
		private void Start()
		{
			this.backgroundSprite.size = new Vector2(Camera.main.orthographicSize * (float)Screen.width / (float)Screen.height * 2f, Camera.main.orthographicSize * 2f);
			this.backgroundSprite.transform.position = Vector2.zero;
			this.spawnRangeMin = -this.backgroundSprite.size / 2f * 0.95f;
			this.spawnRangeMax = this.backgroundSprite.size / 2f * 0.95f;
			this.itemBasePrefab.SetActive(false);
			this.radialMenu.ClearMenu(0);
			for (int i = 0; i < this.items.Length; i++)
			{
				this.items[i].buttonInfo.key = this.items[i].name;
				this.items[i].buttonInfo.name = this.items[i].name;
				this.items[i].buttonInfo.icon = this.items[i].itemSprite;
				this.itemDictionary.Add(this.items[i].name, this.items[i]);
				this.radialMenu.RegisterButton(new Action<string>(this.UseItem), this.items[i].buttonInfo, -1);
				this.items[i].buttonInfo.DisableButton();
				this.items[i].buttonInfo.UpdateText("");
			}
			this.radialMenu.OnMenuEnabled += this.PauseGame;
			this.radialMenu.OnMenuDisabled += this.ResumeGame;
			this.pauseScreen.SetActive(false);
			this.radialMenu.OnButtonInteract += delegate(int buttonIndex)
			{
				if (!this.hasInteractedWithButton)
				{
					this.tutorialText.text = "Awesome! When you interact with a radial button, check out the Console to see that the radial button called the right item.";
				}
				this.hasInteractedWithButton = true;
			};
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00052B0C File Offset: 0x00050D0C
		private void Update()
		{
			this.itemSpawningTimer += Time.deltaTime;
			if (this.itemSpawningTimer >= this.itemSpawningRate)
			{
				this.itemSpawningTimer -= this.itemSpawningRate;
				GameObject gameObject = Object.Instantiate<GameObject>(this.itemBasePrefab, new Vector3(Random.Range(this.spawnRangeMin.x, this.spawnRangeMax.x), Random.Range(this.spawnRangeMin.y, this.spawnRangeMax.y)), Quaternion.identity);
				gameObject.SetActive(true);
				CharacterInventoryGameManager.ItemInformation itemInformation = this.items[Random.Range(0, this.items.Length)];
				gameObject.GetComponent<SpriteRenderer>().sprite = itemInformation.itemSprite;
				gameObject.GetComponent<WorldItem>().myManager = this;
				gameObject.GetComponent<WorldItem>().myInformation = itemInformation;
			}
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00052BE0 File Offset: 0x00050DE0
		public void PickupItem(CharacterInventoryGameManager.ItemInformation itemInfo)
		{
			if (itemInfo.itemCount == 0)
			{
				itemInfo.buttonInfo.EnableButton();
			}
			itemInfo.itemCount++;
			itemInfo.buttonInfo.UpdateText(itemInfo.itemCount.ToString());
			if (!this.hasPickedUpItems)
			{
				this.tutorialText.text = "Great! Now press the TAB key to open your radial menu. Then click on the item that you want to use.";
				this.hasPickedUpItems = true;
			}
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00052C44 File Offset: 0x00050E44
		private void UseItem(string itemKey)
		{
			if (!this.itemDictionary.ContainsKey(itemKey))
			{
				Debug.LogWarning("Key does not exist in the dictionary.");
				return;
			}
			this.itemDictionary[itemKey].itemCount--;
			this.itemDictionary[itemKey].buttonInfo.UpdateText(this.itemDictionary[itemKey].itemCount.ToString());
			this.itemDictionary[itemKey].UseItem();
			if (this.itemDictionary[itemKey].itemCount <= 0)
			{
				this.itemDictionary[itemKey].buttonInfo.DisableButton();
				this.itemDictionary[itemKey].buttonInfo.UpdateText("");
			}
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00052D05 File Offset: 0x00050F05
		private void PauseGame()
		{
			this.pauseScreen.SetActive(true);
			Time.timeScale = 0f;
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00052D1D File Offset: 0x00050F1D
		private void ResumeGame()
		{
			this.pauseScreen.SetActive(false);
			Time.timeScale = 1f;
		}

		// Token: 0x04000BE9 RID: 3049
		public Text tutorialText;

		// Token: 0x04000BEA RID: 3050
		private bool hasPickedUpItems;

		// Token: 0x04000BEB RID: 3051
		private bool hasInteractedWithButton;

		// Token: 0x04000BEC RID: 3052
		public UltimateRadialMenu radialMenu;

		// Token: 0x04000BED RID: 3053
		public GameObject pauseScreen;

		// Token: 0x04000BEE RID: 3054
		public SpriteRenderer backgroundSprite;

		// Token: 0x04000BEF RID: 3055
		private float itemSpawningTimer;

		// Token: 0x04000BF0 RID: 3056
		public float itemSpawningRate = 2.5f;

		// Token: 0x04000BF1 RID: 3057
		private Vector2 spawnRangeMin;

		// Token: 0x04000BF2 RID: 3058
		private Vector2 spawnRangeMax;

		// Token: 0x04000BF3 RID: 3059
		public GameObject itemBasePrefab;

		// Token: 0x04000BF4 RID: 3060
		public CharacterInventoryGameManager.ItemInformation[] items;

		// Token: 0x04000BF5 RID: 3061
		private Dictionary<string, CharacterInventoryGameManager.ItemInformation> itemDictionary = new Dictionary<string, CharacterInventoryGameManager.ItemInformation>();

		// Token: 0x0200017D RID: 381
		[Serializable]
		public class ItemInformation
		{
			// Token: 0x06000BEC RID: 3052 RVA: 0x00052D74 File Offset: 0x00050F74
			public void UseItem()
			{
				Debug.Log("Using: " + this.name);
			}

			// Token: 0x04000BF6 RID: 3062
			public string name;

			// Token: 0x04000BF7 RID: 3063
			public Sprite itemSprite;

			// Token: 0x04000BF8 RID: 3064
			public int itemCount;

			// Token: 0x04000BF9 RID: 3065
			public UltimateRadialButtonInfo buttonInfo;
		}
	}
}
