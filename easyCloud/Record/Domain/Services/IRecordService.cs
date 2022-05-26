using easyCloud.Record.Domain.Services.Communication;

namespace easyCloud.Record.Domain.Services;

public interface IRecordService
{
    Task<IEnumerable<Models.Record>> ListAsync();
    Task<RecordResponse> SaveAsync(Models.Record record);
    Task<RecordResponse> UpdateAsync(int recordId, Models.Record record);
    Task<RecordResponse> DeleteAsync(int recordId);
}