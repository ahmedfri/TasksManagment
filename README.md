TasksManagments 
This is a WebApi Demo Tasksmanagment relay on .Net Core 6 using Repository Pattern,Unit of Work And Entity Framework

login to the Admin account with this creditional:
User: Admin
Password: Admin@123
-Admin can Create Account of type admin or employee according to the type :  0 Stands for employee and 1 stands for admin
-Jwt Token Will be generated, use it for authrization before using End Points, to use token write word Bearer before the token
for example: 
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWl
tcy9uYW1lIjoiQWRtaW4iLCJVc2VyVHlwZSI6IkVtcGxveWVlIiwianRpIjoiOW
RiOWIwZDMtYTM1NC00ODM4LWEwMjYtOWMzYTBjYjE4NmE3IiwiZXhwIjoxNjUyMTI3MDcyLCJpc3MiOiIqIiwiYXVkIjoiKiJ9.Qxd_e_zI1IgnyA-N762OryKhemAHqq9B7fSwWTTGWuM.

 - API for Register
 - API for Login
 - API for Adding/Assign Tasks
 - API for Delete Task
 - API for Edit Task
 - API for listing Task
 - API for listing Tasks By id
 - API for Add Employments
 - API for listing Employments
 - API for listing Employments By id
 Database backup is attached other wise use Migrations commands after altreing the Appsettings.json