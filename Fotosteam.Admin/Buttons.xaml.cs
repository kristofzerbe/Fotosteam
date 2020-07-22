using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Fotosteam.Admin
{
    /// <summary>
    /// Interaction logic for Buttons.xaml
    /// </summary>
    public partial class Buttons : UserControl
    {
        public Buttons()
        {
            InitializeComponent();
        }

	    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
	    {
			Debug.WriteLine("Just checking we haven't suppressed the button.");
		}
    }
}
