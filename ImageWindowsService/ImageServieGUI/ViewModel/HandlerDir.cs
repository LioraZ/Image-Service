using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServieGUI.ViewModel
{
    /// <summary>
    /// Class HandlerDir.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    class HandlerDir : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The name
        /// </summary>
        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != this.name)
                   name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() { return name; }
    }
}
