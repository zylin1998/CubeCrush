using Loyufei;

namespace CubeCrush
{
    public class Report
    {
        public Report(DataUpdater updater) 
        {
            _Updater = updater;
        }

        private DataUpdater _Updater;

        public int Score { get; protected set; }

        public void ReportScore(int score) 
        {
            Score += score;

            _Updater.Update(Declarations.Score, Score);
        }
    }
}
