using System;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x02000493 RID: 1171
	public static class vCharacterExtensions
	{
		// Token: 0x06001CBC RID: 7356 RVA: 0x000B1CD0 File Offset: 0x000AFED0
		public static void LoadActionControllers(this vCharacter character, bool debug = false)
		{
			IActionController[] componentsInChildren = character.GetComponentsInChildren<IActionController>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].enabled)
				{
					if (componentsInChildren[i] is IActionListener)
					{
						IActionListener actionListener = componentsInChildren[i] as IActionListener;
						if (actionListener.actionEnter)
						{
							character.onActionEnter.RemoveListener(new UnityAction<Collider>(actionListener.OnActionEnter));
							character.onActionEnter.AddListener(new UnityAction<Collider>(actionListener.OnActionEnter));
							if (debug)
							{
								Debug.Log("Register Action Enter event to the " + actionListener.GetType().Name);
							}
						}
						if (actionListener.actionStay)
						{
							character.onActionStay.RemoveListener(new UnityAction<Collider>(actionListener.OnActionStay));
							character.onActionStay.AddListener(new UnityAction<Collider>(actionListener.OnActionStay));
							if (debug)
							{
								Debug.Log("Register Action Stay event to the " + actionListener.GetType().Name);
							}
						}
						if (actionListener.actionExit)
						{
							character.onActionExit.RemoveListener(new UnityAction<Collider>(actionListener.OnActionExit));
							character.onActionExit.AddListener(new UnityAction<Collider>(actionListener.OnActionExit));
							if (debug)
							{
								Debug.Log("Register action Exit event to the " + actionListener.GetType().Name);
							}
						}
					}
					else
					{
						if (componentsInChildren[i] is IActionEnterListener)
						{
							character.onActionEnter.RemoveListener(new UnityAction<Collider>((componentsInChildren[i] as IActionEnterListener).OnActionEnter));
							character.onActionEnter.AddListener(new UnityAction<Collider>((componentsInChildren[i] as IActionEnterListener).OnActionEnter));
							if (debug)
							{
								Debug.Log("Register Action Enter event to the " + componentsInChildren[i].GetType().Name);
							}
						}
						if (componentsInChildren[i] is IActionStayListener)
						{
							character.onActionStay.RemoveListener(new UnityAction<Collider>((componentsInChildren[i] as IActionStayListener).OnActionStay));
							character.onActionStay.AddListener(new UnityAction<Collider>((componentsInChildren[i] as IActionStayListener).OnActionStay));
							if (debug)
							{
								Debug.Log("Register Action Stay event to the " + componentsInChildren[i].GetType().Name);
							}
						}
						if (componentsInChildren[i] is IActionExitListener)
						{
							character.onActionExit.RemoveListener(new UnityAction<Collider>((componentsInChildren[i] as IActionExitListener).OnActionExit));
							character.onActionExit.AddListener(new UnityAction<Collider>((componentsInChildren[i] as IActionExitListener).OnActionExit));
							if (debug)
							{
								Debug.Log("Register Action Exit event to the " + componentsInChildren[i].GetType().Name);
							}
						}
					}
				}
			}
		}
	}
}
