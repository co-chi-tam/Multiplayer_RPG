using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml;

public class FSMPostprocessor: AssetPostprocessor {

	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) 
	{
		foreach (string str in importedAssets)
		{
			var path = Environment.CurrentDirectory + "/" + str;
			var extension = Path.GetExtension (path);
			if (extension == ".graphml") {
				ReadNode (path.Replace("/", "\\").Replace ("\\", "\\"));
			}
		}
	}

	static void ReadNode(string path) {
		var stReader = new StreamReader (path);
		XmlReader xReader = XmlReader.Create (new StringReader (stReader.ReadToEnd()));
		while (xReader.Read ()) {
			if (xReader.IsStartElement()) {
				switch (xReader.Name) {
				case "node":
					var nodeID = xReader ["id"].ToString ();
					Debug.Log (nodeID);
					break;
				case "y:ShapeNode":

					break;
				case "y:NodeLabel":
					if (xReader.Read()) {
						var nodeLabel = xReader.Value.Trim ();
						Debug.Log (nodeLabel);
					}
					break;
				case "edge":
					var source = xReader ["source"].ToString ();
					var target = xReader ["target"].ToString ();
					Debug.Log (source + " -> " + target);
					break;
				case "y:EdgeLabel":
					if (xReader.Read()) {
						var edgeLabel = xReader.Value.Trim ();
						Debug.Log (edgeLabel);
					}
					break;
				}
			}
		}
	}

}
