using System;
using UnityEngine;

namespace TMG.ManagedComponents
{
    public class TestGameController : MonoBehaviour
    {
        public static TestGameController Instance;
        
        private int _team1Score;
        private int _team2Score;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _team1Score = _team2Score = 0;
        }

        public void IncrementScore(int teamID, int score)
        {
            switch (teamID)
            {
                case 1:
                    _team1Score += score;
                    break;
                case 2:
                    _team2Score += score;
                    break;
            }
            
            Debug.Log($"Team {teamID} scored {score} point(s).\nThe score is now {_team1Score} - {_team2Score}");
        }
    }
}