using Entities.DataTransferObjects.Commons;

namespace Entities.DataTransferObjects.Tasks
{
    public class TasksDetailsResponseDto : BaseResponse
    {
        public TasksDetailsDto Data { get; set; }
    }
    public class TasksDetailsResponseListDto : BaseResponse
    {
        public List<TasksDetailsDto> Data { get; set; }
    }
}
