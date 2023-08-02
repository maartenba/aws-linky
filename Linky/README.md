
# ASP.NET Core Minimal API's on AWS Lambda with JetBrains Rider
## Maarten Balliauw

* Twitter: [@maartenballiauw](https://twitter.com/maartenballiauw)
* Mastodon: [maartenballiauw@mastodon.online](https://mastodon.online/@maartenballiauw)
* Blog: [blog.maartenballiauw.be](https://blog.maartenballiauw.be)

Along with ASP.NET Core 6.0 came minimal API's, a lighter-weight framework to build HTTP APIs with ASP.NET Core. But did you know you can run your minimal API applications on AWS Lambda as well?

In this talk, we'll take an existing ASP.NET Core minimal API service, and port it to run on AWS Lambda and an RDS database. You will learn how you can use minimal API's with .NET on AWS Lambda, and how the AWS toolkit for JetBrains Rider helps with packaging and deploying your application.

## Useful links:

* [JetBrains Rider](https://www.jetbrains.com/rider/)
* [AWS toolkit](https://aws.amazon.com/rider/)
* [AOT compilation for AWS Lamdas and .NET](https://aws.amazon.com/blogs/compute/building-serverless-net-applications-on-aws-lambda-using-net-7/)

## Useful command-line snippets:

### Create a local tools manifest file

```bash
dotnet new tool-manifest
```

### Install Amazon.Lambda.Tools to the local tools manifest file

```bash
dotnet tool install Amazon.Lambda.Tools
```

### Install Entity Framework Core tools to the local tools manifest

```bash
dotnet tool install dotnet-ef
```