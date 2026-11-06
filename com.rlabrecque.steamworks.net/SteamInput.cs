using System;

namespace Steamworks
{
	// Token: 0x02000013 RID: 19
	public static class SteamInput
	{
		// Token: 0x06000252 RID: 594 RVA: 0x000071B5 File Offset: 0x000053B5
		public static bool Init(bool bExplicitlyCallRunFrame)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_Init(CSteamAPIContext.GetSteamInput(), bExplicitlyCallRunFrame);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x000071C7 File Offset: 0x000053C7
		public static bool Shutdown()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_Shutdown(CSteamAPIContext.GetSteamInput());
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000071D8 File Offset: 0x000053D8
		public static bool SetInputActionManifestFilePath(string pchInputActionManifestAbsolutePath)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchInputActionManifestAbsolutePath))
			{
				flag = NativeMethods.ISteamInput_SetInputActionManifestFilePath(CSteamAPIContext.GetSteamInput(), utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000721C File Offset: 0x0000541C
		public static void RunFrame(bool bReservedValue = true)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_RunFrame(CSteamAPIContext.GetSteamInput(), bReservedValue);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000722E File Offset: 0x0000542E
		public static bool BWaitForData(bool bWaitForever, uint unTimeout)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_BWaitForData(CSteamAPIContext.GetSteamInput(), bWaitForever, unTimeout);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00007241 File Offset: 0x00005441
		public static bool BNewDataAvailable()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_BNewDataAvailable(CSteamAPIContext.GetSteamInput());
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00007252 File Offset: 0x00005452
		public static int GetConnectedControllers(InputHandle_t[] handlesOut)
		{
			InteropHelp.TestIfAvailableClient();
			if (handlesOut != null && handlesOut.Length != 16)
			{
				throw new ArgumentException("handlesOut must be the same size as Constants.STEAM_INPUT_MAX_COUNT!");
			}
			return NativeMethods.ISteamInput_GetConnectedControllers(CSteamAPIContext.GetSteamInput(), handlesOut);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00007279 File Offset: 0x00005479
		public static void EnableDeviceCallbacks()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_EnableDeviceCallbacks(CSteamAPIContext.GetSteamInput());
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000728A File Offset: 0x0000548A
		public static void EnableActionEventCallbacks(SteamInputActionEventCallbackPointer pCallback)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_EnableActionEventCallbacks(CSteamAPIContext.GetSteamInput(), pCallback);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000729C File Offset: 0x0000549C
		public static InputActionSetHandle_t GetActionSetHandle(string pszActionSetName)
		{
			InteropHelp.TestIfAvailableClient();
			InputActionSetHandle_t inputActionSetHandle_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszActionSetName))
			{
				inputActionSetHandle_t = (InputActionSetHandle_t)NativeMethods.ISteamInput_GetActionSetHandle(CSteamAPIContext.GetSteamInput(), utf8StringHandle);
			}
			return inputActionSetHandle_t;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000072E4 File Offset: 0x000054E4
		public static void ActivateActionSet(InputHandle_t inputHandle, InputActionSetHandle_t actionSetHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_ActivateActionSet(CSteamAPIContext.GetSteamInput(), inputHandle, actionSetHandle);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000072F7 File Offset: 0x000054F7
		public static InputActionSetHandle_t GetCurrentActionSet(InputHandle_t inputHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return (InputActionSetHandle_t)NativeMethods.ISteamInput_GetCurrentActionSet(CSteamAPIContext.GetSteamInput(), inputHandle);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000730E File Offset: 0x0000550E
		public static void ActivateActionSetLayer(InputHandle_t inputHandle, InputActionSetHandle_t actionSetLayerHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_ActivateActionSetLayer(CSteamAPIContext.GetSteamInput(), inputHandle, actionSetLayerHandle);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00007321 File Offset: 0x00005521
		public static void DeactivateActionSetLayer(InputHandle_t inputHandle, InputActionSetHandle_t actionSetLayerHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_DeactivateActionSetLayer(CSteamAPIContext.GetSteamInput(), inputHandle, actionSetLayerHandle);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00007334 File Offset: 0x00005534
		public static void DeactivateAllActionSetLayers(InputHandle_t inputHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_DeactivateAllActionSetLayers(CSteamAPIContext.GetSteamInput(), inputHandle);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00007346 File Offset: 0x00005546
		public static int GetActiveActionSetLayers(InputHandle_t inputHandle, InputActionSetHandle_t[] handlesOut)
		{
			InteropHelp.TestIfAvailableClient();
			if (handlesOut != null && handlesOut.Length != 16)
			{
				throw new ArgumentException("handlesOut must be the same size as Constants.STEAM_INPUT_MAX_ACTIVE_LAYERS!");
			}
			return NativeMethods.ISteamInput_GetActiveActionSetLayers(CSteamAPIContext.GetSteamInput(), inputHandle, handlesOut);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00007370 File Offset: 0x00005570
		public static InputDigitalActionHandle_t GetDigitalActionHandle(string pszActionName)
		{
			InteropHelp.TestIfAvailableClient();
			InputDigitalActionHandle_t inputDigitalActionHandle_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszActionName))
			{
				inputDigitalActionHandle_t = (InputDigitalActionHandle_t)NativeMethods.ISteamInput_GetDigitalActionHandle(CSteamAPIContext.GetSteamInput(), utf8StringHandle);
			}
			return inputDigitalActionHandle_t;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x000073B8 File Offset: 0x000055B8
		public static InputDigitalActionData_t GetDigitalActionData(InputHandle_t inputHandle, InputDigitalActionHandle_t digitalActionHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_GetDigitalActionData(CSteamAPIContext.GetSteamInput(), inputHandle, digitalActionHandle);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x000073CB File Offset: 0x000055CB
		public static int GetDigitalActionOrigins(InputHandle_t inputHandle, InputActionSetHandle_t actionSetHandle, InputDigitalActionHandle_t digitalActionHandle, EInputActionOrigin[] originsOut)
		{
			InteropHelp.TestIfAvailableClient();
			if (originsOut != null && originsOut.Length != 8)
			{
				throw new ArgumentException("originsOut must be the same size as Constants.STEAM_INPUT_MAX_ORIGINS!");
			}
			return NativeMethods.ISteamInput_GetDigitalActionOrigins(CSteamAPIContext.GetSteamInput(), inputHandle, actionSetHandle, digitalActionHandle, originsOut);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x000073F4 File Offset: 0x000055F4
		public static string GetStringForDigitalActionName(InputDigitalActionHandle_t eActionHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamInput_GetStringForDigitalActionName(CSteamAPIContext.GetSteamInput(), eActionHandle));
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000740C File Offset: 0x0000560C
		public static InputAnalogActionHandle_t GetAnalogActionHandle(string pszActionName)
		{
			InteropHelp.TestIfAvailableClient();
			InputAnalogActionHandle_t inputAnalogActionHandle_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszActionName))
			{
				inputAnalogActionHandle_t = (InputAnalogActionHandle_t)NativeMethods.ISteamInput_GetAnalogActionHandle(CSteamAPIContext.GetSteamInput(), utf8StringHandle);
			}
			return inputAnalogActionHandle_t;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00007454 File Offset: 0x00005654
		public static InputAnalogActionData_t GetAnalogActionData(InputHandle_t inputHandle, InputAnalogActionHandle_t analogActionHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_GetAnalogActionData(CSteamAPIContext.GetSteamInput(), inputHandle, analogActionHandle);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00007467 File Offset: 0x00005667
		public static int GetAnalogActionOrigins(InputHandle_t inputHandle, InputActionSetHandle_t actionSetHandle, InputAnalogActionHandle_t analogActionHandle, EInputActionOrigin[] originsOut)
		{
			InteropHelp.TestIfAvailableClient();
			if (originsOut != null && originsOut.Length != 8)
			{
				throw new ArgumentException("originsOut must be the same size as Constants.STEAM_INPUT_MAX_ORIGINS!");
			}
			return NativeMethods.ISteamInput_GetAnalogActionOrigins(CSteamAPIContext.GetSteamInput(), inputHandle, actionSetHandle, analogActionHandle, originsOut);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00007490 File Offset: 0x00005690
		public static string GetGlyphPNGForActionOrigin(EInputActionOrigin eOrigin, ESteamInputGlyphSize eSize, uint unFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamInput_GetGlyphPNGForActionOrigin(CSteamAPIContext.GetSteamInput(), eOrigin, eSize, unFlags));
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000074A9 File Offset: 0x000056A9
		public static string GetGlyphSVGForActionOrigin(EInputActionOrigin eOrigin, uint unFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamInput_GetGlyphSVGForActionOrigin(CSteamAPIContext.GetSteamInput(), eOrigin, unFlags));
		}

		// Token: 0x0600026B RID: 619 RVA: 0x000074C1 File Offset: 0x000056C1
		public static string GetGlyphForActionOrigin_Legacy(EInputActionOrigin eOrigin)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamInput_GetGlyphForActionOrigin_Legacy(CSteamAPIContext.GetSteamInput(), eOrigin));
		}

		// Token: 0x0600026C RID: 620 RVA: 0x000074D8 File Offset: 0x000056D8
		public static string GetStringForActionOrigin(EInputActionOrigin eOrigin)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamInput_GetStringForActionOrigin(CSteamAPIContext.GetSteamInput(), eOrigin));
		}

		// Token: 0x0600026D RID: 621 RVA: 0x000074EF File Offset: 0x000056EF
		public static string GetStringForAnalogActionName(InputAnalogActionHandle_t eActionHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamInput_GetStringForAnalogActionName(CSteamAPIContext.GetSteamInput(), eActionHandle));
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00007506 File Offset: 0x00005706
		public static void StopAnalogActionMomentum(InputHandle_t inputHandle, InputAnalogActionHandle_t eAction)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_StopAnalogActionMomentum(CSteamAPIContext.GetSteamInput(), inputHandle, eAction);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00007519 File Offset: 0x00005719
		public static InputMotionData_t GetMotionData(InputHandle_t inputHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_GetMotionData(CSteamAPIContext.GetSteamInput(), inputHandle);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000752B File Offset: 0x0000572B
		public static void TriggerVibration(InputHandle_t inputHandle, ushort usLeftSpeed, ushort usRightSpeed)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_TriggerVibration(CSteamAPIContext.GetSteamInput(), inputHandle, usLeftSpeed, usRightSpeed);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000753F File Offset: 0x0000573F
		public static void TriggerVibrationExtended(InputHandle_t inputHandle, ushort usLeftSpeed, ushort usRightSpeed, ushort usLeftTriggerSpeed, ushort usRightTriggerSpeed)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_TriggerVibrationExtended(CSteamAPIContext.GetSteamInput(), inputHandle, usLeftSpeed, usRightSpeed, usLeftTriggerSpeed, usRightTriggerSpeed);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00007556 File Offset: 0x00005756
		public static void TriggerSimpleHapticEvent(InputHandle_t inputHandle, EControllerHapticLocation eHapticLocation, byte nIntensity, char nGainDB, byte nOtherIntensity, char nOtherGainDB)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_TriggerSimpleHapticEvent(CSteamAPIContext.GetSteamInput(), inputHandle, eHapticLocation, nIntensity, nGainDB, nOtherIntensity, nOtherGainDB);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000756F File Offset: 0x0000576F
		public static void SetLEDColor(InputHandle_t inputHandle, byte nColorR, byte nColorG, byte nColorB, uint nFlags)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_SetLEDColor(CSteamAPIContext.GetSteamInput(), inputHandle, nColorR, nColorG, nColorB, nFlags);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00007586 File Offset: 0x00005786
		public static void Legacy_TriggerHapticPulse(InputHandle_t inputHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_Legacy_TriggerHapticPulse(CSteamAPIContext.GetSteamInput(), inputHandle, eTargetPad, usDurationMicroSec);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000759A File Offset: 0x0000579A
		public static void Legacy_TriggerRepeatedHapticPulse(InputHandle_t inputHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec, ushort usOffMicroSec, ushort unRepeat, uint nFlags)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_Legacy_TriggerRepeatedHapticPulse(CSteamAPIContext.GetSteamInput(), inputHandle, eTargetPad, usDurationMicroSec, usOffMicroSec, unRepeat, nFlags);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000075B3 File Offset: 0x000057B3
		public static bool ShowBindingPanel(InputHandle_t inputHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_ShowBindingPanel(CSteamAPIContext.GetSteamInput(), inputHandle);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000075C5 File Offset: 0x000057C5
		public static ESteamInputType GetInputTypeForHandle(InputHandle_t inputHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_GetInputTypeForHandle(CSteamAPIContext.GetSteamInput(), inputHandle);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000075D7 File Offset: 0x000057D7
		public static InputHandle_t GetControllerForGamepadIndex(int nIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (InputHandle_t)NativeMethods.ISteamInput_GetControllerForGamepadIndex(CSteamAPIContext.GetSteamInput(), nIndex);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x000075EE File Offset: 0x000057EE
		public static int GetGamepadIndexForController(InputHandle_t ulinputHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_GetGamepadIndexForController(CSteamAPIContext.GetSteamInput(), ulinputHandle);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00007600 File Offset: 0x00005800
		public static string GetStringForXboxOrigin(EXboxOrigin eOrigin)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamInput_GetStringForXboxOrigin(CSteamAPIContext.GetSteamInput(), eOrigin));
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00007617 File Offset: 0x00005817
		public static string GetGlyphForXboxOrigin(EXboxOrigin eOrigin)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamInput_GetGlyphForXboxOrigin(CSteamAPIContext.GetSteamInput(), eOrigin));
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000762E File Offset: 0x0000582E
		public static EInputActionOrigin GetActionOriginFromXboxOrigin(InputHandle_t inputHandle, EXboxOrigin eOrigin)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_GetActionOriginFromXboxOrigin(CSteamAPIContext.GetSteamInput(), inputHandle, eOrigin);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00007641 File Offset: 0x00005841
		public static EInputActionOrigin TranslateActionOrigin(ESteamInputType eDestinationInputType, EInputActionOrigin eSourceOrigin)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_TranslateActionOrigin(CSteamAPIContext.GetSteamInput(), eDestinationInputType, eSourceOrigin);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00007654 File Offset: 0x00005854
		public static bool GetDeviceBindingRevision(InputHandle_t inputHandle, out int pMajor, out int pMinor)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_GetDeviceBindingRevision(CSteamAPIContext.GetSteamInput(), inputHandle, out pMajor, out pMinor);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00007668 File Offset: 0x00005868
		public static uint GetRemotePlaySessionID(InputHandle_t inputHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_GetRemotePlaySessionID(CSteamAPIContext.GetSteamInput(), inputHandle);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000767A File Offset: 0x0000587A
		public static ushort GetSessionInputConfigurationSettings()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInput_GetSessionInputConfigurationSettings(CSteamAPIContext.GetSteamInput());
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000768B File Offset: 0x0000588B
		public static void SetDualSenseTriggerEffect(InputHandle_t inputHandle, IntPtr pParam)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInput_SetDualSenseTriggerEffect(CSteamAPIContext.GetSteamInput(), inputHandle, pParam);
		}
	}
}
