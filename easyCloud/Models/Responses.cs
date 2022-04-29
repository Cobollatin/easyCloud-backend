using System;
using System.Collections.Generic;

namespace extraAhorro.Models.Responses
{

    public class Error
    {
        public string status { get; set; } = "error";
        public string message { get; set; } = null;

        public Error(string message)
        {
            this.message = message;
        }
    }

    public class Fail
    {
        public string status { get; set; } = "fail";
        public Dictionary<string, string> data { get; set; } = null;

        public Fail(Dictionary<string, string> data)
        {
            this.data = data;
        }
    }

    public class Get<T>
    {
        public string status { get; set; } = "success";
        public List<T> data { get; set; } = null;

        public Get(List<T> data)
        {
            this.data = data;
        }
    }

    public class Post<T>
    {
        public string status { get; set; } = "success";
        public List<T> data { get; set; } = new List<T>();

        public Post(List<T> data)
        {
            this.data = data;
        }
    }

    public class Delete<T>
    {
        public string status { get; set; } = "success";
        public string data { get; set; } = null;
    }

}
