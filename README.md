# DVD Document Database Utility

The DVD Document Database Utility is a .NET tool designed to create a YAML file database from DVDs containing documents. This utility enables users to search and locate documents based on DVD numbers, providing an efficient way to manage and retrieve information from multiple DVDs.

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Prerequisites](#prerequisites)
4. [Installation](#installation)
5. [Usage](#usage)
6. [Configuration](#configuration)
7. [Searching Documents](#searching-documents)
8. [Contributing](#contributing)
9. [License](#license)

## Introduction

Managing documents spread across multiple DVDs can be challenging. This utility simplifies the process by creating a searchable YAML file database from DVDs, allowing users to quickly locate documents based on DVD numbers.

## Features

- **YAML Database Creation:** Creates a YAML file database containing information about documents on each DVD.
- **Search Functionality:** Enables users to search for documents based on DVD numbers.
- **Intuitive Command-Line Interface:** Simple and straightforward usage with command-line arguments.

## Prerequisites

- **.NET SDK:** Ensure that you have the .NET SDK installed on your machine. If not, you can download it [here](https://dotnet.microsoft.com/download).

## Installation

1. **Clone the repository:**

    ```bash
    git clone https://github.com/yourusername/dvd-document-database-utility-dotnet.git
    ```

2. **Navigate to the project directory:**

    ```bash
    cd dvd-document-database-utility-dotnet
    ```

3. **Build the Solution:**

    ```bash
    dotnet build
    ```

## Usage

1. **Run the Utility:**

    ```bash
    dotnet run --action create --dvdDirectory /path/to/dvd/folder --outputDatabase database.yaml
    ```

    - Replace `/path/to/dvd/folder` with the path to the folder containing DVDs.
    - Specify the desired output database file using `--outputDatabase`.

2. **Search for Documents:**

    ```bash
    dotnet run --action search --database database.yaml --dvdNumber 123
    ```

    - Replace `database.yaml` with the path to your created database file.
    - Specify the DVD number using `--dvdNumber` to search for documents on a specific DVD.

## Configuration

The utility uses command-line arguments for configuration. The key arguments are:
   - `--action`: Specifies the action to perform (`create` to create the database, `search` to search for documents).
   - `--dvdDirectory`: The path to the folder containing DVDs (required for the `create` action).
   - `--outputDatabase`: The path to the output YAML database file (required for the `create` action).
   - `--database`: The path to the YAML database file (required for the `search` action).
   - `--dvdNumber`: The DVD number for searching documents (required for the `search` action).

## Searching Documents

To search for documents, use the `--action search` command along with the `--database` and `--dvdNumber` parameters. The utility will provide information about documents on the specified DVD.

## Contributing

Contributions are welcome! If you have additional features, improvements, or bug fixes, feel free to open an issue or submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE). See the LICENSE file for details.

Happy organizing and searching for documents!
