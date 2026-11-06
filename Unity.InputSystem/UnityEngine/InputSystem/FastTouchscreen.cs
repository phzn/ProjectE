using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000044 RID: 68
	internal class FastTouchscreen : Touchscreen
	{
		// Token: 0x06000680 RID: 1664 RVA: 0x0001E32C File Offset: 0x0001C52C
		public FastTouchscreen()
		{
			InputControlExtensions.DeviceBuilder deviceBuilder = this.Setup(302, 5, 0).WithName("Touchscreen").WithDisplayName("Touchscreen")
				.WithChildren(0, 17)
				.WithLayout(new InternedString("Touchscreen"))
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1414742866),
					sizeInBits = 4928U
				});
			InternedString internedString = new InternedString("Touch");
			InternedString internedString2 = new InternedString("Vector2");
			InternedString internedString3 = new InternedString("Delta");
			InternedString internedString4 = new InternedString("Analog");
			InternedString internedString5 = new InternedString("TouchPress");
			InternedString internedString6 = new InternedString("Integer");
			InternedString internedString7 = new InternedString("Axis");
			InternedString internedString8 = new InternedString("TouchPhase");
			InternedString internedString9 = new InternedString("Button");
			InternedString internedString10 = new InternedString("Double");
			TouchControl touchControl = this.Initialize_ctrlTouchscreenprimaryTouch(internedString, this);
			Vector2Control vector2Control = this.Initialize_ctrlTouchscreenposition(internedString2, this);
			DeltaControl deltaControl = this.Initialize_ctrlTouchscreendelta(internedString3, this);
			AxisControl axisControl = this.Initialize_ctrlTouchscreenpressure(internedString4, this);
			Vector2Control vector2Control2 = this.Initialize_ctrlTouchscreenradius(internedString2, this);
			TouchPressControl touchPressControl = this.Initialize_ctrlTouchscreenpress(internedString5, this);
			IntegerControl integerControl = this.Initialize_ctrlTouchscreendisplayIndex(internedString6, this);
			TouchControl touchControl2 = this.Initialize_ctrlTouchscreentouch0(internedString, this);
			TouchControl touchControl3 = this.Initialize_ctrlTouchscreentouch1(internedString, this);
			TouchControl touchControl4 = this.Initialize_ctrlTouchscreentouch2(internedString, this);
			TouchControl touchControl5 = this.Initialize_ctrlTouchscreentouch3(internedString, this);
			TouchControl touchControl6 = this.Initialize_ctrlTouchscreentouch4(internedString, this);
			TouchControl touchControl7 = this.Initialize_ctrlTouchscreentouch5(internedString, this);
			TouchControl touchControl8 = this.Initialize_ctrlTouchscreentouch6(internedString, this);
			TouchControl touchControl9 = this.Initialize_ctrlTouchscreentouch7(internedString, this);
			TouchControl touchControl10 = this.Initialize_ctrlTouchscreentouch8(internedString, this);
			TouchControl touchControl11 = this.Initialize_ctrlTouchscreentouch9(internedString, this);
			IntegerControl integerControl2 = this.Initialize_ctrlTouchscreenprimaryTouchtouchId(internedString6, touchControl);
			Vector2Control vector2Control3 = this.Initialize_ctrlTouchscreenprimaryTouchposition(internedString2, touchControl);
			DeltaControl deltaControl2 = this.Initialize_ctrlTouchscreenprimaryTouchdelta(internedString3, touchControl);
			AxisControl axisControl2 = this.Initialize_ctrlTouchscreenprimaryTouchpressure(internedString7, touchControl);
			Vector2Control vector2Control4 = this.Initialize_ctrlTouchscreenprimaryTouchradius(internedString2, touchControl);
			TouchPhaseControl touchPhaseControl = this.Initialize_ctrlTouchscreenprimaryTouchphase(internedString8, touchControl);
			TouchPressControl touchPressControl2 = this.Initialize_ctrlTouchscreenprimaryTouchpress(internedString5, touchControl);
			IntegerControl integerControl3 = this.Initialize_ctrlTouchscreenprimaryTouchtapCount(internedString6, touchControl);
			IntegerControl integerControl4 = this.Initialize_ctrlTouchscreenprimaryTouchdisplayIndex(internedString6, touchControl);
			ButtonControl buttonControl = this.Initialize_ctrlTouchscreenprimaryTouchindirectTouch(internedString9, touchControl);
			ButtonControl buttonControl2 = this.Initialize_ctrlTouchscreenprimaryTouchtap(internedString9, touchControl);
			DoubleControl doubleControl = this.Initialize_ctrlTouchscreenprimaryTouchstartTime(internedString10, touchControl);
			Vector2Control vector2Control5 = this.Initialize_ctrlTouchscreenprimaryTouchstartPosition(internedString2, touchControl);
			AxisControl axisControl3 = this.Initialize_ctrlTouchscreenprimaryTouchpositionx(internedString7, vector2Control3);
			AxisControl axisControl4 = this.Initialize_ctrlTouchscreenprimaryTouchpositiony(internedString7, vector2Control3);
			AxisControl axisControl5 = this.Initialize_ctrlTouchscreenprimaryTouchdeltaup(internedString7, deltaControl2);
			AxisControl axisControl6 = this.Initialize_ctrlTouchscreenprimaryTouchdeltadown(internedString7, deltaControl2);
			AxisControl axisControl7 = this.Initialize_ctrlTouchscreenprimaryTouchdeltaleft(internedString7, deltaControl2);
			AxisControl axisControl8 = this.Initialize_ctrlTouchscreenprimaryTouchdeltaright(internedString7, deltaControl2);
			AxisControl axisControl9 = this.Initialize_ctrlTouchscreenprimaryTouchdeltax(internedString7, deltaControl2);
			AxisControl axisControl10 = this.Initialize_ctrlTouchscreenprimaryTouchdeltay(internedString7, deltaControl2);
			AxisControl axisControl11 = this.Initialize_ctrlTouchscreenprimaryTouchradiusx(internedString7, vector2Control4);
			AxisControl axisControl12 = this.Initialize_ctrlTouchscreenprimaryTouchradiusy(internedString7, vector2Control4);
			AxisControl axisControl13 = this.Initialize_ctrlTouchscreenprimaryTouchstartPositionx(internedString7, vector2Control5);
			AxisControl axisControl14 = this.Initialize_ctrlTouchscreenprimaryTouchstartPositiony(internedString7, vector2Control5);
			AxisControl axisControl15 = this.Initialize_ctrlTouchscreenpositionx(internedString7, vector2Control);
			AxisControl axisControl16 = this.Initialize_ctrlTouchscreenpositiony(internedString7, vector2Control);
			AxisControl axisControl17 = this.Initialize_ctrlTouchscreendeltaup(internedString7, deltaControl);
			AxisControl axisControl18 = this.Initialize_ctrlTouchscreendeltadown(internedString7, deltaControl);
			AxisControl axisControl19 = this.Initialize_ctrlTouchscreendeltaleft(internedString7, deltaControl);
			AxisControl axisControl20 = this.Initialize_ctrlTouchscreendeltaright(internedString7, deltaControl);
			AxisControl axisControl21 = this.Initialize_ctrlTouchscreendeltax(internedString7, deltaControl);
			AxisControl axisControl22 = this.Initialize_ctrlTouchscreendeltay(internedString7, deltaControl);
			AxisControl axisControl23 = this.Initialize_ctrlTouchscreenradiusx(internedString7, vector2Control2);
			AxisControl axisControl24 = this.Initialize_ctrlTouchscreenradiusy(internedString7, vector2Control2);
			IntegerControl integerControl5 = this.Initialize_ctrlTouchscreentouch0touchId(internedString6, touchControl2);
			Vector2Control vector2Control6 = this.Initialize_ctrlTouchscreentouch0position(internedString2, touchControl2);
			DeltaControl deltaControl3 = this.Initialize_ctrlTouchscreentouch0delta(internedString3, touchControl2);
			AxisControl axisControl25 = this.Initialize_ctrlTouchscreentouch0pressure(internedString7, touchControl2);
			Vector2Control vector2Control7 = this.Initialize_ctrlTouchscreentouch0radius(internedString2, touchControl2);
			TouchPhaseControl touchPhaseControl2 = this.Initialize_ctrlTouchscreentouch0phase(internedString8, touchControl2);
			TouchPressControl touchPressControl3 = this.Initialize_ctrlTouchscreentouch0press(internedString5, touchControl2);
			IntegerControl integerControl6 = this.Initialize_ctrlTouchscreentouch0tapCount(internedString6, touchControl2);
			IntegerControl integerControl7 = this.Initialize_ctrlTouchscreentouch0displayIndex(internedString6, touchControl2);
			ButtonControl buttonControl3 = this.Initialize_ctrlTouchscreentouch0indirectTouch(internedString9, touchControl2);
			ButtonControl buttonControl4 = this.Initialize_ctrlTouchscreentouch0tap(internedString9, touchControl2);
			DoubleControl doubleControl2 = this.Initialize_ctrlTouchscreentouch0startTime(internedString10, touchControl2);
			Vector2Control vector2Control8 = this.Initialize_ctrlTouchscreentouch0startPosition(internedString2, touchControl2);
			AxisControl axisControl26 = this.Initialize_ctrlTouchscreentouch0positionx(internedString7, vector2Control6);
			AxisControl axisControl27 = this.Initialize_ctrlTouchscreentouch0positiony(internedString7, vector2Control6);
			AxisControl axisControl28 = this.Initialize_ctrlTouchscreentouch0deltaup(internedString7, deltaControl3);
			AxisControl axisControl29 = this.Initialize_ctrlTouchscreentouch0deltadown(internedString7, deltaControl3);
			AxisControl axisControl30 = this.Initialize_ctrlTouchscreentouch0deltaleft(internedString7, deltaControl3);
			AxisControl axisControl31 = this.Initialize_ctrlTouchscreentouch0deltaright(internedString7, deltaControl3);
			AxisControl axisControl32 = this.Initialize_ctrlTouchscreentouch0deltax(internedString7, deltaControl3);
			AxisControl axisControl33 = this.Initialize_ctrlTouchscreentouch0deltay(internedString7, deltaControl3);
			AxisControl axisControl34 = this.Initialize_ctrlTouchscreentouch0radiusx(internedString7, vector2Control7);
			AxisControl axisControl35 = this.Initialize_ctrlTouchscreentouch0radiusy(internedString7, vector2Control7);
			AxisControl axisControl36 = this.Initialize_ctrlTouchscreentouch0startPositionx(internedString7, vector2Control8);
			AxisControl axisControl37 = this.Initialize_ctrlTouchscreentouch0startPositiony(internedString7, vector2Control8);
			IntegerControl integerControl8 = this.Initialize_ctrlTouchscreentouch1touchId(internedString6, touchControl3);
			Vector2Control vector2Control9 = this.Initialize_ctrlTouchscreentouch1position(internedString2, touchControl3);
			DeltaControl deltaControl4 = this.Initialize_ctrlTouchscreentouch1delta(internedString3, touchControl3);
			AxisControl axisControl38 = this.Initialize_ctrlTouchscreentouch1pressure(internedString7, touchControl3);
			Vector2Control vector2Control10 = this.Initialize_ctrlTouchscreentouch1radius(internedString2, touchControl3);
			TouchPhaseControl touchPhaseControl3 = this.Initialize_ctrlTouchscreentouch1phase(internedString8, touchControl3);
			TouchPressControl touchPressControl4 = this.Initialize_ctrlTouchscreentouch1press(internedString5, touchControl3);
			IntegerControl integerControl9 = this.Initialize_ctrlTouchscreentouch1tapCount(internedString6, touchControl3);
			IntegerControl integerControl10 = this.Initialize_ctrlTouchscreentouch1displayIndex(internedString6, touchControl3);
			ButtonControl buttonControl5 = this.Initialize_ctrlTouchscreentouch1indirectTouch(internedString9, touchControl3);
			ButtonControl buttonControl6 = this.Initialize_ctrlTouchscreentouch1tap(internedString9, touchControl3);
			DoubleControl doubleControl3 = this.Initialize_ctrlTouchscreentouch1startTime(internedString10, touchControl3);
			Vector2Control vector2Control11 = this.Initialize_ctrlTouchscreentouch1startPosition(internedString2, touchControl3);
			AxisControl axisControl39 = this.Initialize_ctrlTouchscreentouch1positionx(internedString7, vector2Control9);
			AxisControl axisControl40 = this.Initialize_ctrlTouchscreentouch1positiony(internedString7, vector2Control9);
			AxisControl axisControl41 = this.Initialize_ctrlTouchscreentouch1deltaup(internedString7, deltaControl4);
			AxisControl axisControl42 = this.Initialize_ctrlTouchscreentouch1deltadown(internedString7, deltaControl4);
			AxisControl axisControl43 = this.Initialize_ctrlTouchscreentouch1deltaleft(internedString7, deltaControl4);
			AxisControl axisControl44 = this.Initialize_ctrlTouchscreentouch1deltaright(internedString7, deltaControl4);
			AxisControl axisControl45 = this.Initialize_ctrlTouchscreentouch1deltax(internedString7, deltaControl4);
			AxisControl axisControl46 = this.Initialize_ctrlTouchscreentouch1deltay(internedString7, deltaControl4);
			AxisControl axisControl47 = this.Initialize_ctrlTouchscreentouch1radiusx(internedString7, vector2Control10);
			AxisControl axisControl48 = this.Initialize_ctrlTouchscreentouch1radiusy(internedString7, vector2Control10);
			AxisControl axisControl49 = this.Initialize_ctrlTouchscreentouch1startPositionx(internedString7, vector2Control11);
			AxisControl axisControl50 = this.Initialize_ctrlTouchscreentouch1startPositiony(internedString7, vector2Control11);
			IntegerControl integerControl11 = this.Initialize_ctrlTouchscreentouch2touchId(internedString6, touchControl4);
			Vector2Control vector2Control12 = this.Initialize_ctrlTouchscreentouch2position(internedString2, touchControl4);
			DeltaControl deltaControl5 = this.Initialize_ctrlTouchscreentouch2delta(internedString3, touchControl4);
			AxisControl axisControl51 = this.Initialize_ctrlTouchscreentouch2pressure(internedString7, touchControl4);
			Vector2Control vector2Control13 = this.Initialize_ctrlTouchscreentouch2radius(internedString2, touchControl4);
			TouchPhaseControl touchPhaseControl4 = this.Initialize_ctrlTouchscreentouch2phase(internedString8, touchControl4);
			TouchPressControl touchPressControl5 = this.Initialize_ctrlTouchscreentouch2press(internedString5, touchControl4);
			IntegerControl integerControl12 = this.Initialize_ctrlTouchscreentouch2tapCount(internedString6, touchControl4);
			IntegerControl integerControl13 = this.Initialize_ctrlTouchscreentouch2displayIndex(internedString6, touchControl4);
			ButtonControl buttonControl7 = this.Initialize_ctrlTouchscreentouch2indirectTouch(internedString9, touchControl4);
			ButtonControl buttonControl8 = this.Initialize_ctrlTouchscreentouch2tap(internedString9, touchControl4);
			DoubleControl doubleControl4 = this.Initialize_ctrlTouchscreentouch2startTime(internedString10, touchControl4);
			Vector2Control vector2Control14 = this.Initialize_ctrlTouchscreentouch2startPosition(internedString2, touchControl4);
			AxisControl axisControl52 = this.Initialize_ctrlTouchscreentouch2positionx(internedString7, vector2Control12);
			AxisControl axisControl53 = this.Initialize_ctrlTouchscreentouch2positiony(internedString7, vector2Control12);
			AxisControl axisControl54 = this.Initialize_ctrlTouchscreentouch2deltaup(internedString7, deltaControl5);
			AxisControl axisControl55 = this.Initialize_ctrlTouchscreentouch2deltadown(internedString7, deltaControl5);
			AxisControl axisControl56 = this.Initialize_ctrlTouchscreentouch2deltaleft(internedString7, deltaControl5);
			AxisControl axisControl57 = this.Initialize_ctrlTouchscreentouch2deltaright(internedString7, deltaControl5);
			AxisControl axisControl58 = this.Initialize_ctrlTouchscreentouch2deltax(internedString7, deltaControl5);
			AxisControl axisControl59 = this.Initialize_ctrlTouchscreentouch2deltay(internedString7, deltaControl5);
			AxisControl axisControl60 = this.Initialize_ctrlTouchscreentouch2radiusx(internedString7, vector2Control13);
			AxisControl axisControl61 = this.Initialize_ctrlTouchscreentouch2radiusy(internedString7, vector2Control13);
			AxisControl axisControl62 = this.Initialize_ctrlTouchscreentouch2startPositionx(internedString7, vector2Control14);
			AxisControl axisControl63 = this.Initialize_ctrlTouchscreentouch2startPositiony(internedString7, vector2Control14);
			IntegerControl integerControl14 = this.Initialize_ctrlTouchscreentouch3touchId(internedString6, touchControl5);
			Vector2Control vector2Control15 = this.Initialize_ctrlTouchscreentouch3position(internedString2, touchControl5);
			DeltaControl deltaControl6 = this.Initialize_ctrlTouchscreentouch3delta(internedString3, touchControl5);
			AxisControl axisControl64 = this.Initialize_ctrlTouchscreentouch3pressure(internedString7, touchControl5);
			Vector2Control vector2Control16 = this.Initialize_ctrlTouchscreentouch3radius(internedString2, touchControl5);
			TouchPhaseControl touchPhaseControl5 = this.Initialize_ctrlTouchscreentouch3phase(internedString8, touchControl5);
			TouchPressControl touchPressControl6 = this.Initialize_ctrlTouchscreentouch3press(internedString5, touchControl5);
			IntegerControl integerControl15 = this.Initialize_ctrlTouchscreentouch3tapCount(internedString6, touchControl5);
			IntegerControl integerControl16 = this.Initialize_ctrlTouchscreentouch3displayIndex(internedString6, touchControl5);
			ButtonControl buttonControl9 = this.Initialize_ctrlTouchscreentouch3indirectTouch(internedString9, touchControl5);
			ButtonControl buttonControl10 = this.Initialize_ctrlTouchscreentouch3tap(internedString9, touchControl5);
			DoubleControl doubleControl5 = this.Initialize_ctrlTouchscreentouch3startTime(internedString10, touchControl5);
			Vector2Control vector2Control17 = this.Initialize_ctrlTouchscreentouch3startPosition(internedString2, touchControl5);
			AxisControl axisControl65 = this.Initialize_ctrlTouchscreentouch3positionx(internedString7, vector2Control15);
			AxisControl axisControl66 = this.Initialize_ctrlTouchscreentouch3positiony(internedString7, vector2Control15);
			AxisControl axisControl67 = this.Initialize_ctrlTouchscreentouch3deltaup(internedString7, deltaControl6);
			AxisControl axisControl68 = this.Initialize_ctrlTouchscreentouch3deltadown(internedString7, deltaControl6);
			AxisControl axisControl69 = this.Initialize_ctrlTouchscreentouch3deltaleft(internedString7, deltaControl6);
			AxisControl axisControl70 = this.Initialize_ctrlTouchscreentouch3deltaright(internedString7, deltaControl6);
			AxisControl axisControl71 = this.Initialize_ctrlTouchscreentouch3deltax(internedString7, deltaControl6);
			AxisControl axisControl72 = this.Initialize_ctrlTouchscreentouch3deltay(internedString7, deltaControl6);
			AxisControl axisControl73 = this.Initialize_ctrlTouchscreentouch3radiusx(internedString7, vector2Control16);
			AxisControl axisControl74 = this.Initialize_ctrlTouchscreentouch3radiusy(internedString7, vector2Control16);
			AxisControl axisControl75 = this.Initialize_ctrlTouchscreentouch3startPositionx(internedString7, vector2Control17);
			AxisControl axisControl76 = this.Initialize_ctrlTouchscreentouch3startPositiony(internedString7, vector2Control17);
			IntegerControl integerControl17 = this.Initialize_ctrlTouchscreentouch4touchId(internedString6, touchControl6);
			Vector2Control vector2Control18 = this.Initialize_ctrlTouchscreentouch4position(internedString2, touchControl6);
			DeltaControl deltaControl7 = this.Initialize_ctrlTouchscreentouch4delta(internedString3, touchControl6);
			AxisControl axisControl77 = this.Initialize_ctrlTouchscreentouch4pressure(internedString7, touchControl6);
			Vector2Control vector2Control19 = this.Initialize_ctrlTouchscreentouch4radius(internedString2, touchControl6);
			TouchPhaseControl touchPhaseControl6 = this.Initialize_ctrlTouchscreentouch4phase(internedString8, touchControl6);
			TouchPressControl touchPressControl7 = this.Initialize_ctrlTouchscreentouch4press(internedString5, touchControl6);
			IntegerControl integerControl18 = this.Initialize_ctrlTouchscreentouch4tapCount(internedString6, touchControl6);
			IntegerControl integerControl19 = this.Initialize_ctrlTouchscreentouch4displayIndex(internedString6, touchControl6);
			ButtonControl buttonControl11 = this.Initialize_ctrlTouchscreentouch4indirectTouch(internedString9, touchControl6);
			ButtonControl buttonControl12 = this.Initialize_ctrlTouchscreentouch4tap(internedString9, touchControl6);
			DoubleControl doubleControl6 = this.Initialize_ctrlTouchscreentouch4startTime(internedString10, touchControl6);
			Vector2Control vector2Control20 = this.Initialize_ctrlTouchscreentouch4startPosition(internedString2, touchControl6);
			AxisControl axisControl78 = this.Initialize_ctrlTouchscreentouch4positionx(internedString7, vector2Control18);
			AxisControl axisControl79 = this.Initialize_ctrlTouchscreentouch4positiony(internedString7, vector2Control18);
			AxisControl axisControl80 = this.Initialize_ctrlTouchscreentouch4deltaup(internedString7, deltaControl7);
			AxisControl axisControl81 = this.Initialize_ctrlTouchscreentouch4deltadown(internedString7, deltaControl7);
			AxisControl axisControl82 = this.Initialize_ctrlTouchscreentouch4deltaleft(internedString7, deltaControl7);
			AxisControl axisControl83 = this.Initialize_ctrlTouchscreentouch4deltaright(internedString7, deltaControl7);
			AxisControl axisControl84 = this.Initialize_ctrlTouchscreentouch4deltax(internedString7, deltaControl7);
			AxisControl axisControl85 = this.Initialize_ctrlTouchscreentouch4deltay(internedString7, deltaControl7);
			AxisControl axisControl86 = this.Initialize_ctrlTouchscreentouch4radiusx(internedString7, vector2Control19);
			AxisControl axisControl87 = this.Initialize_ctrlTouchscreentouch4radiusy(internedString7, vector2Control19);
			AxisControl axisControl88 = this.Initialize_ctrlTouchscreentouch4startPositionx(internedString7, vector2Control20);
			AxisControl axisControl89 = this.Initialize_ctrlTouchscreentouch4startPositiony(internedString7, vector2Control20);
			IntegerControl integerControl20 = this.Initialize_ctrlTouchscreentouch5touchId(internedString6, touchControl7);
			Vector2Control vector2Control21 = this.Initialize_ctrlTouchscreentouch5position(internedString2, touchControl7);
			DeltaControl deltaControl8 = this.Initialize_ctrlTouchscreentouch5delta(internedString3, touchControl7);
			AxisControl axisControl90 = this.Initialize_ctrlTouchscreentouch5pressure(internedString7, touchControl7);
			Vector2Control vector2Control22 = this.Initialize_ctrlTouchscreentouch5radius(internedString2, touchControl7);
			TouchPhaseControl touchPhaseControl7 = this.Initialize_ctrlTouchscreentouch5phase(internedString8, touchControl7);
			TouchPressControl touchPressControl8 = this.Initialize_ctrlTouchscreentouch5press(internedString5, touchControl7);
			IntegerControl integerControl21 = this.Initialize_ctrlTouchscreentouch5tapCount(internedString6, touchControl7);
			IntegerControl integerControl22 = this.Initialize_ctrlTouchscreentouch5displayIndex(internedString6, touchControl7);
			ButtonControl buttonControl13 = this.Initialize_ctrlTouchscreentouch5indirectTouch(internedString9, touchControl7);
			ButtonControl buttonControl14 = this.Initialize_ctrlTouchscreentouch5tap(internedString9, touchControl7);
			DoubleControl doubleControl7 = this.Initialize_ctrlTouchscreentouch5startTime(internedString10, touchControl7);
			Vector2Control vector2Control23 = this.Initialize_ctrlTouchscreentouch5startPosition(internedString2, touchControl7);
			AxisControl axisControl91 = this.Initialize_ctrlTouchscreentouch5positionx(internedString7, vector2Control21);
			AxisControl axisControl92 = this.Initialize_ctrlTouchscreentouch5positiony(internedString7, vector2Control21);
			AxisControl axisControl93 = this.Initialize_ctrlTouchscreentouch5deltaup(internedString7, deltaControl8);
			AxisControl axisControl94 = this.Initialize_ctrlTouchscreentouch5deltadown(internedString7, deltaControl8);
			AxisControl axisControl95 = this.Initialize_ctrlTouchscreentouch5deltaleft(internedString7, deltaControl8);
			AxisControl axisControl96 = this.Initialize_ctrlTouchscreentouch5deltaright(internedString7, deltaControl8);
			AxisControl axisControl97 = this.Initialize_ctrlTouchscreentouch5deltax(internedString7, deltaControl8);
			AxisControl axisControl98 = this.Initialize_ctrlTouchscreentouch5deltay(internedString7, deltaControl8);
			AxisControl axisControl99 = this.Initialize_ctrlTouchscreentouch5radiusx(internedString7, vector2Control22);
			AxisControl axisControl100 = this.Initialize_ctrlTouchscreentouch5radiusy(internedString7, vector2Control22);
			AxisControl axisControl101 = this.Initialize_ctrlTouchscreentouch5startPositionx(internedString7, vector2Control23);
			AxisControl axisControl102 = this.Initialize_ctrlTouchscreentouch5startPositiony(internedString7, vector2Control23);
			IntegerControl integerControl23 = this.Initialize_ctrlTouchscreentouch6touchId(internedString6, touchControl8);
			Vector2Control vector2Control24 = this.Initialize_ctrlTouchscreentouch6position(internedString2, touchControl8);
			DeltaControl deltaControl9 = this.Initialize_ctrlTouchscreentouch6delta(internedString3, touchControl8);
			AxisControl axisControl103 = this.Initialize_ctrlTouchscreentouch6pressure(internedString7, touchControl8);
			Vector2Control vector2Control25 = this.Initialize_ctrlTouchscreentouch6radius(internedString2, touchControl8);
			TouchPhaseControl touchPhaseControl8 = this.Initialize_ctrlTouchscreentouch6phase(internedString8, touchControl8);
			TouchPressControl touchPressControl9 = this.Initialize_ctrlTouchscreentouch6press(internedString5, touchControl8);
			IntegerControl integerControl24 = this.Initialize_ctrlTouchscreentouch6tapCount(internedString6, touchControl8);
			IntegerControl integerControl25 = this.Initialize_ctrlTouchscreentouch6displayIndex(internedString6, touchControl8);
			ButtonControl buttonControl15 = this.Initialize_ctrlTouchscreentouch6indirectTouch(internedString9, touchControl8);
			ButtonControl buttonControl16 = this.Initialize_ctrlTouchscreentouch6tap(internedString9, touchControl8);
			DoubleControl doubleControl8 = this.Initialize_ctrlTouchscreentouch6startTime(internedString10, touchControl8);
			Vector2Control vector2Control26 = this.Initialize_ctrlTouchscreentouch6startPosition(internedString2, touchControl8);
			AxisControl axisControl104 = this.Initialize_ctrlTouchscreentouch6positionx(internedString7, vector2Control24);
			AxisControl axisControl105 = this.Initialize_ctrlTouchscreentouch6positiony(internedString7, vector2Control24);
			AxisControl axisControl106 = this.Initialize_ctrlTouchscreentouch6deltaup(internedString7, deltaControl9);
			AxisControl axisControl107 = this.Initialize_ctrlTouchscreentouch6deltadown(internedString7, deltaControl9);
			AxisControl axisControl108 = this.Initialize_ctrlTouchscreentouch6deltaleft(internedString7, deltaControl9);
			AxisControl axisControl109 = this.Initialize_ctrlTouchscreentouch6deltaright(internedString7, deltaControl9);
			AxisControl axisControl110 = this.Initialize_ctrlTouchscreentouch6deltax(internedString7, deltaControl9);
			AxisControl axisControl111 = this.Initialize_ctrlTouchscreentouch6deltay(internedString7, deltaControl9);
			AxisControl axisControl112 = this.Initialize_ctrlTouchscreentouch6radiusx(internedString7, vector2Control25);
			AxisControl axisControl113 = this.Initialize_ctrlTouchscreentouch6radiusy(internedString7, vector2Control25);
			AxisControl axisControl114 = this.Initialize_ctrlTouchscreentouch6startPositionx(internedString7, vector2Control26);
			AxisControl axisControl115 = this.Initialize_ctrlTouchscreentouch6startPositiony(internedString7, vector2Control26);
			IntegerControl integerControl26 = this.Initialize_ctrlTouchscreentouch7touchId(internedString6, touchControl9);
			Vector2Control vector2Control27 = this.Initialize_ctrlTouchscreentouch7position(internedString2, touchControl9);
			DeltaControl deltaControl10 = this.Initialize_ctrlTouchscreentouch7delta(internedString3, touchControl9);
			AxisControl axisControl116 = this.Initialize_ctrlTouchscreentouch7pressure(internedString7, touchControl9);
			Vector2Control vector2Control28 = this.Initialize_ctrlTouchscreentouch7radius(internedString2, touchControl9);
			TouchPhaseControl touchPhaseControl9 = this.Initialize_ctrlTouchscreentouch7phase(internedString8, touchControl9);
			TouchPressControl touchPressControl10 = this.Initialize_ctrlTouchscreentouch7press(internedString5, touchControl9);
			IntegerControl integerControl27 = this.Initialize_ctrlTouchscreentouch7tapCount(internedString6, touchControl9);
			IntegerControl integerControl28 = this.Initialize_ctrlTouchscreentouch7displayIndex(internedString6, touchControl9);
			ButtonControl buttonControl17 = this.Initialize_ctrlTouchscreentouch7indirectTouch(internedString9, touchControl9);
			ButtonControl buttonControl18 = this.Initialize_ctrlTouchscreentouch7tap(internedString9, touchControl9);
			DoubleControl doubleControl9 = this.Initialize_ctrlTouchscreentouch7startTime(internedString10, touchControl9);
			Vector2Control vector2Control29 = this.Initialize_ctrlTouchscreentouch7startPosition(internedString2, touchControl9);
			AxisControl axisControl117 = this.Initialize_ctrlTouchscreentouch7positionx(internedString7, vector2Control27);
			AxisControl axisControl118 = this.Initialize_ctrlTouchscreentouch7positiony(internedString7, vector2Control27);
			AxisControl axisControl119 = this.Initialize_ctrlTouchscreentouch7deltaup(internedString7, deltaControl10);
			AxisControl axisControl120 = this.Initialize_ctrlTouchscreentouch7deltadown(internedString7, deltaControl10);
			AxisControl axisControl121 = this.Initialize_ctrlTouchscreentouch7deltaleft(internedString7, deltaControl10);
			AxisControl axisControl122 = this.Initialize_ctrlTouchscreentouch7deltaright(internedString7, deltaControl10);
			AxisControl axisControl123 = this.Initialize_ctrlTouchscreentouch7deltax(internedString7, deltaControl10);
			AxisControl axisControl124 = this.Initialize_ctrlTouchscreentouch7deltay(internedString7, deltaControl10);
			AxisControl axisControl125 = this.Initialize_ctrlTouchscreentouch7radiusx(internedString7, vector2Control28);
			AxisControl axisControl126 = this.Initialize_ctrlTouchscreentouch7radiusy(internedString7, vector2Control28);
			AxisControl axisControl127 = this.Initialize_ctrlTouchscreentouch7startPositionx(internedString7, vector2Control29);
			AxisControl axisControl128 = this.Initialize_ctrlTouchscreentouch7startPositiony(internedString7, vector2Control29);
			IntegerControl integerControl29 = this.Initialize_ctrlTouchscreentouch8touchId(internedString6, touchControl10);
			Vector2Control vector2Control30 = this.Initialize_ctrlTouchscreentouch8position(internedString2, touchControl10);
			DeltaControl deltaControl11 = this.Initialize_ctrlTouchscreentouch8delta(internedString3, touchControl10);
			AxisControl axisControl129 = this.Initialize_ctrlTouchscreentouch8pressure(internedString7, touchControl10);
			Vector2Control vector2Control31 = this.Initialize_ctrlTouchscreentouch8radius(internedString2, touchControl10);
			TouchPhaseControl touchPhaseControl10 = this.Initialize_ctrlTouchscreentouch8phase(internedString8, touchControl10);
			TouchPressControl touchPressControl11 = this.Initialize_ctrlTouchscreentouch8press(internedString5, touchControl10);
			IntegerControl integerControl30 = this.Initialize_ctrlTouchscreentouch8tapCount(internedString6, touchControl10);
			IntegerControl integerControl31 = this.Initialize_ctrlTouchscreentouch8displayIndex(internedString6, touchControl10);
			ButtonControl buttonControl19 = this.Initialize_ctrlTouchscreentouch8indirectTouch(internedString9, touchControl10);
			ButtonControl buttonControl20 = this.Initialize_ctrlTouchscreentouch8tap(internedString9, touchControl10);
			DoubleControl doubleControl10 = this.Initialize_ctrlTouchscreentouch8startTime(internedString10, touchControl10);
			Vector2Control vector2Control32 = this.Initialize_ctrlTouchscreentouch8startPosition(internedString2, touchControl10);
			AxisControl axisControl130 = this.Initialize_ctrlTouchscreentouch8positionx(internedString7, vector2Control30);
			AxisControl axisControl131 = this.Initialize_ctrlTouchscreentouch8positiony(internedString7, vector2Control30);
			AxisControl axisControl132 = this.Initialize_ctrlTouchscreentouch8deltaup(internedString7, deltaControl11);
			AxisControl axisControl133 = this.Initialize_ctrlTouchscreentouch8deltadown(internedString7, deltaControl11);
			AxisControl axisControl134 = this.Initialize_ctrlTouchscreentouch8deltaleft(internedString7, deltaControl11);
			AxisControl axisControl135 = this.Initialize_ctrlTouchscreentouch8deltaright(internedString7, deltaControl11);
			AxisControl axisControl136 = this.Initialize_ctrlTouchscreentouch8deltax(internedString7, deltaControl11);
			AxisControl axisControl137 = this.Initialize_ctrlTouchscreentouch8deltay(internedString7, deltaControl11);
			AxisControl axisControl138 = this.Initialize_ctrlTouchscreentouch8radiusx(internedString7, vector2Control31);
			AxisControl axisControl139 = this.Initialize_ctrlTouchscreentouch8radiusy(internedString7, vector2Control31);
			AxisControl axisControl140 = this.Initialize_ctrlTouchscreentouch8startPositionx(internedString7, vector2Control32);
			AxisControl axisControl141 = this.Initialize_ctrlTouchscreentouch8startPositiony(internedString7, vector2Control32);
			IntegerControl integerControl32 = this.Initialize_ctrlTouchscreentouch9touchId(internedString6, touchControl11);
			Vector2Control vector2Control33 = this.Initialize_ctrlTouchscreentouch9position(internedString2, touchControl11);
			DeltaControl deltaControl12 = this.Initialize_ctrlTouchscreentouch9delta(internedString3, touchControl11);
			AxisControl axisControl142 = this.Initialize_ctrlTouchscreentouch9pressure(internedString7, touchControl11);
			Vector2Control vector2Control34 = this.Initialize_ctrlTouchscreentouch9radius(internedString2, touchControl11);
			TouchPhaseControl touchPhaseControl11 = this.Initialize_ctrlTouchscreentouch9phase(internedString8, touchControl11);
			TouchPressControl touchPressControl12 = this.Initialize_ctrlTouchscreentouch9press(internedString5, touchControl11);
			IntegerControl integerControl33 = this.Initialize_ctrlTouchscreentouch9tapCount(internedString6, touchControl11);
			IntegerControl integerControl34 = this.Initialize_ctrlTouchscreentouch9displayIndex(internedString6, touchControl11);
			ButtonControl buttonControl21 = this.Initialize_ctrlTouchscreentouch9indirectTouch(internedString9, touchControl11);
			ButtonControl buttonControl22 = this.Initialize_ctrlTouchscreentouch9tap(internedString9, touchControl11);
			DoubleControl doubleControl11 = this.Initialize_ctrlTouchscreentouch9startTime(internedString10, touchControl11);
			Vector2Control vector2Control35 = this.Initialize_ctrlTouchscreentouch9startPosition(internedString2, touchControl11);
			AxisControl axisControl143 = this.Initialize_ctrlTouchscreentouch9positionx(internedString7, vector2Control33);
			AxisControl axisControl144 = this.Initialize_ctrlTouchscreentouch9positiony(internedString7, vector2Control33);
			AxisControl axisControl145 = this.Initialize_ctrlTouchscreentouch9deltaup(internedString7, deltaControl12);
			AxisControl axisControl146 = this.Initialize_ctrlTouchscreentouch9deltadown(internedString7, deltaControl12);
			AxisControl axisControl147 = this.Initialize_ctrlTouchscreentouch9deltaleft(internedString7, deltaControl12);
			AxisControl axisControl148 = this.Initialize_ctrlTouchscreentouch9deltaright(internedString7, deltaControl12);
			AxisControl axisControl149 = this.Initialize_ctrlTouchscreentouch9deltax(internedString7, deltaControl12);
			AxisControl axisControl150 = this.Initialize_ctrlTouchscreentouch9deltay(internedString7, deltaControl12);
			AxisControl axisControl151 = this.Initialize_ctrlTouchscreentouch9radiusx(internedString7, vector2Control34);
			AxisControl axisControl152 = this.Initialize_ctrlTouchscreentouch9radiusy(internedString7, vector2Control34);
			AxisControl axisControl153 = this.Initialize_ctrlTouchscreentouch9startPositionx(internedString7, vector2Control35);
			AxisControl axisControl154 = this.Initialize_ctrlTouchscreentouch9startPositiony(internedString7, vector2Control35);
			deviceBuilder.WithControlUsage(0, new InternedString("PrimaryAction"), buttonControl2);
			deviceBuilder.WithControlUsage(1, new InternedString("Point"), vector2Control);
			deviceBuilder.WithControlUsage(2, new InternedString("Secondary2DMotion"), deltaControl);
			deviceBuilder.WithControlUsage(3, new InternedString("Pressure"), axisControl);
			deviceBuilder.WithControlUsage(4, new InternedString("Radius"), vector2Control2);
			base.touchControlArray = new TouchControl[10];
			base.touchControlArray[0] = touchControl2;
			base.touchControlArray[1] = touchControl3;
			base.touchControlArray[2] = touchControl4;
			base.touchControlArray[3] = touchControl5;
			base.touchControlArray[4] = touchControl6;
			base.touchControlArray[5] = touchControl7;
			base.touchControlArray[6] = touchControl8;
			base.touchControlArray[7] = touchControl9;
			base.touchControlArray[8] = touchControl10;
			base.touchControlArray[9] = touchControl11;
			base.primaryTouch = touchControl;
			base.position = vector2Control;
			base.delta = deltaControl;
			base.radius = vector2Control2;
			base.pressure = axisControl;
			base.press = touchPressControl;
			base.displayIndex = integerControl;
			touchControl.press = touchPressControl2;
			touchControl.displayIndex = integerControl4;
			touchControl.touchId = integerControl2;
			touchControl.position = vector2Control3;
			touchControl.delta = deltaControl2;
			touchControl.pressure = axisControl2;
			touchControl.radius = vector2Control4;
			touchControl.phase = touchPhaseControl;
			touchControl.indirectTouch = buttonControl;
			touchControl.tap = buttonControl2;
			touchControl.tapCount = integerControl3;
			touchControl.startTime = doubleControl;
			touchControl.startPosition = vector2Control5;
			vector2Control.x = axisControl15;
			vector2Control.y = axisControl16;
			deltaControl.up = axisControl17;
			deltaControl.down = axisControl18;
			deltaControl.left = axisControl19;
			deltaControl.right = axisControl20;
			deltaControl.x = axisControl21;
			deltaControl.y = axisControl22;
			vector2Control2.x = axisControl23;
			vector2Control2.y = axisControl24;
			touchControl2.press = touchPressControl3;
			touchControl2.displayIndex = integerControl7;
			touchControl2.touchId = integerControl5;
			touchControl2.position = vector2Control6;
			touchControl2.delta = deltaControl3;
			touchControl2.pressure = axisControl25;
			touchControl2.radius = vector2Control7;
			touchControl2.phase = touchPhaseControl2;
			touchControl2.indirectTouch = buttonControl3;
			touchControl2.tap = buttonControl4;
			touchControl2.tapCount = integerControl6;
			touchControl2.startTime = doubleControl2;
			touchControl2.startPosition = vector2Control8;
			touchControl3.press = touchPressControl4;
			touchControl3.displayIndex = integerControl10;
			touchControl3.touchId = integerControl8;
			touchControl3.position = vector2Control9;
			touchControl3.delta = deltaControl4;
			touchControl3.pressure = axisControl38;
			touchControl3.radius = vector2Control10;
			touchControl3.phase = touchPhaseControl3;
			touchControl3.indirectTouch = buttonControl5;
			touchControl3.tap = buttonControl6;
			touchControl3.tapCount = integerControl9;
			touchControl3.startTime = doubleControl3;
			touchControl3.startPosition = vector2Control11;
			touchControl4.press = touchPressControl5;
			touchControl4.displayIndex = integerControl13;
			touchControl4.touchId = integerControl11;
			touchControl4.position = vector2Control12;
			touchControl4.delta = deltaControl5;
			touchControl4.pressure = axisControl51;
			touchControl4.radius = vector2Control13;
			touchControl4.phase = touchPhaseControl4;
			touchControl4.indirectTouch = buttonControl7;
			touchControl4.tap = buttonControl8;
			touchControl4.tapCount = integerControl12;
			touchControl4.startTime = doubleControl4;
			touchControl4.startPosition = vector2Control14;
			touchControl5.press = touchPressControl6;
			touchControl5.displayIndex = integerControl16;
			touchControl5.touchId = integerControl14;
			touchControl5.position = vector2Control15;
			touchControl5.delta = deltaControl6;
			touchControl5.pressure = axisControl64;
			touchControl5.radius = vector2Control16;
			touchControl5.phase = touchPhaseControl5;
			touchControl5.indirectTouch = buttonControl9;
			touchControl5.tap = buttonControl10;
			touchControl5.tapCount = integerControl15;
			touchControl5.startTime = doubleControl5;
			touchControl5.startPosition = vector2Control17;
			touchControl6.press = touchPressControl7;
			touchControl6.displayIndex = integerControl19;
			touchControl6.touchId = integerControl17;
			touchControl6.position = vector2Control18;
			touchControl6.delta = deltaControl7;
			touchControl6.pressure = axisControl77;
			touchControl6.radius = vector2Control19;
			touchControl6.phase = touchPhaseControl6;
			touchControl6.indirectTouch = buttonControl11;
			touchControl6.tap = buttonControl12;
			touchControl6.tapCount = integerControl18;
			touchControl6.startTime = doubleControl6;
			touchControl6.startPosition = vector2Control20;
			touchControl7.press = touchPressControl8;
			touchControl7.displayIndex = integerControl22;
			touchControl7.touchId = integerControl20;
			touchControl7.position = vector2Control21;
			touchControl7.delta = deltaControl8;
			touchControl7.pressure = axisControl90;
			touchControl7.radius = vector2Control22;
			touchControl7.phase = touchPhaseControl7;
			touchControl7.indirectTouch = buttonControl13;
			touchControl7.tap = buttonControl14;
			touchControl7.tapCount = integerControl21;
			touchControl7.startTime = doubleControl7;
			touchControl7.startPosition = vector2Control23;
			touchControl8.press = touchPressControl9;
			touchControl8.displayIndex = integerControl25;
			touchControl8.touchId = integerControl23;
			touchControl8.position = vector2Control24;
			touchControl8.delta = deltaControl9;
			touchControl8.pressure = axisControl103;
			touchControl8.radius = vector2Control25;
			touchControl8.phase = touchPhaseControl8;
			touchControl8.indirectTouch = buttonControl15;
			touchControl8.tap = buttonControl16;
			touchControl8.tapCount = integerControl24;
			touchControl8.startTime = doubleControl8;
			touchControl8.startPosition = vector2Control26;
			touchControl9.press = touchPressControl10;
			touchControl9.displayIndex = integerControl28;
			touchControl9.touchId = integerControl26;
			touchControl9.position = vector2Control27;
			touchControl9.delta = deltaControl10;
			touchControl9.pressure = axisControl116;
			touchControl9.radius = vector2Control28;
			touchControl9.phase = touchPhaseControl9;
			touchControl9.indirectTouch = buttonControl17;
			touchControl9.tap = buttonControl18;
			touchControl9.tapCount = integerControl27;
			touchControl9.startTime = doubleControl9;
			touchControl9.startPosition = vector2Control29;
			touchControl10.press = touchPressControl11;
			touchControl10.displayIndex = integerControl31;
			touchControl10.touchId = integerControl29;
			touchControl10.position = vector2Control30;
			touchControl10.delta = deltaControl11;
			touchControl10.pressure = axisControl129;
			touchControl10.radius = vector2Control31;
			touchControl10.phase = touchPhaseControl10;
			touchControl10.indirectTouch = buttonControl19;
			touchControl10.tap = buttonControl20;
			touchControl10.tapCount = integerControl30;
			touchControl10.startTime = doubleControl10;
			touchControl10.startPosition = vector2Control32;
			touchControl11.press = touchPressControl12;
			touchControl11.displayIndex = integerControl34;
			touchControl11.touchId = integerControl32;
			touchControl11.position = vector2Control33;
			touchControl11.delta = deltaControl12;
			touchControl11.pressure = axisControl142;
			touchControl11.radius = vector2Control34;
			touchControl11.phase = touchPhaseControl11;
			touchControl11.indirectTouch = buttonControl21;
			touchControl11.tap = buttonControl22;
			touchControl11.tapCount = integerControl33;
			touchControl11.startTime = doubleControl11;
			touchControl11.startPosition = vector2Control35;
			vector2Control3.x = axisControl3;
			vector2Control3.y = axisControl4;
			deltaControl2.up = axisControl5;
			deltaControl2.down = axisControl6;
			deltaControl2.left = axisControl7;
			deltaControl2.right = axisControl8;
			deltaControl2.x = axisControl9;
			deltaControl2.y = axisControl10;
			vector2Control4.x = axisControl11;
			vector2Control4.y = axisControl12;
			vector2Control5.x = axisControl13;
			vector2Control5.y = axisControl14;
			vector2Control6.x = axisControl26;
			vector2Control6.y = axisControl27;
			deltaControl3.up = axisControl28;
			deltaControl3.down = axisControl29;
			deltaControl3.left = axisControl30;
			deltaControl3.right = axisControl31;
			deltaControl3.x = axisControl32;
			deltaControl3.y = axisControl33;
			vector2Control7.x = axisControl34;
			vector2Control7.y = axisControl35;
			vector2Control8.x = axisControl36;
			vector2Control8.y = axisControl37;
			vector2Control9.x = axisControl39;
			vector2Control9.y = axisControl40;
			deltaControl4.up = axisControl41;
			deltaControl4.down = axisControl42;
			deltaControl4.left = axisControl43;
			deltaControl4.right = axisControl44;
			deltaControl4.x = axisControl45;
			deltaControl4.y = axisControl46;
			vector2Control10.x = axisControl47;
			vector2Control10.y = axisControl48;
			vector2Control11.x = axisControl49;
			vector2Control11.y = axisControl50;
			vector2Control12.x = axisControl52;
			vector2Control12.y = axisControl53;
			deltaControl5.up = axisControl54;
			deltaControl5.down = axisControl55;
			deltaControl5.left = axisControl56;
			deltaControl5.right = axisControl57;
			deltaControl5.x = axisControl58;
			deltaControl5.y = axisControl59;
			vector2Control13.x = axisControl60;
			vector2Control13.y = axisControl61;
			vector2Control14.x = axisControl62;
			vector2Control14.y = axisControl63;
			vector2Control15.x = axisControl65;
			vector2Control15.y = axisControl66;
			deltaControl6.up = axisControl67;
			deltaControl6.down = axisControl68;
			deltaControl6.left = axisControl69;
			deltaControl6.right = axisControl70;
			deltaControl6.x = axisControl71;
			deltaControl6.y = axisControl72;
			vector2Control16.x = axisControl73;
			vector2Control16.y = axisControl74;
			vector2Control17.x = axisControl75;
			vector2Control17.y = axisControl76;
			vector2Control18.x = axisControl78;
			vector2Control18.y = axisControl79;
			deltaControl7.up = axisControl80;
			deltaControl7.down = axisControl81;
			deltaControl7.left = axisControl82;
			deltaControl7.right = axisControl83;
			deltaControl7.x = axisControl84;
			deltaControl7.y = axisControl85;
			vector2Control19.x = axisControl86;
			vector2Control19.y = axisControl87;
			vector2Control20.x = axisControl88;
			vector2Control20.y = axisControl89;
			vector2Control21.x = axisControl91;
			vector2Control21.y = axisControl92;
			deltaControl8.up = axisControl93;
			deltaControl8.down = axisControl94;
			deltaControl8.left = axisControl95;
			deltaControl8.right = axisControl96;
			deltaControl8.x = axisControl97;
			deltaControl8.y = axisControl98;
			vector2Control22.x = axisControl99;
			vector2Control22.y = axisControl100;
			vector2Control23.x = axisControl101;
			vector2Control23.y = axisControl102;
			vector2Control24.x = axisControl104;
			vector2Control24.y = axisControl105;
			deltaControl9.up = axisControl106;
			deltaControl9.down = axisControl107;
			deltaControl9.left = axisControl108;
			deltaControl9.right = axisControl109;
			deltaControl9.x = axisControl110;
			deltaControl9.y = axisControl111;
			vector2Control25.x = axisControl112;
			vector2Control25.y = axisControl113;
			vector2Control26.x = axisControl114;
			vector2Control26.y = axisControl115;
			vector2Control27.x = axisControl117;
			vector2Control27.y = axisControl118;
			deltaControl10.up = axisControl119;
			deltaControl10.down = axisControl120;
			deltaControl10.left = axisControl121;
			deltaControl10.right = axisControl122;
			deltaControl10.x = axisControl123;
			deltaControl10.y = axisControl124;
			vector2Control28.x = axisControl125;
			vector2Control28.y = axisControl126;
			vector2Control29.x = axisControl127;
			vector2Control29.y = axisControl128;
			vector2Control30.x = axisControl130;
			vector2Control30.y = axisControl131;
			deltaControl11.up = axisControl132;
			deltaControl11.down = axisControl133;
			deltaControl11.left = axisControl134;
			deltaControl11.right = axisControl135;
			deltaControl11.x = axisControl136;
			deltaControl11.y = axisControl137;
			vector2Control31.x = axisControl138;
			vector2Control31.y = axisControl139;
			vector2Control32.x = axisControl140;
			vector2Control32.y = axisControl141;
			vector2Control33.x = axisControl143;
			vector2Control33.y = axisControl144;
			deltaControl12.up = axisControl145;
			deltaControl12.down = axisControl146;
			deltaControl12.left = axisControl147;
			deltaControl12.right = axisControl148;
			deltaControl12.x = axisControl149;
			deltaControl12.y = axisControl150;
			vector2Control34.x = axisControl151;
			vector2Control34.y = axisControl152;
			vector2Control35.x = axisControl153;
			vector2Control35.y = axisControl154;
			deviceBuilder.WithStateOffsetToControlIndexMap(new uint[]
			{
				32785U, 16810014U, 16810026U, 33587231U, 33587243U, 50364450U, 50364451U, 50364452U, 50364462U, 50364463U,
				50364464U, 67141664U, 67141665U, 67141669U, 67141676U, 67141677U, 67141681U, 83918851U, 83918868U, 100696102U,
				100696114U, 117473319U, 117473331U, 134225925U, 134225942U, 134225943U, 138420248U, 142614534U, 142614553U, 146801690U,
				148898843U, 167837724U, 201359400U, 218136617U, 234913844U, 251691073U, 268468290U, 285245509U, 285245510U, 285245511U,
				302022723U, 302022724U, 302022728U, 318799927U, 335577161U, 352354378U, 369107001U, 369107002U, 373301307U, 377495612U,
				381682749U, 383779902U, 402718783U, 436240459U, 453017676U, 469794893U, 486572122U, 503349339U, 520126558U, 520126559U,
				520126560U, 536903772U, 536903773U, 536903777U, 553680976U, 570458210U, 587235427U, 603988050U, 603988051U, 608182356U,
				612376661U, 616563798U, 618660951U, 637599832U, 671121508U, 687898725U, 704675942U, 721453171U, 738230388U, 755007607U,
				755007608U, 755007609U, 771784821U, 771784822U, 771784826U, 788562025U, 805339259U, 822116476U, 838869099U, 838869100U,
				843063405U, 847257710U, 851444847U, 853542000U, 872480881U, 906002557U, 922779774U, 939556991U, 956334220U, 973111437U,
				989888656U, 989888657U, 989888658U, 1006665870U, 1006665871U, 1006665875U, 1023443074U, 1040220308U, 1056997525U, 1073750148U,
				1073750149U, 1077944454U, 1082138759U, 1086325896U, 1088423049U, 1107361930U, 1140883606U, 1157660823U, 1174438040U, 1191215269U,
				1207992486U, 1224769705U, 1224769706U, 1224769707U, 1241546919U, 1241546920U, 1241546924U, 1258324123U, 1275101357U, 1291878574U,
				1308631197U, 1308631198U, 1312825503U, 1317019808U, 1321206945U, 1323304098U, 1342242979U, 1375764655U, 1392541872U, 1409319089U,
				1426096318U, 1442873535U, 1459650754U, 1459650755U, 1459650756U, 1476427968U, 1476427969U, 1476427973U, 1493205172U, 1509982406U,
				1526759623U, 1543512246U, 1543512247U, 1547706552U, 1551900857U, 1556087994U, 1558185147U, 1577124028U, 1610645704U, 1627422921U,
				1644200138U, 1660977367U, 1677754584U, 1694531803U, 1694531804U, 1694531805U, 1711309017U, 1711309018U, 1711309022U, 1728086221U,
				1744863455U, 1761640672U, 1778393295U, 1778393296U, 1782587601U, 1786781906U, 1790969043U, 1793066196U, 1812005077U, 1845526753U,
				1862303970U, 1879081187U, 1895858416U, 1912635633U, 1929412852U, 1929412853U, 1929412854U, 1946190066U, 1946190067U, 1946190071U,
				1962967270U, 1979744504U, 1996521721U, 2013274344U, 2013274345U, 2017468650U, 2021662955U, 2025850092U, 2027947245U, 2046886126U,
				2080407802U, 2097185019U, 2113962236U, 2130739465U, 2147516682U, 2164293901U, 2164293902U, 2164293903U, 2181071115U, 2181071116U,
				2181071120U, 2197848319U, 2214625553U, 2231402770U, 2248155393U, 2248155394U, 2252349699U, 2256544004U, 2260731141U, 2262828294U,
				2281767175U, 2315288851U, 2332066068U, 2348843285U, 2365620514U, 2382397731U, 2399174950U, 2399174951U, 2399174952U, 2415952164U,
				2415952165U, 2415952169U, 2432729368U, 2449506602U, 2466283819U, 2483036442U, 2483036443U, 2487230748U, 2491425053U, 2495612190U,
				2497709343U, 2516648224U, 2550169900U, 2566947117U
			});
			deviceBuilder.WithControlTree(new byte[]
			{
				63, 19, 1, 0, 0, 0, 0, 192, 8, 3,
				0, 0, 0, 0, 63, 19, 1, 1, 0, 0,
				0, 128, 3, 5, 0, 0, 0, 0, 192, 8,
				103, 0, 0, 0, 0, 192, 1, 7, 0, 0,
				0, 1, 128, 3, 53, 0, 68, 0, 1, 192,
				0, 9, 0, 0, 0, 0, 192, 1, 21, 0,
				0, 0, 0, 96, 0, 11, 0, 0, 0, 0,
				192, 0, 15, 0, 0, 0, 0, 32, 0, byte.MaxValue,
				byte.MaxValue, 1, 0, 1, 96, 0, 13, 0, 2, 0,
				2, 64, 0, byte.MaxValue, byte.MaxValue, 4, 0, 2, 96, 0,
				byte.MaxValue, byte.MaxValue, 6, 0, 2, 144, 0, 17, 0, 8,
				0, 8, 192, 0, 19, 0, 16, 0, 8, 120,
				0, byte.MaxValue, byte.MaxValue, 24, 0, 6, 144, 0, byte.MaxValue, byte.MaxValue,
				30, 0, 6, 168, 0, byte.MaxValue, byte.MaxValue, 36, 0, 2,
				192, 0, byte.MaxValue, byte.MaxValue, 38, 0, 2, 29, 1, 23,
				0, 0, 0, 0, 192, 1, 47, 0, 0, 0,
				0, 8, 1, 25, 0, 0, 0, 0, 29, 1,
				35, 0, 0, 0, 0, 228, 0, 27, 0, 40,
				0, 4, 8, 1, 29, 0, 44, 0, 4, 210,
				0, byte.MaxValue, byte.MaxValue, 48, 0, 2, 228, 0, byte.MaxValue, byte.MaxValue,
				50, 0, 2, 246, 0, byte.MaxValue, byte.MaxValue, 0, 0, 0,
				8, 1, 31, 0, 0, 0, 0, byte.MaxValue, 0, byte.MaxValue,
				byte.MaxValue, 0, 0, 0, 8, 1, 33, 0, 0, 0,
				0, 4, 1, byte.MaxValue, byte.MaxValue, 52, 0, 3, 8, 1,
				byte.MaxValue, byte.MaxValue, 55, 0, 3, 24, 1, 37, 0, 0,
				0, 0, 29, 1, 39, 0, 0, 0, 0, 16,
				1, byte.MaxValue, byte.MaxValue, 58, 0, 1, 24, 1, byte.MaxValue, byte.MaxValue,
				59, 0, 2, 27, 1, 41, 0, 0, 0, 0,
				29, 1, 45, 0, 0, 0, 0, 26, 1, 43,
				0, 0, 0, 0, 27, 1, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 25, 1, byte.MaxValue, byte.MaxValue, 61, 0, 1, 26, 1,
				byte.MaxValue, byte.MaxValue, 0, 0, 0, 28, 1, byte.MaxValue, byte.MaxValue, 0,
				0, 0, 29, 1, byte.MaxValue, byte.MaxValue, 62, 0, 1, 128,
				1, 49, 0, 0, 0, 0, 192, 1, 51, 0,
				65, 0, 1, 79, 1, byte.MaxValue, byte.MaxValue, 63, 0, 1,
				128, 1, byte.MaxValue, byte.MaxValue, 64, 0, 1, 160, 1, byte.MaxValue,
				byte.MaxValue, 66, 0, 1, 192, 1, byte.MaxValue, byte.MaxValue, 67, 0,
				1, 160, 2, 55, 0, 92, 0, 1, 128, 3,
				71, 0, 93, 0, 1, 48, 2, 57, 0, 77,
				0, 4, 160, 2, 65, 0, 81, 0, 4, 248,
				1, 59, 0, 71, 0, 2, 48, 2, 61, 0,
				73, 0, 2, 220, 1, byte.MaxValue, byte.MaxValue, 69, 0, 1,
				248, 1, byte.MaxValue, byte.MaxValue, 70, 0, 1, 32, 2, 63,
				0, 0, 0, 0, 48, 2, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 12, 2, byte.MaxValue, byte.MaxValue, 75, 0, 1, 32, 2,
				byte.MaxValue, byte.MaxValue, 76, 0, 1, 96, 2, 67, 0, 0,
				0, 0, 160, 2, 69, 0, 0, 0, 0, 72,
				2, byte.MaxValue, byte.MaxValue, 85, 0, 3, 96, 2, byte.MaxValue, byte.MaxValue,
				88, 0, 3, 128, 2, byte.MaxValue, byte.MaxValue, 91, 0, 1,
				160, 2, byte.MaxValue, byte.MaxValue, 94, 0, 1, 16, 3, 73,
				0, 105, 0, 1, 128, 3, 99, 0, 106, 0,
				1, 216, 2, 75, 0, 0, 0, 0, 16, 3,
				83, 0, 0, 0, 0, 188, 2, byte.MaxValue, byte.MaxValue, 95,
				0, 1, 216, 2, 77, 0, 96, 0, 1, 200,
				2, 79, 0, 0, 0, 0, 216, 2, 81, 0,
				0, 0, 0, 194, 2, byte.MaxValue, byte.MaxValue, 97, 0, 2,
				200, 2, byte.MaxValue, byte.MaxValue, 99, 0, 2, 208, 2, byte.MaxValue,
				byte.MaxValue, 101, 0, 1, 216, 2, byte.MaxValue, byte.MaxValue, 102, 0,
				1, 244, 2, 85, 0, 0, 0, 0, 16, 3,
				byte.MaxValue, byte.MaxValue, 0, 0, 0, 230, 2, 87, 0, 0,
				0, 0, 244, 2, byte.MaxValue, byte.MaxValue, 0, 0, 0, 223,
				2, 89, 0, 0, 0, 0, 230, 2, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 220, 2, 91, 0, 0, 0, 0,
				223, 2, 95, 0, 0, 0, 0, 218, 2, 93,
				0, 0, 0, 0, 220, 2, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 217, 2, byte.MaxValue, byte.MaxValue, 103, 0, 1, 218, 2,
				byte.MaxValue, byte.MaxValue, 0, 0, 0, 222, 2, 97, 0, 0,
				0, 0, 223, 2, byte.MaxValue, byte.MaxValue, 0, 0, 0, 221,
				2, byte.MaxValue, byte.MaxValue, 104, 0, 1, 222, 2, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 72, 3, byte.MaxValue, byte.MaxValue, 107, 0, 2,
				128, 3, 101, 0, 109, 0, 2, 100, 3, byte.MaxValue,
				byte.MaxValue, 111, 0, 1, 128, 3, byte.MaxValue, byte.MaxValue, 112, 0,
				1, 0, 7, 105, 0, 0, 0, 0, 192, 8,
				207, 0, 203, 0, 1, 64, 5, 107, 0, 113,
				0, 1, 0, 7, 157, 0, 158, 0, 1, 96,
				4, 109, 0, 137, 0, 1, 64, 5, 125, 0,
				138, 0, 1, 240, 3, 111, 0, 122, 0, 4,
				96, 4, 119, 0, 126, 0, 4, 184, 3, 113,
				0, 116, 0, 2, 240, 3, 115, 0, 118, 0,
				2, 156, 3, byte.MaxValue, byte.MaxValue, 114, 0, 1, 184, 3,
				byte.MaxValue, byte.MaxValue, 115, 0, 1, 224, 3, 117, 0, 0,
				0, 0, 240, 3, byte.MaxValue, byte.MaxValue, 0, 0, 0, 204,
				3, byte.MaxValue, byte.MaxValue, 120, 0, 1, 224, 3, byte.MaxValue, byte.MaxValue,
				121, 0, 1, 32, 4, 121, 0, 0, 0, 0,
				96, 4, 123, 0, 0, 0, 0, 8, 4, byte.MaxValue,
				byte.MaxValue, 130, 0, 3, 32, 4, byte.MaxValue, byte.MaxValue, 133, 0,
				3, 64, 4, byte.MaxValue, byte.MaxValue, 136, 0, 1, 96, 4,
				byte.MaxValue, byte.MaxValue, 139, 0, 1, 208, 4, 127, 0, 150,
				0, 1, 64, 5, 153, 0, 151, 0, 1, 152,
				4, 129, 0, 0, 0, 0, 208, 4, 137, 0,
				0, 0, 0, 124, 4, byte.MaxValue, byte.MaxValue, 140, 0, 1,
				152, 4, 131, 0, 141, 0, 1, 136, 4, 133,
				0, 0, 0, 0, 152, 4, 135, 0, 0, 0,
				0, 130, 4, byte.MaxValue, byte.MaxValue, 142, 0, 2, 136, 4,
				byte.MaxValue, byte.MaxValue, 144, 0, 2, 144, 4, byte.MaxValue, byte.MaxValue, 146,
				0, 1, 152, 4, byte.MaxValue, byte.MaxValue, 147, 0, 1, 180,
				4, 139, 0, 0, 0, 0, 208, 4, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 166, 4, 141, 0, 0, 0, 0,
				180, 4, byte.MaxValue, byte.MaxValue, 0, 0, 0, 159, 4, 143,
				0, 0, 0, 0, 166, 4, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 156, 4, 145, 0, 0, 0, 0, 159, 4,
				149, 0, 0, 0, 0, 154, 4, 147, 0, 0,
				0, 0, 156, 4, byte.MaxValue, byte.MaxValue, 0, 0, 0, 153,
				4, byte.MaxValue, byte.MaxValue, 148, 0, 1, 154, 4, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 158, 4, 151, 0, 0, 0, 0,
				159, 4, byte.MaxValue, byte.MaxValue, 0, 0, 0, 157, 4, byte.MaxValue,
				byte.MaxValue, 149, 0, 1, 158, 4, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 8, 5, byte.MaxValue, byte.MaxValue, 152, 0, 2, 64, 5,
				155, 0, 154, 0, 2, 36, 5, byte.MaxValue, byte.MaxValue, 156,
				0, 1, 64, 5, byte.MaxValue, byte.MaxValue, 157, 0, 1, 32,
				6, 159, 0, 182, 0, 1, 0, 7, 175, 0,
				183, 0, 1, 176, 5, 161, 0, 167, 0, 4,
				32, 6, 169, 0, 171, 0, 4, 120, 5, 163,
				0, 161, 0, 2, 176, 5, 165, 0, 163, 0,
				2, 92, 5, byte.MaxValue, byte.MaxValue, 159, 0, 1, 120, 5,
				byte.MaxValue, byte.MaxValue, 160, 0, 1, 160, 5, 167, 0, 0,
				0, 0, 176, 5, byte.MaxValue, byte.MaxValue, 0, 0, 0, 140,
				5, byte.MaxValue, byte.MaxValue, 165, 0, 1, 160, 5, byte.MaxValue, byte.MaxValue,
				166, 0, 1, 224, 5, 171, 0, 0, 0, 0,
				32, 6, 173, 0, 0, 0, 0, 200, 5, byte.MaxValue,
				byte.MaxValue, 175, 0, 3, 224, 5, byte.MaxValue, byte.MaxValue, 178, 0,
				3, 0, 6, byte.MaxValue, byte.MaxValue, 181, 0, 1, 32, 6,
				byte.MaxValue, byte.MaxValue, 184, 0, 1, 144, 6, 177, 0, 195,
				0, 1, 0, 7, 203, 0, 196, 0, 1, 88,
				6, 179, 0, 0, 0, 0, 144, 6, 187, 0,
				0, 0, 0, 60, 6, byte.MaxValue, byte.MaxValue, 185, 0, 1,
				88, 6, 181, 0, 186, 0, 1, 72, 6, 183,
				0, 0, 0, 0, 88, 6, 185, 0, 0, 0,
				0, 66, 6, byte.MaxValue, byte.MaxValue, 187, 0, 2, 72, 6,
				byte.MaxValue, byte.MaxValue, 189, 0, 2, 80, 6, byte.MaxValue, byte.MaxValue, 191,
				0, 1, 88, 6, byte.MaxValue, byte.MaxValue, 192, 0, 1, 116,
				6, 189, 0, 0, 0, 0, 144, 6, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 102, 6, 191, 0, 0, 0, 0,
				116, 6, byte.MaxValue, byte.MaxValue, 0, 0, 0, 95, 6, 193,
				0, 0, 0, 0, 102, 6, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 92, 6, 195, 0, 0, 0, 0, 95, 6,
				199, 0, 0, 0, 0, 90, 6, 197, 0, 0,
				0, 0, 92, 6, byte.MaxValue, byte.MaxValue, 0, 0, 0, 89,
				6, byte.MaxValue, byte.MaxValue, 193, 0, 1, 90, 6, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 94, 6, 201, 0, 0, 0, 0,
				95, 6, byte.MaxValue, byte.MaxValue, 0, 0, 0, 93, 6, byte.MaxValue,
				byte.MaxValue, 194, 0, 1, 94, 6, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 200, 6, byte.MaxValue, byte.MaxValue, 197, 0, 2, 0, 7,
				205, 0, 199, 0, 2, 228, 6, byte.MaxValue, byte.MaxValue, 201,
				0, 1, 0, 7, byte.MaxValue, byte.MaxValue, 202, 0, 1, 224,
				7, 209, 0, 227, 0, 1, 192, 8, 225, 0,
				228, 0, 1, 112, 7, 211, 0, 212, 0, 4,
				224, 7, 219, 0, 216, 0, 4, 56, 7, 213,
				0, 206, 0, 2, 112, 7, 215, 0, 208, 0,
				2, 28, 7, byte.MaxValue, byte.MaxValue, 204, 0, 1, 56, 7,
				byte.MaxValue, byte.MaxValue, 205, 0, 1, 96, 7, 217, 0, 0,
				0, 0, 112, 7, byte.MaxValue, byte.MaxValue, 0, 0, 0, 76,
				7, byte.MaxValue, byte.MaxValue, 210, 0, 1, 96, 7, byte.MaxValue, byte.MaxValue,
				211, 0, 1, 160, 7, 221, 0, 0, 0, 0,
				224, 7, 223, 0, 0, 0, 0, 136, 7, byte.MaxValue,
				byte.MaxValue, 220, 0, 3, 160, 7, byte.MaxValue, byte.MaxValue, 223, 0,
				3, 192, 7, byte.MaxValue, byte.MaxValue, 226, 0, 1, 224, 7,
				byte.MaxValue, byte.MaxValue, 229, 0, 1, 80, 8, 227, 0, 240,
				0, 1, 192, 8, 253, 0, 241, 0, 1, 24,
				8, 229, 0, 0, 0, 0, 80, 8, 237, 0,
				0, 0, 0, 252, 7, byte.MaxValue, byte.MaxValue, 230, 0, 1,
				24, 8, 231, 0, 231, 0, 1, 8, 8, 233,
				0, 0, 0, 0, 24, 8, 235, 0, 0, 0,
				0, 2, 8, byte.MaxValue, byte.MaxValue, 232, 0, 2, 8, 8,
				byte.MaxValue, byte.MaxValue, 234, 0, 2, 16, 8, byte.MaxValue, byte.MaxValue, 236,
				0, 1, 24, 8, byte.MaxValue, byte.MaxValue, 237, 0, 1, 52,
				8, 239, 0, 0, 0, 0, 80, 8, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 38, 8, 241, 0, 0, 0, 0,
				52, 8, byte.MaxValue, byte.MaxValue, 0, 0, 0, 31, 8, 243,
				0, 0, 0, 0, 38, 8, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 28, 8, 245, 0, 0, 0, 0, 31, 8,
				249, 0, 0, 0, 0, 26, 8, 247, 0, 0,
				0, 0, 28, 8, byte.MaxValue, byte.MaxValue, 0, 0, 0, 25,
				8, byte.MaxValue, byte.MaxValue, 238, 0, 1, 26, 8, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 30, 8, 251, 0, 0, 0, 0,
				31, 8, byte.MaxValue, byte.MaxValue, 0, 0, 0, 29, 8, byte.MaxValue,
				byte.MaxValue, 239, 0, 1, 30, 8, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 136, 8, byte.MaxValue, byte.MaxValue, 242, 0, 2, 192, 8,
				byte.MaxValue, 0, 244, 0, 2, 164, 8, byte.MaxValue, byte.MaxValue, 246,
				0, 1, 192, 8, byte.MaxValue, byte.MaxValue, 247, 0, 1, 0,
				14, 3, 1, 0, 0, 0, 63, 19, 157, 1,
				0, 0, 0, 64, 12, 5, 1, 0, 0, 0,
				0, 14, 107, 1, 82, 1, 1, 128, 10, 7,
				1, 248, 0, 1, 64, 12, 57, 1, 37, 1,
				1, 160, 9, 9, 1, 16, 1, 1, 128, 10,
				25, 1, 17, 1, 1, 48, 9, 11, 1, 1,
				1, 4, 160, 9, 19, 1, 5, 1, 4, 248,
				8, 13, 1, 251, 0, 2, 48, 9, 15, 1,
				253, 0, 2, 220, 8, byte.MaxValue, byte.MaxValue, 249, 0, 1,
				248, 8, byte.MaxValue, byte.MaxValue, 250, 0, 1, 32, 9, 17,
				1, 0, 0, 0, 48, 9, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 12, 9, byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, 1, 32, 9,
				byte.MaxValue, byte.MaxValue, 0, 1, 1, 96, 9, 21, 1, 0,
				0, 0, 160, 9, 23, 1, 0, 0, 0, 72,
				9, byte.MaxValue, byte.MaxValue, 9, 1, 3, 96, 9, byte.MaxValue, byte.MaxValue,
				12, 1, 3, 128, 9, byte.MaxValue, byte.MaxValue, 15, 1, 1,
				160, 9, byte.MaxValue, byte.MaxValue, 18, 1, 1, 16, 10, 27,
				1, 29, 1, 1, 128, 10, 53, 1, 30, 1,
				1, 216, 9, 29, 1, 0, 0, 0, 16, 10,
				37, 1, 0, 0, 0, 188, 9, byte.MaxValue, byte.MaxValue, 19,
				1, 1, 216, 9, 31, 1, 20, 1, 1, 200,
				9, 33, 1, 0, 0, 0, 216, 9, 35, 1,
				0, 0, 0, 194, 9, byte.MaxValue, byte.MaxValue, 21, 1, 2,
				200, 9, byte.MaxValue, byte.MaxValue, 23, 1, 2, 208, 9, byte.MaxValue,
				byte.MaxValue, 25, 1, 1, 216, 9, byte.MaxValue, byte.MaxValue, 26, 1,
				1, 244, 9, 39, 1, 0, 0, 0, 16, 10,
				byte.MaxValue, byte.MaxValue, 0, 0, 0, 230, 9, 41, 1, 0,
				0, 0, 244, 9, byte.MaxValue, byte.MaxValue, 0, 0, 0, 223,
				9, 43, 1, 0, 0, 0, 230, 9, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 220, 9, 45, 1, 0, 0, 0,
				223, 9, 49, 1, 0, 0, 0, 218, 9, 47,
				1, 0, 0, 0, 220, 9, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 217, 9, byte.MaxValue, byte.MaxValue, 27, 1, 1, 218, 9,
				byte.MaxValue, byte.MaxValue, 0, 0, 0, 222, 9, 51, 1, 0,
				0, 0, 223, 9, byte.MaxValue, byte.MaxValue, 0, 0, 0, 221,
				9, byte.MaxValue, byte.MaxValue, 28, 1, 1, 222, 9, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 72, 10, byte.MaxValue, byte.MaxValue, 31, 1, 2,
				128, 10, 55, 1, 33, 1, 2, 100, 10, byte.MaxValue,
				byte.MaxValue, 35, 1, 1, 128, 10, byte.MaxValue, byte.MaxValue, 36, 1,
				1, 96, 11, 59, 1, 61, 1, 1, 64, 12,
				75, 1, 62, 1, 1, 240, 10, 61, 1, 46,
				1, 4, 96, 11, 69, 1, 50, 1, 4, 184,
				10, 63, 1, 40, 1, 2, 240, 10, 65, 1,
				42, 1, 2, 156, 10, byte.MaxValue, byte.MaxValue, 38, 1, 1,
				184, 10, byte.MaxValue, byte.MaxValue, 39, 1, 1, 224, 10, 67,
				1, 0, 0, 0, 240, 10, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 204, 10, byte.MaxValue, byte.MaxValue, 44, 1, 1, 224, 10,
				byte.MaxValue, byte.MaxValue, 45, 1, 1, 32, 11, 71, 1, 0,
				0, 0, 96, 11, 73, 1, 0, 0, 0, 8,
				11, byte.MaxValue, byte.MaxValue, 54, 1, 3, 32, 11, byte.MaxValue, byte.MaxValue,
				57, 1, 3, 64, 11, byte.MaxValue, byte.MaxValue, 60, 1, 1,
				96, 11, byte.MaxValue, byte.MaxValue, 63, 1, 1, 208, 11, 77,
				1, 74, 1, 1, 64, 12, 103, 1, 75, 1,
				1, 152, 11, 79, 1, 0, 0, 0, 208, 11,
				87, 1, 0, 0, 0, 124, 11, byte.MaxValue, byte.MaxValue, 64,
				1, 1, 152, 11, 81, 1, 65, 1, 1, 136,
				11, 83, 1, 0, 0, 0, 152, 11, 85, 1,
				0, 0, 0, 130, 11, byte.MaxValue, byte.MaxValue, 66, 1, 2,
				136, 11, byte.MaxValue, byte.MaxValue, 68, 1, 2, 144, 11, byte.MaxValue,
				byte.MaxValue, 70, 1, 1, 152, 11, byte.MaxValue, byte.MaxValue, 71, 1,
				1, 180, 11, 89, 1, 0, 0, 0, 208, 11,
				byte.MaxValue, byte.MaxValue, 0, 0, 0, 166, 11, 91, 1, 0,
				0, 0, 180, 11, byte.MaxValue, byte.MaxValue, 0, 0, 0, 159,
				11, 93, 1, 0, 0, 0, 166, 11, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 156, 11, 95, 1, 0, 0, 0,
				159, 11, 99, 1, 0, 0, 0, 154, 11, 97,
				1, 0, 0, 0, 156, 11, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 153, 11, byte.MaxValue, byte.MaxValue, 72, 1, 1, 154, 11,
				byte.MaxValue, byte.MaxValue, 0, 0, 0, 158, 11, 101, 1, 0,
				0, 0, 159, 11, byte.MaxValue, byte.MaxValue, 0, 0, 0, 157,
				11, byte.MaxValue, byte.MaxValue, 73, 1, 1, 158, 11, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 8, 12, byte.MaxValue, byte.MaxValue, 76, 1, 2,
				64, 12, 105, 1, 78, 1, 2, 36, 12, byte.MaxValue,
				byte.MaxValue, 80, 1, 1, 64, 12, byte.MaxValue, byte.MaxValue, 81, 1,
				1, 32, 13, 109, 1, 106, 1, 1, 0, 14,
				125, 1, 107, 1, 1, 176, 12, 111, 1, 91,
				1, 4, 32, 13, 119, 1, 95, 1, 4, 120,
				12, 113, 1, 85, 1, 2, 176, 12, 115, 1,
				87, 1, 2, 92, 12, byte.MaxValue, byte.MaxValue, 83, 1, 1,
				120, 12, byte.MaxValue, byte.MaxValue, 84, 1, 1, 160, 12, 117,
				1, 0, 0, 0, 176, 12, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 140, 12, byte.MaxValue, byte.MaxValue, 89, 1, 1, 160, 12,
				byte.MaxValue, byte.MaxValue, 90, 1, 1, 224, 12, 121, 1, 0,
				0, 0, 32, 13, 123, 1, 0, 0, 0, 200,
				12, byte.MaxValue, byte.MaxValue, 99, 1, 3, 224, 12, byte.MaxValue, byte.MaxValue,
				102, 1, 3, 0, 13, byte.MaxValue, byte.MaxValue, 105, 1, 1,
				32, 13, byte.MaxValue, byte.MaxValue, 108, 1, 1, 144, 13, 127,
				1, 119, 1, 1, 0, 14, 153, 1, 120, 1,
				1, 88, 13, 129, 1, 0, 0, 0, 144, 13,
				137, 1, 0, 0, 0, 60, 13, byte.MaxValue, byte.MaxValue, 109,
				1, 1, 88, 13, 131, 1, 110, 1, 1, 72,
				13, 133, 1, 0, 0, 0, 88, 13, 135, 1,
				0, 0, 0, 66, 13, byte.MaxValue, byte.MaxValue, 111, 1, 2,
				72, 13, byte.MaxValue, byte.MaxValue, 113, 1, 2, 80, 13, byte.MaxValue,
				byte.MaxValue, 115, 1, 1, 88, 13, byte.MaxValue, byte.MaxValue, 116, 1,
				1, 116, 13, 139, 1, 0, 0, 0, 144, 13,
				byte.MaxValue, byte.MaxValue, 0, 0, 0, 102, 13, 141, 1, 0,
				0, 0, 116, 13, byte.MaxValue, byte.MaxValue, 0, 0, 0, 95,
				13, 143, 1, 0, 0, 0, 102, 13, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 92, 13, 145, 1, 0, 0, 0,
				95, 13, 149, 1, 0, 0, 0, 90, 13, 147,
				1, 0, 0, 0, 92, 13, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 89, 13, byte.MaxValue, byte.MaxValue, 117, 1, 1, 90, 13,
				byte.MaxValue, byte.MaxValue, 0, 0, 0, 94, 13, 151, 1, 0,
				0, 0, 95, 13, byte.MaxValue, byte.MaxValue, 0, 0, 0, 93,
				13, byte.MaxValue, byte.MaxValue, 118, 1, 1, 94, 13, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 200, 13, byte.MaxValue, byte.MaxValue, 121, 1, 2,
				0, 14, 155, 1, 123, 1, 2, 228, 13, byte.MaxValue,
				byte.MaxValue, 125, 1, 1, 0, 14, byte.MaxValue, byte.MaxValue, 126, 1,
				1, 128, 17, 159, 1, 0, 0, 0, 63, 19,
				5, 2, 0, 0, 0, 192, 15, 161, 1, 127,
				1, 1, 128, 17, 211, 1, 172, 1, 1, 224,
				14, 163, 1, 151, 1, 1, 192, 15, 179, 1,
				152, 1, 1, 112, 14, 165, 1, 136, 1, 4,
				224, 14, 173, 1, 140, 1, 4, 56, 14, 167,
				1, 130, 1, 2, 112, 14, 169, 1, 132, 1,
				2, 28, 14, byte.MaxValue, byte.MaxValue, 128, 1, 1, 56, 14,
				byte.MaxValue, byte.MaxValue, 129, 1, 1, 96, 14, 171, 1, 0,
				0, 0, 112, 14, byte.MaxValue, byte.MaxValue, 0, 0, 0, 76,
				14, byte.MaxValue, byte.MaxValue, 134, 1, 1, 96, 14, byte.MaxValue, byte.MaxValue,
				135, 1, 1, 160, 14, 175, 1, 0, 0, 0,
				224, 14, 177, 1, 0, 0, 0, 136, 14, byte.MaxValue,
				byte.MaxValue, 144, 1, 3, 160, 14, byte.MaxValue, byte.MaxValue, 147, 1,
				3, 192, 14, byte.MaxValue, byte.MaxValue, 150, 1, 1, 224, 14,
				byte.MaxValue, byte.MaxValue, 153, 1, 1, 80, 15, 181, 1, 164,
				1, 1, 192, 15, 207, 1, 165, 1, 1, 24,
				15, 183, 1, 0, 0, 0, 80, 15, 191, 1,
				0, 0, 0, 252, 14, byte.MaxValue, byte.MaxValue, 154, 1, 1,
				24, 15, 185, 1, 155, 1, 1, 8, 15, 187,
				1, 0, 0, 0, 24, 15, 189, 1, 0, 0,
				0, 2, 15, byte.MaxValue, byte.MaxValue, 156, 1, 2, 8, 15,
				byte.MaxValue, byte.MaxValue, 158, 1, 2, 16, 15, byte.MaxValue, byte.MaxValue, 160,
				1, 1, 24, 15, byte.MaxValue, byte.MaxValue, 161, 1, 1, 52,
				15, 193, 1, 0, 0, 0, 80, 15, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 38, 15, 195, 1, 0, 0, 0,
				52, 15, byte.MaxValue, byte.MaxValue, 0, 0, 0, 31, 15, 197,
				1, 0, 0, 0, 38, 15, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 28, 15, 199, 1, 0, 0, 0, 31, 15,
				203, 1, 0, 0, 0, 26, 15, 201, 1, 0,
				0, 0, 28, 15, byte.MaxValue, byte.MaxValue, 0, 0, 0, 25,
				15, byte.MaxValue, byte.MaxValue, 162, 1, 1, 26, 15, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 30, 15, 205, 1, 0, 0, 0,
				31, 15, byte.MaxValue, byte.MaxValue, 0, 0, 0, 29, 15, byte.MaxValue,
				byte.MaxValue, 163, 1, 1, 30, 15, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 136, 15, byte.MaxValue, byte.MaxValue, 166, 1, 2, 192, 15,
				209, 1, 168, 1, 2, 164, 15, byte.MaxValue, byte.MaxValue, 170,
				1, 1, 192, 15, byte.MaxValue, byte.MaxValue, 171, 1, 1, 160,
				16, 213, 1, 196, 1, 1, 128, 17, 229, 1,
				197, 1, 1, 48, 16, 215, 1, 181, 1, 4,
				160, 16, 223, 1, 185, 1, 4, 248, 15, 217,
				1, 175, 1, 2, 48, 16, 219, 1, 177, 1,
				2, 220, 15, byte.MaxValue, byte.MaxValue, 173, 1, 1, 248, 15,
				byte.MaxValue, byte.MaxValue, 174, 1, 1, 32, 16, 221, 1, 0,
				0, 0, 48, 16, byte.MaxValue, byte.MaxValue, 0, 0, 0, 12,
				16, byte.MaxValue, byte.MaxValue, 179, 1, 1, 32, 16, byte.MaxValue, byte.MaxValue,
				180, 1, 1, 96, 16, 225, 1, 0, 0, 0,
				160, 16, 227, 1, 0, 0, 0, 72, 16, byte.MaxValue,
				byte.MaxValue, 189, 1, 3, 96, 16, byte.MaxValue, byte.MaxValue, 192, 1,
				3, 128, 16, byte.MaxValue, byte.MaxValue, 195, 1, 1, 160, 16,
				byte.MaxValue, byte.MaxValue, 198, 1, 1, 16, 17, 231, 1, 209,
				1, 1, 128, 17, 1, 2, 210, 1, 1, 216,
				16, 233, 1, 0, 0, 0, 16, 17, 241, 1,
				0, 0, 0, 188, 16, byte.MaxValue, byte.MaxValue, 199, 1, 1,
				216, 16, 235, 1, 200, 1, 1, 200, 16, 237,
				1, 0, 0, 0, 216, 16, 239, 1, 0, 0,
				0, 194, 16, byte.MaxValue, byte.MaxValue, 201, 1, 2, 200, 16,
				byte.MaxValue, byte.MaxValue, 203, 1, 2, 208, 16, byte.MaxValue, byte.MaxValue, 205,
				1, 1, 216, 16, byte.MaxValue, byte.MaxValue, 206, 1, 1, 244,
				16, 243, 1, 0, 0, 0, 16, 17, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 230, 16, 245, 1, 0, 0, 0,
				244, 16, byte.MaxValue, byte.MaxValue, 0, 0, 0, 223, 16, 247,
				1, 0, 0, 0, 230, 16, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 220, 16, 249, 1, 0, 0, 0, 223, 16,
				253, 1, 0, 0, 0, 218, 16, 251, 1, 0,
				0, 0, 220, 16, byte.MaxValue, byte.MaxValue, 0, 0, 0, 217,
				16, byte.MaxValue, byte.MaxValue, 207, 1, 1, 218, 16, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 222, 16, byte.MaxValue, 1, 0, 0, 0,
				223, 16, byte.MaxValue, byte.MaxValue, 0, 0, 0, 221, 16, byte.MaxValue,
				byte.MaxValue, 208, 1, 1, 222, 16, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 72, 17, byte.MaxValue, byte.MaxValue, 211, 1, 2, 128, 17,
				3, 2, 213, 1, 2, 100, 17, byte.MaxValue, byte.MaxValue, 215,
				1, 1, 128, 17, byte.MaxValue, byte.MaxValue, 216, 1, 1, 96,
				18, 7, 2, 217, 1, 2, 63, 19, 23, 2,
				219, 1, 2, 240, 17, 9, 2, 229, 1, 4,
				96, 18, 17, 2, 233, 1, 4, 184, 17, 11,
				2, 223, 1, 2, 240, 17, 13, 2, 225, 1,
				2, 156, 17, byte.MaxValue, byte.MaxValue, 221, 1, 1, 184, 17,
				byte.MaxValue, byte.MaxValue, 222, 1, 1, 224, 17, 15, 2, 0,
				0, 0, 240, 17, byte.MaxValue, byte.MaxValue, 0, 0, 0, 204,
				17, byte.MaxValue, byte.MaxValue, 227, 1, 1, 224, 17, byte.MaxValue, byte.MaxValue,
				228, 1, 1, 32, 18, 19, 2, 0, 0, 0,
				96, 18, 21, 2, 0, 0, 0, 8, 18, byte.MaxValue,
				byte.MaxValue, 237, 1, 3, 32, 18, byte.MaxValue, byte.MaxValue, 240, 1,
				3, 64, 18, byte.MaxValue, byte.MaxValue, 243, 1, 1, 96, 18,
				byte.MaxValue, byte.MaxValue, 244, 1, 1, 208, 18, 25, 2, byte.MaxValue,
				1, 1, 63, 19, 51, 2, 0, 2, 1, 152,
				18, 27, 2, 0, 0, 0, 208, 18, 35, 2,
				0, 0, 0, 124, 18, byte.MaxValue, byte.MaxValue, 245, 1, 1,
				152, 18, 29, 2, 246, 1, 1, 136, 18, 31,
				2, 0, 0, 0, 152, 18, 33, 2, 0, 0,
				0, 130, 18, byte.MaxValue, byte.MaxValue, 247, 1, 2, 136, 18,
				byte.MaxValue, byte.MaxValue, 249, 1, 2, 144, 18, byte.MaxValue, byte.MaxValue, 251,
				1, 1, 152, 18, byte.MaxValue, byte.MaxValue, 252, 1, 1, 180,
				18, 37, 2, 0, 0, 0, 208, 18, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 166, 18, 39, 2, 0, 0, 0,
				180, 18, byte.MaxValue, byte.MaxValue, 0, 0, 0, 159, 18, 41,
				2, 0, 0, 0, 166, 18, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 156, 18, 43, 2, 0, 0, 0, 159, 18,
				47, 2, 0, 0, 0, 154, 18, 45, 2, 0,
				0, 0, 156, 18, byte.MaxValue, byte.MaxValue, 0, 0, 0, 153,
				18, byte.MaxValue, byte.MaxValue, 253, 1, 1, 154, 18, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 158, 18, 49, 2, 0, 0, 0,
				159, 18, byte.MaxValue, byte.MaxValue, 0, 0, 0, 157, 18, byte.MaxValue,
				byte.MaxValue, 254, 1, 1, 158, 18, byte.MaxValue, byte.MaxValue, 0, 0,
				0, 0, 19, byte.MaxValue, byte.MaxValue, 0, 0, 0, 63, 19,
				53, 2, 0, 0, 0, 32, 19, byte.MaxValue, byte.MaxValue, 1,
				2, 2, 63, 19, 55, 2, 3, 2, 1, 48,
				19, byte.MaxValue, byte.MaxValue, 4, 2, 1, 63, 19, byte.MaxValue, byte.MaxValue,
				5, 2, 1
			}, new ushort[]
			{
				0, 17, 18, 1, 30, 42, 31, 43, 19, 32,
				33, 37, 2, 44, 45, 49, 19, 32, 33, 37,
				2, 44, 45, 49, 34, 35, 36, 46, 47, 48,
				34, 35, 36, 46, 47, 48, 20, 3, 20, 3,
				21, 39, 4, 51, 21, 39, 4, 51, 38, 50,
				38, 50, 22, 23, 5, 22, 23, 5, 24, 25,
				6, 26, 27, 28, 28, 29, 40, 41, 7, 52,
				52, 53, 65, 53, 65, 66, 66, 54, 69, 70,
				71, 54, 69, 70, 71, 67, 68, 72, 67, 68,
				72, 55, 56, 56, 73, 74, 74, 57, 58, 57,
				58, 59, 60, 61, 62, 63, 63, 64, 75, 64,
				75, 76, 76, 8, 77, 77, 78, 90, 78, 90,
				91, 91, 79, 94, 95, 96, 79, 94, 95, 96,
				92, 93, 97, 92, 93, 97, 80, 81, 81, 98,
				99, 99, 82, 83, 82, 83, 84, 85, 86, 87,
				88, 88, 89, 100, 89, 100, 101, 101, 9, 102,
				102, 103, 115, 103, 115, 116, 116, 104, 119, 120,
				121, 104, 119, 120, 121, 117, 118, 122, 117, 118,
				122, 105, 106, 106, 123, 124, 124, 107, 108, 107,
				108, 109, 110, 111, 112, 113, 113, 114, 125, 114,
				125, 126, 126, 10, 127, 127, 128, 140, 128, 140,
				141, 141, 129, 144, 145, 146, 129, 144, 145, 146,
				142, 143, 147, 142, 143, 147, 130, 131, 131, 148,
				149, 149, 132, 133, 132, 133, 134, 135, 136, 137,
				138, 138, 139, 150, 139, 150, 151, 151, 11, 152,
				152, 153, 165, 153, 165, 166, 166, 154, 169, 170,
				171, 154, 169, 170, 171, 167, 168, 172, 167, 168,
				172, 155, 156, 156, 173, 174, 174, 157, 158, 157,
				158, 159, 160, 161, 162, 163, 163, 164, 175, 164,
				175, 176, 176, 12, 177, 177, 178, 190, 178, 190,
				191, 191, 179, 194, 195, 196, 179, 194, 195, 196,
				192, 193, 197, 192, 193, 197, 180, 181, 181, 198,
				199, 199, 182, 183, 182, 183, 184, 185, 186, 187,
				188, 188, 189, 200, 189, 200, 201, 201, 13, 202,
				202, 203, 215, 203, 215, 216, 216, 204, 219, 220,
				221, 204, 219, 220, 221, 217, 218, 222, 217, 218,
				222, 205, 206, 206, 223, 224, 224, 207, 208, 207,
				208, 209, 210, 211, 212, 213, 213, 214, 225, 214,
				225, 226, 226, 14, 227, 227, 228, 240, 228, 240,
				241, 241, 229, 244, 245, 246, 229, 244, 245, 246,
				242, 243, 247, 242, 243, 247, 230, 231, 231, 248,
				249, 249, 232, 233, 232, 233, 234, 235, 236, 237,
				238, 238, 239, 250, 239, 250, 251, 251, 15, 252,
				252, 253, 265, 253, 265, 266, 266, 254, 269, 270,
				271, 254, 269, 270, 271, 267, 268, 272, 267, 268,
				272, 255, 256, 256, 273, 274, 274, 257, 258, 257,
				258, 259, 260, 261, 262, 263, 263, 264, 275, 264,
				275, 276, 276, 16, 281, 16, 281, 277, 277, 278,
				290, 278, 290, 291, 291, 279, 294, 295, 296, 279,
				294, 295, 296, 292, 293, 297, 292, 293, 297, 280,
				298, 299, 299, 282, 283, 282, 283, 284, 285, 286,
				287, 288, 288, 289, 300, 289, 301, 301
			});
			deviceBuilder.Finish();
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00020044 File Offset: 0x0001E244
		private TouchControl Initialize_ctrlTouchscreenprimaryTouch(InternedString kTouchLayout, InputControl parent)
		{
			TouchControl touchControl = new TouchControl();
			touchControl.Setup().At(this, 0).WithParent(parent)
				.WithChildren(17, 13)
				.WithName("primaryTouch")
				.WithDisplayName("Primary Touch")
				.WithLayout(kTouchLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1414485315),
					byteOffset = 0U,
					bitOffset = 0U,
					sizeInBits = 448U
				})
				.Finish();
			return touchControl;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x000200F0 File Offset: 0x0001E2F0
		private Vector2Control Initialize_ctrlTouchscreenposition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 1).WithParent(parent)
				.WithChildren(42, 2)
				.WithName("position")
				.WithDisplayName("Position")
				.WithLayout(kVector2Layout)
				.WithUsages(1, 1)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 4U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x000201A0 File Offset: 0x0001E3A0
		private DeltaControl Initialize_ctrlTouchscreendelta(InternedString kDeltaLayout, InputControl parent)
		{
			DeltaControl deltaControl = new DeltaControl();
			deltaControl.Setup().At(this, 2).WithParent(parent)
				.WithChildren(44, 6)
				.WithName("delta")
				.WithDisplayName("Delta")
				.WithLayout(kDeltaLayout)
				.WithUsages(2, 1)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 12U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return deltaControl;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00020248 File Offset: 0x0001E448
		private AxisControl Initialize_ctrlTouchscreenpressure(InternedString kAnalogLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 3).WithParent(parent)
				.WithName("pressure")
				.WithDisplayName("Pressure")
				.WithLayout(kAnalogLayout)
				.WithUsages(3, 1)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 20U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.WithDefaultState(1)
				.Finish();
			return axisControl;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x000202F4 File Offset: 0x0001E4F4
		private Vector2Control Initialize_ctrlTouchscreenradius(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 4).WithParent(parent)
				.WithChildren(50, 2)
				.WithName("radius")
				.WithDisplayName("Radius")
				.WithLayout(kVector2Layout)
				.WithUsages(4, 1)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 24U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0002039C File Offset: 0x0001E59C
		private TouchPressControl Initialize_ctrlTouchscreenpress(InternedString kTouchPressLayout, InputControl parent)
		{
			TouchPressControl touchPressControl = new TouchPressControl();
			touchPressControl.Setup().At(this, 5).WithParent(parent)
				.WithName("press")
				.WithDisplayName("Press")
				.WithLayout(kTouchPressLayout)
				.IsSynthetic(true)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 32U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return touchPressControl;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00020454 File Offset: 0x0001E654
		private IntegerControl Initialize_ctrlTouchscreendisplayIndex(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 6).WithParent(parent)
				.WithName("displayIndex")
				.WithDisplayName("Display Index")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 34U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000204E8 File Offset: 0x0001E6E8
		private TouchControl Initialize_ctrlTouchscreentouch0(InternedString kTouchLayout, InputControl parent)
		{
			TouchControl touchControl = new TouchControl();
			touchControl.Setup().At(this, 7).WithParent(parent)
				.WithChildren(52, 13)
				.WithName("touch0")
				.WithDisplayName("Touch")
				.WithLayout(kTouchLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1414485315),
					byteOffset = 56U,
					bitOffset = 0U,
					sizeInBits = 448U
				})
				.Finish();
			return touchControl;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0002058C File Offset: 0x0001E78C
		private TouchControl Initialize_ctrlTouchscreentouch1(InternedString kTouchLayout, InputControl parent)
		{
			TouchControl touchControl = new TouchControl();
			touchControl.Setup().At(this, 8).WithParent(parent)
				.WithChildren(77, 13)
				.WithName("touch1")
				.WithDisplayName("Touch")
				.WithLayout(kTouchLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1414485315),
					byteOffset = 112U,
					bitOffset = 0U,
					sizeInBits = 448U
				})
				.Finish();
			return touchControl;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00020630 File Offset: 0x0001E830
		private TouchControl Initialize_ctrlTouchscreentouch2(InternedString kTouchLayout, InputControl parent)
		{
			TouchControl touchControl = new TouchControl();
			touchControl.Setup().At(this, 9).WithParent(parent)
				.WithChildren(102, 13)
				.WithName("touch2")
				.WithDisplayName("Touch")
				.WithLayout(kTouchLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1414485315),
					byteOffset = 168U,
					bitOffset = 0U,
					sizeInBits = 448U
				})
				.Finish();
			return touchControl;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x000206D8 File Offset: 0x0001E8D8
		private TouchControl Initialize_ctrlTouchscreentouch3(InternedString kTouchLayout, InputControl parent)
		{
			TouchControl touchControl = new TouchControl();
			touchControl.Setup().At(this, 10).WithParent(parent)
				.WithChildren(127, 13)
				.WithName("touch3")
				.WithDisplayName("Touch")
				.WithLayout(kTouchLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1414485315),
					byteOffset = 224U,
					bitOffset = 0U,
					sizeInBits = 448U
				})
				.Finish();
			return touchControl;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00020780 File Offset: 0x0001E980
		private TouchControl Initialize_ctrlTouchscreentouch4(InternedString kTouchLayout, InputControl parent)
		{
			TouchControl touchControl = new TouchControl();
			touchControl.Setup().At(this, 11).WithParent(parent)
				.WithChildren(152, 13)
				.WithName("touch4")
				.WithDisplayName("Touch")
				.WithLayout(kTouchLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1414485315),
					byteOffset = 280U,
					bitOffset = 0U,
					sizeInBits = 448U
				})
				.Finish();
			return touchControl;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00020828 File Offset: 0x0001EA28
		private TouchControl Initialize_ctrlTouchscreentouch5(InternedString kTouchLayout, InputControl parent)
		{
			TouchControl touchControl = new TouchControl();
			touchControl.Setup().At(this, 12).WithParent(parent)
				.WithChildren(177, 13)
				.WithName("touch5")
				.WithDisplayName("Touch")
				.WithLayout(kTouchLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1414485315),
					byteOffset = 336U,
					bitOffset = 0U,
					sizeInBits = 448U
				})
				.Finish();
			return touchControl;
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000208D0 File Offset: 0x0001EAD0
		private TouchControl Initialize_ctrlTouchscreentouch6(InternedString kTouchLayout, InputControl parent)
		{
			TouchControl touchControl = new TouchControl();
			touchControl.Setup().At(this, 13).WithParent(parent)
				.WithChildren(202, 13)
				.WithName("touch6")
				.WithDisplayName("Touch")
				.WithLayout(kTouchLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1414485315),
					byteOffset = 392U,
					bitOffset = 0U,
					sizeInBits = 448U
				})
				.Finish();
			return touchControl;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00020978 File Offset: 0x0001EB78
		private TouchControl Initialize_ctrlTouchscreentouch7(InternedString kTouchLayout, InputControl parent)
		{
			TouchControl touchControl = new TouchControl();
			touchControl.Setup().At(this, 14).WithParent(parent)
				.WithChildren(227, 13)
				.WithName("touch7")
				.WithDisplayName("Touch")
				.WithLayout(kTouchLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1414485315),
					byteOffset = 448U,
					bitOffset = 0U,
					sizeInBits = 448U
				})
				.Finish();
			return touchControl;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00020A20 File Offset: 0x0001EC20
		private TouchControl Initialize_ctrlTouchscreentouch8(InternedString kTouchLayout, InputControl parent)
		{
			TouchControl touchControl = new TouchControl();
			touchControl.Setup().At(this, 15).WithParent(parent)
				.WithChildren(252, 13)
				.WithName("touch8")
				.WithDisplayName("Touch")
				.WithLayout(kTouchLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1414485315),
					byteOffset = 504U,
					bitOffset = 0U,
					sizeInBits = 448U
				})
				.Finish();
			return touchControl;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00020AC8 File Offset: 0x0001ECC8
		private TouchControl Initialize_ctrlTouchscreentouch9(InternedString kTouchLayout, InputControl parent)
		{
			TouchControl touchControl = new TouchControl();
			touchControl.Setup().At(this, 16).WithParent(parent)
				.WithChildren(277, 13)
				.WithName("touch9")
				.WithDisplayName("Touch")
				.WithLayout(kTouchLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1414485315),
					byteOffset = 560U,
					bitOffset = 0U,
					sizeInBits = 448U
				})
				.Finish();
			return touchControl;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00020B70 File Offset: 0x0001ED70
		private IntegerControl Initialize_ctrlTouchscreenprimaryTouchtouchId(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 17).WithParent(parent)
				.WithName("touchId")
				.WithDisplayName("Primary Touch Touch ID")
				.WithShortDisplayName("Primary Touch Touch ID")
				.WithLayout(kIntegerLayout)
				.IsSynthetic(true)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1229870112),
					byteOffset = 0U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00020C24 File Offset: 0x0001EE24
		private Vector2Control Initialize_ctrlTouchscreenprimaryTouchposition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 18).WithParent(parent)
				.WithChildren(30, 2)
				.WithName("position")
				.WithDisplayName("Primary Touch Position")
				.WithShortDisplayName("Primary Touch Position")
				.WithLayout(kVector2Layout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 4U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00020CD8 File Offset: 0x0001EED8
		private DeltaControl Initialize_ctrlTouchscreenprimaryTouchdelta(InternedString kDeltaLayout, InputControl parent)
		{
			DeltaControl deltaControl = new DeltaControl();
			deltaControl.Setup().At(this, 19).WithParent(parent)
				.WithChildren(32, 6)
				.WithName("delta")
				.WithDisplayName("Primary Touch Delta")
				.WithShortDisplayName("Primary Touch Delta")
				.WithLayout(kDeltaLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 12U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return deltaControl;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00020D84 File Offset: 0x0001EF84
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchpressure(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 20).WithParent(parent)
				.WithName("pressure")
				.WithDisplayName("Primary Touch Pressure")
				.WithShortDisplayName("Primary Touch Pressure")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 20U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00020E24 File Offset: 0x0001F024
		private Vector2Control Initialize_ctrlTouchscreenprimaryTouchradius(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 21).WithParent(parent)
				.WithChildren(38, 2)
				.WithName("radius")
				.WithDisplayName("Primary Touch Radius")
				.WithShortDisplayName("Primary Touch Radius")
				.WithLayout(kVector2Layout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 24U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00020ED0 File Offset: 0x0001F0D0
		private TouchPhaseControl Initialize_ctrlTouchscreenprimaryTouchphase(InternedString kTouchPhaseLayout, InputControl parent)
		{
			TouchPhaseControl touchPhaseControl = new TouchPhaseControl();
			touchPhaseControl.Setup().At(this, 22).WithParent(parent)
				.WithName("phase")
				.WithDisplayName("Primary Touch Touch Phase")
				.WithShortDisplayName("Primary Touch Touch Phase")
				.WithLayout(kTouchPhaseLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 32U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return touchPhaseControl;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00020F78 File Offset: 0x0001F178
		private TouchPressControl Initialize_ctrlTouchscreenprimaryTouchpress(InternedString kTouchPressLayout, InputControl parent)
		{
			TouchPressControl touchPressControl = new TouchPressControl();
			touchPressControl.Setup().At(this, 23).WithParent(parent)
				.WithName("press")
				.WithDisplayName("Primary Touch Touch Contact?")
				.WithShortDisplayName("Primary Touch Touch Contact?")
				.WithLayout(kTouchPressLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 32U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return touchPressControl;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00021034 File Offset: 0x0001F234
		private IntegerControl Initialize_ctrlTouchscreenprimaryTouchtapCount(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 24).WithParent(parent)
				.WithName("tapCount")
				.WithDisplayName("Primary Touch Tap Count")
				.WithShortDisplayName("Primary Touch Tap Count")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 33U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000210D4 File Offset: 0x0001F2D4
		private IntegerControl Initialize_ctrlTouchscreenprimaryTouchdisplayIndex(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 25).WithParent(parent)
				.WithName("displayIndex")
				.WithDisplayName("Primary Touch Display Index")
				.WithShortDisplayName("Primary Touch Display Index")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 34U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00021174 File Offset: 0x0001F374
		private ButtonControl Initialize_ctrlTouchscreenprimaryTouchindirectTouch(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 26).WithParent(parent)
				.WithName("indirectTouch")
				.WithDisplayName("Primary Touch Indirect Touch?")
				.WithShortDisplayName("Primary Touch Indirect Touch?")
				.WithLayout(kButtonLayout)
				.IsSynthetic(true)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 35U,
					bitOffset = 0U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0002123C File Offset: 0x0001F43C
		private ButtonControl Initialize_ctrlTouchscreenprimaryTouchtap(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 27).WithParent(parent)
				.WithName("tap")
				.WithDisplayName("Primary Touch Tap")
				.WithShortDisplayName("Primary Touch Tap")
				.WithLayout(kButtonLayout)
				.WithUsages(0, 1)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 35U,
					bitOffset = 4U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00021304 File Offset: 0x0001F504
		private DoubleControl Initialize_ctrlTouchscreenprimaryTouchstartTime(InternedString kDoubleLayout, InputControl parent)
		{
			DoubleControl doubleControl = new DoubleControl();
			doubleControl.Setup().At(this, 28).WithParent(parent)
				.WithName("startTime")
				.WithDisplayName("Primary Touch Start Time")
				.WithShortDisplayName("Primary Touch Start Time")
				.WithLayout(kDoubleLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1145195552),
					byteOffset = 40U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return doubleControl;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x000213B0 File Offset: 0x0001F5B0
		private Vector2Control Initialize_ctrlTouchscreenprimaryTouchstartPosition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 29).WithParent(parent)
				.WithChildren(40, 2)
				.WithName("startPosition")
				.WithDisplayName("Primary Touch Start Position")
				.WithShortDisplayName("Primary Touch Start Position")
				.WithLayout(kVector2Layout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 48U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00021464 File Offset: 0x0001F664
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchpositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 30).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Primary Touch Primary Touch Position X")
				.WithShortDisplayName("Primary Touch Primary Touch Position X")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 4U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0002150C File Offset: 0x0001F70C
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchpositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 31).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Primary Touch Primary Touch Position Y")
				.WithShortDisplayName("Primary Touch Primary Touch Position Y")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 8U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x000215B4 File Offset: 0x0001F7B4
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchdeltaup(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 32).WithParent(parent)
				.WithName("up")
				.WithDisplayName("Primary Touch Primary Touch Delta Up")
				.WithShortDisplayName("Primary Touch Primary Touch Delta Up")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 16U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00021670 File Offset: 0x0001F870
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchdeltadown(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 33).WithParent(parent)
				.WithName("down")
				.WithDisplayName("Primary Touch Primary Touch Delta Down")
				.WithShortDisplayName("Primary Touch Primary Touch Delta Down")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 16U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00021734 File Offset: 0x0001F934
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchdeltaleft(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 34).WithParent(parent)
				.WithName("left")
				.WithDisplayName("Primary Touch Primary Touch Delta Left")
				.WithShortDisplayName("Primary Touch Primary Touch Delta Left")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 12U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000217F8 File Offset: 0x0001F9F8
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchdeltaright(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 35).WithParent(parent)
				.WithName("right")
				.WithDisplayName("Primary Touch Primary Touch Delta Right")
				.WithShortDisplayName("Primary Touch Primary Touch Delta Right")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 12U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x000218B4 File Offset: 0x0001FAB4
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchdeltax(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 36).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Primary Touch Primary Touch Delta X")
				.WithShortDisplayName("Primary Touch Primary Touch Delta X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 12U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00021954 File Offset: 0x0001FB54
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchdeltay(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 37).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Primary Touch Primary Touch Delta Y")
				.WithShortDisplayName("Primary Touch Primary Touch Delta Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 16U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000219F4 File Offset: 0x0001FBF4
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchradiusx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 38).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Primary Touch Primary Touch Radius X")
				.WithShortDisplayName("Primary Touch Primary Touch Radius X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 24U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00021A94 File Offset: 0x0001FC94
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchradiusy(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 39).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Primary Touch Primary Touch Radius Y")
				.WithShortDisplayName("Primary Touch Primary Touch Radius Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 28U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00021B34 File Offset: 0x0001FD34
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchstartPositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 40).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Primary Touch Primary Touch Start Position X")
				.WithShortDisplayName("Primary Touch Primary Touch Start Position X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 48U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00021BD4 File Offset: 0x0001FDD4
		private AxisControl Initialize_ctrlTouchscreenprimaryTouchstartPositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 41).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Primary Touch Primary Touch Start Position Y")
				.WithShortDisplayName("Primary Touch Primary Touch Start Position Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 52U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00021C74 File Offset: 0x0001FE74
		private AxisControl Initialize_ctrlTouchscreenpositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 42).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Position X")
				.WithShortDisplayName("Position X")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 4U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00021D1C File Offset: 0x0001FF1C
		private AxisControl Initialize_ctrlTouchscreenpositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 43).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Position Y")
				.WithShortDisplayName("Position Y")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 8U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00021DC4 File Offset: 0x0001FFC4
		private AxisControl Initialize_ctrlTouchscreendeltaup(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 44).WithParent(parent)
				.WithName("up")
				.WithDisplayName("Delta Up")
				.WithShortDisplayName("Delta Up")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 16U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00021E80 File Offset: 0x00020080
		private AxisControl Initialize_ctrlTouchscreendeltadown(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 45).WithParent(parent)
				.WithName("down")
				.WithDisplayName("Delta Down")
				.WithShortDisplayName("Delta Down")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 16U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00021F44 File Offset: 0x00020144
		private AxisControl Initialize_ctrlTouchscreendeltaleft(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 46).WithParent(parent)
				.WithName("left")
				.WithDisplayName("Delta Left")
				.WithShortDisplayName("Delta Left")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 12U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00022008 File Offset: 0x00020208
		private AxisControl Initialize_ctrlTouchscreendeltaright(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 47).WithParent(parent)
				.WithName("right")
				.WithDisplayName("Delta Right")
				.WithShortDisplayName("Delta Right")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 12U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x000220C4 File Offset: 0x000202C4
		private AxisControl Initialize_ctrlTouchscreendeltax(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 48).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Delta X")
				.WithShortDisplayName("Delta X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 12U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00022164 File Offset: 0x00020364
		private AxisControl Initialize_ctrlTouchscreendeltay(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 49).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Delta Y")
				.WithShortDisplayName("Delta Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 16U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00022204 File Offset: 0x00020404
		private AxisControl Initialize_ctrlTouchscreenradiusx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 50).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Radius X")
				.WithShortDisplayName("Radius X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 24U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000222A4 File Offset: 0x000204A4
		private AxisControl Initialize_ctrlTouchscreenradiusy(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 51).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Radius Y")
				.WithShortDisplayName("Radius Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 28U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00022344 File Offset: 0x00020544
		private IntegerControl Initialize_ctrlTouchscreentouch0touchId(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 52).WithParent(parent)
				.WithName("touchId")
				.WithDisplayName("Touch Touch ID")
				.WithShortDisplayName("Touch Touch ID")
				.WithLayout(kIntegerLayout)
				.IsSynthetic(true)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1229870112),
					byteOffset = 56U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x000223F8 File Offset: 0x000205F8
		private Vector2Control Initialize_ctrlTouchscreentouch0position(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 53).WithParent(parent)
				.WithChildren(65, 2)
				.WithName("position")
				.WithDisplayName("Touch Position")
				.WithShortDisplayName("Touch Position")
				.WithLayout(kVector2Layout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 60U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x000224AC File Offset: 0x000206AC
		private DeltaControl Initialize_ctrlTouchscreentouch0delta(InternedString kDeltaLayout, InputControl parent)
		{
			DeltaControl deltaControl = new DeltaControl();
			deltaControl.Setup().At(this, 54).WithParent(parent)
				.WithChildren(67, 6)
				.WithName("delta")
				.WithDisplayName("Touch Delta")
				.WithShortDisplayName("Touch Delta")
				.WithLayout(kDeltaLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 68U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return deltaControl;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00022558 File Offset: 0x00020758
		private AxisControl Initialize_ctrlTouchscreentouch0pressure(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 55).WithParent(parent)
				.WithName("pressure")
				.WithDisplayName("Touch Pressure")
				.WithShortDisplayName("Touch Pressure")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 76U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x000225F8 File Offset: 0x000207F8
		private Vector2Control Initialize_ctrlTouchscreentouch0radius(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 56).WithParent(parent)
				.WithChildren(73, 2)
				.WithName("radius")
				.WithDisplayName("Touch Radius")
				.WithShortDisplayName("Touch Radius")
				.WithLayout(kVector2Layout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 80U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000226A4 File Offset: 0x000208A4
		private TouchPhaseControl Initialize_ctrlTouchscreentouch0phase(InternedString kTouchPhaseLayout, InputControl parent)
		{
			TouchPhaseControl touchPhaseControl = new TouchPhaseControl();
			touchPhaseControl.Setup().At(this, 57).WithParent(parent)
				.WithName("phase")
				.WithDisplayName("Touch Touch Phase")
				.WithShortDisplayName("Touch Touch Phase")
				.WithLayout(kTouchPhaseLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 88U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return touchPhaseControl;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0002274C File Offset: 0x0002094C
		private TouchPressControl Initialize_ctrlTouchscreentouch0press(InternedString kTouchPressLayout, InputControl parent)
		{
			TouchPressControl touchPressControl = new TouchPressControl();
			touchPressControl.Setup().At(this, 58).WithParent(parent)
				.WithName("press")
				.WithDisplayName("Touch Touch Contact?")
				.WithShortDisplayName("Touch Touch Contact?")
				.WithLayout(kTouchPressLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 88U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return touchPressControl;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00022808 File Offset: 0x00020A08
		private IntegerControl Initialize_ctrlTouchscreentouch0tapCount(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 59).WithParent(parent)
				.WithName("tapCount")
				.WithDisplayName("Touch Tap Count")
				.WithShortDisplayName("Touch Tap Count")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 89U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x000228A8 File Offset: 0x00020AA8
		private IntegerControl Initialize_ctrlTouchscreentouch0displayIndex(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 60).WithParent(parent)
				.WithName("displayIndex")
				.WithDisplayName("Touch Display Index")
				.WithShortDisplayName("Touch Display Index")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 90U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00022948 File Offset: 0x00020B48
		private ButtonControl Initialize_ctrlTouchscreentouch0indirectTouch(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 61).WithParent(parent)
				.WithName("indirectTouch")
				.WithDisplayName("Touch Indirect Touch?")
				.WithShortDisplayName("Touch Indirect Touch?")
				.WithLayout(kButtonLayout)
				.IsSynthetic(true)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 91U,
					bitOffset = 0U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00022A10 File Offset: 0x00020C10
		private ButtonControl Initialize_ctrlTouchscreentouch0tap(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 62).WithParent(parent)
				.WithName("tap")
				.WithDisplayName("Touch Tap")
				.WithShortDisplayName("Touch Tap")
				.WithLayout(kButtonLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 91U,
					bitOffset = 4U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00022ACC File Offset: 0x00020CCC
		private DoubleControl Initialize_ctrlTouchscreentouch0startTime(InternedString kDoubleLayout, InputControl parent)
		{
			DoubleControl doubleControl = new DoubleControl();
			doubleControl.Setup().At(this, 63).WithParent(parent)
				.WithName("startTime")
				.WithDisplayName("Touch Start Time")
				.WithShortDisplayName("Touch Start Time")
				.WithLayout(kDoubleLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1145195552),
					byteOffset = 96U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return doubleControl;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00022B78 File Offset: 0x00020D78
		private Vector2Control Initialize_ctrlTouchscreentouch0startPosition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 64).WithParent(parent)
				.WithChildren(75, 2)
				.WithName("startPosition")
				.WithDisplayName("Touch Start Position")
				.WithShortDisplayName("Touch Start Position")
				.WithLayout(kVector2Layout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 104U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00022C2C File Offset: 0x00020E2C
		private AxisControl Initialize_ctrlTouchscreentouch0positionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 65).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Position X")
				.WithShortDisplayName("Touch Touch Position X")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 60U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00022CD8 File Offset: 0x00020ED8
		private AxisControl Initialize_ctrlTouchscreentouch0positiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 66).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Position Y")
				.WithShortDisplayName("Touch Touch Position Y")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 64U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00022D84 File Offset: 0x00020F84
		private AxisControl Initialize_ctrlTouchscreentouch0deltaup(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 67).WithParent(parent)
				.WithName("up")
				.WithDisplayName("Touch Touch Delta Up")
				.WithShortDisplayName("Touch Touch Delta Up")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 72U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00022E40 File Offset: 0x00021040
		private AxisControl Initialize_ctrlTouchscreentouch0deltadown(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 68).WithParent(parent)
				.WithName("down")
				.WithDisplayName("Touch Touch Delta Down")
				.WithShortDisplayName("Touch Touch Delta Down")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 72U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00022F04 File Offset: 0x00021104
		private AxisControl Initialize_ctrlTouchscreentouch0deltaleft(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 69).WithParent(parent)
				.WithName("left")
				.WithDisplayName("Touch Touch Delta Left")
				.WithShortDisplayName("Touch Touch Delta Left")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 68U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00022FC8 File Offset: 0x000211C8
		private AxisControl Initialize_ctrlTouchscreentouch0deltaright(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 70).WithParent(parent)
				.WithName("right")
				.WithDisplayName("Touch Touch Delta Right")
				.WithShortDisplayName("Touch Touch Delta Right")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 68U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00023084 File Offset: 0x00021284
		private AxisControl Initialize_ctrlTouchscreentouch0deltax(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 71).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Delta X")
				.WithShortDisplayName("Touch Touch Delta X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 68U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00023124 File Offset: 0x00021324
		private AxisControl Initialize_ctrlTouchscreentouch0deltay(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 72).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Delta Y")
				.WithShortDisplayName("Touch Touch Delta Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 72U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x000231C4 File Offset: 0x000213C4
		private AxisControl Initialize_ctrlTouchscreentouch0radiusx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 73).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Radius X")
				.WithShortDisplayName("Touch Touch Radius X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 80U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00023264 File Offset: 0x00021464
		private AxisControl Initialize_ctrlTouchscreentouch0radiusy(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 74).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Radius Y")
				.WithShortDisplayName("Touch Touch Radius Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 84U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00023304 File Offset: 0x00021504
		private AxisControl Initialize_ctrlTouchscreentouch0startPositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 75).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Start Position X")
				.WithShortDisplayName("Touch Touch Start Position X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 104U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000233A4 File Offset: 0x000215A4
		private AxisControl Initialize_ctrlTouchscreentouch0startPositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 76).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Start Position Y")
				.WithShortDisplayName("Touch Touch Start Position Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 108U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00023444 File Offset: 0x00021644
		private IntegerControl Initialize_ctrlTouchscreentouch1touchId(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 77).WithParent(parent)
				.WithName("touchId")
				.WithDisplayName("Touch Touch ID")
				.WithShortDisplayName("Touch Touch ID")
				.WithLayout(kIntegerLayout)
				.IsSynthetic(true)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1229870112),
					byteOffset = 112U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x000234F8 File Offset: 0x000216F8
		private Vector2Control Initialize_ctrlTouchscreentouch1position(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 78).WithParent(parent)
				.WithChildren(90, 2)
				.WithName("position")
				.WithDisplayName("Touch Position")
				.WithShortDisplayName("Touch Position")
				.WithLayout(kVector2Layout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 116U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x000235AC File Offset: 0x000217AC
		private DeltaControl Initialize_ctrlTouchscreentouch1delta(InternedString kDeltaLayout, InputControl parent)
		{
			DeltaControl deltaControl = new DeltaControl();
			deltaControl.Setup().At(this, 79).WithParent(parent)
				.WithChildren(92, 6)
				.WithName("delta")
				.WithDisplayName("Touch Delta")
				.WithShortDisplayName("Touch Delta")
				.WithLayout(kDeltaLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 124U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return deltaControl;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00023658 File Offset: 0x00021858
		private AxisControl Initialize_ctrlTouchscreentouch1pressure(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 80).WithParent(parent)
				.WithName("pressure")
				.WithDisplayName("Touch Pressure")
				.WithShortDisplayName("Touch Pressure")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 132U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x000236FC File Offset: 0x000218FC
		private Vector2Control Initialize_ctrlTouchscreentouch1radius(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 81).WithParent(parent)
				.WithChildren(98, 2)
				.WithName("radius")
				.WithDisplayName("Touch Radius")
				.WithShortDisplayName("Touch Radius")
				.WithLayout(kVector2Layout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 136U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x000237AC File Offset: 0x000219AC
		private TouchPhaseControl Initialize_ctrlTouchscreentouch1phase(InternedString kTouchPhaseLayout, InputControl parent)
		{
			TouchPhaseControl touchPhaseControl = new TouchPhaseControl();
			touchPhaseControl.Setup().At(this, 82).WithParent(parent)
				.WithName("phase")
				.WithDisplayName("Touch Touch Phase")
				.WithShortDisplayName("Touch Touch Phase")
				.WithLayout(kTouchPhaseLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 144U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return touchPhaseControl;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00023858 File Offset: 0x00021A58
		private TouchPressControl Initialize_ctrlTouchscreentouch1press(InternedString kTouchPressLayout, InputControl parent)
		{
			TouchPressControl touchPressControl = new TouchPressControl();
			touchPressControl.Setup().At(this, 83).WithParent(parent)
				.WithName("press")
				.WithDisplayName("Touch Touch Contact?")
				.WithShortDisplayName("Touch Touch Contact?")
				.WithLayout(kTouchPressLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 144U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return touchPressControl;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00023918 File Offset: 0x00021B18
		private IntegerControl Initialize_ctrlTouchscreentouch1tapCount(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 84).WithParent(parent)
				.WithName("tapCount")
				.WithDisplayName("Touch Tap Count")
				.WithShortDisplayName("Touch Tap Count")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 145U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x000239BC File Offset: 0x00021BBC
		private IntegerControl Initialize_ctrlTouchscreentouch1displayIndex(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 85).WithParent(parent)
				.WithName("displayIndex")
				.WithDisplayName("Touch Display Index")
				.WithShortDisplayName("Touch Display Index")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 146U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00023A60 File Offset: 0x00021C60
		private ButtonControl Initialize_ctrlTouchscreentouch1indirectTouch(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 86).WithParent(parent)
				.WithName("indirectTouch")
				.WithDisplayName("Touch Indirect Touch?")
				.WithShortDisplayName("Touch Indirect Touch?")
				.WithLayout(kButtonLayout)
				.IsSynthetic(true)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 147U,
					bitOffset = 0U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00023B28 File Offset: 0x00021D28
		private ButtonControl Initialize_ctrlTouchscreentouch1tap(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 87).WithParent(parent)
				.WithName("tap")
				.WithDisplayName("Touch Tap")
				.WithShortDisplayName("Touch Tap")
				.WithLayout(kButtonLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 147U,
					bitOffset = 4U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00023BE8 File Offset: 0x00021DE8
		private DoubleControl Initialize_ctrlTouchscreentouch1startTime(InternedString kDoubleLayout, InputControl parent)
		{
			DoubleControl doubleControl = new DoubleControl();
			doubleControl.Setup().At(this, 88).WithParent(parent)
				.WithName("startTime")
				.WithDisplayName("Touch Start Time")
				.WithShortDisplayName("Touch Start Time")
				.WithLayout(kDoubleLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1145195552),
					byteOffset = 152U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return doubleControl;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00023C94 File Offset: 0x00021E94
		private Vector2Control Initialize_ctrlTouchscreentouch1startPosition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 89).WithParent(parent)
				.WithChildren(100, 2)
				.WithName("startPosition")
				.WithDisplayName("Touch Start Position")
				.WithShortDisplayName("Touch Start Position")
				.WithLayout(kVector2Layout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 160U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00023D4C File Offset: 0x00021F4C
		private AxisControl Initialize_ctrlTouchscreentouch1positionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 90).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Position X")
				.WithShortDisplayName("Touch Touch Position X")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 116U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00023DF8 File Offset: 0x00021FF8
		private AxisControl Initialize_ctrlTouchscreentouch1positiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 91).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Position Y")
				.WithShortDisplayName("Touch Touch Position Y")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 120U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00023EA4 File Offset: 0x000220A4
		private AxisControl Initialize_ctrlTouchscreentouch1deltaup(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 92).WithParent(parent)
				.WithName("up")
				.WithDisplayName("Touch Touch Delta Up")
				.WithShortDisplayName("Touch Touch Delta Up")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 128U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00023F64 File Offset: 0x00022164
		private AxisControl Initialize_ctrlTouchscreentouch1deltadown(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 93).WithParent(parent)
				.WithName("down")
				.WithDisplayName("Touch Touch Delta Down")
				.WithShortDisplayName("Touch Touch Delta Down")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 128U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0002402C File Offset: 0x0002222C
		private AxisControl Initialize_ctrlTouchscreentouch1deltaleft(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 94).WithParent(parent)
				.WithName("left")
				.WithDisplayName("Touch Touch Delta Left")
				.WithShortDisplayName("Touch Touch Delta Left")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 124U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x000240F0 File Offset: 0x000222F0
		private AxisControl Initialize_ctrlTouchscreentouch1deltaright(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 95).WithParent(parent)
				.WithName("right")
				.WithDisplayName("Touch Touch Delta Right")
				.WithShortDisplayName("Touch Touch Delta Right")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 124U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x000241AC File Offset: 0x000223AC
		private AxisControl Initialize_ctrlTouchscreentouch1deltax(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 96).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Delta X")
				.WithShortDisplayName("Touch Touch Delta X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 124U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0002424C File Offset: 0x0002244C
		private AxisControl Initialize_ctrlTouchscreentouch1deltay(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 97).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Delta Y")
				.WithShortDisplayName("Touch Touch Delta Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 128U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x000242F0 File Offset: 0x000224F0
		private AxisControl Initialize_ctrlTouchscreentouch1radiusx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 98).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Radius X")
				.WithShortDisplayName("Touch Touch Radius X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 136U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00024394 File Offset: 0x00022594
		private AxisControl Initialize_ctrlTouchscreentouch1radiusy(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 99).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Radius Y")
				.WithShortDisplayName("Touch Touch Radius Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 140U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00024438 File Offset: 0x00022638
		private AxisControl Initialize_ctrlTouchscreentouch1startPositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 100).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Start Position X")
				.WithShortDisplayName("Touch Touch Start Position X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 160U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x000244DC File Offset: 0x000226DC
		private AxisControl Initialize_ctrlTouchscreentouch1startPositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 101).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Start Position Y")
				.WithShortDisplayName("Touch Touch Start Position Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 164U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00024580 File Offset: 0x00022780
		private IntegerControl Initialize_ctrlTouchscreentouch2touchId(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 102).WithParent(parent)
				.WithName("touchId")
				.WithDisplayName("Touch Touch ID")
				.WithShortDisplayName("Touch Touch ID")
				.WithLayout(kIntegerLayout)
				.IsSynthetic(true)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1229870112),
					byteOffset = 168U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00024638 File Offset: 0x00022838
		private Vector2Control Initialize_ctrlTouchscreentouch2position(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 103).WithParent(parent)
				.WithChildren(115, 2)
				.WithName("position")
				.WithDisplayName("Touch Position")
				.WithShortDisplayName("Touch Position")
				.WithLayout(kVector2Layout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 172U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000246F0 File Offset: 0x000228F0
		private DeltaControl Initialize_ctrlTouchscreentouch2delta(InternedString kDeltaLayout, InputControl parent)
		{
			DeltaControl deltaControl = new DeltaControl();
			deltaControl.Setup().At(this, 104).WithParent(parent)
				.WithChildren(117, 6)
				.WithName("delta")
				.WithDisplayName("Touch Delta")
				.WithShortDisplayName("Touch Delta")
				.WithLayout(kDeltaLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 180U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return deltaControl;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x000247A0 File Offset: 0x000229A0
		private AxisControl Initialize_ctrlTouchscreentouch2pressure(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 105).WithParent(parent)
				.WithName("pressure")
				.WithDisplayName("Touch Pressure")
				.WithShortDisplayName("Touch Pressure")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 188U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00024844 File Offset: 0x00022A44
		private Vector2Control Initialize_ctrlTouchscreentouch2radius(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 106).WithParent(parent)
				.WithChildren(123, 2)
				.WithName("radius")
				.WithDisplayName("Touch Radius")
				.WithShortDisplayName("Touch Radius")
				.WithLayout(kVector2Layout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 192U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x000248F4 File Offset: 0x00022AF4
		private TouchPhaseControl Initialize_ctrlTouchscreentouch2phase(InternedString kTouchPhaseLayout, InputControl parent)
		{
			TouchPhaseControl touchPhaseControl = new TouchPhaseControl();
			touchPhaseControl.Setup().At(this, 107).WithParent(parent)
				.WithName("phase")
				.WithDisplayName("Touch Touch Phase")
				.WithShortDisplayName("Touch Touch Phase")
				.WithLayout(kTouchPhaseLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 200U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return touchPhaseControl;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x000249A0 File Offset: 0x00022BA0
		private TouchPressControl Initialize_ctrlTouchscreentouch2press(InternedString kTouchPressLayout, InputControl parent)
		{
			TouchPressControl touchPressControl = new TouchPressControl();
			touchPressControl.Setup().At(this, 108).WithParent(parent)
				.WithName("press")
				.WithDisplayName("Touch Touch Contact?")
				.WithShortDisplayName("Touch Touch Contact?")
				.WithLayout(kTouchPressLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 200U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return touchPressControl;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00024A60 File Offset: 0x00022C60
		private IntegerControl Initialize_ctrlTouchscreentouch2tapCount(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 109).WithParent(parent)
				.WithName("tapCount")
				.WithDisplayName("Touch Tap Count")
				.WithShortDisplayName("Touch Tap Count")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 201U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00024B04 File Offset: 0x00022D04
		private IntegerControl Initialize_ctrlTouchscreentouch2displayIndex(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 110).WithParent(parent)
				.WithName("displayIndex")
				.WithDisplayName("Touch Display Index")
				.WithShortDisplayName("Touch Display Index")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 202U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00024BA8 File Offset: 0x00022DA8
		private ButtonControl Initialize_ctrlTouchscreentouch2indirectTouch(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 111).WithParent(parent)
				.WithName("indirectTouch")
				.WithDisplayName("Touch Indirect Touch?")
				.WithShortDisplayName("Touch Indirect Touch?")
				.WithLayout(kButtonLayout)
				.IsSynthetic(true)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 203U,
					bitOffset = 0U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00024C70 File Offset: 0x00022E70
		private ButtonControl Initialize_ctrlTouchscreentouch2tap(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 112).WithParent(parent)
				.WithName("tap")
				.WithDisplayName("Touch Tap")
				.WithShortDisplayName("Touch Tap")
				.WithLayout(kButtonLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 203U,
					bitOffset = 4U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00024D30 File Offset: 0x00022F30
		private DoubleControl Initialize_ctrlTouchscreentouch2startTime(InternedString kDoubleLayout, InputControl parent)
		{
			DoubleControl doubleControl = new DoubleControl();
			doubleControl.Setup().At(this, 113).WithParent(parent)
				.WithName("startTime")
				.WithDisplayName("Touch Start Time")
				.WithShortDisplayName("Touch Start Time")
				.WithLayout(kDoubleLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1145195552),
					byteOffset = 208U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return doubleControl;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00024DDC File Offset: 0x00022FDC
		private Vector2Control Initialize_ctrlTouchscreentouch2startPosition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 114).WithParent(parent)
				.WithChildren(125, 2)
				.WithName("startPosition")
				.WithDisplayName("Touch Start Position")
				.WithShortDisplayName("Touch Start Position")
				.WithLayout(kVector2Layout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 216U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00024E94 File Offset: 0x00023094
		private AxisControl Initialize_ctrlTouchscreentouch2positionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 115).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Position X")
				.WithShortDisplayName("Touch Touch Position X")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 172U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00024F40 File Offset: 0x00023140
		private AxisControl Initialize_ctrlTouchscreentouch2positiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 116).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Position Y")
				.WithShortDisplayName("Touch Touch Position Y")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 176U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00024FEC File Offset: 0x000231EC
		private AxisControl Initialize_ctrlTouchscreentouch2deltaup(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 117).WithParent(parent)
				.WithName("up")
				.WithDisplayName("Touch Touch Delta Up")
				.WithShortDisplayName("Touch Touch Delta Up")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 184U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000250AC File Offset: 0x000232AC
		private AxisControl Initialize_ctrlTouchscreentouch2deltadown(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 118).WithParent(parent)
				.WithName("down")
				.WithDisplayName("Touch Touch Delta Down")
				.WithShortDisplayName("Touch Touch Delta Down")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 184U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00025174 File Offset: 0x00023374
		private AxisControl Initialize_ctrlTouchscreentouch2deltaleft(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 119).WithParent(parent)
				.WithName("left")
				.WithDisplayName("Touch Touch Delta Left")
				.WithShortDisplayName("Touch Touch Delta Left")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 180U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002523C File Offset: 0x0002343C
		private AxisControl Initialize_ctrlTouchscreentouch2deltaright(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 120).WithParent(parent)
				.WithName("right")
				.WithDisplayName("Touch Touch Delta Right")
				.WithShortDisplayName("Touch Touch Delta Right")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 180U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x000252FC File Offset: 0x000234FC
		private AxisControl Initialize_ctrlTouchscreentouch2deltax(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 121).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Delta X")
				.WithShortDisplayName("Touch Touch Delta X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 180U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x000253A0 File Offset: 0x000235A0
		private AxisControl Initialize_ctrlTouchscreentouch2deltay(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 122).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Delta Y")
				.WithShortDisplayName("Touch Touch Delta Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 184U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00025444 File Offset: 0x00023644
		private AxisControl Initialize_ctrlTouchscreentouch2radiusx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 123).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Radius X")
				.WithShortDisplayName("Touch Touch Radius X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 192U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000254E8 File Offset: 0x000236E8
		private AxisControl Initialize_ctrlTouchscreentouch2radiusy(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 124).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Radius Y")
				.WithShortDisplayName("Touch Touch Radius Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 196U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002558C File Offset: 0x0002378C
		private AxisControl Initialize_ctrlTouchscreentouch2startPositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 125).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Start Position X")
				.WithShortDisplayName("Touch Touch Start Position X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 216U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00025630 File Offset: 0x00023830
		private AxisControl Initialize_ctrlTouchscreentouch2startPositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 126).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Start Position Y")
				.WithShortDisplayName("Touch Touch Start Position Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 220U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000256D4 File Offset: 0x000238D4
		private IntegerControl Initialize_ctrlTouchscreentouch3touchId(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 127).WithParent(parent)
				.WithName("touchId")
				.WithDisplayName("Touch Touch ID")
				.WithShortDisplayName("Touch Touch ID")
				.WithLayout(kIntegerLayout)
				.IsSynthetic(true)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1229870112),
					byteOffset = 224U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0002578C File Offset: 0x0002398C
		private Vector2Control Initialize_ctrlTouchscreentouch3position(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 128).WithParent(parent)
				.WithChildren(140, 2)
				.WithName("position")
				.WithDisplayName("Touch Position")
				.WithShortDisplayName("Touch Position")
				.WithLayout(kVector2Layout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 228U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0002584C File Offset: 0x00023A4C
		private DeltaControl Initialize_ctrlTouchscreentouch3delta(InternedString kDeltaLayout, InputControl parent)
		{
			DeltaControl deltaControl = new DeltaControl();
			deltaControl.Setup().At(this, 129).WithParent(parent)
				.WithChildren(142, 6)
				.WithName("delta")
				.WithDisplayName("Touch Delta")
				.WithShortDisplayName("Touch Delta")
				.WithLayout(kDeltaLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 236U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return deltaControl;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00025900 File Offset: 0x00023B00
		private AxisControl Initialize_ctrlTouchscreentouch3pressure(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 130).WithParent(parent)
				.WithName("pressure")
				.WithDisplayName("Touch Pressure")
				.WithShortDisplayName("Touch Pressure")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 244U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x000259A8 File Offset: 0x00023BA8
		private Vector2Control Initialize_ctrlTouchscreentouch3radius(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 131).WithParent(parent)
				.WithChildren(148, 2)
				.WithName("radius")
				.WithDisplayName("Touch Radius")
				.WithShortDisplayName("Touch Radius")
				.WithLayout(kVector2Layout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 248U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00025A5C File Offset: 0x00023C5C
		private TouchPhaseControl Initialize_ctrlTouchscreentouch3phase(InternedString kTouchPhaseLayout, InputControl parent)
		{
			TouchPhaseControl touchPhaseControl = new TouchPhaseControl();
			touchPhaseControl.Setup().At(this, 132).WithParent(parent)
				.WithName("phase")
				.WithDisplayName("Touch Touch Phase")
				.WithShortDisplayName("Touch Touch Phase")
				.WithLayout(kTouchPhaseLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 256U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return touchPhaseControl;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00025B0C File Offset: 0x00023D0C
		private TouchPressControl Initialize_ctrlTouchscreentouch3press(InternedString kTouchPressLayout, InputControl parent)
		{
			TouchPressControl touchPressControl = new TouchPressControl();
			touchPressControl.Setup().At(this, 133).WithParent(parent)
				.WithName("press")
				.WithDisplayName("Touch Touch Contact?")
				.WithShortDisplayName("Touch Touch Contact?")
				.WithLayout(kTouchPressLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 256U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return touchPressControl;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00025BD0 File Offset: 0x00023DD0
		private IntegerControl Initialize_ctrlTouchscreentouch3tapCount(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 134).WithParent(parent)
				.WithName("tapCount")
				.WithDisplayName("Touch Tap Count")
				.WithShortDisplayName("Touch Tap Count")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 257U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00025C78 File Offset: 0x00023E78
		private IntegerControl Initialize_ctrlTouchscreentouch3displayIndex(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 135).WithParent(parent)
				.WithName("displayIndex")
				.WithDisplayName("Touch Display Index")
				.WithShortDisplayName("Touch Display Index")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 258U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00025D20 File Offset: 0x00023F20
		private ButtonControl Initialize_ctrlTouchscreentouch3indirectTouch(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 136).WithParent(parent)
				.WithName("indirectTouch")
				.WithDisplayName("Touch Indirect Touch?")
				.WithShortDisplayName("Touch Indirect Touch?")
				.WithLayout(kButtonLayout)
				.IsSynthetic(true)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 259U,
					bitOffset = 0U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00025DEC File Offset: 0x00023FEC
		private ButtonControl Initialize_ctrlTouchscreentouch3tap(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 137).WithParent(parent)
				.WithName("tap")
				.WithDisplayName("Touch Tap")
				.WithShortDisplayName("Touch Tap")
				.WithLayout(kButtonLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 259U,
					bitOffset = 4U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00025EB0 File Offset: 0x000240B0
		private DoubleControl Initialize_ctrlTouchscreentouch3startTime(InternedString kDoubleLayout, InputControl parent)
		{
			DoubleControl doubleControl = new DoubleControl();
			doubleControl.Setup().At(this, 138).WithParent(parent)
				.WithName("startTime")
				.WithDisplayName("Touch Start Time")
				.WithShortDisplayName("Touch Start Time")
				.WithLayout(kDoubleLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1145195552),
					byteOffset = 264U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return doubleControl;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00025F60 File Offset: 0x00024160
		private Vector2Control Initialize_ctrlTouchscreentouch3startPosition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 139).WithParent(parent)
				.WithChildren(150, 2)
				.WithName("startPosition")
				.WithDisplayName("Touch Start Position")
				.WithShortDisplayName("Touch Start Position")
				.WithLayout(kVector2Layout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 272U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00026020 File Offset: 0x00024220
		private AxisControl Initialize_ctrlTouchscreentouch3positionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 140).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Position X")
				.WithShortDisplayName("Touch Touch Position X")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 228U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x000260D0 File Offset: 0x000242D0
		private AxisControl Initialize_ctrlTouchscreentouch3positiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 141).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Position Y")
				.WithShortDisplayName("Touch Touch Position Y")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 232U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00026180 File Offset: 0x00024380
		private AxisControl Initialize_ctrlTouchscreentouch3deltaup(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 142).WithParent(parent)
				.WithName("up")
				.WithDisplayName("Touch Touch Delta Up")
				.WithShortDisplayName("Touch Touch Delta Up")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 240U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00026244 File Offset: 0x00024444
		private AxisControl Initialize_ctrlTouchscreentouch3deltadown(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 143).WithParent(parent)
				.WithName("down")
				.WithDisplayName("Touch Touch Delta Down")
				.WithShortDisplayName("Touch Touch Delta Down")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 240U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0002630C File Offset: 0x0002450C
		private AxisControl Initialize_ctrlTouchscreentouch3deltaleft(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 144).WithParent(parent)
				.WithName("left")
				.WithDisplayName("Touch Touch Delta Left")
				.WithShortDisplayName("Touch Touch Delta Left")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 236U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x000263D4 File Offset: 0x000245D4
		private AxisControl Initialize_ctrlTouchscreentouch3deltaright(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 145).WithParent(parent)
				.WithName("right")
				.WithDisplayName("Touch Touch Delta Right")
				.WithShortDisplayName("Touch Touch Delta Right")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 236U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00026498 File Offset: 0x00024698
		private AxisControl Initialize_ctrlTouchscreentouch3deltax(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 146).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Delta X")
				.WithShortDisplayName("Touch Touch Delta X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 236U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00026540 File Offset: 0x00024740
		private AxisControl Initialize_ctrlTouchscreentouch3deltay(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 147).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Delta Y")
				.WithShortDisplayName("Touch Touch Delta Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 240U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x000265E8 File Offset: 0x000247E8
		private AxisControl Initialize_ctrlTouchscreentouch3radiusx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 148).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Radius X")
				.WithShortDisplayName("Touch Touch Radius X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 248U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00026690 File Offset: 0x00024890
		private AxisControl Initialize_ctrlTouchscreentouch3radiusy(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 149).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Radius Y")
				.WithShortDisplayName("Touch Touch Radius Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 252U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00026738 File Offset: 0x00024938
		private AxisControl Initialize_ctrlTouchscreentouch3startPositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 150).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Start Position X")
				.WithShortDisplayName("Touch Touch Start Position X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 272U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x000267E0 File Offset: 0x000249E0
		private AxisControl Initialize_ctrlTouchscreentouch3startPositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 151).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Start Position Y")
				.WithShortDisplayName("Touch Touch Start Position Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 276U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00026888 File Offset: 0x00024A88
		private IntegerControl Initialize_ctrlTouchscreentouch4touchId(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 152).WithParent(parent)
				.WithName("touchId")
				.WithDisplayName("Touch Touch ID")
				.WithShortDisplayName("Touch Touch ID")
				.WithLayout(kIntegerLayout)
				.IsSynthetic(true)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1229870112),
					byteOffset = 280U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00026940 File Offset: 0x00024B40
		private Vector2Control Initialize_ctrlTouchscreentouch4position(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 153).WithParent(parent)
				.WithChildren(165, 2)
				.WithName("position")
				.WithDisplayName("Touch Position")
				.WithShortDisplayName("Touch Position")
				.WithLayout(kVector2Layout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 284U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00026A00 File Offset: 0x00024C00
		private DeltaControl Initialize_ctrlTouchscreentouch4delta(InternedString kDeltaLayout, InputControl parent)
		{
			DeltaControl deltaControl = new DeltaControl();
			deltaControl.Setup().At(this, 154).WithParent(parent)
				.WithChildren(167, 6)
				.WithName("delta")
				.WithDisplayName("Touch Delta")
				.WithShortDisplayName("Touch Delta")
				.WithLayout(kDeltaLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 292U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return deltaControl;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00026AB4 File Offset: 0x00024CB4
		private AxisControl Initialize_ctrlTouchscreentouch4pressure(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 155).WithParent(parent)
				.WithName("pressure")
				.WithDisplayName("Touch Pressure")
				.WithShortDisplayName("Touch Pressure")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 300U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00026B5C File Offset: 0x00024D5C
		private Vector2Control Initialize_ctrlTouchscreentouch4radius(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 156).WithParent(parent)
				.WithChildren(173, 2)
				.WithName("radius")
				.WithDisplayName("Touch Radius")
				.WithShortDisplayName("Touch Radius")
				.WithLayout(kVector2Layout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 304U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00026C10 File Offset: 0x00024E10
		private TouchPhaseControl Initialize_ctrlTouchscreentouch4phase(InternedString kTouchPhaseLayout, InputControl parent)
		{
			TouchPhaseControl touchPhaseControl = new TouchPhaseControl();
			touchPhaseControl.Setup().At(this, 157).WithParent(parent)
				.WithName("phase")
				.WithDisplayName("Touch Touch Phase")
				.WithShortDisplayName("Touch Touch Phase")
				.WithLayout(kTouchPhaseLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 312U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return touchPhaseControl;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00026CC0 File Offset: 0x00024EC0
		private TouchPressControl Initialize_ctrlTouchscreentouch4press(InternedString kTouchPressLayout, InputControl parent)
		{
			TouchPressControl touchPressControl = new TouchPressControl();
			touchPressControl.Setup().At(this, 158).WithParent(parent)
				.WithName("press")
				.WithDisplayName("Touch Touch Contact?")
				.WithShortDisplayName("Touch Touch Contact?")
				.WithLayout(kTouchPressLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 312U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return touchPressControl;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00026D84 File Offset: 0x00024F84
		private IntegerControl Initialize_ctrlTouchscreentouch4tapCount(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 159).WithParent(parent)
				.WithName("tapCount")
				.WithDisplayName("Touch Tap Count")
				.WithShortDisplayName("Touch Tap Count")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 313U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00026E2C File Offset: 0x0002502C
		private IntegerControl Initialize_ctrlTouchscreentouch4displayIndex(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 160).WithParent(parent)
				.WithName("displayIndex")
				.WithDisplayName("Touch Display Index")
				.WithShortDisplayName("Touch Display Index")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 314U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00026ED4 File Offset: 0x000250D4
		private ButtonControl Initialize_ctrlTouchscreentouch4indirectTouch(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 161).WithParent(parent)
				.WithName("indirectTouch")
				.WithDisplayName("Touch Indirect Touch?")
				.WithShortDisplayName("Touch Indirect Touch?")
				.WithLayout(kButtonLayout)
				.IsSynthetic(true)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 315U,
					bitOffset = 0U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00026FA0 File Offset: 0x000251A0
		private ButtonControl Initialize_ctrlTouchscreentouch4tap(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 162).WithParent(parent)
				.WithName("tap")
				.WithDisplayName("Touch Tap")
				.WithShortDisplayName("Touch Tap")
				.WithLayout(kButtonLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 315U,
					bitOffset = 4U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00027064 File Offset: 0x00025264
		private DoubleControl Initialize_ctrlTouchscreentouch4startTime(InternedString kDoubleLayout, InputControl parent)
		{
			DoubleControl doubleControl = new DoubleControl();
			doubleControl.Setup().At(this, 163).WithParent(parent)
				.WithName("startTime")
				.WithDisplayName("Touch Start Time")
				.WithShortDisplayName("Touch Start Time")
				.WithLayout(kDoubleLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1145195552),
					byteOffset = 320U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return doubleControl;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00027114 File Offset: 0x00025314
		private Vector2Control Initialize_ctrlTouchscreentouch4startPosition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 164).WithParent(parent)
				.WithChildren(175, 2)
				.WithName("startPosition")
				.WithDisplayName("Touch Start Position")
				.WithShortDisplayName("Touch Start Position")
				.WithLayout(kVector2Layout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 328U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x000271D4 File Offset: 0x000253D4
		private AxisControl Initialize_ctrlTouchscreentouch4positionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 165).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Position X")
				.WithShortDisplayName("Touch Touch Position X")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 284U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00027284 File Offset: 0x00025484
		private AxisControl Initialize_ctrlTouchscreentouch4positiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 166).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Position Y")
				.WithShortDisplayName("Touch Touch Position Y")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 288U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00027334 File Offset: 0x00025534
		private AxisControl Initialize_ctrlTouchscreentouch4deltaup(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 167).WithParent(parent)
				.WithName("up")
				.WithDisplayName("Touch Touch Delta Up")
				.WithShortDisplayName("Touch Touch Delta Up")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 296U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x000273F8 File Offset: 0x000255F8
		private AxisControl Initialize_ctrlTouchscreentouch4deltadown(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 168).WithParent(parent)
				.WithName("down")
				.WithDisplayName("Touch Touch Delta Down")
				.WithShortDisplayName("Touch Touch Delta Down")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 296U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x000274C0 File Offset: 0x000256C0
		private AxisControl Initialize_ctrlTouchscreentouch4deltaleft(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 169).WithParent(parent)
				.WithName("left")
				.WithDisplayName("Touch Touch Delta Left")
				.WithShortDisplayName("Touch Touch Delta Left")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 292U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00027588 File Offset: 0x00025788
		private AxisControl Initialize_ctrlTouchscreentouch4deltaright(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 170).WithParent(parent)
				.WithName("right")
				.WithDisplayName("Touch Touch Delta Right")
				.WithShortDisplayName("Touch Touch Delta Right")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 292U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0002764C File Offset: 0x0002584C
		private AxisControl Initialize_ctrlTouchscreentouch4deltax(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 171).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Delta X")
				.WithShortDisplayName("Touch Touch Delta X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 292U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000276F4 File Offset: 0x000258F4
		private AxisControl Initialize_ctrlTouchscreentouch4deltay(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 172).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Delta Y")
				.WithShortDisplayName("Touch Touch Delta Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 296U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0002779C File Offset: 0x0002599C
		private AxisControl Initialize_ctrlTouchscreentouch4radiusx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 173).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Radius X")
				.WithShortDisplayName("Touch Touch Radius X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 304U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00027844 File Offset: 0x00025A44
		private AxisControl Initialize_ctrlTouchscreentouch4radiusy(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 174).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Radius Y")
				.WithShortDisplayName("Touch Touch Radius Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 308U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x000278EC File Offset: 0x00025AEC
		private AxisControl Initialize_ctrlTouchscreentouch4startPositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 175).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Start Position X")
				.WithShortDisplayName("Touch Touch Start Position X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 328U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00027994 File Offset: 0x00025B94
		private AxisControl Initialize_ctrlTouchscreentouch4startPositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 176).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Start Position Y")
				.WithShortDisplayName("Touch Touch Start Position Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 332U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00027A3C File Offset: 0x00025C3C
		private IntegerControl Initialize_ctrlTouchscreentouch5touchId(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 177).WithParent(parent)
				.WithName("touchId")
				.WithDisplayName("Touch Touch ID")
				.WithShortDisplayName("Touch Touch ID")
				.WithLayout(kIntegerLayout)
				.IsSynthetic(true)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1229870112),
					byteOffset = 336U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00027AF4 File Offset: 0x00025CF4
		private Vector2Control Initialize_ctrlTouchscreentouch5position(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 178).WithParent(parent)
				.WithChildren(190, 2)
				.WithName("position")
				.WithDisplayName("Touch Position")
				.WithShortDisplayName("Touch Position")
				.WithLayout(kVector2Layout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 340U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00027BB4 File Offset: 0x00025DB4
		private DeltaControl Initialize_ctrlTouchscreentouch5delta(InternedString kDeltaLayout, InputControl parent)
		{
			DeltaControl deltaControl = new DeltaControl();
			deltaControl.Setup().At(this, 179).WithParent(parent)
				.WithChildren(192, 6)
				.WithName("delta")
				.WithDisplayName("Touch Delta")
				.WithShortDisplayName("Touch Delta")
				.WithLayout(kDeltaLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 348U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return deltaControl;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00027C68 File Offset: 0x00025E68
		private AxisControl Initialize_ctrlTouchscreentouch5pressure(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 180).WithParent(parent)
				.WithName("pressure")
				.WithDisplayName("Touch Pressure")
				.WithShortDisplayName("Touch Pressure")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 356U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00027D10 File Offset: 0x00025F10
		private Vector2Control Initialize_ctrlTouchscreentouch5radius(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 181).WithParent(parent)
				.WithChildren(198, 2)
				.WithName("radius")
				.WithDisplayName("Touch Radius")
				.WithShortDisplayName("Touch Radius")
				.WithLayout(kVector2Layout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 360U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00027DC4 File Offset: 0x00025FC4
		private TouchPhaseControl Initialize_ctrlTouchscreentouch5phase(InternedString kTouchPhaseLayout, InputControl parent)
		{
			TouchPhaseControl touchPhaseControl = new TouchPhaseControl();
			touchPhaseControl.Setup().At(this, 182).WithParent(parent)
				.WithName("phase")
				.WithDisplayName("Touch Touch Phase")
				.WithShortDisplayName("Touch Touch Phase")
				.WithLayout(kTouchPhaseLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 368U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return touchPhaseControl;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00027E74 File Offset: 0x00026074
		private TouchPressControl Initialize_ctrlTouchscreentouch5press(InternedString kTouchPressLayout, InputControl parent)
		{
			TouchPressControl touchPressControl = new TouchPressControl();
			touchPressControl.Setup().At(this, 183).WithParent(parent)
				.WithName("press")
				.WithDisplayName("Touch Touch Contact?")
				.WithShortDisplayName("Touch Touch Contact?")
				.WithLayout(kTouchPressLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 368U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return touchPressControl;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00027F38 File Offset: 0x00026138
		private IntegerControl Initialize_ctrlTouchscreentouch5tapCount(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 184).WithParent(parent)
				.WithName("tapCount")
				.WithDisplayName("Touch Tap Count")
				.WithShortDisplayName("Touch Tap Count")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 369U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00027FE0 File Offset: 0x000261E0
		private IntegerControl Initialize_ctrlTouchscreentouch5displayIndex(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 185).WithParent(parent)
				.WithName("displayIndex")
				.WithDisplayName("Touch Display Index")
				.WithShortDisplayName("Touch Display Index")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 370U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00028088 File Offset: 0x00026288
		private ButtonControl Initialize_ctrlTouchscreentouch5indirectTouch(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 186).WithParent(parent)
				.WithName("indirectTouch")
				.WithDisplayName("Touch Indirect Touch?")
				.WithShortDisplayName("Touch Indirect Touch?")
				.WithLayout(kButtonLayout)
				.IsSynthetic(true)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 371U,
					bitOffset = 0U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00028154 File Offset: 0x00026354
		private ButtonControl Initialize_ctrlTouchscreentouch5tap(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 187).WithParent(parent)
				.WithName("tap")
				.WithDisplayName("Touch Tap")
				.WithShortDisplayName("Touch Tap")
				.WithLayout(kButtonLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 371U,
					bitOffset = 4U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00028218 File Offset: 0x00026418
		private DoubleControl Initialize_ctrlTouchscreentouch5startTime(InternedString kDoubleLayout, InputControl parent)
		{
			DoubleControl doubleControl = new DoubleControl();
			doubleControl.Setup().At(this, 188).WithParent(parent)
				.WithName("startTime")
				.WithDisplayName("Touch Start Time")
				.WithShortDisplayName("Touch Start Time")
				.WithLayout(kDoubleLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1145195552),
					byteOffset = 376U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return doubleControl;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000282C8 File Offset: 0x000264C8
		private Vector2Control Initialize_ctrlTouchscreentouch5startPosition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 189).WithParent(parent)
				.WithChildren(200, 2)
				.WithName("startPosition")
				.WithDisplayName("Touch Start Position")
				.WithShortDisplayName("Touch Start Position")
				.WithLayout(kVector2Layout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 384U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00028388 File Offset: 0x00026588
		private AxisControl Initialize_ctrlTouchscreentouch5positionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 190).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Position X")
				.WithShortDisplayName("Touch Touch Position X")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 340U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00028438 File Offset: 0x00026638
		private AxisControl Initialize_ctrlTouchscreentouch5positiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 191).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Position Y")
				.WithShortDisplayName("Touch Touch Position Y")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 344U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000284E8 File Offset: 0x000266E8
		private AxisControl Initialize_ctrlTouchscreentouch5deltaup(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 192).WithParent(parent)
				.WithName("up")
				.WithDisplayName("Touch Touch Delta Up")
				.WithShortDisplayName("Touch Touch Delta Up")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 352U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000285AC File Offset: 0x000267AC
		private AxisControl Initialize_ctrlTouchscreentouch5deltadown(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 193).WithParent(parent)
				.WithName("down")
				.WithDisplayName("Touch Touch Delta Down")
				.WithShortDisplayName("Touch Touch Delta Down")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 352U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00028674 File Offset: 0x00026874
		private AxisControl Initialize_ctrlTouchscreentouch5deltaleft(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 194).WithParent(parent)
				.WithName("left")
				.WithDisplayName("Touch Touch Delta Left")
				.WithShortDisplayName("Touch Touch Delta Left")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 348U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0002873C File Offset: 0x0002693C
		private AxisControl Initialize_ctrlTouchscreentouch5deltaright(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 195).WithParent(parent)
				.WithName("right")
				.WithDisplayName("Touch Touch Delta Right")
				.WithShortDisplayName("Touch Touch Delta Right")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 348U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00028800 File Offset: 0x00026A00
		private AxisControl Initialize_ctrlTouchscreentouch5deltax(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 196).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Delta X")
				.WithShortDisplayName("Touch Touch Delta X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 348U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x000288A8 File Offset: 0x00026AA8
		private AxisControl Initialize_ctrlTouchscreentouch5deltay(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 197).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Delta Y")
				.WithShortDisplayName("Touch Touch Delta Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 352U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00028950 File Offset: 0x00026B50
		private AxisControl Initialize_ctrlTouchscreentouch5radiusx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 198).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Radius X")
				.WithShortDisplayName("Touch Touch Radius X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 360U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x000289F8 File Offset: 0x00026BF8
		private AxisControl Initialize_ctrlTouchscreentouch5radiusy(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 199).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Radius Y")
				.WithShortDisplayName("Touch Touch Radius Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 364U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00028AA0 File Offset: 0x00026CA0
		private AxisControl Initialize_ctrlTouchscreentouch5startPositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 200).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Start Position X")
				.WithShortDisplayName("Touch Touch Start Position X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 384U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00028B48 File Offset: 0x00026D48
		private AxisControl Initialize_ctrlTouchscreentouch5startPositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 201).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Start Position Y")
				.WithShortDisplayName("Touch Touch Start Position Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 388U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00028BF0 File Offset: 0x00026DF0
		private IntegerControl Initialize_ctrlTouchscreentouch6touchId(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 202).WithParent(parent)
				.WithName("touchId")
				.WithDisplayName("Touch Touch ID")
				.WithShortDisplayName("Touch Touch ID")
				.WithLayout(kIntegerLayout)
				.IsSynthetic(true)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1229870112),
					byteOffset = 392U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00028CA8 File Offset: 0x00026EA8
		private Vector2Control Initialize_ctrlTouchscreentouch6position(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 203).WithParent(parent)
				.WithChildren(215, 2)
				.WithName("position")
				.WithDisplayName("Touch Position")
				.WithShortDisplayName("Touch Position")
				.WithLayout(kVector2Layout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 396U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00028D68 File Offset: 0x00026F68
		private DeltaControl Initialize_ctrlTouchscreentouch6delta(InternedString kDeltaLayout, InputControl parent)
		{
			DeltaControl deltaControl = new DeltaControl();
			deltaControl.Setup().At(this, 204).WithParent(parent)
				.WithChildren(217, 6)
				.WithName("delta")
				.WithDisplayName("Touch Delta")
				.WithShortDisplayName("Touch Delta")
				.WithLayout(kDeltaLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 404U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return deltaControl;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00028E1C File Offset: 0x0002701C
		private AxisControl Initialize_ctrlTouchscreentouch6pressure(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 205).WithParent(parent)
				.WithName("pressure")
				.WithDisplayName("Touch Pressure")
				.WithShortDisplayName("Touch Pressure")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 412U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00028EC4 File Offset: 0x000270C4
		private Vector2Control Initialize_ctrlTouchscreentouch6radius(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 206).WithParent(parent)
				.WithChildren(223, 2)
				.WithName("radius")
				.WithDisplayName("Touch Radius")
				.WithShortDisplayName("Touch Radius")
				.WithLayout(kVector2Layout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 416U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00028F78 File Offset: 0x00027178
		private TouchPhaseControl Initialize_ctrlTouchscreentouch6phase(InternedString kTouchPhaseLayout, InputControl parent)
		{
			TouchPhaseControl touchPhaseControl = new TouchPhaseControl();
			touchPhaseControl.Setup().At(this, 207).WithParent(parent)
				.WithName("phase")
				.WithDisplayName("Touch Touch Phase")
				.WithShortDisplayName("Touch Touch Phase")
				.WithLayout(kTouchPhaseLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 424U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return touchPhaseControl;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00029028 File Offset: 0x00027228
		private TouchPressControl Initialize_ctrlTouchscreentouch6press(InternedString kTouchPressLayout, InputControl parent)
		{
			TouchPressControl touchPressControl = new TouchPressControl();
			touchPressControl.Setup().At(this, 208).WithParent(parent)
				.WithName("press")
				.WithDisplayName("Touch Touch Contact?")
				.WithShortDisplayName("Touch Touch Contact?")
				.WithLayout(kTouchPressLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 424U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return touchPressControl;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x000290EC File Offset: 0x000272EC
		private IntegerControl Initialize_ctrlTouchscreentouch6tapCount(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 209).WithParent(parent)
				.WithName("tapCount")
				.WithDisplayName("Touch Tap Count")
				.WithShortDisplayName("Touch Tap Count")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 425U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00029194 File Offset: 0x00027394
		private IntegerControl Initialize_ctrlTouchscreentouch6displayIndex(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 210).WithParent(parent)
				.WithName("displayIndex")
				.WithDisplayName("Touch Display Index")
				.WithShortDisplayName("Touch Display Index")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 426U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0002923C File Offset: 0x0002743C
		private ButtonControl Initialize_ctrlTouchscreentouch6indirectTouch(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 211).WithParent(parent)
				.WithName("indirectTouch")
				.WithDisplayName("Touch Indirect Touch?")
				.WithShortDisplayName("Touch Indirect Touch?")
				.WithLayout(kButtonLayout)
				.IsSynthetic(true)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 427U,
					bitOffset = 0U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00029308 File Offset: 0x00027508
		private ButtonControl Initialize_ctrlTouchscreentouch6tap(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 212).WithParent(parent)
				.WithName("tap")
				.WithDisplayName("Touch Tap")
				.WithShortDisplayName("Touch Tap")
				.WithLayout(kButtonLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 427U,
					bitOffset = 4U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000293CC File Offset: 0x000275CC
		private DoubleControl Initialize_ctrlTouchscreentouch6startTime(InternedString kDoubleLayout, InputControl parent)
		{
			DoubleControl doubleControl = new DoubleControl();
			doubleControl.Setup().At(this, 213).WithParent(parent)
				.WithName("startTime")
				.WithDisplayName("Touch Start Time")
				.WithShortDisplayName("Touch Start Time")
				.WithLayout(kDoubleLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1145195552),
					byteOffset = 432U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return doubleControl;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0002947C File Offset: 0x0002767C
		private Vector2Control Initialize_ctrlTouchscreentouch6startPosition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 214).WithParent(parent)
				.WithChildren(225, 2)
				.WithName("startPosition")
				.WithDisplayName("Touch Start Position")
				.WithShortDisplayName("Touch Start Position")
				.WithLayout(kVector2Layout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 440U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0002953C File Offset: 0x0002773C
		private AxisControl Initialize_ctrlTouchscreentouch6positionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 215).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Position X")
				.WithShortDisplayName("Touch Touch Position X")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 396U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x000295EC File Offset: 0x000277EC
		private AxisControl Initialize_ctrlTouchscreentouch6positiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 216).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Position Y")
				.WithShortDisplayName("Touch Touch Position Y")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 400U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0002969C File Offset: 0x0002789C
		private AxisControl Initialize_ctrlTouchscreentouch6deltaup(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 217).WithParent(parent)
				.WithName("up")
				.WithDisplayName("Touch Touch Delta Up")
				.WithShortDisplayName("Touch Touch Delta Up")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 408U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00029760 File Offset: 0x00027960
		private AxisControl Initialize_ctrlTouchscreentouch6deltadown(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 218).WithParent(parent)
				.WithName("down")
				.WithDisplayName("Touch Touch Delta Down")
				.WithShortDisplayName("Touch Touch Delta Down")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 408U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00029828 File Offset: 0x00027A28
		private AxisControl Initialize_ctrlTouchscreentouch6deltaleft(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 219).WithParent(parent)
				.WithName("left")
				.WithDisplayName("Touch Touch Delta Left")
				.WithShortDisplayName("Touch Touch Delta Left")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 404U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x000298F0 File Offset: 0x00027AF0
		private AxisControl Initialize_ctrlTouchscreentouch6deltaright(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 220).WithParent(parent)
				.WithName("right")
				.WithDisplayName("Touch Touch Delta Right")
				.WithShortDisplayName("Touch Touch Delta Right")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 404U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x000299B4 File Offset: 0x00027BB4
		private AxisControl Initialize_ctrlTouchscreentouch6deltax(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 221).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Delta X")
				.WithShortDisplayName("Touch Touch Delta X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 404U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00029A5C File Offset: 0x00027C5C
		private AxisControl Initialize_ctrlTouchscreentouch6deltay(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 222).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Delta Y")
				.WithShortDisplayName("Touch Touch Delta Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 408U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00029B04 File Offset: 0x00027D04
		private AxisControl Initialize_ctrlTouchscreentouch6radiusx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 223).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Radius X")
				.WithShortDisplayName("Touch Touch Radius X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 416U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00029BAC File Offset: 0x00027DAC
		private AxisControl Initialize_ctrlTouchscreentouch6radiusy(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 224).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Radius Y")
				.WithShortDisplayName("Touch Touch Radius Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 420U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00029C54 File Offset: 0x00027E54
		private AxisControl Initialize_ctrlTouchscreentouch6startPositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 225).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Start Position X")
				.WithShortDisplayName("Touch Touch Start Position X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 440U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00029CFC File Offset: 0x00027EFC
		private AxisControl Initialize_ctrlTouchscreentouch6startPositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 226).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Start Position Y")
				.WithShortDisplayName("Touch Touch Start Position Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 444U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00029DA4 File Offset: 0x00027FA4
		private IntegerControl Initialize_ctrlTouchscreentouch7touchId(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 227).WithParent(parent)
				.WithName("touchId")
				.WithDisplayName("Touch Touch ID")
				.WithShortDisplayName("Touch Touch ID")
				.WithLayout(kIntegerLayout)
				.IsSynthetic(true)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1229870112),
					byteOffset = 448U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00029E5C File Offset: 0x0002805C
		private Vector2Control Initialize_ctrlTouchscreentouch7position(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 228).WithParent(parent)
				.WithChildren(240, 2)
				.WithName("position")
				.WithDisplayName("Touch Position")
				.WithShortDisplayName("Touch Position")
				.WithLayout(kVector2Layout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 452U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00029F1C File Offset: 0x0002811C
		private DeltaControl Initialize_ctrlTouchscreentouch7delta(InternedString kDeltaLayout, InputControl parent)
		{
			DeltaControl deltaControl = new DeltaControl();
			deltaControl.Setup().At(this, 229).WithParent(parent)
				.WithChildren(242, 6)
				.WithName("delta")
				.WithDisplayName("Touch Delta")
				.WithShortDisplayName("Touch Delta")
				.WithLayout(kDeltaLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 460U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return deltaControl;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00029FD0 File Offset: 0x000281D0
		private AxisControl Initialize_ctrlTouchscreentouch7pressure(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 230).WithParent(parent)
				.WithName("pressure")
				.WithDisplayName("Touch Pressure")
				.WithShortDisplayName("Touch Pressure")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 468U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0002A078 File Offset: 0x00028278
		private Vector2Control Initialize_ctrlTouchscreentouch7radius(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 231).WithParent(parent)
				.WithChildren(248, 2)
				.WithName("radius")
				.WithDisplayName("Touch Radius")
				.WithShortDisplayName("Touch Radius")
				.WithLayout(kVector2Layout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 472U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0002A12C File Offset: 0x0002832C
		private TouchPhaseControl Initialize_ctrlTouchscreentouch7phase(InternedString kTouchPhaseLayout, InputControl parent)
		{
			TouchPhaseControl touchPhaseControl = new TouchPhaseControl();
			touchPhaseControl.Setup().At(this, 232).WithParent(parent)
				.WithName("phase")
				.WithDisplayName("Touch Touch Phase")
				.WithShortDisplayName("Touch Touch Phase")
				.WithLayout(kTouchPhaseLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 480U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return touchPhaseControl;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0002A1DC File Offset: 0x000283DC
		private TouchPressControl Initialize_ctrlTouchscreentouch7press(InternedString kTouchPressLayout, InputControl parent)
		{
			TouchPressControl touchPressControl = new TouchPressControl();
			touchPressControl.Setup().At(this, 233).WithParent(parent)
				.WithName("press")
				.WithDisplayName("Touch Touch Contact?")
				.WithShortDisplayName("Touch Touch Contact?")
				.WithLayout(kTouchPressLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 480U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return touchPressControl;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0002A2A0 File Offset: 0x000284A0
		private IntegerControl Initialize_ctrlTouchscreentouch7tapCount(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 234).WithParent(parent)
				.WithName("tapCount")
				.WithDisplayName("Touch Tap Count")
				.WithShortDisplayName("Touch Tap Count")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 481U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0002A348 File Offset: 0x00028548
		private IntegerControl Initialize_ctrlTouchscreentouch7displayIndex(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 235).WithParent(parent)
				.WithName("displayIndex")
				.WithDisplayName("Touch Display Index")
				.WithShortDisplayName("Touch Display Index")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 482U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0002A3F0 File Offset: 0x000285F0
		private ButtonControl Initialize_ctrlTouchscreentouch7indirectTouch(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 236).WithParent(parent)
				.WithName("indirectTouch")
				.WithDisplayName("Touch Indirect Touch?")
				.WithShortDisplayName("Touch Indirect Touch?")
				.WithLayout(kButtonLayout)
				.IsSynthetic(true)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 483U,
					bitOffset = 0U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0002A4BC File Offset: 0x000286BC
		private ButtonControl Initialize_ctrlTouchscreentouch7tap(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 237).WithParent(parent)
				.WithName("tap")
				.WithDisplayName("Touch Tap")
				.WithShortDisplayName("Touch Tap")
				.WithLayout(kButtonLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 483U,
					bitOffset = 4U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0002A580 File Offset: 0x00028780
		private DoubleControl Initialize_ctrlTouchscreentouch7startTime(InternedString kDoubleLayout, InputControl parent)
		{
			DoubleControl doubleControl = new DoubleControl();
			doubleControl.Setup().At(this, 238).WithParent(parent)
				.WithName("startTime")
				.WithDisplayName("Touch Start Time")
				.WithShortDisplayName("Touch Start Time")
				.WithLayout(kDoubleLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1145195552),
					byteOffset = 488U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return doubleControl;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0002A630 File Offset: 0x00028830
		private Vector2Control Initialize_ctrlTouchscreentouch7startPosition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 239).WithParent(parent)
				.WithChildren(250, 2)
				.WithName("startPosition")
				.WithDisplayName("Touch Start Position")
				.WithShortDisplayName("Touch Start Position")
				.WithLayout(kVector2Layout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 496U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0002A6F0 File Offset: 0x000288F0
		private AxisControl Initialize_ctrlTouchscreentouch7positionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 240).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Position X")
				.WithShortDisplayName("Touch Touch Position X")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 452U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0002A7A0 File Offset: 0x000289A0
		private AxisControl Initialize_ctrlTouchscreentouch7positiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 241).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Position Y")
				.WithShortDisplayName("Touch Touch Position Y")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 456U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0002A850 File Offset: 0x00028A50
		private AxisControl Initialize_ctrlTouchscreentouch7deltaup(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 242).WithParent(parent)
				.WithName("up")
				.WithDisplayName("Touch Touch Delta Up")
				.WithShortDisplayName("Touch Touch Delta Up")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 464U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0002A914 File Offset: 0x00028B14
		private AxisControl Initialize_ctrlTouchscreentouch7deltadown(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 243).WithParent(parent)
				.WithName("down")
				.WithDisplayName("Touch Touch Delta Down")
				.WithShortDisplayName("Touch Touch Delta Down")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 464U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0002A9DC File Offset: 0x00028BDC
		private AxisControl Initialize_ctrlTouchscreentouch7deltaleft(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 244).WithParent(parent)
				.WithName("left")
				.WithDisplayName("Touch Touch Delta Left")
				.WithShortDisplayName("Touch Touch Delta Left")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 460U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0002AAA4 File Offset: 0x00028CA4
		private AxisControl Initialize_ctrlTouchscreentouch7deltaright(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 245).WithParent(parent)
				.WithName("right")
				.WithDisplayName("Touch Touch Delta Right")
				.WithShortDisplayName("Touch Touch Delta Right")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 460U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0002AB68 File Offset: 0x00028D68
		private AxisControl Initialize_ctrlTouchscreentouch7deltax(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 246).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Delta X")
				.WithShortDisplayName("Touch Touch Delta X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 460U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0002AC10 File Offset: 0x00028E10
		private AxisControl Initialize_ctrlTouchscreentouch7deltay(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 247).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Delta Y")
				.WithShortDisplayName("Touch Touch Delta Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 464U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0002ACB8 File Offset: 0x00028EB8
		private AxisControl Initialize_ctrlTouchscreentouch7radiusx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 248).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Radius X")
				.WithShortDisplayName("Touch Touch Radius X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 472U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0002AD60 File Offset: 0x00028F60
		private AxisControl Initialize_ctrlTouchscreentouch7radiusy(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 249).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Radius Y")
				.WithShortDisplayName("Touch Touch Radius Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 476U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0002AE08 File Offset: 0x00029008
		private AxisControl Initialize_ctrlTouchscreentouch7startPositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 250).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Start Position X")
				.WithShortDisplayName("Touch Touch Start Position X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 496U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0002AEB0 File Offset: 0x000290B0
		private AxisControl Initialize_ctrlTouchscreentouch7startPositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 251).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Start Position Y")
				.WithShortDisplayName("Touch Touch Start Position Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 500U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0002AF58 File Offset: 0x00029158
		private IntegerControl Initialize_ctrlTouchscreentouch8touchId(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 252).WithParent(parent)
				.WithName("touchId")
				.WithDisplayName("Touch Touch ID")
				.WithShortDisplayName("Touch Touch ID")
				.WithLayout(kIntegerLayout)
				.IsSynthetic(true)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1229870112),
					byteOffset = 504U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0002B010 File Offset: 0x00029210
		private Vector2Control Initialize_ctrlTouchscreentouch8position(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 253).WithParent(parent)
				.WithChildren(265, 2)
				.WithName("position")
				.WithDisplayName("Touch Position")
				.WithShortDisplayName("Touch Position")
				.WithLayout(kVector2Layout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 508U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0002B0D0 File Offset: 0x000292D0
		private DeltaControl Initialize_ctrlTouchscreentouch8delta(InternedString kDeltaLayout, InputControl parent)
		{
			DeltaControl deltaControl = new DeltaControl();
			deltaControl.Setup().At(this, 254).WithParent(parent)
				.WithChildren(267, 6)
				.WithName("delta")
				.WithDisplayName("Touch Delta")
				.WithShortDisplayName("Touch Delta")
				.WithLayout(kDeltaLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 516U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return deltaControl;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0002B184 File Offset: 0x00029384
		private AxisControl Initialize_ctrlTouchscreentouch8pressure(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 255).WithParent(parent)
				.WithName("pressure")
				.WithDisplayName("Touch Pressure")
				.WithShortDisplayName("Touch Pressure")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 524U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0002B22C File Offset: 0x0002942C
		private Vector2Control Initialize_ctrlTouchscreentouch8radius(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 256).WithParent(parent)
				.WithChildren(273, 2)
				.WithName("radius")
				.WithDisplayName("Touch Radius")
				.WithShortDisplayName("Touch Radius")
				.WithLayout(kVector2Layout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 528U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0002B2E0 File Offset: 0x000294E0
		private TouchPhaseControl Initialize_ctrlTouchscreentouch8phase(InternedString kTouchPhaseLayout, InputControl parent)
		{
			TouchPhaseControl touchPhaseControl = new TouchPhaseControl();
			touchPhaseControl.Setup().At(this, 257).WithParent(parent)
				.WithName("phase")
				.WithDisplayName("Touch Touch Phase")
				.WithShortDisplayName("Touch Touch Phase")
				.WithLayout(kTouchPhaseLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 536U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return touchPhaseControl;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0002B390 File Offset: 0x00029590
		private TouchPressControl Initialize_ctrlTouchscreentouch8press(InternedString kTouchPressLayout, InputControl parent)
		{
			TouchPressControl touchPressControl = new TouchPressControl();
			touchPressControl.Setup().At(this, 258).WithParent(parent)
				.WithName("press")
				.WithDisplayName("Touch Touch Contact?")
				.WithShortDisplayName("Touch Touch Contact?")
				.WithLayout(kTouchPressLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 536U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return touchPressControl;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0002B454 File Offset: 0x00029654
		private IntegerControl Initialize_ctrlTouchscreentouch8tapCount(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 259).WithParent(parent)
				.WithName("tapCount")
				.WithDisplayName("Touch Tap Count")
				.WithShortDisplayName("Touch Tap Count")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 537U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0002B4FC File Offset: 0x000296FC
		private IntegerControl Initialize_ctrlTouchscreentouch8displayIndex(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 260).WithParent(parent)
				.WithName("displayIndex")
				.WithDisplayName("Touch Display Index")
				.WithShortDisplayName("Touch Display Index")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 538U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0002B5A4 File Offset: 0x000297A4
		private ButtonControl Initialize_ctrlTouchscreentouch8indirectTouch(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 261).WithParent(parent)
				.WithName("indirectTouch")
				.WithDisplayName("Touch Indirect Touch?")
				.WithShortDisplayName("Touch Indirect Touch?")
				.WithLayout(kButtonLayout)
				.IsSynthetic(true)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 539U,
					bitOffset = 0U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0002B670 File Offset: 0x00029870
		private ButtonControl Initialize_ctrlTouchscreentouch8tap(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 262).WithParent(parent)
				.WithName("tap")
				.WithDisplayName("Touch Tap")
				.WithShortDisplayName("Touch Tap")
				.WithLayout(kButtonLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 539U,
					bitOffset = 4U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0002B734 File Offset: 0x00029934
		private DoubleControl Initialize_ctrlTouchscreentouch8startTime(InternedString kDoubleLayout, InputControl parent)
		{
			DoubleControl doubleControl = new DoubleControl();
			doubleControl.Setup().At(this, 263).WithParent(parent)
				.WithName("startTime")
				.WithDisplayName("Touch Start Time")
				.WithShortDisplayName("Touch Start Time")
				.WithLayout(kDoubleLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1145195552),
					byteOffset = 544U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return doubleControl;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0002B7E4 File Offset: 0x000299E4
		private Vector2Control Initialize_ctrlTouchscreentouch8startPosition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 264).WithParent(parent)
				.WithChildren(275, 2)
				.WithName("startPosition")
				.WithDisplayName("Touch Start Position")
				.WithShortDisplayName("Touch Start Position")
				.WithLayout(kVector2Layout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 552U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0002B8A4 File Offset: 0x00029AA4
		private AxisControl Initialize_ctrlTouchscreentouch8positionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 265).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Position X")
				.WithShortDisplayName("Touch Touch Position X")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 508U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0002B954 File Offset: 0x00029B54
		private AxisControl Initialize_ctrlTouchscreentouch8positiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 266).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Position Y")
				.WithShortDisplayName("Touch Touch Position Y")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 512U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0002BA04 File Offset: 0x00029C04
		private AxisControl Initialize_ctrlTouchscreentouch8deltaup(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 267).WithParent(parent)
				.WithName("up")
				.WithDisplayName("Touch Touch Delta Up")
				.WithShortDisplayName("Touch Touch Delta Up")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 520U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0002BAC8 File Offset: 0x00029CC8
		private AxisControl Initialize_ctrlTouchscreentouch8deltadown(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 268).WithParent(parent)
				.WithName("down")
				.WithDisplayName("Touch Touch Delta Down")
				.WithShortDisplayName("Touch Touch Delta Down")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 520U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0002BB90 File Offset: 0x00029D90
		private AxisControl Initialize_ctrlTouchscreentouch8deltaleft(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 269).WithParent(parent)
				.WithName("left")
				.WithDisplayName("Touch Touch Delta Left")
				.WithShortDisplayName("Touch Touch Delta Left")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 516U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0002BC58 File Offset: 0x00029E58
		private AxisControl Initialize_ctrlTouchscreentouch8deltaright(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 270).WithParent(parent)
				.WithName("right")
				.WithDisplayName("Touch Touch Delta Right")
				.WithShortDisplayName("Touch Touch Delta Right")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 516U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0002BD1C File Offset: 0x00029F1C
		private AxisControl Initialize_ctrlTouchscreentouch8deltax(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 271).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Delta X")
				.WithShortDisplayName("Touch Touch Delta X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 516U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0002BDC4 File Offset: 0x00029FC4
		private AxisControl Initialize_ctrlTouchscreentouch8deltay(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 272).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Delta Y")
				.WithShortDisplayName("Touch Touch Delta Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 520U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0002BE6C File Offset: 0x0002A06C
		private AxisControl Initialize_ctrlTouchscreentouch8radiusx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 273).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Radius X")
				.WithShortDisplayName("Touch Touch Radius X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 528U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0002BF14 File Offset: 0x0002A114
		private AxisControl Initialize_ctrlTouchscreentouch8radiusy(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 274).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Radius Y")
				.WithShortDisplayName("Touch Touch Radius Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 532U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0002BFBC File Offset: 0x0002A1BC
		private AxisControl Initialize_ctrlTouchscreentouch8startPositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 275).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Start Position X")
				.WithShortDisplayName("Touch Touch Start Position X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 552U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0002C064 File Offset: 0x0002A264
		private AxisControl Initialize_ctrlTouchscreentouch8startPositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 276).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Start Position Y")
				.WithShortDisplayName("Touch Touch Start Position Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 556U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0002C10C File Offset: 0x0002A30C
		private IntegerControl Initialize_ctrlTouchscreentouch9touchId(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 277).WithParent(parent)
				.WithName("touchId")
				.WithDisplayName("Touch Touch ID")
				.WithShortDisplayName("Touch Touch ID")
				.WithLayout(kIntegerLayout)
				.IsSynthetic(true)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1229870112),
					byteOffset = 560U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0002C1C4 File Offset: 0x0002A3C4
		private Vector2Control Initialize_ctrlTouchscreentouch9position(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 278).WithParent(parent)
				.WithChildren(290, 2)
				.WithName("position")
				.WithDisplayName("Touch Position")
				.WithShortDisplayName("Touch Position")
				.WithLayout(kVector2Layout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 564U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0002C284 File Offset: 0x0002A484
		private DeltaControl Initialize_ctrlTouchscreentouch9delta(InternedString kDeltaLayout, InputControl parent)
		{
			DeltaControl deltaControl = new DeltaControl();
			deltaControl.Setup().At(this, 279).WithParent(parent)
				.WithChildren(292, 6)
				.WithName("delta")
				.WithDisplayName("Touch Delta")
				.WithShortDisplayName("Touch Delta")
				.WithLayout(kDeltaLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 572U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return deltaControl;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0002C338 File Offset: 0x0002A538
		private AxisControl Initialize_ctrlTouchscreentouch9pressure(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 280).WithParent(parent)
				.WithName("pressure")
				.WithDisplayName("Touch Pressure")
				.WithShortDisplayName("Touch Pressure")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 580U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0002C3E0 File Offset: 0x0002A5E0
		private Vector2Control Initialize_ctrlTouchscreentouch9radius(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 281).WithParent(parent)
				.WithChildren(298, 2)
				.WithName("radius")
				.WithDisplayName("Touch Radius")
				.WithShortDisplayName("Touch Radius")
				.WithLayout(kVector2Layout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 584U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0002C494 File Offset: 0x0002A694
		private TouchPhaseControl Initialize_ctrlTouchscreentouch9phase(InternedString kTouchPhaseLayout, InputControl parent)
		{
			TouchPhaseControl touchPhaseControl = new TouchPhaseControl();
			touchPhaseControl.Setup().At(this, 282).WithParent(parent)
				.WithName("phase")
				.WithDisplayName("Touch Touch Phase")
				.WithShortDisplayName("Touch Touch Phase")
				.WithLayout(kTouchPhaseLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 592U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return touchPhaseControl;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0002C544 File Offset: 0x0002A744
		private TouchPressControl Initialize_ctrlTouchscreentouch9press(InternedString kTouchPressLayout, InputControl parent)
		{
			TouchPressControl touchPressControl = new TouchPressControl();
			touchPressControl.Setup().At(this, 283).WithParent(parent)
				.WithName("press")
				.WithDisplayName("Touch Touch Contact?")
				.WithShortDisplayName("Touch Touch Contact?")
				.WithLayout(kTouchPressLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 592U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return touchPressControl;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0002C608 File Offset: 0x0002A808
		private IntegerControl Initialize_ctrlTouchscreentouch9tapCount(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 284).WithParent(parent)
				.WithName("tapCount")
				.WithDisplayName("Touch Tap Count")
				.WithShortDisplayName("Touch Tap Count")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 593U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0002C6B0 File Offset: 0x0002A8B0
		private IntegerControl Initialize_ctrlTouchscreentouch9displayIndex(InternedString kIntegerLayout, InputControl parent)
		{
			IntegerControl integerControl = new IntegerControl();
			integerControl.Setup().At(this, 285).WithParent(parent)
				.WithName("displayIndex")
				.WithDisplayName("Touch Display Index")
				.WithShortDisplayName("Touch Display Index")
				.WithLayout(kIntegerLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1113150533),
					byteOffset = 594U,
					bitOffset = 0U,
					sizeInBits = 8U
				})
				.Finish();
			return integerControl;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0002C758 File Offset: 0x0002A958
		private ButtonControl Initialize_ctrlTouchscreentouch9indirectTouch(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 286).WithParent(parent)
				.WithName("indirectTouch")
				.WithDisplayName("Touch Indirect Touch?")
				.WithShortDisplayName("Touch Indirect Touch?")
				.WithLayout(kButtonLayout)
				.IsSynthetic(true)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 595U,
					bitOffset = 0U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0002C824 File Offset: 0x0002AA24
		private ButtonControl Initialize_ctrlTouchscreentouch9tap(InternedString kButtonLayout, InputControl parent)
		{
			ButtonControl buttonControl = new ButtonControl();
			buttonControl.Setup().At(this, 287).WithParent(parent)
				.WithName("tap")
				.WithDisplayName("Touch Tap")
				.WithShortDisplayName("Touch Tap")
				.WithLayout(kButtonLayout)
				.IsButton(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1112101920),
					byteOffset = 595U,
					bitOffset = 4U,
					sizeInBits = 1U
				})
				.WithMinAndMax(0, 1)
				.Finish();
			return buttonControl;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0002C8E8 File Offset: 0x0002AAE8
		private DoubleControl Initialize_ctrlTouchscreentouch9startTime(InternedString kDoubleLayout, InputControl parent)
		{
			DoubleControl doubleControl = new DoubleControl();
			doubleControl.Setup().At(this, 288).WithParent(parent)
				.WithName("startTime")
				.WithDisplayName("Touch Start Time")
				.WithShortDisplayName("Touch Start Time")
				.WithLayout(kDoubleLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1145195552),
					byteOffset = 600U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return doubleControl;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0002C998 File Offset: 0x0002AB98
		private Vector2Control Initialize_ctrlTouchscreentouch9startPosition(InternedString kVector2Layout, InputControl parent)
		{
			Vector2Control vector2Control = new Vector2Control();
			vector2Control.Setup().At(this, 289).WithParent(parent)
				.WithChildren(300, 2)
				.WithName("startPosition")
				.WithDisplayName("Touch Start Position")
				.WithShortDisplayName("Touch Start Position")
				.WithLayout(kVector2Layout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1447379762),
					byteOffset = 608U,
					bitOffset = 0U,
					sizeInBits = 64U
				})
				.Finish();
			return vector2Control;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0002CA58 File Offset: 0x0002AC58
		private AxisControl Initialize_ctrlTouchscreentouch9positionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 290).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Position X")
				.WithShortDisplayName("Touch Touch Position X")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 564U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0002CB08 File Offset: 0x0002AD08
		private AxisControl Initialize_ctrlTouchscreentouch9positiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 291).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Position Y")
				.WithShortDisplayName("Touch Touch Position Y")
				.WithLayout(kAxisLayout)
				.DontReset(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 568U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0002CBB8 File Offset: 0x0002ADB8
		private AxisControl Initialize_ctrlTouchscreentouch9deltaup(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 292).WithParent(parent)
				.WithName("up")
				.WithDisplayName("Touch Touch Delta Up")
				.WithShortDisplayName("Touch Touch Delta Up")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 576U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0002CC7C File Offset: 0x0002AE7C
		private AxisControl Initialize_ctrlTouchscreentouch9deltadown(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 293).WithParent(parent)
				.WithName("down")
				.WithDisplayName("Touch Touch Delta Down")
				.WithShortDisplayName("Touch Touch Delta Down")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 576U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0002CD44 File Offset: 0x0002AF44
		private AxisControl Initialize_ctrlTouchscreentouch9deltaleft(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMin = -3.402823E+38f;
			axisControl.invert = true;
			axisControl.Setup().At(this, 294).WithParent(parent)
				.WithName("left")
				.WithDisplayName("Touch Touch Delta Left")
				.WithShortDisplayName("Touch Touch Delta Left")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 572U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0002CE0C File Offset: 0x0002B00C
		private AxisControl Initialize_ctrlTouchscreentouch9deltaright(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.clamp = AxisControl.Clamp.BeforeNormalize;
			axisControl.clampMax = 3.402823E+38f;
			axisControl.Setup().At(this, 295).WithParent(parent)
				.WithName("right")
				.WithDisplayName("Touch Touch Delta Right")
				.WithShortDisplayName("Touch Touch Delta Right")
				.WithLayout(kAxisLayout)
				.IsSynthetic(true)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 572U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0002CED0 File Offset: 0x0002B0D0
		private AxisControl Initialize_ctrlTouchscreentouch9deltax(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 296).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Delta X")
				.WithShortDisplayName("Touch Touch Delta X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 572U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0002CF78 File Offset: 0x0002B178
		private AxisControl Initialize_ctrlTouchscreentouch9deltay(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 297).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Delta Y")
				.WithShortDisplayName("Touch Touch Delta Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 576U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0002D020 File Offset: 0x0002B220
		private AxisControl Initialize_ctrlTouchscreentouch9radiusx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 298).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Radius X")
				.WithShortDisplayName("Touch Touch Radius X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 584U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0002D0C8 File Offset: 0x0002B2C8
		private AxisControl Initialize_ctrlTouchscreentouch9radiusy(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 299).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Radius Y")
				.WithShortDisplayName("Touch Touch Radius Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 588U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0002D170 File Offset: 0x0002B370
		private AxisControl Initialize_ctrlTouchscreentouch9startPositionx(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 300).WithParent(parent)
				.WithName("x")
				.WithDisplayName("Touch Touch Start Position X")
				.WithShortDisplayName("Touch Touch Start Position X")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 608U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0002D218 File Offset: 0x0002B418
		private AxisControl Initialize_ctrlTouchscreentouch9startPositiony(InternedString kAxisLayout, InputControl parent)
		{
			AxisControl axisControl = new AxisControl();
			axisControl.Setup().At(this, 301).WithParent(parent)
				.WithName("y")
				.WithDisplayName("Touch Touch Start Position Y")
				.WithShortDisplayName("Touch Touch Start Position Y")
				.WithLayout(kAxisLayout)
				.WithStateBlock(new InputStateBlock
				{
					format = new FourCC(1179407392),
					byteOffset = 612U,
					bitOffset = 0U,
					sizeInBits = 32U
				})
				.Finish();
			return axisControl;
		}

		// Token: 0x04000246 RID: 582
		public const string metadata = "AutoWindowSpace;Touch;Vector2;Delta;Analog;TouchPress;Button;Axis;Integer;TouchPhase;Double;Touchscreen;Pointer";
	}
}
