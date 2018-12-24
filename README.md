# Provision your Azure IoT Edge device using a TPM

Make use of the TPM 2.0 in your Azure IoT Edge device to provision it using a Azure Device Provisioning Service.

## Introduction

This executable helps you to register your device TPM as an individual enrollment on a Azure Device Provisioning Service.

## Usage

Pass Device Provisioning Service ID_Scope and RegistrationID as a command-prompt argument

Usage: ProvisionTpm \<IDScope\> \<RegistrationID\> \<SkipTest:Y|N\>

*Note*: Run this 'As Adminsitrator' or 'SU'

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
