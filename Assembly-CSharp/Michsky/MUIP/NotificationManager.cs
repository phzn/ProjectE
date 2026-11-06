using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000225 RID: 549
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Animator))]
	public class NotificationManager : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06000F27 RID: 3879 RVA: 0x0006552C File Offset: 0x0006372C
		private void Awake()
		{
			this.isOn = false;
			if (!this.useCustomContent)
			{
				this.UpdateUI();
			}
			if (this.notificationAnimator == null)
			{
				this.notificationAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (this.useStacking)
			{
				try
				{
					base.transform.GetComponentInParent<NotificationStacking>().AddToStack(this);
				}
				catch
				{
					Debug.LogError("<b>[Notification]</b> 'Stacking' is enabled but 'Notification Stacking' cannot be found in parent.", this);
				}
			}
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x000655A8 File Offset: 0x000637A8
		private void Start()
		{
			if (this.startBehaviour == NotificationManager.StartBehaviour.Disable)
			{
				base.gameObject.SetActive(false);
				return;
			}
			if (this.startBehaviour == NotificationManager.StartBehaviour.Open)
			{
				this.Open();
			}
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x000655D0 File Offset: 0x000637D0
		public void Open()
		{
			if (this.isOn)
			{
				return;
			}
			base.gameObject.SetActive(true);
			this.isOn = true;
			base.StopCoroutine("StartTimer");
			base.StopCoroutine("DisableNotification");
			this.notificationAnimator.Play("In");
			this.onOpen.Invoke();
			if (this.enableTimer)
			{
				base.StartCoroutine("StartTimer");
			}
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00065640 File Offset: 0x00063840
		public void Close()
		{
			if (!this.isOn)
			{
				return;
			}
			this.isOn = false;
			this.notificationAnimator.Play("Out");
			this.onClose.Invoke();
			base.StopCoroutine("StartTimer");
			base.StopCoroutine("DisableNotification");
			base.StartCoroutine("DisableNotification");
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0006569A File Offset: 0x0006389A
		public void OpenNotification()
		{
			this.Open();
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x000656A2 File Offset: 0x000638A2
		public void CloseNotification()
		{
			this.Close();
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x000656AC File Offset: 0x000638AC
		public void UpdateUI()
		{
			if (this.iconObj != null)
			{
				this.iconObj.sprite = this.icon;
			}
			if (this.titleObj != null)
			{
				this.titleObj.text = this.title;
			}
			if (this.descriptionObj != null)
			{
				this.descriptionObj.text = this.description;
			}
			if (this.slideDirection == NotificationManager.SlideDirection.Left)
			{
				base.transform.localScale = new Vector3(-1f, base.transform.localScale.y, base.transform.localScale.z);
				base.transform.GetChild(0).transform.localScale = new Vector3(-1f, base.transform.GetChild(0).transform.localScale.y, base.transform.GetChild(0).transform.localScale.z);
				return;
			}
			if (this.slideDirection == NotificationManager.SlideDirection.Right)
			{
				base.transform.localScale = new Vector3(1f, base.transform.localScale.y, base.transform.localScale.z);
				base.transform.GetChild(0).transform.localScale = new Vector3(1f, base.transform.GetChild(0).transform.localScale.y, base.transform.GetChild(0).transform.localScale.z);
			}
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00065845 File Offset: 0x00063A45
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.closeOnClick)
			{
				return;
			}
			this.Close();
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00065856 File Offset: 0x00063A56
		private IEnumerator StartTimer()
		{
			yield return new WaitForSecondsRealtime(this.timer);
			this.Close();
			yield break;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00065865 File Offset: 0x00063A65
		private IEnumerator DisableNotification()
		{
			yield return new WaitForSecondsRealtime(1f);
			if (this.closeBehaviour == NotificationManager.CloseBehaviour.Disable)
			{
				base.gameObject.SetActive(false);
				this.isOn = false;
			}
			else if (this.closeBehaviour == NotificationManager.CloseBehaviour.Destroy)
			{
				Object.Destroy(base.gameObject);
			}
			yield break;
		}

		// Token: 0x04000FFD RID: 4093
		public Sprite icon;

		// Token: 0x04000FFE RID: 4094
		public string title = "Notification Title";

		// Token: 0x04000FFF RID: 4095
		[TextArea(1, 4)]
		public string description = "Notification description";

		// Token: 0x04001000 RID: 4096
		public Animator notificationAnimator;

		// Token: 0x04001001 RID: 4097
		public Image iconObj;

		// Token: 0x04001002 RID: 4098
		public TextMeshProUGUI titleObj;

		// Token: 0x04001003 RID: 4099
		public TextMeshProUGUI descriptionObj;

		// Token: 0x04001004 RID: 4100
		public bool enableTimer = true;

		// Token: 0x04001005 RID: 4101
		public float timer = 3f;

		// Token: 0x04001006 RID: 4102
		[SerializeField]
		private bool useCustomContent;

		// Token: 0x04001007 RID: 4103
		public bool closeOnClick;

		// Token: 0x04001008 RID: 4104
		public bool useStacking;

		// Token: 0x04001009 RID: 4105
		[HideInInspector]
		public bool isOn;

		// Token: 0x0400100A RID: 4106
		public NotificationManager.StartBehaviour startBehaviour = NotificationManager.StartBehaviour.Disable;

		// Token: 0x0400100B RID: 4107
		public NotificationManager.CloseBehaviour closeBehaviour = NotificationManager.CloseBehaviour.Disable;

		// Token: 0x0400100C RID: 4108
		public NotificationManager.SlideDirection slideDirection;

		// Token: 0x0400100D RID: 4109
		public UnityEvent onOpen = new UnityEvent();

		// Token: 0x0400100E RID: 4110
		public UnityEvent onClose = new UnityEvent();

		// Token: 0x02000226 RID: 550
		public enum StartBehaviour
		{
			// Token: 0x04001010 RID: 4112
			None,
			// Token: 0x04001011 RID: 4113
			Disable,
			// Token: 0x04001012 RID: 4114
			Open
		}

		// Token: 0x02000227 RID: 551
		public enum CloseBehaviour
		{
			// Token: 0x04001014 RID: 4116
			None,
			// Token: 0x04001015 RID: 4117
			Disable,
			// Token: 0x04001016 RID: 4118
			Destroy
		}

		// Token: 0x02000228 RID: 552
		public enum SlideDirection
		{
			// Token: 0x04001018 RID: 4120
			Default,
			// Token: 0x04001019 RID: 4121
			Left,
			// Token: 0x0400101A RID: 4122
			Right
		}
	}
}
