using System;
using System.Collections.Generic;

namespace easyCloud.Models.Shared {

    public class RequestResponse {

        public RequestResponse(string status, Dictionary<string, dynamic> payload) {
            Status = status;
            Payload = payload;
        }

        public string Status { get; set; } = "error";
        public Dictionary<string, dynamic> Payload { get; set; }
    }

}
