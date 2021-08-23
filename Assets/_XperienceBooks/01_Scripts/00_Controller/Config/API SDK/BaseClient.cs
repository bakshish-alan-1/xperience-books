
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

            /*  switch (properties.m_CurrentEnvirnment)
              {
                  case DevEnvironment.Testing:
                      APIEnvironment = new APIEnvironment(properties.TestingBaseURL);
                      break;
                  case DevEnvironment.Local:
                      APIEnvironment = new APIEnvironment(properties.ProductionNew);
                      break;
                  case DevEnvironment.Production:
                      APIEnvironment = new APIEnvironment(properties.ProductionBaseURL);
                      break;
              }*/

            switch (PlayerPrefs.GetInt("ENV"))
            {
                case 1:
                    APIEnvironment = new APIEnvironment(properties.TestingBaseURL);
                    break;
                
                case 2:
                    APIEnvironment = new APIEnvironment(properties.ProductionBaseURL);
                    break;

                //case 3:
                //    APIEnvironment = new APIEnvironment(properties.ProductionNew);
                //    break;
            }
        }

    }
}
