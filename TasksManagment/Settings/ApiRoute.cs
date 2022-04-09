namespace TasksManagmentApi
{
    public static class ApiRoute
    {
        public static class ApplicatonUserRoutes
        {
            public const string Register = "register";
            public const string Login = "login";
            public const string GetAllEmployees = "getAllEmployees";
        }
   
        public static class TasksRoutes
        {
            public const string AddTasks = "AddTask";
            public const string EditTasks = "EditTask";
            public const string DeleteTasks = "DeleteTask";
            public const string GetAllTasks = "getAllTasks";
            public const string GetTasksDetailsById = "getTasksDetalsById";
            public const string GetTasksDetailsByUserId = "getTasksDetailsByUserId";
            public const string SearchInTasks = "searchByTerm";
        }
    }
}
