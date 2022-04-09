using Entities.DataTransferObjects.Commons;

namespace Entities.DataTransferObjects.Tasks
{
    public class TasksListResponseDto : BaseResponse
    {
        public IList<TasksDto> Data { get; set; }
    }
}
