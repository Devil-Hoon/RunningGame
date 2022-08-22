using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
	static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	//static char[] TRIM_CHARS = { '\"' };

	public static List<Dictionary<string, object>> Read(string file)
	{
		var list = new List<Dictionary<string, object>>();
		var data = Resources.Load(file) as TextAsset;

		var lines = Regex.Split(data.text, LINE_SPLIT_RE);

		if (lines.Length <= 1) return list;

		var header = Regex.Split(lines[0], SPLIT_RE);
		for (var i = 1; i < lines.Length; i++)
		{
			var values = Regex.Split(lines[i], SPLIT_RE);
			if (values.Length == 0 || values[0] == "") continue;

			var entry = new Dictionary<string, object>();
			for (var j = 0; j < header.Length && j < values.Length; j++)
			{
				string value = values[j];
				//value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
				object finalvalue = value;
				int n;
				float f;
				if (int.TryParse(value, out n))
				{
					finalvalue = n;
				}
				else if (float.TryParse(value, out f))
				{
					finalvalue = f;
				}
				entry[header[j]] = finalvalue;
			}
			list.Add(entry);
		}
		return list;
	}

	public static List<Dictionary<string, string>> Read(string path, string fileName)
	{
		var list = new List<Dictionary<string, string>>();
		string strFile = path + "/" + fileName;

		using (FileStream fs = new FileStream(strFile, FileMode.Open))
		{
			using (StreamReader sr = new StreamReader(fs, Encoding.UTF8, false))
			{
				string strLineValue = null;
				string[] keys = null;
				int count = 0;

				while ((strLineValue = sr.ReadLine()) != null)
				{
					if (string.IsNullOrEmpty(strLineValue))
					{
						return null;
					}

					string[] values = null;

					if (count == 0)
					{
						keys = strLineValue.Split(',');
						keys[0] = keys[0].Replace("#", "");
					}
					else
					{
						values = strLineValue.Split(',');

						var dic = new Dictionary<string, string>();
						for (int i = 0; i < keys.Length; i++)
						{
							dic.Add(keys[i], values[i]);
						}

						list.Add(dic);
					}
					count++;
				}
			}
		}

		return list;
	}
}
