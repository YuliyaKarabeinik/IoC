using System;
using System.Collections.Generic;
using System.Text;

namespace IoC
{
    [ImportConstructor]
    public class CustomerBLL
    {
        public CustomerBLL(ICustomerDAL dal, Logger logger)
        {

        }
    }
}
