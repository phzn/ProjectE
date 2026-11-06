using System;
using Unity.XR.GoogleVr;
using Unity.XR.Oculus.Input;
using Unity.XR.OpenVR;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.XR.WindowsMR.Input;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x02000073 RID: 115
	internal static class XRSupport
	{
		// Token: 0x06000A61 RID: 2657 RVA: 0x000386AC File Offset: 0x000368AC
		public static void Initialize()
		{
			InputSystem.RegisterLayout<PoseControl>("Pose", null);
			InputSystem.RegisterLayout<BoneControl>("Bone", null);
			InputSystem.RegisterLayout<EyesControl>("Eyes", null);
			InputSystem.RegisterLayout<XRHMD>(null, null);
			InputSystem.RegisterLayout<XRController>(null, null);
			InputSystem.onFindLayoutForDevice += XRLayoutBuilder.OnFindLayoutForDevice;
			string text = null;
			InputDeviceMatcher inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<WMRHMD>(text, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("(Windows Mixed Reality HMD)|(Microsoft HoloLens)|(^(WindowsMR Headset))", true)));
			string text2 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<WMRSpatialController>(text2, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("(^(Spatial Controller))|(^(OpenVR Controller\\(WindowsMR))", true)));
			string text3 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<HololensHand>(text3, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("(^(Hand -))", true)));
			string text4 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<OculusHMD>(text4, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("^(Oculus Rift)|^(Oculus Quest)|^(Oculus Go)", true)));
			string text5 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<OculusTouchController>(text5, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("(^(Oculus Touch Controller))|(^(Oculus Quest Controller))", true)));
			string text6 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<OculusRemote>(text6, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("Oculus Remote", true)));
			string text7 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<OculusTrackingReference>(text7, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("((Tracking Reference)|(^(Oculus Rift [a-zA-Z0-9]* \\(Camera)))", true)));
			string text8 = "GearVR";
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<OculusHMDExtended>(text8, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("Oculus HMD", true)));
			string text9 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<GearVRTrackedController>(text9, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("^(Oculus Tracked Remote)", true)));
			string text10 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<DaydreamHMD>(text10, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("Daydream HMD", true)));
			string text11 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<DaydreamController>(text11, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("^(Daydream Controller)", true)));
			string text12 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<OpenVRHMD>(text12, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("^(OpenVR Headset)|^(Vive Pro)", true)));
			string text13 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<OpenVRControllerWMR>(text13, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("^(OpenVR Controller\\(WindowsMR)", true)));
			string text14 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			inputDeviceMatcher = inputDeviceMatcher.WithManufacturer("HTC", true);
			InputSystem.RegisterLayout<ViveWand>(text14, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("^(OpenVR Controller\\(((Vive. Controller)|(VIVE. Controller)|(Vive Controller)))", true)));
			string text15 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			InputSystem.RegisterLayout<OpenVROculusTouchController>(text15, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("^(OpenVR Controller\\(Oculus)", true)));
			string text16 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			inputDeviceMatcher = inputDeviceMatcher.WithManufacturer("HTC", true);
			InputSystem.RegisterLayout<ViveTracker>(text16, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("^(VIVE Tracker)", true)));
			string text17 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			inputDeviceMatcher = inputDeviceMatcher.WithManufacturer("HTC", true);
			InputSystem.RegisterLayout<HandedViveTracker>(text17, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("^(OpenVR Controller\\(VIVE Tracker)", true)));
			string text18 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("^(XRInput)", true);
			inputDeviceMatcher = inputDeviceMatcher.WithManufacturer("HTC", true);
			InputSystem.RegisterLayout<ViveLighthouse>(text18, new InputDeviceMatcher?(inputDeviceMatcher.WithProduct("^(HTC V2-XD/XE)", true)));
		}
	}
}
