# Provision your Azure IoT Edge device using a TPM

Make use of the TPM 2.0 in your Azure IoT Edge device to provision it using a Azure Device Provisioning Service.

## Introduction

This executable helps you to register your device TPM as an individual enrollment on a Azure Device Provisioning Service.

## Usage

Pass Device Provisioning Service ID_Scope and RegistrationID as a command-prompt argument

Usage: ProvisionTpm \<IDScope\> \<RegistrationID\> \<SkipTest:Y|N\>

*Note*: Run this 'As Adminsitrator' or 'SU'

## Prerequisites

The follow prerequisites are in place:

1. Have a TPM 2.0 placed in your device
2. Run Windows 10 1809 or a recent Linux verison
3. Have a DPS created within your Azure subscription
4. Get the ID Scope of your DPS
5. Create a unique RegistrationID for your device

## Flow

The application is divided in two steps:

1. Run this application with the ID scope and the RegistrationID and the optional device client test
2. Retrieve an Endorsement key of your TPM
3. The application asks you to press a key after the folloing steps (You can skip step 4 - 8 if executed already)
4. Switch to the Azure Portal
5. In your Azure Device Provisioning Service please go to 'Manage enrollments' and select 'Individual Enrollments'
6. Select 'Add individual enrollment' and fill in:
    1. Mechanism 'TPM'
    2. Endorsement key
    3. Registration ID
    4. Switch over to the IoT Edge device enrollemnt is needed
    5. Set IoT Hub Device ID to the RegistrationId or any other valid DeviceID
    6. Check if the correct IoT Hub is selected
7. Save this individual enrollment
8. Within the application, press a key
9. See how the applicaiton ends

Within the Azure portal, see that the device is registered now.

## Supported operating systems

This tool should be working both on Windows and Linux due to the usage of .Net Core.

This tool is tested on:

- Windows 10 Enterprise 1809
- Windows 10 IoT Enterprise 1809
- Ubuntu 18.04

## Exceptions

If you get this 'TbsCommandBlocked' execption during the execution of the application:

    Unhandled Exception: Microsoft.Azure.Devices.Provisioning.Client.ProvisioningTransportException: AMQP transport exception ---> Tpm2Lib.TpmException: Error {TbsCommandBlocked} was returned for command ActivateCredential.

then check if you are running it as Adminsitrator or 'SU'.

## Credits

This example is based on the TPM Example in: [github.com/Azure-Samples/azure-iot-samples-csharp/tree/master/provisioning/Samples/device](https://github.com/Azure-Samples/azure-iot-samples-csharp/tree/master/provisioning/Samples/device)
