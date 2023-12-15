using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public partial class DateRangePickerModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime _startDatum = DateTime.Now;

        [ObservableProperty]
        private DateTime _endDatum = DateTime.Now.AddYears(1);
    }
}
