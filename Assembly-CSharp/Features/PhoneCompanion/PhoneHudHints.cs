using System;
using System.Collections.Generic;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E46 RID: 3654
	public static class PhoneHudHints
	{
		// Token: 0x06006D3C RID: 27964 RVA: 0x002BCE90 File Offset: 0x002BB090
		public static void ShowPhoneHeld(bool fullZoom, bool seated = false)
		{
			PhoneHudHints.Context context = (fullZoom ? PhoneHudHints.Context.PhoneHeldFullZoom : PhoneHudHints.Context.PhoneHeld);
			bool isControllerActive = ControllerInput.IsControllerActive;
			if (context == PhoneHudHints._current && PhoneHudHints._phoneHintsController == isControllerActive && PhoneHudHints._phoneHintsSeated == seated)
			{
				return;
			}
			PhoneHudHints._phoneHintsController = isControllerActive;
			PhoneHudHints._phoneHintsSeated = seated;
			PhoneHudHints._current = context;
			if (isControllerActive)
			{
				PhoneHudHints.Push(new List<UsableAction>
				{
					PhoneHudHints.Action("LS / D-pad", "Select"),
					PhoneHudHints.Action("A", "Open / confirm"),
					PhoneHudHints.Action("B", "Back / put away phone"),
					PhoneHudHints.Action("RS", "Scroll"),
					PhoneHudHints.Action("L3 + RS", "Adjust view"),
					PhoneHudHints.Action("LT + RS", "Zoom screen"),
					PhoneHudHints.Action("R3", "Center phone")
				}, false);
				return;
			}
			List<UsableAction> list = new List<UsableAction> { PhoneHudHints.Action("Shift + Scroll", "Zoom screen") };
			if (fullZoom)
			{
				list.Add(PhoneHudHints.Action("Middle Mouse", "Drag up / down"));
			}
			PhoneHudHints.Push(list, false);
		}

		// Token: 0x06006D3D RID: 27965 RVA: 0x002BCFB5 File Offset: 0x002BB1B5
		internal static bool IsPhoneConfig(InteractionConfigSO config)
		{
			return config != null && (config == PhoneHudHints._phoneConfig || config == PhoneHudHints._photoViewerConfig);
		}

		// Token: 0x06006D3E RID: 27966 RVA: 0x002BCFDC File Offset: 0x002BB1DC
		public static void ShowSelfie(bool poseIsDefault = true, bool videoMode = false, bool recording = false, bool tripodActive = false, bool tripodPlacementActive = false, bool bodyOwned = false, bool filterReserved = false, bool shutterReserved = false, bool exitBlocked = false, bool emoteSpeedActive = false, bool mediaModeReserved = false, bool tripodPlacementAllowed = true, bool bodyPosesAllowed = true, bool seated = false)
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			bool flag = instance != null && !instance.IsShowing;
			bool isControllerActive = ControllerInput.IsControllerActive;
			bool flag2 = PlayerActivityGate.Allows(PlayerActivity.RapidHUD);
			if (PhoneHudHints._current == PhoneHudHints.Context.Selfie && poseIsDefault == PhoneHudHints._lastSelfiePoseDefault && videoMode == PhoneHudHints._lastSelfieVideoMode && recording == PhoneHudHints._lastSelfieRecording && tripodActive == PhoneHudHints._lastSelfieTripodActive && tripodPlacementActive == PhoneHudHints._lastTripodPlacementActive && isControllerActive == PhoneHudHints._lastSelfieController && bodyOwned == PhoneHudHints._lastBodyOwned && filterReserved == PhoneHudHints._lastFilterReserved && shutterReserved == PhoneHudHints._lastShutterReserved && exitBlocked == PhoneHudHints._lastExitBlocked && flag2 == PhoneHudHints._lastEmotesAllowed && emoteSpeedActive == PhoneHudHints._lastEmoteSpeedActive && mediaModeReserved == PhoneHudHints._lastMediaModeReserved && tripodPlacementAllowed == PhoneHudHints._lastTripodPlacementAllowed && bodyPosesAllowed == PhoneHudHints._lastBodyPosesAllowed && seated == PhoneHudHints._lastSelfieSeated && !flag)
			{
				return;
			}
			PhoneHudHints._current = PhoneHudHints.Context.Selfie;
			PhoneHudHints._lastSelfiePoseDefault = poseIsDefault;
			PhoneHudHints._lastSelfieVideoMode = videoMode;
			PhoneHudHints._lastSelfieRecording = recording;
			PhoneHudHints._lastSelfieTripodActive = tripodActive;
			PhoneHudHints._lastTripodPlacementActive = tripodPlacementActive;
			PhoneHudHints._lastSelfieController = isControllerActive;
			PhoneHudHints._lastBodyOwned = bodyOwned;
			PhoneHudHints._lastFilterReserved = filterReserved;
			PhoneHudHints._lastShutterReserved = shutterReserved;
			PhoneHudHints._lastExitBlocked = exitBlocked;
			PhoneHudHints._lastEmotesAllowed = flag2;
			PhoneHudHints._lastEmoteSpeedActive = emoteSpeedActive;
			PhoneHudHints._lastMediaModeReserved = mediaModeReserved;
			PhoneHudHints._lastTripodPlacementAllowed = tripodPlacementAllowed;
			PhoneHudHints._lastBodyPosesAllowed = bodyPosesAllowed;
			PhoneHudHints._lastSelfieSeated = seated;
			if (tripodPlacementActive)
			{
				List<UsableAction> list = new List<UsableAction> { PhoneHudHints.Action(isControllerActive ? "RS" : "Mouse", "Aim placement") };
				if (tripodPlacementAllowed)
				{
					list.Add(PhoneHudHints.Action(isControllerActive ? "RB" : "Left Mouse", "Place tripod"));
				}
				list.Add(PhoneHudHints.Action(isControllerActive ? "B" : "Right Mouse / X", "Cancel"));
				PhoneHudHints.Push(list, false);
				return;
			}
			List<UsableAction> list2 = new List<UsableAction>();
			if (!isControllerActive || !emoteSpeedActive)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "LT + RS" : "Scroll", "Zoom"));
			}
			if (!bodyOwned || (seated && !tripodActive))
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "L3 + RS" : "Shift + Mouse", "Move head"));
			}
			if (poseIsDefault && !tripodActive)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "LT + D-pad Up / D-pad Down" : "Up / Down", "Raise / lower arm"));
			}
			else if (tripodActive)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "LT + D-pad Up / D-pad Down" : "Up / Down", "Tripod height"));
			}
			if (!bodyOwned)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "LT + D-pad Left / D-pad Right" : "Left / Right", "Rotate character"));
			}
			if (!recording && !mediaModeReserved)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "LT + RB" : "H", videoMode ? "Switch to photo" : "Switch to video"));
			}
			if (!filterReserved)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "RT" : "F", "Change filter"));
			}
			if (!shutterReserved)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "A" : "Space", videoMode ? (recording ? "Stop recording" : "Start recording") : "Take photo"));
			}
			if (recording)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "View" : "P", "Stop recording"));
			}
			else if (shutterReserved)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "View" : "P", videoMode ? "Start recording" : "Take photo"));
			}
			if (!tripodActive && tripodPlacementAllowed)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "D-pad Down" : "T", "Place tripod"));
			}
			if (!tripodActive || flag2)
			{
				list2.Add(tripodActive ? PhoneHudHints.Action(isControllerActive ? "LT + LB / LT + B" : "Q / Z", "Emotes (hold to change)") : (bodyPosesAllowed ? PhoneHudHints.Action(isControllerActive ? "D-pad Left / D-pad Right" : "1 / 2", "Pose / Face (tap or hold)") : PhoneHudHints.Action(isControllerActive ? "D-pad Right" : "2", "Face (tap or hold)")));
			}
			if (tripodActive)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "D-pad Down" : "6", "Face (tap or hold)"));
				list2.Add(PhoneHudHints.Action(isControllerActive ? "D-pad Left / D-pad Right" : "1 / 2 / 3", "Select / put away item"));
			}
			if (poseIsDefault && !tripodActive)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "LT + X" : "R", "Switch camera"));
			}
			if (!recording && !exitBlocked)
			{
				list2.Add(PhoneHudHints.Action(isControllerActive ? "B" : "X", "Exit"));
			}
			PhoneHudHints.Push(list2, tripodActive);
		}

		// Token: 0x06006D3F RID: 27967 RVA: 0x002BD464 File Offset: 0x002BB664
		public static void ShowPhotoViewer()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			if (PhoneHudHints._photoViewerConfig == null)
			{
				PhoneHudHints._photoViewerConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
				PhoneHudHints._photoViewerConfig.hideFlags = HideFlags.HideAndDontSave;
				PhoneHudHints._photoViewerConfig.objectName = "Photo viewer";
				PhoneHudHints._photoViewerConfig.usableActions = new List<UsableAction>
				{
					PhoneHudHints.Action("Scroll", "Zoom"),
					PhoneHudHints.Action("Drag", "Pan photo")
				};
			}
			bool isControllerActive = ControllerInput.IsControllerActive;
			if (PhoneHudHints._photoHintsController != isControllerActive)
			{
				PhoneHudHints._photoHintsController = isControllerActive;
				InteractionConfigSO photoViewerConfig = PhoneHudHints._photoViewerConfig;
				List<UsableAction> list2;
				if (!isControllerActive)
				{
					List<UsableAction> list = new List<UsableAction>();
					list.Add(PhoneHudHints.Action("Scroll", "Zoom"));
					list2 = list;
					list.Add(PhoneHudHints.Action("Drag", "Pan photo"));
				}
				else
				{
					List<UsableAction> list3 = new List<UsableAction>();
					list3.Add(PhoneHudHints.Action("LT + RS", "Zoom"));
					list3.Add(PhoneHudHints.Action("RS", "Pan photo"));
					list2 = list3;
					list3.Add(PhoneHudHints.Action("B", "Back"));
				}
				photoViewerConfig.usableActions = list2;
			}
			PhoneHudHints._current = PhoneHudHints.Context.PhotoViewer;
			ControllerContextBindings.MarkControllerLabels(PhoneHudHints._photoViewerConfig, isControllerActive);
			instance.Show(PhoneHudHints._photoViewerConfig);
		}

		// Token: 0x06006D40 RID: 27968 RVA: 0x002BD5A1 File Offset: 0x002BB7A1
		public static void HidePhotoViewer()
		{
			if (PhoneHudHints._current != PhoneHudHints.Context.PhotoViewer)
			{
				return;
			}
			PhoneHudHints._current = PhoneHudHints.Context.None;
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.HideIfCurrent(PhoneHudHints._photoViewerConfig);
		}

		// Token: 0x06006D41 RID: 27969 RVA: 0x002BD5C8 File Offset: 0x002BB7C8
		public static void ShowVideoViewer()
		{
			if (PhoneHudHints._current == PhoneHudHints.Context.VideoViewer)
			{
				return;
			}
			PhoneHudHints._current = PhoneHudHints.Context.VideoViewer;
			PhoneHudHints.Push(new List<UsableAction>
			{
				PhoneHudHints.Action("Space", "Play / pause"),
				PhoneHudHints.Action("Left / Right", "Seek 5 seconds"),
				PhoneHudHints.Action("M", "Mute"),
				PhoneHudHints.Action("Esc", "Close")
			}, false);
		}

		// Token: 0x06006D42 RID: 27970 RVA: 0x002BD644 File Offset: 0x002BB844
		public static void Hide()
		{
			if (PhoneHudHints._current == PhoneHudHints.Context.PhotoViewer)
			{
				PhoneHudHints.HidePhotoViewer();
				return;
			}
			if (PhoneHudHints._current == PhoneHudHints.Context.None)
			{
				return;
			}
			PhoneHudHints._current = PhoneHudHints.Context.None;
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.HideSupplemental(PhoneHudHints._phoneConfig);
				instance.HideIfCurrent(PhoneHudHints._phoneConfig);
			}
		}

		// Token: 0x06006D43 RID: 27971 RVA: 0x002BD694 File Offset: 0x002BB894
		private static void Push(List<UsableAction> actions, bool supplemental = false)
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			if (PhoneHudHints._phoneConfig == null)
			{
				PhoneHudHints._phoneConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
				PhoneHudHints._phoneConfig.hideFlags = HideFlags.HideAndDontSave;
				PhoneHudHints._phoneConfig.objectName = "Phone";
			}
			PhoneHudHints._phoneConfig.usableActions = actions;
			ControllerContextBindings.MarkControllerLabels(PhoneHudHints._phoneConfig, ControllerInput.IsActive);
			if (supplemental)
			{
				instance.HideIfCurrent(PhoneHudHints._phoneConfig);
				instance.ShowSupplemental(PhoneHudHints._phoneConfig, PhoneHudHints._phoneConfig);
				return;
			}
			instance.HideSupplemental(PhoneHudHints._phoneConfig);
			instance.Show(PhoneHudHints._phoneConfig);
		}

		// Token: 0x06006D44 RID: 27972 RVA: 0x002BD733 File Offset: 0x002BB933
		private static UsableAction Action(string key, string name)
		{
			return new UsableAction
			{
				keyDisplayLabel = key,
				actionName = name
			};
		}

		// Token: 0x04006FDE RID: 28638
		private static PhoneHudHints.Context _current = PhoneHudHints.Context.None;

		// Token: 0x04006FDF RID: 28639
		private static InteractionConfigSO _photoViewerConfig;

		// Token: 0x04006FE0 RID: 28640
		private static InteractionConfigSO _phoneConfig;

		// Token: 0x04006FE1 RID: 28641
		private static bool _phoneHintsController;

		// Token: 0x04006FE2 RID: 28642
		private static bool _phoneHintsSeated;

		// Token: 0x04006FE3 RID: 28643
		private static bool _photoHintsController;

		// Token: 0x04006FE4 RID: 28644
		private static bool _lastSelfiePoseDefault = true;

		// Token: 0x04006FE5 RID: 28645
		private static bool _lastSelfieVideoMode;

		// Token: 0x04006FE6 RID: 28646
		private static bool _lastSelfieRecording;

		// Token: 0x04006FE7 RID: 28647
		private static bool _lastSelfieTripodActive;

		// Token: 0x04006FE8 RID: 28648
		private static bool _lastTripodPlacementActive;

		// Token: 0x04006FE9 RID: 28649
		private static bool _lastSelfieController;

		// Token: 0x04006FEA RID: 28650
		private static bool _lastBodyOwned;

		// Token: 0x04006FEB RID: 28651
		private static bool _lastFilterReserved;

		// Token: 0x04006FEC RID: 28652
		private static bool _lastShutterReserved;

		// Token: 0x04006FED RID: 28653
		private static bool _lastExitBlocked;

		// Token: 0x04006FEE RID: 28654
		private static bool _lastEmotesAllowed;

		// Token: 0x04006FEF RID: 28655
		private static bool _lastEmoteSpeedActive;

		// Token: 0x04006FF0 RID: 28656
		private static bool _lastMediaModeReserved;

		// Token: 0x04006FF1 RID: 28657
		private static bool _lastTripodPlacementAllowed = true;

		// Token: 0x04006FF2 RID: 28658
		private static bool _lastBodyPosesAllowed = true;

		// Token: 0x04006FF3 RID: 28659
		private static bool _lastSelfieSeated;

		// Token: 0x02000E47 RID: 3655
		private enum Context
		{
			// Token: 0x04006FF5 RID: 28661
			None,
			// Token: 0x04006FF6 RID: 28662
			PhoneHeld,
			// Token: 0x04006FF7 RID: 28663
			PhoneHeldFullZoom,
			// Token: 0x04006FF8 RID: 28664
			Selfie,
			// Token: 0x04006FF9 RID: 28665
			PhotoViewer,
			// Token: 0x04006FFA RID: 28666
			VideoViewer
		}
	}
}
