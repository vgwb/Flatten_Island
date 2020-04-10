public interface ISaveGameStorage
{
	void Delete(string fileName);

	void Write(string fileName, string contents);

	string Read(string fileName);
}