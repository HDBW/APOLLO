using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public class BoolenValue
        : BaseValue<bool>
    {
        public BoolenValue(
          long? id,
          string lable,
          bool value,
          bool defaultValue)
          : base(id, lable, value, defaultValue)
        {
        }
    }
}
