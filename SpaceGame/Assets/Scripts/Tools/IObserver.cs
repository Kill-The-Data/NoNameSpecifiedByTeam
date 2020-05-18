public interface IObserver
{
    void GetUpdate(ISubject subject);

}
public interface ISubject
{
    void Notify();
    void Attach(IObserver observer);
}

public interface ISubjectFilter : IObserver, ISubject
{
}