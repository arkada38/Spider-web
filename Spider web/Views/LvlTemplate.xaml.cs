using Spider_web.Models;

namespace Spider_web.Views
{
    internal sealed partial class LvlTemplate
    {
        public UserLvl UserLvl => DataContext as UserLvl;

        public LvlTemplate()
        {
            InitializeComponent();

            DataContextChanged += (s, e) => Bindings.Update();
        }
    }
}
