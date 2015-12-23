using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BucketGame
{
    class Game
    {
        private int[] scorePerTarget;
        public event Action<Game> GameWon = (sender) => { };

        public IEnumerable<int> ScorePerTarget
        {
            get { return scorePerTarget.AsReadOnly(); }
        }

        public int GetScoreOfTarget(int targetIndex)
        {
            return scorePerTarget[targetIndex];
        }

        public void SetScore(int targetIndex, int value)
        {
            scorePerTarget[targetIndex] = value;
        }

        public void IncrementScore(int targetIndex, int toAdd)
        {
            SetScore(targetIndex, GetScoreOfTarget(targetIndex) + toAdd);
        }

        public void IncrementScore(int targetIndex)
        {
            IncrementScore(targetIndex, 1);
        }

        public int TotalScore
        {
            get
            {
                return scorePerTarget.Sum();
            }
        }

        public int TargetCount
        {
            get
            {
                return scorePerTarget.Length;
            }
        }

        private DateTime timeStarted;
        public DateTime TimeStarted
        {
            get
            {
                return timeStarted;
            }

            set
            {
                timeStarted = value;
            }
        }

        public TimeSpan TimeElapsed
        {
            get
            {
                return DateTime.Now - timeStarted;
            }
        }

        internal void Start()
        {
            timeStarted = DateTime.Now;

        }

        public Game()
        {
            this.scorePerTarget = new int[Consts.BagPaths.Length];
        }
    }
}
