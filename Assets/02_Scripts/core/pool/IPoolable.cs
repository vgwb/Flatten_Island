public interface IPoolable
{
	void OnInstantiatedFromPool();
	void OnReleasedToPool();
}