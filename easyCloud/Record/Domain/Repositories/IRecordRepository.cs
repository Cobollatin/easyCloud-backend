namespace easyCloud.Record.Domain.Repositories;

public interface IRecordRepository
{
    Task<IEnumerable<Models.Record>> ListAsync();
    Task AddAsync(Models.Record record);
    void Update(Models.Record record);
    void Remove(Models.Record record);
}