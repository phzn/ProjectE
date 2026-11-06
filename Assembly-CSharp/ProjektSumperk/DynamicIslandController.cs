using System;
using System.Collections;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x020003D8 RID: 984
	public class DynamicIslandController : MonoBehaviour
	{
		// Token: 0x06001924 RID: 6436 RVA: 0x0009FD2D File Offset: 0x0009DF2D
		private static void SetDynamic(TMP_Text text, string value)
		{
			if (text == null)
			{
				return;
			}
			LocalizedGameText.BindFontOnly(text);
			text.text = value;
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x00004F31 File Offset: 0x00003131
		private void Start()
		{
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0009FD47 File Offset: 0x0009DF47
		private void MoneyNotification()
		{
			this.NewCashDINotification("Jimmy", 5000, "Send");
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0009FD5E File Offset: 0x0009DF5E
		public void UniversalBack(Animator anim)
		{
			if (anim == null)
			{
				return;
			}
			anim.Play("BackPanelAnimation", -1, 0f);
			PhoneAppAutoHide.Schedule(anim.gameObject, 0.55f);
			base.StartCoroutine(this.NotchBounce(0.5f));
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0009FDA0 File Offset: 0x0009DFA0
		public void ResetToIdle()
		{
			base.StopAllCoroutines();
			if (this.DIEmailSMSActivity != null)
			{
				this.DIEmailSMSActivity.SetActive(false);
			}
			if (this.MusicNotchAnim != null)
			{
				this.MusicNotchAnim.gameObject.SetActive(false);
			}
			if (this.TimerNotchAnim != null)
			{
				this.TimerNotchAnim.gameObject.SetActive(false);
			}
			if (this.TimeText != null)
			{
				this.TimeText.SetActive(true);
			}
			if (this.SigBatt != null)
			{
				this.SigBatt.SetActive(true);
			}
			if (this.NotchAnim != null)
			{
				this.NotchAnim.Play("NotchBounceAnimation", -1, 0f);
			}
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x0009FE63 File Offset: 0x0009E063
		public void DITimerAnimation(Animator anim)
		{
			if (this.timerController.isTimerRunning)
			{
				this.UniversalBack(anim);
				base.StartCoroutine(this.TimerNotch(0.6f));
				return;
			}
			this.UniversalBack(anim);
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0009FE93 File Offset: 0x0009E093
		private IEnumerator TimerNotch(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.TimeText.SetActive(false);
			this.SigBatt.SetActive(false);
			this.TimerNotchAnim.gameObject.SetActive(true);
			this.TimerNotchAnim.Play("TimerDIAnimation", -1, 0f);
			this.TimerNotchAnim.transform.SetAsLastSibling();
			yield break;
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0009FEAC File Offset: 0x0009E0AC
		public void DIRadioAnimation(Animator anim)
		{
			if (this.radioController.isRadioPlaying)
			{
				this.musicIcon.sprite = this.radio;
				this.UniversalBack(anim);
				base.StartCoroutine(this.MusicNotch(0.6f));
				return;
			}
			this.UniversalBack(anim);
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0009FEF8 File Offset: 0x0009E0F8
		public void DIMusicAnimation(Animator anim)
		{
			if (this.musicPlayerController.isMusicPlaying)
			{
				this.musicIcon.sprite = this.music;
				this.UniversalBack(anim);
				base.StartCoroutine(this.MusicNotch(0.6f));
				return;
			}
			this.UniversalBack(anim);
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0009FF44 File Offset: 0x0009E144
		private IEnumerator MusicNotch(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.TimeText.SetActive(false);
			this.SigBatt.SetActive(false);
			this.MusicNotchAnim.gameObject.SetActive(true);
			this.MusicNotchAnim.Play("NotchMusicAnimation", -1, 0f);
			this.MusicNotchAnim.transform.SetAsLastSibling();
			yield break;
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x0009FF5A File Offset: 0x0009E15A
		private IEnumerator HideAfterDelay(GameObject objToHide, float delay)
		{
			yield return new WaitForSeconds(delay);
			objToHide.SetActive(false);
			yield break;
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x0009FF70 File Offset: 0x0009E170
		private IEnumerator NotchBounce(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.NotchAnim.Play("NotchBounceAnimation", -1, 0f);
			yield break;
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x0009FF88 File Offset: 0x0009E188
		public void GetCallDINotification(string callerName)
		{
			DynamicIslandController.SetDynamic(this.DICallerName, callerName);
			this.DIGetCall.SetActive(true);
			DynamicIslandController.SetDynamic(this.DIGetCall.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>(), callerName);
			this.DIGetCall.transform.SetAsLastSibling();
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0009FFE0 File Offset: 0x0009E1E0
		public void GetTrophyDINotification(string trophyTitle, string status, float progress, int spriteIndex)
		{
			this.DIGetTropy.transform.GetChild(0).GetChild(0).GetComponent<Image>()
				.sprite = this.trophyManager.trophySprites[spriteIndex];
			DynamicIslandController.SetDynamic(this.DIGetTropy.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>(), trophyTitle);
			this.DIGetTropy.SetActive(true);
			this.recentTrophyImage.sprite = this.trophyManager.trophySprites[spriteIndex];
			DynamicIslandController.SetDynamic(this.recentTrophyText, trophyTitle);
			this.trophyManager.SetTrophyClaimed(trophyTitle, status, progress, spriteIndex);
			this.DIGetTropy.transform.SetAsLastSibling();
			base.StartCoroutine(this.HideAfterDelay(this.DIGetTropy, 3f));
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x000A00A8 File Offset: 0x0009E2A8
		public void NewQuestDINotification(string title, string type, string description, int xpReward, string location)
		{
			LocalizedGameText.BindSource(this.DIQuest.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>(), "phone_story", "New Quest Add!", null);
			DynamicIslandController.SetDynamic(this.DIQuest.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>(), title);
			this.DIQuest.SetActive(true);
			this.questManager.CreateNewQuest(title, type, description, xpReward, location);
			this.questRedDot.SetActive(true);
			this.DIQuest.transform.SetAsLastSibling();
			base.StartCoroutine(this.HideAfterDelay(this.DIQuest, 3f));
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x000A0158 File Offset: 0x0009E358
		public void QuestCompletedDINotification(string title, string status)
		{
			LocalizedGameText.BindSource(this.DIQuest.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>(), "phone_story", "Quest Completed!", null);
			DynamicIslandController.SetDynamic(this.DIQuest.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>(), title);
			this.DIQuest.SetActive(true);
			this.questManager.UpdateQuestStatus(title, status);
			this.questRedDot.SetActive(true);
			this.DIQuest.transform.SetAsLastSibling();
			base.StartCoroutine(this.HideAfterDelay(this.DIQuest, 3f));
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x000A0204 File Offset: 0x0009E404
		public void NewActivityDINotification(string senderName, string body)
		{
			this.DIEmailSMSActivity.transform.GetChild(0).GetComponent<Image>().sprite = this.activity;
			LocalizedGameText.BindSource(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>(), "phone_story", "New Activity Received!", null);
			DynamicIslandController.SetDynamic(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>(), body);
			this.DIEmailSMSActivity.SetActive(true);
			this.activityManager.AddNewActivity(senderName, body, DateTime.Now.ToString());
			this.activityRedDot.SetActive(true);
			base.StartCoroutine(this.HideAfterDelay(this.DIEmailSMSActivity, 3f));
			this.DIEmailSMSActivity.transform.SetAsLastSibling();
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x000A02DC File Offset: 0x0009E4DC
		public void NewEmailDINotification(string senderName, string body)
		{
			this.DIEmailSMSActivity.transform.GetChild(0).GetComponent<Image>().sprite = this.email;
			LocalizedGameText.BindSource(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>(), "phone_story", "New Email Received!", null);
			DynamicIslandController.SetDynamic(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>(), body);
			this.DIEmailSMSActivity.SetActive(true);
			this.emailManager.CreateNewEmail(senderName, body, DateTime.Now.ToString());
			this.emailRedDot.SetActive(true);
			base.StartCoroutine(this.HideAfterDelay(this.DIEmailSMSActivity, 3f));
			this.DIEmailSMSActivity.transform.SetAsLastSibling();
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x000A03B4 File Offset: 0x0009E5B4
		public void NewSMSDINotification(string senderName, string body)
		{
			this.DIEmailSMSActivity.transform.GetChild(0).GetComponent<Image>().sprite = this.msg;
			LocalizedGameText.BindSource(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>(), "phone_story", "New Message Received!", null);
			DynamicIslandController.SetDynamic(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>(), body);
			this.DIEmailSMSActivity.SetActive(true);
			this.messageManager.CreateNewMessage(senderName, body, DateTime.Now.ToString());
			this.msgRedDot.SetActive(true);
			base.StartCoroutine(this.HideAfterDelay(this.DIEmailSMSActivity, 3f));
			this.DIEmailSMSActivity.transform.SetAsLastSibling();
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x000A048C File Offset: 0x0009E68C
		public void NewLoadoutDINotification(string weaponName)
		{
			this.DIEmailSMSActivity.transform.GetChild(0).GetComponent<Image>().sprite = this.loadout;
			LocalizedGameText.BindSource(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>(), "phone_story", "New weapon item acquired!", null);
			DynamicIslandController.SetDynamic(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>(), weaponName);
			this.DIEmailSMSActivity.SetActive(true);
			this.loadoutRedDot.SetActive(true);
			this.DIEmailSMSActivity.transform.SetAsLastSibling();
			base.StartCoroutine(this.HideAfterDelay(this.DIEmailSMSActivity, 3f));
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x000A054C File Offset: 0x0009E74C
		public void WeatherDINotification(string city, float temperature, string weatherCondition)
		{
			this.DIEmailSMSActivity.transform.GetChild(0).GetComponent<Image>().sprite = this.weather;
			LocalizedGameText.BindSource(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>(), "phone_story", "Weather has been update!", null);
			LocalizedGameText.BindComputed(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>(), () => GameLocalization.FormatSource("phone_story", "{0}, {1} °C, {2}", new object[]
			{
				GameLocalization.GetSource("phone_story", city),
				temperature,
				GameLocalization.GetSource("phone_story", weatherCondition)
			}));
			this.DIEmailSMSActivity.SetActive(true);
			this.weatherController.SetWeather(city, temperature, weatherCondition);
			this.DIEmailSMSActivity.transform.SetAsLastSibling();
			base.StartCoroutine(this.HideAfterDelay(this.DIEmailSMSActivity, 3f));
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x000A0644 File Offset: 0x0009E844
		public void NewCashDINotification(string name, int amount, string type)
		{
			this.DIEmailSMSActivity.transform.GetChild(0).GetComponent<Image>().sprite = this.cash;
			DynamicIslandController.SetDynamic(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>(), "$" + amount.ToString());
			if (type == "Send" && amount > this.moneyManager.BankBalance)
			{
				LocalizedGameText.BindSource(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>(), "phone_story", "Insufficient Bank Balance", null);
				this.DIEmailSMSActivity.SetActive(true);
				return;
			}
			if (type == "Receive")
			{
				LocalizedGameText.BindFormat(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>(), "phone_story", "{0} sent you cash", new object[] { name });
				this.bankRedDot.SetActive(true);
			}
			else if (type == "Send" && amount < this.moneyManager.BankBalance)
			{
				LocalizedGameText.BindFormat(this.DIEmailSMSActivity.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>(), "phone_story", "Cash sent to {0}", new object[] { name });
				this.bankRedDot.SetActive(true);
			}
			this.DIEmailSMSActivity.SetActive(true);
			this.moneyManager.CreateNewTransaction(name, amount, DateTime.Now.ToString(), type);
			base.StartCoroutine(this.HideAfterDelay(this.DIEmailSMSActivity, 3f));
			this.DIEmailSMSActivity.transform.SetAsLastSibling();
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x000A07F6 File Offset: 0x0009E9F6
		public void NextButton()
		{
			this.currentNotificationIndex = (this.currentNotificationIndex + 1) % 15;
			this.ShowNextNotification();
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x000A080F File Offset: 0x0009EA0F
		public void PreviousButton()
		{
			this.currentNotificationIndex = (this.currentNotificationIndex - 1 + 15) % 15;
			this.ShowNextNotification();
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x000A082C File Offset: 0x0009EA2C
		private void ShowNextNotification()
		{
			switch (this.currentNotificationIndex)
			{
			case 0:
				this.NewEmailDINotification("John Doe", "Hello, this is an email notification.");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Email Notification", null);
				return;
			case 1:
				this.GetCallDINotification("Alice");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Call Notification", null);
				return;
			case 2:
				this.GetTrophyDINotification("Champion of Chaos", "Claimed", 100f, 2);
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Trophy Notification", null);
				return;
			case 3:
				this.NewSMSDINotification("Bob", "Hello, this is an messgae notification.");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Message Notification", null);
				return;
			case 4:
				this.NewCashDINotification("Alice", 4500, "Receive");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Money Receive Notification", null);
				return;
			case 5:
				this.NewCashDINotification("Alice", 500, "Send");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Money Send Notification", null);
				return;
			case 6:
				this.NewLoadoutDINotification("AK 47");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Loadout Notification", null);
				return;
			case 7:
				this.WeatherDINotification("London", 34f, "Sunny");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Weather Notification: Sunny", null);
				return;
			case 8:
				this.WeatherDINotification("Mumbai", 36f, "Overcast");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Weather Notification: Overcast", null);
				return;
			case 9:
				this.WeatherDINotification("New York", 34f, "Rainy");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Weather Notification: Rainy", null);
				return;
			case 10:
				this.NewQuestDINotification("New Main Quest Title", "Main Quest", "New Main Quest description text comes here.", 600, "location name");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "New Main Quest Notification", null);
				return;
			case 11:
				this.NewQuestDINotification("New Side Quest Title", "Side Quest", "New Side Quest description text comes here.", 300, "location name");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "New Side Quest Notification", null);
				return;
			case 12:
				this.QuestCompletedDINotification("The Elemental Crystals", "Completed");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Quest Completed Notification", null);
				return;
			case 13:
				this.QuestCompletedDINotification("The Dark Veil Conspiracy", "Tracking");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Quest Tracking Notification", null);
				return;
			case 14:
				this.NewActivityDINotification("John Doe", "Hello, this is an Activity notification.");
				LocalizedGameText.BindSource(this.notificationText, "phone_story", "Activity Notification", null);
				return;
			default:
				return;
			}
		}

		// Token: 0x04001A48 RID: 6728
		public Animator NotchAnim;

		// Token: 0x04001A49 RID: 6729
		public Animator MusicNotchAnim;

		// Token: 0x04001A4A RID: 6730
		public Animator TimerNotchAnim;

		// Token: 0x04001A4B RID: 6731
		public GameObject TimeText;

		// Token: 0x04001A4C RID: 6732
		public GameObject SigBatt;

		// Token: 0x04001A4D RID: 6733
		public GameObject DIGetCall;

		// Token: 0x04001A4E RID: 6734
		public TMP_Text DICallerName;

		// Token: 0x04001A4F RID: 6735
		public GameObject DIGetTropy;

		// Token: 0x04001A50 RID: 6736
		public TrophyManager trophyManager;

		// Token: 0x04001A51 RID: 6737
		public QuestManager questManager;

		// Token: 0x04001A52 RID: 6738
		public Image recentTrophyImage;

		// Token: 0x04001A53 RID: 6739
		public TMP_Text recentTrophyText;

		// Token: 0x04001A54 RID: 6740
		public GameObject DIQuest;

		// Token: 0x04001A55 RID: 6741
		public Sprite email;

		// Token: 0x04001A56 RID: 6742
		public Sprite msg;

		// Token: 0x04001A57 RID: 6743
		public Sprite activity;

		// Token: 0x04001A58 RID: 6744
		public Sprite cash;

		// Token: 0x04001A59 RID: 6745
		public Sprite loadout;

		// Token: 0x04001A5A RID: 6746
		public Sprite radio;

		// Token: 0x04001A5B RID: 6747
		public Sprite music;

		// Token: 0x04001A5C RID: 6748
		public Sprite weather;

		// Token: 0x04001A5D RID: 6749
		public Image musicIcon;

		// Token: 0x04001A5E RID: 6750
		public GameObject DIEmailSMSActivity;

		// Token: 0x04001A5F RID: 6751
		public ActivityManager activityManager;

		// Token: 0x04001A60 RID: 6752
		public MessageManager messageManager;

		// Token: 0x04001A61 RID: 6753
		public EmailManager emailManager;

		// Token: 0x04001A62 RID: 6754
		public MoneyManager moneyManager;

		// Token: 0x04001A63 RID: 6755
		public TimerUIController timerController;

		// Token: 0x04001A64 RID: 6756
		public RadioController radioController;

		// Token: 0x04001A65 RID: 6757
		public MusicPlayerController musicPlayerController;

		// Token: 0x04001A66 RID: 6758
		public WeatherController weatherController;

		// Token: 0x04001A67 RID: 6759
		public GameObject questRedDot;

		// Token: 0x04001A68 RID: 6760
		public GameObject emailRedDot;

		// Token: 0x04001A69 RID: 6761
		public GameObject msgRedDot;

		// Token: 0x04001A6A RID: 6762
		public GameObject activityRedDot;

		// Token: 0x04001A6B RID: 6763
		public GameObject bankRedDot;

		// Token: 0x04001A6C RID: 6764
		public GameObject loadoutRedDot;

		// Token: 0x04001A6D RID: 6765
		public TMP_Text notificationText;

		// Token: 0x04001A6E RID: 6766
		private int currentNotificationIndex;
	}
}
