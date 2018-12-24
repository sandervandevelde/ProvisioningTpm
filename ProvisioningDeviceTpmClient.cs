namespace ProvisioningTpm
{
    // This example is based on:
    // https://github.com/Azure-Samples/azure-iot-samples-csharp/tree/master/provisioning/Samples/device

    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Provisioning.Client;
    using Microsoft.Azure.Devices.Shared;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class ProvisioningDeviceTpmClient
    {
        ProvisioningDeviceClient _provClient;
        SecurityProvider _security;
        string _skipTest;

        public ProvisioningDeviceTpmClient(ProvisioningDeviceClient provisioningDeviceClient, SecurityProvider security, string skipTest)
        {
            _provClient = provisioningDeviceClient;
            _security = security;
            _skipTest = skipTest;
        }

        public async Task RunTestAsync()
        {
            Console.WriteLine($"RegistrationID = {_security.GetRegistrationID()}");
            VerifyRegistrationIdFormat(_security.GetRegistrationID());

            Console.Write("ProvisioningClient RegisterAsync . . . ");
            DeviceRegistrationResult result = await _provClient.RegisterAsync().ConfigureAwait(false);

            Console.WriteLine($"{result.Status}");
            Console.WriteLine($"ProvisioningClient AssignedHub: {result.AssignedHub}; DeviceID: {result.DeviceId}");

            if (result.Status != ProvisioningRegistrationStatusType.Assigned) return;

            IAuthenticationMethod auth;
            if (_security is SecurityProviderTpm)
            {
                Console.WriteLine("Creating TPM DeviceClient authentication.");
                auth = new DeviceAuthenticationWithTpm(result.DeviceId, _security as SecurityProviderTpm);
            }
            else if (_security is SecurityProviderX509)
            {
                Console.WriteLine("Creating X509 DeviceClient authentication.");
                auth = new DeviceAuthenticationWithX509Certificate(result.DeviceId, (_security as SecurityProviderX509).GetAuthenticationCertificate());
            }
            else
            {
                throw new NotSupportedException("Unknown authentication type.");
            }

            if (_skipTest != "Y")
            {
                using (DeviceClient iotClient = DeviceClient.Create(result.AssignedHub, auth, TransportType.Amqp))
                {
                    Console.WriteLine("DeviceClient OpenAsync.");
                    await iotClient.OpenAsync().ConfigureAwait(false);
                    Console.WriteLine("DeviceClient SendEventAsync TpmTestMessage.");
                    await iotClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes("TpmTestMessage"))).ConfigureAwait(false);
                    Console.WriteLine("DeviceClient CloseAsync.");
                    await iotClient.CloseAsync().ConfigureAwait(false);
                }
            }
        }

        private void VerifyRegistrationIdFormat(string v)
        {
            var r = new Regex("^[a-z0-9-]*$");
            if (!r.IsMatch(v))
            {
                throw new FormatException("Invalid registrationId: The registration ID is alphanumeric, lowercase, and may contain hyphens");
            }
        }
    }
}