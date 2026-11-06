using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B14 RID: 2836
	public class NpcSystemIconBatchGenerator : MonoBehaviour
	{
		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06005043 RID: 20547 RVA: 0x001F1324 File Offset: 0x001EF524
		public bool IsRunning
		{
			get
			{
				return this._running;
			}
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x001F132C File Offset: 0x001EF52C
		public void StartBatch()
		{
			if (this._running)
			{
				Debug.LogWarning("[NpcIconBatch] Già in esecuzione.");
				return;
			}
			CustomizationHubController customizationHubController = Object.FindObjectOfType<CustomizationHubController>(true);
			CustomizationHubPresetListController customizationHubPresetListController = Object.FindObjectOfType<CustomizationHubPresetListController>(true);
			if (customizationHubController == null || customizationHubPresetListController == null)
			{
				Debug.LogError("[NpcIconBatch] Hub o PresetListController non trovati in scena. Apri la Customization prima di avviare.");
				return;
			}
			Camera camera = this.ResolveCamera();
			if (camera == null)
			{
				Debug.LogError("[NpcIconBatch] Camera di cattura non trovata (campo captureCamera nullo e nessuna camera '" + this.cameraName + "' in scena).");
				return;
			}
			base.StartCoroutine(this.RunBatch(customizationHubController, customizationHubPresetListController, camera));
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x001F13B4 File Offset: 0x001EF5B4
		private Camera ResolveCamera()
		{
			if (this.captureCamera != null)
			{
				return this.captureCamera;
			}
			Camera[] array = Resources.FindObjectsOfTypeAll<Camera>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].gameObject.scene.IsValid() && array[i].name == this.cameraName)
				{
					this.captureCamera = array[i];
					return this.captureCamera;
				}
			}
			return null;
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x001F1432 File Offset: 0x001EF632
		private IEnumerator RunBatch(CustomizationHubController hub, CustomizationHubPresetListController list, Camera cam)
		{
			this._running = true;
			GameDataPaths.EnsureCustomizationDirectories();
			hub.SelectNpcTab();
			list.LoadPresetsForTarget(CustomizationTargetType.Npc, null, null);
			yield return null;
			List<string> ids = list.GetActiveNpcSystemPresetIds();
			if (ids == null || ids.Count == 0)
			{
				Debug.LogWarning("[NpcIconBatch] Nessun system NPC da processare.");
				this._running = false;
				yield break;
			}
			bool camWasActive = cam.gameObject.activeSelf;
			if (!camWasActive)
			{
				cam.gameObject.SetActive(true);
			}
			int saved = 0;
			int failed = 0;
			int num;
			for (int i = 0; i < ids.Count; i = num + 1)
			{
				string presetId = ids[i];
				list.SelectPreset(presetId, true, false);
				for (int f = 0; f < this.framesPerNpc; f = num + 1)
				{
					yield return null;
					num = f;
				}
				yield return new WaitForEndOfFrame();
				GameObject npcPreviewTarget = list.NpcPreviewTarget;
				if (npcPreviewTarget == null || !npcPreviewTarget.activeInHierarchy)
				{
					Debug.LogWarning("[NpcIconBatch] Preview NPC non attiva per '" + presetId + "', salto.");
					num = failed;
					failed = num + 1;
				}
				else
				{
					string systemPresetIconFileName = CustomizationHubPresetListController.GetSystemPresetIconFileName(presetId);
					string text = Path.Combine(GameDataPaths.CustomizationPresetIconsDirectory, systemPresetIconFileName);
					if (this.CaptureIcon(cam, text, systemPresetIconFileName))
					{
						num = saved;
						saved = num + 1;
					}
					else
					{
						num = failed;
						failed = num + 1;
					}
					if ((i + 1) % 20 == 0)
					{
						Debug.Log(string.Format("[NpcIconBatch] Progresso {0}/{1} (salvate {2}, fallite {3}).", new object[]
						{
							i + 1,
							ids.Count,
							saved,
							failed
						}));
					}
					presetId = null;
				}
				num = i;
			}
			if (!camWasActive)
			{
				cam.gameObject.SetActive(false);
			}
			Debug.Log(string.Format("[NpcIconBatch] COMPLETATO: {0} icone salvate, {1} fallite, su {2} NPC. ", saved, failed, ids.Count) + "Dir: " + GameDataPaths.CustomizationPresetIconsDirectory);
			list.LoadPresetsForTarget(CustomizationTargetType.Npc, null, null);
			this._running = false;
			yield break;
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x001F1458 File Offset: 0x001EF658
		private bool CaptureIcon(Camera cam, string outputPath, string fileName)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture targetTexture = cam.targetTexture;
			CameraClearFlags clearFlags = cam.clearFlags;
			Color color = cam.backgroundColor;
			RenderTexture temporary = RenderTexture.GetTemporary(this.iconWidth, this.iconHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			bool flag;
			try
			{
				if (this.overrideBackground)
				{
					cam.clearFlags = CameraClearFlags.Color;
					cam.backgroundColor = this.backgroundColor;
				}
				cam.targetTexture = temporary;
				CustomizationHubPresetListController customizationHubPresetListController = Object.FindObjectOfType<CustomizationHubPresetListController>(true);
				NpcPortraitRendering.Render(cam, temporary, (customizationHubPresetListController != null) ? customizationHubPresetListController.NpcPreviewTarget : null, true);
				RenderTexture.active = temporary;
				Texture2D texture2D = new Texture2D(this.iconWidth, this.iconHeight, TextureFormat.RGBA32, false);
				texture2D.ReadPixels(new Rect(0f, 0f, (float)this.iconWidth, (float)this.iconHeight), 0, 0);
				texture2D.Apply();
				byte[] array = texture2D.EncodeToPNG();
				Object.Destroy(texture2D);
				File.WriteAllBytes(outputPath, array);
				PresetIconCache.Invalidate(fileName);
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[NpcIconBatch] Cattura fallita per '" + outputPath + "': " + ex.Message);
				flag = false;
			}
			finally
			{
				RenderTexture.active = active;
				cam.targetTexture = targetTexture;
				cam.clearFlags = clearFlags;
				cam.backgroundColor = color;
				RenderTexture.ReleaseTemporary(temporary);
			}
			return flag;
		}

		// Token: 0x04005100 RID: 20736
		[Header("Camera")]
		[Tooltip("Camera dedicata per lo scatto (già posizionata, con cullingMask che mostra solo l'NPC). Se nulla, viene cercata in scena per nome (cameraName).")]
		[SerializeField]
		private Camera captureCamera;

		// Token: 0x04005101 RID: 20737
		[Tooltip("Nome della camera da cercare in scena se captureCamera è nulla.")]
		[SerializeField]
		private string cameraName = "NPCPresetsCamera";

		// Token: 0x04005102 RID: 20738
		[Header("Capture")]
		[Tooltip("Larghezza (px) del ritratto NPC quadrato a mezzo busto.")]
		[SerializeField]
		private int iconWidth = 512;

		// Token: 0x04005103 RID: 20739
		[Tooltip("Altezza (px) del ritratto NPC quadrato a mezzo busto.")]
		[SerializeField]
		private int iconHeight = 512;

		// Token: 0x04005104 RID: 20740
		[Tooltip("Se true, forza clearFlags=SolidColor + backgroundColor durante lo scatto (utile per sfondo trasparente). Se false, usa le impostazioni della camera così come sono.")]
		[SerializeField]
		private bool overrideBackground = true;

		// Token: 0x04005105 RID: 20741
		[Tooltip("Sfondo usato se overrideBackground è true. Alpha 0 = trasparente.")]
		[SerializeField]
		private Color backgroundColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04005106 RID: 20742
		[Tooltip("Frame da attendere dopo aver selezionato un NPC, per far completare il load BMAC.")]
		[SerializeField]
		private int framesPerNpc = 8;

		// Token: 0x04005107 RID: 20743
		[Tooltip("In Editor aggiorna anche le icone Resources incluse nella build, non solo la cache persistente.")]
		[SerializeField]
		private bool updateProjectResourceIconsInEditor = true;

		// Token: 0x04005108 RID: 20744
		private bool _running;
	}
}
