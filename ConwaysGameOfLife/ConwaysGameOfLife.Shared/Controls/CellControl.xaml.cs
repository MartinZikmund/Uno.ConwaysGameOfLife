using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ConwaysGameOfLife.Shared.Controls
{
    public sealed partial class CellControl : UserControl
    {
        public CellControl()
        {
            this.InitializeComponent();
        }

        public static DependencyProperty IsAliveProperty { get; } = DependencyProperty.Register(
            nameof(IsAlive), typeof(bool), typeof(CellControl), new PropertyMetadata(default(bool)));

        public bool IsAlive
        {
            get => (bool) GetValue(IsAliveProperty);
            set => SetValue(IsAliveProperty, value);
        }
    }
}
