using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TestDDD.Domain.Entities;
using Xunit.Abstractions;

namespace Store.IntegrationTests;

public class UserServicesTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly HttpClient _client;
    
    public UserServicesTests(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetUser_Should_ReturnUser()
    {
        //Arrange - prepara el escenario para el test
        var userId = 2;
        var requestUrl = $"/api/Users/get-user/{userId}";
        
        //Act - realizar la accion 
        var response = await _client.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();
        
        //Assert - comprobar que el estado del sistema es lo esperado
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Alvaro", responseString);
        _testOutputHelper.WriteLine(responseString);
        
    }
    [Fact]
    public async Task CreateUser_Should_CreateUser()
    {
        //Arrange
        var user = new TestDDD.API.DTOs.CreateUserRequest
        {
            Username = "Nestor"
        };
        var requestUrl = "/api/Users/create-user";
        //Act
        var userJson = JsonSerializer.Serialize(user);
        var content = new StringContent(userJson, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(requestUrl, content);

        //Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(responseString);
    }
    [Fact]
    public async Task GetAllUsers_Should_ReturnAllUsers()
    {
      //Arrange 
      var requestUrl = "api/Users/get-all-users";
      
      //Act
      var response = await _client.GetAsync(requestUrl);
      
      //Assert
      Assert.True(response.IsSuccessStatusCode);
      var responseBody = await response.Content.ReadAsStringAsync();
      _testOutputHelper.WriteLine(responseBody);
      
    }
    
   [Fact]
    public async Task AssignTaskToUser_Should_AssignTaskToUser()
    {
        //Arrange
        var userA = new TestDDD.API.DTOs.CreateUserRequest
        {
            Username = "Alejandra"
        };
        var userB = new TestDDD.API.DTOs.CreateUserRequest
        {
            Username = "Ghazal"
        };
        var task = new TestDDD.API.DTOs.CreateTaskRequest
        {
            Title = "Fregar",
            Description = "you know",
        };
        
        //Act
        var userAId = await CreateUserAndGetId(userA);
        _testOutputHelper.WriteLine(userAId.ToString());
        var taskId = await CreateTaskAndGetId(userAId, task);
        var userBId = await CreateUserAndGetId(userB);
        _testOutputHelper.WriteLine(userBId.ToString());
        await AssignTaskToUserB(userBId, taskId);
        await GetUserById(userBId);

        //Assert

    }
    public async Task<int> CreateUserAndGetId(TestDDD.API.DTOs.CreateUserRequest user)
    {
        var url = "/api/Users/create-user";
        var userSerialized = JsonSerializer.Serialize(user);
        var bodyHeaderForCreateUser = new StringContent(userSerialized, Encoding.UTF8, "application/json");
        var responseCreateUser = await _client.PostAsync(url, bodyHeaderForCreateUser);
        var userContent =  await responseCreateUser.Content.ReadAsStringAsync(); 
        var userDeserialized = JsonSerializer.Deserialize<User>(userContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return userDeserialized.Id;
    }
    public async Task<int> CreateTaskAndGetId(int userId, TestDDD.API.DTOs.CreateTaskRequest task)
    {
        task.UserId = userId;
        var taskSerialized = JsonSerializer.Serialize(task);
        var content = new StringContent(taskSerialized, Encoding.UTF8, "application/json");
        var finalUrl = $"/api/Todo/create-task?userId={task.UserId}";
        var response = await _client.PostAsync(finalUrl,content);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var taskContent = JsonSerializer.Deserialize<TodoItem>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        _testOutputHelper.WriteLine($"Created Task Id: {taskContent.Id} \nAssigned to user: {userId} \nTask title: {taskContent.Title} \nTask Description: {taskContent.Description}");
        return taskContent.Id;

    }
    public async Task AssignTaskToUserB(int userId, int taskId)
    {
        var finalurl = $"/api/Users/assign-task/{userId}/{taskId}";
        _testOutputHelper.WriteLine(finalurl);
        var response = await _client.PutAsync(finalurl, null);
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            response.EnsureSuccessStatusCode();
        }
        var responseString = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(responseString);
    }
    public async Task GetUserById(int userId)
    {
        var url = $"/api/Users/get-user/{userId}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"User Created: {responseString}");
        var user = JsonSerializer.Deserialize<User>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (user == null)
        {
            _testOutputHelper.WriteLine("No se pudo deserializar el usuario.");
            return;
        }
        if (user.Tasks != null && user.Tasks.Any())
        {
            _testOutputHelper.WriteLine("Tareas asignadas:");
            foreach (var task in user.Tasks)
            {
                _testOutputHelper.WriteLine($" - [{task.Id}] {task.Title}: {task.Description}");
            }
        }
        else
        {
            _testOutputHelper.WriteLine("No tiene tareas asignadas.");
        }
    }
    
    
  


    
}














