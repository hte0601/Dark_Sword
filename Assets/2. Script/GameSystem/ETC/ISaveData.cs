
// 직접 구현(상속) x
public interface ISaveData
{
    public void Save();
}

public interface ISingleSaveData : ISaveData { }
public interface IMultipleSaveData : ISaveData
{
    public int DataID { get; set; }
}


// 다음 중 하나만 구현(상속) 할 것
public interface ISystemSaveData : ISingleSaveData { }
public interface ISingleGameSaveData : ISingleSaveData { }
public interface IMultipleGameSaveData : IMultipleSaveData { }
