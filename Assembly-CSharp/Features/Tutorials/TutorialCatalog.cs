using System;
using System.Collections.Generic;
using Features.CielOS.UI;

namespace Features.Tutorials
{
	// Token: 0x02000BD1 RID: 3025
	public static class TutorialCatalog
	{
		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06005709 RID: 22281 RVA: 0x0021D6BD File Offset: 0x0021B8BD
		public static IReadOnlyList<TutorialCategory> Categories
		{
			get
			{
				return TutorialCatalog.CategoryData;
			}
		}

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x0600570A RID: 22282 RVA: 0x0021D6C4 File Offset: 0x0021B8C4
		public static IReadOnlyList<TutorialTopic> Topics
		{
			get
			{
				return TutorialCatalog.TopicData;
			}
		}

		// Token: 0x0600570B RID: 22283 RVA: 0x0021D6CC File Offset: 0x0021B8CC
		private static TutorialTopic Topic(string id, string category, string title, string kicker, string summary, string description, string glyph, string[] bindings, string tip, TutorialVisualKind visualKind, string visualResource, CielOSIconId icon, string action, string feedback, params string[] visualCaptions)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(id);
			string[] array;
			if (num <= 2248688428U)
			{
				if (num <= 278748797U)
				{
					if (num != 5149376U)
					{
						if (num != 89534040U)
						{
							if (num == 278748797U)
							{
								if (id == "heartbeat_orgasm")
								{
									array = new string[] { "Tutorials/Guide/heartbeat_orgasm" };
									visualCaptions = new string[] { "HUD detail — heartbeat and climax build-up are different signals" };
									goto IL_0486;
								}
							}
						}
						else if (id == "movement")
						{
							array = new string[] { "Tutorials/Guide/movement" };
							visualCaptions = new string[] { "Movement keys — walk, run and keep your current pace" };
							goto IL_0486;
						}
					}
					else if (id == "poi_travel")
					{
						array = new string[] { "Tutorials/Guide/poi_travel" };
						visualCaptions = new string[] { "Use an entrance to visit a separate area, then return to the city" };
						goto IL_0486;
					}
				}
				else if (num <= 1029965593U)
				{
					if (num != 630905185U)
					{
						if (num == 1029965593U)
						{
							if (id == "rapid_actions")
							{
								array = new string[] { "Tutorials/Art/project_exhibitionist_background" };
								visualCaptions = new string[] { "Three action groups — tap to use, hold to choose" };
								goto IL_0486;
							}
						}
					}
					else if (id == "cielhome_pc")
					{
						array = new string[] { "Tutorials/Guide/cielhome_pc" };
						visualCaptions = new string[] { "CielHome desk — use the PC to prepare and start your run" };
						goto IL_0486;
					}
				}
				else if (num != 2190321876U)
				{
					if (num == 2248688428U)
					{
						if (id == "detection")
						{
							array = new string[] { "Tutorials/Art/project_exhibitionist_background" };
							visualCaptions = new string[] { "Notice, report, pursuit — react before attention escalates" };
							goto IL_0486;
						}
					}
				}
				else if (id == "vehicles")
				{
					array = new string[] { "Tutorials/Guide/vehicles" };
					visualCaptions = new string[] { "Vehicle use — mount, drive and watch the remaining time" };
					goto IL_0486;
				}
			}
			else if (num <= 2930960964U)
			{
				if (num <= 2848265721U)
				{
					if (num != 2662856321U)
					{
						if (num == 2848265721U)
						{
							if (id == "introduction")
							{
								array = new string[] { "Tutorials/Art/project_exhibitionist_background" };
								goto IL_0486;
							}
						}
					}
					else if (id == "looting")
					{
						array = new string[] { "Tutorials/Guide/looting" };
						visualCaptions = new string[] { "Search progress — hold E and watch the drop stages" };
						goto IL_0486;
					}
				}
				else if (num != 2902356131U)
				{
					if (num == 2930960964U)
					{
						if (id == "minimap")
						{
							array = new string[] { "Tutorials/Guide/minimap" };
							visualCaptions = new string[] { "Expanded map — destinations, position and navigation" };
							goto IL_0486;
						}
					}
				}
				else if (id == "mood_switch")
				{
					array = new string[] { "Tutorials/Art/ciel_emb" };
					visualCaptions = new string[] { "Embarrassed and Confident — press Tab to switch Ciel's mood" };
					goto IL_0486;
				}
			}
			else if (num <= 3526332565U)
			{
				if (num != 3508635376U)
				{
					if (num == 3526332565U)
					{
						if (id == "score")
						{
							array = new string[] { "Tutorials/Guide/score" };
							visualCaptions = new string[] { "Reward detail — the label explains why you earned Score" };
							goto IL_0486;
						}
					}
				}
				else if (id == "item_types")
				{
					array = new string[] { "Tutorials/Guide/item_types", "Tutorials/Guide/item_types_2" };
					visualCaptions = new string[] { "Cover items — use the prop and follow its action prompt", "Selected item — read the available contextual action" };
					goto IL_0486;
				}
			}
			else if (num != 4244489279U)
			{
				if (num == 4270009365U)
				{
					if (id == "perspective")
					{
						array = new string[] { "Tutorials/Art/perspective_third", "Tutorials/Art/perspective_first" };
						visualCaptions = new string[] { "Third Person — keep Ciel and her surroundings in view", "First Person — see from Ciel's viewpoint" };
						goto IL_0486;
					}
				}
			}
			else if (id == "inventory")
			{
				array = new string[] { "Tutorials/Guide/inventory", "Tutorials/Art/wardrobe_inventory", "Tutorials/Guide/inventory_3" };
				visualCaptions = new string[] { "Items — select a slot and read its details", "Clothing — a separate six-slot collection", "Equipped outfit — see the change on Ciel" };
				goto IL_0486;
			}
			array = new string[] { "Tutorials/Guide/" + id };
			IL_0486:
			string[] array2;
			if (!string.IsNullOrWhiteSpace(tip))
			{
				(array2 = new string[1])[0] = tip;
			}
			else
			{
				array2 = Array.Empty<string>();
			}
			return new TutorialTopic(id, category, title, kicker, summary, description, glyph, bindings, array2, visualKind, visualResource, icon, action, feedback, visualCaptions, array);
		}

		// Token: 0x0600570C RID: 22284 RVA: 0x0021DB98 File Offset: 0x0021BD98
		public static bool TryGetTopic(string id, out TutorialTopic topic)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				return TutorialCatalog.ById.TryGetValue(id, out topic);
			}
			topic = null;
			return false;
		}

		// Token: 0x0600570D RID: 22285 RVA: 0x0021DBB4 File Offset: 0x0021BDB4
		public static TutorialTopic GetFirstInCategory(string categoryId)
		{
			for (int i = 0; i < TutorialCatalog.TopicData.Length; i++)
			{
				if (TutorialCatalog.TopicData[i].CategoryId == categoryId)
				{
					return TutorialCatalog.TopicData[i];
				}
			}
			return null;
		}

		// Token: 0x0600570E RID: 22286 RVA: 0x0021DBF0 File Offset: 0x0021BDF0
		public static IEnumerable<TutorialTopic> GetTopicsInCategory(string categoryId)
		{
			int num;
			for (int i = 0; i < TutorialCatalog.TopicData.Length; i = num + 1)
			{
				if (TutorialCatalog.TopicData[i].CategoryId == categoryId)
				{
					yield return TutorialCatalog.TopicData[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600570F RID: 22287 RVA: 0x0021DC00 File Offset: 0x0021BE00
		private static Dictionary<string, TutorialTopic> BuildIndex()
		{
			Dictionary<string, TutorialTopic> dictionary = new Dictionary<string, TutorialTopic>(StringComparer.Ordinal);
			for (int i = 0; i < TutorialCatalog.TopicData.Length; i++)
			{
				dictionary[TutorialCatalog.TopicData[i].Id] = TutorialCatalog.TopicData[i];
			}
			return dictionary;
		}

		// Token: 0x0400578A RID: 22410
		public const string ControlsCategory = "controls";

		// Token: 0x0400578B RID: 22411
		public const string HudGearCategory = "hud_gear";

		// Token: 0x0400578C RID: 22412
		public const string WorldCategory = "world";

		// Token: 0x0400578D RID: 22413
		public const string RiskCategory = "risk_progression";

		// Token: 0x0400578E RID: 22414
		public const string HomeCategory = "cielhome";

		// Token: 0x0400578F RID: 22415
		public const string BasicsCategory = "controls";

		// Token: 0x04005790 RID: 22416
		public const string ToolsCategory = "hud_gear";

		// Token: 0x04005791 RID: 22417
		public const string ExpressionCategory = "hud_gear";

		// Token: 0x04005792 RID: 22418
		private static readonly TutorialCategory[] CategoryData = new TutorialCategory[]
		{
			new TutorialCategory("controls", "CONTROLS", "MOVE, LOOK, REACT", "01"),
			new TutorialCategory("hud_gear", "HUD & GEAR", "READ WHAT THE GAME SHOWS", "02"),
			new TutorialCategory("world", "WORLD", "USE THE CITY", "03"),
			new TutorialCategory("risk_progression", "RISK & PROGRESSION", "ATTENTION BECOMES OPPORTUNITY", "04"),
			new TutorialCategory("cielhome", "CIELHOME", "PREPARE AND START A RUN", "05")
		};

		// Token: 0x04005793 RID: 22419
		private static readonly TutorialTopic[] TopicData = new TutorialTopic[]
		{
			TutorialCatalog.Topic("introduction", "controls", "Welcome to Project Exhibitionist", "MAKE YOURSELF AT HOME", "Project Exhibitionist is a game about exhibitionism, with plenty of self-humiliation along the way.", "Complete the main story to fully unlock the procedurally generated map. Then explore freely or compete in Ranked mode.", "01", new string[] { "E - USE THE HOME COMPUTER", "ESC - PAUSE / TUTORIAL" }, "Open Run Modes, choose Classic, then Continue Classic.", TutorialVisualKind.CielOsDesktop, null, CielOSIconId.Play, "Walk to the desk and press E to use the PC.", "Open Story and read the highlighted chapter.", new string[] { "Welcome to Project Exhibitionist" }),
			TutorialCatalog.Topic("movement", "controls", "Basic movement", "MOVE WITHOUT THINKING", "Move, run and keep an eye on stamina.", "Use WASD to walk through the world and hold Shift whenever you need to run. Running consumes stamina: the HUD bar drops while Ciel pushes herself and begins recovering after you slow down. Press Caps Lock while moving to keep the current pace automatically; any new directional input returns control to you immediately.", "WASD", new string[] { "WASD  •  MOVE", "SHIFT  •  RUN", "CAPS LOCK  •  AUTO WALK / RUN" }, "Any new WASD input cancels automatic movement.", TutorialVisualKind.MovementHud, "Tutorials/movement", CielOSIconId.Play, "WASD to move. Hold Shift to run. Caps Lock starts AutoWalk or AutoRun.", "Watch the real stamina and auto-movement widgets: they show energy, mode and speed.", new string[] { "Walking through the city", "Running with the stamina HUD visible", "AutoWalk or AutoRun active" }),
			TutorialCatalog.Topic("crouch_actions", "controls", "Crouch actions", "HOLD C TO CHANGE ACTION", "Tap C to use the selected action. Hold C to choose another.", "Keep C held down to open the action wheel. While still holding C, point at Crouch, Crawl or Prone, then release C to select it. A quick tap of C performs the selected action; tap again to stop. Tapping alone does not change the selected action.", "C", new string[] { "HOLD C - OPEN ACTION WHEEL", "MOUSE - CHOOSE ACTION", "RELEASE C - SELECT", "TAP C - USE / STOP" }, "Keep the key pressed until the wheel appears, then release it over your choice.", TutorialVisualKind.RapidActionsHud, null, CielOSIconId.Play, "Hold C, point at an action and release C. Tap C to use it.", "The wheel changes the C action shown in the HUD; a tap starts or stops it.", new string[] { "Hold C to choose Crouch, Crawl or Prone" }),
			TutorialCatalog.Topic("photo_mode", "controls", "Photo Mode", "FRAME THE MOMENT", "Pause the scene and move the camera freely to take a photo.", "Press K when camera control is available to enter Photo Mode. The scene pauses while you frame the shot. Use WASD to move, Q and E to move down and up, and the mouse to look around. Hold Shift to move faster or use the mouse wheel to adjust speed. Press Space to save a photo, then K again to return to Ciel.", "K", new string[] { "K - PHOTO MODE / RETURN", "WASD / Q / E - MOVE CAMERA", "MOUSE - LOOK", "SPACE - TAKE PHOTO" }, "Your photo is saved when you press Space. K returns to your previous gameplay view.", TutorialVisualKind.Perspective, null, CielOSIconId.Photos, "Press K, frame a shot, press Space and use K to return.", "The scene freezes, the free camera moves independently, and the shutter confirms the photo.", new string[] { "Photo Mode viewfinder and free camera controls" }),
			new TutorialTopic("smartphone", "hud_gear", "Smartphone camera", "CAPTURE YOUR VIEW", "Open the phone and use its Camera app.", "Press P to take out the phone, then select Camera. Use Space to take a photo, the mouse wheel to zoom and F to change the filter. R switches between the front and rear camera. Hold 2 to choose a facial expression. Press X to leave the camera or P to put the phone away.", "P", new string[] { "P - PHONE / PUT AWAY", "CAMERA - OPEN APP", "SPACE - TAKE PHOTO", "R - FRONT / REAR CAMERA", "HOLD 2 - FACIAL EXPRESSIONS", "X - EXIT CAMERA" }, new string[] { "Put away any item held in Ciel's hand before opening the phone." }, TutorialVisualKind.Perspective, null, CielOSIconId.Photos, "Press P, select Camera and press Space to take a photo.", "The phone opens; Camera shows the live preview and shutter controls.", new string[] { "Selfie camera with facial expressions and shutter controls" }, new string[] { "Tutorials/Guide/smartphone_2" }),
			TutorialCatalog.Topic("perspective", "controls", "Third / First Person", "CHOOSE YOUR VIEW", "Choose the view that fits the moment.", "Third Person keeps Ciel, nearby NPCs and possible cover visible, which makes it easier to read busy streets. First Person places the camera at Ciel's viewpoint for precise movement and close interactions. Press V while camera control is available; the viewpoint changes without cancelling your movement or changing direction.", "V", new string[] { "V  •  SWITCH POV" }, "Use third person around crowds and first person for close interactions.", TutorialVisualKind.Perspective, "Tutorials/camera", CielOSIconId.Photos, "Press V to switch between Third and First Person.", "The camera moves between Ciel's third-person and first-person viewpoints.", new string[] { "Third Person while running through the city", "First Person during the same run" }),
			TutorialCatalog.Topic("rapid_actions", "controls", "Rapid Actions", "TAP TO ACT · HOLD TO CHOOSE", "Use poses and emotes without opening a menu.", "Q and Z perform the assigned emotes. C performs Crouch, Crawl or Prone. Right Mouse uses the assigned arms/cover pose. Hold any slot input to open its real radial selector.", "Q Z C RMB", new string[] { "Q / Z  •  EMOTES", "C  •  CROUCH / CRAWL / PRONE", "RMB  •  COVER POSE" }, "Tap executes. Hold briefly to change what that slot does.", TutorialVisualKind.RapidActionsHud, null, CielOSIconId.Grid, "Q/Z: emotes. C: crouch, prone or crawl. RMB: cover pose.", "The slot label identifies the action; holding the key opens its selection wheel.", Array.Empty<string>()),
			TutorialCatalog.Topic("minimap", "hud_gear", "Minimap", "POSITION FIRST", "Read your position and find nearby destinations.", "Press M for the expanded real map. Scroll to zoom, drag while zoomed to explore and use Middle Mouse to place or remove a pin.", "M", new string[] { "M  •  EXPAND", "WHEEL / DRAG  •  INSPECT", "MMB  •  MAP PIN" }, "POI icons become useful references as you discover the city.", TutorialVisualKind.MinimapHud, null, CielOSIconId.MapPin, "Press M. Scroll to zoom, drag to move, MMB to place a pin.", "The real minimap enlarges, keeps Ciel's arrow visible and reveals its POI legend.", Array.Empty<string>()),
			TutorialCatalog.Topic("heartbeat_orgasm", "hud_gear", "Heartbeat & climax meter", "READ CIEL'S BODY", "Read Ciel's intensity and build-up at a glance.", "Running, exposure, attention and stimulating actions can speed up Ciel's heartbeat. Calming down, hiding or stopping lets it settle. During stimulating actions the climax meter builds; stronger or faster stimulation builds it more quickly and stopping lets it cool.", "♥", new string[] { "HEART  •  CURRENT INTENSITY", "METER  •  BUILD-UP", "CTRL + WHEEL  •  ACTION SPEED WHEN SHOWN" }, "Follow the pulse, colour and fill; you do not need the numbers.", TutorialVisualKind.HeartbeatHud, null, CielOSIconId.Heart, "Risk and stimulation raise them. Slowing down or hiding lets them settle.", "The heart reacts quickly; the meter rises only during actions that build toward a climax.", Array.Empty<string>()),
			TutorialCatalog.Topic("inventory", "hud_gear", "Inventory", "ITEMS AND CLOTHING ARE SEPARATE", "Items and clothing have separate, simple actions.", "Press I to open the inventory. Items and clothing are kept in separate sections so their actions remain clear. Item slots use 1, 2 and 3; clothing has its own six-slot collection, with 4 to focus it and Space to cycle. Select a filled slot to read its details, click the available action, drag to reorder or swap, and drag outside when the interface offers a drop action. Clothing changes are reflected on Ciel as soon as they are equipped.", "I", new string[] { "I  •  OPEN", "1 / 2 / 3  •  ITEM SLOTS", "4 + SPACE  •  CLOTHING FOCUS / CYCLE" }, "The ring or bar on a slot is its remaining durability.", TutorialVisualKind.InventoryHud, null, CielOSIconId.Grid, "Press I. Select a slot; click for details or drag to move it.", "The details panel explains it; drag-and-drop performs the visible inventory action.", new string[] { "Items stored in the inventory", "Clothing collection with filled slots", "Ciel wearing a different outfit" }),
			TutorialCatalog.Topic("mood_switch", "hud_gear", "Mood Switch", "EMBARRASSED ↔ CONFIDENT", "Switch how Ciel reacts and carries herself.", "Press Tab to switch between Embarrassed and Confident. The real mood widget and Ciel's body language change together. It changes presentation and available reactions, not whether danger exists.", "TAB", new string[] { "TAB  •  SWITCH MOOD" }, "Check the mood label before a cover pose or risky emote.", TutorialVisualKind.MoodHud, null, CielOSIconId.Avatar, "Press Tab to switch between Embarrassed and Confident.", "The active mood in the HUD flips and Ciel immediately adopts its posture.", Array.Empty<string>()),
			TutorialCatalog.Topic("item_types", "hud_gear", "Item types", "READ THE ACTION, NOT JUST THE ICON", "Each item type unlocks a different action.", "Cover items hide Ciel behind a prop and can enable Flash. Stimulation items start a dedicated controllable action. Wearables change clothing or perks; vehicle items deploy transport. The selected slot always shows the action currently available.", "1 2 3", new string[] { "COVER ITEM  •  HIDE / FLASH", "STIMULATION ITEM  •  USE / SPEED", "WEARABLE / VEHICLE  •  EQUIP / DEPLOY" }, "Read the contextual action HUD after selecting an unfamiliar item.", TutorialVisualKind.ItemTypesHud, null, CielOSIconId.Files, "Select with 1, 2 or 3, then follow the action shown on screen.", "The real action icon and prompt change to match that item's mechanic.", Array.Empty<string>()),
			new TutorialTopic("world_cover", "world", "Hiding spots", "FIND COVER AROUND THE CITY", "Small bins, large dumpsters and lockers can give Ciel a place to hide.", "Look for small bins, large dumpsters and lockers around the map. Approach a hiding spot and follow its Hide prompt. Press F to enter a small bin, or E to enter a large dumpster or locker. Wait for Ciel to finish climbing inside. When you are ready to leave, press F again in a small bin, or E again in a dumpster or locker.", "E / F", new string[] { "F - SMALL BIN: HIDE / EXIT", "E - DUMPSTER OR LOCKER: HIDE / EXIT" }, new string[] { "Walls also offer Wall Cover: follow the shown interaction and crouch prompts. Look for the Hide prompt before entering. You can reread this guide from the pause menu's Tutorial library." }, TutorialVisualKind.WorldCover, null, CielOSIconId.Play, "Find a hiding spot, approach its entrance and press the key shown by Hide.", "Ciel climbs inside. Once the entry finishes, the exit prompt shows how to leave.", new string[] { "Small bin - F to hide and exit", "Large dumpster - E to hide and exit", "Locker - E to hide and exit" }, new string[] { "Tutorials/Guide/hiding_bin", "Tutorials/Guide/hiding_dumpster", "Tutorials/Guide/hiding_locker" }),
			TutorialCatalog.Topic("looting", "world", "Looting trashcans", "HOLD, READ, COLLECT", "Search trashcans and judge what you find.", "Hold E on a lootable trashcan. Its real progress bar fills through the search and rarity markers show when drops can appear. Common, Rare and Legendary colours carry from the world aura into inventory. Durability shows how long that found instance lasts.", "HOLD E", new string[] { "HOLD E  •  RUMMAGE", "COLOUR  •  RARITY", "RING / BAR  •  DURABILITY" }, "Found rarity can also make an item or garment last longer.", TutorialVisualKind.LootHud, null, CielOSIconId.Search, "Hold E to search. Colour shows rarity; the ring shows durability.", "Loot appears with its rarity colour; inventory keeps that colour and durability.", Array.Empty<string>()),
			TutorialCatalog.Topic("return_home", "world", "Returning home", "YOUR FRONT DOOR", "In Classic, you can end a run by returning to Ciel's house.", "CLASSIC\nCome back to your front door and press E whenever you want to end the run. The prompt appears after the city intro, once you have moved away from where you started. The key lights up when you can use it.\n\nYour run ends when you enter. You keep your run score. If you reached at least 100 m from home, you also earn 1 extra point for each whole metre of your furthest distance. Returning earlier grants no return bonus. Walking back and forth does not increase this bonus. Return missions are counted at the door after an outing of at least 100 m; XP and ranked results are saved normally.\n\nYou must be on foot and free to interact. Finish any ongoing action or encounter first. You cannot enter while being caught, during a detection transition, or while recovering. A capture takes priority over returning home.\n\nLONG WAY HOME\nReaching the entrance completes this mode automatically. The Classic distance bonus and return missions do not apply. Free mode has no Classic return bonus.", "E", new string[] { "E - RETURN HOME IN CLASSIC" }, "The return bonus uses your furthest point from home, not the total distance walked.", TutorialVisualKind.PoiTravel, null, CielOSIconId.Play, "Classic: return to the door and press E. Reach 100 m to earn a return bonus.", "Check your score and bonus, then continue to CielHome.", new string[] { "Return to Ciel's front door. The E key lights up when you can enter." }),
			TutorialCatalog.Topic("poi_travel", "world", "POIs & travel", "ONE CITY, TWO TYPES OF PLACE", "Some POIs are in the city; larger ones load separately.", "Some locations are part of the city: reach their icon and keep exploring without leaving the map. Other locations have an entrance that loads a separate area. Use E at the entrance to enter; inside, use Return to City to load the main map at the saved return point.", "E", new string[] { "CITY LOCATIONS  •  EXPLORE IN-MAP", "ENTRANCES  •  SEPARATE AREAS", "E  •  ENTER / RETURN" }, "The minimap finds the entrance; the portal prompt performs travel.", TutorialVisualKind.PoiTravel, null, CielOSIconId.MapPin, "Reach a location's entrance and press E. Use Return to City to leave.", "The separate area loads, and returning restores Ciel at the saved point on the city map.", Array.Empty<string>()),
			TutorialCatalog.Topic("score", "risk_progression", "Score & EXP", "PLAY BOLD, ESCAPE SMART", "Bold actions earn Score; completed runs turn it into EXP.", "Score comes from witnessed exhibition actions, new POIs or crowds, escaping attention or special pursuers, effective hiding and vehicle distance. The real score pop-up names the reward; the run recap converts the result into progression.", "+", new string[] { "ACTION + WITNESS  •  SCORE", "ESCAPE / HIDE  •  SCORE", "RUN RECAP  •  EXP" }, "A new situation is worth more than farming the same safe action.", TutorialVisualKind.ScoreHud, null, CielOSIconId.LevelUp, "Take a risk, escape attention or discover something new.", "Its label names the reason; total score and EXP are summarized after the run.", Array.Empty<string>()),
			TutorialCatalog.Topic("detection", "risk_progression", "Detection & Wanted level", "NOTICE → REPORT → PURSUIT", "Stay unseen, lower suspicion and control Wanted pressure.", "More covering clothing and useful perks can slow or alter detection; exposure and conspicuous actions make you easier to notice. Break sight, cover or hide to cool individual attention. Repeated detections raise Wanted: civilians can report, reporters pressure you and police become the main threat. Wanted Posters and perks can counter that pressure.", "!", new string[] { "ATTENTION MARKER  •  WHO SEES YOU", "WANTED HUD  •  CITY PRESSURE", "COVER / POSTERS / PERKS  •  COUNTERS" }, "React to the first attention marker; waiting for police costs options.", TutorialVisualKind.DetectionHud, null, CielOSIconId.Bell, "When attention rises, break sight or hide before detection completes.", "That NPC cools; repeated completed detections instead escalate reporters and police.", Array.Empty<string>()),
			TutorialCatalog.Topic("vehicles", "world", "Vehicles", "FASTER, TEMPORARY MOVEMENT", "Vehicles are faster, but every ride has a time limit.", "Use the vehicle's E prompt to mount or deploy it, then follow the real vehicle HUD for acceleration, steering, special action and dismount. Every usable vehicle has a maximum use duration: its timer or durability ring shows what remains.", "E", new string[] { "E  •  MOUNT / DISMOUNT", "WASD  •  DRIVE", "TIMER / RING  •  USE REMAINING" }, "Vehicle-specific controls shown on screen are authoritative.", TutorialVisualKind.VehiclesHud, null, CielOSIconId.StartRun, "Press E to mount. Drive with WASD and watch the remaining-use timer.", "The real vehicle HUD appears with type-specific actions and remaining use.", Array.Empty<string>()),
			TutorialCatalog.Topic("cielhome_pc", "cielhome", "CielHome PC", "PREPARE EVERYTHING, THEN START", "CielOS is where you prepare Ciel, the city and every new run.", "Approach the desk and press E to enter CielOS. Use the desktop icons, dock or app menu to open applications. Apps open in windows: select a window to bring it forward, use its title-bar controls to minimize or close it, and press ESC when you want to leave the computer and return to the room.\n\nThe preparation apps affect the game outside the PC. Customization changes Ciel's appearance. NPC customization controls the presets used by people in the city. Dialogues lets you manage the lines available to civilians, reporters and police. Rank collects progression, missions and unlocks. Run Modes is the final step: choose Classic or Free Mode, read the available options and confirm Start only when the setup is ready.", "E", new string[] { "E  •  USE THE PC / OPEN CIELOS", "DESKTOP / DOCK  •  FIND AND OPEN APPS", "CUSTOMIZE CIEL  •  PLAYER APPEARANCE", "CUSTOMIZE NPCS  •  CITY CHARACTER PRESETS", "DIALOGUES  •  CIVILIAN / REPORTER / POLICE LINES", "RANK  •  PROGRESSION / MISSIONS / UNLOCKS", "RUN MODES  •  CLASSIC / FREE / START RUN", "ESC  •  CLOSE CIELOS AND RETURN TO THE ROOM" }, "Nothing starts automatically: changes remain available while you review other apps, and the run begins only after you confirm it in Run Modes.", TutorialVisualKind.CielOsDesktop, "Tutorials/home", CielOSIconId.RunModes, "Press E at the desk. Review Customization, NPCs, Dialogues and Rank, then open Run Modes.", "CielOS keeps the chosen setup. Confirming a valid mode closes the computer and starts the loading transition.", new string[] { "Using the PC in CielHome", "Reviewing the important CielOS apps", "Choosing and confirming a run mode" })
		};

		// Token: 0x04005794 RID: 22420
		private static readonly Dictionary<string, TutorialTopic> ById = TutorialCatalog.BuildIndex();
	}
}
