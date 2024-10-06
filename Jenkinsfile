pipeline {
    agent any

    environment {
        DOCKER_IMAGE = 'riga-services-app:latest'
        POSTGRES_DB = credentials('database_name')
        POSTGRES_USER = credentials('database-user')
        POSTGRES_PASSWORD = credentials('postgres-password') // Reference Jenkins credentials
        CONNECTION_STRING = "Host=db;Database=${env.POSTGRES_DB};Username=${env.POSTGRES_USER};Password=${env.POSTGRES_PASSWORD};Port=5432;"
    }

    stages {
        stage('Checkout') {
            steps {
                // Pull the latest code from GitHub
                git branch: 'main', url: 'https://github.com/Sanchistor/Riga-Services.API.git'
            }
        }

        stage('Build Docker Image') {
            steps {
                script {
                    // Build the Docker image
                    sh "docker build -t ${DOCKER_IMAGE} ."
                }
            }
        }

        stage('Deploy to Docker') {
            steps {
                script {
                    // Bring down existing containers if necessary
                    sh "docker-compose down"

                    // Create .env file dynamically
                    writeFile file: '.env', text: """
                    POSTGRES_DB=${POSTGRES_DB}
                    POSTGRES_USER=${POSTGRES_USER}
                    POSTGRES_PASSWORD=${POSTGRES_PASSWORD}
                    CONNECTION_STRING=Host=db;Database=${env.POSTGRES_DB};Username=${env.POSTGRES_USER};Password=${env.POSTGRES_PASSWORD};Port=5432;
                    """

                    // Run Docker Compose
                    sh "docker-compose up -d --build"
                }
            }
        }
        
        stage('Apply Database Migrations') {
            steps {
                script {
                    // Debugging output
                    echo "Connection String: ${CONNECTION_STRING}"
                    echo "Docker Image: ${DOCKER_IMAGE}"
        
                    // Run migrations without -it
                    sh """
                    docker exec apirigaservices-web-1 \
                      dotnet ef database update --environment Production \
                      --connection "${CONNECTION_STRING}"
                    """
                }
            }
        }
    }
}
