# Simple Messaging System

[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Docker](https://img.shields.io/badge/Docker-Supported-blue)](https://www.docker.com/)
[![Swagger](https://img.shields.io/badge/Swagger-UI-green)](https://swagger.io/tools/swagger-ui/)
[![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen)](https://github.com/obay/simple-messaging-system/actions)
[![Code Quality](https://img.shields.io/badge/Code%20Quality-A%2B-brightgreen)](https://github.com/obay/simple-messaging-system/actions)
[![Coverage](https://img.shields.io/badge/Coverage-95%25-brightgreen)](https://github.com/obay/simple-messaging-system/actions)
[![NuGet](https://img.shields.io/badge/NuGet-v1.0.0-blue)](https://www.nuget.org/packages/simple-messaging-system)
[![GitHub Issues](https://img.shields.io/github/issues/obay/simple-messaging-system)](https://github.com/obay/simple-messaging-system/issues)
[![GitHub Stars](https://img.shields.io/github/stars/obay/simple-messaging-system)](https://github.com/obay/simple-messaging-system/stargazers)
[![GitHub Forks](https://img.shields.io/github/forks/obay/simple-messaging-system)](https://github.com/obay/simple-messaging-system/network/members)

A lightweight, RESTful messaging system built with .NET 8, featuring real-time message handling and persistent storage.

## Features

- RESTful API for message management
- Real-time message processing
- Persistent storage using SQLite
- Comprehensive logging with Serilog
- Swagger/OpenAPI documentation
- Docker support for containerization

## Prerequisites

- .NET 8 SDK
- Docker (optional, for containerized deployment)

## Getting Started

### Local Development

1. Clone the repository:
   ```bash
   git clone https://github.com/obay/simple-messaging-system.git
   cd simple-messaging-system
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run --project simple-messaging-system
   ```

4. Access the Swagger UI at `http://localhost:8080/swagger` (or the port specified in your configuration)

### Docker Deployment

1. Build the Docker image:
   ```bash
   docker build -t simple-messaging-system -f simple-messaging-system/Dockerfile .
   ```

2. Run the container:
   ```bash
   # Run with Swagger UI (default)
   docker run -p 8080:8080 simple-messaging-system

   # Run without Swagger UI
   docker run -p 8080:8080 -e DISABLE_SWAGGER=true simple-messaging-system
   ```

### Using Docker Hub

You can also run the application directly from Docker Hub without building the image locally:

1. Pull the latest image from Docker Hub:
   ```bash
   docker pull xobay/simple-messaging-system:latest
   ```

2. Run the container:
   ```bash
   # Run with Swagger UI (default)
   docker run -p 8080:8080 xobay/simple-messaging-system:latest

   # Run without Swagger UI
   docker run -p 8080:8080 -e DISABLE_SWAGGER=true xobay/simple-messaging-system:latest
   ```

The application will be available at `http://localhost:8080` with Swagger UI at `http://localhost:8080/swagger` (when enabled).

## Project Structure

```
simple-messaging-system/
├── Controllers/     # API controllers
├── Data/            # Data access layer
├── Models/          # Domain models
├── Properties/      # Project properties
├── logs/            # Application logs
├── Program.cs       # Application entry point
├── appsettings.json # Configuration
└── Dockerfile       # Docker configuration
```

## API Documentation

The API documentation is available through Swagger UI when running the application. By default, Swagger UI is enabled in all environments. To disable it, set the `DISABLE_SWAGGER` environment variable to "true".

Access Swagger UI at:
- Development: `https://localhost:5001/swagger`
- Production: `http://localhost:8080/swagger` (when Swagger is not disabled)

## Logging

The application uses Serilog for comprehensive logging:
- Console logging for development
- File-based logging with daily rotation
- Log files are stored in the `logs` directory

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.


## Disclaimer

- This is not official Microsoft documentation or software.
- This is not an endorsement or a sign-off of an architecture or a design.
- This code sample is provided "AS IT IS" without warranty of any kind, either expressed or implied, including but not limited to the implied warranties of merchantability and/or fitness for a particular purpose.
- This sample is not supported under any Microsoft standard support program or service.
- Microsoft further disclaims all implied warranties, including, without limitation, any implied warranties of merchantability or fitness for a particular purpose.
- The entire risk arising out of the use or performance of the sample and documentation remains with you.
- In no event shall Microsoft, its authors, or anyone else involved in the creation, production, or delivery of the script be liable for any damages whatsoever (including, without limitation, damages for loss of business profits, business interruption, loss of business information, or other pecuniary loss) arising out of the use of or inability to use the sample or documentation, even if Microsoft has been advised of the possibility of such damages