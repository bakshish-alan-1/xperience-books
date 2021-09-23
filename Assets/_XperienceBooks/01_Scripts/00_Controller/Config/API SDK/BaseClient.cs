
namespace Intellify.core
{
    using UnityEngine;
    public abstract class BaseClient
    {
        protected Properties properties;
        protected APIEnvironment APIEnvironment;

        public BaseClient()
        {
            Init();
        }

        public void Init()
        {
            properties = Resources.Load<Properties>("Properties");

            switch (properties.m_CurrentEnvirnment)
            {
                case DevEnvironment.Testing:
                    APIEnvironment = new APIEnvironment(properties.TestingBaseURL);
                    break;
                
                case DevEnvironment.Production:
                    APIEnvironment = new APIEnvironment(properties.ProductionBaseURL);
                    break;

                case DevEnvironment.Staging:
                    APIEnvironment = new APIEnvironment(properties.Staging);
                    break;
            }
        }
    }
}
