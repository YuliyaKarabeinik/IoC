using System.Reflection;
using IoCContainer;

namespace IoC
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var instance1 = container.CreateInstance<Logger>();
            var instance2 = container.CreateInstance<ICustomerDAL>();
            var instance3 = container.CreateInstance<CustomerBLL>();
            var instance4 = container.CreateInstance<CustomerBLL2>();

            //container.AddType(typeof(CustomerBLL));
            //container.AddType(typeof(Logger));
            //container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));
            //var instance = container.CreateInstance<CustomerBLL>();
        }
    }
}
