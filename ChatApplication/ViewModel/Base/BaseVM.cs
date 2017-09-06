using PropertyChanged;
using System.ComponentModel;

namespace ChatApplication
{
    [AddINotifyPropertyChangedInterface]
    public class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender,e) => { };

        /// <summary>
        /// Allows custom firing of PropertyChanged event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this , new PropertyChangedEventArgs(name));
        }
    }
}
