using Zenject;
using UnityEngine;
using System.Collections.Generic;


namespace Dices.UIConnection
{ 
public class ScoreManager : IInitializable // Class for cotein score
{
        public int Score;
        public int[] ScoreDetales = { 0, 0, 0, 0, 0, 0 };
        public int[] ScoreHistory = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        public bool IsSpawned, SpawnBySwipe;
        public bool[] SelectedScores = {false, false, false, false, false, false};
        public List<Material> DetailMarkerMaterials = new List<Material>();
        public GameObject ScoreCountPlane;
        
        public void Initialize()
        {

        }
    }
}
