using PaymentSDK.Core;
using UnityEngine;




[CreateAssetMenu(fileName = "PaypalConfig", menuName = "PaymentSDK/PayPal", order =1)]
public class PaypalConfig : ScriptableObject
{
    #region PayPal Configuration
    [Space(5)]

    public PaymentEnvironment m_CurrentEnvirnment;

    [Header("PayPal SandBox")]
    public string payPalSandboxClientId;
    public string payPalSandboxClientSecret;
    public string paypalSandboxBaseURL= "https://api.sandbox.paypal.com";


    [Header("PayPal Live")]
    public string payPalClientId;
    public string payPalClientSecret;
    public string paypalBaseURL= "https://api.paypal.com";


    [Header("Response Configration")]
    [Tooltip("Local File must be exist in  Assets/StreamingAssets/PaymentSDK/payment-approved.html ")]
    public string payPalSuccessUrl = "payment-approved.html";
    [Tooltip("Local File must be exist in  Assets/StreamingAssets/PaymentSDK/payment-canceled.html")]
    public string payPalCancelUrl = "payment-canceled.html";
    #endregion

   
    #region Other Settings
    [Header("Other Settings")]
    [Tooltip("Type of currency to use for purchases. In three-letter ISO-4217 format.")]
    public string currencyCode = "USD";
    #endregion
}
