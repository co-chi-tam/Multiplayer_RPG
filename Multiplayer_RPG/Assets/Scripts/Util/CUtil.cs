using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public static class CUtil {

	public static Sprite FindSprite(string name) {
		var resourceSprites = Resources.LoadAll<Sprite> ("Image");
		for (int i = 0; i < resourceSprites.Length; i++) {
			var spriteObj = resourceSprites [i];
			if (spriteObj.name.Equals ("Images")) {
				return spriteObj;
			}
		}
		return null;
	}

	public static Vector3 ToV3(this string value) {
		return CUtil.V3Parser (value);
	}

	public static Vector3 V3Parser (string value) {
		var resultV3 = Vector3.zero;
		value = value.Replace ("(", "").Replace(")", ""); // (x,y,z)
		var splits = value.Split (','); // x,y,z
		resultV3.x = float.Parse (splits[0].ToString());
		resultV3.y = float.Parse (splits[1].ToString());
		resultV3.z = float.Parse (splits[2].ToString());
		return resultV3;
	}

	public static string V3StrParser (Vector3 value) {
		var result = new StringBuilder ("(");
		result.Append (value.x + ",");
		result.Append (value.y + ",");
		result.Append (value.z + "");
		result.Append (")");
		return result.ToString();
	}

	public static bool IsPointerOverUIObject(Vector2 position) {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = position;
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

}
