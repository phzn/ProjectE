using System;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.Missions
{
	// Token: 0x02000FB7 RID: 4023
	public sealed class PauseMissionTabs : MonoBehaviour
	{
		// Token: 0x06007973 RID: 31091 RVA: 0x003197D1 File Offset: 0x003179D1
		public void Initialize(GameObject run, GameObject account, Button switchButton)
		{
			this._run = run;
			this._account = account;
			this._switchButton = switchButton;
			this._switchLabel = switchButton.GetComponentInChildren<TMP_Text>();
			switchButton.onClick.AddListener(new UnityAction(this.Toggle));
			this.ShowDefault();
		}

		// Token: 0x06007974 RID: 31092 RVA: 0x00319811 File Offset: 0x00317A11
		private void OnEnable()
		{
			MissionService.Changed += this.RefreshAvailability;
			this.ShowDefault();
		}

		// Token: 0x06007975 RID: 31093 RVA: 0x0031982A File Offset: 0x00317A2A
		private void OnDisable()
		{
			MissionService.Changed -= this.RefreshAvailability;
		}

		// Token: 0x06007976 RID: 31094 RVA: 0x0031983D File Offset: 0x00317A3D
		private void RefreshAvailability()
		{
			this.ShowRun(this._run != null && this._run.activeSelf);
		}

		// Token: 0x06007977 RID: 31095 RVA: 0x00319864 File Offset: 0x00317A64
		public void ShowDefault()
		{
			this.ShowRun(MissionService.AreRunMissionsEnabled && !SceneManager.GetSceneByName("CielHome").isLoaded && SceneManager.GetActiveScene().name != "MainMenu");
		}

		// Token: 0x06007978 RID: 31096 RVA: 0x003198AC File Offset: 0x00317AAC
		private void Toggle()
		{
			this.ShowRun(this._run != null && !this._run.activeSelf);
		}

		// Token: 0x06007979 RID: 31097 RVA: 0x003198D4 File Offset: 0x00317AD4
		private void ShowRun(bool show)
		{
			if (this._run == null || this._account == null)
			{
				return;
			}
			bool flag = MissionService.AreRunMissionsEnabled && MissionService.IsRunSceneContext && !SceneManager.GetSceneByName("CielHome").isLoaded;
			show = show && flag;
			if (this._switchButton != null)
			{
				this._switchButton.gameObject.SetActive(flag);
			}
			this._run.SetActive(show);
			this._account.SetActive(!show);
			if (this._switchLabel != null)
			{
				LocalizedGameText.BindSource(this._switchLabel, "rank_ui", show ? "ACCOUNT MISSIONS" : "RUN MISSIONS", null).FitTranslationToRectangle(false);
			}
		}

		// Token: 0x04007CC0 RID: 31936
		private GameObject _run;

		// Token: 0x04007CC1 RID: 31937
		private GameObject _account;

		// Token: 0x04007CC2 RID: 31938
		private TMP_Text _switchLabel;

		// Token: 0x04007CC3 RID: 31939
		private Button _switchButton;
	}
}
