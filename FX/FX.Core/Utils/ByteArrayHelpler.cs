using Newtonsoft.Json;
using System.Text;

namespace FX.Core.Utils
{
    public static class ByteArrayHelpler
    {
        public static T ByteArrayToObject<T>(byte[] arrBytes)
        {
            string result = Encoding.UTF8.GetString(arrBytes, 0, arrBytes.Length);
            var obj = JsonConvert.DeserializeObject<T>(result);
            return obj;
        }

        public static byte[] ObjectToByteArray(this object obj)
        {
            string str = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(str);
        }
    }
}