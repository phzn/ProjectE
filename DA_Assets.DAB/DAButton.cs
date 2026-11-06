using System;
using System.Collections.Generic;
using DA_Assets.DAI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DA_Assets.DAB
{
	// Token: 0x02000008 RID: 8
	[AddComponentMenu("UI/D.A. Button (DAB)", 32)]
	public class DAButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002996 File Offset: 0x00000B96
		[SerializeProperty("_targetGraphics")]
		public List<MonoBehaviour> TargetGraphics
		{
			get
			{
				return this._targetGraphics;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000299E File Offset: 0x00000B9E
		[SerializeProperty("_audio")]
		public DAB_Audio Audio
		{
			get
			{
				return this._audio;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000029A6 File Offset: 0x00000BA6
		[SerializeProperty("_onPointerClick")]
		public UnityEvent OnPointerClickEvent
		{
			get
			{
				return this._onPointerClick;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000029AE File Offset: 0x00000BAE
		[SerializeProperty("_onPointerEnter")]
		public UnityEvent OnPointerEnterEvent
		{
			get
			{
				return this._onPointerEnter;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000029B6 File Offset: 0x00000BB6
		[SerializeProperty("_onPointerExit")]
		public UnityEvent OnPointerExitEvent
		{
			get
			{
				return this._onPointerExit;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000029BE File Offset: 0x00000BBE
		// (set) Token: 0x06000033 RID: 51 RVA: 0x000029C6 File Offset: 0x00000BC6
		[SerializeProperty("_interactable")]
		public bool Interactable
		{
			get
			{
				return this._interactable;
			}
			set
			{
				if (this._interactable != value)
				{
					this._interactable = value;
					if (this._interactable)
					{
						this.SendEvent(DAPointerEvent.OnInteractable);
						return;
					}
					this.SendEvent(DAPointerEvent.OnNonInteractable);
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000029F0 File Offset: 0x00000BF0
		private void OnEnable()
		{
			this._animators.Clear();
			foreach (MonoBehaviour monoBehaviour in this._targetGraphics)
			{
				if (!(monoBehaviour == null))
				{
					BasePropertyAnimatorBase[] componentsInChildren = monoBehaviour.GetComponentsInChildren<BasePropertyAnimatorBase>();
					if (componentsInChildren != null && componentsInChildren.Length != 0)
					{
						this._animators[monoBehaviour] = new List<BasePropertyAnimatorBase>(componentsInChildren);
					}
				}
			}
			if (!this._interactable)
			{
				this.SendEvent(DAPointerEvent.OnNonInteractable);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002A80 File Offset: 0x00000C80
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this._interactable)
			{
				return;
			}
			UnityEvent onPointerClick = this._onPointerClick;
			if (onPointerClick != null)
			{
				onPointerClick.Invoke();
			}
			this.SendEvent(DAPointerEvent.OnPointerClick);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002AA4 File Offset: 0x00000CA4
		private void PlaySound(DAPointerEvent pointerEvent)
		{
			if (this._audio.AudioSource == null)
			{
				return;
			}
			this._audio.AudioSource.Stop();
			switch (pointerEvent)
			{
			case DAPointerEvent.OnPointerClick:
				if (this._audio.OnPointerClick != null)
				{
					this._audio.AudioSource.PlayOneShot(this._audio.OnPointerClick);
					return;
				}
				break;
			case DAPointerEvent.OnPointerEnter:
				if (this._audio.OnPointerEnter != null)
				{
					this._audio.AudioSource.PlayOneShot(this._audio.OnPointerEnter);
					return;
				}
				break;
			case DAPointerEvent.OnPointerExit:
				if (this._audio.OnPointerExit != null)
				{
					this._audio.AudioSource.PlayOneShot(this._audio.OnPointerExit);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002B76 File Offset: 0x00000D76
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!this._interactable)
			{
				return;
			}
			UnityEvent onPointerEnter = this._onPointerEnter;
			if (onPointerEnter != null)
			{
				onPointerEnter.Invoke();
			}
			this.SendEvent(DAPointerEvent.OnPointerEnter);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002B99 File Offset: 0x00000D99
		public void OnPointerExit(PointerEventData eventData)
		{
			if (!this._interactable)
			{
				return;
			}
			UnityEvent onPointerExit = this._onPointerExit;
			if (onPointerExit != null)
			{
				onPointerExit.Invoke();
			}
			this.SendEvent(DAPointerEvent.OnPointerExit);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002BBC File Offset: 0x00000DBC
		private void SendEvent(DAPointerEvent pointerEvent)
		{
			this.PlaySound(pointerEvent);
			foreach (KeyValuePair<MonoBehaviour, List<BasePropertyAnimatorBase>> keyValuePair in this._animators)
			{
				foreach (BasePropertyAnimatorBase basePropertyAnimatorBase in keyValuePair.Value)
				{
					basePropertyAnimatorBase.SendPointerEvent(pointerEvent);
				}
			}
		}

		// Token: 0x0400001B RID: 27
		private Dictionary<MonoBehaviour, List<BasePropertyAnimatorBase>> _animators = new Dictionary<MonoBehaviour, List<BasePropertyAnimatorBase>>();

		// Token: 0x0400001C RID: 28
		[SerializeField]
		private List<MonoBehaviour> _targetGraphics = new List<MonoBehaviour>();

		// Token: 0x0400001D RID: 29
		[SerializeField]
		private DAB_Audio _audio;

		// Token: 0x0400001E RID: 30
		[SerializeField]
		private UnityEvent _onPointerClick;

		// Token: 0x0400001F RID: 31
		[SerializeField]
		private UnityEvent _onPointerEnter;

		// Token: 0x04000020 RID: 32
		[SerializeField]
		private UnityEvent _onPointerExit;

		// Token: 0x04000021 RID: 33
		[SerializeField]
		private bool _interactable = true;
	}
}
