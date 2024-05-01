
// 직접 구현(상속) x
public interface ISaveData
{
    public void Save();
}

// 둘 중 하나만 구현(상속) 할 것
public interface ISystemSaveData : ISaveData { }
public interface IGameSaveData : ISaveData { }
