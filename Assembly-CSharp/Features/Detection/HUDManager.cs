using System;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012B6 RID: 4790
	public class HUDManager : MonoBehaviour
	{
		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x06009491 RID: 38033 RVA: 0x003D165E File Offset: 0x003CF85E
		// (set) Token: 0x06009492 RID: 38034 RVA: 0x003D1665 File Offset: 0x003CF865
		public static HUDManager Instance { get; private set; }

		// Token: 0x170017A8 RID: 6056
		// (get) Token: 0x06009493 RID: 38035 RVA: 0x003D166D File Offset: 0x003CF86D
		public bool IsHidden
		{
			get
			{
				return this._isHidden;
			}
		}

		// Token: 0x06009494 RID: 38036 RVA: 0x003D1675 File Offset: 0x003CF875
		private void Awake()
		{
			if (HUDManager.Instance == null)
			{
				HUDManager.Instance = this;
				return;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06009495 RID: 38037 RVA: 0x003D1696 File Offset: 0x003CF896
		private void Start()
		{
			if (this.autoFindHUD)
			{
				this.AutoFindHUDElements();
			}
		}

		// Token: 0x06009496 RID: 38038 RVA: 0x003D16A8 File Offset: 0x003CF8A8
		private void AutoFindHUDElements()
		{
			vHUDController vHUDController = Object.FindObjectOfType<vHUDController>();
			if (vHUDController != null && !this.hudElements.Contains(vHUDController.gameObject))
			{
				this.hudElements.Add(vHUDController.gameObject);
			}
			AttentionUIController attentionUIController = Object.FindObjectOfType<AttentionUIController>();
			if (attentionUIController != null && !this.hudElements.Contains(attentionUIController.gameObject))
			{
				this.hudElements.Add(attentionUIController.gameObject);
			}
		}

		// Token: 0x06009497 RID: 38039 RVA: 0x003D171B File Offset: 0x003CF91B
		public void RegisterHUDElement(GameObject element)
		{
			if (element != null && !this.hudElements.Contains(element))
			{
				this.hudElements.Add(element);
			}
		}

		// Token: 0x06009498 RID: 38040 RVA: 0x003D1740 File Offset: 0x003CF940
		public void UnregisterHUDElement(GameObject element)
		{
			if (element != null)
			{
				this.hudElements.Remove(element);
				this._originalStates.Remove(element);
			}
		}

		// Token: 0x06009499 RID: 38041 RVA: 0x003D1768 File Offset: 0x003CF968
		public void HideHUD()
		{
			if (this._isHidden)
			{
				return;
			}
			this._originalStates.Clear();
			this._canvasGroupStates.Clear();
			foreach (GameObject gameObject in this.hudElements)
			{
				if (!(gameObject == null))
				{
					if (this.hideViaCanvasGroup)
					{
						CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
						bool flag = false;
						if (canvasGroup == null)
						{
							canvasGroup = gameObject.AddComponent<CanvasGroup>();
							flag = true;
						}
						this._canvasGroupStates[canvasGroup] = new HUDManager.CanvasGroupSnapshot
						{
							alpha = canvasGroup.alpha,
							interactable = canvasGroup.interactable,
							blocksRaycasts = canvasGroup.blocksRaycasts,
							wasAddedByUs = flag
						};
						canvasGroup.alpha = 0f;
						canvasGroup.interactable = false;
						canvasGroup.blocksRaycasts = false;
					}
					else
					{
						this._originalStates[gameObject] = gameObject.activeSelf;
						gameObject.SetActive(false);
					}
				}
			}
			this._isHidden = true;
		}

		// Token: 0x0600949A RID: 38042 RVA: 0x003D1888 File Offset: 0x003CFA88
		public void ShowHUD()
		{
			if (!this._isHidden)
			{
				return;
			}
			foreach (KeyValuePair<CanvasGroup, HUDManager.CanvasGroupSnapshot> keyValuePair in this._canvasGroupStates)
			{
				CanvasGroup key = keyValuePair.Key;
				if (!(key == null))
				{
					HUDManager.CanvasGroupSnapshot value = keyValuePair.Value;
					if (value.wasAddedByUs)
					{
						Object.Destroy(key);
					}
					else
					{
						key.alpha = value.alpha;
						key.interactable = value.interactable;
						key.blocksRaycasts = value.blocksRaycasts;
					}
				}
			}
			this._canvasGroupStates.Clear();
			foreach (KeyValuePair<GameObject, bool> keyValuePair2 in this._originalStates)
			{
				if (keyValuePair2.Key != null)
				{
					keyValuePair2.Key.SetActive(keyValuePair2.Value);
				}
			}
			this._originalStates.Clear();
			this._isHidden = false;
		}

		// Token: 0x0600949B RID: 38043 RVA: 0x003D19A8 File Offset: 0x003CFBA8
		public void ToggleHUD()
		{
			if (this._isHidden)
			{
				this.ShowHUD();
				return;
			}
			this.HideHUD();
		}

		// Token: 0x040099A3 RID: 39331
		[Header("HUD Elements")]
		[Tooltip("List of HUD GameObjects to hide during detection")]
		[SerializeField]
		private List<GameObject> hudElements = new List<GameObject>();

		// Token: 0x040099A4 RID: 39332
		[Tooltip("If true, automatically finds common HUD elements on start")]
		[SerializeField]
		private bool autoFindHUD = true;

		// Token: 0x040099A5 RID: 39333
		[Tooltip("Se true, nasconde gli elementi HUD rendendoli INVISIBILI via CanvasGroup (alpha=0) invece di disattivare il GameObject. Cosi' i componenti sull'HUD (es. HeartbeatController) continuano a girare l'Update anche mentre l'HUD e' nascosto (il battito prosegue durante la detection). Se un elemento non e' UI/non ha CanvasGroup, si ripiega su SetActive(false).")]
		[SerializeField]
		private bool hideViaCanvasGroup = true;

		// Token: 0x040099A6 RID: 39334
		private Dictionary<GameObject, bool> _originalStates = new Dictionary<GameObject, bool>();

		// Token: 0x040099A7 RID: 39335
		private readonly Dictionary<CanvasGroup, HUDManager.CanvasGroupSnapshot> _canvasGroupStates = new Dictionary<CanvasGroup, HUDManager.CanvasGroupSnapshot>();

		// Token: 0x040099A8 RID: 39336
		private bool _isHidden;

		// Token: 0x020012B7 RID: 4791
		private struct CanvasGroupSnapshot
		{
			// Token: 0x040099A9 RID: 39337
			public float alpha;

			// Token: 0x040099AA RID: 39338
			public bool interactable;

			// Token: 0x040099AB RID: 39339
			public bool blocksRaycasts;

			// Token: 0x040099AC RID: 39340
			public bool wasAddedByUs;
		}
	}
}
