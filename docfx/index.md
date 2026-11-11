---
_layout: landing
---

[![Banner](https://raw.githubusercontent.com/jscarle/OnePassword.NET/main/Banner.png)](https://github.com/jscarle/OnePassword.NET)

# OnePassword.NET - 1Password CLI Wrapper

This library serves as a .NET wrapper for the [1Password](https://1password.com/) command-line tool
`op` ( [Download](https://app-updates.agilebits.com/product_history/CLI2) | [Documentation](https://developer.1password.com/docs/cli/reference) ).

[![build](https://img.shields.io/github/actions/workflow/status/jscarle/OnePassword.NET/develop.yml?branch=develop&logo=github&label=build)](https://github.com/jscarle/OnePassword.NET/actions/workflows/develop.yml)
[![nuget](https://img.shields.io/nuget/v/OnePassword.NET)](https://www.nuget.org/packages/OnePassword.NET)
[![downloads](https://img.shields.io/nuget/dt/OnePassword.NET)](https://www.nuget.org/packages/OnePassword.NET)

## References

This library targets .NET 11.

Version 2.6.0 requires .NET 11 and no longer includes the previous .NET Standard 2.1 target.

The referenced 1Password CLI version is **2.42.0**. Compatibility was reviewed against the official command reference and full CLI 2 changelog on November 10, 2026; this version has not been validated against a live account. See the [CLI compatibility review](docs/cli-compatibility.md).

## Dependencies

This library has no dependencies.

> #### Disclaimer
> 
> While the use of the 1Password name and logo is authorized, it's important to note that this library is not an official product developed or maintained by AgileBits, Inc.
