using MyUserWebApp.Models;

namespace MyUserWebApp.MyException
{
    public class AllException
    {
        public MyUser NotFoundObjectResult(string v)
        {
            throw new NotImplementedException();
        }
        public bool RankException(string v)
        {
            throw new NotImplementedException();
        }
        public string ArgumentNullException(string v)
        {
            throw new NotImplementedException();
        }
    }
}
