using System;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Tutorials
{
	// Token: 0x02000BD4 RID: 3028
	public static class TutorialGuideMedia
	{
		// Token: 0x06005733 RID: 22323 RVA: 0x0021EE64 File Offset: 0x0021D064
		public static float GetAspectRatio(TutorialTopic topic, Texture2D texture, int index)
		{
			if (!(topic.Id == "world_cover") || !(texture != null))
			{
				return 1.7777778f;
			}
			return (float)texture.width / (float)Mathf.Max(1, texture.height);
		}

		// Token: 0x06005734 RID: 22324 RVA: 0x0021EE9C File Offset: 0x0021D09C
		public static bool Build(RectTransform host, TutorialTopic topic, Texture2D texture, int index)
		{
			if (topic.Id == "world_cover")
			{
				return false;
			}
			if (topic.Id == "introduction")
			{
				PauseMenuTutorialPanel componentInParent = host.GetComponentInParent<PauseMenuTutorialPanel>();
				TMP_Text tmp_Text = ((componentInParent != null) ? componentInParent.GetComponentInChildren<TMP_Text>(true) : null);
				TutorialIntroductionPage.Build(host, LocalizedGameText.SourceFont(tmp_Text), null);
				return true;
			}
			RectTransform rectTransform = TutorialGuideMedia.Rect("Guide Artwork", host, 0f, 0f, 1280f, 720f);
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			RectTransform rectTransform4 = rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform4.pivot = vector;
			rectTransform2.anchorMin = (rectTransform3.anchorMax = vector);
			rectTransform.anchoredPosition = Vector2.zero;
			host.gameObject.AddComponent<TutorialMediaFit>().Configure(rectTransform);
			TutorialGuideMedia.Picture(rectTransform, "Project Exhibitionist Background", Resources.Load<Texture2D>("Tutorials/Art/project_exhibitionist_background"), new Rect(0f, 0f, 1f, 1f), 0f, 0f, 1280f, 720f).color = new Color(0.43f, 0.43f, 0.43f, 1f);
			TutorialGuideMedia.Panel(rectTransform, "Top Accent", 64f, 50f, 48f, 3f, TutorialGuideMedia.Pink);
			TutorialGuideMedia.Label(rectTransform, "Series", "PROJECT EXHIBITIONIST  /  FIELD GUIDE", 64f, 65f, 1100f, 24f, 15f, TutorialGuideMedia.Muted, false, TextAlignmentOptions.TopLeft);
			string id = topic.Id;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(id);
			if (num <= 2248688428U)
			{
				if (num <= 630905185U)
				{
					if (num <= 89534040U)
					{
						if (num != 5149376U)
						{
							if (num == 89534040U)
							{
								if (id == "movement")
								{
									TutorialGuideMedia.Movement(rectTransform, texture);
									return true;
								}
							}
						}
						else if (id == "poi_travel")
						{
							TutorialGuideMedia.Intro(rectTransform, "A PLACE\nTO GO", "Discover locations as you explore.\nSome are part of the city; others\nhave an entrance to a separate area.", "E", "ENTER / RETURN");
							TutorialGuideMedia.Label(rectTransform, "Entrance Hint", "LOOK FOR AN ENTRANCE", 528f, 135f, 670f, 38f, 22f, TutorialGuideMedia.Pink, true, TextAlignmentOptions.TopLeft);
							TutorialGuideMedia.Photo(rectTransform, texture, new Rect(0.11f, 0.22f, 0.8f, 0.59f), 528f, 181f, 670f, 360f);
							TutorialGuideMedia.Footer(rectTransform, "EXPLORE  /  USE RETURN TO CITY WHEN YOU LEAVE");
							return true;
						}
					}
					else if (num != 278748797U)
					{
						if (num == 630905185U)
						{
							if (id == "cielhome_pc")
							{
								TutorialHomeGuideMedia.Build(rectTransform, texture);
								return true;
							}
						}
					}
					else if (id == "heartbeat_orgasm")
					{
						TutorialHeartbeatGuideMedia.Build(rectTransform);
						return true;
					}
				}
				else if (num <= 1029965593U)
				{
					if (num != 746569015U)
					{
						if (num == 1029965593U)
						{
							if (id == "rapid_actions")
							{
								TutorialGuideMedia.RapidActions(rectTransform);
								return true;
							}
						}
					}
					else if (id == "photo_mode")
					{
						TutorialGuideMedia.WidePhoto(rectTransform, texture, "PHOTO MODE", "K  OPEN / RETURN     ·     SPACE  SAVE PHOTO");
						return true;
					}
				}
				else if (num != 2190321876U)
				{
					if (num == 2248688428U)
					{
						if (id == "detection")
						{
							TutorialGuideMedia.Detection(rectTransform);
							return true;
						}
					}
				}
				else if (id == "vehicles")
				{
					TutorialGuideMedia.Intro(rectTransform, "TAKE\nA RIDE", "Mount or deploy a vehicle,\nthen use its on-screen controls.\nWatch its remaining-use timer.", "E", "MOUNT / DISMOUNT");
					TutorialGuideMedia.Picture(rectTransform, "Ciel's Bicycle", Resources.Load<Texture2D>("Tutorials/Art/bicycle"), new Rect(0f, 0f, 1f, 1f), 505f, 22f, 744f, 744f);
					TutorialGuideMedia.Footer(rectTransform, "WASD  DRIVE     /     TIMER OR RING  USE REMAINING");
					return true;
				}
			}
			else if (num <= 2902356131U)
			{
				if (num <= 2662856321U)
				{
					if (num != 2310703474U)
					{
						if (num == 2662856321U)
						{
							if (id == "looting")
							{
								TutorialGuideMedia.Looting(rectTransform, texture);
								return true;
							}
						}
					}
					else if (id == "smartphone")
					{
						TutorialGuideMedia.WidePhoto(rectTransform, texture, "THE PHONE CAMERA", "P  PHONE     ·     R  FRONT / REAR     ·     SPACE  SHUTTER");
						return true;
					}
				}
				else if (num != 2760576167U)
				{
					if (num == 2902356131U)
					{
						if (id == "mood_switch")
						{
							TutorialGuideMedia.Mood(rectTransform);
							return true;
						}
					}
				}
				else if (id == "crouch_actions")
				{
					TutorialGuideMedia.Intro(rectTransform, "LOWER YOUR\nPROFILE", "Hold C to open the wheel.\nPoint at an action and release C.\n\nTap C to use or stop it.", "HOLD C", "CHOOSE AN ACTION");
					TutorialGuideMedia.Photo(rectTransform, texture, new Rect(0.365f, 0.14f, 0.29f, 0.61f), 558f, 118f, 620f, 495f);
					TutorialGuideMedia.Footer(rectTransform, "CROUCH  /  CRAWL  /  PRONE");
					return true;
				}
			}
			else if (num <= 3508635376U)
			{
				if (num != 2930960964U)
				{
					if (num == 3508635376U)
					{
						if (id == "item_types")
						{
							TutorialGuideMedia.ItemTypes(rectTransform, texture, index);
							return true;
						}
					}
				}
				else if (id == "minimap")
				{
					TutorialGuideMedia.Intro(rectTransform, "FIND YOUR\nWAY", "Expand the map to read the city.\nZoom in, drag to explore and\nplace a pin for your destination.", "M", "EXPAND THE MAP");
					TutorialGuideMedia.Photo(rectTransform, texture, new Rect(0.125f, 0.12f, 0.64f, 0.78f), 520f, 132f, 694f, 440f);
					TutorialGuideMedia.Footer(rectTransform, "WHEEL  ZOOM     /     DRAG  EXPLORE     /     MMB  PIN");
					return true;
				}
			}
			else if (num != 3526332565U)
			{
				if (num != 4244489279U)
				{
					if (num == 4270009365U)
					{
						if (id == "perspective")
						{
							TutorialGuideMedia.Intro(rectTransform, (index == 0) ? "THIRD\nPERSON" : "FIRST\nPERSON", (index == 0) ? "Keep Ciel and the space around\nher in view. Read nearby people\nand possible cover as you move." : "See from Ciel's viewpoint.\nLine up close interactions and\nmove through narrow spaces.", "V", "SWITCH YOUR VIEW");
							TutorialGuideMedia.Photo(rectTransform, texture, new Rect(0f, 0f, 1f, 1f), 550f, 118f, 620f, 495f);
							TutorialGuideMedia.Footer(rectTransform, "PRESS V AGAIN TO RETURN TO THE OTHER VIEW");
							return true;
						}
					}
				}
				else if (id == "inventory")
				{
					TutorialGuideMedia.Inventory(rectTransform, texture, index);
					return true;
				}
			}
			else if (id == "score")
			{
				TutorialGuideMedia.Score(rectTransform, texture);
				return true;
			}
			Object.Destroy(rectTransform.gameObject);
			return false;
		}

		// Token: 0x06005735 RID: 22325 RVA: 0x0021F5A4 File Offset: 0x0021D7A4
		private static void Mood(RectTransform board)
		{
			TutorialGuideMedia.Label(board, "Heading", "TWO MOODS. YOUR CHOICE.", 64f, 110f, 1140f, 66f, 44f, TutorialGuideMedia.White, true, TextAlignmentOptions.TopLeft);
			TutorialGuideMedia.Panel(board, "Embarrassed Stage", 64f, 196f, 506f, 394f, new Color(0.45f, 0.16f, 0.37f, 0.23f));
			TutorialGuideMedia.Panel(board, "Confident Stage", 710f, 196f, 506f, 394f, new Color(0.28f, 0.19f, 0.48f, 0.23f));
			TutorialGuideMedia.Picture(board, "Ciel Embarrassed", Resources.Load<Texture2D>("Tutorials/Art/ciel_emb"), new Rect(0.18f, 0.08f, 0.64f, 0.64f), 116f, 185f, 402f, 402f);
			TutorialGuideMedia.Picture(board, "Ciel Confident", Resources.Load<Texture2D>("Tutorials/Art/ciel_confident"), new Rect(0.18f, 0.08f, 0.64f, 0.64f), 762f, 185f, 402f, 402f);
			TutorialGuideMedia.Key(board, "TAB", 587f, 341f, 106f, 58f);
			TutorialGuideMedia.Label(board, "Switch", "SWITCH", 581f, 410f, 118f, 28f, 16f, TutorialGuideMedia.Muted, true, TextAlignmentOptions.Center);
			TutorialGuideMedia.Label(board, "Emb Label", "EMBARRASSED", 64f, 590f, 506f, 42f, 30f, TutorialGuideMedia.Pink, true, TextAlignmentOptions.Center);
			TutorialGuideMedia.Label(board, "Conf Label", "CONFIDENT", 710f, 590f, 506f, 42f, 30f, TutorialGuideMedia.Pink, true, TextAlignmentOptions.Center);
			TutorialGuideMedia.Label(board, "Emb Caption", "A shy expression and guarded posture", 64f, 640f, 506f, 30f, 19f, TutorialGuideMedia.Muted, false, TextAlignmentOptions.Center);
			TutorialGuideMedia.Label(board, "Conf Caption", "A self-assured expression and posture", 710f, 640f, 506f, 30f, 19f, TutorialGuideMedia.Muted, false, TextAlignmentOptions.Center);
		}

		// Token: 0x06005736 RID: 22326 RVA: 0x0021F804 File Offset: 0x0021DA04
		private static void Movement(RectTransform board, Texture2D texture)
		{
			TutorialGuideMedia.Intro(board, "SET YOUR\nPACE", "Walk to explore. Hold Shift to run.\nSlow down to recover stamina.\n\nCaps Lock keeps your current pace.", "WASD", "MOVE THROUGH THE CITY");
			TutorialGuideMedia.Panel(board, "Movement Controls", 536f, 138f, 680f, 450f, TutorialGuideMedia.Surface);
			TutorialGuideMedia.Key(board, "W", 812f, 180f, 104f, 84f);
			TutorialGuideMedia.Key(board, "A", 692f, 279f, 104f, 84f);
			TutorialGuideMedia.Key(board, "S", 812f, 279f, 104f, 84f);
			TutorialGuideMedia.Key(board, "D", 932f, 279f, 104f, 84f);
			TutorialGuideMedia.Key(board, "SHIFT", 598f, 412f, 192f, 60f);
			TutorialGuideMedia.Label(board, "Run Label", "HOLD TO RUN", 826f, 427f, 330f, 34f, 25f, TutorialGuideMedia.White, true, TextAlignmentOptions.TopLeft);
			TutorialGuideMedia.Key(board, "CAPS LOCK", 598f, 500f, 192f, 52f);
			TutorialGuideMedia.Label(board, "Auto Label", "KEEP YOUR PACE", 826f, 511f, 330f, 34f, 23f, TutorialGuideMedia.White, true, TextAlignmentOptions.TopLeft);
			TutorialGuideMedia.Footer(board, "ANY NEW DIRECTIONAL INPUT CANCELS AUTO WALK / RUN");
		}

		// Token: 0x06005737 RID: 22327 RVA: 0x0021F980 File Offset: 0x0021DB80
		private static void RapidActions(RectTransform board)
		{
			TutorialGuideMedia.Intro(board, "TAP TO ACT.\nHOLD TO PICK.", "Use the assigned action with a tap.\nHold the same input to open its\nselector and change the assignment.", "TAP / HOLD", "SAME INPUT, TWO USES");
			string[] array = new string[] { "Q / Z", "C", "RMB" };
			string[] array2 = new string[] { "EMOTES", "LOW STANCES", "ARMS / COVER" };
			string[] array3 = new string[] { "Your two assigned emote slots", "Crouch, Crawl or Prone", "The assigned arms or cover pose" };
			for (int i = 0; i < array.Length; i++)
			{
				float num = (float)(154 + i * 142);
				TutorialGuideMedia.Panel(board, "Action Slot " + i.ToString(), 536f, num, 680f, 122f, TutorialGuideMedia.Surface);
				TutorialGuideMedia.Key(board, array[i], 558f, num + 27f, 140f, 66f);
				TutorialGuideMedia.Label(board, "Slot Title " + i.ToString(), array2[i], 726f, num + 25f, 460f, 32f, 26f, TutorialGuideMedia.White, true, TextAlignmentOptions.TopLeft);
				TutorialGuideMedia.Label(board, "Slot Hint " + i.ToString(), array3[i], 726f, num + 65f, 460f, 40f, 20f, TutorialGuideMedia.Muted, false, TextAlignmentOptions.TopLeft);
			}
			TutorialGuideMedia.Footer(board, "THE HUD LABEL SHOWS THE ACTION CURRENTLY ASSIGNED TO EACH SLOT");
		}

		// Token: 0x06005738 RID: 22328 RVA: 0x0021FB04 File Offset: 0x0021DD04
		private static void Inventory(RectTransform board, Texture2D texture, int index)
		{
			if (index == 2)
			{
				TutorialGuideMedia.Intro(board, "WEAR IT.\nSEE IT.", "Equipping clothing changes Ciel\nimmediately. Keep useful pieces\nready in the clothing collection.", "4 + SPACE", "FOCUS / CYCLE CLOTHING");
				TutorialGuideMedia.Photo(board, texture, new Rect(0.395f, 0.34f, 0.2f, 0.6f), 657f, 124f, 425f, 482f);
				TutorialGuideMedia.Footer(board, "YOUR EQUIPPED OUTFIT IS VISIBLE ON CIEL");
				return;
			}
			bool flag = index == 1;
			TutorialGuideMedia.Intro(board, flag ? "YOUR\nWARDROBE" : "ITEMS AT\nA GLANCE", flag ? "Six clothing slots, separate\nfrom your items. Select a piece\nto read its details and actions." : "Select a filled slot to read details.\nDrag to reorder or swap.\nUse the available contextual action.", flag ? "4 + SPACE" : "1 / 2 / 3", flag ? "FOCUS / CYCLE CLOTHING" : "SELECT AN ITEM");
			TutorialGuideMedia.Panel(board, "Inventory Details", 536f, 133f, 680f, 469f, TutorialGuideMedia.Surface);
			if (flag)
			{
				TutorialGuideMedia.Label(board, "Details Label", "SELECTED CLOTHING", 565f, 154f, 622f, 36f, 20f, TutorialGuideMedia.Pink, true, TextAlignmentOptions.TopLeft);
				TutorialGuideMedia.Photo(board, texture, new Rect(0.042f, 0.095f, 0.91f, 0.252f), 565f, 207f, 622f, 328f);
				TutorialGuideMedia.Label(board, "Clothing Details Hint", "DESCRIPTION  /  COVERAGE  /  WEAR TIME", 565f, 554f, 622f, 30f, 18f, TutorialGuideMedia.Muted, false, TextAlignmentOptions.TopLeft);
				TutorialGuideMedia.Footer(board, "I  OPEN / CLOSE INVENTORY     /     SELECT A PIECE TO READ ITS EFFECTS");
				return;
			}
			TutorialGuideMedia.Label(board, "Details Label", "SELECTED ITEM", 565f, 154f, 622f, 36f, 20f, TutorialGuideMedia.Pink, true, TextAlignmentOptions.TopLeft);
			TutorialGuideMedia.Photo(board, texture, new Rect(0.646f, 0.101f, 0.343f, 0.104f), 565f, 204f, 622f, 145f);
			TutorialGuideMedia.Label(board, "Slots Label", flag ? "CLOTHING COLLECTION" : "ITEM SLOTS", 565f, 377f, 622f, 34f, 20f, TutorialGuideMedia.Pink, true, TextAlignmentOptions.TopLeft);
			TutorialGuideMedia.Photo(board, texture, flag ? new Rect(0.646f, 0.658f, 0.294f, 0.119f) : new Rect(0.646f, 0.48f, 0.295f, 0.153f), 565f, 422f, 622f, 150f);
			TutorialGuideMedia.Footer(board, "I  OPEN / CLOSE INVENTORY     /     RING OR BAR  DURABILITY");
		}

		// Token: 0x06005739 RID: 22329 RVA: 0x0021FD90 File Offset: 0x0021DF90
		private static void ItemTypes(RectTransform board, Texture2D texture, int index)
		{
			TutorialGuideMedia.Intro(board, (index == 0) ? "COVER\nITEMS" : "CONTEXT\nMATTERS", (index == 0) ? "A cover prop gives Ciel something\nto hide behind. Follow the action\nprompt to use its available moves." : "Each item brings its own action.\nRead the contextual prompt after\nselecting an unfamiliar item.", "1 / 2 / 3", "SELECT / READ / USE");
			if (index == 0)
			{
				TutorialGuideMedia.Photo(board, texture, new Rect(0.395f, 0.31f, 0.21f, 0.64f), 672f, 124f, 396f, 474f);
			}
			else
			{
				TutorialGuideMedia.Photo(board, texture, new Rect(0.79f, 0.852f, 0.205f, 0.145f), 568f, 225f, 590f, 236f);
				TutorialGuideMedia.Label(board, "Item Focus", "THE SELECTED SLOT", 568f, 484f, 590f, 36f, 23f, TutorialGuideMedia.White, true, TextAlignmentOptions.Center);
			}
			TutorialGuideMedia.Footer(board, "COVER  /  USE  /  EQUIP  /  DEPLOY   —   FOLLOW THE ITEM'S PROMPT");
		}

		// Token: 0x0600573A RID: 22330 RVA: 0x0021FE78 File Offset: 0x0021E078
		private static void Looting(RectTransform board, Texture2D texture)
		{
			TutorialGuideMedia.Intro(board, "SEARCH.\nREAD. COLLECT.", "Hold E at a lootable trashcan.\nKeep searching while the bar fills.\nRead rarity and durability on the loot.", "HOLD E", "RUMMAGE");
			TutorialGuideMedia.Picture(board, "Lootable Trashcan", Resources.Load<Texture2D>("Tutorials/Art/loot_trashcan"), new Rect(0f, 0f, 1f, 1f), 668f, 92f, 420f, 420f);
			TutorialGuideMedia.Photo(board, texture, new Rect(0.438f, 0.329f, 0.126f, 0.065f), 599f, 494f, 558f, 92f);
			TutorialGuideMedia.Label(board, "Loot Bar Label", "SEARCH PROGRESS / DROP STAGES", 558f, 601f, 640f, 30f, 18f, TutorialGuideMedia.Muted, false, TextAlignmentOptions.Center);
			TutorialGuideMedia.Footer(board, "COMMON  /  RARE  /  LEGENDARY     ·     CHECK THE REMAINING DURABILITY");
		}

		// Token: 0x0600573B RID: 22331 RVA: 0x0021FF58 File Offset: 0x0021E158
		private static void Score(RectTransform board, Texture2D texture)
		{
			TutorialGuideMedia.Intro(board, "MAKE IT\nCOUNT", "Discover, take risks and escape\nattention. The reward label tells\nyou which action earned points.", "SCORE", "EARN DURING YOUR RUN");
			TutorialGuideMedia.Panel(board, "Score Detail", 536f, 151f, 680f, 422f, TutorialGuideMedia.Surface);
			TutorialGuideMedia.Photo(board, texture, new Rect(0.545f, 0.355f, 0.052f, 0.056f), 758f, 198f, 244f, 148f);
			TutorialGuideMedia.Label(board, "Score Label", "ACTION  >  SCORE  >  RUN RECAP", 574f, 418f, 610f, 40f, 27f, TutorialGuideMedia.Pink, true, TextAlignmentOptions.TopLeft);
			TutorialGuideMedia.Label(board, "Score Hint", "Completed runs turn your result into EXP.\nUse the recap to review your progression.", 574f, 478f, 610f, 75f, 23f, TutorialGuideMedia.Muted, false, TextAlignmentOptions.TopLeft);
			TutorialGuideMedia.Footer(board, "A NEW SITUATION IS WORTH MORE THAN REPEATING THE SAME SAFE ACTION");
		}

		// Token: 0x0600573C RID: 22332 RVA: 0x0022004C File Offset: 0x0021E24C
		private static void Detection(RectTransform board)
		{
			TutorialGuideMedia.Label(board, "Heading", "SPOT THE WARNING. BREAK SIGHT.", 64f, 112f, 1152f, 65f, 42f, TutorialGuideMedia.White, true, TextAlignmentOptions.TopLeft);
			string[] array = new string[] { "NOTICE", "REPORT", "PURSUIT" };
			string[] array2 = new string[] { "Attention starts with an NPC.\nReact to the first marker.", "Completed detections\nraise city-wide pressure.", "Wanted pressure escalates\nreporters and police." };
			for (int i = 0; i < 3; i++)
			{
				float num = (float)(64 + i * 390);
				TutorialGuideMedia.Panel(board, "Detection Stage " + i.ToString(), num, 229f, 366f, 334f, TutorialGuideMedia.Surface);
				TutorialGuideMedia.Label(board, "Stage Number " + i.ToString(), "0" + (i + 1).ToString(), num + 26f, 255f, 300f, 63f, 48f, TutorialGuideMedia.Pink, true, TextAlignmentOptions.TopLeft);
				TutorialGuideMedia.Label(board, "Stage Title " + i.ToString(), array[i], num + 26f, 342f, 310f, 46f, 31f, TutorialGuideMedia.White, true, TextAlignmentOptions.TopLeft);
				TutorialGuideMedia.Label(board, "Stage Hint " + i.ToString(), array2[i], num + 26f, 419f, 310f, 110f, 23f, TutorialGuideMedia.Muted, false, TextAlignmentOptions.TopLeft);
			}
			TutorialGuideMedia.Footer(board, "BREAK LINE OF SIGHT  /  COVER UP  /  HIDE BEFORE DETECTION COMPLETES");
		}

		// Token: 0x0600573D RID: 22333 RVA: 0x002201F8 File Offset: 0x0021E3F8
		private static void WidePhoto(RectTransform board, Texture2D texture, string title, string controls)
		{
			TutorialGuideMedia.Label(board, "Heading", title, 64f, 108f, 1152f, 62f, 43f, TutorialGuideMedia.White, true, TextAlignmentOptions.TopLeft);
			TutorialGuideMedia.Photo(board, texture, new Rect(0f, 0f, 1f, 1f), 64f, 187f, 1152f, 444f);
			TutorialGuideMedia.Label(board, "Camera Controls", controls, 64f, 657f, 1152f, 31f, 21f, TutorialGuideMedia.Pink, true, TextAlignmentOptions.TopLeft);
		}

		// Token: 0x0600573E RID: 22334 RVA: 0x0022029C File Offset: 0x0021E49C
		private static void Intro(RectTransform board, string title, string body, string key, string action)
		{
			TutorialGuideMedia.Label(board, "Heading", title, 64f, 129f, 465f, 151f, 47f, TutorialGuideMedia.White, true, TextAlignmentOptions.TopLeft);
			TutorialGuideMedia.Label(board, "Guide Explanation", body, 64f, 320f, 436f, 159f, 25f, TutorialGuideMedia.Muted, false, TextAlignmentOptions.TopLeft);
			float num = (float)((key.Length > 7) ? 192 : 140);
			TutorialGuideMedia.Key(board, key, 64f, 505f, num, 55f);
			TutorialGuideMedia.Label(board, "Guide Action", action, 64f, 575f, 438f, 38f, 19f, TutorialGuideMedia.Pink, true, TextAlignmentOptions.TopLeft);
		}

		// Token: 0x0600573F RID: 22335 RVA: 0x0022036C File Offset: 0x0021E56C
		private static void Footer(RectTransform board, string text)
		{
			TutorialGuideMedia.Panel(board, "Footer Rule", 64f, 642f, 1152f, 1f, new Color(0.65f, 0.47f, 0.68f, 0.3f));
			TutorialGuideMedia.Label(board, "Artwork Footer", text, 64f, 660f, 1152f, 33f, 17f, TutorialGuideMedia.Muted, false, TextAlignmentOptions.TopLeft);
		}

		// Token: 0x06005740 RID: 22336 RVA: 0x002203E4 File Offset: 0x0021E5E4
		private static void Key(RectTransform board, string text, float x, float y, float width, float height)
		{
			TutorialGuideMedia.Panel(board, "Key Border " + text, x, y, width, height, new Color(0.75f, 0.48f, 0.69f, 0.8f));
			TutorialGuideMedia.Panel(board, "Key " + text, x + 2f, y + 2f, width - 4f, height - 4f, TutorialGuideMedia.Surface);
			TutorialGuideMedia.Label(board, "Key Text " + text, text, x + 5f, y + 3f, width - 10f, height - 6f, (float)((text.Length > 7) ? 23 : 28), TutorialGuideMedia.White, true, TextAlignmentOptions.Center);
		}

		// Token: 0x06005741 RID: 22337 RVA: 0x002204A4 File Offset: 0x0021E6A4
		private static void Photo(RectTransform parent, Texture2D texture, Rect crop, float x, float y, float width, float height)
		{
			if (texture == null)
			{
				return;
			}
			float num = (float)texture.width * crop.width / ((float)texture.height * crop.height);
			float num2 = Mathf.Min(width, height * num);
			float num3 = num2 / num;
			TutorialGuideMedia.Panel(parent, "Photo Border", x + (width - num2) * 0.5f - 2f, y + (height - num3) * 0.5f - 2f, num2 + 4f, num3 + 4f, new Color(0.59f, 0.39f, 0.63f, 0.65f));
			TutorialGuideMedia.Picture(parent, "Mechanic Detail", texture, new Rect(crop.x, 1f - crop.y - crop.height, crop.width, crop.height), x + (width - num2) * 0.5f, y + (height - num3) * 0.5f, num2, num3);
		}

		// Token: 0x06005742 RID: 22338 RVA: 0x0022059C File Offset: 0x0021E79C
		private static RawImage Picture(RectTransform parent, string name, Texture texture, Rect uv, float x, float y, float width, float height)
		{
			RawImage rawImage = TutorialGuideMedia.Rect(name, parent, x, y, width, height).gameObject.AddComponent<RawImage>();
			rawImage.texture = texture;
			rawImage.uvRect = uv;
			rawImage.raycastTarget = false;
			if (texture == null)
			{
				rawImage.color = Color.clear;
			}
			TutorialImageTextLocalization.Bind(rawImage);
			return rawImage;
		}

		// Token: 0x06005743 RID: 22339 RVA: 0x002205F3 File Offset: 0x0021E7F3
		private static void Panel(RectTransform parent, string name, float x, float y, float width, float height, Color color)
		{
			Image image = TutorialGuideMedia.Rect(name, parent, x, y, width, height).gameObject.AddComponent<Image>();
			image.color = color;
			image.raycastTarget = false;
		}

		// Token: 0x06005744 RID: 22340 RVA: 0x0022061C File Offset: 0x0021E81C
		private static TMP_Text Label(RectTransform parent, string name, string value, float x, float y, float width, float height, float size, Color color, bool bold = false, TextAlignmentOptions alignment = TextAlignmentOptions.TopLeft)
		{
			TextMeshProUGUI textMeshProUGUI = TutorialGuideMedia.Rect(name, parent, x, y, width, height).gameObject.AddComponent<TextMeshProUGUI>();
			PauseMenuTutorialPanel componentInParent = parent.GetComponentInParent<PauseMenuTutorialPanel>();
			TMP_Text tmp_Text = ((componentInParent != null) ? componentInParent.GetComponentInChildren<TMP_Text>() : null);
			textMeshProUGUI.font = ((tmp_Text != null) ? LocalizedGameText.SourceFont(tmp_Text) : TMP_Settings.defaultFontAsset);
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = (bold ? FontStyles.Bold : FontStyles.Normal);
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.characterSpacing = (bold ? 0.6f : 0f);
			LocalizedGameText.BindSource(textMeshProUGUI, "tutorial", value, null).FitTranslationToRectangle(false);
			return textMeshProUGUI;
		}

		// Token: 0x06005745 RID: 22341 RVA: 0x002206D4 File Offset: 0x0021E8D4
		private static RectTransform Rect(string name, Transform parent, float x, float y, float width, float height)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			Vector2 vector = new Vector2(0f, 1f);
			component.pivot = vector;
			component.anchorMin = (component.anchorMax = vector);
			component.anchoredPosition = new Vector2(x, -y);
			component.sizeDelta = new Vector2(width, height);
			return component;
		}

		// Token: 0x040057A7 RID: 22439
		private static readonly Color Pink = new Color(1f, 0.24f, 0.59f);

		// Token: 0x040057A8 RID: 22440
		private static readonly Color White = new Color(0.99f, 0.95f, 1f);

		// Token: 0x040057A9 RID: 22441
		private static readonly Color Muted = new Color(0.79f, 0.72f, 0.83f);

		// Token: 0x040057AA RID: 22442
		private static readonly Color Surface = new Color(0.055f, 0.028f, 0.078f, 0.96f);

		// Token: 0x040057AB RID: 22443
		private const string Art = "Tutorials/Art/";
	}
}
