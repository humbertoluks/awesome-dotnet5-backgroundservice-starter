using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace awesome_dotnet5_backgroundservice_starter
{
    public class ToDoService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ToDoService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<string> GetTodoAsync()
        {
            try
            {

                Random rnd = new Random();
                int id = rnd.Next(1, 200);
                var TodoApiUrl = $"https://jsonplaceholder.typicode.com/todos/{id}";

                // The API returns an array with a single entry.
                Todo? todo = await _httpClient.GetFromJsonAsync<Todo>(
                    TodoApiUrl, _options);

                return todo is not null
                    ? $"{todo.id}{Environment.NewLine}{todo.title}"
                    : "No todo here...";
            }
            catch (Exception ex)
            {
                return $"That's not funny! {ex}";
            }
        }
    }

    public record Todo(int userId, int id, string title, Boolean completed);
}


