# Bank Management System

Architecture Layout and Planning Overview: [Drawio Sketch](https://drive.google.com/file/d/1c2hQbYKuiETfPsRRWvzG2IGT6YBvaC83/view?usp=sharing)
## Project Setup Instructions

Follow the steps below to set up and run the Bank Management System on your local machine.

### 1. Clone the Repository

Start by cloning the repository to your local machine:

```bash
git clone https://github.com/mohamadalzhori/Bank-Management-System.git
```
### 2. Extract the Data Folder
After cloning the repository, you'll need to extract the data folder. Ensure that the data directory contains the required subfolders for Keycloak and Postgres.

Extract the data folder into the following directory: 
- data.zip is in this directory
```bash
Bank-Management-System/BMS.BMS/data
```
After extraction, your folder structure should look like this:
```bash
Bank-Management-System/
└── BMS.BMS/
    └── data/
        ├── keycloak/
        └── postgres/
```

### 3. Start the Docker Containers
Navigate to the BMS.BMS directory and bring up the necessary Docker containers using Docker Compose:
```bash
cd Bank-Management-System/BMS.BMS
docker compose up -d
```
This command will start the following services:

- Postgres: The database for the application.
- Keycloak: The authentication and authorization server.
- RabbitMQ: For messaging.
- Redis: For caching.
- Seq: For logging.

### 4. Run the Application Projects
After the Docker containers are up and running, you need to start each project manually. Follow these steps for each project:

Open your IDE or terminal.
Navigate to the root directory of each project.
Run the project using the appropriate commands for your environment (e.g., dotnet run for .NET projects).

### 5. Verify Setup
After starting all the services and projects, verify that everything is running correctly:

- Keycloak: Access the Keycloak admin console at http://localhost:8080. Log in using the admin credentials you set in the docker-compose.yml file (admin/admin by default).
- RabbitMQ: Access the RabbitMQ management dashboard at http://localhost:15672. Log in using the default credentials (guest/guest) unless you've set up custom ones.
