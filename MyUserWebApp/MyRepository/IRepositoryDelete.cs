using Microsoft.AspNetCore.Mvc;

namespace MyUserWebApp.MyRepository
{
    public interface IRepositoryDelete
    {
        public  Task<ActionResult> Delete(string id);
    }
}
