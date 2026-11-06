using System;
using Features.Detection;
using Features.Localization;
using Features.PhoneCompanion;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;

namespace Features.RapidActions
{
	// Token: 0x02000D88 RID: 3464
	public static class RadialControllerNavigation
	{
		// Token: 0x060065F1 RID: 26097 RVA: 0x0028B288 File Offset: 0x00289488
		public static void TakeManualOwnership(UltimateRadialMenu wheel)
		{
			if (wheel == null)
			{
				return;
			}
			UltimateRadialMenuInputManager inputManager = wheel.inputManager;
			UltimateRadialMenuInputManager ultimateRadialMenuInputManager = wheel.GetComponent<UltimateRadialMenuInputManager>();
			if (ultimateRadialMenuInputManager == null)
			{
				ultimateRadialMenuInputManager = wheel.gameObject.AddComponent<UltimateRadialMenuInputManager>();
			}
			ultimateRadialMenuInputManager.enableMenuSetting = UltimateRadialMenuInputManager.EnableMenuSetting.Manual;
			ultimateRadialMenuInputManager.enabled = false;
			if (inputManager != null && inputManager != ultimateRadialMenuInputManager && inputManager.UltimateRadialMenuInformations != null)
			{
				inputManager.UltimateRadialMenuInformations.RemoveAll((UltimateRadialMenuInputManager.UltimateRadialMenuInformation info) => info != null && info.radialMenu == wheel);
			}
			ultimateRadialMenuInputManager.AddRadialMenuToList(wheel, ref wheel.inputManager);
		}

		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x060065F2 RID: 26098 RVA: 0x0028B33A File Offset: 0x0028953A
		public static bool AnyWheelOpen
		{
			get
			{
				return RapidActionsHUDController.AnyWheelOpen || SelfieRadialMenuController.AnyWheelOpen || EmoteWheelUI.AnyWheelOpen;
			}
		}

		// Token: 0x060065F3 RID: 26099 RVA: 0x0028B354 File Offset: 0x00289554
		public static void ShowControllerHint(UltimateRadialMenu wheel, string pageButtons = null)
		{
			if (wheel == null)
			{
				return;
			}
			Transform transform = wheel.transform.Find("Controller controls");
			if (transform == null && ControllerInput.IsActive && !string.IsNullOrEmpty(pageButtons))
			{
				GameObject gameObject = new GameObject("Controller controls", new Type[]
				{
					typeof(RectTransform),
					typeof(TextMeshProUGUI)
				});
				gameObject.layer = wheel.gameObject.layer;
				gameObject.transform.SetParent(wheel.transform, false);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				Vector2 vector = new Vector2(0.5f, 0f);
				component.anchorMax = vector;
				component.anchorMin = vector;
				component.pivot = new Vector2(0.5f, 1f);
				component.anchoredPosition = new Vector2(0f, -90f);
				component.sizeDelta = new Vector2(620f, 64f);
				TextMeshProUGUI component2 = gameObject.GetComponent<TextMeshProUGUI>();
				component2.fontSize = 18f;
				component2.alignment = TextAlignmentOptions.Center;
				component2.raycastTarget = false;
				transform = gameObject.transform;
			}
			if (transform == null)
			{
				return;
			}
			transform.gameObject.SetActive(ControllerInput.IsActive && wheel.IsEnabled && !string.IsNullOrEmpty(pageButtons));
			if (!transform.gameObject.activeSelf)
			{
				return;
			}
			string text = ControllerGlyphs.Format(pageButtons) + ": " + RadialControllerNavigation.Hint("Change page");
			transform.GetComponent<TMP_Text>().text = text;
		}

		// Token: 0x060065F4 RID: 26100 RVA: 0x0028B4DA File Offset: 0x002896DA
		private static string Hint(string text)
		{
			return GameLocalization.GetSource("gameplay_prompts", text);
		}

		// Token: 0x060065F5 RID: 26101 RVA: 0x0028B4E8 File Offset: 0x002896E8
		internal static Vector2 SnapDirection(UltimateRadialMenu wheel, Vector2 direction)
		{
			float num = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
			UltimateRadialMenu.UltimateRadialButton ultimateRadialButton = null;
			float num2 = float.MaxValue;
			foreach (UltimateRadialMenu.UltimateRadialButton ultimateRadialButton2 in wheel.UltimateRadialButtonList)
			{
				if (ultimateRadialButton2 != null && !ultimateRadialButton2.buttonDisabled && ultimateRadialButton2.Registered)
				{
					float num3 = Mathf.Abs(Mathf.DeltaAngle(num, ultimateRadialButton2.angle));
					if (num3 < num2)
					{
						num2 = num3;
						ultimateRadialButton = ultimateRadialButton2;
					}
				}
			}
			if (ultimateRadialButton == null)
			{
				return direction.normalized;
			}
			float num4 = ultimateRadialButton.angle * 0.017453292f;
			return new Vector2(Mathf.Cos(num4), Mathf.Sin(num4));
		}

		// Token: 0x060065F6 RID: 26102 RVA: 0x0028B5B4 File Offset: 0x002897B4
		public static void ProcessPointer(UltimateRadialMenu wheel, Vector2 screenPoint)
		{
			if (wheel == null || !wheel.IsEnabled || wheel.BaseTransform == null)
			{
				return;
			}
			Vector2 vector;
			if (!UIScreenCoordinates.TryScreenPointToLocalPoint(wheel.BaseTransform, wheel.ParentCanvas, screenPoint, out vector))
			{
				wheel.ProcessInput(Vector2.zero, -1f, false, false);
				return;
			}
			Vector2 vector2 = (wheel.IsWorldSpaceRadialMenu ? vector.normalized : wheel.BaseTransform.TransformVector(vector.normalized));
			wheel.ProcessInput(vector2, vector.magnitude, false, false);
		}

		// Token: 0x060065F7 RID: 26103 RVA: 0x0028B648 File Offset: 0x00289848
		public static bool Process(UltimateRadialMenu wheel, bool retainSelection = false)
		{
			if (!ControllerInput.IsActive || wheel == null)
			{
				return false;
			}
			Vector2 vector = ControllerInput.RadialDirection;
			float num = -1f;
			if (vector.sqrMagnitude >= 0.0625f)
			{
				num = Mathf.Lerp(wheel.CalculatedMinRange, wheel.CalculatedMaxRange, 0.5f);
				vector = RadialControllerNavigation.SnapDirection(wheel, vector);
			}
			else
			{
				if (retainSelection && wheel.IsEnabled && wheel.CurrentButtonIndex >= 0)
				{
					return true;
				}
				vector = Vector2.zero;
			}
			wheel.ProcessInput(vector, num, false, false);
			return true;
		}
	}
}
