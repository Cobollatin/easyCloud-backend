using easyCloud.Shared.Domain.Services.Communication;

namespace easyCloud.Record.Domain.Services.Communication;

public class RecordResponse: BaseResponse<Models.Record>
{
    public RecordResponse(string message) : base(message)
    {
    }

    public RecordResponse(Models.Record record) : base(record)
    {
    }
}