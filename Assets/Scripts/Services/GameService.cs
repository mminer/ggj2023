public class GameService : Services.Service
{
    void Start()
    {
        var dungeonService = Services.Get<DungeonService>();
        dungeonService.GenerateDungeon();
    }
}
