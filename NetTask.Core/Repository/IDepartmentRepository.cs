namespace NetTask.Core.Repository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        void Update(Department obj);

    }
}
