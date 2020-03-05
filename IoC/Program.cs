using IoCContainer;

namespace IoC
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();
            container.Register<ICustomerDAL, CustomerDAL>();
            container.Register<Logger, Logger>();
            container.Register<CustomerBLL, CustomerBLL>();

            var instance = container.Resolve<CustomerBLL>();
        }
    }
}
