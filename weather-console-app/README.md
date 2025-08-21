# Weather Console App

This is a simple console application that fetches and displays the weather information for a given city.

## Project Structure

```
weather-console-app
├── src
│   ├── WeatherConsole.csproj
│   ├── Program.cs
│   ├── Services
│   │   └── WeatherService.cs
│   ├── Interfaces
│   │   └── IWeatherService.cs
│   ├── Models
│   │   └── WeatherResponse.cs
│   ├── Configuration
│   │   └── ApiSettings.cs
│   └── appsettings.json
├── .gitignore
└── README.md
```

## Getting Started

### Prerequisites

- .NET SDK (version 5.0 or later)
- An API key for the weather service (to be configured in `appsettings.json`)

### Installation

1. Clone the repository:
   ```
   git clone <repository-url>
   ```
2. Navigate to the project directory:
   ```
   cd weather-console-app/src
   ```
3. Restore the dependencies:
   ```
   dotnet restore
   ```

### Configuration

Before running the application, you need to set up your API key and base URL in the `appsettings.json` file located in the `src` directory.

### Running the Application

To run the application, use the following command:
```
dotnet run
```

You will be prompted to enter a city name. The application will then fetch and display the current weather information for that city.

### Contributing

Feel free to submit issues or pull requests if you would like to contribute to the project.