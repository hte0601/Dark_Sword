
/// <summary>
/// 이 인터페이스를 직접 사용하지 말 것
/// </summary>
public interface ISaveData
{
    public void Save();
}


/// <summary>
/// 이 인터페이스를 직접 사용하지 말 것 <br />
/// 대신 ISystemSaveData 또는 ISingleGameSaveData 사용
/// </summary>
public interface ISingleSaveData : ISaveData { }

/// <summary>
/// 이 인터페이스를 직접 사용하지 말 것 <br />
/// 대신 IMultipleGameSaveData 사용
/// </summary>
public interface IMultipleSaveData : ISaveData
{
    public int DataID { get; set; }
}


// 다음 중 하나만 구현할 것
public interface ISystemSaveData : ISingleSaveData { }
public interface ISingleGameSaveData : ISingleSaveData { }
public interface IMultipleGameSaveData : IMultipleSaveData { }
