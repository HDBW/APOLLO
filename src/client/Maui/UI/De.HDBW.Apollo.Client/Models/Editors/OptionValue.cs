using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public class OptionValue : BaseValue<string>
    {
        public OptionValue(
          long? id,
          string lable,
          string value)
          : base(id, lable, value, null)
        {
        }
    }
}
