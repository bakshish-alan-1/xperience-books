

using PaymentSDK.Core;
using UnityEngine;

public abstract class BaseClient
{
    protected PaypalConfig properties;
    protected PayPalEnvironment PayPalEnvironment;

    public BaseClient()
    {
        Init();
    }

    public void Init()
    {
      
        properties = Resources.Load<PaypalConfig>("PaypalConfig");

        switch (properties.m_CurrentEnvirnment)
        {
            case PaymentEnvironment.Sandbox:
                Debug.Log("Testing Envirnment Set");
                PayPalEnvironment = new PayPalEnvironment(properties.payPalSandboxClientId, properties.payPalSandboxClientSecret, properties.paypalSandboxBaseURL);
                break;
            case PaymentEnvironment.Production:
                Debug.Log("Production Envirnment Set");
                PayPalEnvironment = new PayPalEnvironment(properties.payPalClientId, properties.payPalClientSecret, properties.paypalBaseURL);
                break;
        }
    }
}
