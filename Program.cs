namespace ProvisioningTpm
{
    // This example is based on:
    // https://github.com/Azure-Samples/azure-iot-samples-csharp/tree/master/provisioning/Samples/device

    using Microsoft.Azure.Devices.Provisioning.Client;
    using Microsoft.Azure.Devices.Provisioning.Client.Transport;
    using Microsoft.Azure.Devices.Shared;
    using System;
    using Microsoft.Azure.Devices.Provisioning.Security;

    public static class Program
    {
        // - pass Device Provisioning Service ID_Scope as a command-prompt argument
        private static string _idScope = string.Empty;

        // - pass an individual enrollment registration id for this device
        private static string _registrationId = string.Empty;

        // - If you want to skip the device message send test, pass 'Y'
        private static string _skipTest = string.Empty;

        private const string GlobalDeviceEndpoint = "global.azure-devices-provisioning.net";
        
        public static int Main(string[] args)
        {
            Console.WriteLine("Provision your TPM");
            Console.WriteLine("------------------");
            Console.WriteLine("Usage: ProvisionTpm <IDScope> <RegistrationID> <SkipTest:Y|N>");
            Console.WriteLine("Run this 'As Adminsitrator'");

            if (string.IsNullOrWhiteSpace(_idScope) && (args.Length > 0))
            {
                _idScope = args[0];
            }

            if (string.IsNullOrWhiteSpace(_registrationId) && (args.Length > 1))
            {
                _registrationId = args[1];
            }

            if (string.IsNullOrWhiteSpace(_skipTest) && (args.Length > 2))
            {
                _skipTest = args[2].ToUpper();
            }

            if (string.IsNullOrWhiteSpace(_idScope)
                    || string.IsNullOrWhiteSpace(_registrationId)
                    || string.IsNullOrWhiteSpace(_skipTest))
            {
                Console.WriteLine("Check if the parameters are corrent: ProvisionTpm <IDScope> <RegistrationID> <SkipTest:Y|N>");
                return 1;
            }

            using (var security = new SecurityProviderTpmHsm(_registrationId))
            using (var transport = new ProvisioningTransportHandlerAmqp(TransportFallbackType.TcpOnly))
            {
                // Note that the TPM simulator will create an NVChip file containing the simulated TPM state.
                Console.WriteLine("Extracting endorsement key.");
                string base64EK = Convert.ToBase64String(security.GetEndorsementKey());

                Console.WriteLine(
                    "In your Azure Device Provisioning Service please go to 'Manage enrollments' and select " +
                    "'Individual Enrollments'. Select 'Add individual enrollment' then fill in the following:");

                Console.WriteLine($"\tMechanism: TPM");
                Console.WriteLine($"\tEndorsement key: {base64EK}");
                Console.WriteLine($"\tRegistration ID: {_registrationId}");
                Console.WriteLine($"\tSwitch over to the IoT Edge device enrollemnt is needed");
                Console.WriteLine($"\tIoT Hub Device ID: {_registrationId} (or any other valid DeviceID)");
                Console.WriteLine($"\tCheck if the correct IoT Hub is selected");
                Console.WriteLine($"\tFinally, Save this individual enrollment");
                Console.WriteLine();
                Console.WriteLine("Press ENTER when ready. This will start finalizing the registration on your TPM");
                Console.ReadLine();

                ProvisioningDeviceClient provClient =
                    ProvisioningDeviceClient.Create(GlobalDeviceEndpoint, _idScope, security, transport);

                var client = new ProvisioningDeviceTpmClient(provClient, security, _skipTest);
                client.RunTestAsync().GetAwaiter().GetResult();

                Console.WriteLine("The registration is finalized on the TPM");

                if (_skipTest != "Y")
                {
                    Console.WriteLine("The connection is tested by sending a test message");
                }
            }

            return 0;
        }
    }
}