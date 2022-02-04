

namespace HahnDroneAPI.Configurations
{
    public interface ICustomConfiguration
    {
        int MemoryCacheTimeOut();
        int DroneCount();
        int BatteryLowerLimit();
        int GetDroneUpperWeightLimit();
    }
}
