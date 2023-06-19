namespace MyUserWebApp.MyRepository
{
    public interface ICRUD_Repository:IDelete_Repository,IRead_Repository,
        IUpdate_Repository, ICreate_Repository,ILogin_Repository
    {

    }
}
