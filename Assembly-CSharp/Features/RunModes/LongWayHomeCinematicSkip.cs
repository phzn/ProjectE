using System;
using System.Collections.Generic;
using System.Text;
using Features.Detection;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CB2 RID: 3250
	internal sealed class LongWayHomeCinematicSkip : IDisposable
	{
		// Token: 0x06005E57 RID: 24151 RVA: 0x0025510E File Offset: 0x0025330E
		internal LongWayHomeCinematicSkip(Object owner)
		{
			this._owner = owner;
		}

		// Token: 0x06005E58 RID: 24152 RVA: 0x00255128 File Offset: 0x00253328
		internal static bool TryConsume(Object owner)
		{
			if (Time.timeScale <= 0f || !EscapeInputCoordinator.TryConsume(owner))
			{
				return false;
			}
			ControllerInput.ConsumeUIInput();
			return true;
		}

		// Token: 0x06005E59 RID: 24153 RVA: 0x00255146 File Offset: 0x00253346
		internal static void AddAction(InteractionConfigSO config)
		{
			config.usableActions.Add(new UsableAction
			{
				keyDisplayLabel = "Escape",
				actionName = "Skip cinematic"
			});
		}

		// Token: 0x06005E5A RID: 24154 RVA: 0x0025516E File Offset: 0x0025336E
		internal void Show()
		{
			this.ShowWith(null, true);
		}

		// Token: 0x06005E5B RID: 24155 RVA: 0x00255178 File Offset: 0x00253378
		internal void ShowWith(IReadOnlyList<LongWayHomeCinematicSkip.Hint> extra, bool skip)
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			this._signature.Clear().Append(skip ? '1' : '0');
			if (extra != null)
			{
				foreach (LongWayHomeCinematicSkip.Hint hint in extra)
				{
					this._signature.Append('|').Append(hint.Key).Append(':')
						.Append(hint.Action);
				}
			}
			string text = this._signature.ToString();
			if (this._hud == instance && this._config != null)
			{
				if (text == this._shownSignature)
				{
					return;
				}
				this.FillActions(extra, skip);
				this._shownSignature = text;
				instance.ShowSupplemental(this._owner, this._config);
				return;
			}
			else
			{
				this.Dispose();
				if (instance == null)
				{
					return;
				}
				this._hud = instance;
				this._config = ScriptableObject.CreateInstance<InteractionConfigSO>();
				this._config.name = "LongWayHomeSkipHints_Runtime";
				this._config.hideFlags = HideFlags.HideAndDontSave;
				this.FillActions(extra, skip);
				this._shownSignature = text;
				this._canvas = instance.GetComponent<Canvas>();
				this._createdCanvas = this._canvas == null;
				if (this._createdCanvas)
				{
					this._canvas = instance.gameObject.AddComponent<Canvas>();
					this._canvas.hideFlags = HideFlags.DontSave;
				}
				this._canvasEnabled = this._canvas.enabled;
				this._overrideSorting = this._canvas.overrideSorting;
				this._sortingOrder = this._canvas.sortingOrder;
				this._group = instance.GetComponent<CanvasGroup>();
				this._createdGroup = this._group == null;
				if (this._createdGroup)
				{
					this._group = instance.gameObject.AddComponent<CanvasGroup>();
				}
				this._alpha = this._group.alpha;
				this._hudWasHidden = HUDManager.Instance != null && HUDManager.Instance.IsHidden;
				this._ignoreParentGroups = this._group.ignoreParentGroups;
				this._group.alpha = 1f;
				this._group.ignoreParentGroups = true;
				instance.ShowSupplemental(this._owner, this._config);
				this._canvas.enabled = true;
				this._canvas.overrideSorting = true;
				this._canvas.sortingOrder = 30;
				GameplayHudVisibility.RegisterCanvas(this._canvas);
				return;
			}
		}

		// Token: 0x06005E5C RID: 24156 RVA: 0x002553FC File Offset: 0x002535FC
		private void FillActions(IReadOnlyList<LongWayHomeCinematicSkip.Hint> extra, bool skip)
		{
			this._config.usableActions.Clear();
			if (skip)
			{
				LongWayHomeCinematicSkip.AddAction(this._config);
			}
			if (extra == null)
			{
				return;
			}
			foreach (LongWayHomeCinematicSkip.Hint hint in extra)
			{
				this._config.usableActions.Add(new UsableAction
				{
					keyDisplayLabel = hint.Key,
					actionName = hint.Action
				});
				if (!string.IsNullOrEmpty(hint.ControllerKey))
				{
					ControllerContextBindings.SetLabel(this._config, hint.Key, hint.ControllerKey, false);
				}
			}
		}

		// Token: 0x06005E5D RID: 24157 RVA: 0x002554B4 File Offset: 0x002536B4
		public void Dispose()
		{
			this._shownSignature = null;
			if (this._hud != null)
			{
				this._hud.HideSupplemental(this._owner);
			}
			this._hud = null;
			if (this._canvas != null)
			{
				if (this._createdCanvas)
				{
					this._canvas.enabled = false;
					LongWayHomeCinematicSkip.DestroyOwned(this._canvas);
				}
				else
				{
					this._canvas.overrideSorting = this._overrideSorting;
					this._canvas.sortingOrder = this._sortingOrder;
					this._canvas.enabled = this._canvasEnabled;
				}
			}
			this._canvas = null;
			this._createdCanvas = false;
			if (this._group != null)
			{
				if (this._createdGroup)
				{
					LongWayHomeCinematicSkip.DestroyOwned(this._group);
				}
				else
				{
					if (!this._hudWasHidden || !(HUDManager.Instance != null) || HUDManager.Instance.IsHidden)
					{
						this._group.alpha = this._alpha;
					}
					this._group.ignoreParentGroups = this._ignoreParentGroups;
				}
			}
			this._group = null;
			this._createdGroup = false;
			if (this._config != null)
			{
				LongWayHomeCinematicSkip.DestroyOwned(this._config);
			}
			this._config = null;
		}

		// Token: 0x06005E5E RID: 24158 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void DestroyOwned(Object target)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(target);
				return;
			}
			Object.DestroyImmediate(target);
		}

		// Token: 0x04005F58 RID: 24408
		internal const string KeyLabel = "Escape";

		// Token: 0x04005F59 RID: 24409
		internal const string ActionLabel = "Skip cinematic";

		// Token: 0x04005F5A RID: 24410
		private readonly Object _owner;

		// Token: 0x04005F5B RID: 24411
		private readonly StringBuilder _signature = new StringBuilder();

		// Token: 0x04005F5C RID: 24412
		private string _shownSignature;

		// Token: 0x04005F5D RID: 24413
		private InteractionHUDController _hud;

		// Token: 0x04005F5E RID: 24414
		private InteractionConfigSO _config;

		// Token: 0x04005F5F RID: 24415
		private Canvas _canvas;

		// Token: 0x04005F60 RID: 24416
		private bool _createdCanvas;

		// Token: 0x04005F61 RID: 24417
		private bool _canvasEnabled;

		// Token: 0x04005F62 RID: 24418
		private bool _overrideSorting;

		// Token: 0x04005F63 RID: 24419
		private int _sortingOrder;

		// Token: 0x04005F64 RID: 24420
		private CanvasGroup _group;

		// Token: 0x04005F65 RID: 24421
		private bool _createdGroup;

		// Token: 0x04005F66 RID: 24422
		private bool _ignoreParentGroups;

		// Token: 0x04005F67 RID: 24423
		private float _alpha;

		// Token: 0x04005F68 RID: 24424
		private bool _hudWasHidden;

		// Token: 0x02000CB3 RID: 3251
		internal readonly struct Hint
		{
			// Token: 0x06005E5F RID: 24159 RVA: 0x002555F8 File Offset: 0x002537F8
			public Hint(string key, string action, string controllerKey = null)
			{
				this.Key = key;
				this.Action = action;
				this.ControllerKey = controllerKey;
			}

			// Token: 0x04005F69 RID: 24425
			public readonly string Key;

			// Token: 0x04005F6A RID: 24426
			public readonly string Action;

			// Token: 0x04005F6B RID: 24427
			public readonly string ControllerKey;
		}
	}
}
