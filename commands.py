
import os

# command = 'cd HRManagement.Infrastructure'
# os.system(command)
# command = "dotnet ef migrations add InitMigrations -s ../HRManagement.API/HRManagement.API.csproj"
command = "dotnet ef database update -s ../HRManagement.API/HRManagement.API.csproj"
os.system(command)