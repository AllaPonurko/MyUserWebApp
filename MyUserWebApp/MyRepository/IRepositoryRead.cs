using Microsoft.AspNetCore.Mvc;

namespace MyUserWebApp.MyRepository
{
    public interface IRepositoryRead 
    {
        public Task<IItem> Profile(string name);
    }
}
