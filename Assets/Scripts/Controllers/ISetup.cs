public interface ISetup { }

public interface ISetup<T> : ISetup
{
    void Setup(T model);
}