
import os

# command = 'cd HRManagement.Infrastructure'
# os.system(command)
command = "dotnet ef migrations add MoveColumns -s ../HRManagement.API/HRManagement.API.csproj"
# command = "dotnet ef database update -s ../HRManagement.API/HRManagement.API.csproj"
os.system(command)