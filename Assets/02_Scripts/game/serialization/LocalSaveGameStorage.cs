using UnityEngine;
using System.IO;

public class LocalSaveGameStorage : ISaveGameStorage
{
	private string dataPath;

	public LocalSaveGameStorage()
	{
		dataPath = Application.dataPath;
	}

	private string GetFilePath(string fileName)
	{
		return dataPath + "/" + fileName;
	}

	public void Delete(string fileName)
	{
		string filePath = GetFilePath(fileName);
		if (File.Exists(filePath))
		{
			File.Delete(filePath);
		}
	}

	public void Write(string fileName, string contents)
	{
		string filePath = GetFilePath(fileName);
		File.WriteAllText(filePath, contents);
	}

	public string Read(string fileName)
	{
		string filePath = GetFilePath(fileName);

		try
		{
			return File.ReadAllText(filePath);
		}
		catch (FileNotFoundException)
		{
			return null;
		}
	}
}