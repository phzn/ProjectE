using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000166 RID: 358
public class GunWheelRadialMenuHandler : MonoBehaviour
{
	// Token: 0x06000B96 RID: 2966 RVA: 0x000502A8 File Offset: 0x0004E4A8
	private void Start()
	{
		this.weaponRadialMenu.ClearMenu(0);
		for (int i = 0; i < this.LightWeapons.Length; i++)
		{
			this.LightWeapons[i].radialButtonInfo.name = this.LightWeapons[i].name;
			this.LightWeapons[i].radialButtonInfo.key = this.LightWeapons[i].key;
			this.LightWeapons[i].radialButtonInfo.description = this.LightWeapons[i].description;
			this.LightWeapons[i].radialButtonInfo.icon = this.LightWeapons[i].weaponIcon;
			this.weaponRadialMenu.RegisterButton(new Action<string>(this.ShowCurrentWeaponInfo), this.LightWeapons[i].radialButtonInfo, -1);
			this.WeaponDictionary.Add(this.LightWeapons[i].key, this.LightWeapons[i]);
		}
		for (int j = 0; j < this.UtilityWeapons.Length; j++)
		{
			this.UtilityWeapons[j].radialButtonInfo.name = this.UtilityWeapons[j].name;
			this.UtilityWeapons[j].radialButtonInfo.key = this.UtilityWeapons[j].key;
			this.UtilityWeapons[j].radialButtonInfo.description = this.UtilityWeapons[j].description;
			this.UtilityWeapons[j].radialButtonInfo.icon = this.UtilityWeapons[j].weaponIcon;
			this.WeaponDictionary.Add(this.UtilityWeapons[j].key, this.UtilityWeapons[j]);
		}
		for (int k = 0; k < this.HeavyWeapons.Length; k++)
		{
			this.HeavyWeapons[k].radialButtonInfo.name = this.HeavyWeapons[k].name;
			this.HeavyWeapons[k].radialButtonInfo.key = this.HeavyWeapons[k].key;
			this.HeavyWeapons[k].radialButtonInfo.description = this.HeavyWeapons[k].description;
			this.HeavyWeapons[k].radialButtonInfo.icon = this.HeavyWeapons[k].weaponIcon;
			this.WeaponDictionary.Add(this.HeavyWeapons[k].key, this.HeavyWeapons[k]);
		}
		this.weaponRadialMenu.OnMenuEnabled += this.OnRadialMenuEnabled;
		this.weaponRadialMenu.OnMenuDisabled += this.OnRadialMenuDisabled;
		this.backgroundPanel.SetActive(false);
		this.horizontalInputMin = this.weaponRadialMenu.BasePosition.x - this.weaponRadialMenu.GetComponent<RectTransform>().sizeDelta.x / 2f;
		this.horizontalInputMax = this.weaponRadialMenu.BasePosition.x + this.weaponRadialMenu.GetComponent<RectTransform>().sizeDelta.x / 2f;
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x000505A4 File Offset: 0x0004E7A4
	public void NavigateLeft()
	{
		if (!this.weaponRadialMenu.IsEnabled)
		{
			return;
		}
		if (this.currentMenu == GunWheelRadialMenuHandler.CurrentMenu.UtilityWeapons)
		{
			return;
		}
		if (this.currentMenu == GunWheelRadialMenuHandler.CurrentMenu.LightWeapons)
		{
			this.currentMenu = GunWheelRadialMenuHandler.CurrentMenu.UtilityWeapons;
			this.PopulateRadialMenu(this.UtilityWeapons, "Utility Weapons");
			this.navigateLeftText.text = "";
			this.navigateRightText.text = ">\nLight\n>";
			return;
		}
		this.currentMenu = GunWheelRadialMenuHandler.CurrentMenu.LightWeapons;
		this.PopulateRadialMenu(this.LightWeapons, "Light Weapons");
		this.navigateLeftText.text = "<\nUtility\n<";
		this.navigateRightText.text = ">\nHeavy\n>";
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x00050644 File Offset: 0x0004E844
	public void NavigateRight()
	{
		if (!this.weaponRadialMenu.IsEnabled)
		{
			return;
		}
		if (this.currentMenu == GunWheelRadialMenuHandler.CurrentMenu.HeavyWeapons)
		{
			return;
		}
		if (this.currentMenu == GunWheelRadialMenuHandler.CurrentMenu.UtilityWeapons)
		{
			this.currentMenu = GunWheelRadialMenuHandler.CurrentMenu.LightWeapons;
			this.PopulateRadialMenu(this.LightWeapons, "Light Weapons");
			this.navigateLeftText.text = "<\nUtility\n<";
			this.navigateRightText.text = ">\nHeavy\n>";
			return;
		}
		this.currentMenu = GunWheelRadialMenuHandler.CurrentMenu.HeavyWeapons;
		this.PopulateRadialMenu(this.HeavyWeapons, "Heavy Weapons");
		this.navigateLeftText.text = "<\nLight\n<";
		this.navigateRightText.text = "";
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x000506E4 File Offset: 0x0004E8E4
	private void PopulateRadialMenu(GunWheelRadialMenuHandler.WeaponBase[] weapons, string menuName)
	{
		this.weaponRadialMenu.ClearMenu(0);
		for (int i = 0; i < weapons.Length; i++)
		{
			this.weaponRadialMenu.RegisterButton(new Action<string>(this.ShowCurrentWeaponInfo), weapons[i].radialButtonInfo, -1);
		}
		this.radialMenuTitleText.text = menuName;
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x00050737 File Offset: 0x0004E937
	private void OnRadialMenuEnabled()
	{
		this.backgroundPanel.SetActive(true);
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x00050745 File Offset: 0x0004E945
	private void OnRadialMenuDisabled()
	{
		this.backgroundPanel.SetActive(false);
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x00050754 File Offset: 0x0004E954
	private void ShowCurrentWeaponInfo(string key)
	{
		if (!this.WeaponDictionary.ContainsKey(key))
		{
			return;
		}
		if (this.currentWeapon != null)
		{
			this.currentWeapon.radialButtonInfo.Selected = false;
		}
		this.nameText.text = this.WeaponDictionary[key].name;
		this.descriptionText.text = "Description:\n" + this.WeaponDictionary[key].description;
		this.damageText.text = "Damage: " + this.WeaponDictionary[key].damage.ToString();
		this.roundsPerMiinute.text = "Rounds Per Minute: " + this.WeaponDictionary[key].roundsPerMinute.ToString();
		this.gunIcon.sprite = this.WeaponDictionary[key].weaponIcon;
		this.currentWeapon = this.WeaponDictionary[key];
		this.WeaponDictionary[key].radialButtonInfo.Selected = true;
	}

	// Token: 0x04000B4F RID: 2895
	public GunWheelRadialMenuHandler.WeaponBase[] LightWeapons;

	// Token: 0x04000B50 RID: 2896
	public GunWheelRadialMenuHandler.WeaponBase[] HeavyWeapons;

	// Token: 0x04000B51 RID: 2897
	public GunWheelRadialMenuHandler.WeaponBase[] UtilityWeapons;

	// Token: 0x04000B52 RID: 2898
	private Dictionary<string, GunWheelRadialMenuHandler.WeaponBase> WeaponDictionary = new Dictionary<string, GunWheelRadialMenuHandler.WeaponBase>();

	// Token: 0x04000B53 RID: 2899
	private GunWheelRadialMenuHandler.WeaponBase currentWeapon;

	// Token: 0x04000B54 RID: 2900
	public GameObject backgroundPanel;

	// Token: 0x04000B55 RID: 2901
	public UltimateRadialMenu weaponRadialMenu;

	// Token: 0x04000B56 RID: 2902
	public Text radialMenuTitleText;

	// Token: 0x04000B57 RID: 2903
	public Text nameText;

	// Token: 0x04000B58 RID: 2904
	public Text descriptionText;

	// Token: 0x04000B59 RID: 2905
	public Text damageText;

	// Token: 0x04000B5A RID: 2906
	public Text roundsPerMiinute;

	// Token: 0x04000B5B RID: 2907
	public Image gunIcon;

	// Token: 0x04000B5C RID: 2908
	private float horizontalInputMin;

	// Token: 0x04000B5D RID: 2909
	private float horizontalInputMax;

	// Token: 0x04000B5E RID: 2910
	private GunWheelRadialMenuHandler.CurrentMenu currentMenu;

	// Token: 0x04000B5F RID: 2911
	public Text navigateLeftText;

	// Token: 0x04000B60 RID: 2912
	public Text navigateRightText;

	// Token: 0x02000167 RID: 359
	[Serializable]
	public class WeaponBase
	{
		// Token: 0x04000B61 RID: 2913
		public string name;

		// Token: 0x04000B62 RID: 2914
		public string description;

		// Token: 0x04000B63 RID: 2915
		public string key;

		// Token: 0x04000B64 RID: 2916
		public int damage = 10;

		// Token: 0x04000B65 RID: 2917
		public int roundsPerMinute = 100;

		// Token: 0x04000B66 RID: 2918
		public Sprite weaponIcon;

		// Token: 0x04000B67 RID: 2919
		public UltimateRadialButtonInfo radialButtonInfo;
	}

	// Token: 0x02000168 RID: 360
	private enum CurrentMenu
	{
		// Token: 0x04000B69 RID: 2921
		LightWeapons,
		// Token: 0x04000B6A RID: 2922
		HeavyWeapons,
		// Token: 0x04000B6B RID: 2923
		UtilityWeapons
	}
}
