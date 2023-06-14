using Microsoft.AspNetCore.Mvc;
using System.Collections;


namespace MyUserWebApp.MyRepository
{
    public interface IRead_Repository 
    {
        public Task<IItem> Profile(string name);
        public Task<ICollection> GetAllUsers();
    }
}
