using System;

/// <summary>
/// Classes that implement this interface should have an serialized instance of DataSettings to register through.
/// </summary>
public interface IDataPersister
{
    DataSettings GetDataSettings();

    void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType);

    SData SaveData();

    void LoadData(SData data);
}

[Serializable]
public class DataSettings
{
    public enum PersistenceType
    {
        DoNotPersist,
        ReadOnly,
        WriteOnly,
        ReadWrite,
    }

    public string dataTag = Guid.NewGuid().ToString();
    public PersistenceType persistenceType = PersistenceType.ReadWrite;

    public override string ToString()
    {
        return dataTag + " " + persistenceType.ToString();
    }
}

public class SData
{

}

public class SData<T> : SData
{
    public T value;

    public SData(T value)
    {
        this.value = value;
    }
}

public class SData<T0, T1> : SData
{
    public T0 value0;
    public T1 value1;

    public SData(T0 value0, T1 value1)
    {
        this.value0 = value0;
        this.value1 = value1;
    }
}

public class SData<T0, T1, T2> : SData
{
    public T0 value0;
    public T1 value1;
    public T2 value2;

    public SData(T0 value0, T1 value1, T2 value2)
    {
        this.value0 = value0;
        this.value1 = value1;
        this.value2 = value2;
    }
}

public class SData<T0, T1, T2, T3> : SData
{
    public T0 value0;
    public T1 value1;
    public T2 value2;
    public T3 value3;

    public SData(T0 value0, T1 value1, T2 value2, T3 value3)
    {
        this.value0 = value0;
        this.value1 = value1;
        this.value2 = value2;
        this.value3 = value3;
    }
}

public class SData<T0, T1, T2, T3, T4> : SData
{
    public T0 value0;
    public T1 value1;
    public T2 value2;
    public T3 value3;
    public T4 value4;

    public SData(T0 value0, T1 value1, T2 value2, T3 value3, T4 value4)
    {
        this.value0 = value0;
        this.value1 = value1;
        this.value2 = value2;
        this.value3 = value3;
        this.value4 = value4;
    }
}