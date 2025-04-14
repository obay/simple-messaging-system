# Simple Messaging System

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
   git clone https://github.com/yourusername/simple-messaging-system.git
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

4. Access the Swagger UI at `https://localhost:5001/swagger` (or the port specified in your configuration)

### Docker Deployment

1. Build the Docker image:
   ```bash
   docker build -t simple-messaging-system .
   ```

2. Run the container:
   ```bash
   docker run -p 5001:80 simple-messaging-system
   ```

## Project Structure

```
simple-messaging-system/
├── Controllers/     # API controllers
├── Data/           # Data access layer
├── Models/         # Domain models
├── Properties/     # Project properties
├── logs/           # Application logs
├── Program.cs      # Application entry point
├── appsettings.json # Configuration
└── Dockerfile      # Docker configuration
```

## API Documentation

The API documentation is available through Swagger UI when running the application. Access it at:
- Development: `https://localhost:5001/swagger`
- Production: `https://your-domain/swagger`

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
