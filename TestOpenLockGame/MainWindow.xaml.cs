using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TestOpenLockGame.Data;

namespace TestOpenLockGame;

public partial class MainWindow : Window
{
    private DispatcherTimer Timer;

    private bool MoveUpOrDown = true;

    private int ChoiceSpeedway = 0;

    private int VictoryPoints = 0;

    private int SpeedChoice = 0;   
    private readonly int[] PercentSpeeds = [1, 2, 3, 4, 5];

    private const int PercentSaveZone = 10;    


    private double SpeedwayActualHeight() => Speedway.ActualHeight;
    private Border ChoiceRunner() => FindName("Runner" + ChoiceSpeedway) as Border;
    private double CheckZone() => SpeedwayActualHeight() / 100 * 30;
    private double SaveZone() => SpeedwayActualHeight() / 100 * PercentSaveZone;
    private double Speed(int Speed) => SpeedwayActualHeight() / 100 * Speed;


    public MainWindow()
    {
        InitializeComponent();

        Timer = new DispatcherTimer();
        Timer.Interval = TimeSpan.FromMilliseconds(1);

        Timer.Tick += Timer_Tick;
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        Border runner = ChoiceRunner();
        double speed = Speed(PercentSpeeds[SpeedChoice]);

        if (MoveUpOrDown)
        {
            if (MovementRunner.UpMove(runner, SpeedwayActualHeight(), speed))
            {
                MoveUpOrDown = false;
            }
        }
        else
        {
            if (MovementRunner.DownMove(runner, speed))
            {
                MoveUpOrDown = true;
                Timer.Stop();
            }
        }
    }

    private void Control(object sender, KeyEventArgs e)
    {        
        switch (e.Key.ToString())
        {
            case "Space":
                Start();
                break;

            case "Left":
                ChoiceLeft();
                break;

            case "Right":
                ChoiceRight();
                break;
        }
    }     

    private void Start()
    {
        if (!Timer.IsEnabled)
        {   
            if (ChoiceRunner().Margin.Bottom != CheckZone())
            {
                Timer.Start();
            }
        }
        else
        {
            Lock(ChoiceRunner());
        }

        SpeedChoice = SpeedChoice < PercentSpeeds.Length - 1 ? ++SpeedChoice : 0;
    }

    private void Lock(Border Runner)
    {
        Timer.Stop();

        double max = Math.Abs(Runner.Margin.Bottom - CheckZone());

        if (max <= SaveZone())
        {
            Runner.Margin = new Thickness(0, 0, 0, CheckZone());

            VictoryPoints++;

            if (VictoryPoints == 5)
            {
                MessageBox.Show("ПОБЕДА!");
                Close();
            }
        }
        else
        {
            for (int i = 0; i <= 4; i++)
            {
                (FindName("Runner" + i) as Border).Margin = new Thickness(0);
                VictoryPoints = 0;
            }
        }
    }

    private void ChoiceLeft()
    {
        if (ChoiceSpeedway > 0 & !Timer.IsEnabled)
        {
            Grid.SetColumn(Selector, --ChoiceSpeedway);
        }
    }

    private void ChoiceRight()
    {
        if (ChoiceSpeedway < 4 & !Timer.IsEnabled)
        {
            Grid.SetColumn(Selector, ++ChoiceSpeedway);
        }
    }
}