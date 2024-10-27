namespace PR_lab1;

using System.Text;
using System.Collections;
using System.Reflection;

public class CustomSerializer
{
    public string Serialize(object obj)
    {
        StringBuilder result = new StringBuilder();
        SerializeObject(obj, result);
        return result.ToString();
    }

    private void SerializeObject(object obj, StringBuilder result)
    {
        if (obj == null)
        {
            result.Append("null");
            return;
        }

        Type type = obj.GetType();

        if (type.IsPrimitive || obj is string || obj is decimal)
        {
            result.Append(obj.ToString());
            return;
        }

        if (obj is IEnumerable list)
        {
            result.Append("[");
            foreach (var item in list)
            {
                SerializeObject(item, result);
                result.Append(", ");
            }

            if (result[result.Length - 2] == ',')
                result.Length -= 2; // Remove trailing comma

            result.Append("]");
            return;
        }

        result.Append("{");
        var properties = type.GetProperties();
        foreach (var prop in properties)
        {
            result.Append(prop.Name + ": ");
            SerializeObject(prop.GetValue(obj), result);
            result.Append("; ");
        }

        if (result[result.Length - 2] == ';')
            result.Length -= 2; // Remove trailing semicolon

        result.Append("}");
    }

    public object Deserialize(string data, Type targetType)
    {
        return DeserializeObject(data, targetType);
    }

    private object DeserializeObject(string data, Type targetType)
    {
        if (string.IsNullOrEmpty(data)) return null;

        // Primitive types handling
        if (targetType == typeof(int)) return int.Parse(data);
        if (targetType == typeof(decimal)) return decimal.Parse(data);
        if (targetType == typeof(string)) return data;
        if (targetType == typeof(double)) return double.Parse(data);

        // List handling (assuming it's a list of strings for simplicity)
        if (data.StartsWith("[") && data.EndsWith("]"))
        {
            string content = data.Substring(1, data.Length - 2);
            var items = content.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            return items.Select(item => item.Trim()).ToList();
        }

        // Object deserialization
        if (data.StartsWith("{") && data.EndsWith("}"))
        {
            string content = data.Substring(1, data.Length - 2);
            object instance = Activator.CreateInstance(targetType);
            var properties = targetType.GetProperties();
            var pairs = content.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var keyValue = pair.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                if (keyValue.Length == 2)
                {
                    var property = properties.FirstOrDefault(p => p.Name == keyValue[0]);
                    if (property != null)
                    {
                        var value = DeserializeObject(keyValue[1], property.PropertyType);
                        property.SetValue(instance, value);
                    }
                }
            }
            return instance;
        }

        return null;
    }
}
