using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CommonServices
{
    public interface IUtilsService
    {
        public T? JsonDeserialize<T>(string input);

        public string JsonSerialize(object input);
        public bool IsStringValidJson(string input);
        public string ToBase64(string input);

        public string ToBase64(object input);
    }
    public class UtilsService : IUtilsService
    {
        public T? JsonDeserialize<T>(string input)
        {
            if (IsStringValidJson(input))
            {
                return JsonSerializer.Deserialize<T>(input);
            }
            return default(T);
        }

        public string JsonSerialize(object input)
        {
            return JsonSerializer.Serialize(input);
        }

        public string ToBase64(string input)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(plainTextBytes);
        }

        public string ToBase64(object input)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(JsonSerialize(input));
            return Convert.ToBase64String(plainTextBytes);
        }

        public bool IsStringValidJson(string input)
        {
            try
            {
                JsonSerializer.Deserialize<object>(input);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

