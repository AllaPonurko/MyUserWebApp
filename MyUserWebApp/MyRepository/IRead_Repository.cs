using Microsoft.AspNetCore.Mvc;

namespace MyUserWebApp.MyRepository
{
    public interface IRead_Repository 
    {
        public Task<IItem> Profile(string name);
    }
}
