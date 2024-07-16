using PlatformService.Dto;

namespace PlatformService.SyncDataService.Http
{
    public interface ICommandDataClient
    {
        Task SentPlatformToCommand(PlatformReadDto platform);
    }
}