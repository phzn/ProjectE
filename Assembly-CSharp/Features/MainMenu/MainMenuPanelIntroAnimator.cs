using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x02001067 RID: 4199
	[RequireComponent(typeof(CanvasGroup))]
	public sealed class MainMenuPanelIntroAnimator : MonoBehaviour
	{
		// Token: 0x06007F78 RID: 32632 RVA: 0x003415D3 File Offset: 0x0033F7D3
		public void Configure(RectTransform[] containers, RectTransform title)
		{
			this.rowContainers = containers;
			this.titleBlock = title;
		}

		// Token: 0x06007F79 RID: 32633 RVA: 0x003415E3 File Offset: 0x0033F7E3
		private void Awake()
		{
			this._group = base.GetComponent<CanvasGroup>();
		}

		// Token: 0x06007F7A RID: 32634 RVA: 0x003415F1 File Offset: 0x0033F7F1
		private void OnEnable()
		{
			this.StopAll();
			this._group.alpha = 0f;
			this._group.interactable = false;
			this._panelRoutine = base.StartCoroutine(this.OpenRoutine());
		}

		// Token: 0x06007F7B RID: 32635 RVA: 0x00341627 File Offset: 0x0033F827
		private void OnDisable()
		{
			this.StopAll();
			this.RestoreRows();
			this._group.alpha = 1f;
			this._group.interactable = true;
		}

		// Token: 0x06007F7C RID: 32636 RVA: 0x00341651 File Offset: 0x0033F851
		public void PlayRows(RectTransform container)
		{
			if (!base.isActiveAndEnabled || container == null)
			{
				return;
			}
			this._rowRoutines.Add(base.StartCoroutine(this.RowsRoutine(container)));
		}

		// Token: 0x06007F7D RID: 32637 RVA: 0x0034167D File Offset: 0x0033F87D
		public void CloseAndDeactivate()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.StopAll();
			this._panelRoutine = base.StartCoroutine(this.CloseRoutine());
		}

		// Token: 0x06007F7E RID: 32638 RVA: 0x003416B1 File Offset: 0x0033F8B1
		private IEnumerator OpenRoutine()
		{
			foreach (RectTransform rectTransform in this.EnumerateContainers())
			{
				this.PrepareRows(rectTransform);
			}
			if (this.titleBlock != null)
			{
				this.RememberRestPosition(this.titleBlock);
			}
			float elapsed = 0f;
			while (elapsed < this.panelFade)
			{
				elapsed += Time.unscaledDeltaTime;
				this._group.alpha = Mathf.Clamp01(elapsed / this.panelFade);
				yield return null;
			}
			this._group.alpha = 1f;
			this._group.interactable = true;
			if (this.titleBlock != null)
			{
				this._rowRoutines.Add(base.StartCoroutine(this.SlideRoutine(this.titleBlock, this._restPositions[this.titleBlock] + new Vector2(0f, 18f), this._restPositions[this.titleBlock], this.rowDuration, 0f)));
			}
			foreach (RectTransform rectTransform2 in this.EnumerateContainers())
			{
				this._rowRoutines.Add(base.StartCoroutine(this.RowsRoutine(rectTransform2)));
			}
			this._panelRoutine = null;
			yield break;
		}

		// Token: 0x06007F7F RID: 32639 RVA: 0x003416C0 File Offset: 0x0033F8C0
		private IEnumerator CloseRoutine()
		{
			this._group.interactable = false;
			float startAlpha = this._group.alpha;
			float elapsed = 0f;
			while (elapsed < this.closeFade)
			{
				elapsed += Time.unscaledDeltaTime;
				this._group.alpha = Mathf.Lerp(startAlpha, 0f, Mathf.Clamp01(elapsed / this.closeFade));
				yield return null;
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06007F80 RID: 32640 RVA: 0x003416CF File Offset: 0x0033F8CF
		private IEnumerator RowsRoutine(RectTransform container)
		{
			List<RectTransform> list = this.CollectRows(container);
			for (int i = 0; i < list.Count; i++)
			{
				RectTransform rectTransform = list[i];
				Vector2 vector = this._restPositions[rectTransform];
				rectTransform.anchoredPosition = vector + new Vector2(-this.rowSlideDistance, 0f);
				this._rowRoutines.Add(base.StartCoroutine(this.SlideRoutine(rectTransform, vector + new Vector2(-this.rowSlideDistance, 0f), vector, this.rowDuration, (float)i * this.rowStagger)));
			}
			yield break;
		}

		// Token: 0x06007F81 RID: 32641 RVA: 0x003416E5 File Offset: 0x0033F8E5
		private IEnumerator SlideRoutine(RectTransform target, Vector2 from, Vector2 to, float duration, float delay = 0f)
		{
			if (target == null)
			{
				yield break;
			}
			Transform originalParent = target.parent;
			if (delay > 0f)
			{
				float wait = 0f;
				while (wait < delay)
				{
					if (target == null || target.parent != originalParent)
					{
						yield break;
					}
					wait += Time.unscaledDeltaTime;
					yield return null;
				}
			}
			float elapsed = 0f;
			while (elapsed < duration)
			{
				if (target == null || target.parent != originalParent)
				{
					yield break;
				}
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				float num2 = 1f - Mathf.Pow(1f - num, 3f) + Mathf.Sin(num * 3.1415927f) * 0.06f;
				target.anchoredPosition = Vector2.LerpUnclamped(from, to, num2);
				yield return null;
			}
			if (target == null || target.parent != originalParent)
			{
				yield break;
			}
			target.anchoredPosition = to;
			yield break;
		}

		// Token: 0x06007F82 RID: 32642 RVA: 0x00341714 File Offset: 0x0033F914
		private void PrepareRows(RectTransform container)
		{
			foreach (RectTransform rectTransform in this.CollectRows(container))
			{
				rectTransform.anchoredPosition = this._restPositions[rectTransform] + new Vector2(-this.rowSlideDistance, 0f);
			}
		}

		// Token: 0x06007F83 RID: 32643 RVA: 0x0034178C File Offset: 0x0033F98C
		private List<RectTransform> CollectRows(RectTransform container)
		{
			List<RectTransform> list = new List<RectTransform>();
			if (container == null)
			{
				return list;
			}
			for (int i = 0; i < container.childCount; i++)
			{
				RectTransform rectTransform = container.GetChild(i) as RectTransform;
				if (rectTransform != null && rectTransform.gameObject.activeSelf)
				{
					this.RememberRestPosition(rectTransform);
					list.Add(rectTransform);
				}
			}
			return list;
		}

		// Token: 0x06007F84 RID: 32644 RVA: 0x003417E8 File Offset: 0x0033F9E8
		private void RememberRestPosition(RectTransform row)
		{
			Transform transform;
			if (!this._restParents.TryGetValue(row, out transform) || transform != row.parent)
			{
				this._restParents[row] = row.parent;
				this._restPositions[row] = row.anchoredPosition;
			}
		}

		// Token: 0x06007F85 RID: 32645 RVA: 0x00341837 File Offset: 0x0033FA37
		private IEnumerable<RectTransform> EnumerateContainers()
		{
			if (this.rowContainers == null)
			{
				yield break;
			}
			foreach (RectTransform rectTransform in this.rowContainers)
			{
				if (rectTransform != null)
				{
					yield return rectTransform;
				}
			}
			RectTransform[] array = null;
			yield break;
		}

		// Token: 0x06007F86 RID: 32646 RVA: 0x00341848 File Offset: 0x0033FA48
		private void RestoreRows()
		{
			foreach (KeyValuePair<RectTransform, Vector2> keyValuePair in this._restPositions)
			{
				Transform transform;
				if (keyValuePair.Key != null && this._restParents.TryGetValue(keyValuePair.Key, out transform) && transform == keyValuePair.Key.parent)
				{
					keyValuePair.Key.anchoredPosition = keyValuePair.Value;
				}
			}
		}

		// Token: 0x06007F87 RID: 32647 RVA: 0x003418E0 File Offset: 0x0033FAE0
		private void StopAll()
		{
			if (this._panelRoutine != null)
			{
				base.StopCoroutine(this._panelRoutine);
				this._panelRoutine = null;
			}
			foreach (Coroutine coroutine in this._rowRoutines)
			{
				if (coroutine != null)
				{
					base.StopCoroutine(coroutine);
				}
			}
			this._rowRoutines.Clear();
		}

		// Token: 0x040082E8 RID: 33512
		[Tooltip("Contenitori le cui righe figlie attive vengono animate all'apertura.")]
		[SerializeField]
		private RectTransform[] rowContainers;

		// Token: 0x040082E9 RID: 33513
		[Tooltip("Blocco titolo (slide verticale leggero all'apertura). Opzionale.")]
		[SerializeField]
		private RectTransform titleBlock;

		// Token: 0x040082EA RID: 33514
		[SerializeField]
		[Min(0.01f)]
		private float panelFade = 0.2f;

		// Token: 0x040082EB RID: 33515
		[SerializeField]
		[Min(0.01f)]
		private float rowDuration = 0.4f;

		// Token: 0x040082EC RID: 33516
		[SerializeField]
		[Min(0f)]
		private float rowStagger = 0.07f;

		// Token: 0x040082ED RID: 33517
		[SerializeField]
		[Min(0f)]
		private float rowSlideDistance = 62f;

		// Token: 0x040082EE RID: 33518
		[SerializeField]
		[Min(0.01f)]
		private float closeFade = 0.14f;

		// Token: 0x040082EF RID: 33519
		private CanvasGroup _group;

		// Token: 0x040082F0 RID: 33520
		private Coroutine _panelRoutine;

		// Token: 0x040082F1 RID: 33521
		private readonly List<Coroutine> _rowRoutines = new List<Coroutine>();

		// Token: 0x040082F2 RID: 33522
		private readonly Dictionary<RectTransform, Vector2> _restPositions = new Dictionary<RectTransform, Vector2>();

		// Token: 0x040082F3 RID: 33523
		private readonly Dictionary<RectTransform, Transform> _restParents = new Dictionary<RectTransform, Transform>();
	}
}
