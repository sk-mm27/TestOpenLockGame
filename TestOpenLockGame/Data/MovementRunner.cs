using System.Windows.Controls;
using System.Windows;

namespace TestOpenLockGame.Data
{
    class MovementRunner
    {
        public static bool UpMove(Border Runner, double SpeedwayActualHeight, double Speed)
            => Move(Runner, Speed, true, SpeedwayActualHeight);
        public static bool DownMove(Border Runner, double Speed)
            => Move(Runner, Speed, false);

        private static bool Move(Border Runner, double Speed, bool UpOrDown, double SpeedwayActualHeight = -1)
        {
            double boundary = UpOrDown ?
                SpeedwayActualHeight - (Runner.Margin.Bottom + Runner.ActualHeight) :
                Runner.Margin.Bottom;

            if (boundary > 0)
            {
                Runner.Margin = boundary - Speed > 0 ?
                        new Thickness(0, 0, 0, Runner.Margin.Bottom + (Speed * (UpOrDown ? 1 : -1))) : // Увеличиваем/уменьшаем расстояние между бегунком и началом трека
                        new Thickness(0, 0, 0, UpOrDown ? SpeedwayActualHeight - Runner.ActualHeight : 0); // Соприкасаем бегунок с концом/началом трека

                return false;
            }

            return true;
        }
    }
}
