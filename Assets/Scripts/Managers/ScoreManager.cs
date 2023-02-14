using Zenject;

namespace Dices.UIConnection
{ 
public class ScoreManager : IInitializable // Class for cotein score
{
        public int Score;
        public int[] ScoreDetales = { 0, 0, 0, 0, 0, 0 };
        public int[] ScoreHistory = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        public bool IsSpawned, SpawnBySwipe;
        
        public void Initialize()
        {

        }
    }
}
