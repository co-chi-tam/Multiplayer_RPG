using System;
using System.Collections;

public class CEnum {

	public enum EObjectType: byte {
		None 		= 0,
		Survivaler	= 1,
		Monster	 	= 2,
		Object		= 3,
		Skill		= 4, 
		User 		= 100
	}

	public enum EAnimation : int {
		Idle 		= 0,
		Attack_1 	= 10,
		Attack_2 	= 11,
		Attack_3 	= 12,
		Attack_4 	= 13,
		Attack_5 	= 14,
		Attack_6 	= 15,
		Attack_7 	= 16,
		Attack_8 	= 17,
		Attack_9 	= 18,
		Attack_10 	= 19,
		Move 		= 20,
		Death 		= 100
	}

	public enum EElementType : int  {
		None 		= 0,
		Neutral		= 1,
		Poison 		= 2,
		Fire		= 3,
		Ice			= 4,
		Earth		= 5,
		Wind		= 6,
		Light		= 7,
		Dark 		= 8,
		Grass 		= 9,
		Pure 		= 100
	}

	public enum EStatusType : int  {
		None 		= 0,
		Health 		= 1,
		Mana 		= 2,
		Lucky 		= 3
	}

	public enum EEnviromentType : int {
		None 		= 0,
		Nature 		= 1,
		Magmar 		= 2,
		Ice 		= 3,
		Desert 		= 4,
		Marsh 		= 5
	}

	public enum EContructionLevel : int {
		None = 0,
		Level1 = 1,
		Level2 = 2,
		Level3 = 3
	}

	public enum EItemType: byte {
		None = 0,
		Common = 1,
		Uncommon = 2,
	}

	public enum EItem: int {
		None = 0,
		// Food
		Rice = 1,
		Fish = 2,

		// Nature
		Nature = 20,
		Water = 21,
		Fire = 22
	}

	public enum EBlockDirection: byte {
		Center = 0,
		Left = 1,
		Top = 2,
		Right = 3,
		Bottom = 4,
		TopLeft = 5,
		TopRight = 6,
		BottomRight = 7,
		BottomLeft = 8
	}

}
