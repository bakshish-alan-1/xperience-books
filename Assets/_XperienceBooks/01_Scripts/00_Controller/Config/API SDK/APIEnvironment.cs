using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Intellify.core
{
    public class APIEnvironment
    {
        private string baseUrl;


        public APIEnvironment(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public string BaseUrl()
        {
            return this.baseUrl;
        }
    }
}
