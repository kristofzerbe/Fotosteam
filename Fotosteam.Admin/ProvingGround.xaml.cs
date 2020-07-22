using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Fotosteam.Admin
{
    /// <summary>
    /// Interaction logic for ProvingGround.xaml
    /// </summary>
    public partial class ProvingGround : UserControl
    {
        public ProvingGround()
        {
            InitializeComponent();
	        DataContext = new ProvingGroundViewModel();
        }
    }

	public class ProvingGroundViewModel : INotifyPropertyChanged
	{
		private string _name;

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
