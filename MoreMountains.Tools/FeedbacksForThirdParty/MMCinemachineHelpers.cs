using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002E5 RID: 741
	public class MMCinemachineHelpers : MonoBehaviour
	{
		// Token: 0x060014C0 RID: 5312 RVA: 0x00058AE0 File Offset: 0x00056CE0
		public static GameObject AutomaticCinemachineShakersSetup(MMF_Player owner, string feedbackName)
		{
			bool flag = false;
			string text = owner.name + " " + feedbackName + " feedback automatic shaker setup : ";
			if ((CinemachineBrain)Object.FindObjectOfType(typeof(CinemachineBrain)) == null)
			{
				Camera.main.gameObject.AddComponent<CinemachineBrain>();
				text += "Added a Cinemachine Brain to the scene. ";
			}
			CinemachineVirtualCamera cinemachineVirtualCamera = (CinemachineVirtualCamera)Object.FindObjectOfType(typeof(CinemachineVirtualCamera));
			if (cinemachineVirtualCamera == null)
			{
				GameObject gameObject = new GameObject("CinemachineVirtualCamera");
				if (Camera.main != null)
				{
					gameObject.transform.position = Camera.main.transform.position;
				}
				cinemachineVirtualCamera = gameObject.AddComponent<CinemachineVirtualCamera>();
				text += "Added a Cinemachine Virtual Camera to the scene. ";
				flag = true;
			}
			GameObject gameObject2 = cinemachineVirtualCamera.gameObject;
			if (cinemachineVirtualCamera.GetComponent<CinemachineImpulseListener>() == null)
			{
				cinemachineVirtualCamera.gameObject.AddComponent<CinemachineImpulseListener>();
				text += "Added an impulse listener. ";
			}
			if (flag)
			{
				gameObject2.MMGetOrAddComponent<MMCinemachineCameraShaker>();
				gameObject2.MMGetOrAddComponent<MMCinemachineZoom>();
				gameObject2.MMGetOrAddComponent<MMCinemachinePriorityListener>();
				gameObject2.MMGetOrAddComponent<MMCinemachineClippingPlanesShaker>();
				gameObject2.MMGetOrAddComponent<MMCinemachineFieldOfViewShaker>();
				text += "Added camera shaker, zoom, priority listener, clipping planes shaker and field of view shaker to the Cinemachine Camera. ";
			}
			MMDebug.DebugLogInfo(text + "You're all set.", "", 3, true);
			return gameObject2;
		}
	}
}
