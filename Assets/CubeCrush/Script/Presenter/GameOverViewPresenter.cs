using Loyufei.DomainEvents;

namespace CubeCrush
{
    public class GameOverViewPresenter : Presenter
    {
        public GameOverViewPresenter(GameOverView view)
        {
            View = view;
        }

        public GameOverView View { get; }

        protected override void RegisterEvents()
        {
            Register<StartGame>(StartGame);
            Register<GameOver> (GameOver);
        }

        private void StartGame(StartGame start) 
        {
            View.Close();
        }

        private void GameOver(GameOver gameOver)
        {
            View.Open();
        }
    }
}