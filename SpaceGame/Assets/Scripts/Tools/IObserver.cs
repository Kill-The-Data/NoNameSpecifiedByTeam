using UnityEngine;

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

public abstract class AObserver : IObserver
{
    protected abstract void AGetUpdate(ISubject subject);
    public void GetUpdate(ISubject subject)
    {
        AGetUpdate(subject);
    }
}

public abstract class AUnityObserver : MonoBehaviour, IObserver
{
    protected abstract void AGetUpdate(ISubject subject);
    public void GetUpdate(ISubject subject)
    {
        AGetUpdate(subject);
    }
}

public abstract class ASubject : ISubject
{
    protected abstract void ANotify();
    public void Notify()
    {
        ANotify();
    }

    protected abstract void AAttach();
    public void Attach(IObserver observer)
    {
        AAttach();
    }
}

public abstract class AUnitySubject : MonoBehaviour, ISubject
{
    protected abstract void ANotify();
    public void Notify()
    {
        ANotify();
    }

    protected abstract void AAttach(IObserver observer);
    public void Attach(IObserver observer)
    {
        AAttach(observer);
    }
}

public abstract class ASubjectFilter : ISubject, IObserver
{
    protected abstract void AGetUpdate(ISubject subject);
    public void GetUpdate(ISubject subject)
    {
        AGetUpdate(subject);
    }
    
    protected abstract void ANotify();
    public void Notify()
    {
        ANotify();
    }

    protected abstract void AAttach(IObserver observer);
    public void Attach(IObserver observer)
    {
        AAttach(observer);
    }
}

public abstract class AUnitySubjectFilter : MonoBehaviour, ISubject, IObserver
{
    protected abstract void AGetUpdate(ISubject subject);
    public void GetUpdate(ISubject subject)
    {
        AGetUpdate(subject);
    }
    
    protected abstract void ANotify();
    public void Notify()
    {
        ANotify();
    }

    protected abstract void AAttach();
    public void Attach(IObserver observer)
    {
        AAttach();
    }
}