namespace FlinkFood.Services.Extensions
{
    public class CategoryAlreadyExisted:Exception
    {
        public CategoryAlreadyExisted(string message ):base(message)
        {
            
        }
    }
}
