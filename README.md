# ProvisioningTpm

## Introduction

This executable helps you to register your device TPM as an individual enrollment on a Azure Device Provisioning Service. 

## Usage

Pass Device Provisioning Service ID_Scope and RegistrationID as a command-prompt argument

Usage: ProvisionTpm \<IDScope\> \<RegistrationID\>

*Note*: Run this 'As Adminsitrator'

## Supported operating systems

This tool should be working both on Windows and Linux due to the usage of .Net Core.

This tool is tested on:

- Windows 10 Enterprise 1809

## Credits

This example is based on: https://github.com/Azure-Samples/azure-iot-samples-csharp/tree/master/provisioning/Samples/device