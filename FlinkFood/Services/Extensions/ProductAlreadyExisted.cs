namespace FlinkFood.Services.Extensions
{
    public class ProductAlreadyExisted:Exception
    {
        public ProductAlreadyExisted(string message):base(message)
        {
            
        }
    }
}
