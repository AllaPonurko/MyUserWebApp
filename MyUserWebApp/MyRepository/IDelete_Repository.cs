using Microsoft.AspNetCore.Mvc;

namespace MyUserWebApp.MyRepository
{
    public interface IDelete_Repository
    {
        public  Task<bool> Delete(string id);
    }
}
