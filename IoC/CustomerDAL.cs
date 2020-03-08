using IoCContainer;

namespace IoC
{
    [Export(typeof(ICustomerDAL))]
    public class CustomerDAL : ICustomerDAL
    {

    }
    public interface ICustomerDAL
    {
    }
}
