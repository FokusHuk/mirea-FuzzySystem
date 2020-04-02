using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FuzzySystemGUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        AirLandingSystem airLandingSystem;

        public MainWindow()
        {
            InitializeComponent();

            airLandingSystem = new AirLandingSystem(0, 1500, 0);
            airLandingSystem.execute();

            FlapValue.Text = airLandingSystem.flapOutput;
            LandAngelValue.Text = airLandingSystem.angelOutput;
            BrakeValue.Text = airLandingSystem.brakeOutput;
        }

        private void WeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (WeightSliderLabel != null)
            {
                WeightSliderLabel.Content = Math.Round(WeightSlider.Value, 0);

                airLandingSystem.weight = Math.Round(WeightSlider.Value, 0);

                airLandingSystem.execute();

                FlapValue.Text = airLandingSystem.flapOutput;
                LandAngelValue.Text = airLandingSystem.angelOutput;
                BrakeValue.Text = airLandingSystem.brakeOutput;
            }
        }

        private void WindSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (WindSliderLabel != null)
            {
                WindSliderLabel.Content = Math.Round(WindSlider.Value, 1);

                airLandingSystem.windPower = Math.Round(WindSlider.Value, 1);

                airLandingSystem.execute();

                FlapValue.Text = airLandingSystem.flapOutput;
                LandAngelValue.Text = airLandingSystem.angelOutput;
                BrakeValue.Text = airLandingSystem.brakeOutput;
            }
        }

        private void RunwayInclineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (RunwayInclineSliderLabel != null)
            {
                RunwayInclineSliderLabel.Content = Math.Round(RunwayInclineSlider.Value, 1);

                airLandingSystem.runwayIncline = Math.Round(RunwayInclineSlider.Value, 1);

                airLandingSystem.execute();

                FlapValue.Text = airLandingSystem.flapOutput;
                LandAngelValue.Text = airLandingSystem.angelOutput;
                BrakeValue.Text = airLandingSystem.brakeOutput;
            }
        }
    }
}
