using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;

public class LocalSaveGameStorage : ISaveGameStorage
{
	private string dataPath;

	public LocalSaveGameStorage()
	{
		dataPath = Application.persistentDataPath;
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
		WriteAllLines(filePath, new string[] { contents });
	}

	private void WriteAllLines(string fileName, string[] lines)
	{
		if (lines == null)
		{
			throw new ArgumentNullException("lines");
		}

		string directoryPath = Path.GetDirectoryName(fileName);
		if (!Directory.Exists(directoryPath))
		{
			try
			{
				Directory.CreateDirectory(directoryPath);
			}
			catch
			{
				throw new IOException("Cant create directory at:" + directoryPath);
			}
		}

		StreamWriter streamWriter = new StreamWriter(fileName, false, Encoding.UTF8);
		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			streamWriter.WriteLine(line);
		}
		streamWriter.Close();
	}

	public string Read(string fileName)
	{
		string filePath = GetFilePath(fileName);
		if (File.Exists(filePath))
		{
			string[] lines = ReadAllLines(filePath);
			if (lines == null || lines.Length == 0)
			{
				return null;
			}
			else
			{
				return lines[0];
			}
		}

		return null;
	}

	public string[] ReadAllLines(string filePath)
	{
		ArrayList arrayList = new ArrayList();
		StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8);

		string line;
		while ((line = streamReader.ReadLine()) != null)
		{
			arrayList.Add(line);
		}

		streamReader.Close();

		return (string[])arrayList.ToArray(typeof(string));
	}
}