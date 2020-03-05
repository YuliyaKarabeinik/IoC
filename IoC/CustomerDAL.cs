using System;
using System.Collections.Generic;
using System.Text;

namespace IoC
{
    [Export(typeof(ICustomerDAL))]
    public class CustomerDAL : ICustomerDAL
    {

    }
}
