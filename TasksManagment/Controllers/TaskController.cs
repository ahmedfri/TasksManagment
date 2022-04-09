using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Commons;
using Entities.DataTransferObjects.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TasksManagmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public readonly string _userId;
        public readonly string _userType;
        private readonly ClaimsPrincipal _claimsPrincipal;
        public TaskController(IUnitOfWork unitOfWork,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
            _claimsPrincipal = httpContextAccessor.HttpContext.User;
            Claim userClaim = _claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
            Claim userClaimType = _claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "UserType");
            _userId = userClaim.Value;
            _userType= userClaimType.Value;
        }

        [HttpGet]
        [Route(ApiRoute.TasksRoutes.GetTasksDetailsById)] 
        public async Task<IActionResult> GetTaskDetailsById(int Id)
        {
            var task = await _unitOfWork.Tasks.Get(q => q.Id == Id);
            try
            {
                var result = _mapper.Map<TasksDetailsDto>(task);
                TasksDetailsResponseDto tasksResponseDetailsDto = new()
                {
                    Data = result,
                    ResponseMessage = "Requeste Completed Successfully",
                    StatusCode = 200
                };
                return Ok(tasksResponseDetailsDto);
            }
            catch (Exception ex)
            {
                BaseResponse response = new() { ResponseMessage = ex.Message, StatusCode = 401 };
                return BadRequest(response);
            }

        }
        [HttpGet]
        [Route(ApiRoute.TasksRoutes.GetTasksDetailsByUserId)]
        public async Task<IActionResult> GetTasksDetailsByUserId(string UserId)
        {
            IList<Tasks> tasks = await _unitOfWork.Tasks.GetAll(q => q.ApplicationUserId == UserId);
            try
            {
                List<TasksDto> result = _mapper.Map<List<TasksDto>>(tasks);
                TasksListResponseDto tasksResponseDetailsDto = new()
                {
                    Data = result,
                    ResponseMessage = "Requeste Completed Successfully",
                    StatusCode = 200
                };
                return Ok(tasksResponseDetailsDto);
            }
            catch (Exception ex)
            {
                BaseResponse response = new() { ResponseMessage = ex.Message, StatusCode = 401 };
                return BadRequest(response);
            }

        }
        [HttpGet]
        [Route(ApiRoute.TasksRoutes.GetAllTasks)]
        public async Task<IActionResult> GetAllTasks([FromQuery] PaginationParameters paginationParameters)
        {
            var hotels = await _unitOfWork.Tasks.GetPagedList(paginationParameters);
            try
            {
                var results = _mapper.Map<IList<TasksDto>>(hotels);
                TasksListResponseDto tasksListResponseDto = new()
                {
                    Data = results,
                    ResponseMessage = "Requeste Completed Successfully",
                    StatusCode = 200
                };
                return Ok(tasksListResponseDto);
            }
            catch (Exception ex)
            {
                BaseResponse response = new() { ResponseMessage = ex.Message, StatusCode = 401 };
                return BadRequest(response);
            }
        }

        [HttpGet]
        [Route(ApiRoute.TasksRoutes.SearchInTasks)]
        public async Task<IActionResult> SearchInTasks(string searchTerm)
        {
            try
            {
                var hotels = await _unitOfWork.Tasks.GetAll(q => q.Name.Contains(searchTerm));
                var results = _mapper.Map<IList<TasksDto>>(hotels);
                TasksListResponseDto hotelListResponseDto = new()
                {
                    Data = results,
                    ResponseMessage = "Requeste Completed Successfully",
                    StatusCode = 200
                };
                return Ok(hotelListResponseDto);
            }
            catch (Exception ex)
            {
                BaseResponse response = new() { ResponseMessage = ex.Message, StatusCode = 401 };
                return BadRequest(response);
            }


        }
        [Authorize]
        [HttpPost]
        [Route(ApiRoute.TasksRoutes.AddTasks)]
        public async Task<IActionResult> AddTasks([FromBody] AddTasksDto addTaskDto)
        {
            try
            {
                Tasks task = _mapper.Map<AddTasksDto, Tasks>(addTaskDto);
                task.ApplicationUserId = _userId;
                task.CreatedDate = DateTime.Now;
                await _unitOfWork.Tasks.AddAsync(task);
                await _unitOfWork.Save();
                BaseResponse response = new() { ResponseMessage = "Task added successfully", StatusCode = 200 };
                return Ok(response);
            }
            catch (Exception ex)
            {
                BaseResponse response = new() { ResponseMessage = ex.Message, StatusCode = 401 };
                return BadRequest(response);
            }

        }

        [Authorize]
        [HttpPost]
        [Route(ApiRoute.TasksRoutes.EditTasks)]
        public async Task<IActionResult> EditTasks([FromBody] EditTasksDto EditTasksDto)
        {
            try
            {
                var task = await _unitOfWork.Tasks.Get(x => x.Id == EditTasksDto.Id);

                task = _mapper.Map<EditTasksDto, Tasks>(EditTasksDto);
                task.ApplicationUserId = _userId;
                task.CreatedDate = DateTime.Now;
                _unitOfWork.Tasks.Update(task);
                await _unitOfWork.Save();
                BaseResponse response = new() { ResponseMessage = "Task Updated successfully", StatusCode = 200 };
                return Ok(response);
            }
            catch (Exception ex)
            {
                BaseResponse response = new() { ResponseMessage = ex.Message, StatusCode = 401 };
                return BadRequest(response);
            }

        }

        [Authorize]
        [HttpPost]
        [Route(ApiRoute.TasksRoutes.DeleteTasks)]
        public async Task<IActionResult> DeleteTasks(int Id)
        {
            try
            {

                await _unitOfWork.Tasks.Delete(Id);
                await _unitOfWork.Save();
                BaseResponse response = new() { ResponseMessage = "Task deleted successfully", StatusCode = 200 };
                return Ok(response);
            }
            catch (Exception ex)
            {
                BaseResponse response = new() { ResponseMessage = ex.Message, StatusCode = 401 };
                return BadRequest(response);
            }

        }
    }
}
