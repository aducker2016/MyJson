using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyJson
{
    public class BossSkill
    {
        public int bornTime { get; set; }
        public int code { get; set; }
        public int maxLife { get; set; }
        public int order { get; set; }
        public int rate { get; set; }
        public int skillId { get; set; }
        public int noticeSec { get; set; }
        public string noticeText { get; set; }
    }

    public static class JSON
    {
        public static T parse<T>(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }

        public static string stringify(object jsonObject)
        {
            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(jsonObject.GetType()).WriteObject(ms, jsonObject);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //第一种用法
            string jsStr = File.ReadAllText(@"test.json");
            List<BossSkill> l = JSON.parse<List<BossSkill>>(jsStr);

            var jsonString = JSON.stringify(l[0]);
            Console.WriteLine(jsonString);

            //第二种用法
            JArray json = JArray.Parse(jsStr);
            foreach(JObject obj in json)
            {
                int code = (int)obj["code"];
                string skillId = obj["skillId"].ToString();
                Console.WriteLine("code:{0} skillId:{1} noticeText:{2}", code, skillId, obj["noticeText"]);
            }

            var jsonString2 = json.ToString();
            Console.WriteLine(jsonString2);

            //第三种方法
            BossSkill bs = new BossSkill();
            bs.code = 11111;
            bs.noticeText = "测试中...1/2/3";
            Console.WriteLine(JsonConvert.SerializeObject(bs, Formatting.Indented));

            List<BossSkill> bslist = JsonConvert.DeserializeObject<List<BossSkill>>(jsStr);

            //其他
            JToken test = JValue.Parse(jsStr);

            dynamic address = new JObject();
            address.Province = "GuangDong";
            address.City = "GuangZhou";
            address.County = "PanYu";
            address.Villages = new JArray("大龙村", "小龙村");
            Console.WriteLine(address.ToString());

            JObject ob = new JObject();
            ob["title"] = "test";
            ob["contents"] = new JArray(
                new JObject(new JProperty("name", "Li"), new JProperty("age", 13)),
                new JObject(new JProperty("name", "Wang"), new JProperty("age", 25))
                );
            ob["contents"][0]["name"] = "XiaoLi";
            ob["contents"][0].Remove();
            var names = from o in ob["contents"].Children()
                select (string)o["name"];
            foreach (string name in names)
            {
                Console.WriteLine(name);
            }

            Console.ReadKey();
        }
    }
}
